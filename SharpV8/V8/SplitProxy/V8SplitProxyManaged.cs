// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.Properties;
using Microsoft.ClearScript.Util;

namespace Microsoft.ClearScript.V8.SplitProxy
{
    // ReSharper disable once PartialTypeWithSinglePart
    internal static unsafe partial class V8SplitProxyManaged
    {
        private static IntPtr DelegateTable { get; }
        public static IntPtr MethodTable { get; }
        public static int MethodCount { get; }

        [ThreadStatic]
        public static Exception? ScheduledException;

        static V8SplitProxyManaged()
        {
            (DelegateTable, MethodTable, MethodCount) = Initialize();
        }

        private static (IntPtr delegatePtrs, IntPtr functionPtrs, int methodCount) Initialize()
        {
            var nativeVersion = V8SplitProxyNative.GetVersion();
            if (!nativeVersion.SequenceEqual(ClearScriptVersion.Informational))
            {
                throw new InvalidOperationException(
                    $"V8 native assembly: loaded version {nativeVersion} does not" +
                    $" match required version {ClearScriptVersion.Informational}.");
            }

            return CreateMethodTable();
        }

        private static void Teardown()
        {
            DestroyMethodTable();
        }

        private static void ScheduleHostException(IntPtr pObject, Exception exception)
        {
            V8SplitProxyNative.InvokeNoThrow(() => V8SplitProxyNative.HostException_Schedule(
                exception.GetBaseException().Message, V8ProxyHelpers.MarshalExceptionToScript(pObject, exception)));
        }

        private static void ScheduleHostException(Exception exception)
        {
            V8SplitProxyNative.InvokeNoThrow(() => V8SplitProxyNative.HostException_Schedule(
                exception.GetBaseException().Message, ScriptEngine.Current?.MarshalToScript(exception)));
        }

        private static uint GetMaxCacheSizeForCategory(DocumentCategory category)
        {
            return Math.Max(16U, category.MaxCacheSize);
        }

        #region method delegates

