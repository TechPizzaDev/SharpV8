// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Threading;
using Microsoft.ClearScript.V8;

namespace Microsoft.ClearScript.Util
{
    internal sealed class NativeCallbackTimer : IDisposable
    {
        private readonly Timer timer;
        private readonly NativeCallback callback;
        private InterlockedOneWayFlag disposedFlag = new();

        public NativeCallbackTimer(int dueTime, int period, NativeCallback callback)
        {
            this.callback = callback;
            timer = new Timer(OnTimer, this, Timeout.Infinite, Timeout.Infinite);

            if ((dueTime != Timeout.Infinite) || (period != Timeout.Infinite))
            {
                timer.Change(dueTime, period);
            }
        }

        public bool Change(int dueTime, int period)
        {
            if (!disposedFlag.IsSet)
            {
                if (MiscHelpers.Try(out var result, () => timer.Change(dueTime, period)))
                {
                    return result;
                }
            }

            return false;
        }

        private static void OnTimer(object? state)
        {
            NativeCallbackTimer timer = (NativeCallbackTimer)state!;

            if (!timer.disposedFlag.IsSet)
            {
                try
                {
                    timer.callback.Invoke();
                }
                catch
                {
                }
            }
        }

        public void Dispose()
        {
            if (disposedFlag.Set())
            {
                timer.Dispose();
                callback.Dispose();
            }
        }
    }
}
