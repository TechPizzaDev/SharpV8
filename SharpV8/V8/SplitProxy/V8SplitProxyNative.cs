// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.ClearScript.Util;

namespace Microsoft.ClearScript.V8.SplitProxy
{
    internal static partial class V8SplitProxyNative
    {
        public static ReadOnlySpan<char> GetVersion()
        {
            try
            {
                return V8SplitProxyNative_GetVersion();
            }
            catch (EntryPointNotFoundException)
            {
                return "[unknown]";
            }
        }

        public static void Invoke(Action action)
        {
            var previousScheduledException = MiscHelpers.Exchange(ref V8SplitProxyManaged.ScheduledException, null);
            try
            {
                action();
                if (V8SplitProxyManaged.ScheduledException != null)
                {
                    ThrowScheduledException(V8SplitProxyManaged.ScheduledException);
                }
            }
            finally
            {
                V8SplitProxyManaged.ScheduledException = previousScheduledException;
            }
        }

        public static T Invoke<T>(Func<T> func)
        {
            var previousScheduledException = MiscHelpers.Exchange(ref V8SplitProxyManaged.ScheduledException, null);
            try
            {
                var result = func();
                if (V8SplitProxyManaged.ScheduledException != null)
                {
                    ThrowScheduledException(V8SplitProxyManaged.ScheduledException);
                }
                return result;
            }
            finally
            {
                V8SplitProxyManaged.ScheduledException = previousScheduledException;
            }
        }

        public static void InvokeThrowing<TAction>(ref TAction action)
            where TAction : IProxyAction
        {
            var previousScheduledException = MiscHelpers.Exchange(ref V8SplitProxyManaged.ScheduledException, null);
            try
            {
                action.Invoke();
                if (V8SplitProxyManaged.ScheduledException != null)
                {
                    ThrowScheduledException(V8SplitProxyManaged.ScheduledException);
                }
            }
            finally
            {
                V8SplitProxyManaged.ScheduledException = previousScheduledException;
            }
        }

        private static void ThrowScheduledException(Exception exception)
        {
            throw exception;
        }
    }

    public interface IProxyAction
    {
        void Invoke();
    }
}