        // ReSharper disable UnusedType.Local

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawScheduleForwardingException(
            V8ValuePtr pException
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawScheduleInvalidOperationException(
            StdStringPtr pMessage
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawScheduleScriptEngineException(
            StdStringPtr pEngineName,
            StdStringPtr pMessage,
            StdStringPtr pStackTrace,
            sbyte isFatal,
            sbyte executionStarted,
            V8ValuePtr pScriptException,
            V8ValuePtr pInnerException
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawScheduleScriptInterruptedException(
            StdStringPtr pEngineName,
            StdStringPtr pMessage,
            StdStringPtr pStackTrace,
            sbyte isFatal,
            sbyte executionStarted,
            V8ValuePtr pScriptException,
            V8ValuePtr pInnerException
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawInvokeAction(
            IntPtr pAction
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawProcessArrayBufferOrViewData(
            IntPtr pData,
            IntPtr pAction
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawProcessCpuProfile(
            V8CpuProfilePtr pProfile,
            IntPtr pAction
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr RawCreateV8ObjectCache();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawCacheV8Object(
            IntPtr pCache,
            IntPtr pObject,
            IntPtr pV8Object
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr RawGetCachedV8Object(
            IntPtr pCache,
            IntPtr pObject
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawGetAllCachedV8Objects(
            IntPtr pCache,
            StdPtrArrayPtr pV8ObjectPtrs
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate sbyte RawRemoveV8ObjectCacheEntry(
            IntPtr pCache,
            IntPtr pObject
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr RawCreateDebugAgent(
            StdStringPtr pName,
            StdStringPtr pVersion,
            int port,
            sbyte remote,
            V8DebugCallbackHandle hCallback
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawSendDebugMessage(
            IntPtr pAgent,
            StdStringPtr pContent
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawDestroyDebugAgent(
            IntPtr pAgent
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint RawGetMaxScriptCacheSize();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint RawGetMaxModuleCacheSize();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr RawAddRefHostObject(
            IntPtr pObject
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawReleaseHostObject(
            IntPtr pObject
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Invocability RawGetHostObjectInvocability(
            IntPtr pObject
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawGetHostObjectNamedProperty(
            IntPtr pObject,
            StdStringPtr pName,
            V8ValuePtr pValue
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawGetHostObjectNamedPropertyWithCacheability(
            IntPtr pObject,
            StdStringPtr pName,
            V8ValuePtr pValue,
            sbyte* isCacheable
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawSetHostObjectNamedProperty(
            IntPtr pObject,
            StdStringPtr pName,
            V8ValuePtr pValue
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate sbyte RawDeleteHostObjectNamedProperty(
            IntPtr pObject,
            StdStringPtr pName
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawGetHostObjectPropertyNames(
            IntPtr pObject,
            StdStringArrayPtr pNames
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawGetHostObjectIndexedProperty(
            IntPtr pObject,
            int index,
            V8ValuePtr pValue
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawSetHostObjectIndexedProperty(
            IntPtr pObject,
            int index,
            V8ValuePtr pValue
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate sbyte RawDeleteHostObjectIndexedProperty(
            IntPtr pObject,
            int index
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawGetHostObjectPropertyIndices(
            IntPtr pObject,
            StdInt32ArrayPtr pIndices
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawInvokeHostObject(
            IntPtr pObject,
            sbyte asConstructor,
            StdV8ValueArrayPtr pArgs,
            V8ValuePtr pResult
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawInvokeHostObjectMethod(
            IntPtr pObject,
            StdStringPtr pName,
            StdV8ValueArrayPtr pArgs,
            V8ValuePtr pResult
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawGetHostObjectEnumerator(
            IntPtr pObject,
            V8ValuePtr pResult
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawGetHostObjectAsyncEnumerator(
            IntPtr pObject,
            V8ValuePtr pResult
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawQueueNativeCallback(
            NativeCallbackHandle hCallback
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr RawCreateNativeCallbackTimer(
            int dueTime,
            int period,
            NativeCallbackHandle hCallback
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate sbyte RawChangeNativeCallbackTimer(
            IntPtr pTimer,
            int dueTime,
            int period
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawDestroyNativeCallbackTimer(
            IntPtr pTimer
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawLoadModule(
            IntPtr pSourceDocumentInfo,
            StdStringPtr pSpecifier,
            StdStringPtr pResourceName,
            StdStringPtr pSourceMapUrl,
            ulong* uniqueId,
            sbyte* isModule,
            StdStringPtr pCode,
            IntPtr* pDocumentInfo
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawCreateModuleContext(
            IntPtr pDocumentInfo,
            StdStringArrayPtr pNames,
            StdV8ValueArrayPtr pValues
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RawWriteBytesToStream(
            IntPtr pStream,
            IntPtr pBytes,
            int count
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate V8GlobalFlags RawGetGlobalFlags();

        // ReSharper restore UnusedType.Local

        #endregion

        #region method table construction and teardown

        private static (IntPtr delegatePtrs, IntPtr functionPtrs, int methodCount) CreateMethodTable()
        {
            (IntPtr, IntPtr)[] methodPairs =
            {
                //----------------------------------------------------------------------------
                // IMPORTANT: maintain synchronization with V8_SPLIT_PROXY_MANAGED_METHOD_LIST
                //----------------------------------------------------------------------------

                GetMethodPair<RawScheduleForwardingException>(ScheduleForwardingException),
                GetMethodPair<RawScheduleInvalidOperationException>(ScheduleInvalidOperationException),
                GetMethodPair<RawScheduleScriptEngineException>(ScheduleScriptEngineException),
                GetMethodPair<RawScheduleScriptInterruptedException>(ScheduleScriptInterruptedException),

            #if NET5_0_OR_GREATER
                (IntPtr.Zero, InvokeHostActionFastMethodPtr),
            #else
                GetMethodPair<RawInvokeAction>(InvokeHostAction),
            #endif

                GetMethodPair<RawProcessArrayBufferOrViewData>(ProcessArrayBufferOrViewData),
                GetMethodPair<RawProcessCpuProfile>(ProcessCpuProfile),
                GetMethodPair<RawCreateV8ObjectCache>(CreateV8ObjectCache),

            #if NET5_0_OR_GREATER
                (IntPtr.Zero, CacheV8ObjectFastMethodPtr),
                (IntPtr.Zero, GetCachedV8ObjectFastMethodPtr),
            #else
                GetMethodPair<RawCacheV8Object>(CacheV8Object),
                GetMethodPair<RawGetCachedV8Object>(GetCachedV8Object),
            #endif

                GetMethodPair<RawGetAllCachedV8Objects>(GetAllCachedV8Objects),
                GetMethodPair<RawRemoveV8ObjectCacheEntry>(RemoveV8ObjectCacheEntry),
                GetMethodPair<RawCreateDebugAgent>(CreateDebugAgent),
                GetMethodPair<RawSendDebugMessage>(SendDebugMessage),
                GetMethodPair<RawDestroyDebugAgent>(DestroyDebugAgent),
                GetMethodPair<RawGetMaxScriptCacheSize>(GetMaxScriptCacheSize),
                GetMethodPair<RawGetMaxModuleCacheSize>(GetMaxModuleCacheSize),

            #if NET5_0_OR_GREATER
                (IntPtr.Zero, AddRefHostObjectFastMethodPtr),
                (IntPtr.Zero, ReleaseHostObjectFastMethodPtr),
                (IntPtr.Zero, GetHostObjectInvocabilityFastMethodPtr),
            #else
                GetMethodPair<RawAddRefHostObject>(AddRefHostObject),
                GetMethodPair<RawReleaseHostObject>(ReleaseHostObject),
                GetMethodPair<RawGetHostObjectInvocability>(GetHostObjectInvocability),
            #endif

            #if NET5_0_OR_GREATER
                (IntPtr.Zero, GetHostObjectNamedPropertyFastMethodPtr),
                (IntPtr.Zero, GetHostObjectNamedPropertyWithCacheabilityFastMethodPtr),
                (IntPtr.Zero, SetHostObjectNamedPropertyFastMethodPtr),
            #else
                GetMethodPair<RawGetHostObjectNamedProperty>(GetHostObjectNamedProperty),
                GetMethodPair<RawGetHostObjectNamedPropertyWithCacheability>(GetHostObjectNamedPropertyWithCacheability),
                GetMethodPair<RawSetHostObjectNamedProperty>(SetHostObjectNamedProperty),
            #endif

                GetMethodPair<RawDeleteHostObjectNamedProperty>(DeleteHostObjectNamedProperty),
                GetMethodPair<RawGetHostObjectPropertyNames>(GetHostObjectPropertyNames),

            #if NET5_0_OR_GREATER
                (IntPtr.Zero, GetHostObjectIndexedPropertyFastMethodPtr),
                (IntPtr.Zero, SetHostObjectIndexedPropertyFastMethodPtr),
            #else
                GetMethodPair<RawGetHostObjectIndexedProperty>(GetHostObjectIndexedProperty),
                GetMethodPair<RawSetHostObjectIndexedProperty>(SetHostObjectIndexedProperty),
            #endif

                GetMethodPair<RawDeleteHostObjectIndexedProperty>(DeleteHostObjectIndexedProperty),
                GetMethodPair<RawGetHostObjectPropertyIndices>(GetHostObjectPropertyIndices),

            #if NET5_0_OR_GREATER
                (IntPtr.Zero, InvokeHostObjectFastMethodPtr),
                (IntPtr.Zero, InvokeHostObjectMethodFastMethodPtr),
            #else
                GetMethodPair<RawInvokeHostObject>(InvokeHostObject),
                GetMethodPair<RawInvokeHostObjectMethod>(InvokeHostObjectMethod),
            #endif

                GetMethodPair<RawGetHostObjectEnumerator>(GetHostObjectEnumerator),
                GetMethodPair<RawGetHostObjectAsyncEnumerator>(GetHostObjectAsyncEnumerator),
                GetMethodPair<RawQueueNativeCallback>(QueueNativeCallback),
                GetMethodPair<RawCreateNativeCallbackTimer>(CreateNativeCallbackTimer),
                GetMethodPair<RawChangeNativeCallbackTimer>(ChangeNativeCallbackTimer),
                GetMethodPair<RawDestroyNativeCallbackTimer>(DestroyNativeCallbackTimer),
                GetMethodPair<RawLoadModule>(LoadModule),
                GetMethodPair<RawCreateModuleContext>(CreateModuleContext),
                GetMethodPair<RawWriteBytesToStream>(WriteBytesToStream),
                GetMethodPair<RawGetGlobalFlags>(GetGlobalFlags)
            };

            int methodCount = methodPairs.Length;
            IntPtr pDelegatePtrs = Marshal.AllocCoTaskMem(methodCount * IntPtr.Size);
            IntPtr pFunctionPtrs = Marshal.AllocCoTaskMem(methodCount * IntPtr.Size);

            for (var index = 0; index < methodCount; index++)
            {
                var (pDelegate, pFunction) = methodPairs[index];
                Marshal.WriteIntPtr(pDelegatePtrs, index * IntPtr.Size, pDelegate);
                Marshal.WriteIntPtr(pFunctionPtrs, index * IntPtr.Size, pFunction);
            }

            return (pDelegatePtrs, pFunctionPtrs, methodCount);
        }

        private static void DestroyMethodTable()
        {
            Debug.Assert(MethodCount > 0);

            for (var index = 0; index < MethodCount; index++)
            {
                var pDelegate = Marshal.ReadIntPtr(DelegateTable, index * IntPtr.Size);
                if (pDelegate != IntPtr.Zero)
                {
                    V8ProxyHelpers.ReleaseHostObject(pDelegate);
                }
            }

            Marshal.FreeCoTaskMem(DelegateTable);
            Marshal.FreeCoTaskMem(MethodTable);
        }

        private static (IntPtr, IntPtr) GetMethodPair<T>(T del)
        {
            return (V8ProxyHelpers.AddRefHostObject(del), Marshal.GetFunctionPointerForDelegate<T>(del));
        }

        #endregion

        #region method table implementation

        private static void ScheduleForwardingException(V8ValuePtr pException)
        {
            Debug.Assert(ScheduledException == null);

            var exception = V8ProxyHelpers.MarshalExceptionToHost(V8Value.Get(pException));
            if (exception is ScriptEngineException scriptEngineException)
            {
                ScheduledException = new ScriptEngineException(
                    scriptEngineException.EngineName,
                    scriptEngineException.Message,
                    scriptEngineException.ErrorDetails,
                    scriptEngineException.HResult,
                    scriptEngineException.IsFatal,
                    scriptEngineException.ExecutionStarted,
                    scriptEngineException.ScriptExceptionAsObject,
                    scriptEngineException);
            }
            else if (exception is ScriptInterruptedException scriptInterruptedException)
            {
                ScheduledException = new ScriptInterruptedException(
                    scriptInterruptedException.EngineName,
                    scriptInterruptedException.Message,
                    scriptInterruptedException.ErrorDetails,
                    scriptInterruptedException.HResult,
                    scriptInterruptedException.IsFatal,
                    scriptInterruptedException.ExecutionStarted,
                    scriptInterruptedException.ScriptExceptionAsObject,
                    scriptInterruptedException);
            }
            else
            {
                ScheduledException = exception;
            }
        }

        private static void ScheduleInvalidOperationException(StdStringPtr pMessage)
        {
            Debug.Assert(ScheduledException == null);
            ScheduledException = new InvalidOperationException(StdString.GetValue(pMessage));
        }

        private static void ScheduleScriptEngineException(
            StdStringPtr pEngineName,
            StdStringPtr pMessage,
            StdStringPtr pStackTrace,
            sbyte isFatal,
            sbyte executionStarted,
            V8ValuePtr pScriptException,
            V8ValuePtr pInnerException)
        {
            Debug.Assert(ScheduledException == null);
            var scriptException = ScriptEngine.Current?.MarshalToHost(V8Value.Get(pScriptException), false);
            var innerException = V8ProxyHelpers.MarshalExceptionToHost(V8Value.Get(pInnerException));
            ScheduledException = new ScriptEngineException(
                StdString.GetValue(pEngineName),
                StdString.GetValue(pMessage),
                StdString.GetValue(pStackTrace),
                0,
                isFatal.ToBool(),
                executionStarted.ToBool(),
                scriptException,
                innerException);
        }

        private static void ScheduleScriptInterruptedException(
            StdStringPtr pEngineName,
            StdStringPtr pMessage,
            StdStringPtr pStackTrace,
            sbyte isFatal,
            sbyte executionStarted,
            V8ValuePtr pScriptException,
            V8ValuePtr pInnerException)
        {
            Debug.Assert(ScheduledException == null);
            var scriptException = ScriptEngine.Current?.MarshalToHost(V8Value.Get(pScriptException), false);
            var innerException = V8ProxyHelpers.MarshalExceptionToHost(V8Value.Get(pInnerException));
            ScheduledException = new ScriptInterruptedException(
                StdString.GetValue(pEngineName),
                StdString.GetValue(pMessage),
                StdString.GetValue(pStackTrace),
                0,
                isFatal.ToBool(),
                executionStarted.ToBool(),
                scriptException,
                innerException);
        }

        private static void InvokeHostAction(IntPtr pAction)
        {
            try
            {
                V8ProxyHelpers.GetHostObject<Action>(pAction)();
            }
            catch (Exception exception)
            {
                ScheduleHostException(exception);
            }
        }

        private static void ProcessArrayBufferOrViewData(IntPtr pData, IntPtr pAction)
        {
            try
            {
                V8ProxyHelpers.GetHostObject<Action<IntPtr>>(pAction)(pData);
            }
            catch (Exception exception)
            {
                ScheduleHostException(exception);
            }
        }

        private static void ProcessCpuProfile(V8CpuProfilePtr pProfile, IntPtr pAction)
        {
            try
            {
                V8ProxyHelpers.GetHostObject<Action<V8CpuProfilePtr>>(pAction)(pProfile);
            }
            catch (Exception exception)
            {
                ScheduleHostException(exception);
            }
        }

        private static IntPtr CreateV8ObjectCache()
        {
            return V8ProxyHelpers.AddRefHostObject(new Dictionary<object, IntPtr>());
        }

        private static void CacheV8Object(IntPtr pCache, IntPtr pObject, IntPtr pV8Object)
        {
            V8ProxyHelpers.GetHostObject<Dictionary<object, IntPtr>>(pCache)
                .Add(V8ProxyHelpers.GetHostObject(pObject), pV8Object);
        }

        private static IntPtr GetCachedV8Object(IntPtr pCache, IntPtr pObject)
        {
            return V8ProxyHelpers.GetHostObject<Dictionary<object, IntPtr>>(pCache)
                .TryGetValue(V8ProxyHelpers.GetHostObject(pObject), out IntPtr pV8Object) ? pV8Object : IntPtr.Zero;
        }

        private static void GetAllCachedV8Objects(IntPtr pCache, StdPtrArrayPtr pV8ObjectPtrs)
        {
            var cache = V8ProxyHelpers.GetHostObject<Dictionary<object, IntPtr>>(pCache);
            StdPtrArray.CopyFromArray(pV8ObjectPtrs, cache.Values.ToArray());
        }

        private static sbyte RemoveV8ObjectCacheEntry(IntPtr pCache, IntPtr pObject)
        {
            return V8ProxyHelpers.GetHostObject<Dictionary<object, IntPtr>>(pCache).Remove(V8ProxyHelpers.GetHostObject(pObject)).ToSbyte();
        }

        private static IntPtr CreateDebugAgent(StdStringPtr pName, StdStringPtr pVersion, int port, sbyte remote, V8DebugCallbackHandle hCallback)
        {
            return V8ProxyHelpers.AddRefHostObject(new V8DebugAgent(
                StdString.GetValue(pName), 
                StdString.GetValue(pVersion),
                port, 
                remote.ToBool(), 
                new V8DebugListener(hCallback)));
        }

        private static void SendDebugMessage(IntPtr pAgent, StdStringPtr pContent)
        {
            V8ProxyHelpers.GetHostObject<V8DebugAgent>(pAgent).SendMessage(StdString.GetValue(pContent));
        }

        private static void DestroyDebugAgent(IntPtr pAgent)
        {
            V8ProxyHelpers.GetHostObject<V8DebugAgent>(pAgent).Dispose();
            V8ProxyHelpers.ReleaseHostObject(pAgent);
        }

        private static uint GetMaxScriptCacheSize()
        {
            return GetMaxCacheSizeForCategory(DocumentCategory.Script);
        }

        private static uint GetMaxModuleCacheSize()
        {
            return GetMaxCacheSizeForCategory(ModuleCategory.Standard);
        }

        private static IntPtr AddRefHostObject(IntPtr pObject)
        {
            return V8ProxyHelpers.AddRefHostObject(pObject);
        }

        private static void ReleaseHostObject(IntPtr pObject)
        {
            V8ProxyHelpers.ReleaseHostObject(pObject);
        }

        private static Invocability GetHostObjectInvocability(IntPtr pObject)
        {
            try
            {
                return V8ProxyHelpers.GetHostObjectInvocability(pObject);
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
                return default;
            }
        }

        private static void GetHostObjectNamedProperty(IntPtr pObject, StdStringPtr pName, V8ValuePtr pValue)
        {
            try
            {
                V8Value.Set(pValue, V8ProxyHelpers.GetHostObjectProperty(pObject, StdString.GetSpan(pName)));
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
            }
        }

        private static void GetHostObjectNamedPropertyWithCacheability(IntPtr pObject, StdStringPtr pName, V8ValuePtr pValue, out bool isCacheable)
        {
            try
            {
                V8Value.Set(pValue, V8ProxyHelpers.GetHostObjectProperty(pObject, StdString.GetSpan(pName), out isCacheable));
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
                isCacheable = false;
            }
        }

        private static void SetHostObjectNamedProperty(IntPtr pObject, StdStringPtr pName, V8ValuePtr pValue)
        {
            try
            {
                V8ProxyHelpers.SetHostObjectProperty(pObject, StdString.GetSpan(pName), V8Value.Get(pValue));
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
            }
        }

        private static sbyte DeleteHostObjectNamedProperty(IntPtr pObject, StdStringPtr pName)
        {
            try
            {
                return V8ProxyHelpers.DeleteHostObjectProperty(pObject, StdString.GetSpan(pName)).ToSbyte();
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
                return default;
            }
        }

        private static void GetHostObjectPropertyNames(IntPtr pObject, StdStringArrayPtr pNames)
        {
            string[] names;
            try
            {
                names = V8ProxyHelpers.GetHostObjectPropertyNames(pObject);
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
                return;
            }

            StdStringArray.CopyFromArray(pNames, names);
        }

        private static void GetHostObjectIndexedProperty(IntPtr pObject, int index, V8ValuePtr pValue)
        {
            try
            {
                V8Value.Set(pValue, V8ProxyHelpers.GetHostObjectProperty(pObject, index));
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
            }
        }

        private static void SetHostObjectIndexedProperty(IntPtr pObject, int index, V8ValuePtr pValue)
        {
            try
            {
                V8ProxyHelpers.SetHostObjectProperty(pObject, index, V8Value.Get(pValue));
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
            }
        }

        private static sbyte DeleteHostObjectIndexedProperty(IntPtr pObject, int index)
        {
            try
            {
                return V8ProxyHelpers.DeleteHostObjectProperty(pObject, index).ToSbyte();
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
                return default;
            }
        }

        private static void GetHostObjectPropertyIndices(IntPtr pObject, StdInt32ArrayPtr pIndices)
        {
            int[] indices;
            try
            {
                indices = V8ProxyHelpers.GetHostObjectPropertyIndices(pObject);
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
                return;
            }

            StdInt32Array.CopyFromArray(pIndices, indices);
        }

        private static void InvokeHostObject(IntPtr pObject, bool asConstructor, StdV8ValueArrayPtr pArgs, V8ValuePtr pResult)
        {
            try
            {
                V8Value.Set(pResult, V8ProxyHelpers.InvokeHostObject(pObject, asConstructor, StdV8ValueArray.ToArray(pArgs)));
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
            }
        }

        private static void InvokeHostObjectMethod(IntPtr pObject, StdStringPtr pName, StdV8ValueArrayPtr pArgs, V8ValuePtr pResult)
        {
            try
            {
                V8Value.Set(pResult, V8ProxyHelpers.InvokeHostObjectMethod(pObject, StdString.GetSpan(pName), StdV8ValueArray.ToArray(pArgs)));
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
            }
        }

        private static void GetHostObjectEnumerator(IntPtr pObject, V8ValuePtr pResult)
        {
            try
            {
                V8Value.Set(pResult, V8ProxyHelpers.GetHostObjectEnumerator(pObject));
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
            }
        }

        private static void GetHostObjectAsyncEnumerator(IntPtr pObject, V8ValuePtr pResult)
        {
            try
            {
                V8Value.Set(pResult, V8ProxyHelpers.GetHostObjectAsyncEnumerator(pObject));
            }
            catch (Exception exception)
            {
                ScheduleHostException(pObject, exception);
            }
        }

        private static void QueueNativeCallback(NativeCallbackHandle hCallback)
        {
            MiscHelpers.QueueNativeCallback(new NativeCallback(hCallback));
        }

        private static IntPtr CreateNativeCallbackTimer(int dueTime, int period, NativeCallbackHandle hCallback)
        {
            return V8ProxyHelpers.AddRefHostObject(new NativeCallbackTimer(dueTime, period, new NativeCallback(hCallback)));
        }

        private static sbyte ChangeNativeCallbackTimer(IntPtr pTimer, int dueTime, int period)
        {
            return V8ProxyHelpers.GetHostObject<NativeCallbackTimer>(pTimer).Change(dueTime, period).ToSbyte();
        }

        private static void DestroyNativeCallbackTimer(IntPtr pTimer)
        {
            V8ProxyHelpers.GetHostObject<NativeCallbackTimer>(pTimer).Dispose();
            V8ProxyHelpers.ReleaseHostObject(pTimer);
        }

        private static void LoadModule(
            IntPtr pSourceDocumentInfo, 
            StdStringPtr pSpecifier, 
            StdStringPtr pResourceName, 
            StdStringPtr pSourceMapUrl, 
            ulong* uniqueId,
            sbyte* isModule,
            StdStringPtr pCode, 
            IntPtr* pDocumentInfo)
        {
            string code;
            UniqueDocumentInfo documentInfo;

            try
            {
                code = V8ProxyHelpers.LoadModule(
                    pSourceDocumentInfo, 
                    StdString.GetValue(pSpecifier), 
                    ModuleCategory.Standard, 
                    out documentInfo);
            }
            catch (Exception exception)
            {
                ScheduleHostException(exception);
                *uniqueId = default;
                *isModule = default;
                *pDocumentInfo = default;
                return;
            }

            StdString.SetValue(pResourceName, MiscHelpers.GetUrlOrPath(documentInfo.Uri, documentInfo.UniqueName));
            StdString.SetValue(pSourceMapUrl, MiscHelpers.GetUrlOrPath(documentInfo.SourceMapUri, string.Empty));
            *uniqueId = documentInfo.UniqueId;
            *isModule = (documentInfo.Category == ModuleCategory.Standard).ToSbyte();
            StdString.SetValue(pCode, code);
            *pDocumentInfo = V8ProxyHelpers.AddRefHostObject(documentInfo);
        }

        private static void CreateModuleContext(IntPtr pDocumentInfo, StdStringArrayPtr pNames, StdV8ValueArrayPtr pValues)
        {
            IDictionary<string, object> context;
            try
            {
                context = V8ProxyHelpers.CreateModuleContext(pDocumentInfo);
            }
            catch (Exception exception)
            {
                ScheduleHostException(exception);
                return;
            }

            if (context == null)
            {
                StdStringArray.SetElementCount(pNames, 0);
                StdV8ValueArray.SetElementCount(pValues, 0);
            }
            else
            {
                StdStringArray.CopyFromArray(pNames, context.Keys.ToArray());
                StdV8ValueArray.CopyFromArray(pValues, context.Values.ToArray());
            }
        }

        private static void WriteBytesToStream(IntPtr pStream, IntPtr pBytes, int count)
        {
            try
            {
                Span<byte> span = new((void*)pBytes, count);
                V8ProxyHelpers.GetHostObject<Stream>(pStream).Write(span);
            }
            catch (Exception exception)
            {
                ScheduleHostException(exception);
            }
        }

        private static V8GlobalFlags GetGlobalFlags()
        {
            return V8Settings.GlobalFlags;
        }

        #endregion
    }
}
