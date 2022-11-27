// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.ClearScript.Util
{
    internal static unsafe class UnmanagedMemoryHelpers
    {
        public static ulong Copy<T>(IntPtr pSource, ulong length, T[] destination, ulong destinationIndex)
            where T : unmanaged
        {
            ulong destinationLength = (ulong)destination.LongLength;
            if (destinationIndex >= destinationLength)
            {
                ThrowDestinationIndexOutOfRange();
            }

            length = Math.Min(length, destinationLength - destinationIndex);

            ulong left = length;
            do
            {
                uint toCopy = int.MaxValue / (uint)sizeof(T);
                if (left < toCopy)
                    toCopy = (uint)left;

                uint bytesToCopy = toCopy * (uint)sizeof(T);

                ref T dst = ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(destination), (nuint)destinationIndex);
                Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref dst), ref *(byte*)pSource, bytesToCopy);

                pSource += (nint)bytesToCopy;
                destinationIndex += toCopy;
                left -= toCopy;
            }
            while (left > 0);

            return length;
        }

        public static ulong Copy<T>(T[] source, ulong sourceIndex, ulong length, IntPtr pDestination)
            where T : unmanaged
        {
            ulong sourceLength = (ulong)source.LongLength;
            if (sourceIndex >= sourceLength)
            {
                ThrowSourceIndexOutOfRange();
            }

            length = Math.Min(length, sourceLength - sourceIndex);

            ulong left = length;
            do
            {
                uint toCopy = int.MaxValue / (uint)sizeof(T);
                if (left < toCopy)
                    toCopy = (uint)left;

                uint bytesToCopy = toCopy * (uint)sizeof(T);
                
                ref T src = ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(source), (nuint)sourceIndex);
                Unsafe.CopyBlockUnaligned(ref *(byte*)pDestination, ref Unsafe.As<T, byte>(ref src), bytesToCopy);

                sourceIndex += toCopy;
                pDestination += (nint)bytesToCopy;
                left -= toCopy;
            }
            while (left > 0);

            return length;
        }

        [DoesNotReturn]
        private static void ThrowDestinationIndexOutOfRange()
        {
            throw new ArgumentOutOfRangeException("destinationIndex");
        }

        [DoesNotReturn]
        private static void ThrowSourceIndexOutOfRange()
        {
            throw new ArgumentOutOfRangeException("sourceIndex");
        }
    }
}
