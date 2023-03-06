// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.ClearScript.Util;

namespace Microsoft.ClearScript.V8.SplitProxy
{
    internal static unsafe partial class V8SplitProxyNative
    {
        #region V8SplitProxyNative implementation

        #region initialization

        public static IntPtr V8SplitProxyManaged_SetMethodTable(IntPtr pMethodTable)
        {
            return Imports.V8SplitProxyManaged_SetMethodTable(pMethodTable);
        }

        public static ReadOnlySpan<char> V8SplitProxyNative_GetVersion()
        {
            return MemoryMarshal.CreateReadOnlySpanFromNullTerminated((char*)Imports.V8SplitProxyNative_GetVersion());
        }

        public static void V8Environment_InitializeICU(IntPtr pICUData, uint size)
        {
            Imports.V8Environment_InitializeICU(pICUData, size);
        }

        #endregion

        #region StdString methods

        public static StdStringPtr StdString_New(ReadOnlySpan<char> value)
        {
            fixed (char* ptr = value)
            {
                return Imports.StdString_New(ptr, value.Length);
            }
        }

        public static ReadOnlySpan<char> StdString_GetValue(StdStringPtr pString)
        {
            int length;
            var pValue = Imports.StdString_GetValue(pString, &length);
            return new ReadOnlySpan<char>((void*)pValue, length);
        }

        public static void StdString_SetValue(StdStringPtr pString, ReadOnlySpan<char> value)
        {
            fixed (char* ptr = value)
            {
                Imports.StdString_SetValue(pString, ptr, value.Length);
            }
        }

        public static void StdString_Delete(StdStringPtr pString)
        {
            Imports.StdString_Delete(pString);
        }

        #endregion

        #region StdStringArray methods

        public static StdStringArrayPtr StdStringArray_New(int elementCount)
        {
            return Imports.StdStringArray_New(elementCount);
        }

        public static int StdStringArray_GetElementCount(StdStringArrayPtr pArray)
        {
            return Imports.StdStringArray_GetElementCount(pArray);
        }

        public static void StdStringArray_SetElementCount(StdStringArrayPtr pArray, int elementCount)
        {
            Imports.StdStringArray_SetElementCount(pArray, elementCount);
        }

        public static ReadOnlySpan<char> StdStringArray_GetElement(StdStringArrayPtr pArray, int index)
        {
            int length;
            var pValue = Imports.StdStringArray_GetElement(pArray, index, &length);
            return new ReadOnlySpan<char>((void*)pValue, length);
        }

        public static void StdStringArray_SetElement(StdStringArrayPtr pArray, int index, ReadOnlySpan<char> value)
        {
            fixed (char* ptr = value)
            {
                Imports.StdStringArray_SetElement(pArray, index, ptr, value.Length);
            }
        }

        public static void StdStringArray_Delete(StdStringArrayPtr pArray)
        {
            Imports.StdStringArray_Delete(pArray);
        }

        #endregion

        #region StdByteArray methods

        public static StdByteArrayPtr StdByteArray_New(int elementCount)
        {
            return Imports.StdByteArray_New(elementCount);
        }

        public static int StdByteArray_GetElementCount(StdByteArrayPtr pArray)
        {
            return Imports.StdByteArray_GetElementCount(pArray);
        }

        public static void StdByteArray_SetElementCount(StdByteArrayPtr pArray, int elementCount)
        {
            Imports.StdByteArray_SetElementCount(pArray, elementCount);
        }

        public static IntPtr StdByteArray_GetData(StdByteArrayPtr pArray)
        {
            return Imports.StdByteArray_GetData(pArray);
        }

        public static void StdByteArray_Delete(StdByteArrayPtr pArray)
        {
            Imports.StdByteArray_Delete(pArray);
        }

        #endregion

        #region StdInt32Array methods

        public static StdInt32ArrayPtr StdInt32Array_New(int elementCount)
        {
            return Imports.StdInt32Array_New(elementCount);
        }

        public static int StdInt32Array_GetElementCount(StdInt32ArrayPtr pArray)
        {
            return Imports.StdInt32Array_GetElementCount(pArray);
        }

        public static void StdInt32Array_SetElementCount(StdInt32ArrayPtr pArray, int elementCount)
        {
            Imports.StdInt32Array_SetElementCount(pArray, elementCount);
        }

        public static IntPtr StdInt32Array_GetData(StdInt32ArrayPtr pArray)
        {
            return Imports.StdInt32Array_GetData(pArray);
        }

        public static void StdInt32Array_Delete(StdInt32ArrayPtr pArray)
        {
            Imports.StdInt32Array_Delete(pArray);
        }

        #endregion

        #region StdUInt32Array methods

        public static StdUInt32ArrayPtr StdUInt32Array_New(int elementCount)
        {
            return Imports.StdUInt32Array_New(elementCount);
        }

        public static int StdUInt32Array_GetElementCount(StdUInt32ArrayPtr pArray)
        {
            return Imports.StdUInt32Array_GetElementCount(pArray);
        }

        public static void StdUInt32Array_SetElementCount(StdUInt32ArrayPtr pArray, int elementCount)
        {
            Imports.StdUInt32Array_SetElementCount(pArray, elementCount);
        }

        public static IntPtr StdUInt32Array_GetData(StdUInt32ArrayPtr pArray)
        {
            return Imports.StdUInt32Array_GetData(pArray);
        }

        public static void StdUInt32Array_Delete(StdUInt32ArrayPtr pArray)
        {
            Imports.StdUInt32Array_Delete(pArray);
        }

        #endregion

        #region StdUInt64Array methods

        public static StdUInt64ArrayPtr StdUInt64Array_New(int elementCount)
        {
            return Imports.StdUInt64Array_New(elementCount);
        }

        public static int StdUInt64Array_GetElementCount(StdUInt64ArrayPtr pArray)
        {
            return Imports.StdUInt64Array_GetElementCount(pArray);
        }

        public static void StdUInt64Array_SetElementCount(StdUInt64ArrayPtr pArray, int elementCount)
        {
            Imports.StdUInt64Array_SetElementCount(pArray, elementCount);
        }

        public static IntPtr StdUInt64Array_GetData(StdUInt64ArrayPtr pArray)
        {
            return Imports.StdUInt64Array_GetData(pArray);
        }

        public static void StdUInt64Array_Delete(StdUInt64ArrayPtr pArray)
        {
            Imports.StdUInt64Array_Delete(pArray);
        }

        #endregion

        #region StdPtrArray methods

        public static StdPtrArrayPtr StdPtrArray_New(int elementCount)
        {
            return Imports.StdPtrArray_New(elementCount);
        }

        public static int StdPtrArray_GetElementCount(StdPtrArrayPtr pArray)
        {
            return Imports.StdPtrArray_GetElementCount(pArray);
        }

        public static void StdPtrArray_SetElementCount(StdPtrArrayPtr pArray, int elementCount)
        {
            Imports.StdPtrArray_SetElementCount(pArray, elementCount);
        }

        public static IntPtr StdPtrArray_GetData(StdPtrArrayPtr pArray)
        {
            return Imports.StdPtrArray_GetData(pArray);
        }

        public static void StdPtrArray_Delete(StdPtrArrayPtr pArray)
        {
            Imports.StdPtrArray_Delete(pArray);
        }

        #endregion

        #region StdV8ValueArray methods

        public static StdV8ValueArrayPtr StdV8ValueArray_New(int elementCount)
        {
            return Imports.StdV8ValueArray_New(elementCount);
        }

        public static int StdV8ValueArray_GetElementCount(StdV8ValueArrayPtr pArray)
        {
            return Imports.StdV8ValueArray_GetElementCount(pArray);
        }

        public static void StdV8ValueArray_SetElementCount(StdV8ValueArrayPtr pArray, int elementCount)
        {
            Imports.StdV8ValueArray_SetElementCount(pArray, elementCount);
        }

