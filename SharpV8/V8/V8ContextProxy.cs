// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.IO;

namespace Microsoft.ClearScript.V8
{
    internal abstract unsafe class V8ContextProxy : V8Proxy
    {
        public static V8ContextProxyImpl Create(V8IsolateProxy isolateProxy, string name, V8ScriptEngineFlags flags, int debugPort)
        {
            return new V8ContextProxyImpl(isolateProxy, name, flags, debugPort);
        }

        public abstract UIntPtr MaxIsolateHeapSize { get; set; }

        public abstract TimeSpan IsolateHeapSizeSampleInterval { get; set; }

        public abstract UIntPtr MaxIsolateStackUsage { get; set; }

        public abstract void InvokeWithLock(delegate* unmanaged[Stdcall]<void*, void> action, void* state);

        public abstract object GetRootItem();

        public abstract void AddGlobalItem(ReadOnlySpan<char> name, object item, bool globalMembers);

        public abstract void AwaitDebuggerAndPause();

        public abstract void CancelAwaitDebugger();

        public abstract object Execute(UniqueDocumentInfo documentInfo, ReadOnlySpan<char> code, bool evaluate);

        public abstract V8Script Compile(UniqueDocumentInfo documentInfo, ReadOnlySpan<char> code);

        public abstract V8Script Compile(
            UniqueDocumentInfo documentInfo, 
            ReadOnlySpan<char> code, 
            V8CacheKind cacheKind,
            out byte[] cacheBytes);

        public abstract V8Script Compile(
            UniqueDocumentInfo documentInfo,
            ReadOnlySpan<char> code, 
            V8CacheKind cacheKind,
            ReadOnlySpan<byte> cacheBytes, 
            out bool cacheAccepted);

        public abstract object Execute(V8Script script, bool evaluate);

        public abstract void Interrupt();

        public abstract void CancelInterrupt();

        public abstract bool EnableIsolateInterruptPropagation { get; set; }

        public abstract bool DisableIsolateHeapSizeViolationInterrupt { get; set; }

        public abstract V8RuntimeHeapInfo GetIsolateHeapInfo();

        public abstract V8Runtime.Statistics GetIsolateStatistics();

        public abstract V8ScriptEngine.Statistics GetStatistics();

        public abstract void CollectGarbage(bool exhaustive);

        public abstract void OnAccessSettingsChanged();

        public abstract bool BeginCpuProfile(string name, V8CpuProfileFlags flags);

        public abstract V8CpuProfile EndCpuProfile(string name);

        public abstract void CollectCpuProfileSample();

        public abstract uint CpuProfileSampleInterval { get; set; }

        public abstract void WriteIsolateHeapSnapshot(Stream stream);
    }
}
