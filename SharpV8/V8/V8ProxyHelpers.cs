// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.ClearScript.Util;

namespace Microsoft.ClearScript.V8
{
    internal static class V8ProxyHelpers
    {
        #region host object lifetime

        public static IntPtr AddRefHostObject(IntPtr pObject)
        {
            return AddRefHostObject(GetHostObject(pObject));
        }

        public static IntPtr AddRefHostObject(object obj)
        {
            return GCHandle.ToIntPtr(GCHandle.Alloc(obj));
        }

        public static void ReleaseHostObject(IntPtr pObject)
        {
            GCHandle.FromIntPtr(pObject).Free();
        }

        public static Scope<IntPtr, HostObjectReleaseAction> CreateAddRefHostObjectScope(object obj)
        {
            return new Scope<IntPtr, HostObjectReleaseAction>(AddRefHostObject(obj));
        }

        public readonly struct HostObjectReleaseAction : IScopeAction<IntPtr>
        {
            public void Invoke(IntPtr obj) => ReleaseHostObject(obj);
        }

        #endregion

        #region host object access

        public static object GetHostObject(IntPtr pObject)
        {
            return GCHandle.FromIntPtr(pObject).Target;
        }

        public static T GetHostObject<T>(IntPtr pObject) where T : class
        {
            return (T)GetHostObject(pObject);
        }

        public static object GetHostObjectProperty(IntPtr pObject, ReadOnlySpan<char> name)
        {
            return GetHostObjectProperty(GetHostObject(pObject), name);
        }

        public static object GetHostObjectProperty(object obj, ReadOnlySpan<char> name)
        {
            return ((IDynamic)obj).GetProperty(name.ToString());
        }

        public static object GetHostObjectProperty(IntPtr pObject, ReadOnlySpan<char> name, out bool isCacheable)
        {
            return GetHostObjectProperty(GetHostObject(pObject), name, out isCacheable);
        }

        public static object GetHostObjectProperty(object obj, ReadOnlySpan<char> name, out bool isCacheable)
        {
            return ((IDynamic)obj).GetProperty(name.ToString(), out isCacheable);
        }

        public static void SetHostObjectProperty(IntPtr pObject, ReadOnlySpan<char> name, object value)
        {
            SetHostObjectProperty(GetHostObject(pObject), name, value);
        }

        public static void SetHostObjectProperty(object obj, ReadOnlySpan<char> name, object value)
        {
            ((IDynamic)obj).SetProperty(name.ToString(), value);
        }

        public static bool DeleteHostObjectProperty(IntPtr pObject, ReadOnlySpan<char> name)
        {
            return DeleteHostObjectProperty(GetHostObject(pObject), name);
        }

        public static bool DeleteHostObjectProperty(object obj, ReadOnlySpan<char> name)
        {
            return ((IDynamic)obj).DeleteProperty(name.ToString());
        }

        public static string[] GetHostObjectPropertyNames(IntPtr pObject)
        {
            return GetHostObjectPropertyNames(GetHostObject(pObject));
        }

        public static string[] GetHostObjectPropertyNames(object obj)
        {
            return ((IDynamic)obj).GetPropertyNames();
        }

        public static object GetHostObjectProperty(IntPtr pObject, int index)
        {
            return GetHostObjectProperty(GetHostObject(pObject), index);
        }

        public static object GetHostObjectProperty(object obj, int index)
        {
            return ((IDynamic)obj).GetProperty(index);
        }

        public static void SetHostObjectProperty(IntPtr pObject, int index, object value)
        {
            SetHostObjectProperty(GetHostObject(pObject), index, value);
        }

        public static void SetHostObjectProperty(object obj, int index, object value)
        {
            ((IDynamic)obj).SetProperty(index, value);
        }

        public static bool DeleteHostObjectProperty(IntPtr pObject, int index)
        {
            return DeleteHostObjectProperty(GetHostObject(pObject), index);
        }

        public static bool DeleteHostObjectProperty(object obj, int index)
        {
            return ((IDynamic)obj).DeleteProperty(index);
        }

