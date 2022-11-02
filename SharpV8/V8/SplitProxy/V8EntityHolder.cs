// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Microsoft.ClearScript.V8.SplitProxy
{
    internal readonly struct V8EntityHolder
    {
        private readonly string name;
        private readonly V8Entity.Handle handle;

        public V8Entity.Handle Handle => (handle != V8Entity.Handle.Empty) ? handle : throw new InvalidOperationException("The " + name + " proxy has been destroyed");

        public V8EntityHolder(string name, V8Entity.Handle handle)
        {
            this.name = name;
            this.handle = handle;
        }

        private V8EntityHolder(string name)
        {
            this.name = name;
            handle = V8Entity.Handle.Empty;
        }

        public void ReleaseEntity()
        {
            var tempHandle = handle;
            if (tempHandle != V8Entity.Handle.Empty)
            {
                V8SplitProxyNative.InvokeNoThrow(() => V8SplitProxyNative.V8Entity_Release(tempHandle));
            }
        }

        public static void Destroy(ref V8EntityHolder holder)
        {
            var tempHandle = holder.handle;
            if (tempHandle != V8Entity.Handle.Empty)
            {
                V8SplitProxyNative.InvokeNoThrow(() => V8SplitProxyNative.V8Entity_DestroyHandle(tempHandle));
            }

            holder = new V8EntityHolder(holder.name);
        }
    }
}
