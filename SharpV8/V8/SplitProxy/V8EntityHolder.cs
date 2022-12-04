// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.ClearScript.V8.SplitProxy;

namespace Microsoft.ClearScript.V8
{
    internal readonly struct V8EntityHolder
    {
        private readonly string name;
        private readonly V8Entity handle;

        public V8Entity Handle => handle != V8Entity.Empty
            ? handle
            : throw new InvalidOperationException("The " + name + " proxy has been destroyed");

        public V8EntityHolder(string name, V8Entity handle)
        {
            this.name = name;
            this.handle = handle;
        }

        private V8EntityHolder(string name)
        {
            this.name = name;
            handle = V8Entity.Empty;
        }

        public void ReleaseEntity()
        {
            var tempHandle = handle;
            if (tempHandle != V8Entity.Empty)
            {
                ReleaseAction action = new() { Handle = tempHandle };
                V8SplitProxyNative.Invoke(ref action);
            }
        }

        private struct ReleaseAction : IProxyAction
        {
            public V8Entity Handle;

            public void Invoke() => V8SplitProxyNative.V8Entity_Release(Handle);
        }

        public static void Destroy(ref V8EntityHolder holder)
        {
            var tempHandle = holder.handle;
            if (tempHandle != V8Entity.Empty)
            {
                DestroyAction action = new() { Handle = tempHandle };
                V8SplitProxyNative.Invoke(ref action);
            }

            holder = new V8EntityHolder(holder.name);
        }

        private struct DestroyAction : IProxyAction
        {
            public V8Entity Handle;

            public void Invoke() => V8SplitProxyNative.V8Entity_DestroyHandle(Handle);
        }
    }
}
