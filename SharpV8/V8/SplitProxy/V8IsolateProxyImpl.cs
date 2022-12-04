// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.IO;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.Util;
using Microsoft.ClearScript.V8.SplitProxy;

namespace Microsoft.ClearScript.V8
{
    internal sealed unsafe class V8IsolateProxyImpl : V8IsolateProxy
    {
        private V8EntityHolder holder;

        private V8IsolateHandle Handle => (V8IsolateHandle)holder.Handle;

        public V8IsolateProxyImpl(string name, V8RuntimeConstraints constraints, V8RuntimeFlags flags, int debugPort)
        {
            holder = new V8EntityHolder("V8 runtime", V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_Create(
                name,
                constraints?.MaxNewSpaceSize ?? -1,
                constraints?.MaxOldSpaceSize ?? -1,
                constraints?.HeapExpansionMultiplier ?? 0,
                constraints?.MaxArrayBufferAllocation ?? ulong.MaxValue,
                flags,
                debugPort
            )));
        }

        public V8ContextHandle CreateContext(string name, V8ScriptEngineFlags flags, int debugPort)
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_CreateContext(
                Handle,
                name,
                flags,
                debugPort
            ));
        }

        #region V8IsolateProxy overrides

        public override UIntPtr MaxHeapSize
        {
            get => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_GetMaxHeapSize(Handle));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_SetMaxHeapSize(Handle, value));
        }

        public override TimeSpan HeapSizeSampleInterval
        {
            get => V8SplitProxyNative.Invoke(() => TimeSpan.FromMilliseconds(V8SplitProxyNative.V8Isolate_GetHeapSizeSampleInterval(Handle)));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_SetHeapSizeSampleInterval(Handle, value.TotalMilliseconds));
        }

        public override UIntPtr MaxStackUsage
        {
            get => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_GetMaxStackUsage(Handle));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_SetMaxStackUsage(Handle, value));
        }

        public override void AwaitDebuggerAndPause()
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_AwaitDebuggerAndPause(Handle));
        }

        public override void CancelAwaitDebugger()
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_CancelAwaitDebugger(Handle));
        }

        public override V8Script Compile(UniqueDocumentInfo documentInfo, ReadOnlySpan<char> code)
        {
            CompileAction action = new()
            {
                Handle = Handle,
                DocumentInfo = documentInfo,
                Code = &code,
            };
            V8SplitProxyNative.InvokeThrowing(ref action);
            return new V8ScriptImpl(documentInfo, code.GetDigest(), action.Result);
        }

        private struct CompileAction : IProxyAction
        {
            public V8IsolateHandle Handle;
            public UniqueDocumentInfo DocumentInfo;
            public ReadOnlySpan<char>* Code;

            public V8ScriptHandle Result;

            public void Invoke()
            {
                Result = V8SplitProxyNative.V8Isolate_Compile(
                    Handle,
                    MiscHelpers.GetUrlOrPath(DocumentInfo.Uri, DocumentInfo.UniqueName),
                    MiscHelpers.GetUrlOrPath(DocumentInfo.SourceMapUri, string.Empty),
                    DocumentInfo.UniqueId,
                    DocumentInfo.Category == ModuleCategory.Standard,
                    V8ProxyHelpers.AddRefHostObject(DocumentInfo),
                    *Code
                );
            }
        }

        public override V8Script Compile(
            UniqueDocumentInfo documentInfo,
            ReadOnlySpan<char> code,
            V8CacheKind cacheKind,
            out byte[] cacheBytes)
        {
            if (cacheKind == V8CacheKind.None)
            {
                cacheBytes = null;
                return Compile(documentInfo, code);
            }

            CompileProducingCacheAction action = new()
            {
                Handle = Handle,
                DocumentInfo = documentInfo,
                Code = &code,
                CacheKind = cacheKind,
            };
            V8SplitProxyNative.InvokeThrowing(ref action);
            var script = new V8ScriptImpl(documentInfo, code.GetDigest(), action.Result);

            cacheBytes = action.CacheBytes;
            return script;
        }

        private struct CompileProducingCacheAction : IProxyAction
        {
            public V8IsolateHandle Handle;
            public UniqueDocumentInfo DocumentInfo;
            public ReadOnlySpan<char>* Code;
            public V8CacheKind CacheKind;

            public V8ScriptHandle Result;
            public byte[] CacheBytes;

            public void Invoke()
            {
                Result = V8SplitProxyNative.V8Isolate_CompileProducingCache(
                    Handle,
                    MiscHelpers.GetUrlOrPath(DocumentInfo.Uri, DocumentInfo.UniqueName),
                    MiscHelpers.GetUrlOrPath(DocumentInfo.SourceMapUri, string.Empty),
                    DocumentInfo.UniqueId,
                    DocumentInfo.Category == ModuleCategory.Standard,
                    V8ProxyHelpers.AddRefHostObject(DocumentInfo),
                    *Code,
                    CacheKind,
                    out CacheBytes
                );
            }
        }

        public override V8Script Compile(
            UniqueDocumentInfo documentInfo,
            ReadOnlySpan<char> code,
            V8CacheKind cacheKind,
            ReadOnlySpan<byte> cacheBytes,
            out bool cacheAccepted)
        {
            if (cacheKind == V8CacheKind.None || cacheBytes.IsEmpty)
            {
                cacheAccepted = false;
                return Compile(documentInfo, code);
            }

            CompileConsumingCacheAction action = new()
            {
                Handle = Handle,
                DocumentInfo = documentInfo,
                Code = &code,
                CacheKind = cacheKind,
                CacheBytes = &cacheBytes,
            };
            V8SplitProxyNative.InvokeThrowing(ref action);
            var script = new V8ScriptImpl(documentInfo, code.GetDigest(), action.Result);

            cacheAccepted = action.CacheAccepted;
            return script;
        }

        private struct CompileConsumingCacheAction : IProxyAction
        {
            public V8IsolateHandle Handle;
            public UniqueDocumentInfo DocumentInfo;
            public ReadOnlySpan<char>* Code;
            public V8CacheKind CacheKind;
            public ReadOnlySpan<byte>* CacheBytes;

            public V8ScriptHandle Result;
            public bool CacheAccepted;

            public void Invoke()
            {
                Result = V8SplitProxyNative.V8Isolate_CompileConsumingCache(
                    Handle,
                    MiscHelpers.GetUrlOrPath(DocumentInfo.Uri, DocumentInfo.UniqueName),
                    MiscHelpers.GetUrlOrPath(DocumentInfo.SourceMapUri, string.Empty),
                    DocumentInfo.UniqueId,
                    DocumentInfo.Category == ModuleCategory.Standard,
                    V8ProxyHelpers.AddRefHostObject(DocumentInfo),
                    *Code,
                    CacheKind,
                    *CacheBytes,
                    out CacheAccepted);
            }
        }

        public override bool EnableInterruptPropagation
        {
            get => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_GetEnableInterruptPropagation(Handle));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_SetEnableInterruptPropagation(Handle, value));
        }

        public override bool DisableHeapSizeViolationInterrupt
        {
            get => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_GetDisableHeapSizeViolationInterrupt(Handle));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_SetDisableHeapSizeViolationInterrupt(Handle, value));
        }

        public override V8RuntimeHeapInfo GetHeapInfo()
        {
            var totalHeapSize = 0UL;
            var totalHeapSizeExecutable = 0UL;
            var totalPhysicalSize = 0UL;
            var totalAvailableSize = 0UL;
            var usedHeapSize = 0UL;
            var heapSizeLimit = 0UL;
            var totalExternalSize = 0UL;
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_GetHeapStatistics(Handle, out totalHeapSize, out totalHeapSizeExecutable, out totalPhysicalSize, out totalAvailableSize, out usedHeapSize, out heapSizeLimit, out totalExternalSize));

            return new V8RuntimeHeapInfo
            {
                TotalHeapSize = totalHeapSize,
                TotalHeapSizeExecutable = totalHeapSizeExecutable,
                TotalPhysicalSize = totalPhysicalSize,
                TotalAvailableSize = totalAvailableSize,
                UsedHeapSize = usedHeapSize,
                HeapSizeLimit = heapSizeLimit,
                TotalExternalSize = totalExternalSize
            };
        }

        public override V8Runtime.Statistics GetStatistics()
        {
            var statistics = new V8Runtime.Statistics();
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_GetStatistics(Handle, out statistics.ScriptCount, out statistics.ScriptCacheSize, out statistics.ModuleCount, out statistics.PostedTaskCounts, out statistics.InvokedTaskCounts));
            return statistics;
        }

        public override void CollectGarbage(bool exhaustive)
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_CollectGarbage(Handle, exhaustive));
        }

        public override bool BeginCpuProfile(string name, V8CpuProfileFlags flags)
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_BeginCpuProfile(Handle, name, flags.HasFlag(V8CpuProfileFlags.EnableSampleCollection)));
        }

        public override V8CpuProfile EndCpuProfile(string name)
        {
            var profile = new V8CpuProfile();

            Action<V8CpuProfilePtr> action = pProfile => V8CpuProfileImpl.ProcessProfile(Handle, pProfile, profile);
            using (var actionScope = V8ProxyHelpers.CreateAddRefHostObjectScope(action))
            {
                var pAction = actionScope.Value;
                V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_EndCpuProfile(Handle, name, pAction));
            }

            return profile;
        }

        public override void CollectCpuProfileSample()
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_CollectCpuProfileSample(Handle));
        }

        public override uint CpuProfileSampleInterval
        {
            get => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_GetCpuProfileSampleInterval(Handle));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_SetCpuProfileSampleInterval(Handle, value));
        }

        public override void WriteHeapSnapshot(Stream stream)
        {
            using (var streamScope = V8ProxyHelpers.CreateAddRefHostObjectScope(stream))
            {
                var pStream = streamScope.Value;
                V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Isolate_WriteHeapSnapshot(Handle, pStream));
            }
        }

        #endregion

        #region disposal / finalization

        public override void Dispose()
        {
            holder.ReleaseEntity();
            GC.KeepAlive(this);
        }

        ~V8IsolateProxyImpl()
        {
            V8EntityHolder.Destroy(ref holder);
        }

        #endregion
    }

    public readonly struct V8IsolateHandle
    {
        private readonly IntPtr guts;

        private V8IsolateHandle(IntPtr guts) => this.guts = guts;

        public static readonly V8IsolateHandle Empty = new(IntPtr.Zero);

        public static bool operator ==(V8IsolateHandle left, V8IsolateHandle right) => left.guts == right.guts;
        public static bool operator !=(V8IsolateHandle left, V8IsolateHandle right) => left.guts != right.guts;

        public static explicit operator IntPtr(V8IsolateHandle handle) => handle.guts;
        public static explicit operator V8IsolateHandle(IntPtr guts) => new(guts);

        public static implicit operator V8Entity(V8IsolateHandle handle) => (V8Entity)handle.guts;
        public static explicit operator V8IsolateHandle(V8Entity handle) => (V8IsolateHandle)(IntPtr)handle;

        public override bool Equals(object? obj) => obj is V8IsolateHandle handle && this == handle;
        public override int GetHashCode() => guts.GetHashCode();
    }
}
