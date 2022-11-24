// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.ClearScript.Util
{
    internal static partial class ObjectHelpers
    {
        private static readonly object[] zeroes =
        {
            (sbyte)0,
            (byte)0,
            (short)0,
            (ushort)0,
            0,
            0U,
            0L,
            0UL,
            IntPtr.Zero,
            UIntPtr.Zero,
            0.0f,
            0.0d,
            0.0m
        };

        public static bool IsZero(this object value) => Array.IndexOf(zeroes, value) >= 0;

        public static bool IsWholeNumber(this object value)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator

            if (value == null)
            {
                return false;
            }

            if (value.GetType().IsIntegral())
            {
                return true;
            }

            if (value is float floatValue)
            {
                return Math.Round(floatValue) == floatValue;
            }

            if (value is double doubleValue)
            {
                return Math.Round(doubleValue) == doubleValue;
            }

            if (value is decimal decimalValue)
            {
                return Math.Round(decimalValue) == decimalValue;
            }

            return false;

            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        public static Type GetTypeOrTypeInfo(this object value)
        {
            return value.GetType();
        }

        public static string GetFriendlyName(this object value)
        {
            return value.GetFriendlyName(null);
        }

        public static string GetFriendlyName(this object value, Type type)
        {
            if (type == null)
            {
                if (value == null)
                {
                    return "[null]";
                }

                type = value.GetType();
            }

            if (type.IsArray && (value != null))
            {
                var array = (Array)value;
                var dimensions = Enumerable.Range(0, type.GetArrayRank());
                var lengths = string.Join(",", dimensions.Select(array.GetLength));
                return MiscHelpers.FormatInvariant("{0}[{1}]", type.GetElementType().GetFriendlyName(), lengths);
            }

            return type.GetFriendlyName();
        }

        public static T DynamicCast<T>(this object value)
        {
            return DynamicCaster<T>.Cast(value);
        }

        public static object ToDynamicResult(this object result, ScriptEngine engine)
        {
            if (result is Nonexistent)
            {
                return Undefined.Value;
            }

            if ((result is HostTarget) || (result is IPropertyBag))
            {
                // Returning an instance of HostTarget (an internal type) isn't likely to be
                // useful. Wrapping it in a dynamic object makes sense in this context. Wrapping
                // a property bag allows it to participate in dynamic invocation chaining, which
                // may be useful when dealing with things like host type collections. HostItem
                // supports dynamic conversion, so the client can unwrap the object if necessary.

                return HostItem.Wrap(engine, result);
            }

            return result;
        }

        #region Nested type: DynamicCaster<T>

        private static class DynamicCaster<T>
        {
            public static T Cast(object value)
            {
                // ReSharper disable EmptyGeneralCatchClause

                try
                {
                    if (!typeof(T).IsValueType)
                    {
                        return (T)value;
                    }

                    if (typeof(T).IsEnum)
                    {
                        return (T)Enum.ToObject(typeof(T), value);
                    }

                    if (typeof(T).IsNullable())
                    {
                        return (T)CastToNullable(value);
                    }

                    if (value is IConvertible)
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }

                    return (T)value;
                }
                catch
                {
                }

                return (T)(dynamic)value;

                // ReSharper restore EmptyGeneralCatchClause
            }

            private static object CastToNullable(object value)
            {
                if (value != null)
                {
                    var valueCastType = typeof(DynamicCaster<>).MakeGenericType(Nullable.GetUnderlyingType(typeof(T)));
                    value = valueCastType.InvokeMember("Cast", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, new[] { value });
                    return typeof(T).CreateInstance(value);
                }

                return null;
            }
        }

        #endregion
    }
}
