// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.ClearScript.V8.SplitProxy;

namespace Microsoft.ClearScript.V8
{
    internal sealed class NativeCallback : IDisposable
    {
        private V8EntityHolder holder;

        private NativeCallbackHandle Handle => (NativeCallbackHandle)holder.Handle;

        public NativeCallback(NativeCallbackHandle hCallback)
        {
            holder = new V8EntityHolder("native callback", hCallback);
        }

        #region INativeCallback implementation

        public void Invoke()
        {
            V8SplitProxyNative.InvokeNoThrow(() => V8SplitProxyNative.NativeCallback_Invoke(Handle));
        }

        #endregion

        #region disposal / finalization

        public void Dispose()
        {
            holder.ReleaseEntity();
            GC.KeepAlive(this);
        }

        ~NativeCallback()
        {
            V8EntityHolder.Destroy(ref holder);
        }

        #endregion
    }

    public readonly struct NativeCallbackHandle
    {
        private readonly IntPtr guts;

        private NativeCallbackHandle(IntPtr guts) => this.guts = guts;

        public static readonly NativeCallbackHandle Empty = new(IntPtr.Zero);

        public static bool operator ==(NativeCallbackHandle left, NativeCallbackHandle right) => left.guts == right.guts;
        public static bool operator !=(NativeCallbackHandle left, NativeCallbackHandle right) => left.guts != right.guts;

        public static explicit operator IntPtr(NativeCallbackHandle handle) => handle.guts;
        public static explicit operator NativeCallbackHandle(IntPtr guts) => new(guts);

        public static implicit operator V8Entity(NativeCallbackHandle handle) => (V8Entity)handle.guts;
        public static explicit operator NativeCallbackHandle(V8Entity handle) => (NativeCallbackHandle)(IntPtr)handle;

        public override bool Equals(object? obj) => obj is NativeCallbackHandle handle && this == handle;
        public override int GetHashCode() => guts.GetHashCode();
    }
}