        public static V8ValuePtr StdV8ValueArray_GetData(StdV8ValueArrayPtr pArray)
        {
            return Imports.StdV8ValueArray_GetData(pArray);
        }

        public static void StdV8ValueArray_Delete(StdV8ValueArrayPtr pArray)
        {
            Imports.StdV8ValueArray_Delete(pArray);
        }

        #endregion

        #region V8Value methods

        public static V8ValuePtr V8Value_New()
        {
            return Imports.V8Value_New();
        }

        public static void V8Value_SetNonexistent(V8ValuePtr pV8Value)
        {
            Imports.V8Value_SetNonexistent(pV8Value);
        }

        public static void V8Value_SetUndefined(V8ValuePtr pV8Value)
        {
            Imports.V8Value_SetUndefined(pV8Value);
        }

        public static void V8Value_SetNull(V8ValuePtr pV8Value)
        {
            Imports.V8Value_SetNull(pV8Value);
        }

        public static void V8Value_SetBoolean(V8ValuePtr pV8Value, bool value)
        {
            Imports.V8Value_SetBoolean(pV8Value, value.ToSbyte());
        }

        public static void V8Value_SetNumber(V8ValuePtr pV8Value, double value)
        {
            Imports.V8Value_SetNumber(pV8Value, value);
        }

        public static void V8Value_SetInt32(V8ValuePtr pV8Value, int value)
        {
            Imports.V8Value_SetInt32(pV8Value, value);
        }

        public static void V8Value_SetUInt32(V8ValuePtr pV8Value, uint value)
        {
            Imports.V8Value_SetUInt32(pV8Value, value);
        }

        public static void V8Value_SetString(V8ValuePtr pV8Value, ReadOnlySpan<char> value)
        {
            fixed (char* ptr = value)
            {
                Imports.V8Value_SetString(pV8Value, ptr, value.Length);
            }
        }

        public static void V8Value_SetDateTime(V8ValuePtr pV8Value, double value)
        {
            Imports.V8Value_SetDateTime(pV8Value, value);
        }

        public static void V8Value_SetBigInt(V8ValuePtr pV8Value, int signBit, ReadOnlySpan<byte> bytes)
        {
            fixed (byte* ptr = bytes)
            {
                Imports.V8Value_SetBigInt(pV8Value, signBit, ptr, bytes.Length);
            }
        }

        public static void V8Value_SetV8Object(
            V8ValuePtr pV8Value, V8ObjectHandle hObject, V8ValueSubtype subtype, V8ValueFlags flags)
        {
            Imports.V8Value_SetV8Object(pV8Value, hObject, subtype, flags);
        }

        public static void V8Value_SetHostObject(V8ValuePtr pV8Value, IntPtr pObject)
        {
            Imports.V8Value_SetHostObject(pV8Value, pObject);
        }

        public static V8ValueType V8Value_Decode(
            V8ValuePtr pV8Value,
            out int intValue,
            out uint uintValue,
            out double doubleValue,
            out IntPtr ptrOrHandle)
        {
            return Imports.V8Value_Decode(pV8Value, out intValue, out uintValue, out doubleValue, out ptrOrHandle);
        }

        public static void V8Value_Delete(V8ValuePtr pV8Value)
        {
            Imports.V8Value_Delete(pV8Value);
        }

        #endregion

        #region V8CpuProfile methods

        public static void V8CpuProfile_GetInfo(
            V8CpuProfilePtr pProfile,
            V8Entity hEntity,
            out string name,
            out ulong startTimestamp,
            out ulong endTimestamp,
            out int sampleCount,
            out V8CpuProfileImpl.NodePtr pRootNode)
        {
            using (var nameScope = StdString.CreateScope())
            {
                Imports.V8CpuProfile_GetInfo(
                    pProfile,
                    hEntity,
                    nameScope.Value,
                    out startTimestamp,
                    out endTimestamp,
                    out sampleCount,
                    out pRootNode);
                name = StdString.GetValue(nameScope.Value);
            }
        }

        public static bool V8CpuProfile_GetSample(V8CpuProfilePtr pProfile, int index, out ulong nodeId, out ulong timestamp)
        {
            return Imports.V8CpuProfile_GetSample(pProfile, index, out nodeId, out timestamp);
        }

        public static void V8CpuProfileNode_GetInfo(
            V8CpuProfileImpl.NodePtr pNode,
            V8Entity hEntity,
            out ulong nodeId,
            out long scriptId,
            out string scriptName,
            out string functionName,
            out string bailoutReason,
            out long lineNumber,
            out long columnNumber,
            out ulong hitCount,
            out uint hitLineCount,
            out int childCount)
        {
            using (var scriptNameScope = StdString.CreateScope())
            {
                using (var functionNameScope = StdString.CreateScope())
                {
                    using (var bailoutReasonScope = StdString.CreateScope())
                    {
                        Imports.V8CpuProfileNode_GetInfo(
                            pNode,
                            hEntity,
                            out nodeId,
                            out scriptId,
                            scriptNameScope.Value,
                            functionNameScope.Value,
                            bailoutReasonScope.Value,
                            out lineNumber,
                            out columnNumber,
                            out hitCount,
                            out hitLineCount,
                            out childCount);
                        scriptName = StdString.GetValue(scriptNameScope.Value);
                        functionName = StdString.GetValue(functionNameScope.Value);
                        bailoutReason = StdString.GetValue(bailoutReasonScope.Value);
                    }
                }
            }
        }

        public static bool V8CpuProfileNode_GetHitLines(
            V8CpuProfileImpl.NodePtr pNode, out int[] lineNumbers, out uint[] hitCounts)
        {
            using (var lineNumbersScope = StdInt32Array.CreateScope())
            {
                using (var hitCountsScope = StdUInt32Array.CreateScope())
                {
                    var result = Imports.V8CpuProfileNode_GetHitLines(pNode, lineNumbersScope.Value, hitCountsScope.Value);
                    lineNumbers = StdInt32Array.ToArray(lineNumbersScope.Value);
                    hitCounts = StdUInt32Array.ToArray(hitCountsScope.Value);
                    return result;
                }
            }
        }

        public static V8CpuProfileImpl.NodePtr V8CpuProfileNode_GetChildNode(V8CpuProfileImpl.NodePtr pNode, int index)
        {
            return Imports.V8CpuProfileNode_GetChildNode(pNode, index);
        }

        #endregion

        #region V8 isolate methods

