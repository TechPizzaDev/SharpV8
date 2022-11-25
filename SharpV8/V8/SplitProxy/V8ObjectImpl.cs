// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.ClearScript.V8.SplitProxy;

namespace Microsoft.ClearScript.V8
{
    public sealed class V8Object
    {
        private V8EntityHolder holder;

        public V8ObjectHandle Handle => (V8ObjectHandle)holder.Handle;

        public V8Object(V8ObjectHandle hObject, V8ValueSubtype subtype, V8ValueFlags flags, int identityHash)
        {
            holder = new V8EntityHolder("V8 object", hObject);
            Subtype = subtype;
            Flags = flags;
            IdentityHash = identityHash;
        }

        public V8ValueSubtype Subtype { get; }

        public V8ValueFlags Flags { get; }

        #region IV8Object implementation

        public int IdentityHash { get; }

        public object GetProperty(string name)
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_GetNamedProperty(Handle, name));
        }

        public void SetProperty(string name, object value)
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_SetNamedProperty(Handle, name, value));
        }

        public bool DeleteProperty(string name)
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_DeleteNamedProperty(Handle, name));
        }

        public string[] GetPropertyNames()
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_GetPropertyNames(Handle));
        }

        public object GetProperty(int index)
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_GetIndexedProperty(Handle, index));
        }

        public void SetProperty(int index, object value)
        {
            V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_SetIndexedProperty(Handle, index, value));
        }

        public bool DeleteProperty(int index)
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_DeleteIndexedProperty(Handle, index));
        }

        public int[] GetPropertyIndices()
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_GetPropertyIndices(Handle));
        }

        public object Invoke(bool asConstructor, object[] args)
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_Invoke(Handle, asConstructor, args));
        }

        public object InvokeMethod(string name, object[] args)
        {
            return V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_InvokeMethod(Handle, name, args));
        }

        public bool IsPromise => Subtype == V8ValueSubtype.Promise;

        public bool IsArray => Subtype == V8ValueSubtype.Array;

        public bool IsShared => Flags.HasFlag(V8ValueFlags.Shared);

        public bool IsArrayBufferOrView
        {
            get
            {
                switch (Subtype)
                {
                    case V8ValueSubtype.ArrayBuffer:
                    case V8ValueSubtype.DataView:
                    case V8ValueSubtype.Uint8Array:
                    case V8ValueSubtype.Uint8ClampedArray:
                    case V8ValueSubtype.Int8Array:
                    case V8ValueSubtype.Uint16Array:
                    case V8ValueSubtype.Int16Array:
                    case V8ValueSubtype.Uint32Array:
                    case V8ValueSubtype.Int32Array:
                    case V8ValueSubtype.BigUint64Array:
                    case V8ValueSubtype.BigInt64Array:
                    case V8ValueSubtype.Float32Array:
                    case V8ValueSubtype.Float64Array:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public V8ArrayBufferOrViewKind GetArrayBufferOrViewKind()
        {
            var kind = V8ArrayBufferOrViewKind.None;

            if (Subtype == V8ValueSubtype.ArrayBuffer)
                kind = V8ArrayBufferOrViewKind.ArrayBuffer;
            else if (Subtype == V8ValueSubtype.DataView)
                kind = V8ArrayBufferOrViewKind.DataView;
            else if (Subtype == V8ValueSubtype.Uint8Array)
                kind = V8ArrayBufferOrViewKind.Uint8Array;
            else if (Subtype == V8ValueSubtype.Uint8ClampedArray)
                kind = V8ArrayBufferOrViewKind.Uint8ClampedArray;
            else if (Subtype == V8ValueSubtype.Int8Array)
                kind = V8ArrayBufferOrViewKind.Int8Array;
            else if (Subtype == V8ValueSubtype.Uint16Array)
                kind = V8ArrayBufferOrViewKind.Uint16Array;
            else if (Subtype == V8ValueSubtype.Int16Array)
                kind = V8ArrayBufferOrViewKind.Int16Array;
            else if (Subtype == V8ValueSubtype.Uint32Array)
                kind = V8ArrayBufferOrViewKind.Uint32Array;
            else if (Subtype == V8ValueSubtype.Int32Array)
                kind = V8ArrayBufferOrViewKind.Int32Array;
            else if (Subtype == V8ValueSubtype.BigUint64Array)
                kind = V8ArrayBufferOrViewKind.BigUint64Array;
            else if (Subtype == V8ValueSubtype.BigInt64Array)
                kind = V8ArrayBufferOrViewKind.BigInt64Array;
            else if (Subtype == V8ValueSubtype.Float32Array)
                kind = V8ArrayBufferOrViewKind.Float32Array;
            else if (Subtype == V8ValueSubtype.Float64Array)
                kind = V8ArrayBufferOrViewKind.Float64Array;

            return kind;
        }

        public V8ArrayBufferOrViewInfo GetArrayBufferOrViewInfo()
        {
            var kind = GetArrayBufferOrViewKind();
            if (kind != V8ArrayBufferOrViewKind.None)
            {
                V8Object arrayBuffer = null;
                var offset = 0UL;
                var size = 0UL;
                var length = 0UL;
                V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_GetArrayBufferOrViewInfo(Handle, out arrayBuffer, out offset, out size, out length));
                return new V8ArrayBufferOrViewInfo(kind, arrayBuffer, offset, size, length);
            }

            return default;
        }

        public void InvokeWithArrayBufferOrViewData(Action<IntPtr> action)
        {
            using (var actionScope = V8ProxyHelpers.CreateAddRefHostObjectScope(action))
            {
                var pAction = actionScope.Value;
                V8SplitProxyNative.Invoke(() => V8SplitProxyNative.V8Object_InvokeWithArrayBufferOrViewData(Handle, pAction));
            }
        }

        #endregion

        #region disposal / finalization

        public void Dispose()
        {
            holder.ReleaseEntity();
            GC.KeepAlive(this);
        }

        ~V8Object()
        {
            V8EntityHolder.Destroy(ref holder);
        }

        #endregion
    }

    public readonly struct V8ObjectHandle
    {
        private readonly IntPtr guts;

        private V8ObjectHandle(IntPtr guts) => this.guts = guts;

        public static V8ObjectHandle Empty => new(IntPtr.Zero);

        public static bool operator ==(V8ObjectHandle left, V8ObjectHandle right) => left.guts == right.guts;
        public static bool operator !=(V8ObjectHandle left, V8ObjectHandle right) => left.guts != right.guts;

        public static explicit operator IntPtr(V8ObjectHandle handle) => handle.guts;
        public static explicit operator V8ObjectHandle(IntPtr guts) => new(guts);

        public static implicit operator V8Entity(V8ObjectHandle handle) => (V8Entity)handle.guts;
        public static explicit operator V8ObjectHandle(V8Entity handle) => (V8ObjectHandle)(IntPtr)handle;

        public override bool Equals(object? obj) => obj is V8ObjectHandle handle && this == handle;
        public override int GetHashCode() => guts.GetHashCode();
    }
}
