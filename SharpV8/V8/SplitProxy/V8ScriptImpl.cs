// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Microsoft.ClearScript.V8
{
    internal sealed class V8ScriptImpl : V8Script
    {
        private V8EntityHolder holder;

        public V8ScriptHandle Handle => (V8ScriptHandle)holder.Handle;

        public V8ScriptImpl(UniqueDocumentInfo documentInfo, UIntPtr codeDigest, V8ScriptHandle hScript)
            : base(documentInfo, codeDigest)
        {
            holder = new V8EntityHolder("V8 compiled script", hScript);
        }

        #region disposal / finalization

        public override void Dispose()
        {
            holder.ReleaseEntity();
            GC.KeepAlive(this);
        }

        ~V8ScriptImpl()
        {
            V8EntityHolder.Destroy(ref holder);
        }

        #endregion
    }

    public readonly struct V8ScriptHandle
    {
        private readonly IntPtr guts;

        private V8ScriptHandle(IntPtr guts) => this.guts = guts;

        public static readonly V8ScriptHandle Empty = new(IntPtr.Zero);

        public static bool operator ==(V8ScriptHandle left, V8ScriptHandle right) => left.guts == right.guts;
        public static bool operator !=(V8ScriptHandle left, V8ScriptHandle right) => left.guts != right.guts;

        public static explicit operator IntPtr(V8ScriptHandle handle) => handle.guts;
        public static explicit operator V8ScriptHandle(IntPtr guts) => new(guts);

        public static implicit operator V8Entity(V8ScriptHandle handle) => (V8Entity)handle.guts;
        public static explicit operator V8ScriptHandle(V8Entity handle) => (V8ScriptHandle)(IntPtr)handle;

        public override bool Equals(object? obj) => obj is V8ScriptHandle handle && this == handle;
        public override int GetHashCode() => guts.GetHashCode();
    }
}