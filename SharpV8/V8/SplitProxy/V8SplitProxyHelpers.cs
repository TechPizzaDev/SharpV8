// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.ClearScript.Util;
using Microsoft.ClearScript.V8.SplitProxy;

namespace Microsoft.ClearScript.V8
{
    using static SplitProxy.V8SplitProxyNative;

    #region native object helpers

    internal static unsafe class StdString
    {
        public static Scope<StdStringPtr, Destructor> CreateScope(ReadOnlySpan<char> value = default)
        {
            return new Scope<StdStringPtr, Destructor>(StdString_New(value));
        }

        public static ReadOnlySpan<char> GetSpan(StdStringPtr pString)
        {
            return StdString_GetValue(pString);
        }

        public static string GetValue(StdStringPtr pString)
        {
            return GetSpan(pString).ToString();
        }

        public static void SetValue(StdStringPtr pString, ReadOnlySpan<char> value)
        {
            StdString_SetValue(pString, value);
        }

        public readonly struct Destructor : IScopeAction<StdStringPtr>
        {
            public void Invoke(StdStringPtr value) => StdString_Delete(value);
        }
    }

    #region Type: StdStringPtr

    public readonly struct StdStringPtr
    {
        private readonly IntPtr bits;

        private StdStringPtr(IntPtr bits) => this.bits = bits;

        public static StdStringPtr Null => new(IntPtr.Zero);

        public static bool operator ==(StdStringPtr left, StdStringPtr right) => left.bits == right.bits;
        public static bool operator !=(StdStringPtr left, StdStringPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdStringPtr ptr) => ptr.bits;
        public static explicit operator StdStringPtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is StdStringPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    internal static unsafe class StdStringArray
    {
        public static Scope<StdStringArrayPtr, Destructor> CreateScope(int elementCount = 0)
        {
            return new Scope<StdStringArrayPtr, Destructor>(StdStringArray_New(elementCount));
        }

        public static Scope<StdStringArrayPtr, Destructor> CreateScope(ReadOnlySpan<string> array)
        {
            return new Scope<StdStringArrayPtr, Destructor>(NewFromArray(array));
        }

        public static int GetElementCount(StdStringArrayPtr pArray)
        {
            return StdStringArray_GetElementCount(pArray);
        }

        public static void SetElementCount(StdStringArrayPtr pArray, int elementCount)
        {
            StdStringArray_SetElementCount(pArray, elementCount);
        }

        public static string[] ToArray(StdStringArrayPtr pArray)
        {
            var elementCount = StdStringArray_GetElementCount(pArray);
            if (elementCount <= 0)
            {
                return Array.Empty<string>();
            }

            var array = new string[elementCount];
            for (var index = 0; index < array.Length; index++)
            {
                array[index] = StdStringArray_GetElement(pArray, index).ToString();
            }
            return array;
        }

        public static void CopyFromArray(StdStringArrayPtr pArray, ReadOnlySpan<string> array)
        {
            StdStringArray_SetElementCount(pArray, array.Length);

            for (var index = 0; index < array.Length; index++)
            {
                StdStringArray_SetElement(pArray, index, array[index]);
            }
        }

        private static StdStringArrayPtr NewFromArray(ReadOnlySpan<string> array)
        {
            var pArray = StdStringArray_New(array.Length);

            for (var index = 0; index < array.Length; index++)
            {
                StdStringArray_SetElement(pArray, index, array[index]);
            }

            return pArray;
        }

        public readonly struct Destructor : IScopeAction<StdStringArrayPtr>
        {
            public void Invoke(StdStringArrayPtr value) => StdStringArray_Delete(value);
        }
    }

    #region Type: StdStringArrayPtr

    public readonly struct StdStringArrayPtr
    {
        private readonly IntPtr bits;

        private StdStringArrayPtr(IntPtr bits) => this.bits = bits;

        public static StdStringArrayPtr Null => new(IntPtr.Zero);

        public static bool operator ==(StdStringArrayPtr left, StdStringArrayPtr right) => left.bits == right.bits;
        public static bool operator !=(StdStringArrayPtr left, StdStringArrayPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdStringArrayPtr ptr) => ptr.bits;
        public static explicit operator StdStringArrayPtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is StdStringArrayPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    internal static unsafe class StdByteArray
    {
        public static Scope<StdByteArrayPtr, Destructor> CreateScope(int elementCount = 0)
        {
            return new Scope<StdByteArrayPtr, Destructor>(StdByteArray_New(elementCount));
        }

        public static Scope<StdByteArrayPtr, Destructor> CreateScope(ReadOnlySpan<byte> array)
        {
            return new Scope<StdByteArrayPtr, Destructor>(NewFromArray(array));
        }

        public static int GetElementCount(StdByteArrayPtr pArray)
        {
            return StdByteArray_GetElementCount(pArray);
        }

        public static void SetElementCount(StdByteArrayPtr pArray, int elementCount)
        {
            StdByteArray_SetElementCount(pArray, elementCount);
        }

        public static Span<byte> GetSpan(StdByteArrayPtr pArray)
        {
            var elementCount = StdByteArray_GetElementCount(pArray);
            if (elementCount <= 0)
            {
                return default;
            }
            return new Span<byte>((void*)StdByteArray_GetData(pArray), elementCount);
        }

        public static byte[] ToArray(StdByteArrayPtr pArray)
        {
            return GetSpan(pArray).ToArray();
        }

        public static void CopyFromArray(StdByteArrayPtr pArray, ReadOnlySpan<byte> array)
        {
            StdByteArray_SetElementCount(pArray, array.Length);

            if (array.Length > 0)
            {
                array.CopyTo(new Span<byte>((void*)StdByteArray_GetData(pArray), array.Length));
            }
        }

        private static StdByteArrayPtr NewFromArray(ReadOnlySpan<byte> array)
        {
            var pArray = StdByteArray_New(array.Length);

            if (array.Length > 0)
            {
                array.CopyTo(new Span<byte>((void*)StdByteArray_GetData(pArray), array.Length));
            }

            return pArray;
        }

        public readonly struct Destructor : IScopeAction<StdByteArrayPtr>
        {
            public void Invoke(StdByteArrayPtr value) => StdByteArray_Delete(value);
        }
    }

    #region Type: StdByteArrayPtr

    public readonly struct StdByteArrayPtr
    {
        private readonly IntPtr bits;

        private StdByteArrayPtr(IntPtr bits) => this.bits = bits;

        public static StdByteArrayPtr Null => new(IntPtr.Zero);

        public static bool operator ==(StdByteArrayPtr left, StdByteArrayPtr right) => left.bits == right.bits;
        public static bool operator !=(StdByteArrayPtr left, StdByteArrayPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdByteArrayPtr ptr) => ptr.bits;
        public static explicit operator StdByteArrayPtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is StdByteArrayPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    internal static unsafe class StdInt32Array
    {
        public static Scope<StdInt32ArrayPtr, Destructor> CreateScope(int elementCount = 0)
        {
            return new Scope<StdInt32ArrayPtr, Destructor>(StdInt32Array_New(elementCount));
        }

        public static Scope<StdInt32ArrayPtr, Destructor> CreateScope(ReadOnlySpan<int> array)
        {
            return new Scope<StdInt32ArrayPtr, Destructor>(NewFromArray(array));
        }

        public static int GetElementCount(StdInt32ArrayPtr pArray)
        {
            return StdInt32Array_GetElementCount(pArray);
        }

        public static void SetElementCount(StdInt32ArrayPtr pArray, int elementCount)
        {
            StdInt32Array_SetElementCount(pArray, elementCount);
        }

        public static Span<int> GetSpan(StdInt32ArrayPtr pArray)
        {
            var elementCount = StdInt32Array_GetElementCount(pArray);
            if (elementCount <= 0)
            {
                return default;
            }
            return new Span<int>((void*)StdInt32Array_GetData(pArray), elementCount);
        }

        public static int[] ToArray(StdInt32ArrayPtr pArray)
        {
            return GetSpan(pArray).ToArray();
        }

        public static void CopyFromArray(StdInt32ArrayPtr pArray, ReadOnlySpan<int> array)
        {
            StdInt32Array_SetElementCount(pArray, array.Length);

            if (array.Length > 0)
            {
                array.CopyTo(new Span<int>((void*)StdInt32Array_GetData(pArray), array.Length));
            }
        }

        private static StdInt32ArrayPtr NewFromArray(ReadOnlySpan<int> array)
        {
            var pArray = StdInt32Array_New(array.Length);

            if (array.Length > 0)
            {
                array.CopyTo(new Span<int>((void*)StdInt32Array_GetData(pArray), array.Length));
            }

            return pArray;
        }

        public readonly struct Destructor : IScopeAction<StdInt32ArrayPtr>
        {
            public void Invoke(StdInt32ArrayPtr value) => StdInt32Array_Delete(value);
        }
    }

    #region Type: StdInt32ArrayPtr

    public readonly struct StdInt32ArrayPtr
    {
        private readonly IntPtr bits;

        private StdInt32ArrayPtr(IntPtr bits) => this.bits = bits;

        public static StdInt32ArrayPtr Null => new(IntPtr.Zero);

        public static bool operator ==(StdInt32ArrayPtr left, StdInt32ArrayPtr right) => left.bits == right.bits;
        public static bool operator !=(StdInt32ArrayPtr left, StdInt32ArrayPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdInt32ArrayPtr ptr) => ptr.bits;
        public static explicit operator StdInt32ArrayPtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is StdInt32ArrayPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    internal static class StdUInt32Array
    {
        public static Scope<StdUInt32ArrayPtr, Destructor> CreateScope(int elementCount = 0)
        {
            return new Scope<StdUInt32ArrayPtr, Destructor>(StdUInt32Array_New(elementCount));
        }

        public static Scope<StdUInt32ArrayPtr, Destructor> CreateScope(uint[] array)
        {
            return new Scope<StdUInt32ArrayPtr, Destructor>(NewFromArray(array));
        }

        public static int GetElementCount(StdUInt32ArrayPtr pArray)
        {
            return StdUInt32Array_GetElementCount(pArray);
        }

        public static void SetElementCount(StdUInt32ArrayPtr pArray, int elementCount)
        {
            StdUInt32Array_SetElementCount(pArray, elementCount);
        }

        public static uint[] ToArray(StdUInt32ArrayPtr pArray)
        {
            var elementCount = StdUInt32Array_GetElementCount(pArray);
            if (elementCount <= 0)
            {
                return Array.Empty<uint>();
            }

            var array = new uint[elementCount];
            UnmanagedMemoryHelpers.Copy(StdUInt32Array_GetData(pArray), (ulong)elementCount, array, 0);
            return array;
        }

        public static void CopyFromArray(StdUInt32ArrayPtr pArray, uint[] array)
        {
            var elementCount = array?.Length ?? 0;
            StdUInt32Array_SetElementCount(pArray, elementCount);

            if (elementCount > 0)
            {
                UnmanagedMemoryHelpers.Copy(array, 0, (ulong)elementCount, StdUInt32Array_GetData(pArray));
            }
        }

        private static StdUInt32ArrayPtr NewFromArray(uint[] array)
        {
            var elementCount = array?.Length ?? 0;
            var pArray = StdUInt32Array_New(elementCount);

            if (elementCount > 0)
            {
                UnmanagedMemoryHelpers.Copy(array, 0, (ulong)elementCount, StdUInt32Array_GetData(pArray));
            }

            return pArray;
        }

        public readonly struct Destructor : IScopeAction<StdUInt32ArrayPtr>
        {
            public void Invoke(StdUInt32ArrayPtr value) => StdUInt32Array_Delete(value);
        }
    }

    #region Type: StdUInt32ArrayPtr

    public readonly struct StdUInt32ArrayPtr
    {
        private readonly IntPtr bits;

        private StdUInt32ArrayPtr(IntPtr bits) => this.bits = bits;

        public static StdUInt32ArrayPtr Null => new(IntPtr.Zero);

        public static bool operator ==(StdUInt32ArrayPtr left, StdUInt32ArrayPtr right) => left.bits == right.bits;
        public static bool operator !=(StdUInt32ArrayPtr left, StdUInt32ArrayPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdUInt32ArrayPtr ptr) => ptr.bits;
        public static explicit operator StdUInt32ArrayPtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is StdUInt32ArrayPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    internal static class StdUInt64Array
    {
        public static Scope<StdUInt64ArrayPtr, Destructor> CreateScope(int elementCount = 0)
        {
            return new Scope<StdUInt64ArrayPtr, Destructor>(StdUInt64Array_New(elementCount));
        }

        public static Scope<StdUInt64ArrayPtr, Destructor> CreateScope(ulong[] array)
        {
            return new Scope<StdUInt64ArrayPtr, Destructor>(NewFromArray(array));
        }

        public static int GetElementCount(StdUInt64ArrayPtr pArray)
        {
            return StdUInt64Array_GetElementCount(pArray);
        }

        public static void SetElementCount(StdUInt64ArrayPtr pArray, int elementCount)
        {
            StdUInt64Array_SetElementCount(pArray, elementCount);
        }

        public static ulong[] ToArray(StdUInt64ArrayPtr pArray)
        {
            var elementCount = StdUInt64Array_GetElementCount(pArray);
            if (elementCount <= 0)
            {
                return Array.Empty<ulong>();
            }

            var array = new ulong[elementCount];
            UnmanagedMemoryHelpers.Copy(StdUInt64Array_GetData(pArray), (ulong)elementCount, array, 0);
            return array;
        }

        public static void CopyFromArray(StdUInt64ArrayPtr pArray, ulong[] array)
        {
            var elementCount = array?.Length ?? 0;
            StdUInt64Array_SetElementCount(pArray, elementCount);

            if (elementCount > 0)
            {
                UnmanagedMemoryHelpers.Copy(array, 0, (ulong)elementCount, StdUInt64Array_GetData(pArray));
            }
        }

        private static StdUInt64ArrayPtr NewFromArray(ulong[] array)
        {
            var elementCount = array?.Length ?? 0;
            var pArray = StdUInt64Array_New(elementCount);

            if (elementCount > 0)
            {
                UnmanagedMemoryHelpers.Copy(array, 0, (ulong)elementCount, StdUInt64Array_GetData(pArray));
            }

            return pArray;
        }

        public readonly struct Destructor : IScopeAction<StdUInt64ArrayPtr>
        {
            public void Invoke(StdUInt64ArrayPtr value) => StdUInt64Array_Delete(value);
        }
    }

    #region Type: StdUInt64ArrayPtr

    public readonly struct StdUInt64ArrayPtr
    {
        private readonly IntPtr bits;

        private StdUInt64ArrayPtr(IntPtr bits) => this.bits = bits;

        public static StdUInt64ArrayPtr Null => new(IntPtr.Zero);

        public static bool operator ==(StdUInt64ArrayPtr left, StdUInt64ArrayPtr right) => left.bits == right.bits;
        public static bool operator !=(StdUInt64ArrayPtr left, StdUInt64ArrayPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdUInt64ArrayPtr ptr) => ptr.bits;
        public static explicit operator StdUInt64ArrayPtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is StdUInt64ArrayPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    internal static class StdPtrArray
    {
        public static Scope<StdPtrArrayPtr, Destructor> CreateScope(int elementCount = 0)
        {
            return new Scope<StdPtrArrayPtr, Destructor>(StdPtrArray_New(elementCount));
        }

        public static Scope<StdPtrArrayPtr, Destructor> CreateScope(IntPtr[] array)
        {
            return new Scope<StdPtrArrayPtr, Destructor>(NewFromArray(array));
        }

        public static int GetElementCount(StdPtrArrayPtr pArray)
        {
            return StdPtrArray_GetElementCount(pArray);
        }

        public static void SetElementCount(StdPtrArrayPtr pArray, int elementCount)
        {
            StdPtrArray_SetElementCount(pArray, elementCount);
        }

        public static IntPtr[] ToArray(StdPtrArrayPtr pArray)
        {
            var elementCount = StdPtrArray_GetElementCount(pArray);
            if (elementCount <= 0)
            {
                return Array.Empty<IntPtr>();
            }

            var array = new IntPtr[elementCount];
            Marshal.Copy(StdPtrArray_GetData(pArray), array, 0, elementCount);
            return array;
        }

        public static void CopyFromArray(StdPtrArrayPtr pArray, IntPtr[] array)
        {
            var elementCount = array?.Length ?? 0;
            StdPtrArray_SetElementCount(pArray, elementCount);

            if (elementCount > 0)
            {
                Marshal.Copy(array, 0, StdPtrArray_GetData(pArray), elementCount);
            }
        }

        private static StdPtrArrayPtr NewFromArray(IntPtr[] array)
        {
            var elementCount = array?.Length ?? 0;
            var pArray = StdPtrArray_New(elementCount);

            if (elementCount > 0)
            {
                Marshal.Copy(array, 0, StdPtrArray_GetData(pArray), elementCount);
            }

            return pArray;
        }

        public readonly struct Destructor : IScopeAction<StdPtrArrayPtr>
        {
            public void Invoke(StdPtrArrayPtr value) => StdPtrArray_Delete(value);
        }
    }

    #region Type: StdPtrArrayPtr

    public readonly struct StdPtrArrayPtr
    {
        private readonly IntPtr bits;

        private StdPtrArrayPtr(IntPtr bits) => this.bits = bits;

        public static StdPtrArrayPtr Null => new(IntPtr.Zero);

        public static bool operator ==(StdPtrArrayPtr left, StdPtrArrayPtr right) => left.bits == right.bits;
        public static bool operator !=(StdPtrArrayPtr left, StdPtrArrayPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdPtrArrayPtr ptr) => ptr.bits;
        public static explicit operator StdPtrArrayPtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is StdPtrArrayPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    internal static unsafe class StdV8ValueArray
    {
        public static Scope<StdV8ValueArrayPtr, Destructor> CreateScope(int elementCount = 0)
        {
            return new Scope<StdV8ValueArrayPtr, Destructor>(StdV8ValueArray_New(elementCount));
        }

        public static Scope<StdV8ValueArrayPtr, Destructor> CreateScope(ReadOnlySpan<object> array)
        {
            return new Scope<StdV8ValueArrayPtr, Destructor>(NewFromArray(array));
        }

        public static int GetElementCount(StdV8ValueArrayPtr pArray)
        {
            return StdV8ValueArray_GetElementCount(pArray);
        }

        public static void SetElementCount(StdV8ValueArrayPtr pArray, int elementCount)
        {
            StdV8ValueArray_SetElementCount(pArray, elementCount);
        }

        public static object[] ToArray(StdV8ValueArrayPtr pArray)
        {
            var elementCount = StdV8ValueArray_GetElementCount(pArray);
            if (elementCount <= 0)
            {
                return Array.Empty<object>();
            }

            var array = new object[elementCount];
            var pElements = StdV8ValueArray_GetData(pArray);
            for (var index = 0; index < array.Length; index++)
            {
                array[index] = V8Value.Get(GetElementPtr(pElements, index));
            }
            return array;
        }

        public static void CopyFromArray(StdV8ValueArrayPtr pArray, ReadOnlySpan<object> array)
        {
            StdV8ValueArray_SetElementCount(pArray, array.Length);

            if (array.Length > 0)
            {
                var pElements = StdV8ValueArray_GetData(pArray);
                for (var index = 0; index < array.Length; index++)
                {
                    V8Value.Set(GetElementPtr(pElements, index), array[index]);
                }
            }
        }

        private static StdV8ValueArrayPtr NewFromArray(ReadOnlySpan<object> array)
        {
            var pArray = StdV8ValueArray_New(array.Length);

            var pData = StdV8ValueArray_GetData(pArray);
            for (var index = 0; index < array.Length; index++)
            {
                V8Value.Set(GetElementPtr(pData, index), array[index]);
            }

            return pArray;
        }

        public static V8ValuePtr GetElementPtr(V8ValuePtr pV8Value, int index)
        {
            return (V8ValuePtr)((IntPtr)pV8Value + index * V8Value.Size);
        }

        public readonly struct Destructor : IScopeAction<StdV8ValueArrayPtr>
        {
            public void Invoke(StdV8ValueArrayPtr value) => StdV8ValueArray_Delete(value);
        }
    }

    #region Type: StdV8ValueArrayPtr

    public readonly struct StdV8ValueArrayPtr
    {
        private readonly IntPtr bits;

        private StdV8ValueArrayPtr(IntPtr bits) => this.bits = bits;

        public static StdV8ValueArrayPtr Null => new(IntPtr.Zero);

        public static bool operator ==(StdV8ValueArrayPtr left, StdV8ValueArrayPtr right) => left.bits == right.bits;
        public static bool operator !=(StdV8ValueArrayPtr left, StdV8ValueArrayPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdV8ValueArrayPtr ptr) => ptr.bits;
        public static explicit operator StdV8ValueArrayPtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is StdV8ValueArrayPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    public static unsafe class V8Value
    {
        public const int Size = 16;

        public static Scope<V8ValuePtr, Destructor> CreateScope()
        {
            return new Scope<V8ValuePtr, Destructor>(V8Value_New());
        }

        public static Scope<V8ValuePtr, Destructor> CreateScope<T>(T obj)
        {
            var scope = CreateScope();
            Set(scope.Value, obj);
            return scope;
        }

        public static Scope<V8ValuePtr, Destructor> CreateScope(object obj)
        {
            var scope = CreateScope();
            Set(scope.Value, obj);
            return scope;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Set<T>(V8ValuePtr pV8Value, T obj)
        {
            if (obj == null)
            {
                SetUndefined(pV8Value);
                return;
            }

            Type t = typeof(T);

            if (t == typeof(bool))
            {
                SetBoolean(pV8Value, (bool)(object)obj);
                return;
            }

            if (t == typeof(char))
            {
                SetNumeric(pV8Value, (char)(object)obj);
                return;
            }

            if (t == typeof(sbyte))
            {
                SetNumeric(pV8Value, (sbyte)(object)obj);
                return;
            }

            if (t == typeof(byte))
            {
                SetNumeric(pV8Value, (byte)(object)obj);
                return;
            }

            if (t == typeof(short))
            {
                SetNumeric(pV8Value, (short)(object)obj);
                return;
            }

            if (t == typeof(ushort))
            {
                SetNumeric(pV8Value, (ushort)(object)obj);
                return;
            }

            if (t == typeof(int))
            {
                SetNumeric(pV8Value, (int)(object)obj);
                return;
            }

            if (t == typeof(uint))
            {
                SetNumeric(pV8Value, (uint)(object)obj);
                return;
            }

            if (t == typeof(long))
            {
                SetNumeric(pV8Value, (long)(object)obj);
                return;
            }

            if (t == typeof(ulong))
            {
                SetNumeric(pV8Value, (ulong)(object)obj);
                return;
            }

            if (t == typeof(float))
            {
                SetNumeric(pV8Value, (float)(object)obj);
                return;
            }

            if (t == typeof(double))
            {
                SetNumeric(pV8Value, (double)(object)obj);
                return;
            }

            if (t == typeof(decimal))
            {
                SetNumeric(pV8Value, (double)(decimal)(object)obj);
                return;
            }

            if (t == typeof(DateTime))
            {
                SetDateTime(pV8Value, (DateTime)(object)obj);
                return;
            }

            if (t == typeof(BigInteger))
            {
                SetBigInt(pV8Value, (BigInteger)(object)obj);
                return;
            }

            SetObject(pV8Value, obj);
        }

        public static void Set(V8ValuePtr pV8Value, object obj)
        {
            if (obj == null)
            {
                SetUndefined(pV8Value);
                return;
            }

            {
                if (obj is bool value)
                {
                    SetBoolean(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is char value)
                {
                    SetNumeric(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is sbyte value)
                {
                    SetNumeric(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is byte value)
                {
                    SetNumeric(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is short value)
                {
                    SetNumeric(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is ushort value)
                {
                    SetNumeric(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is int value)
                {
                    SetNumeric(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is uint value)
                {
                    SetNumeric(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is long value)
                {
                    SetNumeric(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is ulong value)
                {
                    SetNumeric(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is float value)
                {
                    SetNumeric(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is double value)
                {
                    SetNumeric(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is decimal value)
                {
                    SetNumeric(pV8Value, (double)value);
                    return;
                }
            }

            {
                if (obj is DateTime value)
                {
                    SetDateTime(pV8Value, value);
                    return;
                }
            }

            {
                if (obj is BigInteger value)
                {
                    SetBigInt(pV8Value, value);
                    return;
                }
            }

            SetObject(pV8Value, obj);
        }

        private static void SetObject(V8ValuePtr pV8Value, object obj)
        {
            if (obj is string str)
            {
                SetString(pV8Value, str);
                return;
            }

            if (obj is V8Object v8Obj)
            {
                SetV8Object(pV8Value, v8Obj);
                return;
            }

            if (obj is Nonexistent)
            {
                SetNonexistent(pV8Value);
                return;
            }

            if (obj is DBNull)
            {
                SetNull(pV8Value);
                return;
            }

            SetHostObject(pV8Value, obj);
        }

        public static object? Get(V8ValuePtr pV8Value)
        {
            V8ValueType Type = V8Value_Decode(pV8Value, out var intValue, out var uintValue, out var doubleValue, out var ptrOrHandle);

            switch (Type)
            {
                case V8ValueType.Nonexistent:
                    return Nonexistent.Value;

                case V8ValueType.Null:
                    return DBNull.Value;

                case V8ValueType.Boolean:
                    return intValue != 0;

                case V8ValueType.Number:
                    return doubleValue;

                case V8ValueType.Int32:
                    return intValue;

                case V8ValueType.UInt32:
                    return uintValue;

                case V8ValueType.String:
                    return Marshal.PtrToStringUni(ptrOrHandle, intValue);

                case V8ValueType.DateTime:
                    return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromMilliseconds(doubleValue);

                case V8ValueType.BigInt:
                    return TryGetBigInteger(
                        intValue, (int)uintValue, ptrOrHandle, out var result) ? result : null;

                case V8ValueType.V8Object:
                    return new V8Object(
                        (V8ObjectHandle)ptrOrHandle,
                        (V8ValueSubtype)(uintValue & 0xFFFFU),
                        (V8ValueFlags)(uintValue >> 16),
                        intValue);

                case V8ValueType.HostObject:
                    return V8ProxyHelpers.GetHostObject(ptrOrHandle);

                default:
                    return null;
            }
        }

        private static bool TryGetBigInteger(int signBit, int wordCount, IntPtr pWords, out BigInteger result)
        {
            result = BigInteger.Zero;

            if (wordCount > 0)
            {
                var byteCount = (ulong)wordCount * sizeof(ulong);
                if (byteCount >= (ulong)Array.MaxLength)
                {
                    return false;
                }

                // use extra zero byte to force unsigned interpretation
                var bytes = new byte[byteCount];
                UnmanagedMemoryHelpers.Copy(pWords, byteCount, bytes, 0);

                // construct result and negate if necessary
                result = new BigInteger(bytes, isUnsigned: true);
                if (signBit != 0)
                {
                    result = BigInteger.Negate(result);
                }
            }

            return true;
        }

        private static void SetNonexistent(V8ValuePtr pV8Value)
        {
            V8Value_SetNonexistent(pV8Value);
        }

        private static void SetUndefined(V8ValuePtr pV8Value)
        {
            V8Value_SetUndefined(pV8Value);
        }

        private static void SetNull(V8ValuePtr pV8Value)
        {
            V8Value_SetNull(pV8Value);
        }

        private static void SetBoolean(V8ValuePtr pV8Value, bool value)
        {
            V8Value_SetBoolean(pV8Value, value);
        }

        private static void SetNumeric(V8ValuePtr pV8Value, double value)
        {
            V8Value_SetNumber(pV8Value, value);
        }

        private static void SetNumeric(V8ValuePtr pV8Value, int value)
        {
            V8Value_SetInt32(pV8Value, value);
        }

        private static void SetNumeric(V8ValuePtr pV8Value, uint value)
        {
            V8Value_SetUInt32(pV8Value, value);
        }

        private static void SetString(V8ValuePtr pV8Value, ReadOnlySpan<char> value)
        {
            V8Value_SetString(pV8Value, value);
        }

        private static void SetDateTime(V8ValuePtr pV8Value, DateTime value)
        {
            double millis = (value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            V8Value_SetDateTime(pV8Value, millis);
        }

        private static void SetBigInt(V8ValuePtr pV8Value, BigInteger value)
        {
            int signBit = 0;
            if (value.Sign < 0)
            {
                signBit = 1;
                value = BigInteger.Negate(value);
            }

            byte[] bytes = value.ToByteArray();
            ReadOnlySpan<byte> span = bytes;

            V8Value_SetBigInt(pV8Value, signBit, span);
        }

        private static void SetV8Object(V8ValuePtr pV8Value, V8Object v8ObjectImpl)
        {
            V8Value_SetV8Object(pV8Value, v8ObjectImpl.Handle, v8ObjectImpl.Subtype, v8ObjectImpl.Flags);
        }

        private static void SetHostObject(V8ValuePtr pV8Value, object obj)
        {
            V8Value_SetHostObject(pV8Value, V8ProxyHelpers.AddRefHostObject(obj));
        }

        public readonly struct Destructor : IScopeAction<V8ValuePtr>
        {
            public void Invoke(V8ValuePtr value) => V8Value_Delete(value);
        }
    }

    #region Enum: V8ValueType

    public enum V8ValueType : ushort
    {
        // IMPORTANT: maintain bitwise equivalence with native enum V8Value::Type
        Nonexistent,
        Undefined,
        Null,
        Boolean,
        Number,
        Int32,
        UInt32,
        String,
        DateTime,
        BigInt,
        V8Object,
        HostObject
    }

    #endregion

    #region Enum: V8ValueSubtype

    public enum V8ValueSubtype : ushort
    {
        // IMPORTANT: maintain bitwise equivalence with native enum V8Value::Subtype
        None,
        Promise,
        Array,
        ArrayBuffer,
        DataView,
        Uint8Array,
        Uint8ClampedArray,
        Int8Array,
        Uint16Array,
        Int16Array,
        Uint32Array,
        Int32Array,
        BigUint64Array,
        BigInt64Array,
        Float32Array,
        Float64Array
    }

    #endregion

    #region Enum: V8ValueFlags

    [Flags]
    public enum V8ValueFlags : ushort
    {
        // IMPORTANT: maintain bitwise equivalence with native enum V8Value::Flags
        None = 0,
        Shared = 0x0001
    }

    #endregion

    #region Type: V8ValuePtr

    public readonly struct V8ValuePtr
    {
        private readonly IntPtr bits;

        private V8ValuePtr(IntPtr bits) => this.bits = bits;

        public static V8ValuePtr Null => new(IntPtr.Zero);

        public static bool operator ==(V8ValuePtr left, V8ValuePtr right) => left.bits == right.bits;
        public static bool operator !=(V8ValuePtr left, V8ValuePtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(V8ValuePtr ptr) => ptr.bits;
        public static explicit operator V8ValuePtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is V8ValuePtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    internal static class V8CpuProfileImpl
    {
        public static void ProcessProfile(V8Entity hEntity, V8CpuProfilePtr pProfile, V8CpuProfile profile)
        {
            V8CpuProfile_GetInfo(pProfile, hEntity, out string name, out var startTimestamp, out var endTimestamp, out var sampleCount, out var pRootNode);

            profile.Name = name;
            profile.StartTimestamp = startTimestamp;
            profile.EndTimestamp = endTimestamp;

            if (pRootNode != NodePtr.Null)
            {
                profile.RootNode = CreateNode(hEntity, pRootNode);
            }

            if (sampleCount > 0)
            {
                var samples = new List<V8CpuProfile.Sample>(sampleCount);

                for (var index = 0; index < sampleCount; index++)
                {
                    var sampleIndex = index;
                    var found = V8CpuProfile_GetSample(pProfile, sampleIndex, out ulong nodeId, out ulong timestamp);
                    if (found)
                    {
                        var node = profile.FindNode(nodeId);
                        if (node != null)
                        {
                            samples.Add(new V8CpuProfile.Sample(node, timestamp));
                        }
                    }
                }

                if (samples.Count > 0)
                {
                    profile.Samples = samples;
                }
            }
        }

        private static V8CpuProfile.Node CreateNode(V8Entity hEntity, NodePtr pNode)
        {
            V8CpuProfileNode_GetInfo(
                pNode, hEntity, 
                out ulong nodeId, out long scriptId, out string scriptName, out string functionName, out string bailoutReason, 
                out long lineNumber, out long columnNumber, out ulong hitCount, out uint hitLineCount, out int childCount);

            var node = new V8CpuProfile.Node
            {
                NodeId = nodeId,
                ScriptId = scriptId,
                ScriptName = scriptName,
                FunctionName = functionName,
                BailoutReason = bailoutReason,
                LineNumber = lineNumber,
                ColumnNumber = columnNumber,
                HitCount = hitCount,
            };

            if (hitLineCount > 0)
            {
                if (V8CpuProfileNode_GetHitLines(pNode, out int[] lineNumbers, out uint[] hitCounts))
                {
                    var actualHitLineCount = Math.Min(lineNumbers.Length, hitCounts.Length);
                    if (actualHitLineCount > 0)
                    {
                        var hitLines = new V8CpuProfile.Node.HitLine[actualHitLineCount];

                        for (var index = 0; index < actualHitLineCount; index++)
                        {
                            hitLines[index] = new V8CpuProfile.Node.HitLine(
                                lineNumbers[index],
                                hitCounts[index]);
                        }

                        node.HitLines = hitLines;
                    }
                }
            }

            if (childCount > 0)
            {
                var childNodes = new List<V8CpuProfile.Node>(childCount);

                for (var index = 0; index < childCount; index++)
                {
                    var childIndex = index;
                    var pChildNode = V8CpuProfileNode_GetChildNode(pNode, childIndex);
                    if (pChildNode != NodePtr.Null)
                    {
                        childNodes.Add(CreateNode(hEntity, pChildNode));
                    }
                }

                if (childNodes.Count > 0)
                {
                    node.ChildNodes = childNodes;
                }
            }

            return node;
        }

        public readonly struct NodePtr
        {
            private readonly IntPtr bits;

            private NodePtr(IntPtr bits) => this.bits = bits;

            public static NodePtr Null => new(IntPtr.Zero);

            public static bool operator ==(NodePtr left, NodePtr right) => left.bits == right.bits;
            public static bool operator !=(NodePtr left, NodePtr right) => left.bits != right.bits;

            public static explicit operator IntPtr(NodePtr ptr) => ptr.bits;
            public static explicit operator NodePtr(IntPtr bits) => new(bits);

            public override bool Equals(object? obj) => obj is NodePtr ptr && this == ptr;
            public override int GetHashCode() => bits.GetHashCode();
        }
    }

    #region Type: V8CpuProfilePtr

    public readonly struct V8CpuProfilePtr
    {
        private readonly IntPtr bits;

        private V8CpuProfilePtr(IntPtr bits) => this.bits = bits;

        public static V8CpuProfilePtr Null => new(IntPtr.Zero);

        public static bool operator ==(V8CpuProfilePtr left, V8CpuProfilePtr right) => left.bits == right.bits;
        public static bool operator !=(V8CpuProfilePtr left, V8CpuProfilePtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(V8CpuProfilePtr ptr) => ptr.bits;
        public static explicit operator V8CpuProfilePtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is V8CpuProfilePtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    #endregion

    #region V8 entity helpers

    #region Nested type: V8Entity

    public readonly struct V8Entity
    {
        private readonly IntPtr guts;

        private V8Entity(IntPtr guts) => this.guts = guts;

        public static V8Entity Empty => new(IntPtr.Zero);

        public static bool operator ==(V8Entity left, V8Entity right) => left.guts == right.guts;
        public static bool operator !=(V8Entity left, V8Entity right) => left.guts != right.guts;

        public static explicit operator IntPtr(V8Entity handle) => handle.guts;
        public static explicit operator V8Entity(IntPtr guts) => new(guts);

        public override bool Equals(object? obj) => obj is V8Entity handle && this == handle;
        public override int GetHashCode() => guts.GetHashCode();
    }

    #endregion

    #region Nested type: V8DebugCallbackHandle

    public readonly struct V8DebugCallbackHandle
    {
        private readonly IntPtr guts;

        private V8DebugCallbackHandle(IntPtr guts) => this.guts = guts;

        public static V8DebugCallbackHandle Empty => new(IntPtr.Zero);

        public static bool operator ==(V8DebugCallbackHandle left, V8DebugCallbackHandle right) => left.guts == right.guts;
        public static bool operator !=(V8DebugCallbackHandle left, V8DebugCallbackHandle right) => left.guts != right.guts;

        public static explicit operator IntPtr(V8DebugCallbackHandle handle) => handle.guts;
        public static explicit operator V8DebugCallbackHandle(IntPtr guts) => new(guts);

        public static implicit operator V8Entity(V8DebugCallbackHandle handle) => (V8Entity)handle.guts;
        public static explicit operator V8DebugCallbackHandle(V8Entity handle) => (V8DebugCallbackHandle)(IntPtr)handle;

        public override bool Equals(object? obj) => obj is V8DebugCallbackHandle handle && this == handle;
        public override int GetHashCode() => guts.GetHashCode();
    }

    #endregion

    #endregion
}
