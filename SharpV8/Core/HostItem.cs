// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.Expando;
using Microsoft.ClearScript.Util;

namespace Microsoft.ClearScript
{
    internal partial class HostItem : DynamicObject, IReflect, IDynamic, IScriptMarshalWrapper, IHostInvokeContext
    {
        #region data

        private HostTargetMemberData targetMemberData;

        private static readonly PropertyInfo[] reflectionProperties =
        {
            typeof(Delegate).GetProperty("Method"),
            typeof(Exception).GetProperty("TargetSite")
        };

        internal static bool EnableVTablePatching;
        [ThreadStatic] private static bool bypassVTablePatching;

        #endregion

        #region constructors

        private HostItem(ScriptEngine engine, HostTarget target, HostItemFlags flags)
        {
            Engine = engine;
            Target = target;
            Flags = flags;

            BindSpecialTarget();
            TryBindTargetMemberData();
        }

        #endregion

        #region wrappers

        public static object Wrap(ScriptEngine engine, object obj)
        {
            return Wrap(engine, obj, null, HostItemFlags.None);
        }

        public static object Wrap(ScriptEngine engine, object obj, Type type)
        {
            return Wrap(engine, obj, type, HostItemFlags.None);
        }

        public static object Wrap(ScriptEngine engine, object obj, HostItemFlags flags)
        {
            return Wrap(engine, obj, null, flags);
        }

        private static object Wrap(ScriptEngine engine, object obj, Type type, HostItemFlags flags)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is HostItem hostItem)
            {
                obj = hostItem.Target;
            }

            if (obj is HostTarget hostTarget)
            {
                return BindOrCreate(engine, hostTarget, flags);
            }

            if (type == null)
            {
                type = obj.GetTypeOrTypeInfo();
            }
            else
            {
                Debug.Assert(type.IsInstanceOfType(obj));
            }

            if (obj is Enum)
            {
                return BindOrCreate(engine, obj, type, flags);
            }

            var typeCode = Type.GetTypeCode(type);
            if ((typeCode == TypeCode.Object) || (typeCode == TypeCode.DateTime))
            {
                return BindOrCreate(engine, obj, type, flags);
            }

