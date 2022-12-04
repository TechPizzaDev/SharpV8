// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Runtime.CompilerServices;
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
            var previousMethodTable = V8SplitProxyManaged_SetMethodTable(V8SplitProxyManaged.MethodTable);
            try
            {
                action();
                ThrowScheduledException();
            }
            finally
            {
                V8SplitProxyManaged_SetMethodTable(previousMethodTable);
                V8SplitProxyManaged.ScheduledException = previousScheduledException;
            }
        }

        public static T Invoke<T>(Func<T> func)
        {
            var previousScheduledException = MiscHelpers.Exchange(ref V8SplitProxyManaged.ScheduledException, null);
            var previousMethodTable = V8SplitProxyManaged_SetMethodTable(V8SplitProxyManaged.MethodTable);
            try
            {
                var result = func();
                ThrowScheduledException();
                return result;
            }
            finally
            {
                V8SplitProxyManaged_SetMethodTable(previousMethodTable);
                V8SplitProxyManaged.ScheduledException = previousScheduledException;
            }
        }

        public static void InvokeNoThrow(Action action)
        {
            var previousMethodTable = V8SplitProxyManaged_SetMethodTable(V8SplitProxyManaged.MethodTable);
            try
            {
                action();
            }
            finally
            {
                V8SplitProxyManaged_SetMethodTable(previousMethodTable);
            }
        }

        public static T InvokeNoThrow<T>(Func<T> func)
        {
            var previousMethodTable = V8SplitProxyManaged_SetMethodTable(V8SplitProxyManaged.MethodTable);
            try
            {
                return func();
            }
            finally
            {
                V8SplitProxyManaged_SetMethodTable(previousMethodTable);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invoke<TAction>(ref TAction action)
            where TAction : IProxyAction
        {
            var previousMethodTable = V8SplitProxyManaged_SetMethodTable(V8SplitProxyManaged.MethodTable);
            action.Invoke();
            V8SplitProxyManaged_SetMethodTable(previousMethodTable);
        }

        private static void ThrowScheduledException()
        {
            if (V8SplitProxyManaged.ScheduledException != null)
            {
                throw V8SplitProxyManaged.ScheduledException;
            }
        }
    }

    public interface IProxyAction
    {
        void Invoke();
    }
}