        public static int[] GetHostObjectPropertyIndices(IntPtr pObject)
        {
            return GetHostObjectPropertyIndices(GetHostObject(pObject));
        }

        public static int[] GetHostObjectPropertyIndices(object obj)
        {
            return ((IDynamic)obj).GetPropertyIndices();
        }

        public static object InvokeHostObject(IntPtr pObject, bool asConstructor, object[] args)
        {
            return InvokeHostObject(GetHostObject(pObject), asConstructor, args);
        }

        public static object InvokeHostObject(object obj, bool asConstructor, object[] args)
        {
            return ((IDynamic)obj).Invoke(asConstructor, args);
        }

        public static object InvokeHostObjectMethod(IntPtr pObject, ReadOnlySpan<char> name, object[] args)
        {
            return InvokeHostObjectMethod(GetHostObject(pObject), name, args);
        }

        public static object InvokeHostObjectMethod(object obj, ReadOnlySpan<char> name, object[] args)
        {
            return ((IDynamic)obj).InvokeMethod(name.ToString(), args);
        }

        public static Invocability GetHostObjectInvocability(IntPtr pObject)
        {
            return GetHostObjectInvocability(GetHostObject(pObject));
        }

        public static Invocability GetHostObjectInvocability(object obj)
        {
            var hostItem = obj as HostItem;
            if (hostItem == null)
            {
                return Invocability.None;
            }

            return hostItem.Invocability;
        }

        public static object GetHostObjectEnumerator(IntPtr pObject)
        {
            return GetHostObjectEnumerator(GetHostObject(pObject));
        }

        public static object GetHostObjectEnumerator(object obj)
        {
            return ((IDynamic)obj).GetProperty(SpecialMemberNames.NewEnum);
        }

        public static object GetHostObjectAsyncEnumerator(IntPtr pObject)
        {
            return GetHostObjectAsyncEnumerator(GetHostObject(pObject));
        }

        public static object GetHostObjectAsyncEnumerator(object obj)
        {
            return ((IDynamic)obj).GetProperty(SpecialMemberNames.NewAsyncEnum);
        }

        #endregion

        #region exception marshaling

        public static object MarshalExceptionToScript(IntPtr pSource, Exception exception)
        {
            return MarshalExceptionToScript(GetHostObject(pSource), exception);
        }

        public static object MarshalExceptionToScript(object source, Exception exception)
        {
            return ((IScriptMarshalWrapper)source).Engine.MarshalToScript(exception);
        }

        public static Exception MarshalExceptionToHost(object exception)
        {
            return (Exception)((IScriptMarshalWrapper)exception)?.Engine.MarshalToHost(exception, false);
        }

        #endregion

        #region module support

        public static string LoadModule(IntPtr pSourceDocumentInfo, string specifier, DocumentCategory category, out UniqueDocumentInfo documentInfo)
        {
            var engine = ScriptEngine.Current;
            if (engine == null)
            {
                throw new InvalidOperationException("Module loading requires a script engine");
            }

            var settings = engine.DocumentSettings;
            var document = settings.LoadDocument(((UniqueDocumentInfo)GetHostObject(pSourceDocumentInfo)).Info, specifier, category, null);
            var code = document.GetTextContents();

            documentInfo = document.Info.MakeUnique(engine);
            return code;
        }

        public static IDictionary<string, object> CreateModuleContext(IntPtr pDocumentInfo)
        {
            var engine = ScriptEngine.Current;
            if (engine == null)
            {
                throw new InvalidOperationException("Module context construction requires a script engine");
            }

            var documentInfo = (UniqueDocumentInfo)GetHostObject(pDocumentInfo);

            var callback = documentInfo.ContextCallback ?? engine.DocumentSettings.ContextCallback;
            if ((callback != null) && MiscHelpers.Try(out var sharedContext, () => callback(documentInfo.Info)) && (sharedContext != null))
            {
                var context = new Dictionary<string, object>(sharedContext.Count);
                foreach (var pair in sharedContext)
                {
                    context.Add(pair.Key, engine.MarshalToScript(pair.Value));
                }

                return context;
            }

            return null;
        }

        #endregion
    }
}
