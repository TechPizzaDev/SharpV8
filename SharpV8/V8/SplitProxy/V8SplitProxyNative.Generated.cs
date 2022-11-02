// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.ClearScript.V8.SplitProxy
{
    internal static partial class V8SplitProxyNative
    {
        #region V8SplitProxyNative implementation

        #region initialization

        public static IntPtr V8SplitProxyManaged_SetMethodTable(IntPtr pMethodTable)
        {
            return Imports.V8SplitProxyManaged_SetMethodTable(pMethodTable);
        }

        public static string V8SplitProxyNative_GetVersion()
        {
            return Marshal.PtrToStringUni(Imports.V8SplitProxyNative_GetVersion());
        }

        public static void V8Environment_InitializeICU(IntPtr pICUData, uint size)
        {
            Imports.V8Environment_InitializeICU(pICUData, size);
        }

        #endregion

        #region StdString methods

        public static StdString.Ptr StdString_New(string value)
        {
            return Imports.StdString_New(value, value.Length);
        }

        public static string StdString_GetValue(StdString.Ptr pString)
        {
            var pValue = Imports.StdString_GetValue(pString, out var length);
            return Marshal.PtrToStringUni(pValue, length);
        }

        public static void StdString_SetValue(StdString.Ptr pString, string value)
        {
            Imports.StdString_SetValue(pString, value, value.Length);
        }

        public static void StdString_Delete(StdString.Ptr pString)
        {
            Imports.StdString_Delete(pString);
        }

        #endregion

        #region StdStringArray methods

        public static StdStringArray.Ptr StdStringArray_New(int elementCount)
        {
            return Imports.StdStringArray_New(elementCount);
        }

        public static int StdStringArray_GetElementCount(StdStringArray.Ptr pArray)
        {
            return Imports.StdStringArray_GetElementCount(pArray);
        }

        public static void StdStringArray_SetElementCount(StdStringArray.Ptr pArray, int elementCount)
        {
            Imports.StdStringArray_SetElementCount(pArray, elementCount);
        }

        public static string StdStringArray_GetElement(StdStringArray.Ptr pArray, int index)
        {
            var pValue = Imports.StdStringArray_GetElement(pArray, index, out var length);
            return Marshal.PtrToStringUni(pValue, length);
        }

        public static void StdStringArray_SetElement(StdStringArray.Ptr pArray, int index, string value)
        {
            Imports.StdStringArray_SetElement(pArray, index, value, value.Length);
        }

        public static void StdStringArray_Delete(StdStringArray.Ptr pArray)
        {
            Imports.StdStringArray_Delete(pArray);
        }

        #endregion

        #region StdByteArray methods

        public static StdByteArray.Ptr StdByteArray_New(int elementCount)
        {
            return Imports.StdByteArray_New(elementCount);
        }

        public static int StdByteArray_GetElementCount(StdByteArray.Ptr pArray)
        {
            return Imports.StdByteArray_GetElementCount(pArray);
        }

        public static void StdByteArray_SetElementCount(StdByteArray.Ptr pArray, int elementCount)
        {
            Imports.StdByteArray_SetElementCount(pArray, elementCount);
        }

        public static IntPtr StdByteArray_GetData(StdByteArray.Ptr pArray)
        {
            return Imports.StdByteArray_GetData(pArray);
        }

        public static void StdByteArray_Delete(StdByteArray.Ptr pArray)
        {
            Imports.StdByteArray_Delete(pArray);
        }

        #endregion

        #region StdInt32Array methods

        public static StdInt32Array.Ptr StdInt32Array_New(int elementCount)
        {
            return Imports.StdInt32Array_New(elementCount);
        }

        public static int StdInt32Array_GetElementCount(StdInt32Array.Ptr pArray)
        {
            return Imports.StdInt32Array_GetElementCount(pArray);
        }

        public static void StdInt32Array_SetElementCount(StdInt32Array.Ptr pArray, int elementCount)
        {
            Imports.StdInt32Array_SetElementCount(pArray, elementCount);
        }

        public static IntPtr StdInt32Array_GetData(StdInt32Array.Ptr pArray)
        {
            return Imports.StdInt32Array_GetData(pArray);
        }

        public static void StdInt32Array_Delete(StdInt32Array.Ptr pArray)
        {
            Imports.StdInt32Array_Delete(pArray);
        }

        #endregion

        #region StdUInt32Array methods

        public static StdUInt32Array.Ptr StdUInt32Array_New(int elementCount)
        {
            return Imports.StdUInt32Array_New(elementCount);
        }

        public static int StdUInt32Array_GetElementCount(StdUInt32Array.Ptr pArray)
        {
            return Imports.StdUInt32Array_GetElementCount(pArray);
        }

        public static void StdUInt32Array_SetElementCount(StdUInt32Array.Ptr pArray, int elementCount)
        {
            Imports.StdUInt32Array_SetElementCount(pArray, elementCount);
        }

        public static IntPtr StdUInt32Array_GetData(StdUInt32Array.Ptr pArray)
        {
            return Imports.StdUInt32Array_GetData(pArray);
        }

        public static void StdUInt32Array_Delete(StdUInt32Array.Ptr pArray)
        {
            Imports.StdUInt32Array_Delete(pArray);
        }

        #endregion

        #region StdUInt64Array methods

        public static StdUInt64Array.Ptr StdUInt64Array_New(int elementCount)
        {
            return Imports.StdUInt64Array_New(elementCount);
        }

        public static int StdUInt64Array_GetElementCount(StdUInt64Array.Ptr pArray)
        {
            return Imports.StdUInt64Array_GetElementCount(pArray);
        }

        public static void StdUInt64Array_SetElementCount(StdUInt64Array.Ptr pArray, int elementCount)
        {
            Imports.StdUInt64Array_SetElementCount(pArray, elementCount);
        }

        public static IntPtr StdUInt64Array_GetData(StdUInt64Array.Ptr pArray)
        {
            return Imports.StdUInt64Array_GetData(pArray);
        }

        public static void StdUInt64Array_Delete(StdUInt64Array.Ptr pArray)
        {
            Imports.StdUInt64Array_Delete(pArray);
        }

        #endregion

        #region StdPtrArray methods

        public static StdPtrArray.Ptr StdPtrArray_New(int elementCount)
        {
            return Imports.StdPtrArray_New(elementCount);
        }

        public static int StdPtrArray_GetElementCount(StdPtrArray.Ptr pArray)
        {
            return Imports.StdPtrArray_GetElementCount(pArray);
        }

        public static void StdPtrArray_SetElementCount(StdPtrArray.Ptr pArray, int elementCount)
        {
            Imports.StdPtrArray_SetElementCount(pArray, elementCount);
        }

        public static IntPtr StdPtrArray_GetData(StdPtrArray.Ptr pArray)
        {
            return Imports.StdPtrArray_GetData(pArray);
        }

        public static void StdPtrArray_Delete(StdPtrArray.Ptr pArray)
        {
            Imports.StdPtrArray_Delete(pArray);
        }

        #endregion

        #region StdV8ValueArray methods

        public static StdV8ValueArray.Ptr StdV8ValueArray_New(int elementCount)
        {
            return Imports.StdV8ValueArray_New(elementCount);
        }

        public static int StdV8ValueArray_GetElementCount(StdV8ValueArray.Ptr pArray)
        {
            return Imports.StdV8ValueArray_GetElementCount(pArray);
        }

        public static void StdV8ValueArray_SetElementCount(StdV8ValueArray.Ptr pArray, int elementCount)
        {
            Imports.StdV8ValueArray_SetElementCount(pArray, elementCount);
        }

        public static V8Value.Ptr StdV8ValueArray_GetData(StdV8ValueArray.Ptr pArray)
        {
            return Imports.StdV8ValueArray_GetData(pArray);
        }

        public static void StdV8ValueArray_Delete(StdV8ValueArray.Ptr pArray)
        {
            Imports.StdV8ValueArray_Delete(pArray);
        }

        #endregion

        #region V8Value methods

        public static V8Value.Ptr V8Value_New()
        {
            return Imports.V8Value_New();
        }

        public static void V8Value_SetNonexistent(V8Value.Ptr pV8Value)
        {
            Imports.V8Value_SetNonexistent(pV8Value);
        }

        public static void V8Value_SetUndefined(V8Value.Ptr pV8Value)
        {
            Imports.V8Value_SetUndefined(pV8Value);
        }

        public static void V8Value_SetNull(V8Value.Ptr pV8Value)
        {
            Imports.V8Value_SetNull(pV8Value);
        }

        public static void V8Value_SetBoolean(V8Value.Ptr pV8Value, bool value)
        {
            Imports.V8Value_SetBoolean(pV8Value, value);
        }

        public static void V8Value_SetNumber(V8Value.Ptr pV8Value, double value)
        {
            Imports.V8Value_SetNumber(pV8Value, value);
        }

        public static void V8Value_SetInt32(V8Value.Ptr pV8Value, int value)
        {
            Imports.V8Value_SetInt32(pV8Value, value);
        }

        public static void V8Value_SetUInt32(V8Value.Ptr pV8Value, uint value)
        {
            Imports.V8Value_SetUInt32(pV8Value, value);
        }

        public static void V8Value_SetString(V8Value.Ptr pV8Value, string value)
        {
            Imports.V8Value_SetString(pV8Value, value, value.Length);
        }

        public static void V8Value_SetDateTime(V8Value.Ptr pV8Value, double value)
        {
            Imports.V8Value_SetDateTime(pV8Value, value);
        }

        public static void V8Value_SetBigInt(V8Value.Ptr pV8Value, int signBit, byte[] bytes)
        {
            Imports.V8Value_SetBigInt(pV8Value, signBit, bytes, bytes.Length);
        }

        public static void V8Value_SetV8Object(V8Value.Ptr pV8Value, V8Object.Handle hObject, V8Value.Subtype subtype, V8Value.Flags flags)
        {
            Imports.V8Value_SetV8Object(pV8Value, hObject, subtype, flags);
        }

        public static void V8Value_SetHostObject(V8Value.Ptr pV8Value, IntPtr pObject)
        {
            Imports.V8Value_SetHostObject(pV8Value, pObject);
        }

        public static V8Value.Type V8Value_Decode(V8Value.Ptr pV8Value, out int intValue, out uint uintValue, out double doubleValue, out IntPtr ptrOrHandle)
        {
            return Imports.V8Value_Decode(pV8Value, out intValue, out uintValue, out doubleValue, out ptrOrHandle);
        }

        public static void V8Value_Delete(V8Value.Ptr pV8Value)
        {
            Imports.V8Value_Delete(pV8Value);
        }

        #endregion

        #region V8CpuProfile methods

        public static void V8CpuProfile_GetInfo(V8CpuProfile.Ptr pProfile, V8Entity.Handle hEntity, out string name, out ulong startTimestamp, out ulong endTimestamp, out int sampleCount, out V8CpuProfile.Node.Ptr pRootNode)
        {
            using (var nameScope = StdString.CreateScope())
            {
                Imports.V8CpuProfile_GetInfo(pProfile, hEntity, nameScope.Value, out startTimestamp, out endTimestamp, out sampleCount, out pRootNode);
                name = StdString.GetValue(nameScope.Value);
            }
        }

        public static bool V8CpuProfile_GetSample(V8CpuProfile.Ptr pProfile, int index, out ulong nodeId, out ulong timestamp)
        {
            return Imports.V8CpuProfile_GetSample(pProfile, index, out nodeId, out timestamp);
        }

        public static void V8CpuProfileNode_GetInfo(V8CpuProfile.Node.Ptr pNode, V8Entity.Handle hEntity, out ulong nodeId, out long scriptId, out string scriptName, out string functionName, out string bailoutReason, out long lineNumber, out long columnNumber, out ulong hitCount, out uint hitLineCount, out int childCount)
        {
            using (var scriptNameScope = StdString.CreateScope())
            {
                using (var functionNameScope = StdString.CreateScope())
                {
                    using (var bailoutReasonScope = StdString.CreateScope())
                    {
                        Imports.V8CpuProfileNode_GetInfo(pNode, hEntity, out nodeId, out scriptId, scriptNameScope.Value, functionNameScope.Value, bailoutReasonScope.Value, out lineNumber, out columnNumber, out hitCount, out hitLineCount, out childCount);
                        scriptName = StdString.GetValue(scriptNameScope.Value);
                        functionName = StdString.GetValue(functionNameScope.Value);
                        bailoutReason = StdString.GetValue(bailoutReasonScope.Value);

                    }
                }
            }
        }

        public static bool V8CpuProfileNode_GetHitLines(V8CpuProfile.Node.Ptr pNode, out int[] lineNumbers, out uint[] hitCounts)
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

        public static V8CpuProfile.Node.Ptr V8CpuProfileNode_GetChildNode(V8CpuProfile.Node.Ptr pNode, int index)
        {
            return Imports.V8CpuProfileNode_GetChildNode(pNode, index);
        }

        #endregion

        #region V8 isolate methods

        public static V8Isolate.Handle V8Isolate_Create(string name, int maxNewSpaceSize, int maxOldSpaceSize, double heapExpansionMultiplier, ulong maxArrayBufferAllocation, V8RuntimeFlags flags, int debugPort)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                return Imports.V8Isolate_Create(nameScope.Value, maxNewSpaceSize, maxOldSpaceSize, heapExpansionMultiplier, maxArrayBufferAllocation, flags, debugPort);
            }
        }

        public static V8Context.Handle V8Isolate_CreateContext(V8Isolate.Handle hIsolate, string name, V8ScriptEngineFlags flags, int debugPort)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                return Imports.V8Isolate_CreateContext(hIsolate, nameScope.Value, flags, debugPort);
            }
        }

        public static UIntPtr V8Isolate_GetMaxHeapSize(V8Isolate.Handle hIsolate)
        {
            return Imports.V8Isolate_GetMaxHeapSize(hIsolate);
        }

        public static void V8Isolate_SetMaxHeapSize(V8Isolate.Handle hIsolate, UIntPtr size)
        {
            Imports.V8Isolate_SetMaxHeapSize(hIsolate, size);
        }

        public static double V8Isolate_GetHeapSizeSampleInterval(V8Isolate.Handle hIsolate)
        {
            return Imports.V8Isolate_GetHeapSizeSampleInterval(hIsolate);
        }

        public static void V8Isolate_SetHeapSizeSampleInterval(V8Isolate.Handle hIsolate, double milliseconds)
        {
            Imports.V8Isolate_SetHeapSizeSampleInterval(hIsolate, milliseconds);
        }

        public static UIntPtr V8Isolate_GetMaxStackUsage(V8Isolate.Handle hIsolate)
        {
            return Imports.V8Isolate_GetMaxStackUsage(hIsolate);
        }

        public static void V8Isolate_SetMaxStackUsage(V8Isolate.Handle hIsolate, UIntPtr size)
        {
            Imports.V8Isolate_SetMaxStackUsage(hIsolate, size);
        }

        public static void V8Isolate_AwaitDebuggerAndPause(V8Isolate.Handle hIsolate)
        {
            Imports.V8Isolate_AwaitDebuggerAndPause(hIsolate);
        }

        public static void V8Isolate_CancelAwaitDebugger(V8Isolate.Handle hIsolate)
        {
            Imports.V8Isolate_CancelAwaitDebugger(hIsolate);
        }

        public static V8Script.Handle V8Isolate_Compile(V8Isolate.Handle hIsolate, string resourceName, string sourceMapUrl, ulong uniqueId, bool isModule, IntPtr pDocumentInfo, string code)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        return Imports.V8Isolate_Compile(hIsolate, resourceNameScope.Value, sourceMapUrlScope.Value, uniqueId, isModule, pDocumentInfo, codeScope.Value);
                    }
                }
            }
        }

        public static V8Script.Handle V8Isolate_CompileProducingCache(V8Isolate.Handle hIsolate, string resourceName, string sourceMapUrl, ulong uniqueId, bool isModule, IntPtr pDocumentInfo, string code, V8CacheKind cacheKind, out byte[] cacheBytes)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        using (var cacheBytesScope = StdByteArray.CreateScope())
                        {
                            var hScript = Imports.V8Isolate_CompileProducingCache(hIsolate, resourceNameScope.Value, sourceMapUrlScope.Value, uniqueId, isModule, pDocumentInfo, codeScope.Value, cacheKind, cacheBytesScope.Value);
                            cacheBytes = StdByteArray.ToArray(cacheBytesScope.Value);
                            return hScript;
                        }
                    }
                }
            }
        }

        public static V8Script.Handle V8Isolate_CompileConsumingCache(V8Isolate.Handle hIsolate, string resourceName, string sourceMapUrl, ulong uniqueId, bool isModule, IntPtr pDocumentInfo, string code, V8CacheKind cacheKind, byte[] cacheBytes, out bool cacheAccepted)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        using (var cacheBytesScope = StdByteArray.CreateScope(cacheBytes))
                        {
                            return Imports.V8Isolate_CompileConsumingCache(hIsolate, resourceNameScope.Value, sourceMapUrlScope.Value, uniqueId, isModule, pDocumentInfo, codeScope.Value, cacheKind, cacheBytesScope.Value, out cacheAccepted);
                        }
                    }
                }
            }
        }

        public static bool V8Isolate_GetEnableInterruptPropagation(V8Isolate.Handle hIsolate)
        {
            return Imports.V8Isolate_GetEnableInterruptPropagation(hIsolate);
        }

        public static void V8Isolate_SetEnableInterruptPropagation(V8Isolate.Handle hIsolate, bool value)
        {
            Imports.V8Isolate_SetEnableInterruptPropagation(hIsolate, value);
        }

        public static bool V8Isolate_GetDisableHeapSizeViolationInterrupt(V8Isolate.Handle hIsolate)
        {
            return Imports.V8Isolate_GetDisableHeapSizeViolationInterrupt(hIsolate);
        }

        public static void V8Isolate_SetDisableHeapSizeViolationInterrupt(V8Isolate.Handle hIsolate, bool value)
        {
            Imports.V8Isolate_SetDisableHeapSizeViolationInterrupt(hIsolate, value);
        }

        public static void V8Isolate_GetHeapStatistics(V8Isolate.Handle hIsolate, out ulong totalHeapSize, out ulong totalHeapSizeExecutable, out ulong totalPhysicalSize, out ulong totalAvailableSize, out ulong usedHeapSize, out ulong heapSizeLimit, out ulong totalExternalSize)
        {
            Imports.V8Isolate_GetHeapStatistics(hIsolate, out totalHeapSize, out totalHeapSizeExecutable, out totalPhysicalSize, out totalAvailableSize, out usedHeapSize, out heapSizeLimit, out totalExternalSize);
        }

        public static void V8Isolate_GetStatistics(V8Isolate.Handle hIsolate, out ulong scriptCount, out ulong scriptCacheSize, out ulong moduleCount, out ulong[] postedTaskCounts, out ulong[] invokedTaskCounts)
        {
            using (var postedTaskCountsScope = StdUInt64Array.CreateScope())
            {
                using (var invokedTaskCountsScope = StdUInt64Array.CreateScope())
                {
                    Imports.V8Isolate_GetStatistics(hIsolate, out scriptCount, out scriptCacheSize, out moduleCount, postedTaskCountsScope.Value, invokedTaskCountsScope.Value);
                    postedTaskCounts = StdUInt64Array.ToArray(postedTaskCountsScope.Value);
                    invokedTaskCounts = StdUInt64Array.ToArray(invokedTaskCountsScope.Value);
                }
            }
        }

        public static void V8Isolate_CollectGarbage(V8Isolate.Handle hIsolate, bool exhaustive)
        {
            Imports.V8Isolate_CollectGarbage(hIsolate, exhaustive);
        }

        public static bool V8Isolate_BeginCpuProfile(V8Isolate.Handle hIsolate, string name, bool recordSamples)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                return Imports.V8Isolate_BeginCpuProfile(hIsolate, nameScope.Value, recordSamples);
            }
        }

        public static void V8Isolate_EndCpuProfile(V8Isolate.Handle hIsolate, string name, IntPtr pAction)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                Imports.V8Isolate_EndCpuProfile(hIsolate, nameScope.Value, pAction);
            }
        }

        public static void V8Isolate_CollectCpuProfileSample(V8Isolate.Handle hIsolate)
        {
            V8Isolate_CollectCpuProfileSample(hIsolate);
        }

        public static uint V8Isolate_GetCpuProfileSampleInterval(V8Isolate.Handle hIsolate)
        {
            return Imports.V8Isolate_GetCpuProfileSampleInterval(hIsolate);
        }

        public static void V8Isolate_SetCpuProfileSampleInterval(V8Isolate.Handle hIsolate, uint value)
        {
            Imports.V8Isolate_SetCpuProfileSampleInterval(hIsolate, value);
        }

        public static void V8Isolate_WriteHeapSnapshot(V8Isolate.Handle hIsolate, IntPtr pStream)
        {
            Imports.V8Isolate_WriteHeapSnapshot(hIsolate, pStream);
        }

        #endregion

        #region V8 context methods

        public static UIntPtr V8Context_GetMaxIsolateHeapSize(V8Context.Handle hContext)
        {
            return Imports.V8Context_GetMaxIsolateHeapSize(hContext);
        }

        public static void V8Context_SetMaxIsolateHeapSize(V8Context.Handle hContext, UIntPtr size)
        {
            Imports.V8Context_SetMaxIsolateHeapSize(hContext, size);
        }

        public static double V8Context_GetIsolateHeapSizeSampleInterval(V8Context.Handle hContext)
        {
            return Imports.V8Context_GetIsolateHeapSizeSampleInterval(hContext);
        }

        public static void V8Context_SetIsolateHeapSizeSampleInterval(V8Context.Handle hContext, double milliseconds)
        {
            Imports.V8Context_SetIsolateHeapSizeSampleInterval(hContext, milliseconds);
        }

        public static UIntPtr V8Context_GetMaxIsolateStackUsage(V8Context.Handle hContext)
        {
            return Imports.V8Context_GetMaxIsolateStackUsage(hContext);
        }

        public static void V8Context_SetMaxIsolateStackUsage(V8Context.Handle hContext, UIntPtr size)
        {
            Imports.V8Context_SetMaxIsolateStackUsage(hContext, size);
        }

        public static void V8Context_InvokeWithLock(V8Context.Handle hContext, IntPtr pAction)
        {
            Imports.V8Context_InvokeWithLock(hContext, pAction);
        }

        public static object V8Context_GetRootItem(V8Context.Handle hContext)
        {
            using (var itemScope = V8Value.CreateScope())
            {
                Imports.V8Context_GetRootItem(hContext, itemScope.Value);
                return V8Value.Get(itemScope.Value);
            }
        }

        public static void V8Context_AddGlobalItem(V8Context.Handle hContext, string name, object value, bool globalMembers)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                using (var valueScope = V8Value.CreateScope(value))
                {
                    Imports.V8Context_AddGlobalItem(hContext, nameScope.Value, valueScope.Value, globalMembers);
                }
            }
        }

        public static void V8Context_AwaitDebuggerAndPause(V8Context.Handle hContext)
        {
            Imports.V8Context_AwaitDebuggerAndPause(hContext);
        }

        public static void V8Context_CancelAwaitDebugger(V8Context.Handle hContext)
        {
            Imports.V8Context_CancelAwaitDebugger(hContext);
        }

        public static object V8Context_ExecuteCode(V8Context.Handle hContext, string resourceName, string sourceMapUrl, ulong uniqueId, bool isModule, IntPtr pDocumentInfo, string code, bool evaluate)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        using (var resultScope = V8Value.CreateScope())
                        {
                            Imports.V8Context_ExecuteCode(hContext, resourceNameScope.Value, sourceMapUrlScope.Value, uniqueId, isModule, pDocumentInfo, codeScope.Value, evaluate, resultScope.Value);
                            return V8Value.Get(resultScope.Value);
                        }
                    }
                }
            }
        }

        public static V8Script.Handle V8Context_Compile(V8Context.Handle hContext, string resourceName, string sourceMapUrl, ulong uniqueId, bool isModule, IntPtr pDocumentInfo, string code)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        return Imports.V8Context_Compile(hContext, resourceNameScope.Value, sourceMapUrlScope.Value, uniqueId, isModule, pDocumentInfo, codeScope.Value);
                    }
                }
            }
        }

        public static V8Script.Handle V8Context_CompileProducingCache(V8Context.Handle hContext, string resourceName, string sourceMapUrl, ulong uniqueId, bool isModule, IntPtr pDocumentInfo, string code, V8CacheKind cacheKind, out byte[] cacheBytes)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        using (var cacheBytesScope = StdByteArray.CreateScope())
                        {
                            var hScript = Imports.V8Context_CompileProducingCache(hContext, resourceNameScope.Value, sourceMapUrlScope.Value, uniqueId, isModule, pDocumentInfo, codeScope.Value, cacheKind, cacheBytesScope.Value);
                            cacheBytes = StdByteArray.ToArray(cacheBytesScope.Value);
                            return hScript;
                        }
                    }
                }
            }
        }

        public static V8Script.Handle V8Context_CompileConsumingCache(V8Context.Handle hContext, string resourceName, string sourceMapUrl, ulong uniqueId, bool isModule, IntPtr pDocumentInfo, string code, V8CacheKind cacheKind, byte[] cacheBytes, out bool cacheAccepted)
        {
            using (var resourceNameScope = StdString.CreateScope(resourceName))
            {
                using (var sourceMapUrlScope = StdString.CreateScope(sourceMapUrl))
                {
                    using (var codeScope = StdString.CreateScope(code))
                    {
                        using (var cacheBytesScope = StdByteArray.CreateScope(cacheBytes))
                        {
                            return Imports.V8Context_CompileConsumingCache(hContext, resourceNameScope.Value, sourceMapUrlScope.Value, uniqueId, isModule, pDocumentInfo, codeScope.Value, cacheKind, cacheBytesScope.Value, out cacheAccepted);
                        }
                    }
                }
            }
        }

        public static object V8Context_ExecuteScript(V8Context.Handle hContext, V8Script.Handle hScript, bool evaluate)
        {
            using (var resultScope = V8Value.CreateScope())
            {
                Imports.V8Context_ExecuteScript(hContext, hScript, evaluate, resultScope.Value);
                return V8Value.Get(resultScope.Value);
            }
        }

        public static void V8Context_Interrupt(V8Context.Handle hContext)
        {
            Imports.V8Context_Interrupt(hContext);
        }

        public static void V8Context_CancelInterrupt(V8Context.Handle hContext)
        {
            Imports.V8Context_CancelInterrupt(hContext);
        }

        public static bool V8Context_GetEnableIsolateInterruptPropagation(V8Context.Handle hContext)
        {
            return Imports.V8Context_GetEnableIsolateInterruptPropagation(hContext);
        }

        public static void V8Context_SetEnableIsolateInterruptPropagation(V8Context.Handle hContext, bool value)
        {
            Imports.V8Context_SetEnableIsolateInterruptPropagation(hContext, value);
        }

        public static bool V8Context_GetDisableIsolateHeapSizeViolationInterrupt(V8Context.Handle hContext)
        {
            return Imports.V8Context_GetDisableIsolateHeapSizeViolationInterrupt(hContext);
        }

        public static void V8Context_SetDisableIsolateHeapSizeViolationInterrupt(V8Context.Handle hContext, bool value)
        {
            Imports.V8Context_SetDisableIsolateHeapSizeViolationInterrupt(hContext, value);
        }

        public static void V8Context_GetIsolateHeapStatistics(V8Context.Handle hContext, out ulong totalHeapSize, out ulong totalHeapSizeExecutable, out ulong totalPhysicalSize, out ulong totalAvailableSize, out ulong usedHeapSize, out ulong heapSizeLimit, out ulong totalExternalSize)
        {
            Imports.V8Context_GetIsolateHeapStatistics(hContext, out totalHeapSize, out totalHeapSizeExecutable, out totalPhysicalSize, out totalAvailableSize, out usedHeapSize, out heapSizeLimit, out totalExternalSize);
        }

        public static void V8Context_GetIsolateStatistics(V8Context.Handle hContext, out ulong scriptCount, out ulong scriptCacheSize, out ulong moduleCount, out ulong[] postedTaskCounts, out ulong[] invokedTaskCounts)
        {
            using (var postedTaskCountsScope = StdUInt64Array.CreateScope())
            {
                using (var invokedTaskCountsScope = StdUInt64Array.CreateScope())
                {
                    Imports.V8Context_GetIsolateStatistics(hContext, out scriptCount, out scriptCacheSize, out moduleCount, postedTaskCountsScope.Value, invokedTaskCountsScope.Value);
                    postedTaskCounts = StdUInt64Array.ToArray(postedTaskCountsScope.Value);
                    invokedTaskCounts = StdUInt64Array.ToArray(invokedTaskCountsScope.Value);
                }
            }
        }

        public static void V8Context_GetStatistics(V8Context.Handle hContext, out ulong scriptCount, out ulong moduleCount, out ulong moduleCacheSize)
        {
            Imports.V8Context_GetStatistics(hContext, out scriptCount, out moduleCount, out moduleCacheSize);
        }

        public static void V8Context_CollectGarbage(V8Context.Handle hContext, bool exhaustive)
        {
            Imports.V8Context_CollectGarbage(hContext, exhaustive);
        }

        public static void V8Context_OnAccessSettingsChanged(V8Context.Handle hContext)
        {
            Imports.V8Context_OnAccessSettingsChanged(hContext);
        }

        public static bool V8Context_BeginCpuProfile(V8Context.Handle hContext, string name, bool recordSamples)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                return Imports.V8Context_BeginCpuProfile(hContext, nameScope.Value, recordSamples);
            }
        }

        public static void V8Context_EndCpuProfile(V8Context.Handle hContext, string name, IntPtr pAction)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                Imports.V8Context_EndCpuProfile(hContext, nameScope.Value, pAction);
            }
        }

        public static void V8Context_CollectCpuProfileSample(V8Context.Handle hContext)
        {
            Imports.V8Context_CollectCpuProfileSample(hContext);
        }

        public static uint V8Context_GetCpuProfileSampleInterval(V8Context.Handle hContext)
        {
            return Imports.V8Context_GetCpuProfileSampleInterval(hContext);
        }

        public static void V8Context_SetCpuProfileSampleInterval(V8Context.Handle hContext, uint value)
        {
            Imports.V8Context_SetCpuProfileSampleInterval(hContext, value);
        }

        public static void V8Context_WriteIsolateHeapSnapshot(V8Context.Handle hContext, IntPtr pStream)
        {
            Imports.V8Context_WriteIsolateHeapSnapshot(hContext, pStream);
        }

        #endregion

        #region V8 object methods

        public static object V8Object_GetNamedProperty(V8Object.Handle hObject, string name)
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

        public static void V8Object_SetNamedProperty(V8Object.Handle hObject, string name, object value)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                using (var valueScope = V8Value.CreateScope(value))
                {
                    Imports.V8Object_SetNamedProperty(hObject, nameScope.Value, valueScope.Value);
                }
            }
        }

        public static bool V8Object_DeleteNamedProperty(V8Object.Handle hObject, string name)
        {
            using (var nameScope = StdString.CreateScope(name))
            {
                return Imports.V8Object_DeleteNamedProperty(hObject, nameScope.Value);
            }
        }

        public static string[] V8Object_GetPropertyNames(V8Object.Handle hObject)
        {
            using (var namesScope = StdStringArray.CreateScope())
            {
                Imports.V8Object_GetPropertyNames(hObject, namesScope.Value);
                return StdStringArray.ToArray(namesScope.Value);
            }
        }

        public static object V8Object_GetIndexedProperty(V8Object.Handle hObject, int index)
        {
            using (var valueScope = V8Value.CreateScope())
            {
                Imports.V8Object_GetIndexedProperty(hObject, index, valueScope.Value);
                return V8Value.Get(valueScope.Value);
            }
        }

        public static void V8Object_SetIndexedProperty(V8Object.Handle hObject, int index, object value)
        {
            using (var valueScope = V8Value.CreateScope(value))
            {
                Imports.V8Object_SetIndexedProperty(hObject, index, valueScope.Value);
            }
        }

        public static bool V8Object_DeleteIndexedProperty(V8Object.Handle hObject, int index)
        {
            return Imports.V8Object_DeleteIndexedProperty(hObject, index);
        }

        public static int[] V8Object_GetPropertyIndices(V8Object.Handle hObject)
        {
            using (var indicesScope = StdInt32Array.CreateScope())
            {
                Imports.V8Object_GetPropertyIndices(hObject, indicesScope.Value);
                return StdInt32Array.ToArray(indicesScope.Value);
            }
        }

        public static object V8Object_Invoke(V8Object.Handle hObject, bool asConstructor, object[] args)
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

        public static object V8Object_InvokeMethod(V8Object.Handle hObject, string name, object[] args)
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

        public static void V8Object_GetArrayBufferOrViewInfo(V8Object.Handle hObject, out IV8Object arrayBuffer, out ulong offset, out ulong size, out ulong length)
        {
            using (var arrayBufferScope = V8Value.CreateScope())
            {
                Imports.V8Object_GetArrayBufferOrViewInfo(hObject, arrayBufferScope.Value, out offset, out size, out length);
                arrayBuffer = (IV8Object)V8Value.Get(arrayBufferScope.Value);
            }
        }

        public static void V8Object_InvokeWithArrayBufferOrViewData(V8Object.Handle hObject, IntPtr pAction)
        {
            Imports.V8Object_InvokeWithArrayBufferOrViewData(hObject, pAction);
        }

        #endregion

        #region V8 debug callback methods

        public static void V8DebugCallback_ConnectClient(V8DebugCallback.Handle hCallback)
        {
            Imports.V8DebugCallback_ConnectClient(hCallback);
        }

        public static void V8DebugCallback_SendCommand(V8DebugCallback.Handle hCallback, string command)
        {
            using (var commandScope = StdString.CreateScope(command))
            {
                Imports.V8DebugCallback_SendCommand(hCallback, commandScope.Value);
            }
        }

        public static void V8DebugCallback_DisconnectClient(V8DebugCallback.Handle hCallback)
        {
            Imports.V8DebugCallback_DisconnectClient(hCallback);
        }

        #endregion

        #region native callback methods

        public static void NativeCallback_Invoke(NativeCallback.Handle hCallback)
        {
            Imports.NativeCallback_Invoke(hCallback);
        }

        #endregion

        #region V8 entity cleanup

        public static void V8Entity_Release(V8Entity.Handle hEntity)
        {
            Imports.V8Entity_Release(hEntity);
        }

        public static void V8Entity_DestroyHandle(V8Entity.Handle hEntity)
        {
            Imports.V8Entity_DestroyHandle(hEntity);
        }

        #endregion

        #region error handling

        public static void HostException_Schedule(string message, object exception)
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

        public static UIntPtr V8UnitTestSupport_GetTextDigest(string value)
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

        public static class Imports
        {
            public const string DllName = "ClearScriptV8";

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

            public static bool TryResolveLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchPath, out IntPtr nativeLibrary)
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

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr V8SplitProxyManaged_SetMethodTable(
                [In] IntPtr pMethodTable
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr V8SplitProxyNative_GetVersion();

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Environment_InitializeICU(
                [In] IntPtr pICUData,
                [In] uint size
            );

            #endregion

            #region StdString methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern StdString.Ptr StdString_New(
                [In][MarshalAs(UnmanagedType.LPWStr)] string value,
                [In] int length
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr StdString_GetValue(
                [In] StdString.Ptr pString,
                [Out] out int length
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdString_SetValue(
                [In] StdString.Ptr pString,
                [In][MarshalAs(UnmanagedType.LPWStr)] string value,
                [In] int length
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdString_Delete(
                [In] StdString.Ptr pString
            );

            #endregion

            #region StdStringArray methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern StdStringArray.Ptr StdStringArray_New(
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int StdStringArray_GetElementCount(
                [In] StdStringArray.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdStringArray_SetElementCount(
                [In] StdStringArray.Ptr pArray,
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr StdStringArray_GetElement(
                [In] StdStringArray.Ptr pArray,
                [In] int index,
                [Out] out int length
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdStringArray_SetElement(
                [In] StdStringArray.Ptr pArray,
                [In] int index,
                [In][MarshalAs(UnmanagedType.LPWStr)] string value,
                [In] int length
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdStringArray_Delete(
                [In] StdStringArray.Ptr pArray
            );

            #endregion

            #region StdByteArray methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern StdByteArray.Ptr StdByteArray_New(
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int StdByteArray_GetElementCount(
                [In] StdByteArray.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdByteArray_SetElementCount(
                [In] StdByteArray.Ptr pArray,
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr StdByteArray_GetData(
                [In] StdByteArray.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdByteArray_Delete(
                [In] StdByteArray.Ptr pArray
            );

            #endregion

            #region StdInt32Array methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern StdInt32Array.Ptr StdInt32Array_New(
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int StdInt32Array_GetElementCount(
                [In] StdInt32Array.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdInt32Array_SetElementCount(
                [In] StdInt32Array.Ptr pArray,
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr StdInt32Array_GetData(
                [In] StdInt32Array.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdInt32Array_Delete(
                [In] StdInt32Array.Ptr pArray
            );

            #endregion

            #region StdUInt32Array methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern StdUInt32Array.Ptr StdUInt32Array_New(
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int StdUInt32Array_GetElementCount(
                [In] StdUInt32Array.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdUInt32Array_SetElementCount(
                [In] StdUInt32Array.Ptr pArray,
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr StdUInt32Array_GetData(
                [In] StdUInt32Array.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdUInt32Array_Delete(
                [In] StdUInt32Array.Ptr pArray
            );

            #endregion

            #region StdUInt64Array methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern StdUInt64Array.Ptr StdUInt64Array_New(
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int StdUInt64Array_GetElementCount(
                [In] StdUInt64Array.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdUInt64Array_SetElementCount(
                [In] StdUInt64Array.Ptr pArray,
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr StdUInt64Array_GetData(
                [In] StdUInt64Array.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdUInt64Array_Delete(
                [In] StdUInt64Array.Ptr pArray
            );

            #endregion

            #region StdPtrArray methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern StdPtrArray.Ptr StdPtrArray_New(
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int StdPtrArray_GetElementCount(
                [In] StdPtrArray.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdPtrArray_SetElementCount(
                [In] StdPtrArray.Ptr pArray,
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr StdPtrArray_GetData(
                [In] StdPtrArray.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdPtrArray_Delete(
                [In] StdPtrArray.Ptr pArray
            );

            #endregion

            #region StdV8ValueArray methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern StdV8ValueArray.Ptr StdV8ValueArray_New(
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int StdV8ValueArray_GetElementCount(
                [In] StdV8ValueArray.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdV8ValueArray_SetElementCount(
                [In] StdV8ValueArray.Ptr pArray,
                [In] int elementCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8Value.Ptr StdV8ValueArray_GetData(
                [In] StdV8ValueArray.Ptr pArray
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void StdV8ValueArray_Delete(
                [In] StdV8ValueArray.Ptr pArray
            );

            #endregion

            #region V8Value methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8Value.Ptr V8Value_New();

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetNonexistent(
                [In] V8Value.Ptr pV8Value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetUndefined(
                [In] V8Value.Ptr pV8Value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetNull(
                [In] V8Value.Ptr pV8Value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetBoolean(
                [In] V8Value.Ptr pV8Value,
                [In][MarshalAs(UnmanagedType.I1)] bool value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetNumber(
                [In] V8Value.Ptr pV8Value,
                [In] double value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetInt32(
                [In] V8Value.Ptr pV8Value,
                [In] int value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetUInt32(
                [In] V8Value.Ptr pV8Value,
                [In] uint value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetString(
                [In] V8Value.Ptr pV8Value,
                [In][MarshalAs(UnmanagedType.LPWStr)] string value,
                [In] int length
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetDateTime(
                [In] V8Value.Ptr pV8Value,
                [In] double value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetBigInt(
                [In] V8Value.Ptr pV8Value,
                [In] int signBit,
                [In][MarshalAs(UnmanagedType.LPArray)] byte[] bytes,
                [In] int length
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetV8Object(
                [In] V8Value.Ptr pV8Value,
                [In] V8Object.Handle hObject,
                [In] V8Value.Subtype subtype,
                [In] V8Value.Flags flags
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_SetHostObject(
                [In] V8Value.Ptr pV8Value,
                [In] IntPtr pObject
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8Value.Type V8Value_Decode(
                [In] V8Value.Ptr pV8Value,
                [Out] out int intValue,
                [Out] out uint uintValue,
                [Out] out double doubleValue,
                [Out] out IntPtr ptrOrHandle
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Value_Delete(
                [In] V8Value.Ptr pV8Value
            );

            #endregion

            #region V8CpuProfile methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8CpuProfile_GetInfo(
                [In] V8CpuProfile.Ptr pProfile,
                [In] V8Entity.Handle hEntity,
                [In] StdString.Ptr pName,
                [Out] out ulong startTimestamp,
                [Out] out ulong endTimestamp,
                [Out] out int sampleCount,
                [Out] out V8CpuProfile.Node.Ptr pRootNode
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool V8CpuProfile_GetSample(
                [In] V8CpuProfile.Ptr pProfile,
                [In] int index,
                [Out] out ulong nodeId,
                [Out] out ulong timestamp
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8CpuProfileNode_GetInfo(
                [In] V8CpuProfile.Node.Ptr pNode,
                [In] V8Entity.Handle hEntity,
                [Out] out ulong nodeId,
                [Out] out long scriptId,
                [In] StdString.Ptr pScriptName,
                [In] StdString.Ptr pFunctionName,
                [In] StdString.Ptr pBailoutReason,
                [Out] out long lineNumber,
                [Out] out long columnNumber,
                [Out] out ulong hitCount,
                [Out] out uint hitLineCount,
                [Out] out int childCount
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool V8CpuProfileNode_GetHitLines(
                [In] V8CpuProfile.Node.Ptr pNode,
                [In] StdInt32Array.Ptr pLineNumbers,
                [In] StdUInt32Array.Ptr pHitCounts
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8CpuProfile.Node.Ptr V8CpuProfileNode_GetChildNode(
                [In] V8CpuProfile.Node.Ptr pNode,
                [In] int index
            );

            #endregion

            #region V8 isolate methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8Isolate.Handle V8Isolate_Create(
                [In] StdString.Ptr pName,
                [In] int maxNewSpaceSize,
                [In] int maxOldSpaceSize,
                [In] double heapExpansionMultiplier,
                [In] ulong maxArrayBufferAllocation,
                [In] V8RuntimeFlags flags,
                [In] int debugPort
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8Context.Handle V8Isolate_CreateContext(
                [In] V8Isolate.Handle hIsolate,
                [In] StdString.Ptr pName,
                [In] V8ScriptEngineFlags flags,
                [In] int debugPort
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern UIntPtr V8Isolate_GetMaxHeapSize(
                [In] V8Isolate.Handle hIsolate
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_SetMaxHeapSize(
                [In] V8Isolate.Handle hIsolate,
                [In] UIntPtr size
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern double V8Isolate_GetHeapSizeSampleInterval(
                [In] V8Isolate.Handle hIsolate
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_SetHeapSizeSampleInterval(
                [In] V8Isolate.Handle hIsolate,
                [In] double milliseconds
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern UIntPtr V8Isolate_GetMaxStackUsage(
                [In] V8Isolate.Handle hIsolate
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_SetMaxStackUsage(
                [In] V8Isolate.Handle hIsolate,
                [In] UIntPtr size
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_AwaitDebuggerAndPause(
                [In] V8Isolate.Handle hIsolate
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_CancelAwaitDebugger(
                [In] V8Isolate.Handle hIsolate
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8Script.Handle V8Isolate_Compile(
                [In] V8Isolate.Handle hIsolate,
                [In] StdString.Ptr pResourceName,
                [In] StdString.Ptr pSourceMapUrl,
                [In] ulong uniqueId,
                [In][MarshalAs(UnmanagedType.I1)] bool isModule,
                [In] IntPtr pDocumentInfo,
                [In] StdString.Ptr pCode
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8Script.Handle V8Isolate_CompileProducingCache(
                [In] V8Isolate.Handle hIsolate,
                [In] StdString.Ptr pResourceName,
                [In] StdString.Ptr pSourceMapUrl,
                [In] ulong uniqueId,
                [In][MarshalAs(UnmanagedType.I1)] bool isModule,
                [In] IntPtr pDocumentInfo,
                [In] StdString.Ptr pCode,
                [In] V8CacheKind cacheKind,
                [In] StdByteArray.Ptr pCacheBytes
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8Script.Handle V8Isolate_CompileConsumingCache(
                [In] V8Isolate.Handle hIsolate,
                [In] StdString.Ptr pResourceName,
                [In] StdString.Ptr pSourceMapUrl,
                [In] ulong uniqueId,
                [In][MarshalAs(UnmanagedType.I1)] bool isModule,
                [In] IntPtr pDocumentInfo,
                [In] StdString.Ptr pCode,
                [In] V8CacheKind cacheKind,
                [In] StdByteArray.Ptr pCacheBytes,
                [Out][MarshalAs(UnmanagedType.I1)] out bool cacheAccepted
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool V8Isolate_GetEnableInterruptPropagation(
                [In] V8Isolate.Handle hIsolate
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_SetEnableInterruptPropagation(
                [In] V8Isolate.Handle hIsolate,
                [In][MarshalAs(UnmanagedType.I1)] bool value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool V8Isolate_GetDisableHeapSizeViolationInterrupt(
                [In] V8Isolate.Handle hIsolate
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_SetDisableHeapSizeViolationInterrupt(
                [In] V8Isolate.Handle hIsolate,
                [In][MarshalAs(UnmanagedType.I1)] bool value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_GetHeapStatistics(
                [In] V8Isolate.Handle hIsolate,
                [Out] out ulong totalHeapSize,
                [Out] out ulong totalHeapSizeExecutable,
                [Out] out ulong totalPhysicalSize,
                [Out] out ulong totalAvailableSize,
                [Out] out ulong usedHeapSize,
                [Out] out ulong heapSizeLimit,
                [Out] out ulong totalExternalSize
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_GetStatistics(
                [In] V8Isolate.Handle hIsolate,
                [Out] out ulong scriptCount,
                [Out] out ulong scriptCacheSize,
                [Out] out ulong moduleCount,
                [In] StdUInt64Array.Ptr pPostedTaskCounts,
                [In] StdUInt64Array.Ptr pInvokedTaskCounts
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_CollectGarbage(
                [In] V8Isolate.Handle hIsolate,
                [In][MarshalAs(UnmanagedType.I1)] bool exhaustive
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool V8Isolate_BeginCpuProfile(
                [In] V8Isolate.Handle hIsolate,
                [In] StdString.Ptr pName,
                [In][MarshalAs(UnmanagedType.I1)] bool recordSamples
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_EndCpuProfile(
                [In] V8Isolate.Handle hIsolate,
                [In] StdString.Ptr pName,
                [In] IntPtr pAction
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_CollectCpuProfileSample(
                [In] V8Isolate.Handle hIsolate
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern uint V8Isolate_GetCpuProfileSampleInterval(
                [In] V8Isolate.Handle hIsolate
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_SetCpuProfileSampleInterval(
                [In] V8Isolate.Handle hIsolate,
                [In] uint value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Isolate_WriteHeapSnapshot(
                [In] V8Isolate.Handle hIsolate,
                [In] IntPtr pStream
            );

            #endregion

            #region V8 context methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern UIntPtr V8Context_GetMaxIsolateHeapSize(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_SetMaxIsolateHeapSize(
                [In] V8Context.Handle hContext,
                [In] UIntPtr size
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern double V8Context_GetIsolateHeapSizeSampleInterval(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_SetIsolateHeapSizeSampleInterval(
                [In] V8Context.Handle hContext,
                [In] double milliseconds
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern UIntPtr V8Context_GetMaxIsolateStackUsage(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_SetMaxIsolateStackUsage(
                [In] V8Context.Handle hContext,
                [In] UIntPtr size
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_InvokeWithLock(
                [In] V8Context.Handle hContext,
                [In] IntPtr pAction
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_GetRootItem(
                [In] V8Context.Handle hContext,
                [In] V8Value.Ptr pItem
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_AddGlobalItem(
                [In] V8Context.Handle hContext,
                [In] StdString.Ptr pName,
                [In] V8Value.Ptr pValue,
                [In][MarshalAs(UnmanagedType.I1)] bool globalMembers
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_AwaitDebuggerAndPause(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_CancelAwaitDebugger(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_ExecuteCode(
                [In] V8Context.Handle hContext,
                [In] StdString.Ptr pResourceName,
                [In] StdString.Ptr pSourceMapUrl,
                [In] ulong uniqueId,
                [In][MarshalAs(UnmanagedType.I1)] bool isModule,
                [In] IntPtr pDocumentInfo,
                [In] StdString.Ptr pCode,
                [In][MarshalAs(UnmanagedType.I1)] bool evaluate,
                [In] V8Value.Ptr pResult
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8Script.Handle V8Context_Compile(
                [In] V8Context.Handle hContext,
                [In] StdString.Ptr pResourceName,
                [In] StdString.Ptr pSourceMapUrl,
                [In] ulong uniqueId,
                [In][MarshalAs(UnmanagedType.I1)] bool isModule,
                [In] IntPtr pDocumentInfo,
                [In] StdString.Ptr pCode
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8Script.Handle V8Context_CompileProducingCache(
                [In] V8Context.Handle hContext,
                [In] StdString.Ptr pResourceName,
                [In] StdString.Ptr pSourceMapUrl,
                [In] ulong uniqueId,
                [In][MarshalAs(UnmanagedType.I1)] bool isModule,
                [In] IntPtr pDocumentInfo,
                [In] StdString.Ptr pCode,
                [In] V8CacheKind cacheKind,
                [In] StdByteArray.Ptr pCacheBytes
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern V8Script.Handle V8Context_CompileConsumingCache(
                [In] V8Context.Handle hContext,
                [In] StdString.Ptr pResourceName,
                [In] StdString.Ptr pSourceMapUrl,
                [In] ulong uniqueId,
                [In][MarshalAs(UnmanagedType.I1)] bool isModule,
                [In] IntPtr pDocumentInfo,
                [In] StdString.Ptr pCode,
                [In] V8CacheKind cacheKind,
                [In] StdByteArray.Ptr pCacheBytes,
                [Out][MarshalAs(UnmanagedType.I1)] out bool cacheAccepted
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_ExecuteScript(
                [In] V8Context.Handle hContext,
                [In] V8Script.Handle hScript,
                [In][MarshalAs(UnmanagedType.I1)] bool evaluate,
                [In] V8Value.Ptr pResult
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_Interrupt(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_CancelInterrupt(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool V8Context_GetEnableIsolateInterruptPropagation(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_SetEnableIsolateInterruptPropagation(
                [In] V8Context.Handle hContext,
                [In][MarshalAs(UnmanagedType.I1)] bool value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool V8Context_GetDisableIsolateHeapSizeViolationInterrupt(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_SetDisableIsolateHeapSizeViolationInterrupt(
                [In] V8Context.Handle hContext,
                [In][MarshalAs(UnmanagedType.I1)] bool value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_GetIsolateHeapStatistics(
                [In] V8Context.Handle hContext,
                [Out] out ulong totalHeapSize,
                [Out] out ulong totalHeapSizeExecutable,
                [Out] out ulong totalPhysicalSize,
                [Out] out ulong totalAvailableSize,
                [Out] out ulong usedHeapSize,
                [Out] out ulong heapSizeLimit,
                [Out] out ulong totalExternalSize
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_GetIsolateStatistics(
                [In] V8Context.Handle hContext,
                [Out] out ulong scriptCount,
                [Out] out ulong scriptCacheSize,
                [Out] out ulong moduleCount,
                [In] StdUInt64Array.Ptr pPostedTaskCounts,
                [In] StdUInt64Array.Ptr pInvokedTaskCounts
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_GetStatistics(
                [In] V8Context.Handle hContext,
                [Out] out ulong scriptCount,
                [Out] out ulong moduleCount,
                [Out] out ulong moduleCacheSize
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_CollectGarbage(
                [In] V8Context.Handle hContext,
                [In][MarshalAs(UnmanagedType.I1)] bool exhaustive
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_OnAccessSettingsChanged(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool V8Context_BeginCpuProfile(
                [In] V8Context.Handle hContext,
                [In] StdString.Ptr pName,
                [In][MarshalAs(UnmanagedType.I1)] bool recordSamples
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_EndCpuProfile(
                [In] V8Context.Handle hContext,
                [In] StdString.Ptr pName,
                [In] IntPtr pAction
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_CollectCpuProfileSample(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern uint V8Context_GetCpuProfileSampleInterval(
                [In] V8Context.Handle hContext
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_SetCpuProfileSampleInterval(
                [In] V8Context.Handle hContext,
                [In] uint value
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Context_WriteIsolateHeapSnapshot(
                [In] V8Context.Handle hContext,
                [In] IntPtr pStream
            );

            #endregion

            #region V8 object methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Object_GetNamedProperty(
                [In] V8Object.Handle hObject,
                [In] StdString.Ptr pName,
                [In] V8Value.Ptr pValue
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Object_SetNamedProperty(
                [In] V8Object.Handle hObject,
                [In] StdString.Ptr pName,
                [In] V8Value.Ptr pValue
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool V8Object_DeleteNamedProperty(
                [In] V8Object.Handle hObject,
                [In] StdString.Ptr pName
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Object_GetPropertyNames(
                [In] V8Object.Handle hObject,
                [In] StdStringArray.Ptr pNames
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Object_GetIndexedProperty(
                [In] V8Object.Handle hObject,
                [In] int index,
                [In] V8Value.Ptr pValue
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Object_SetIndexedProperty(
                [In] V8Object.Handle hObject,
                [In] int index,
                [In] V8Value.Ptr pValue
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool V8Object_DeleteIndexedProperty(
                [In] V8Object.Handle hObject,
                [In] int index
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Object_GetPropertyIndices(
                [In] V8Object.Handle hObject,
                [In] StdInt32Array.Ptr pIndices
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Object_Invoke(
                [In] V8Object.Handle hObject,
                [In][MarshalAs(UnmanagedType.I1)] bool asConstructor,
                [In] StdV8ValueArray.Ptr pArgs,
                [In] V8Value.Ptr pResult
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Object_InvokeMethod(
                [In] V8Object.Handle hObject,
                [In] StdString.Ptr pName,
                [In] StdV8ValueArray.Ptr pArgs,
                [In] V8Value.Ptr pResult
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Object_GetArrayBufferOrViewInfo(
                [In] V8Object.Handle hObject,
                [In] V8Value.Ptr pArrayBuffer,
                [Out] out ulong offset,
                [Out] out ulong size,
                [Out] out ulong length
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Object_InvokeWithArrayBufferOrViewData(
                [In] V8Object.Handle hObject,
                [In] IntPtr pAction
            );

            #endregion

            #region V8 debug callback methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8DebugCallback_ConnectClient(
                [In] V8DebugCallback.Handle hCallback
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8DebugCallback_SendCommand(
                [In] V8DebugCallback.Handle hCallback,
                [In] StdString.Ptr pCommand
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8DebugCallback_DisconnectClient(
                [In] V8DebugCallback.Handle hCallback
            );

            #endregion

            #region native callback methods

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void NativeCallback_Invoke(
                [In] NativeCallback.Handle hCallback
            );

            #endregion

            #region V8 entity cleanup

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Entity_Release(
                [In] V8Entity.Handle hEntity
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8Entity_DestroyHandle(
                [In] V8Entity.Handle hEntity
            );

            #endregion

            #region error handling

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void HostException_Schedule(
                [In] StdString.Ptr pMessage,
                [In] V8Value.Ptr pException
            );

            #endregion

            #region unit test support

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern UIntPtr V8UnitTestSupport_GetTextDigest(
                [In] StdString.Ptr pString
            );

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern void V8UnitTestSupport_GetStatistics(
                [Out] out ulong isolateCount,
                [Out] out ulong contextCount
            );

            #endregion

            #endregion
        }

        #endregion
    }
}

