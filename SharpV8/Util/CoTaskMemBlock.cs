// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.ClearScript.Util
{
    internal class CoTaskMemBlock : IDisposable
    {
        public IntPtr Addr { get; private set; }

        public CoTaskMemBlock(int size)
        {
            Addr = Marshal.AllocCoTaskMem(size);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Addr != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(Addr);
                Addr = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CoTaskMemBlock()
        {
            Dispose(false);
        }
    }

    internal sealed class CoTaskMemArrayBlock : CoTaskMemBlock
    {
        private readonly int elementSize;
        private readonly int length;

        public CoTaskMemArrayBlock(int elementSize, int length)
            : base(elementSize * length)
        {
            this.elementSize = elementSize;
            this.length = length;
        }

        public IntPtr GetAddr(int index)
        {
            if ((index < 0) || (index >= length))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (Addr == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString());
            }

            return GetAddrInternal(index);
        }

        private IntPtr GetAddrInternal(int index)
        {
            return Addr + (index * elementSize);
        }
    }
}
