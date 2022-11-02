// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.ClearScript.V8.SplitProxy;

namespace Microsoft.ClearScript.V8
{
    internal sealed class V8DebugListener
    {
        private V8EntityHolder holder;

        private V8DebugCallbackHandle Handle => (V8DebugCallbackHandle)holder.Handle;

        public V8DebugListener(V8DebugCallbackHandle hCallback)
        {
            holder = new V8EntityHolder("V8 debug listener", hCallback);
        }

        #region IV8DebugListener implementation

        public void ConnectClient()
        {
            V8SplitProxyNative.InvokeNoThrow(() => V8SplitProxyNative.V8DebugCallback_ConnectClient(Handle));
        }

        public void SendCommand(string command)
        {
            V8SplitProxyNative.InvokeNoThrow(() => V8SplitProxyNative.V8DebugCallback_SendCommand(Handle, command));
        }

        public void DisconnectClient()
        {
            V8SplitProxyNative.InvokeNoThrow(() => V8SplitProxyNative.V8DebugCallback_DisconnectClient(Handle));
        }

        #endregion

        #region disposal / finalization

        public void Dispose()
        {
            holder.ReleaseEntity();
            GC.KeepAlive(this);
        }

        ~V8DebugListener()
        {
            V8EntityHolder.Destroy(ref holder);
        }

        #endregion
    }
}
