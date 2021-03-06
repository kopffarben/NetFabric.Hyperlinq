﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueReadOnlyCollection
    {
        [Pure]
        public static List<TSource> ToList<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : notnull, IValueReadOnlyCollection<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source switch
            {
                ICollection<TSource> collection => new List<TSource>(collection), // no need to allocate helper class

                _ => new List<TSource>(new ToListCollection<TEnumerable, TEnumerator, TSource>(source)),
            };

        [Pure]
        public static List<TResult> ToList<TEnumerable, TEnumerator, TSource, TResult>(this TEnumerable source, Selector<TSource, TResult> selector)
            where TEnumerable : notnull, IValueReadOnlyCollection<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => new List<TResult>(new ToListCollection<TEnumerable, TEnumerator, TSource, TResult>(source, selector));

        [Pure]
        public static List<TResult> ToList<TEnumerable, TEnumerator, TSource, TResult>(this TEnumerable source, SelectorAt<TSource, TResult> selector)
            where TEnumerable : notnull, IValueReadOnlyCollection<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => new List<TResult>(new IndexedToListCollection<TEnumerable, TEnumerator, TSource, TResult>(source, selector));

        // helper implementation of ICollection<> so that CopyTo() is used to convert to List<>
        [GeneratorIgnore]
        internal sealed class ToListCollection<TEnumerable, TEnumerator, TSource>
            : ToListCollectionBase<TSource>
            where TEnumerable : notnull, IValueReadOnlyCollection<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            readonly TEnumerable source;

            public ToListCollection(TEnumerable source)
                : base(source.Count) 
                => this.source = source;

            public override void CopyTo(TSource[] array, int _)
            {
                // List<T> constructor checks if Count is zero
                using var enumerator = source.GetEnumerator();
                checked
                {
                    for (var index = 0; enumerator.MoveNext(); index++)
                        array[index] = enumerator.Current;
                }
            }
        }

        [GeneratorIgnore]
        internal sealed class ToListCollection<TEnumerable, TEnumerator, TSource, TResult>
            : ToListCollectionBase<TResult>
            where TEnumerable : notnull, IValueReadOnlyCollection<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            readonly TEnumerable source;
            readonly Selector<TSource, TResult> selector;

            public ToListCollection(TEnumerable source, Selector<TSource, TResult> selector)
                : base(source.Count) 
            {
                this.source = source;
                this.selector = selector;
            }

            public override void CopyTo(TResult[] array, int _)
            {
                // List<T> constructor checks if Count is zero
                using var enumerator = source.GetEnumerator();
                checked
                {
                    for (var index = 0; enumerator.MoveNext(); index++)
                        array[index] = selector(enumerator.Current);
                }
            }
        }

        [GeneratorIgnore]
        internal sealed class IndexedToListCollection<TEnumerable, TEnumerator, TSource, TResult>
            : ToListCollectionBase<TResult>
            where TEnumerable : notnull, IValueReadOnlyCollection<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            readonly TEnumerable source;
            readonly SelectorAt<TSource, TResult> selector;

            public IndexedToListCollection(TEnumerable source, SelectorAt<TSource, TResult> selector)
                : base(source.Count) 
            {
                this.source = source;
                this.selector = selector;
            }

            public override void CopyTo(TResult[] array, int _)
            {
                // List<T> constructor checks if Count is zero
                using var enumerator = source.GetEnumerator();
                checked
                {
                    for (var index = 0; enumerator.MoveNext(); index++)
                        array[index] = selector(enumerator.Current, index);
                }
            }
        }
    }
}
