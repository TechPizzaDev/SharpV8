// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Microsoft.ClearScript.Util
{
    public interface IScopeAction<T>
    {
        void Invoke(T value);
    }

    public struct Scope<T, TAction> : IDisposable
        where TAction : IScopeAction<T>
    {
        private readonly TAction? exitAction;
        private OneWayFlag disposedFlag = new();

        public Scope(T value, TAction? exitAction = default)
        {
            this.exitAction = exitAction;
            disposedFlag = new();
            Value = value;
        }

        public T Value { get; }

        public void Dispose()
        {
            if (disposedFlag.Set())
            {
                exitAction?.Invoke(Value);
            }
        }
    }
}