            return obj;
        }

        #endregion

        #region public members

        public delegate HostItem CreateFunc(ScriptEngine engine, HostTarget target, HostItemFlags flags);

        public HostTarget Target { get; }

        public HostItemFlags Flags { get; }

        public Invocability Invocability
        {
            get
            {
                if (TargetInvocability == null)
                {
                    TargetInvocability = Target.GetInvocability(GetCommonBindFlags(), AccessContext, DefaultAccess, Flags.HasFlag(HostItemFlags.HideDynamicMembers));
                }

                return TargetInvocability.GetValueOrDefault();
            }
        }

        public object InvokeMember(string name, BindingFlags invokeFlags, object[] args, object[] bindArgs, CultureInfo culture, bool bypassTunneling)
        {
            return InvokeMember(name, invokeFlags, args, bindArgs, culture, bypassTunneling, out _);
        }

        public object InvokeMember(string name, BindingFlags invokeFlags, object[] args, object[] bindArgs, CultureInfo culture, bool bypassTunneling, out bool isCacheable)
        {
            name = AdjustInvokeName(name);
            AdjustInvokeFlags(ref invokeFlags);

            isCacheable = false;

            if (Target.TryInvokeAuxMember(this, name, invokeFlags, args, bindArgs, out var result))
            {
                if (Target is IHostVariable)
                {
                    // the variable may have been reassigned
                    BindSpecialTarget();
                }

                return result;
            }

            if (TargetDynamic != null)
            {
                return InvokeDynamicMember(name, invokeFlags, args);
            }

            if (TargetPropertyBag != null)
            {
                return InvokePropertyBagMember(name, invokeFlags, args, bindArgs);
            }

            if (TargetList != null)
            {
                if (int.TryParse(name, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index))
                {
                    return InvokeListElement(index, invokeFlags, args, bindArgs);
                }
            }

            if (!bypassTunneling)
            {
                int testLength;
                if (invokeFlags.HasFlag(BindingFlags.InvokeMethod))
                {
                    testLength = invokeFlags.HasFlag(BindingFlags.GetField) ? 0 : -1;
                }
                else
                {
                    testLength = invokeFlags.HasFlag(BindingFlags.SetField) ? 1 : 0;
                }

                if ((args.Length > testLength) && (name != SpecialMemberNames.Default))
                {
                    var value = GetHostProperty(name, GetCommonBindFlags(), ArrayHelpers.GetEmptyArray<object>(), ArrayHelpers.GetEmptyArray<object>(), false, out _);
                    if (!(value is Nonexistent))
                    {
                        if (Engine.MarshalToScript(value) is HostItem hostItem)
                        {
                            return hostItem.InvokeMember(SpecialMemberNames.Default, invokeFlags, args, bindArgs, culture, true, out isCacheable);
                        }
                    }
                }
            }

            return InvokeHostMember(name, invokeFlags, args, bindArgs, out isCacheable);
        }

        #endregion

        #region internal members

        #region interface accessors

        private IReflect ThisReflect => this;

        private IDynamic ThisDynamic => this;

        #endregion

        #region collateral accessors

        private IDynamic TargetDynamic => Collateral.TargetDynamic.Get(this);

        private IPropertyBag TargetPropertyBag
        {
            get => Collateral.TargetPropertyBag.Get(this);
            set => Collateral.TargetPropertyBag.Set(this, value);
        }

        private IHostList TargetList
        {
            get => Collateral.TargetList.Get(this);
            set => Collateral.TargetList.Set(this, value);
        }

        private DynamicHostMetaObject TargetDynamicMetaObject
        {
            get => Collateral.TargetDynamicMetaObject.Get(this);
            set => Collateral.TargetDynamicMetaObject.Set(this, value);
        }

        private IEnumerator TargetEnumerator => Collateral.TargetEnumerator.Get(this);

        private HashSet<string> ExpandoMemberNames
        {
            get => Collateral.ExpandoMemberNames.Get(this);
            set => Collateral.ExpandoMemberNames.Set(this, value);
        }

        private Dictionary<string, HostMethod> HostMethodMap
        {
            get => Collateral.HostMethodMap.Get(this);
            set => Collateral.HostMethodMap.Set(this, value);
        }

        private Dictionary<string, HostIndexedProperty> HostIndexedPropertyMap
        {
            get => Collateral.HostIndexedPropertyMap.Get(this);
            set => Collateral.HostIndexedPropertyMap.Set(this, value);
        }

        private int[] PropertyIndices
        {
            get => Collateral.ListData.GetOrCreate(this).PropertyIndices;
            set => Collateral.ListData.GetOrCreate(this).PropertyIndices = value;
        }

        private int CachedListCount
        {
            get => Collateral.ListData.GetOrCreate(this).CachedCount;
            set => Collateral.ListData.GetOrCreate(this).CachedCount = value;
        }

        private HostItemCollateral Collateral => Engine.HostItemCollateral;

        #endregion

        #region target member data accessors

        private string[] TypeEventNames
        {
            get => targetMemberData.TypeEventNames;
            set => targetMemberData.TypeEventNames = value;
        }

        private string[] TypeFieldNames
        {
            get => targetMemberData.TypeFieldNames;
            set => targetMemberData.TypeFieldNames = value;
        }

        private string[] TypeMethodNames
        {
            get => targetMemberData.TypeMethodNames;
            set => targetMemberData.TypeMethodNames = value;
        }

        private string[] TypePropertyNames
        {
            get => targetMemberData.TypePropertyNames;
            set => targetMemberData.TypePropertyNames = value;
        }

        private string[] AllFieldNames
        {
            get => targetMemberData.AllFieldNames;
            set => targetMemberData.AllFieldNames = value;
        }

        private string[] AllMethodNames
        {
            get => targetMemberData.AllMethodNames;
            set => targetMemberData.AllMethodNames = value;
        }

        private string[] OwnMethodNames
        {
            get => targetMemberData.OwnMethodNames;
            set => targetMemberData.OwnMethodNames = value;
        }

        private string[] EnumeratedMethodNames => Engine.EnumerateInstanceMethods ? (Engine.EnumerateExtensionMethods ? AllMethodNames : OwnMethodNames) : ArrayHelpers.GetEmptyArray<string>();

        private string[] AllPropertyNames
        {
            get => targetMemberData.AllPropertyNames;
            set => targetMemberData.AllPropertyNames = value;
        }

        private string[] AllMemberNames
        {
            get => targetMemberData.AllMemberNames;
            set => targetMemberData.AllMemberNames = value;
        }

        private FieldInfo[] AllFields
        {
            get => targetMemberData.AllFields;
            set => targetMemberData.AllFields = value;
        }

        private MethodInfo[] AllMethods
        {
            get => targetMemberData.AllMethods;
            set => targetMemberData.AllMethods = value;
        }

        private PropertyInfo[] AllProperties
        {
            get => targetMemberData.AllProperties;
            set => targetMemberData.AllProperties = value;
        }

        private object EnumerationSettingsToken
        {
            get => targetMemberData.EnumerationSettingsToken;
            set => targetMemberData.EnumerationSettingsToken = value;
        }

        private ExtensionMethodSummary ExtensionMethodSummary
        {
            get => targetMemberData.ExtensionMethodSummary;
            set => targetMemberData.ExtensionMethodSummary = value;
        }

        private Invocability? TargetInvocability
        {
            get => targetMemberData.TargetInvocability;
            set => targetMemberData.TargetInvocability = value;
        }

        private Type CurrentAccessContext => (Flags.HasFlag(HostItemFlags.PrivateAccess) || (Target.Type.IsAnonymous() && !Engine.EnforceAnonymousTypeAccess)) ? Target.Type : Engine.AccessContext;

        private ScriptAccess CurrentDefaultAccess => Engine.DefaultAccess;

        private HostTargetFlags CurrentTargetFlags => Target.GetFlags(this);

        private Type CachedAccessContext => (targetMemberData is HostTargetMemberDataWithContext targetMemberDataWithContext) ? targetMemberDataWithContext.AccessContext : CurrentAccessContext;

        private ScriptAccess CachedDefaultAccess
        {
            get
            {
                var targetMemberDataWithContext = targetMemberData as HostTargetMemberDataWithContext;
                return targetMemberDataWithContext?.DefaultAccess ?? CurrentDefaultAccess;
            }
        }

        private HostTargetFlags CachedTargetFlags
        {
            get
            {
                var targetMemberDataWithContext = targetMemberData as HostTargetMemberDataWithContext;
                return targetMemberDataWithContext?.TargetFlags ?? CurrentTargetFlags;
            }
        }

        #endregion

        #region initialization

        private static bool TargetSupportsExpandoMembers(HostTarget target, HostItemFlags flags)
        {
            if (!TargetSupportsSpecialTargets(target))
            {
                return false;
            }

            if (typeof(IDynamic).IsAssignableFrom(target.Type))
            {
                return true;
            }

            if (typeof(IPropertyBag).IsAssignableFrom(target.Type))
            {
                return true;
            }

            if (!flags.HasFlag(HostItemFlags.HideDynamicMembers) && typeof(IDynamicMetaObjectProvider).IsAssignableFrom(target.Type))
            {
                return true;
            }

            return false;
        }

        private bool CanAddExpandoMembers()
        {
            return (TargetDynamic != null) || ((TargetPropertyBag != null) && !TargetPropertyBag.IsReadOnly) || (TargetDynamicMetaObject != null);
        }

        private static object BindOrCreate(ScriptEngine engine, object target, Type type, HostItemFlags flags)
        {
            return BindOrCreate(engine, HostObject.Wrap(target, type), flags);
        }

        private static object BindOrCreate(ScriptEngine engine, HostTarget target, HostItemFlags flags)
        {
            return engine.GetOrCreateHostItem(target, flags, Create);
        }

        private void BindSpecialTarget()
        {
            if (TargetSupportsSpecialTargets(Target))
            {
                if (BindSpecialTarget(Collateral.TargetDynamic))
                {
                    TargetPropertyBag = null;
                    TargetList = null;
                    TargetDynamicMetaObject = null;
                }
                else if (BindSpecialTarget(Collateral.TargetPropertyBag))
                {
                    TargetList = null;
                    TargetDynamicMetaObject = null;
                }
                else
                {
                    if (!Flags.HasFlag(HostItemFlags.HideDynamicMembers) && BindSpecialTarget(out IDynamicMetaObjectProvider dynamicMetaObjectProvider))
                    {
                        var dynamicMetaObject = dynamicMetaObjectProvider.GetMetaObject(Expression.Constant(Target.InvokeTarget));
                        TargetDynamicMetaObject = new DynamicHostMetaObject(dynamicMetaObjectProvider, dynamicMetaObject);
                        TargetList = null;
                    }
                    else
                    {
                        TargetDynamicMetaObject = null;
                        BindSpecialTarget(Collateral.TargetList);
                    }
                }
            }
        }

        private bool BindSpecialTarget<T>(CollateralObject<HostItem, T> property) where T : class
        {
            if (BindSpecialTarget(out T value))
            {
                property.Set(this, value);
                return true;
            }

            property.Clear(this);
            return false;
        }

        private bool BindSpecialTarget<T>(out T specialTarget) where T : class
        {
            if (Target.InvokeTarget == null)
            {
                specialTarget = null;
                return false;
            }

            if (typeof(T) == typeof(IHostList))
            {
                // generic list support

                if (Target.Type.IsAssignableToGenericType(typeof(IList<>), out var typeArgs))
                {
                    if (typeof(IList).IsAssignableFrom(Target.Type))
                    {
                        specialTarget = new HostList(Engine, (IList)Target.InvokeTarget, typeArgs[0]) as T;
                        return specialTarget != null;
                    }

                    specialTarget = typeof(HostList<>).MakeGenericType(typeArgs).CreateInstance(Engine, Target.InvokeTarget) as T;
                    return specialTarget != null;
                }

                if (typeof(IList).IsAssignableFrom(Target.Type))
                {
                    specialTarget = new HostList(Engine, (IList)Target.InvokeTarget, typeof(object)) as T;
                    return specialTarget != null;
                }

                if (Target.Type.IsAssignableToGenericType(typeof(IReadOnlyList<>), out typeArgs))
                {
                    specialTarget = typeof(ReadOnlyHostList<>).MakeGenericType(typeArgs).CreateInstance(Engine, Target.InvokeTarget) as T;
                    return specialTarget != null;
                }

                specialTarget = null;
                return false;
            }

            // The check here is required because the item may be bound to a specific target base
            // class or interface - one that must not trigger special treatment.

            if (typeof(T).IsAssignableFrom(Target.Type))
            {
                specialTarget = Target.InvokeTarget as T;
                return specialTarget != null;
            }

            specialTarget = null;
            return false;
        }

        private void TryBindTargetMemberData()
        {
            if ((targetMemberData == null) ||
                (AccessContext != CurrentAccessContext) || 
                (DefaultAccess != CurrentDefaultAccess) ||
                (TargetFlags != CurrentTargetFlags))
            {
                BindTargetMemberData();
            }
        }

        private void BindTargetMemberData()
        {
            if (Target is HostMethod)
            {
                // host methods can share their (dummy) member data
                targetMemberData = Engine.SharedHostMethodMemberData;
                return;
            }

            if (Target is HostIndexedProperty)
            {
                // host indexed properties can share their (dummy) member data
                targetMemberData = Engine.SharedHostIndexedPropertyMemberData;
                return;
            }

            if (Target is ScriptMethod)
            {
                // script methods can share their (dummy) member data
                targetMemberData = Engine.SharedScriptMethodMemberData;
                return;
            }

            if (Target is HostObject hostObject)
            {
                if ((TargetDynamic == null) && (TargetPropertyBag == null) && (TargetList == null) && (TargetDynamicMetaObject == null))
                {
                    // host objects without dynamic members can share their member data
                    targetMemberData = Engine.GetSharedHostObjectMemberData(hostObject, CurrentAccessContext, CurrentDefaultAccess, CurrentTargetFlags);
                    return;
                }
            }

            // all other targets use unique member data
            targetMemberData = new HostTargetMemberDataWithContext(CurrentAccessContext, CurrentDefaultAccess, CurrentTargetFlags);
        }

        private static bool TargetSupportsSpecialTargets(HostTarget target)
        {
            return (target is HostObject) || (target is IHostVariable) || (target is IByRefArg);
        }

        #endregion

        #region member data maintenance

        private string[] GetLocalEventNames()
        {
            if (TypeEventNames == null)
            {
                var localEvents = Target.Type.GetScriptableEvents(GetCommonBindFlags(), AccessContext, DefaultAccess);
                TypeEventNames = localEvents.Select(eventInfo => eventInfo.GetScriptName()).ToArray();
            }

            return TypeEventNames;
        }

        private string[] GetLocalFieldNames()
        {
            if (TypeFieldNames == null)
            {
                var localFields = Target.Type.GetScriptableFields(GetCommonBindFlags(), AccessContext, DefaultAccess);
                TypeFieldNames = localFields.Select(field => field.GetScriptName()).ToArray();
            }

            return TypeFieldNames;
        }

        private string[] GetLocalMethodNames()
        {
            if (TypeMethodNames == null)
            {
                var localMethods = Target.Type.GetScriptableMethods(GetMethodBindFlags(), AccessContext, DefaultAccess);
                TypeMethodNames = localMethods.Select(method => method.GetScriptName()).ToArray();
            }

            return TypeMethodNames;
        }

        private string[] GetLocalPropertyNames()
        {
            if (TypePropertyNames == null)
            {
                var localProperties = Target.Type.GetScriptableProperties(GetCommonBindFlags(), AccessContext, DefaultAccess);
                TypePropertyNames = localProperties.Select(property => property.GetScriptName()).ToArray();
            }

            return TypePropertyNames;
        }

        private string[] GetAllFieldNames()
        {
            if ((TargetDynamic == null) && (TargetPropertyBag == null))
            {
                return GetLocalFieldNames().Concat(GetLocalEventNames()).Distinct().ToArray();
            }

            return ArrayHelpers.GetEmptyArray<string>();
        }

        private string[] GetAllMethodNames(out string[] ownMethodNames)
        {
            ownMethodNames = null;

            var names = Target.GetAuxMethodNames(this, GetMethodBindFlags()).AsEnumerable() ?? Enumerable.Empty<string>();
            if ((TargetDynamic == null) && (TargetPropertyBag == null))
            {
                names = names.Concat(GetLocalMethodNames());
                if (TargetFlags.HasFlag(HostTargetFlags.AllowExtensionMethods))
                {
                    var extensionMethodSummary = Engine.ExtensionMethodSummary;
                    ExtensionMethodSummary = extensionMethodSummary;

                    var extensionMethodNames = extensionMethodSummary.MethodNames;
                    if (extensionMethodNames.Length > 0)
                    {
                        ownMethodNames = names.Distinct().ToArray();
                        names = ownMethodNames.Concat(extensionMethodNames);
                    }
                }
            }

            var result = names.Distinct().ToArray();
            if (ownMethodNames == null)
            {
                ownMethodNames = result;
            }

            return result;
        }

        private string[] GetAllPropertyNames()
        {
            var names = Target.GetAuxPropertyNames(this, GetCommonBindFlags()).AsEnumerable() ?? Enumerable.Empty<string>();
            if (TargetDynamic != null)
            {
                names = names.Concat(TargetDynamic.GetPropertyNames());
                names = names.Concat(TargetDynamic.GetPropertyIndices().Select(index => index.ToString(CultureInfo.InvariantCulture)));
            }
            else if (TargetPropertyBag != null)
            {
                names = names.Concat(TargetPropertyBag.Keys);
            }
            else
            {
                names = names.Concat(GetLocalPropertyNames());

                if (TargetList != null)
                {
                    CachedListCount = TargetList.Count;
                    if (CachedListCount > 0)
                    {
                        names = names.Concat(Enumerable.Range(0, CachedListCount).Select(index => index.ToString(CultureInfo.InvariantCulture)));
                    }
                }

                if (TargetDynamicMetaObject != null)
                {
                    names = names.Concat(TargetDynamicMetaObject.GetDynamicMemberNames());
                }
            }

            if (ExpandoMemberNames != null)
            {
                names = names.Except(ExpandoMemberNames);
            }

            return names.Distinct().ToArray();
        }

        private void UpdateFieldNames(out bool updated)
        {
            if (AllFieldNames == null)
            {
                AllFieldNames = GetAllFieldNames();
                updated = true;
            }
            else
            {
                updated = false;
            }
        }

        private void UpdateMethodNames(out bool updated)
        {
            if ((AllMethodNames == null) ||
                (TargetFlags.HasFlag(HostTargetFlags.AllowExtensionMethods) && (ExtensionMethodSummary != Engine.ExtensionMethodSummary)))
            {
                AllMethodNames = GetAllMethodNames(out var ownMethodNames);
                OwnMethodNames = ownMethodNames;
                updated = true;
            }
            else
            {
                updated = false;
            }
        }

        private void UpdatePropertyNames(out bool updated)
        {
            if ((AllPropertyNames == null) ||
                (TargetDynamic != null) ||
                (TargetPropertyBag != null) ||
                (TargetDynamicMetaObject != null) ||
                ((TargetList != null) && (CachedListCount != TargetList.Count)))
            {
                AllPropertyNames = GetAllPropertyNames();
                updated = true;
            }
            else
            {
                updated = false;
            }
        }

        private void UpdateEnumerationSettingsToken(out bool updated)
        {
            var enumerationSettingsToken = Engine.EnumerationSettingsToken;
            if (EnumerationSettingsToken != enumerationSettingsToken)
            {
                EnumerationSettingsToken = enumerationSettingsToken;
                updated = true;
            }
            else
            {
                updated = false;
            }
        }

        protected virtual void AddExpandoMemberName(string name)
        {
            if (ExpandoMemberNames == null)
            {
                ExpandoMemberNames = new HashSet<string>();
            }

            ExpandoMemberNames.Add(name);
        }

        protected virtual void RemoveExpandoMemberName(string name)
        {
            ExpandoMemberNames?.Remove(name);
        }

        #endregion

        #region member invocation

        private bool UseCaseInsensitiveMemberBinding => Engine.UseCaseInsensitiveMemberBinding;

        private StringComparison MemberNameComparison => UseCaseInsensitiveMemberBinding ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        private StringComparer MemberNameComparer => UseCaseInsensitiveMemberBinding ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        private BindingFlags GetCommonBindFlags()
        {
            var bindFlags = BindingFlags.Public | BindingFlags.NonPublic;

            if (TargetFlags.HasFlag(HostTargetFlags.AllowStaticMembers))
            {
                bindFlags |= BindingFlags.Static | BindingFlags.FlattenHierarchy;
            }

            if (TargetFlags.HasFlag(HostTargetFlags.AllowInstanceMembers))
            {
                bindFlags |= BindingFlags.Instance;
            }

            if (UseCaseInsensitiveMemberBinding)
            {
                bindFlags |= BindingFlags.IgnoreCase;
            }

            return bindFlags;
        }

        private BindingFlags GetMethodBindFlags()
        {
            return GetCommonBindFlags() | BindingFlags.OptionalParamBinding;
        }

        protected virtual string AdjustInvokeName(string name)
        {
            return name;
        }

        private void AdjustInvokeFlags(ref BindingFlags invokeFlags)
        {
            const BindingFlags onFlags =
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.OptionalParamBinding;

            const BindingFlags offFlags =
                BindingFlags.DeclaredOnly |
                BindingFlags.ExactBinding;

            const BindingFlags setPropertyFlags =
                BindingFlags.SetProperty |
                BindingFlags.PutDispProperty |
                BindingFlags.PutRefDispProperty;

            invokeFlags |= onFlags;
            invokeFlags &= ~offFlags;

            if (TargetFlags.HasFlag(HostTargetFlags.AllowStaticMembers))
            {
                invokeFlags |= BindingFlags.Static | BindingFlags.FlattenHierarchy;
            }
            else
            {
                invokeFlags &= ~BindingFlags.Static;
            }

            if (TargetFlags.HasFlag(HostTargetFlags.AllowInstanceMembers))
            {
                invokeFlags |= BindingFlags.Instance;
            }
            else
            {
                invokeFlags &= ~BindingFlags.Instance;
            }

            if (UseCaseInsensitiveMemberBinding)
            {
                invokeFlags |= BindingFlags.IgnoreCase;
            }
            else
            {
                invokeFlags &= ~BindingFlags.IgnoreCase;
            }

            if (invokeFlags.HasFlag(BindingFlags.GetProperty))
            {
                invokeFlags |= BindingFlags.GetField;
            }

            if ((invokeFlags & setPropertyFlags) != 0)
            {
                invokeFlags |= BindingFlags.SetField;
            }
        }

        private object InvokeReflectMember(string name, BindingFlags invokeFlags, object[] wrappedArgs, CultureInfo culture, string[] namedParams)
        {
            return InvokeReflectMember(name, invokeFlags, wrappedArgs, culture, namedParams, out _);
        }

        private object InvokeReflectMember(string name, BindingFlags invokeFlags, object[] wrappedArgs, CultureInfo culture, string[] namedParams, out bool isCacheable)
        {
            TryBindTargetMemberData();

            var args = Engine.MarshalToHost(wrappedArgs, false);

            var argOffset = 0;
            if ((namedParams != null) && (namedParams.Length > 0) && (namedParams[0] == SpecialParamNames.This))
            {
                args = args.Skip(1).ToArray();
                argOffset = 1;
            }

            object result;
            var bindArgs = args;
            if ((args.Length > 0) && (invokeFlags.HasFlag(BindingFlags.InvokeMethod) || invokeFlags.HasFlag(BindingFlags.CreateInstance)))
            {
                bindArgs = Engine.MarshalToHost(wrappedArgs, true);
                if (argOffset > 0)
                {
                    bindArgs = bindArgs.Skip(argOffset).ToArray();
                }

                var savedArgs = (object[])args.Clone();
                result = InvokeMember(name, invokeFlags, args, bindArgs, culture, false, out isCacheable);

                for (var index = 0; index < args.Length; index++)
                {
                    var arg = args[index];
                    if (!ReferenceEquals(arg, savedArgs[index]))
                    {
                        wrappedArgs[argOffset + index] = Engine.MarshalToScript(arg);
                    }
                }
            }
            else
            {
                result = InvokeMember(name, invokeFlags, args, bindArgs, culture, false, out isCacheable);
            }

            var marshalledResult = Engine.MarshalToScript(result);
            return marshalledResult;
        }

        private object InvokeDynamicMember(string name, BindingFlags invokeFlags, object[] args)
        {
            if (invokeFlags.HasFlag(BindingFlags.CreateInstance))
            {
                if (name == SpecialMemberNames.Default)
                {
                    return TargetDynamic.Invoke(true, args);
                }

                throw new InvalidOperationException("Invalid constructor invocation");
            }

            if (invokeFlags.HasFlag(BindingFlags.InvokeMethod))
            {
                if (name == SpecialMemberNames.Default)
                {
                    try
                    {
                        return TargetDynamic.Invoke(false, args);
                    }
                    catch
                    {
                        if (invokeFlags.HasFlag(BindingFlags.GetField) && (args.Length < 1))
                        {
                            return Target;
                        }

                        throw;
                    }
                }

                try
                {
                    return TargetDynamic.InvokeMethod(name, args);
                }
                catch
                {
                    if (invokeFlags.HasFlag(BindingFlags.GetField))
                    {
                        return TargetDynamic.GetProperty(name, args);
                    }

                    throw;
                }
            }

            if (invokeFlags.HasFlag(BindingFlags.GetField))
            {
                return TargetDynamic.GetProperty(name, args);
            }

            if (invokeFlags.HasFlag(BindingFlags.SetField))
            {
                if (args.Length > 0)
                {
                    TargetDynamic.SetProperty(name, args);
                    return args[args.Length - 1];
                }

                throw new InvalidOperationException("Invalid argument count");
            }

            throw new InvalidOperationException("Invalid member invocation mode");
        }

        private object InvokePropertyBagMember(string name, BindingFlags invokeFlags, object[] args, object[] bindArgs)
        {
            if (invokeFlags.HasFlag(BindingFlags.InvokeMethod))
            {
                object value;

                if (name == SpecialMemberNames.Default)
                {
                    if (invokeFlags.HasFlag(BindingFlags.GetField))
                    {
                        if (args.Length < 1)
                        {
                            return TargetPropertyBag;
                        }

                        if (args.Length == 1)
                        {
                            return TargetPropertyBag.TryGetValue(Convert.ToString(args[0]), out value) ? value : Nonexistent.Value;
                        }
                    }

                    throw new NotSupportedException("The object does not support the requested invocation operation");
                }

                if (name == SpecialMemberNames.NewEnum)
                {
                    return CreateEnumerator(TargetPropertyBag);
                }

                if (name == SpecialMemberNames.NewAsyncEnum)
                {
                    return CreateAsyncEnumerator(TargetPropertyBag);
                }

                if (!TargetPropertyBag.TryGetValue(name, out value))
                {
                    throw new MissingMemberException(MiscHelpers.FormatInvariant("The object has no property named '{0}'", name));
                }

                if (InvokeHelpers.TryInvokeObject(this, value, invokeFlags, args, bindArgs, true, out var result))
                {
                    return result;
                }

                if (invokeFlags.HasFlag(BindingFlags.GetField))
                {
                    if (args.Length < 1)
                    {
                        return value;
                    }

                    if (args.Length == 1)
                    {
                        if (value == null)
                        {
                            throw new InvalidOperationException("Cannot invoke a null property value");
                        }

                        return ((HostItem)Wrap(Engine, value)).InvokeMember(SpecialMemberNames.Default, invokeFlags, args, bindArgs, null, true);
                    }
                }

                throw new NotSupportedException("The object does not support the requested invocation operation");
            }

            if (invokeFlags.HasFlag(BindingFlags.GetField))
            {
                if (name == SpecialMemberNames.Default)
                {
                    if (args.Length == 1)
                    {
                        return TargetPropertyBag[Convert.ToString(args[0])];
                    }

                    throw new InvalidOperationException("Invalid argument count");
                }

                if (name == SpecialMemberNames.NewEnum)
                {
                    return CreateEnumerator(TargetPropertyBag);
                }

                if (name == SpecialMemberNames.NewAsyncEnum)
                {
                    return CreateAsyncEnumerator(TargetPropertyBag);
                }

                if (args.Length < 1)
                {
                    return TargetPropertyBag.TryGetValue(name, out var value) ? value : Nonexistent.Value;
                }

                throw new InvalidOperationException("Invalid argument count");
            }

            if (invokeFlags.HasFlag(BindingFlags.SetField))
            {
                if (name == SpecialMemberNames.Default)
                {
                    if (args.Length == 2)
                    {
                        return TargetPropertyBag[Convert.ToString(args[0])] = args[1];
                    }

                    throw new InvalidOperationException("Invalid argument count");
                }

                if (args.Length == 1)
                {
                    return TargetPropertyBag[name] = args[0];
                }

                if (args.Length == 2)
                {
                    if (TargetPropertyBag.TryGetValue(name, out var value))
                    {
                        if (value == null)
                        {
                            throw new InvalidOperationException("Cannot invoke a null property value");
                        }

                        return ((HostItem)Wrap(Engine, value)).InvokeMember(SpecialMemberNames.Default, invokeFlags, args, bindArgs, null, true);
                    }

                    throw new MissingMemberException(MiscHelpers.FormatInvariant("The object has no property named '{0}'", name));
                }

                throw new InvalidOperationException("Invalid argument count");
            }

            throw new InvalidOperationException("Invalid member invocation mode");
        }

        private object InvokeListElement(int index, BindingFlags invokeFlags, object[] args, object[] bindArgs)
        {
            if (invokeFlags.HasFlag(BindingFlags.InvokeMethod))
            {
                if (InvokeHelpers.TryInvokeObject(this, TargetList[index], invokeFlags, args, bindArgs, true, out var result))
                {
                    return result;
                }

                if (invokeFlags.HasFlag(BindingFlags.GetField) && (args.Length < 1))
                {
                    return TargetList[index];
                }

                throw new NotSupportedException("The object does not support the requested invocation operation");
            }

            if (invokeFlags.HasFlag(BindingFlags.GetField))
            {
                if (args.Length < 1)
                {
                    return TargetList[index];
                }

                throw new InvalidOperationException("Invalid argument count");
            }

            if (invokeFlags.HasFlag(BindingFlags.SetField))
            {
                if (args.Length == 1)
                {
                    return TargetList[index] = args[0];
                }

                throw new InvalidOperationException("Invalid argument count");
            }

            throw new InvalidOperationException("Invalid member invocation mode");
        }

        private object InvokeHostMember(string name, BindingFlags invokeFlags, object[] args, object[] bindArgs, out bool isCacheable)
        {
            isCacheable = false;
            object result;

            if (invokeFlags.HasFlag(BindingFlags.CreateInstance))
            {
                if (name == SpecialMemberNames.Default)
                {
                    if (Target is HostType hostType)
                    {
                        var typeArgs = GetTypeArgs(args).Select(HostType.Wrap).ToArray();
                        if (typeArgs.Length > 0)
                        {
                            // ReSharper disable CoVariantArrayConversion

                            if (hostType.TryInvoke(this, BindingFlags.InvokeMethod, typeArgs, typeArgs, out result))
                            {
                                hostType = result as HostType;
                                if (hostType != null)
                                {
                                    args = args.Skip(typeArgs.Length).ToArray();
                                    bindArgs = bindArgs.Skip(typeArgs.Length).ToArray();

                                    var specificType = hostType.GetSpecificType();
                                    if (typeof(Delegate).IsAssignableFrom(specificType))
                                    {
                                        if (args.Length != 1)
                                        {
                                            throw new InvalidOperationException("Invalid constructor invocation");
                                        }

                                        return DelegateFactory.CreateDelegate(Engine, args[0], specificType);
                                    }

                                    return specificType.CreateInstance(this, AccessContext, DefaultAccess, args, bindArgs);
                                }
                            }

                            throw new InvalidOperationException("Invalid constructor invocation");

                            // ReSharper restore CoVariantArrayConversion
                        }

                        var type = hostType.GetSpecificType();
                        if (typeof(Delegate).IsAssignableFrom(type))
                        {
                            if (args.Length != 1)
                            {
                                throw new InvalidOperationException("Invalid constructor invocation");
                            }

                            return DelegateFactory.CreateDelegate(Engine, args[0], type);
                        }

                        return type.CreateInstance(this, AccessContext, DefaultAccess, args, bindArgs);
                    }

                    if (TargetDynamicMetaObject != null)
                    {
                        if (TargetDynamicMetaObject.TryCreateInstance(args, out result))
                        {
                            return result;
                        }
                    }
                }

                throw new InvalidOperationException("Invalid constructor invocation");
            }

            if (invokeFlags.HasFlag(BindingFlags.InvokeMethod))
            {
                if (name == SpecialMemberNames.Default)
                {
                    if (InvokeHelpers.TryInvokeObject(this, Target, invokeFlags, args, bindArgs, TargetDynamicMetaObject != null, out result))
                    {
                        return result;
                    }

                    if (invokeFlags.HasFlag(BindingFlags.GetField))
                    {
                        result = GetHostProperty(name, invokeFlags, args, bindArgs, true, out isCacheable);
                        if (!(result is Nonexistent))
                        {
                            return result;
                        }

                        if (args.Length < 1)
                        {
                            return Target;
                        }

                        if (TargetDynamicMetaObject != null)
                        {
                            // dynamic target; don't throw for default indexed property retrieval failure

                            return result;
                        }
                    }

                    throw new NotSupportedException("The object does not support the requested invocation operation");
                }

                if (name == SpecialMemberNames.NewEnum)
                {
                    return CreateEnumerator();
                }

                if (name == SpecialMemberNames.NewAsyncEnum)
                {
                    return CreateAsyncEnumerator();
                }

                if ((TargetDynamicMetaObject != null) && TargetDynamicMetaObject.HasMember(name, invokeFlags.HasFlag(BindingFlags.IgnoreCase)))
                {
                    if (TargetDynamicMetaObject.TryInvokeMember(this, name, invokeFlags, args, out result))
                    {
                        return result;
                    }
                }

                if (ThisReflect.GetMethods(GetMethodBindFlags()).Any(method => string.Equals(method.Name, name, MemberNameComparison)))
                {
                    // The target appears to have a method with the right name, but it could be an
                    // extension method that fails to bind. If that happens, we should attempt the
                    // fallback but throw the original exception if the fallback fails as well.

                    try
                    {
                        return InvokeMethod(name, args, bindArgs);
                    }
                    catch (TargetInvocationException)
                    {
                        throw;
                    }
                    catch
                    {
                        // ReSharper disable EmptyGeneralCatchClause

                        try
                        {
                            if (invokeFlags.HasFlag(BindingFlags.GetField))
                            {
                                return GetHostProperty(name, invokeFlags, args, bindArgs, true, out isCacheable);
                            }

                        }
                        catch (TargetInvocationException)
                        {
                            throw;
                        }
                        catch
                        {
                        }

                        throw;

                        // ReSharper restore EmptyGeneralCatchClause
                    }
                }

                if (invokeFlags.HasFlag(BindingFlags.GetField))
                {
                    return GetHostProperty(name, invokeFlags, args, bindArgs, true, out isCacheable);
                }

                throw new MissingMethodException(MiscHelpers.FormatInvariant("The object has no suitable method named '{0}'", name));
            }

            if (invokeFlags.HasFlag(BindingFlags.GetField))
            {
                return GetHostProperty(name, invokeFlags, args, bindArgs, true, out isCacheable);
            }

            if (invokeFlags.HasFlag(BindingFlags.SetField))
            {
                return SetHostProperty(name, invokeFlags, args, bindArgs);
            }

            throw new InvalidOperationException("Invalid member invocation mode");
        }

        private object GetHostProperty(string name, BindingFlags invokeFlags, object[] args, object[] bindArgs, bool includeBoundMembers, out bool isCacheable)
        {
            isCacheable = false;

            if (name == SpecialMemberNames.Default)
            {
                var defaultProperty = Target.Type.GetScriptableDefaultProperty(invokeFlags, args, bindArgs, AccessContext, DefaultAccess);
                if (defaultProperty != null)
                {
                    return GetHostProperty(defaultProperty, invokeFlags, args);
                }

                if (TargetDynamicMetaObject != null)
                {
                    if (TargetDynamicMetaObject.TryGetIndex(args, out var result))
                    {
                        return result;
                    }
                }

                return Nonexistent.Value;
            }

            if (name == SpecialMemberNames.NewEnum)
            {
                return CreateEnumerator();
            }

            if (name == SpecialMemberNames.NewAsyncEnum)
            {
                return CreateAsyncEnumerator();
            }

            if ((TargetDynamicMetaObject != null) && (args.Length < 1))
            {
                int index;
                object result;

                if (TargetDynamicMetaObject.HasMember(name, invokeFlags.HasFlag(BindingFlags.IgnoreCase)))
                {
                    if (TargetDynamicMetaObject.TryGetMember(name, out result))
                    {
                        return result;
                    }

                    if (int.TryParse(name, NumberStyles.Integer, CultureInfo.InvariantCulture, out index))
                    {
                        if (TargetDynamicMetaObject.TryGetIndex(new object[] { index }, out result))
                        {
                            return result;
                        }
                    }

                    if (TargetDynamicMetaObject.TryGetIndex(new object[] { name }, out result))
                    {
                        return result;
                    }

                    if (includeBoundMembers)
                    {
                        if (HostMethodMap == null)
                        {
                            HostMethodMap = new Dictionary<string, HostMethod>(MemberNameComparer);
                        }

                        if (!HostMethodMap.TryGetValue(name, out var hostMethod))
                        {
                            hostMethod = new HostMethod(this, name);
                            HostMethodMap.Add(name, hostMethod);
                        }

                        return hostMethod;
                    }

                    return Nonexistent.Value;
                }

                if (int.TryParse(name, NumberStyles.Integer, CultureInfo.InvariantCulture, out index))
                {
                    if (TargetDynamicMetaObject.TryGetIndex(new object[] { index }, out result))
                    {
                        return result;
                    }
                }

                if (TargetDynamicMetaObject.TryGetIndex(new object[] { name }, out result))
                {
                    return result;
                }
            }

            var property = Target.Type.GetScriptableProperty(name, invokeFlags, args, bindArgs, AccessContext, DefaultAccess);
            if (property != null)
            {
                return GetHostProperty(property, invokeFlags, args);
            }

            if (args.Length > 0)
            {
                throw new MissingMemberException(MiscHelpers.FormatInvariant("The object has no suitable property named '{0}'", name));
            }

            var eventInfo = Target.Type.GetScriptableEvent(name, invokeFlags, AccessContext, DefaultAccess);
            if (eventInfo != null)
            {
                isCacheable = (TargetDynamicMetaObject == null);
                return typeof(EventSource<>).MakeSpecificType(eventInfo.EventHandlerType).CreateInstance(BindingFlags.NonPublic, Engine, Target.InvokeTarget, eventInfo);
            }

            var field = Target.Type.GetScriptableField(name, invokeFlags, AccessContext, DefaultAccess);
            if (field != null)
            {
                var result = field.GetValue(Target.InvokeTarget);
                isCacheable = (TargetDynamicMetaObject == null) && (field.IsLiteral || field.IsInitOnly);
                return Engine.PrepareResult(result, field.FieldType, field.GetScriptMemberFlags(), false);
            }

            if (includeBoundMembers)
            {
                if (Target.Type.GetScriptableProperties(name, invokeFlags, AccessContext, DefaultAccess).Any())
                {
                    if (HostIndexedPropertyMap == null)
                    {
                        HostIndexedPropertyMap = new Dictionary<string, HostIndexedProperty>();
                    }

                    if (!HostIndexedPropertyMap.TryGetValue(name, out var hostIndexedProperty))
                    {
                        hostIndexedProperty = new HostIndexedProperty(this, name);
                        HostIndexedPropertyMap.Add(name, hostIndexedProperty);
                    }

                    return hostIndexedProperty;
                }

                var method = ThisReflect.GetMethods(GetMethodBindFlags()).FirstOrDefault(testMethod => string.Equals(testMethod.Name, name, MemberNameComparison));
                if (method != null)
                {
                    if (HostMethodMap == null)
                    {
                        HostMethodMap = new Dictionary<string, HostMethod>(MemberNameComparer);
                    }

                    if (!HostMethodMap.TryGetValue(name, out var hostMethod))
                    {
                        hostMethod = new HostMethod(this, name);
                        HostMethodMap.Add(name, hostMethod);
                    }

                    isCacheable = (TargetDynamicMetaObject == null);
                    return hostMethod;
                }
            }

            return Nonexistent.Value;
        }

        private object GetHostProperty(PropertyInfo property, BindingFlags invokeFlags, object[] args)
        {
            if (reflectionProperties.Contains(property, MemberComparer<PropertyInfo>.Instance))
            {
                Engine.CheckReflection();
            }

            if ((property.GetIndexParameters().Length > 0) && (args.Length < 1) && !invokeFlags.HasFlag(BindingFlags.SuppressChangeType))
            {
                if (HostIndexedPropertyMap == null)
                {
                    HostIndexedPropertyMap = new Dictionary<string, HostIndexedProperty>();
                }

                var name = property.Name;
                if (!HostIndexedPropertyMap.TryGetValue(name, out var hostIndexedProperty))
                {
                    hostIndexedProperty = new HostIndexedProperty(this, name);
                    HostIndexedPropertyMap.Add(name, hostIndexedProperty);
                }

                return hostIndexedProperty;
            }

            var getMethod = property.GetMethod;
            if ((getMethod == null) || !getMethod.IsAccessible(AccessContext) || getMethod.IsBlockedFromScript(property.GetScriptAccess(DefaultAccess), false))
            {
                throw new UnauthorizedAccessException("The property get method is unavailable or inaccessible");
            }

            return InvokeHelpers.InvokeMethod(this, getMethod, Target.InvokeTarget, args, property.GetScriptMemberFlags());
        }

        private object SetHostProperty(string name, BindingFlags invokeFlags, object[] args, object[] bindArgs)
        {
            if (name == SpecialMemberNames.Default)
            {
                if (args.Length < 1)
                {
                    throw new InvalidOperationException("Invalid argument count");
                }

                object result;

                var defaultProperty = Target.Type.GetScriptableDefaultProperty(invokeFlags, args.Take(args.Length - 1).ToArray(), bindArgs.Take(bindArgs.Length - 1).ToArray(), AccessContext, DefaultAccess);
                if (defaultProperty != null)
                {
                    return SetHostProperty(defaultProperty, args);
                }

                if (args.Length < 2)
                {
                    throw new InvalidOperationException("Invalid argument count");
                }

                if (TargetDynamicMetaObject != null)
                {
                    if (TargetDynamicMetaObject.TrySetIndex(args.Take(args.Length - 1).ToArray(), args[args.Length - 1], out result))
                    {
                        return result;
                    }
                }

                // special case to enable JScript/VBScript "x(a) = b" syntax when x is a host indexed property 

                if (InvokeHelpers.TryInvokeObject(this, Target, invokeFlags, args, bindArgs, false, out result))
                {
                    return result;
                }

                throw new InvalidOperationException("Invalid property assignment");
            }

            if ((TargetDynamicMetaObject != null) && (args.Length == 1))
            {
                if (TargetDynamicMetaObject.TrySetMember(name, args[0], out var result))
                {
                    return result;
                }

                if (int.TryParse(name, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index))
                {
                    if (TargetDynamicMetaObject.TrySetIndex(new object[] { index }, args[0], out result))
                    {
                        return result;
                    }
                }

                if (TargetDynamicMetaObject.TrySetIndex(new object[] { name }, args[0], out result))
                {
                    return result;
                }
            }

            if (args.Length < 1)
            {
                throw new InvalidOperationException("Invalid argument count");
            }

            var property = Target.Type.GetScriptableProperty(name, invokeFlags, args.Take(args.Length - 1).ToArray(), bindArgs.Take(bindArgs.Length - 1).ToArray(), AccessContext, DefaultAccess);
            if (property != null)
            {
                return SetHostProperty(property, args);
            }

            var field = Target.Type.GetScriptableField(name, invokeFlags, AccessContext, DefaultAccess);
            if (field != null)
            {
                if (args.Length == 1)
                {
                    if (field.IsLiteral || field.IsInitOnly || field.IsReadOnlyForScript(DefaultAccess))
                    {
                        throw new UnauthorizedAccessException("The field is read-only");
                    }

                    var value = args[0];
                    if (field.FieldType.IsAssignableFromValue(ref value))
                    {
                        field.SetValue(Target.InvokeTarget, value);
                        return value;
                    }

                    throw new ArgumentException("Invalid field assignment");
                }

                throw new InvalidOperationException("Invalid argument count");
            }

            throw new MissingMemberException(MiscHelpers.FormatInvariant("The object has no suitable property or field named '{0}'", name));
        }

        private object SetHostProperty(PropertyInfo property, object[] args)
        {
            var scriptAccess = property.GetScriptAccess(DefaultAccess);
            if (scriptAccess == ScriptAccess.ReadOnly)
            {
                throw new UnauthorizedAccessException("The property is read-only");
            }

            var setMethod = property.SetMethod;
            if ((setMethod == null) || !setMethod.IsAccessible(AccessContext) || setMethod.IsBlockedFromScript(scriptAccess, false))
            {
                throw new UnauthorizedAccessException("The property set method is unavailable or inaccessible");
            }

            var value = args[args.Length - 1];

            var argCount = args.Length - 1;
            var paramCount = property.GetIndexParameters().Length;
            if (argCount < paramCount)
            {
                var missingArgs = Enumerable.Repeat(Missing.Value, paramCount - argCount).ToArray();
                args = args.Take(argCount).Concat(missingArgs).Concat(value.ToEnumerable()).ToArray();
            }

            if (property.PropertyType.IsAssignableFromValue(ref value))
            {
                args[args.Length - 1] = value;
                InvokeHelpers.InvokeMethod(this, setMethod, Target.InvokeTarget, args, property.GetScriptMemberFlags());
                return value;
            }

            // Some COM properties have setters where the final parameter type doesn't match
            // the property type. The latter has failed, so let's try the former.

            var setParams = setMethod.GetParameters();
            if ((setParams.Length >= args.Length) && (setParams[args.Length - 1].ParameterType.IsAssignableFromValue(ref value)))
            {
                args[args.Length - 1] = value;
                InvokeHelpers.InvokeMethod(this, setMethod, Target.InvokeTarget, args, property.GetScriptMemberFlags());
                return value;
            }

            throw new ArgumentException("Invalid property assignment");
        }

        private static object CreateEnumerator<T>(IEnumerable<T> enumerable)
        {
            return HostObject.Wrap(enumerable.GetEnumerator(), typeof(IEnumerator<T>));
        }

        private object CreateEnumerator()
        {
            if ((Target is HostObject) || (Target is IHostVariable) || (Target is IByRefArg))
            {
                if ((Target.InvokeTarget != null) && Target.Type.IsAssignableToGenericType(typeof(IEnumerable<>), out var typeArgs))
                {
                    var enumerableHelpersHostItem = Wrap(Engine, typeof(EnumerableHelpers<>).MakeGenericType(typeArgs).InvokeMember("HostType", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, null, null), HostItemFlags.PrivateAccess);
                    if (MiscHelpers.Try(out var enumerator, () => ((IDynamic)enumerableHelpersHostItem).InvokeMethod("GetEnumerator", this)))
                    {
                        return enumerator;
                    }
                }
                else if (BindSpecialTarget(out IEnumerable _))
                {
                    var enumerableHelpersHostItem = Wrap(Engine, EnumerableHelpers.HostType, HostItemFlags.PrivateAccess);
                    if (MiscHelpers.Try(out var enumerator, () => ((IDynamic)enumerableHelpersHostItem).InvokeMethod("GetEnumerator", this)))
                    {
                        return enumerator;
                    }
                }
            }

            throw new NotSupportedException("The object is not enumerable");
        }

        #endregion

        #endregion

        #region Object overrides

        public override string ToString()
        {
            return MiscHelpers.FormatInvariant("[{0}]", Target);
        }

        #endregion

        #region DynamicObject overrides

        public override bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result)
        {
            result = ThisDynamic.Invoke(true, args).ToDynamicResult(Engine);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = ThisDynamic.GetProperty(binder.Name).ToDynamicResult(Engine);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            ThisDynamic.SetProperty(binder.Name, value);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indices, out object result)
        {
            if (indices.Length == 1)
            {
                if (MiscHelpers.TryGetNumericIndex(indices[0], out int index))
                {
                    result = ThisDynamic.GetProperty(index).ToDynamicResult(Engine);
                    return true;
                }

                result = ThisDynamic.GetProperty(indices[0].ToString()).ToDynamicResult(Engine);
                return true;
            }

            if (indices.Length > 1)
            {
                result = ThisDynamic.GetProperty(SpecialMemberNames.Default, indices).ToDynamicResult(Engine);
                return true;
            }

            throw new InvalidOperationException("Invalid argument or index count");
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indices, object value)
        {
            if (indices.Length == 1)
            {
                if (MiscHelpers.TryGetNumericIndex(indices[0], out int index))
                {
                    ThisDynamic.SetProperty(index, value);
                    return true;
                }

                ThisDynamic.SetProperty(indices[0].ToString(), value);
                return true;
            }

            if (indices.Length > 1)
            {
                ThisDynamic.SetProperty(SpecialMemberNames.Default, indices.Concat(value.ToEnumerable()).ToArray());
            }

            throw new InvalidOperationException("Invalid argument or index count");
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            result = ThisDynamic.Invoke(false, args).ToDynamicResult(Engine);
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = ThisDynamic.InvokeMethod(binder.Name, args).ToDynamicResult(Engine);
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if ((Target is HostObject) || (Target is IHostVariable) || (Target is IByRefArg))
            {
                if (binder.Type.IsAssignableFrom(Target.Type))
                {
                    result = Convert.ChangeType(Target.InvokeTarget, binder.Type);
                    return true;
                }
            }

            result = null;
            return false;
        }

        #endregion

        #region IReflect implementation

        FieldInfo IReflect.GetField(string name, BindingFlags bindFlags)
        {
            var fields = ThisReflect.GetFields(bindFlags).Where(field => string.Equals(field.Name, name, bindFlags.GetMemberNameComparison())).ToArray();
            if (fields.Length < 1)
            {
                return null;
            }

            if (fields.Length > 1)
            {
                throw new AmbiguousMatchException(MiscHelpers.FormatInvariant("The object has multiple fields named '{0}'", name));
            }

            return fields[0];
        }

        FieldInfo[] IReflect.GetFields(BindingFlags bindFlags)
        {
            TryBindTargetMemberData();

            UpdateFieldNames(out var updated);
            if (updated || (AllFields == null))
            {
                AllFields = MemberMap.GetFields(AllFieldNames);
            }

            return AllFields;
        }

        MemberInfo[] IReflect.GetMember(string name, BindingFlags bindFlags)
        {
            return ThisReflect.GetMembers(bindFlags).Where(member => string.Equals(member.Name, name, bindFlags.GetMemberNameComparison())).ToArray();
        }

        MemberInfo[] IReflect.GetMembers(BindingFlags bindFlags)
        {
            return ThisReflect.GetFields(bindFlags).Cast<MemberInfo>().Concat(ThisReflect.GetMethods(bindFlags)).Concat(ThisReflect.GetProperties(bindFlags)).ToArray();
        }

        MethodInfo IReflect.GetMethod(string name, BindingFlags bindFlags)
        {
            var methods = ThisReflect.GetMethods(bindFlags).Where(method => string.Equals(method.Name, name, bindFlags.GetMemberNameComparison())).ToArray();
            if (methods.Length < 1)
            {
                return null;
            }

            if (methods.Length > 1)
            {
                throw new AmbiguousMatchException(MiscHelpers.FormatInvariant("The object has multiple methods named '{0}'", name));
            }

            return methods[0];
        }

        MethodInfo IReflect.GetMethod(string name, BindingFlags bindFlags, Binder binder, Type[] types, ParameterModifier[] modifiers)
        {
            throw new NotImplementedException();
        }

        MethodInfo[] IReflect.GetMethods(BindingFlags bindFlags)
        {
            TryBindTargetMemberData();

            UpdateMethodNames(out var updated);
            if (updated || (AllMethods == null))
            {
                AllMethods = MemberMap.GetMethods(AllMethodNames);
            }

            return AllMethods;
        }

        PropertyInfo[] IReflect.GetProperties(BindingFlags bindFlags)
        {
            TryBindTargetMemberData();

            UpdatePropertyNames(out var updated);
            if (updated || (AllProperties == null))
            {
                AllProperties = MemberMap.GetProperties(AllPropertyNames);
            }

            return AllProperties;
        }

        PropertyInfo IReflect.GetProperty(string name, BindingFlags bindFlags, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            throw new NotImplementedException();
        }

        PropertyInfo IReflect.GetProperty(string name, BindingFlags bindFlags)
        {
            var properties = ThisReflect.GetProperties(bindFlags).Where(property => string.Equals(property.Name, name, bindFlags.GetMemberNameComparison())).ToArray();
            if (properties.Length < 1)
            {
                return null;
            }

            if (properties.Length > 1)
            {
                throw new AmbiguousMatchException(MiscHelpers.FormatInvariant("The object has multiple properties named '{0}'", name));
            }

            return properties[0];
        }

        object IReflect.InvokeMember(string name, BindingFlags invokeFlags, Binder binder, object invokeTarget, object[] wrappedArgs, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParams)
        {
            return InvokeReflectMember(name, invokeFlags, wrappedArgs, culture, namedParams);
        }

        Type IReflect.UnderlyingSystemType => throw new NotImplementedException();

        #endregion

        #region IDynamic implementation

        object IDynamic.GetProperty(string name, params object[] args)
        {
            return InvokeReflectMember(name, BindingFlags.GetProperty, args, CultureInfo.InvariantCulture, null);
        }

        object IDynamic.GetProperty(string name, out bool isCacheable, params object[] args)
        {
            return InvokeReflectMember(name, BindingFlags.GetProperty, args, CultureInfo.InvariantCulture, null, out isCacheable);
        }

        void IDynamic.SetProperty(string name, object[] args)
        {
            ThisReflect.InvokeMember(name, BindingFlags.SetProperty, null, ThisReflect, args, null, CultureInfo.InvariantCulture, null);
        }

        bool IDynamic.DeleteProperty(string name)
        {
            TryBindTargetMemberData();

            if (TargetDynamic != null)
            {
                return TargetDynamic.DeleteProperty(name);
            }

            if (TargetPropertyBag != null)
            {
                return TargetPropertyBag.Remove(name);
            }

            if (TargetDynamicMetaObject != null)
            {
                if (TargetDynamicMetaObject.TryDeleteMember(name, out var result) && result)
                {
                    return true;
                }

                if (TargetDynamicMetaObject.TryDeleteIndex(new object[] { name }, out result))
                {
                    return result;
                }

                throw new InvalidOperationException("Invalid dynamic member deletion");
            }

            throw new NotSupportedException("The object does not support dynamic members");
        }

        string[] IDynamic.GetPropertyNames()
        {
            TryBindTargetMemberData();

            UpdateFieldNames(out var updatedFieldNames);
            UpdateMethodNames(out var updatedMethodNames);
            UpdatePropertyNames(out var updatedPropertyNames);
            UpdateEnumerationSettingsToken(out var updatedEnumerationSettingsToken);

            if (updatedFieldNames || updatedMethodNames || updatedPropertyNames || updatedEnumerationSettingsToken || (AllMemberNames == null))
            {
                AllMemberNames = AllFieldNames.Concat(EnumeratedMethodNames).Concat(AllPropertyNames).ExcludeIndices().Distinct().ToArray();
            }

            return AllMemberNames;
        }

        object IDynamic.GetProperty(int index)
        {
            return ThisDynamic.GetProperty(index.ToString(CultureInfo.InvariantCulture));
        }

        void IDynamic.SetProperty(int index, object value)
        {
            ThisDynamic.SetProperty(index.ToString(CultureInfo.InvariantCulture), value);
        }

        bool IDynamic.DeleteProperty(int index)
        {
            TryBindTargetMemberData();

            if (TargetDynamic != null)
            {
                return TargetDynamic.DeleteProperty(index);
            }

            if (TargetPropertyBag != null)
            {
                return TargetPropertyBag.Remove(index.ToString(CultureInfo.InvariantCulture));
            }

            if (TargetDynamicMetaObject != null)
            {
                if (TargetDynamicMetaObject.TryDeleteMember(index.ToString(CultureInfo.InvariantCulture), out var result) && result)
                {
                    return true;
                }

                if (TargetDynamicMetaObject.TryDeleteIndex(new object[] { index }, out result))
                {
                    return result;
                }

                throw new InvalidOperationException("Invalid dynamic member deletion");
            }

            return false;
        }

        int[] IDynamic.GetPropertyIndices()
        {
            TryBindTargetMemberData();

            UpdatePropertyNames(out var updated);
            if (updated || (PropertyIndices == null))
            {
                PropertyIndices = AllPropertyNames.GetIndices().Distinct().ToArray();
            }

            return PropertyIndices;
        }

        object IDynamic.Invoke(bool asConstructor, params object[] args)
        {
            return ThisReflect.InvokeMember(SpecialMemberNames.Default, asConstructor ? BindingFlags.CreateInstance : ((args.Length < 1) ? BindingFlags.InvokeMethod : BindingFlags.InvokeMethod | BindingFlags.GetProperty), null, ThisReflect, args, null, CultureInfo.InvariantCulture, null);
        }

        object IDynamic.InvokeMethod(string name, params object[] args)
        {
            return ThisReflect.InvokeMember(name, BindingFlags.InvokeMethod, null, ThisReflect, args, null, CultureInfo.InvariantCulture, null);
        }

        #endregion

        #region IScriptMarshalWrapper implementation

        public ScriptEngine Engine { get; }

        public object Unwrap()
        {
            return Target.Target;
        }

        #endregion

        #region IHostInvokeContext implementation

        public Type AccessContext => CachedAccessContext;

        public ScriptAccess DefaultAccess => CachedDefaultAccess;

        public HostTargetFlags TargetFlags => CachedTargetFlags;

        #endregion

        #region Nested type: ExpandoHostItem

        private class ExpandoHostItem : HostItem, IExpando
        {
            #region constructors

            // ReSharper disable MemberCanBeProtected.Local

            public ExpandoHostItem(ScriptEngine engine, HostTarget target, HostItemFlags flags)
                : base(engine, target, flags)
            {
            }

            // ReSharper restore MemberCanBeProtected.Local

            #endregion

            #region IExpando implementation

            FieldInfo IExpando.AddField(string name)
            {
                TryBindTargetMemberData();

                if (CanAddExpandoMembers())
                {
                    AddExpandoMemberName(name);
                    return MemberMap.GetField(name);
                }

                throw new NotSupportedException("The object does not support dynamic fields");
            }

            PropertyInfo IExpando.AddProperty(string name)
            {
                TryBindTargetMemberData();

                if (CanAddExpandoMembers())
                {
                    AddExpandoMemberName(name);
                    return MemberMap.GetProperty(name);
                }

                throw new NotSupportedException("The object does not support dynamic properties");
            }

            MethodInfo IExpando.AddMethod(string name, Delegate method)
            {
                throw new NotImplementedException();
            }

            void IExpando.RemoveMember(MemberInfo member)
            {
                RemoveMember(member.Name);
            }

            protected virtual bool RemoveMember(string name)
            {
                TryBindTargetMemberData();

                if (TargetDynamic != null)
                {
                    if (int.TryParse(name, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index))
                    {
                        if (TargetDynamic.DeleteProperty(index))
                        {
                            RemoveExpandoMemberName(index.ToString(CultureInfo.InvariantCulture));
                            return true;
                        }
                    }
                    else if (TargetDynamic.DeleteProperty(name))
                    {
                        RemoveExpandoMemberName(name);
                        return true;
                    }
                }
                else if (TargetPropertyBag != null)
                {
                    if (TargetPropertyBag.Remove(name))
                    {
                        RemoveExpandoMemberName(name);
                        return true;
                    }
                }
                else if (TargetDynamicMetaObject != null)
                {
                    if (TargetDynamicMetaObject.TryDeleteMember(name, out var result) && result)
                    {
                        RemoveExpandoMemberName(name);
                        return true;
                    }

                    if (int.TryParse(name, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index) && TargetDynamicMetaObject.TryDeleteIndex(new object[] { index }, out result))
                    {
                        RemoveExpandoMemberName(index.ToString(CultureInfo.InvariantCulture));
                        return true;
                    }

                    if (TargetDynamicMetaObject.TryDeleteIndex(new object[] { name }, out result))
                    {
                        RemoveExpandoMemberName(name);
                        return true;
                    }
                }
                else
                {
                    throw new NotSupportedException("The object does not support dynamic members");
                }

                return false;
            }

            #endregion
        }

        #endregion
    }
}
