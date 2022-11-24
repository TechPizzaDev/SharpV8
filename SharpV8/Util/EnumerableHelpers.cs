// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Microsoft.ClearScript.Util
{
    /// <exclude/>
    public interface IDisposableEnumerator : IEnumerator, IDisposable
    {
    }

    internal static partial class EnumerableHelpers
    {
        public static readonly HostType HostType = HostType.Wrap(typeof(EnumerableHelpers));

        public static IList<T> ToIList<T>(this IEnumerable<T> source)
        {
            return (source as IList<T>) ?? source.ToList();
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var element in source)
            {
                action(element);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var index = 0;
            foreach (var element in source)
            {
                action(element, index++);
            }
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (var element in source)
            {
                yield return element;

                foreach (var descendant in selector(element).Flatten(selector))
                {
                    yield return descendant;
                }
            }
        }

        public static IEnumerable<T> ToEnumerable<T>(this T element)
        {
            yield return element;
        }

        public static IEnumerable<string> ExcludeIndices(this IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                if (!int.TryParse(name, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                {
                    yield return name;
                }
            }
        }

        public static IEnumerable<int> GetIndices(this IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                if (int.TryParse(name, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index))
                {
                    yield return index;
                }
            }
        }

        public static IEnumerator<T> GetEnumerator<T>(IEnumerable<T> source)
        {
            return source.GetEnumerator();
        }

        public static IDisposableEnumerator GetEnumerator(IEnumerable source)
        {
            return new DisposableEnumeratorOnEnumerator(source.GetEnumerator());
        }
    }

    internal static partial class EnumerableHelpers<T>
    {
        public static readonly HostType HostType = HostType.Wrap(typeof(EnumerableHelpers<T>));

        public static IEnumerator<T> GetEnumerator(IEnumerable<T> source)
        {
            return source.GetEnumerator();
        }
    }

    internal sealed class DisposableEnumeratorOnEnumerator : IDisposableEnumerator
    {
        private readonly IEnumerator enumerator;

        public DisposableEnumeratorOnEnumerator(IEnumerator enumerator)
        {
            this.enumerator = enumerator;
        }

        public object Current => enumerator.Current;

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        public void Dispose()
        {
            (enumerator as IDisposable)?.Dispose();
        }
    }
}
