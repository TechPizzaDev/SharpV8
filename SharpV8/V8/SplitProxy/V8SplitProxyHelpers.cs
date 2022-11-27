// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Runtime.InteropServices;
using Microsoft.ClearScript.Util;

namespace Microsoft.ClearScript.V8
{
    using static SplitProxy.V8SplitProxyNative;

    #region native object helpers

    internal static class StdString
    {
        public static IScope<StdStringPtr> CreateScope(string value = null)
        {
            return InvokeNoThrow(() => Scope.Create(() => StdString_New(value ?? string.Empty), StdString_Delete));
        }

        public static string GetValue(StdStringPtr pString)
        {
            return InvokeNoThrow(() => StdString_GetValue(pString));
        }

        public static void SetValue(StdStringPtr pString, string value)
        {
            InvokeNoThrow(() => StdString_SetValue(pString, value));
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

    internal static class StdStringArray
    {
        public static IScope<StdStringArrayPtr> CreateScope(int elementCount = 0)
        {
            return InvokeNoThrow(() => Scope.Create(() => StdStringArray_New(elementCount), StdStringArray_Delete));
        }

        public static IScope<StdStringArrayPtr> CreateScope(string[] array)
        {
            return InvokeNoThrow(() => Scope.Create(() => NewFromArray(array), StdStringArray_Delete));
        }

        public static int GetElementCount(StdStringArrayPtr pArray)
        {
            return InvokeNoThrow(() => StdStringArray_GetElementCount(pArray));
        }

        public static void SetElementCount(StdStringArrayPtr pArray, int elementCount)
        {
            InvokeNoThrow(() => StdStringArray_SetElementCount(pArray, elementCount));
        }

        public static string[] ToArray(StdStringArrayPtr pArray)
        {
            return InvokeNoThrow(() =>
            {
                var elementCount = StdStringArray_GetElementCount(pArray);
                var array = new string[elementCount];

                if (elementCount > 0)
                {
                    for (var index = 0; index < elementCount; index++)
                    {
                        array[index] = StdStringArray_GetElement(pArray, index);
                    }
                }

                return array;
            });
        }

        public static void CopyFromArray(StdStringArrayPtr pArray, string[] array)
        {
            InvokeNoThrow(() =>
            {
                var elementCount = array?.Length ?? 0;
                StdStringArray_SetElementCount(pArray, elementCount);

                for (var index = 0; index < elementCount; index++)
                {
                    StdStringArray_SetElement(pArray, index, array[index]);
                }
            });
        }

        private static StdStringArrayPtr NewFromArray(string[] array)
        {
            var elementCount = array?.Length ?? 0;
            var pArray = StdStringArray_New(elementCount);

            if (elementCount > 0)
            {
                for (var index = 0; index < elementCount; index++)
                {
                    StdStringArray_SetElement(pArray, index, array[index]);
                }
            }

            return pArray;
        }
    }

    #region Type: StdStringArrayPtr

    public readonly struct StdStringArrayPtr
    {
        private readonly IntPtr bits;

        private StdStringArrayPtr(IntPtr bits) => this.bits = bits;

        public static readonly StdStringArrayPtr Null = new(IntPtr.Zero);

        public static bool operator ==(StdStringArrayPtr left, StdStringArrayPtr right) => left.bits == right.bits;
        public static bool operator !=(StdStringArrayPtr left, StdStringArrayPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdStringArrayPtr ptr) => ptr.bits;
        public static explicit operator StdStringArrayPtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is StdStringArrayPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    internal static class StdByteArray
    {
        public static IScope<StdByteArrayPtr> CreateScope(int elementCount = 0)
        {
            return InvokeNoThrow(() => Scope.Create(() => StdByteArray_New(elementCount), StdByteArray_Delete));
        }

        public static IScope<StdByteArrayPtr> CreateScope(byte[] array)
        {
            return InvokeNoThrow(() => Scope.Create(() => NewFromArray(array), StdByteArray_Delete));
        }

        public static int GetElementCount(StdByteArrayPtr pArray)
        {
            return InvokeNoThrow(() => StdByteArray_GetElementCount(pArray));
        }

        public static void SetElementCount(StdByteArrayPtr pArray, int elementCount)
        {
            InvokeNoThrow(() => StdByteArray_SetElementCount(pArray, elementCount));
        }

        public static byte[] ToArray(StdByteArrayPtr pArray)
        {
            return InvokeNoThrow(() =>
            {
                var elementCount = StdByteArray_GetElementCount(pArray);
                var array = new byte[elementCount];

                if (elementCount > 0)
                {
                    Marshal.Copy(StdByteArray_GetData(pArray), array, 0, elementCount);
                }

                return array;
            });
        }

        public static void CopyFromArray(StdByteArrayPtr pArray, byte[] array)
        {
            InvokeNoThrow(() =>
            {
                var elementCount = array?.Length ?? 0;
                StdByteArray_SetElementCount(pArray, elementCount);

                if (elementCount > 0)
                {
                    Marshal.Copy(array, 0, StdByteArray_GetData(pArray), elementCount);
                }
            });
        }

        private static StdByteArrayPtr NewFromArray(byte[] array)
        {
            var elementCount = array?.Length ?? 0;
            var pArray = StdByteArray_New(elementCount);

            if (elementCount > 0)
            {
                Marshal.Copy(array, 0, StdByteArray_GetData(pArray), elementCount);
            }

            return pArray;
        }
    }

    #region Type: StdByteArrayPtr

    public readonly struct StdByteArrayPtr
    {
        private readonly IntPtr bits;

        private StdByteArrayPtr(IntPtr bits) => this.bits = bits;

        public static readonly StdByteArrayPtr Null = new StdByteArrayPtr(IntPtr.Zero);

        public static bool operator ==(StdByteArrayPtr left, StdByteArrayPtr right) => left.bits == right.bits;
        public static bool operator !=(StdByteArrayPtr left, StdByteArrayPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdByteArrayPtr ptr) => ptr.bits;
        public static explicit operator StdByteArrayPtr(IntPtr bits) => new StdByteArrayPtr(bits);

        public override bool Equals(object? obj) => obj is StdByteArrayPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    internal static class StdInt32Array
    {
        public static IScope<StdInt32ArrayPtr> CreateScope(int elementCount = 0)
        {
            return InvokeNoThrow(() => Scope.Create(() => StdInt32Array_New(elementCount), StdInt32Array_Delete));
        }

        public static IScope<StdInt32ArrayPtr> CreateScope(int[] array)
        {
            return InvokeNoThrow(() => Scope.Create(() => NewFromArray(array), StdInt32Array_Delete));
        }

        public static int GetElementCount(StdInt32ArrayPtr pArray)
        {
            return InvokeNoThrow(() => StdInt32Array_GetElementCount(pArray));
        }

        public static void SetElementCount(StdInt32ArrayPtr pArray, int elementCount)
        {
            InvokeNoThrow(() => StdInt32Array_SetElementCount(pArray, elementCount));
        }

        public static int[] ToArray(StdInt32ArrayPtr pArray)
        {
            return InvokeNoThrow(() =>
            {
                var elementCount = StdInt32Array_GetElementCount(pArray);
                var array = new int[elementCount];

                if (elementCount > 0)
                {
                    Marshal.Copy(StdInt32Array_GetData(pArray), array, 0, elementCount);
                }

                return array;
            });
        }

        public static void CopyFromArray(StdInt32ArrayPtr pArray, int[] array)
        {
            InvokeNoThrow(() =>
            {
                var elementCount = array?.Length ?? 0;
                StdInt32Array_SetElementCount(pArray, elementCount);

                if (elementCount > 0)
                {
                    Marshal.Copy(array, 0, StdInt32Array_GetData(pArray), elementCount);
                }
            });
        }

        private static StdInt32ArrayPtr NewFromArray(int[] array)
        {
            var elementCount = array?.Length ?? 0;
            var pArray = StdInt32Array_New(elementCount);

            if (elementCount > 0)
            {
                Marshal.Copy(array, 0, StdInt32Array_GetData(pArray), elementCount);
            }

            return pArray;
        }
    }

    #region Type: StdInt32ArrayPtr

    public readonly struct StdInt32ArrayPtr
    {
        private readonly IntPtr bits;

        private StdInt32ArrayPtr(IntPtr bits) => this.bits = bits;

        public static readonly StdInt32ArrayPtr Null = new(IntPtr.Zero);

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
        public static IScope<StdUInt32ArrayPtr> CreateScope(int elementCount = 0)
        {
            return InvokeNoThrow(() => Scope.Create(() => StdUInt32Array_New(elementCount), StdUInt32Array_Delete));
        }

        public static IScope<StdUInt32ArrayPtr> CreateScope(uint[] array)
        {
            return InvokeNoThrow(() => Scope.Create(() => NewFromArray(array), StdUInt32Array_Delete));
        }

        public static int GetElementCount(StdUInt32ArrayPtr pArray)
        {
            return InvokeNoThrow(() => StdUInt32Array_GetElementCount(pArray));
        }

        public static void SetElementCount(StdUInt32ArrayPtr pArray, int elementCount)
        {
            InvokeNoThrow(() => StdUInt32Array_SetElementCount(pArray, elementCount));
        }

        public static uint[] ToArray(StdUInt32ArrayPtr pArray)
        {
            return InvokeNoThrow(() =>
            {
                var elementCount = StdUInt32Array_GetElementCount(pArray);
                var array = new uint[elementCount];

                if (elementCount > 0)
                {
                    UnmanagedMemoryHelpers.Copy(StdUInt32Array_GetData(pArray), (ulong)elementCount, array, 0);
                }

                return array;
            });
        }

        public static void CopyFromArray(StdUInt32ArrayPtr pArray, uint[] array)
        {
            InvokeNoThrow(() =>
            {
                var elementCount = array?.Length ?? 0;
                StdUInt32Array_SetElementCount(pArray, elementCount);

                if (elementCount > 0)
                {
                    UnmanagedMemoryHelpers.Copy(array, 0, (ulong)elementCount, StdUInt32Array_GetData(pArray));
                }
            });
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
    }

    #region Type: StdUInt32ArrayPtr

    public readonly struct StdUInt32ArrayPtr
    {
        private readonly IntPtr bits;

        private StdUInt32ArrayPtr(IntPtr bits) => this.bits = bits;

        public static readonly StdUInt32ArrayPtr Null = new(IntPtr.Zero);

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
        public static IScope<StdUInt64ArrayPtr> CreateScope(int elementCount = 0)
        {
            return InvokeNoThrow(() => Scope.Create(() => StdUInt64Array_New(elementCount), StdUInt64Array_Delete));
        }

        public static IScope<StdUInt64ArrayPtr> CreateScope(ulong[] array)
        {
            return InvokeNoThrow(() => Scope.Create(() => NewFromArray(array), StdUInt64Array_Delete));
        }

        public static int GetElementCount(StdUInt64ArrayPtr pArray)
        {
            return InvokeNoThrow(() => StdUInt64Array_GetElementCount(pArray));
        }

        public static void SetElementCount(StdUInt64ArrayPtr pArray, int elementCount)
        {
            InvokeNoThrow(() => StdUInt64Array_SetElementCount(pArray, elementCount));
        }

        public static ulong[] ToArray(StdUInt64ArrayPtr pArray)
        {
            return InvokeNoThrow(() =>
            {
                var elementCount = StdUInt64Array_GetElementCount(pArray);
                var array = new ulong[elementCount];

                if (elementCount > 0)
                {
                    UnmanagedMemoryHelpers.Copy(StdUInt64Array_GetData(pArray), (ulong)elementCount, array, 0);
                }

                return array;
            });
        }

        public static void CopyFromArray(StdUInt64ArrayPtr pArray, ulong[] array)
        {
            InvokeNoThrow(() =>
            {
                var elementCount = array?.Length ?? 0;
                StdUInt64Array_SetElementCount(pArray, elementCount);

                if (elementCount > 0)
                {
                    UnmanagedMemoryHelpers.Copy(array, 0, (ulong)elementCount, StdUInt64Array_GetData(pArray));
                }
            });
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
    }

    #region Type: StdUInt64ArrayPtr

    public readonly struct StdUInt64ArrayPtr
    {
        private readonly IntPtr bits;

        private StdUInt64ArrayPtr(IntPtr bits) => this.bits = bits;

        public static readonly StdUInt64ArrayPtr Null = new(IntPtr.Zero);

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
        public static IScope<StdPtrArrayPtr> CreateScope(int elementCount = 0)
        {
            return InvokeNoThrow(() => Scope.Create(() => StdPtrArray_New(elementCount), StdPtrArray_Delete));
        }

        public static IScope<StdPtrArrayPtr> CreateScope(IntPtr[] array)
        {
            return InvokeNoThrow(() => Scope.Create(() => NewFromArray(array), StdPtrArray_Delete));
        }

        public static int GetElementCount(StdPtrArrayPtr pArray)
        {
            return InvokeNoThrow(() => StdPtrArray_GetElementCount(pArray));
        }

        public static void SetElementCount(StdPtrArrayPtr pArray, int elementCount)
        {
            InvokeNoThrow(() => StdPtrArray_SetElementCount(pArray, elementCount));
        }

        public static IntPtr[] ToArray(StdPtrArrayPtr pArray)
        {
            return InvokeNoThrow(() =>
            {
                var elementCount = StdPtrArray_GetElementCount(pArray);
                var array = new IntPtr[elementCount];

                if (elementCount > 0)
                {
                    Marshal.Copy(StdPtrArray_GetData(pArray), array, 0, elementCount);
                }

                return array;
            });
        }

        public static void CopyFromArray(StdPtrArrayPtr pArray, IntPtr[] array)
        {
            InvokeNoThrow(() =>
            {
                var elementCount = array?.Length ?? 0;
                StdPtrArray_SetElementCount(pArray, elementCount);

                if (elementCount > 0)
                {
                    Marshal.Copy(array, 0, StdPtrArray_GetData(pArray), elementCount);
                }
            });
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
    }

    #region Type: StdPtrArrayPtr

    public readonly struct StdPtrArrayPtr
    {
        private readonly IntPtr bits;

        private StdPtrArrayPtr(IntPtr bits) => this.bits = bits;

        public static readonly StdPtrArrayPtr Null = new StdPtrArrayPtr(IntPtr.Zero);

        public static bool operator ==(StdPtrArrayPtr left, StdPtrArrayPtr right) => left.bits == right.bits;
        public static bool operator !=(StdPtrArrayPtr left, StdPtrArrayPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdPtrArrayPtr ptr) => ptr.bits;
        public static explicit operator StdPtrArrayPtr(IntPtr bits) => new StdPtrArrayPtr(bits);

        public override bool Equals(object? obj) => obj is StdPtrArrayPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    internal static class StdV8ValueArray
    {
        public static IScope<StdV8ValueArrayPtr> CreateScope(int elementCount = 0)
        {
            return InvokeNoThrow(() => Scope.Create(() => StdV8ValueArray_New(elementCount), StdV8ValueArray_Delete));
        }

        public static IScope<StdV8ValueArrayPtr> CreateScope(object[] array)
        {
            return InvokeNoThrow(() => Scope.Create(() => NewFromArray(array), StdV8ValueArray_Delete));
        }

        public static int GetElementCount(StdV8ValueArrayPtr pArray)
        {
            return InvokeNoThrow(() => StdV8ValueArray_GetElementCount(pArray));
        }

        public static void SetElementCount(StdV8ValueArrayPtr pArray, int elementCount)
        {
            InvokeNoThrow(() => StdV8ValueArray_SetElementCount(pArray, elementCount));
        }

        public static object[] ToArray(StdV8ValueArrayPtr pArray)
        {
            return InvokeNoThrow(() =>
            {
                var elementCount = StdV8ValueArray_GetElementCount(pArray);
                var array = new object[elementCount];

                if (elementCount > 0)
                {
                    var pElements = StdV8ValueArray_GetData(pArray);
                    for (var index = 0; index < elementCount; index++)
                    {
                        array[index] = V8Value.Get(GetElementPtr(pElements, index));
                    }
                }

                return array;
            });
        }

        public static void CopyFromArray(StdV8ValueArrayPtr pArray, object[] array)
        {
            InvokeNoThrow(() =>
            {
                var elementCount = array?.Length ?? 0;
                StdV8ValueArray_SetElementCount(pArray, elementCount);

                if (elementCount > 0)
                {
                    var pElements = StdV8ValueArray_GetData(pArray);
                    for (var index = 0; index < elementCount; index++)
                    {
                        V8Value.Set(GetElementPtr(pElements, index), array[index]);
                    }
                }
            });
        }

        private static StdV8ValueArrayPtr NewFromArray(object[] array)
        {
            var elementCount = array?.Length ?? 0;
            var pArray = StdV8ValueArray_New(elementCount);

            var pData = StdV8ValueArray_GetData(pArray);
            for (var index = 0; index < elementCount; index++)
            {
                V8Value.Set(GetElementPtr(pData, index), array[index]);
            }

            return pArray;
        }

        public static V8ValuePtr GetElementPtr(V8ValuePtr pV8Value, int index)
        {
            return (V8ValuePtr)((IntPtr)pV8Value + index * V8Value.Size);
        }
    }

    #region Type: StdV8ValueArrayPtr

    public readonly struct StdV8ValueArrayPtr
    {
        private readonly IntPtr bits;

        private StdV8ValueArrayPtr(IntPtr bits) => this.bits = bits;

        public static readonly StdV8ValueArrayPtr Null = new(IntPtr.Zero);

        public static bool operator ==(StdV8ValueArrayPtr left, StdV8ValueArrayPtr right) => left.bits == right.bits;
        public static bool operator !=(StdV8ValueArrayPtr left, StdV8ValueArrayPtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(StdV8ValueArrayPtr ptr) => ptr.bits;
        public static explicit operator StdV8ValueArrayPtr(IntPtr bits) => new(bits);

        public override bool Equals(object? obj) => obj is StdV8ValueArrayPtr ptr && this == ptr;
        public override int GetHashCode() => bits.GetHashCode();
    }

    #endregion

    public static class V8Value
    {
        public const int Size = 16;

        public static IScope<V8ValuePtr> CreateScope()
        {
            return InvokeNoThrow(() => Scope.Create(V8Value_New, V8Value_Delete));
        }

        public static IScope<V8ValuePtr> CreateScope(object obj)
        {
            var scope = CreateScope();
            Set(scope.Value, obj);
            return scope;
        }

        public static void Set(V8ValuePtr pV8Value, object obj)
        {
            if (obj is Nonexistent)
            {
                SetNonexistent(pV8Value);
                return;
            }

            if (obj == null)
            {
                SetUndefined(pV8Value);
                return;
            }

            if (obj is DBNull)
            {
                SetNull(pV8Value);
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
                if (obj is string value)
                {
                    SetString(pV8Value, value);
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

            {
                if (obj is V8Object v8ObjectImpl)
                {
                    SetV8Object(pV8Value, v8ObjectImpl);
                    return;
                }
            }

            SetHostObject(pV8Value, obj);
        }

        public static object Get(V8ValuePtr pV8Value)
        {
            var intValue = 0;
            var uintValue = 0U;
            var doubleValue = 0D;
            var ptrOrHandle = IntPtr.Zero;

            var valueType = InvokeNoThrow(() => V8Value_Decode(pV8Value, out intValue, out uintValue, out doubleValue, out ptrOrHandle));
            switch (valueType)
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
                    return TryGetBigInteger(intValue, (int)uintValue, ptrOrHandle, out var result) ? result : null;

                case V8ValueType.V8Object:
                    return new V8Object(
                        (V8ObjectHandle)ptrOrHandle, (V8ValueSubtype)(uintValue & 0xFFFFU), (V8ValueFlags)(uintValue >> 16), intValue);

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
            InvokeNoThrow(() => V8Value_SetNonexistent(pV8Value));
        }

        private static void SetUndefined(V8ValuePtr pV8Value)
        {
            InvokeNoThrow(() => V8Value_SetUndefined(pV8Value));
        }

        private static void SetNull(V8ValuePtr pV8Value)
        {
            InvokeNoThrow(() => V8Value_SetNull(pV8Value));
        }

        private static void SetBoolean(V8ValuePtr pV8Value, bool value)
        {
            InvokeNoThrow(() => V8Value_SetBoolean(pV8Value, value));
        }

        private static void SetNumeric(V8ValuePtr pV8Value, double value)
        {
            InvokeNoThrow(() => V8Value_SetNumber(pV8Value, value));
        }

        private static void SetNumeric(V8ValuePtr pV8Value, int value)
        {
            InvokeNoThrow(() => V8Value_SetInt32(pV8Value, value));
        }

        private static void SetNumeric(V8ValuePtr pV8Value, uint value)
        {
            InvokeNoThrow(() => V8Value_SetUInt32(pV8Value, value));
        }

        private static void SetString(V8ValuePtr pV8Value, string value)
        {
            InvokeNoThrow(() => V8Value_SetString(pV8Value, value));
        }

        private static void SetDateTime(V8ValuePtr pV8Value, DateTime value)
        {
            InvokeNoThrow(() => V8Value_SetDateTime(pV8Value, (value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds));
        }

        private static void SetBigInt(V8ValuePtr pV8Value, BigInteger value)
        {
            var signBit = 0;
            if (value.Sign < 0)
            {
                signBit = 1;
                value = BigInteger.Negate(value);
            }

            var bytes = value.ToByteArray();
            InvokeNoThrow(() => V8Value_SetBigInt(pV8Value, signBit, bytes));
        }

        private static void SetV8Object(V8ValuePtr pV8Value, V8Object v8ObjectImpl)
        {
            InvokeNoThrow(() => V8Value_SetV8Object(pV8Value, v8ObjectImpl.Handle, v8ObjectImpl.Subtype, v8ObjectImpl.Flags));
        }

        private static void SetHostObject(V8ValuePtr pV8Value, object obj)
        {
            InvokeNoThrow(() => V8Value_SetHostObject(pV8Value, V8ProxyHelpers.AddRefHostObject(obj)));
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
            string name = null;
            var startTimestamp = 0UL;
            var endTimestamp = 0UL;
            var sampleCount = 0;
            var pRootNode = NodePtr.Null;
            InvokeNoThrow(() => V8CpuProfile_GetInfo(pProfile, hEntity, out name, out startTimestamp, out endTimestamp, out sampleCount, out pRootNode));

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
                    var nodeId = 0UL;
                    var timestamp = 0UL;
                    var sampleIndex = index;
                    var found = InvokeNoThrow(() => V8CpuProfile_GetSample(pProfile, sampleIndex, out nodeId, out timestamp));
                    if (found)
                    {
                        var node = profile.FindNode(nodeId);
                        if (node != null)
                        {
                            samples.Add(new V8CpuProfile.Sample { Node = node, Timestamp = timestamp });
                        }
                    }
                }

                if (samples.Count > 0)
                {
                    profile.Samples = new ReadOnlyCollection<V8CpuProfile.Sample>(samples);
                }
            }
        }

        private static V8CpuProfile.Node CreateNode(V8Entity hEntity, NodePtr pNode)
        {
            var nodeId = 0UL;
            var scriptId = 0L;
            string scriptName = null;
            string functionName = null;
            string bailoutReason = null;
            var lineNumber = 0L;
            var columnNumber = 0L;
            var hitCount = 0UL;
            var hitLineCount = 0U;
            var childCount = 0;
            InvokeNoThrow(() => V8CpuProfileNode_GetInfo(pNode, hEntity, out nodeId, out scriptId, out scriptName, out functionName, out bailoutReason, out lineNumber, out columnNumber, out hitCount, out hitLineCount, out childCount));

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
                int[] lineNumbers = null;
                uint[] hitCounts = null;
                if (InvokeNoThrow(() => V8CpuProfileNode_GetHitLines(pNode, out lineNumbers, out hitCounts)))
                {
                    var actualHitLineCount = Math.Min(lineNumbers.Length, hitCounts.Length);
                    if (actualHitLineCount > 0)
                    {
                        var hitLines = new V8CpuProfile.Node.HitLine[actualHitLineCount];

                        for (var index = 0; index < actualHitLineCount; index++)
                        {
                            hitLines[index].LineNumber = lineNumbers[index];
                            hitLines[index].HitCount = hitCounts[index];
                        }

                        node.HitLines = new ReadOnlyCollection<V8CpuProfile.Node.HitLine>(hitLines);
                    }
                }
            }

            if (childCount > 0)
            {
                var childNodes = new List<V8CpuProfile.Node>(childCount);

                for (var index = 0; index < childCount; index++)
                {
                    var childIndex = index;
                    var pChildNode = InvokeNoThrow(() => V8CpuProfileNode_GetChildNode(pNode, childIndex));
                    if (pChildNode != NodePtr.Null)
                    {
                        childNodes.Add(CreateNode(hEntity, pChildNode));
                    }
                }

                if (childNodes.Count > 0)
                {
                    node.ChildNodes = new ReadOnlyCollection<V8CpuProfile.Node>(childNodes);
                }
            }

            return node;
        }

        public readonly struct NodePtr
        {
            private readonly IntPtr bits;

            private NodePtr(IntPtr bits) => this.bits = bits;

            public static readonly NodePtr Null = new(IntPtr.Zero);

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

        public static readonly V8CpuProfilePtr Null = new(IntPtr.Zero);

        public static bool operator ==(V8CpuProfilePtr left, V8CpuProfilePtr right) => left.bits == right.bits;
        public static bool operator !=(V8CpuProfilePtr left, V8CpuProfilePtr right) => left.bits != right.bits;

        public static explicit operator IntPtr(V8CpuProfilePtr ptr) => ptr.bits;
        public static explicit operator V8CpuProfilePtr(IntPtr bits) => new(bits);

        public override bool Equals(object obj) => obj is V8CpuProfilePtr ptr && this == ptr;
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

        public static readonly V8Entity Empty = new(IntPtr.Zero);

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

        public static readonly V8DebugCallbackHandle Empty = new(IntPtr.Zero);

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