        public static V8IsolateHandle V8Isolate_Create(
            ReadOnlySpan<char> name,
            int maxNewSpaceSize,
            int maxOldSpaceSize,
            double heapExpansionMultiplier,
            ulong maxArrayBufferAllocation,
            V8RuntimeFlags flags,
            int debugPort)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                return Imports.V8Isolate_Create(
                    nameScope.Value,
                    maxNewSpaceSize,
                    maxOldSpaceSize,
                    heapExpansionMultiplier,
                    maxArrayBufferAllocation,
                    flags,
                    debugPort);
            }
        }

        public static V8ContextHandle V8Isolate_CreateContext(
            V8IsolateHandle hIsolate, ReadOnlySpan<char> name, V8ScriptEngineFlags flags, int debugPort)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                return Imports.V8Isolate_CreateContext(hIsolate, nameScope.Value, flags, debugPort);
            }
        }

        public static UIntPtr V8Isolate_GetMaxHeapSize(V8IsolateHandle hIsolate)
        {
            return Imports.V8Isolate_GetMaxHeapSize(hIsolate);
        }

        public static void V8Isolate_SetMaxHeapSize(V8IsolateHandle hIsolate, UIntPtr size)
        {
            Imports.V8Isolate_SetMaxHeapSize(hIsolate, size);
        }

        public static double V8Isolate_GetHeapSizeSampleInterval(V8IsolateHandle hIsolate)
        {
            return Imports.V8Isolate_GetHeapSizeSampleInterval(hIsolate);
        }

        public static void V8Isolate_SetHeapSizeSampleInterval(V8IsolateHandle hIsolate, double milliseconds)
        {
            Imports.V8Isolate_SetHeapSizeSampleInterval(hIsolate, milliseconds);
        }

        public static UIntPtr V8Isolate_GetMaxStackUsage(V8IsolateHandle hIsolate)
        {
            return Imports.V8Isolate_GetMaxStackUsage(hIsolate);
        }

        public static void V8Isolate_SetMaxStackUsage(V8IsolateHandle hIsolate, UIntPtr size)
        {
            Imports.V8Isolate_SetMaxStackUsage(hIsolate, size);
        }

        public static void V8Isolate_AwaitDebuggerAndPause(V8IsolateHandle hIsolate)
        {
            Imports.V8Isolate_AwaitDebuggerAndPause(hIsolate);
        }

        public static void V8Isolate_CancelAwaitDebugger(V8IsolateHandle hIsolate)
        {
            Imports.V8Isolate_CancelAwaitDebugger(hIsolate);
        }

        public static V8ScriptHandle V8Isolate_Compile(
            V8IsolateHandle hIsolate,
            ReadOnlySpan<char> resourceName,
            ReadOnlySpan<char> sourceMapUrl,
            ulong uniqueId,
            bool isModule,
            IntPtr pDocumentInfo,
            ReadOnlySpan<char> code)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        return Imports.V8Isolate_Compile(
                            hIsolate,
                            resourceNameScope.Value,
                            sourceMapUrlScope.Value,
                            uniqueId,
                            isModule,
                            pDocumentInfo,
                            codeScope.Value);
                    }
                }
            }
        }

        public static V8ScriptHandle V8Isolate_CompileProducingCache(
            V8IsolateHandle hIsolate,
            ReadOnlySpan<char> resourceName,
            ReadOnlySpan<char> sourceMapUrl,
            ulong uniqueId,
            bool isModule,
            IntPtr pDocumentInfo,
            ReadOnlySpan<char> code,
            V8CacheKind cacheKind,
            out byte[] cacheBytes)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        using (var cacheBytesScope = StdByteArray.CreateScope())
                        {
                            var hScript = Imports.V8Isolate_CompileProducingCache(
                                hIsolate,
                                resourceNameScope.Value,
                                sourceMapUrlScope.Value,
                                uniqueId,
                                isModule,
                                pDocumentInfo,
                                codeScope.Value,
                                cacheKind,
                                cacheBytesScope.Value);
                            cacheBytes = StdByteArray.ToArray(cacheBytesScope.Value);
                            return hScript;
                        }
                    }
                }
            }
        }

        public static V8ScriptHandle V8Isolate_CompileConsumingCache(
            V8IsolateHandle hIsolate,
            ReadOnlySpan<char> resourceName,
            ReadOnlySpan<char> sourceMapUrl,
            ulong uniqueId,
            bool isModule,
            IntPtr pDocumentInfo,
            ReadOnlySpan<char> code,
            V8CacheKind cacheKind,
            ReadOnlySpan<byte> cacheBytes,
            out bool cacheAccepted)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        using (var cacheBytesScope = StdByteArray.CreateScope(cacheBytes))
                        {
                            return Imports.V8Isolate_CompileConsumingCache(
                                hIsolate,
                                resourceNameScope.Value,
                                sourceMapUrlScope.Value,
                                uniqueId,
                                isModule,
                                pDocumentInfo,
                                codeScope.Value,
                                cacheKind,
                                cacheBytesScope.Value,
                                out cacheAccepted);
                        }
                    }
                }
            }
        }

        public static bool V8Isolate_GetEnableInterruptPropagation(V8IsolateHandle hIsolate)
        {
            return Imports.V8Isolate_GetEnableInterruptPropagation(hIsolate);
        }

        public static void V8Isolate_SetEnableInterruptPropagation(V8IsolateHandle hIsolate, bool value)
        {
            Imports.V8Isolate_SetEnableInterruptPropagation(hIsolate, value);
        }

        public static bool V8Isolate_GetDisableHeapSizeViolationInterrupt(V8IsolateHandle hIsolate)
        {
            return Imports.V8Isolate_GetDisableHeapSizeViolationInterrupt(hIsolate);
        }

        public static void V8Isolate_SetDisableHeapSizeViolationInterrupt(V8IsolateHandle hIsolate, bool value)
        {
            Imports.V8Isolate_SetDisableHeapSizeViolationInterrupt(hIsolate, value);
        }

        public static void V8Isolate_GetHeapStatistics(
            V8IsolateHandle hIsolate,
            out ulong totalHeapSize,
            out ulong totalHeapSizeExecutable,
            out ulong totalPhysicalSize,
            out ulong totalAvailableSize,
            out ulong usedHeapSize,
            out ulong heapSizeLimit,
            out ulong totalExternalSize)
        {
            Imports.V8Isolate_GetHeapStatistics(
                hIsolate,
                out totalHeapSize,
                out totalHeapSizeExecutable,
                out totalPhysicalSize,
                out totalAvailableSize,
                out usedHeapSize,
                out heapSizeLimit,
                out totalExternalSize);
        }

        public static void V8Isolate_GetStatistics(
            V8IsolateHandle hIsolate,
            out ulong scriptCount,
            out ulong scriptCacheSize,
            out ulong moduleCount,
            out ulong[] postedTaskCounts,
            out ulong[] invokedTaskCounts)
        {
            using (var postedTaskCountsScope = StdUInt64Array.CreateScope())
            {
                using (var invokedTaskCountsScope = StdUInt64Array.CreateScope())
                {
                    Imports.V8Isolate_GetStatistics(
                        hIsolate,
                        out scriptCount,
                        out scriptCacheSize,
                        out moduleCount,
                        postedTaskCountsScope.Value,
                        invokedTaskCountsScope.Value);
                    postedTaskCounts = StdUInt64Array.ToArray(postedTaskCountsScope.Value);
                    invokedTaskCounts = StdUInt64Array.ToArray(invokedTaskCountsScope.Value);
                }
            }
        }

        public static void V8Isolate_CollectGarbage(V8IsolateHandle hIsolate, bool exhaustive)
        {
            Imports.V8Isolate_CollectGarbage(hIsolate, exhaustive);
        }

        public static bool V8Isolate_BeginCpuProfile(V8IsolateHandle hIsolate, ReadOnlySpan<char> name, bool recordSamples)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                return Imports.V8Isolate_BeginCpuProfile(hIsolate, nameScope.Value, recordSamples);
            }
        }

        public static void V8Isolate_EndCpuProfile(V8IsolateHandle hIsolate, ReadOnlySpan<char> name, IntPtr pAction)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                Imports.V8Isolate_EndCpuProfile(hIsolate, nameScope.Value, pAction);
            }
        }

        public static void V8Isolate_CollectCpuProfileSample(V8IsolateHandle hIsolate)
        {
            V8Isolate_CollectCpuProfileSample(hIsolate);
        }

        public static uint V8Isolate_GetCpuProfileSampleInterval(V8IsolateHandle hIsolate)
        {
            return Imports.V8Isolate_GetCpuProfileSampleInterval(hIsolate);
        }

        public static void V8Isolate_SetCpuProfileSampleInterval(V8IsolateHandle hIsolate, uint value)
        {
            Imports.V8Isolate_SetCpuProfileSampleInterval(hIsolate, value);
        }

        public static void V8Isolate_WriteHeapSnapshot(V8IsolateHandle hIsolate, IntPtr pStream)
        {
            Imports.V8Isolate_WriteHeapSnapshot(hIsolate, pStream);
        }

        #endregion

        #region V8 context methods

        public static UIntPtr V8Context_GetMaxIsolateHeapSize(V8ContextHandle hContext)
        {
            return Imports.V8Context_GetMaxIsolateHeapSize(hContext);
        }

        public static void V8Context_SetMaxIsolateHeapSize(V8ContextHandle hContext, UIntPtr size)
        {
            Imports.V8Context_SetMaxIsolateHeapSize(hContext, size);
        }

        public static double V8Context_GetIsolateHeapSizeSampleInterval(V8ContextHandle hContext)
        {
            return Imports.V8Context_GetIsolateHeapSizeSampleInterval(hContext);
        }

        public static void V8Context_SetIsolateHeapSizeSampleInterval(V8ContextHandle hContext, double milliseconds)
        {
            Imports.V8Context_SetIsolateHeapSizeSampleInterval(hContext, milliseconds);
        }

        public static UIntPtr V8Context_GetMaxIsolateStackUsage(V8ContextHandle hContext)
        {
            return Imports.V8Context_GetMaxIsolateStackUsage(hContext);
        }

        public static void V8Context_SetMaxIsolateStackUsage(V8ContextHandle hContext, UIntPtr size)
        {
            Imports.V8Context_SetMaxIsolateStackUsage(hContext, size);
        }

        public static void V8Context_InvokeWithLock(V8ContextHandle hContext, IntPtr pAction)
        {
            Imports.V8Context_InvokeWithLock(hContext, pAction);
        }

        public static object V8Context_GetRootItem(V8ContextHandle hContext)
        {
            using (var itemScope = V8Value.CreateScope())
            {
                Imports.V8Context_GetRootItem(hContext, itemScope.Value);
                return V8Value.Get(itemScope.Value);
            }
        }

        public static void V8Context_AddGlobalItem<T>(
            V8ContextHandle hContext, ReadOnlySpan<char> name, T value, bool globalMembers)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                using (var valueScope = V8Value.CreateScope(value))
                {
                    Imports.V8Context_AddGlobalItem(hContext, nameScope.Value, valueScope.Value, globalMembers);
                }
            }
        }

        public static void V8Context_AwaitDebuggerAndPause(V8ContextHandle hContext)
        {
            Imports.V8Context_AwaitDebuggerAndPause(hContext);
        }

        public static void V8Context_CancelAwaitDebugger(V8ContextHandle hContext)
        {
            Imports.V8Context_CancelAwaitDebugger(hContext);
        }

        public static object V8Context_ExecuteCode(
            V8ContextHandle hContext,
            ReadOnlySpan<char> resourceName,
            ReadOnlySpan<char> sourceMapUrl,
            ulong uniqueId,
            bool isModule,
            IntPtr pDocumentInfo,
            ReadOnlySpan<char> code,
            bool evaluate)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        using (var resultScope = V8Value.CreateScope())
                        {
                            Imports.V8Context_ExecuteCode(
                                hContext,
                                resourceNameScope.Value,
                                sourceMapUrlScope.Value,
                                uniqueId, isModule,
                                pDocumentInfo,
                                codeScope.Value,
                                evaluate,
                                resultScope.Value);
                            return V8Value.Get(resultScope.Value);
                        }
                    }
                }
            }
        }

        public static V8ScriptHandle V8Context_Compile(
            V8ContextHandle hContext,
            ReadOnlySpan<char> resourceName,
            ReadOnlySpan<char> sourceMapUrl,
            ulong uniqueId,
            bool isModule,
            IntPtr pDocumentInfo,
            ReadOnlySpan<char> code)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        return Imports.V8Context_Compile(
                            hContext,
                            resourceNameScope.Value,
                            sourceMapUrlScope.Value,
                            uniqueId,
                            isModule,
                            pDocumentInfo,
                            codeScope.Value);
                    }
                }
            }
        }

        public static V8ScriptHandle V8Context_CompileProducingCache(
            V8ContextHandle hContext,
            ReadOnlySpan<char> resourceName,
            ReadOnlySpan<char> sourceMapUrl,
            ulong uniqueId,
            bool isModule,
            IntPtr pDocumentInfo,
            ReadOnlySpan<char> code,
            V8CacheKind cacheKind,
            out byte[] cacheBytes)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        using (var cacheBytesScope = StdByteArray.CreateScope())
                        {
                            var hScript = Imports.V8Context_CompileProducingCache(
                                hContext,
                                resourceNameScope.Value,
                                sourceMapUrlScope.Value,
                                uniqueId,
                                isModule,
                                pDocumentInfo,
                                codeScope.Value,
                                cacheKind,
                                cacheBytesScope.Value);
                            cacheBytes = StdByteArray.ToArray(cacheBytesScope.Value);
                            return hScript;
                        }
                    }
                }
            }
        }

        public static V8ScriptHandle V8Context_CompileConsumingCache(
            V8ContextHandle hContext,
            ReadOnlySpan<char> resourceName,
            ReadOnlySpan<char> sourceMapUrl,
            ulong uniqueId,
            bool isModule,
            IntPtr pDocumentInfo,
            ReadOnlySpan<char> code,
            V8CacheKind cacheKind,
            ReadOnlySpan<byte> cacheBytes,
            out bool cacheAccepted)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        using (var cacheBytesScope = StdByteArray.CreateScope(cacheBytes))
                        {
                            return Imports.V8Context_CompileConsumingCache(
                                hContext,
                                resourceNameScope.Value,
                                sourceMapUrlScope.Value,
                                uniqueId,
                                isModule,
                                pDocumentInfo,
                                codeScope.Value,
                                cacheKind,
                                cacheBytesScope.Value,
                                out cacheAccepted);
                        }
                    }
                }
            }
        }

        public static object V8Context_ExecuteScript(V8ContextHandle hContext, V8ScriptHandle hScript, bool evaluate)
        {
            using (var resultScope = V8Value.CreateScope())
            {
                Imports.V8Context_ExecuteScript(hContext, hScript, evaluate, resultScope.Value);
                return V8Value.Get(resultScope.Value);
            }
        }

        public static void V8Context_Interrupt(V8ContextHandle hContext)
        {
            Imports.V8Context_Interrupt(hContext);
        }

        public static void V8Context_CancelInterrupt(V8ContextHandle hContext)
        {
            Imports.V8Context_CancelInterrupt(hContext);
        }

        public static bool V8Context_GetEnableIsolateInterruptPropagation(V8ContextHandle hContext)
        {
            return Imports.V8Context_GetEnableIsolateInterruptPropagation(hContext);
        }

        public static void V8Context_SetEnableIsolateInterruptPropagation(V8ContextHandle hContext, bool value)
        {
            Imports.V8Context_SetEnableIsolateInterruptPropagation(hContext, value);
        }

        public static bool V8Context_GetDisableIsolateHeapSizeViolationInterrupt(V8ContextHandle hContext)
        {
            return Imports.V8Context_GetDisableIsolateHeapSizeViolationInterrupt(hContext);
        }

        public static void V8Context_SetDisableIsolateHeapSizeViolationInterrupt(V8ContextHandle hContext, bool value)
        {
            Imports.V8Context_SetDisableIsolateHeapSizeViolationInterrupt(hContext, value);
        }

        public static void V8Context_GetIsolateHeapStatistics(
            V8ContextHandle hContext,
            out ulong totalHeapSize, 
            out ulong totalHeapSizeExecutable, 
            out ulong totalPhysicalSize, 
            out ulong totalAvailableSize, 
            out ulong usedHeapSize, 
            out ulong heapSizeLimit,
            out ulong totalExternalSize)
        {
            Imports.V8Context_GetIsolateHeapStatistics(
                hContext, 
                out totalHeapSize, 
                out totalHeapSizeExecutable, 
                out totalPhysicalSize,
                out totalAvailableSize,
                out usedHeapSize, 
                out heapSizeLimit,
                out totalExternalSize);
        }

        public static void V8Context_GetIsolateStatistics(
            V8ContextHandle hContext,
            out ulong scriptCount, 
            out ulong scriptCacheSize, 
            out ulong moduleCount, 
            out ulong[] postedTaskCounts,
            out ulong[] invokedTaskCounts)
        {
            using (var postedTaskCountsScope = StdUInt64Array.CreateScope())
            {
                using (var invokedTaskCountsScope = StdUInt64Array.CreateScope())
                {
                    Imports.V8Context_GetIsolateStatistics(
                        hContext, 
                        out scriptCount,
                        out scriptCacheSize,
                        out moduleCount, 
                        postedTaskCountsScope.Value,
                        invokedTaskCountsScope.Value);
                    postedTaskCounts = StdUInt64Array.ToArray(postedTaskCountsScope.Value);
                    invokedTaskCounts = StdUInt64Array.ToArray(invokedTaskCountsScope.Value);
                }
            }
        }

        public static void V8Context_GetStatistics(
            V8ContextHandle hContext, out ulong scriptCount, out ulong moduleCount, out ulong moduleCacheSize)
        {
            Imports.V8Context_GetStatistics(hContext, out scriptCount, out moduleCount, out moduleCacheSize);
        }

        public static void V8Context_CollectGarbage(V8ContextHandle hContext, bool exhaustive)
        {
            Imports.V8Context_CollectGarbage(hContext, exhaustive);
        }

        public static void V8Context_OnAccessSettingsChanged(V8ContextHandle hContext)
        {
            Imports.V8Context_OnAccessSettingsChanged(hContext);
        }

        public static bool V8Context_BeginCpuProfile(V8ContextHandle hContext, ReadOnlySpan<char> name, bool recordSamples)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                return Imports.V8Context_BeginCpuProfile(hContext, nameScope.Value, recordSamples);
            }
        }

        public static void V8Context_EndCpuProfile(V8ContextHandle hContext, ReadOnlySpan<char> name, IntPtr pAction)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                Imports.V8Context_EndCpuProfile(hContext, nameScope.Value, pAction);
            }
        }

        public static void V8Context_CollectCpuProfileSample(V8ContextHandle hContext)
        {
            Imports.V8Context_CollectCpuProfileSample(hContext);
        }

        public static uint V8Context_GetCpuProfileSampleInterval(V8ContextHandle hContext)
        {
            return Imports.V8Context_GetCpuProfileSampleInterval(hContext);
        }

        public static void V8Context_SetCpuProfileSampleInterval(V8ContextHandle hContext, uint value)
        {
            Imports.V8Context_SetCpuProfileSampleInterval(hContext, value);
        }

        public static void V8Context_WriteIsolateHeapSnapshot(V8ContextHandle hContext, IntPtr pStream)
        {
            Imports.V8Context_WriteIsolateHeapSnapshot(hContext, pStream);
        }

        #endregion

        #region V8 object methods

        public static object V8Object_GetNamedProperty(V8ObjectHandle hObject, ReadOnlySpan<char> name)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                using (var valueScope = V8Value.CreateScope())
                {
                    Imports.V8Object_GetNamedProperty(hObject, nameScope.Value, valueScope.Value);
                    return V8Value.Get(valueScope.Value);
                }
            }
        }

        public static void V8Object_SetNamedProperty(V8ObjectHandle hObject, ReadOnlySpan<char> name, object value)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                using (var valueScope = V8Value.CreateScope(value))
                {
                    Imports.V8Object_SetNamedProperty(hObject, nameScope.Value, valueScope.Value);
                }
            }
        }

        public static bool V8Object_DeleteNamedProperty(V8ObjectHandle hObject, ReadOnlySpan<char> name)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                return Imports.V8Object_DeleteNamedProperty(hObject, nameScope.Value);
            }
        }

        public static string[] V8Object_GetPropertyNames(V8ObjectHandle hObject)
        {
            using (var namesScope = StdStringArray.CreateScope())
            {
                Imports.V8Object_GetPropertyNames(hObject, namesScope.Value);
                return StdStringArray.ToArray(namesScope.Value);
            }
        }

        public static object V8Object_GetIndexedProperty(V8ObjectHandle hObject, int index)
        {
            using (var valueScope = V8Value.CreateScope())
            {
                Imports.V8Object_GetIndexedProperty(hObject, index, valueScope.Value);
                return V8Value.Get(valueScope.Value);
            }
        }

        public static void V8Object_SetIndexedProperty(V8ObjectHandle hObject, int index, object value)
        {
            using (var valueScope = V8Value.CreateScope(value))
            {
                Imports.V8Object_SetIndexedProperty(hObject, index, valueScope.Value);
            }
        }

        public static bool V8Object_DeleteIndexedProperty(V8ObjectHandle hObject, int index)
        {
            return Imports.V8Object_DeleteIndexedProperty(hObject, index);
        }

        public static int[] V8Object_GetPropertyIndices(V8ObjectHandle hObject)
        {
            using (var indicesScope = StdInt32Array.CreateScope())
            {
                Imports.V8Object_GetPropertyIndices(hObject, indicesScope.Value);
                return StdInt32Array.ToArray(indicesScope.Value);
            }
        }

        public static object V8Object_Invoke(V8ObjectHandle hObject, bool asConstructor, ReadOnlySpan<object> args)
        {
            using (var argsScope = StdV8ValueArray.CreateScope(args))
            {
                using (var resultScope = V8Value.CreateScope())
                {
                    Imports.V8Object_Invoke(hObject, asConstructor, argsScope.Value, resultScope.Value);
                    return V8Value.Get(resultScope.Value);
                }
            }
        }

        public static object V8Object_InvokeMethod(V8ObjectHandle hObject, ReadOnlySpan<char> name, ReadOnlySpan<object> args)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                using (var argsScope = StdV8ValueArray.CreateScope(args))
                {
                    using (var resultScope = V8Value.CreateScope())
                    {
                        Imports.V8Object_InvokeMethod(hObject, nameScope.Value, argsScope.Value, resultScope.Value);
                        return V8Value.Get(resultScope.Value);
                    }
                }
            }
        }

        public static void V8Object_GetArrayBufferOrViewInfo(
            V8ObjectHandle hObject, out V8Object arrayBuffer, out ulong offset, out ulong size, out ulong length)
        {
            using (var arrayBufferScope = V8Value.CreateScope())
            {
                Imports.V8Object_GetArrayBufferOrViewInfo(hObject, arrayBufferScope.Value, out offset, out size, out length);
                arrayBuffer = (V8Object)V8Value.Get(arrayBufferScope.Value);
            }
        }

        public static void V8Object_InvokeWithArrayBufferOrViewData(V8ObjectHandle hObject, IntPtr pAction)
        {
            Imports.V8Object_InvokeWithArrayBufferOrViewData(hObject, pAction);
        }

        #endregion

        #region V8 debug callback methods

        public static void V8DebugCallback_ConnectClient(V8DebugCallbackHandle hCallback)
        {
            Imports.V8DebugCallback_ConnectClient(hCallback);
        }

        public static void V8DebugCallback_SendCommand(V8DebugCallbackHandle hCallback, ReadOnlySpan<char> command)
        {
            using (var commandScope = StdString.CreateScope(command))
            {
                Imports.V8DebugCallback_SendCommand(hCallback, commandScope.Value);
            }
        }

        public static void V8DebugCallback_DisconnectClient(V8DebugCallbackHandle hCallback)
        {
            Imports.V8DebugCallback_DisconnectClient(hCallback);
        }

        #endregion

        #region native callback methods

        public static void NativeCallback_Invoke(NativeCallbackHandle hCallback)
        {
            Imports.NativeCallback_Invoke(hCallback);
        }

        #endregion

        #region V8 entity cleanup

        public static void V8Entity_Release(V8Entity hEntity)
        {
            Imports.V8Entity_Release(hEntity);
        }

        public static void V8Entity_DestroyHandle(V8Entity hEntity)
        {
            Imports.V8Entity_DestroyHandle(hEntity);
        }

        #endregion

        #region error handling

        public static void HostException_Schedule(ReadOnlySpan<char> message, object? exception)
        {
            using (var messageScope = StdString.CreateScope(message))
            {
                using (var exceptionScope = V8Value.CreateScope(exception))
                {
                    Imports.HostException_Schedule(messageScope.Value, exceptionScope.Value);
                }
            }
        }

        #endregion

        #region unit test support

        public static UIntPtr V8UnitTestSupport_GetTextDigest(ReadOnlySpan<char> value)
        {
            using (var valueScope = StdString.CreateScope(value))
            {
                return Imports.V8UnitTestSupport_GetTextDigest(valueScope.Value);
            }
        }

        public static void V8UnitTestSupport_GetStatistics(out ulong isolateCount, out ulong contextCount)
        {
            Imports.V8UnitTestSupport_GetStatistics(out isolateCount, out contextCount);
        }

        #endregion

        #endregion

        #region Imports holder

        public static unsafe partial class Imports
        {
            public const string DllName = "SharpV8.Native";

            public static event DllImportResolver? ResolveLibrary;

            static Imports()
            {
                NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), OnDllImport);
            }

            private static IntPtr OnDllImport(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
            {
                if (TryResolveLibrary(libraryName, assembly, searchPath, out IntPtr nativeLibrary))
                {
                    return nativeLibrary;
                }

                if (libraryName.Equals(DllName))
                {
                    GetRuntimeInfo(out string platform, out string architecture, out string extension);
                    string fileName = GetDllName(platform, architecture, extension);

                    if (NativeLibrary.TryLoad(fileName, assembly, searchPath, out nativeLibrary))
                    {
                        return nativeLibrary;
                    }
                }

                return IntPtr.Zero;
            }

            public static string GetDllName(
                string platform,
                string architecture,
                string extension)
            {
                string name = $"{DllName}.{platform}-{architecture}.{extension}";
                return name;
            }

            public static void GetRuntimeInfo(
                out string platform,
                out string architecture,
                out string extension)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    platform = "win";
                    extension = "dll";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    platform = "linux";
                    extension = "so";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    platform = "osx";
                    extension = "dylib";
                }
                else
                {
                    throw new PlatformNotSupportedException("Unsupported OS platform");
                }

                if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                {
                    architecture = "x64";
                }
                else if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
                {
                    architecture = "x86";
                }
                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                {
                    architecture = "arm";
                }
                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                {
                    architecture = "arm64";
                }
                else
                {
                    throw new PlatformNotSupportedException("Unsupported process architecture");
                }
            }

            public static bool TryResolveLibrary(
                string libraryName, Assembly assembly, DllImportSearchPath? searchPath, out IntPtr nativeLibrary)
            {
                DllImportResolver? resolveLibrary = ResolveLibrary;
                if (resolveLibrary != null)
                {
                    Delegate[] resolvers = resolveLibrary.GetInvocationList();
                    foreach (Delegate resolver in resolvers)
                    {
                        nativeLibrary = ((DllImportResolver)resolver).Invoke(libraryName, assembly, searchPath);
                        if (nativeLibrary != IntPtr.Zero)
                        {
                            return true;
                        }
                    }
                }

                nativeLibrary = IntPtr.Zero;
                return false;
            }

            #region native methods

            #region initialization

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern IntPtr V8SplitProxyManaged_SetMethodTable(
                IntPtr pMethodTable
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial IntPtr V8SplitProxyNative_GetVersion();

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Environment_InitializeICU(
                IntPtr pICUData,
                uint size
            );

            #endregion

            #region StdString methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial StdStringPtr StdString_New(
                char* value,
                int length
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern IntPtr StdString_GetValue(
                StdStringPtr pString,
                int* length
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdString_SetValue(
                StdStringPtr pString,
                char* value,
                int length
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdString_Delete(
                StdStringPtr pString
            );

            #endregion

            #region StdStringArray methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial StdStringArrayPtr StdStringArray_New(
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern int StdStringArray_GetElementCount(
                StdStringArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdStringArray_SetElementCount(
                StdStringArrayPtr pArray,
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern IntPtr StdStringArray_GetElement(
                StdStringArrayPtr pArray,
                int index,
                int* length
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdStringArray_SetElement(
                StdStringArrayPtr pArray,
                int index,
                char* value,
                int length
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdStringArray_Delete(
                StdStringArrayPtr pArray
            );

            #endregion

            #region StdByteArray methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial StdByteArrayPtr StdByteArray_New(
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern int StdByteArray_GetElementCount(
                StdByteArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdByteArray_SetElementCount(
                StdByteArrayPtr pArray,
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern IntPtr StdByteArray_GetData(
                StdByteArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdByteArray_Delete(
                StdByteArrayPtr pArray
            );

            #endregion

            #region StdInt32Array methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial StdInt32ArrayPtr StdInt32Array_New(
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern int StdInt32Array_GetElementCount(
                StdInt32ArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdInt32Array_SetElementCount(
                StdInt32ArrayPtr pArray,
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern IntPtr StdInt32Array_GetData(
                StdInt32ArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdInt32Array_Delete(
                StdInt32ArrayPtr pArray
            );

            #endregion

            #region StdUInt32Array methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial StdUInt32ArrayPtr StdUInt32Array_New(
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern int StdUInt32Array_GetElementCount(
                StdUInt32ArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdUInt32Array_SetElementCount(
                StdUInt32ArrayPtr pArray,
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern IntPtr StdUInt32Array_GetData(
                StdUInt32ArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdUInt32Array_Delete(
                StdUInt32ArrayPtr pArray
            );

            #endregion

            #region StdUInt64Array methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial StdUInt64ArrayPtr StdUInt64Array_New(
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern int StdUInt64Array_GetElementCount(
                StdUInt64ArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdUInt64Array_SetElementCount(
                StdUInt64ArrayPtr pArray,
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern IntPtr StdUInt64Array_GetData(
                StdUInt64ArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdUInt64Array_Delete(
                StdUInt64ArrayPtr pArray
            );

            #endregion

            #region StdPtrArray methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial StdPtrArrayPtr StdPtrArray_New(
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern int StdPtrArray_GetElementCount(
                StdPtrArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdPtrArray_SetElementCount(
                StdPtrArrayPtr pArray,
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern IntPtr StdPtrArray_GetData(
                StdPtrArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdPtrArray_Delete(
                StdPtrArrayPtr pArray
            );

            #endregion

            #region StdV8ValueArray methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial StdV8ValueArrayPtr StdV8ValueArray_New(
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern int StdV8ValueArray_GetElementCount(
                StdV8ValueArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdV8ValueArray_SetElementCount(
                StdV8ValueArrayPtr pArray,
                int elementCount
            );

            [DllImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [SuppressGCTransition]
            public static extern V8ValuePtr StdV8ValueArray_GetData(
                StdV8ValueArrayPtr pArray
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void StdV8ValueArray_Delete(
                StdV8ValueArrayPtr pArray
            );

            #endregion

            #region V8Value methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial V8ValuePtr V8Value_New();

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetNonexistent(
                V8ValuePtr pV8Value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetUndefined(
                V8ValuePtr pV8Value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetNull(
                V8ValuePtr pV8Value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetBoolean(
                V8ValuePtr pV8Value,
                sbyte value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetNumber(
                V8ValuePtr pV8Value,
                double value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetInt32(
                V8ValuePtr pV8Value,
                int value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetUInt32(
                V8ValuePtr pV8Value,
                uint value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetString(
                V8ValuePtr pV8Value,
                char* value,
                int length
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetDateTime(
                V8ValuePtr pV8Value,
                double value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetBigInt(
                V8ValuePtr pV8Value,
                int signBit,
                byte* bytes,
                int length
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetV8Object(
                V8ValuePtr pV8Value,
                V8ObjectHandle hObject,
                V8ValueSubtype subtype,
                V8ValueFlags flags
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_SetHostObject(
                V8ValuePtr pV8Value,
                IntPtr pObject
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial V8ValueType V8Value_Decode(
                V8ValuePtr pV8Value,
                out int intValue,
                out uint uintValue,
                out double doubleValue,
                out IntPtr ptrOrHandle
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Value_Delete(
                V8ValuePtr pV8Value
            );

            #endregion

            #region V8CpuProfile methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8CpuProfile_GetInfo(
                V8CpuProfilePtr pProfile,
                V8Entity hEntity,
                StdStringPtr pName,
                out ulong startTimestamp,
                out ulong endTimestamp,
                out int sampleCount,
                out V8CpuProfileImpl.NodePtr pRootNode
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [return: MarshalAs(UnmanagedType.I1)]
            public static partial bool V8CpuProfile_GetSample(
                V8CpuProfilePtr pProfile,
                int index,
                out ulong nodeId,
                out ulong timestamp
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8CpuProfileNode_GetInfo(
                V8CpuProfileImpl.NodePtr pNode,
                V8Entity hEntity,
                out ulong nodeId,
                out long scriptId,
                StdStringPtr pScriptName,
                StdStringPtr pFunctionName,
                StdStringPtr pBailoutReason,
                out long lineNumber,
                out long columnNumber,
                out ulong hitCount,
                out uint hitLineCount,
                out int childCount
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [return: MarshalAs(UnmanagedType.I1)]
            public static partial bool V8CpuProfileNode_GetHitLines(
                V8CpuProfileImpl.NodePtr pNode,
                StdInt32ArrayPtr pLineNumbers,
                StdUInt32ArrayPtr pHitCounts
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial V8CpuProfileImpl.NodePtr V8CpuProfileNode_GetChildNode(
                V8CpuProfileImpl.NodePtr pNode,
                int index
            );

            #endregion

            #region V8 isolate methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial V8IsolateHandle V8Isolate_Create(
                StdStringPtr pName,
                int maxNewSpaceSize,
                int maxOldSpaceSize,
                double heapExpansionMultiplier,
                ulong maxArrayBufferAllocation,
                V8RuntimeFlags flags,
                int debugPort
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial V8ContextHandle V8Isolate_CreateContext(
                V8IsolateHandle hIsolate,
                StdStringPtr pName,
                V8ScriptEngineFlags flags,
                int debugPort
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial UIntPtr V8Isolate_GetMaxHeapSize(
                V8IsolateHandle hIsolate
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_SetMaxHeapSize(
                V8IsolateHandle hIsolate,
                UIntPtr size
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial double V8Isolate_GetHeapSizeSampleInterval(
                V8IsolateHandle hIsolate
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_SetHeapSizeSampleInterval(
                V8IsolateHandle hIsolate,
                double milliseconds
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial UIntPtr V8Isolate_GetMaxStackUsage(
                V8IsolateHandle hIsolate
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_SetMaxStackUsage(
                V8IsolateHandle hIsolate,
                UIntPtr size
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_AwaitDebuggerAndPause(
                V8IsolateHandle hIsolate
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_CancelAwaitDebugger(
                V8IsolateHandle hIsolate
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial V8ScriptHandle V8Isolate_Compile(
                V8IsolateHandle hIsolate,
                StdStringPtr pResourceName,
                StdStringPtr pSourceMapUrl,
                ulong uniqueId,
                [MarshalAs(UnmanagedType.I1)] bool isModule,
                IntPtr pDocumentInfo,
                StdStringPtr pCode
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial V8ScriptHandle V8Isolate_CompileProducingCache(
                V8IsolateHandle hIsolate,
                StdStringPtr pResourceName,
                StdStringPtr pSourceMapUrl,
                ulong uniqueId,
                [MarshalAs(UnmanagedType.I1)] bool isModule,
                IntPtr pDocumentInfo,
                StdStringPtr pCode,
                V8CacheKind cacheKind,
                StdByteArrayPtr pCacheBytes
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial V8ScriptHandle V8Isolate_CompileConsumingCache(
                V8IsolateHandle hIsolate,
                StdStringPtr pResourceName,
                StdStringPtr pSourceMapUrl,
                ulong uniqueId,
                [MarshalAs(UnmanagedType.I1)] bool isModule,
                IntPtr pDocumentInfo,
                StdStringPtr pCode,
                V8CacheKind cacheKind,
                StdByteArrayPtr pCacheBytes,
                [MarshalAs(UnmanagedType.I1)] out bool cacheAccepted
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [return: MarshalAs(UnmanagedType.I1)]
            public static partial bool V8Isolate_GetEnableInterruptPropagation(
                V8IsolateHandle hIsolate
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_SetEnableInterruptPropagation(
                V8IsolateHandle hIsolate,
                [MarshalAs(UnmanagedType.I1)] bool value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [return: MarshalAs(UnmanagedType.I1)]
            public static partial bool V8Isolate_GetDisableHeapSizeViolationInterrupt(
                V8IsolateHandle hIsolate
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_SetDisableHeapSizeViolationInterrupt(
                V8IsolateHandle hIsolate,
                [MarshalAs(UnmanagedType.I1)] bool value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_GetHeapStatistics(
                V8IsolateHandle hIsolate,
                out ulong totalHeapSize,
                out ulong totalHeapSizeExecutable,
                out ulong totalPhysicalSize,
                out ulong totalAvailableSize,
                out ulong usedHeapSize,
                out ulong heapSizeLimit,
                out ulong totalExternalSize
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_GetStatistics(
                V8IsolateHandle hIsolate,
                out ulong scriptCount,
                out ulong scriptCacheSize,
                out ulong moduleCount,
                StdUInt64ArrayPtr pPostedTaskCounts,
                StdUInt64ArrayPtr pInvokedTaskCounts
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_CollectGarbage(
                V8IsolateHandle hIsolate,
                [MarshalAs(UnmanagedType.I1)] bool exhaustive
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [return: MarshalAs(UnmanagedType.I1)]
            public static partial bool V8Isolate_BeginCpuProfile(
                V8IsolateHandle hIsolate,
                StdStringPtr pName,
                [MarshalAs(UnmanagedType.I1)] bool recordSamples
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_EndCpuProfile(
                V8IsolateHandle hIsolate,
                StdStringPtr pName,
                IntPtr pAction
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_CollectCpuProfileSample(
                V8IsolateHandle hIsolate
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial uint V8Isolate_GetCpuProfileSampleInterval(
                V8IsolateHandle hIsolate
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_SetCpuProfileSampleInterval(
                V8IsolateHandle hIsolate,
                uint value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Isolate_WriteHeapSnapshot(
                V8IsolateHandle hIsolate,
                IntPtr pStream
            );

            #endregion

            #region V8 context methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial UIntPtr V8Context_GetMaxIsolateHeapSize(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_SetMaxIsolateHeapSize(
                V8ContextHandle hContext,
                UIntPtr size
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial double V8Context_GetIsolateHeapSizeSampleInterval(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_SetIsolateHeapSizeSampleInterval(
                V8ContextHandle hContext,
                double milliseconds
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial UIntPtr V8Context_GetMaxIsolateStackUsage(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_SetMaxIsolateStackUsage(
                V8ContextHandle hContext,
                UIntPtr size
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_InvokeWithLock(
                V8ContextHandle hContext,
                IntPtr pAction
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_GetRootItem(
                V8ContextHandle hContext,
                V8ValuePtr pItem
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_AddGlobalItem(
                V8ContextHandle hContext,
                StdStringPtr pName,
                V8ValuePtr pValue,
                [MarshalAs(UnmanagedType.I1)] bool globalMembers
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_AwaitDebuggerAndPause(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_CancelAwaitDebugger(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_ExecuteCode(
                V8ContextHandle hContext,
                StdStringPtr pResourceName,
                StdStringPtr pSourceMapUrl,
                ulong uniqueId,
                [MarshalAs(UnmanagedType.I1)] bool isModule,
                IntPtr pDocumentInfo,
                StdStringPtr pCode,
                [MarshalAs(UnmanagedType.I1)] bool evaluate,
                V8ValuePtr pResult
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial V8ScriptHandle V8Context_Compile(
                V8ContextHandle hContext,
                StdStringPtr pResourceName,
                StdStringPtr pSourceMapUrl,
                ulong uniqueId,
                [MarshalAs(UnmanagedType.I1)] bool isModule,
                IntPtr pDocumentInfo,
                StdStringPtr pCode
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial V8ScriptHandle V8Context_CompileProducingCache(
                V8ContextHandle hContext,
                StdStringPtr pResourceName,
                StdStringPtr pSourceMapUrl,
                ulong uniqueId,
                [MarshalAs(UnmanagedType.I1)] bool isModule,
                IntPtr pDocumentInfo,
                StdStringPtr pCode,
                V8CacheKind cacheKind,
                StdByteArrayPtr pCacheBytes
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial V8ScriptHandle V8Context_CompileConsumingCache(
                V8ContextHandle hContext,
                StdStringPtr pResourceName,
                StdStringPtr pSourceMapUrl,
                ulong uniqueId,
                [MarshalAs(UnmanagedType.I1)] bool isModule,
                IntPtr pDocumentInfo,
                StdStringPtr pCode,
                V8CacheKind cacheKind,
                StdByteArrayPtr pCacheBytes,
                [MarshalAs(UnmanagedType.I1)] out bool cacheAccepted
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_ExecuteScript(
                V8ContextHandle hContext,
                V8ScriptHandle hScript,
                [MarshalAs(UnmanagedType.I1)] bool evaluate,
                V8ValuePtr pResult
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_Interrupt(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_CancelInterrupt(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [return: MarshalAs(UnmanagedType.I1)]
            public static partial bool V8Context_GetEnableIsolateInterruptPropagation(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_SetEnableIsolateInterruptPropagation(
                V8ContextHandle hContext,
                [MarshalAs(UnmanagedType.I1)] bool value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [return: MarshalAs(UnmanagedType.I1)]
            public static partial bool V8Context_GetDisableIsolateHeapSizeViolationInterrupt(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_SetDisableIsolateHeapSizeViolationInterrupt(
                V8ContextHandle hContext,
                [MarshalAs(UnmanagedType.I1)] bool value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_GetIsolateHeapStatistics(
                V8ContextHandle hContext,
                out ulong totalHeapSize,
                out ulong totalHeapSizeExecutable,
                out ulong totalPhysicalSize,
                out ulong totalAvailableSize,
                out ulong usedHeapSize,
                out ulong heapSizeLimit,
                out ulong totalExternalSize
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_GetIsolateStatistics(
                V8ContextHandle hContext,
                out ulong scriptCount,
                out ulong scriptCacheSize,
                out ulong moduleCount,
                StdUInt64ArrayPtr pPostedTaskCounts,
                StdUInt64ArrayPtr pInvokedTaskCounts
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_GetStatistics(
                V8ContextHandle hContext,
                out ulong scriptCount,
                out ulong moduleCount,
                out ulong moduleCacheSize
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_CollectGarbage(
                V8ContextHandle hContext,
                [MarshalAs(UnmanagedType.I1)] bool exhaustive
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_OnAccessSettingsChanged(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [return: MarshalAs(UnmanagedType.I1)]
            public static partial bool V8Context_BeginCpuProfile(
                V8ContextHandle hContext,
                StdStringPtr pName,
                [MarshalAs(UnmanagedType.I1)] bool recordSamples
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_EndCpuProfile(
                V8ContextHandle hContext,
                StdStringPtr pName,
                IntPtr pAction
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_CollectCpuProfileSample(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial uint V8Context_GetCpuProfileSampleInterval(
                V8ContextHandle hContext
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_SetCpuProfileSampleInterval(
                V8ContextHandle hContext,
                uint value
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Context_WriteIsolateHeapSnapshot(
                V8ContextHandle hContext,
                IntPtr pStream
            );

            #endregion

            #region V8 object methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Object_GetNamedProperty(
                V8ObjectHandle hObject,
                StdStringPtr pName,
                V8ValuePtr pValue
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Object_SetNamedProperty(
                V8ObjectHandle hObject,
                StdStringPtr pName,
                V8ValuePtr pValue
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [return: MarshalAs(UnmanagedType.I1)]
            public static partial bool V8Object_DeleteNamedProperty(
                V8ObjectHandle hObject,
                StdStringPtr pName
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Object_GetPropertyNames(
                V8ObjectHandle hObject,
                StdStringArrayPtr pNames
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Object_GetIndexedProperty(
                V8ObjectHandle hObject,
                int index,
                V8ValuePtr pValue
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Object_SetIndexedProperty(
                V8ObjectHandle hObject,
                int index,
                V8ValuePtr pValue
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            [return: MarshalAs(UnmanagedType.I1)]
            public static partial bool V8Object_DeleteIndexedProperty(
                V8ObjectHandle hObject,
                int index
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Object_GetPropertyIndices(
                V8ObjectHandle hObject,
                StdInt32ArrayPtr pIndices
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Object_Invoke(
                V8ObjectHandle hObject,
                [MarshalAs(UnmanagedType.I1)] bool asConstructor,
                StdV8ValueArrayPtr pArgs,
                V8ValuePtr pResult
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Object_InvokeMethod(
                V8ObjectHandle hObject,
                StdStringPtr pName,
                StdV8ValueArrayPtr pArgs,
                V8ValuePtr pResult
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Object_GetArrayBufferOrViewInfo(
                V8ObjectHandle hObject,
                V8ValuePtr pArrayBuffer,
                out ulong offset,
                out ulong size,
                out ulong length
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Object_InvokeWithArrayBufferOrViewData(
                V8ObjectHandle hObject,
                IntPtr pAction
            );

            #endregion

            #region V8 debug callback methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8DebugCallback_ConnectClient(
                V8DebugCallbackHandle hCallback
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8DebugCallback_SendCommand(
                V8DebugCallbackHandle hCallback,
                StdStringPtr pCommand
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8DebugCallback_DisconnectClient(
                V8DebugCallbackHandle hCallback
            );

            #endregion

            #region native callback methods

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void NativeCallback_Invoke(
                NativeCallbackHandle hCallback
            );

            #endregion

            #region V8 entity cleanup

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Entity_Release(
                V8Entity hEntity
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8Entity_DestroyHandle(
                V8Entity hEntity
            );

            #endregion

            #region error handling

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void HostException_Schedule(
                StdStringPtr pMessage,
                V8ValuePtr pException
            );

            #endregion

            #region unit test support

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial UIntPtr V8UnitTestSupport_GetTextDigest(
                StdStringPtr pString
            );

            [LibraryImport(DllName)]
            [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
            public static partial void V8UnitTestSupport_GetStatistics(
                out ulong isolateCount,
                out ulong contextCount
            );

            #endregion

            #endregion
        }

        #endregion
    }
}

