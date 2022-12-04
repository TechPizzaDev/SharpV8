// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.IO;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.Util;
using Microsoft.ClearScript.V8.SplitProxy;

namespace Microsoft.ClearScript.V8
{
    internal sealed unsafe class V8ContextProxyImpl : V8ContextProxy
    {
        private V8EntityHolder holder;

        private V8ContextHandle Handle => (V8ContextHandle)holder.Handle;

        public V8ContextProxyImpl(V8IsolateProxy isolateProxy, string name, V8ScriptEngineFlags flags, int debugPort)
        {
            holder = new V8EntityHolder("V8 script engine", ((V8IsolateProxyImpl)isolateProxy).CreateContext(name, flags, debugPort));
        }

        #region V8ContextProxy overrides

        public override UIntPtr MaxIsolateHeapSize
        {
            get => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_GetMaxIsolateHeapSize(Handle));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_SetMaxIsolateHeapSize(Handle, value));
        }

        public override TimeSpan IsolateHeapSizeSampleInterval
        {
            get => V8SplitProxyNative.Invoke(() => TimeSpan.FromMilliseconds(V8SplitProxyNative.V8Context_GetIsolateHeapSizeSampleInterval(Handle)));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_SetIsolateHeapSizeSampleInterval(Handle, value.TotalMilliseconds));
        }

        public override UIntPtr MaxIsolateStackUsage
        {
            get => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_GetMaxIsolateStackUsage(Handle));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_SetMaxIsolateStackUsage(Handle, value));
        }

        public override void InvokeWithLock(Action action)
        {
            using (var actionScope = V8ProxyHelpers.CreateAddRefHostObjectScope(action))
            {
                var pAction = actionScope.Value;
                V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_InvokeWithLock(Handle, pAction));
            }
        }

        public override object GetRootItem()
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_GetRootItem(Handle));
        }

        public override void AddGlobalItem(ReadOnlySpan<char> name, object item, bool globalMembers)
        {
            AddGlobalItemAction action = new()
            {
                Handle = Handle,
                Name = &name,
                Item = item,
                GlobalMembers = globalMembers,
            };
            V8SplitProxyNative.InvokeThrowing(ref action);
        }

        private struct AddGlobalItemAction : IProxyAction
        {
            public V8ContextHandle Handle;
            public ReadOnlySpan<char>* Name;
            public object Item;
            public bool GlobalMembers;

            public void Invoke() => V8SplitProxyNative.V8Context_AddGlobalItem(Handle, *Name, Item, GlobalMembers);
        }

        public override void AwaitDebuggerAndPause()
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_AwaitDebuggerAndPause(Handle));
        }

        public override void CancelAwaitDebugger()
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_CancelAwaitDebugger(Handle));
        }

        public override object Execute(UniqueDocumentInfo documentInfo, ReadOnlySpan<char> code, bool evaluate)
        {
            ExecuteCodeAction action = new()
            {
                Handle = Handle,
                DocumentInfo = documentInfo,
                Code = &code,
                Evaluate = evaluate,
            };
            V8SplitProxyNative.InvokeThrowing(ref action);
            return action.Result;
        }

        private struct ExecuteCodeAction : IProxyAction
        {
            public V8ContextHandle Handle;
            public UniqueDocumentInfo DocumentInfo;
            public ReadOnlySpan<char>* Code;
            public bool Evaluate;
            public object Result;

            public void Invoke()
            {
                Result = V8SplitProxyNative.V8Context_ExecuteCode(
                    Handle,
                    MiscHelpers.GetUrlOrPath(DocumentInfo.Uri, DocumentInfo.UniqueName),
                    MiscHelpers.GetUrlOrPath(DocumentInfo.SourceMapUri, string.Empty),
                    DocumentInfo.UniqueId,
                    DocumentInfo.Category == ModuleCategory.Standard,
                    V8ProxyHelpers.AddRefHostObject(DocumentInfo),
                    *Code,
                    Evaluate);
            }
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
            public V8ContextHandle Handle;
            public UniqueDocumentInfo DocumentInfo;
            public ReadOnlySpan<char>* Code;

            public V8ScriptHandle Result;

            public void Invoke()
            {
                Result = V8SplitProxyNative.V8Context_Compile(
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
            public V8ContextHandle Handle;
            public UniqueDocumentInfo DocumentInfo;
            public ReadOnlySpan<char>* Code;
            public V8CacheKind CacheKind;

            public V8ScriptHandle Result;
            public byte[] CacheBytes;

            public void Invoke()
            {
                Result = V8SplitProxyNative.V8Context_CompileProducingCache(
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
            public V8ContextHandle Handle;
            public UniqueDocumentInfo DocumentInfo;
            public ReadOnlySpan<char>* Code;
            public V8CacheKind CacheKind;
            public ReadOnlySpan<byte>* CacheBytes;

            public V8ScriptHandle Result;
            public bool CacheAccepted;

            public void Invoke()
            {
                Result = V8SplitProxyNative.V8Context_CompileConsumingCache(
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

        public override object Execute(V8Script script, bool evaluate)
        {
            if (script is V8ScriptImpl scriptImpl)
            {
                ExecuteScriptAction action = new()
                {
                    Handle = Handle,
                    Script = scriptImpl.Handle,
                    Evaluate = evaluate,
                };
                V8SplitProxyNative.InvokeThrowing(ref action);
                return action.Result;
            }
            throw new ArgumentException("Invalid compiled script", nameof(script));
        }

        private struct ExecuteScriptAction : IProxyAction
        {
            public V8ContextHandle Handle;
            public V8ScriptHandle Script;
            public bool Evaluate;
            public object Result;

            public void Invoke()
            {
                Result = V8SplitProxyNative.V8Context_ExecuteScript(
                    Handle,
                    Script,
                    Evaluate);
            }
        }

        public override void Interrupt()
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_Interrupt(Handle));
        }

        public override void CancelInterrupt()
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_CancelInterrupt(Handle));
        }

        public override bool EnableIsolateInterruptPropagation
        {
            get => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_GetEnableIsolateInterruptPropagation(Handle));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_SetEnableIsolateInterruptPropagation(Handle, value));
        }

        public override bool DisableIsolateHeapSizeViolationInterrupt
        {
            get => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_GetDisableIsolateHeapSizeViolationInterrupt(Handle));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_SetDisableIsolateHeapSizeViolationInterrupt(Handle, value));
        }

        public override V8RuntimeHeapInfo GetIsolateHeapInfo()
        {
            var totalHeapSize = 0UL;
            var totalHeapSizeExecutable = 0UL;
            var totalPhysicalSize = 0UL;
            var totalAvailableSize = 0UL;
            var usedHeapSize = 0UL;
            var heapSizeLimit = 0UL;
            var totalExternalSize = 0UL;
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_GetIsolateHeapStatistics(Handle, out totalHeapSize, out totalHeapSizeExecutable, out totalPhysicalSize, out totalAvailableSize, out usedHeapSize, out heapSizeLimit, out totalExternalSize));

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

        public override V8Runtime.Statistics GetIsolateStatistics()
        {
            var statistics = new V8Runtime.Statistics();
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_GetIsolateStatistics(Handle, out statistics.ScriptCount, out statistics.ScriptCacheSize, out statistics.ModuleCount, out statistics.PostedTaskCounts, out statistics.InvokedTaskCounts));
            return statistics;
        }

        public override V8ScriptEngine.Statistics GetStatistics()
        {
            var statistics = new V8ScriptEngine.Statistics();
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_GetStatistics(Handle, out statistics.ScriptCount, out statistics.ModuleCount, out statistics.ModuleCacheSize));
            return statistics;
        }

        public override void CollectGarbage(bool exhaustive)
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_CollectGarbage(Handle, exhaustive));
        }

        public override void OnAccessSettingsChanged()
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_OnAccessSettingsChanged(Handle));
        }

        public override bool BeginCpuProfile(string name, V8CpuProfileFlags flags)
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_BeginCpuProfile(Handle, name, flags.HasFlag(V8CpuProfileFlags.EnableSampleCollection)));
        }

        public override V8CpuProfile EndCpuProfile(string name)
        {
            var profile = new V8CpuProfile();

            Action<V8CpuProfilePtr> action = pProfile => V8CpuProfileImpl.ProcessProfile(Handle, pProfile, profile);
            using (var actionScope = V8ProxyHelpers.CreateAddRefHostObjectScope(action))
            {
                var pAction = actionScope.Value;
                V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_EndCpuProfile(Handle, name, pAction));
            }

            return profile;
        }

        public override void CollectCpuProfileSample()
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_CollectCpuProfileSample(Handle));
        }

        public override uint CpuProfileSampleInterval
        {
            get => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_GetCpuProfileSampleInterval(Handle));
            set => V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_SetCpuProfileSampleInterval(Handle, value));
        }

        public override void WriteIsolateHeapSnapshot(Stream stream)
        {
            using (var streamScope = V8ProxyHelpers.CreateAddRefHostObjectScope(stream))
            {
                var pStream = streamScope.Value;
                V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Context_WriteIsolateHeapSnapshot(Handle, pStream));
            }
        }

        #endregion

        #region disposal / finalization

        public override void Dispose()
        {
            holder.ReleaseEntity();
            GC.KeepAlive(this);
        }

        ~V8ContextProxyImpl()
        {
            V8EntityHolder.Destroy(ref holder);
        }

        #endregion
    }

    public readonly struct V8ContextHandle
    {
        private readonly IntPtr guts;

        private V8ContextHandle(IntPtr guts) => this.guts = guts;

        public static V8ContextHandle Empty => new(IntPtr.Zero);

        public static bool operator ==(V8ContextHandle left, V8ContextHandle right) => left.guts == right.guts;
        public static bool operator !=(V8ContextHandle left, V8ContextHandle right) => left.guts != right.guts;

        public static explicit operator IntPtr(V8ContextHandle handle) => handle.guts;
        public static explicit operator V8ContextHandle(IntPtr guts) => new(guts);

        public static implicit operator V8Entity(V8ContextHandle handle) => (V8Entity)handle.guts;
        public static explicit operator V8ContextHandle(V8Entity handle) => (V8ContextHandle)(IntPtr)handle;

        public override bool Equals(object? obj) => obj is V8ContextHandle handle && this == handle;
        public override int GetHashCode() => guts.GetHashCode();
    }
}
