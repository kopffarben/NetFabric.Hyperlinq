﻿using System;

namespace NetFabric.Hyperlinq
{
    public static partial class SpanExtensions
    {
        public static SelectEnumerable<TSource, TResult> Select<TSource, TResult>(
            this Span<TSource> source, 
            Func<TSource, int, TResult> selector)
        {
            if (selector is null) ThrowHelper.ThrowArgumentNullException(nameof(selector));

            return new SelectEnumerable<TSource, TResult>(in source, selector);
        }

        public readonly ref struct SelectEnumerable<TSource, TResult>
        {
            internal readonly Span<TSource> source;
            internal readonly Func<TSource, int, TResult> selector;

            internal SelectEnumerable(in Span<TSource> source, Func<TSource, int, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            public Enumerator GetEnumerator() => new Enumerator(in this);

            public ref struct Enumerator
            {
                Span<TSource> source;
                readonly Func<TSource, int, TResult> selector;
                readonly int count;
                int index;

                internal Enumerator(in SelectEnumerable<TSource, TResult> enumerable)
                {
                    source = enumerable.source;
                    selector = enumerable.selector;
                    count = enumerable.source.Length;
                    index = -1;
                }

                public TResult Current => selector(source[index], index);

                public bool MoveNext() => ++index < count;
            }

            public int Count()
                => source.Length;

            public TResult First()
                => selector(source.First(), 0);

            public TResult FirstOrDefault()
                => selector(source.FirstOrDefault(), 0);

            public TResult Single()
                => selector(source.Single(), 0);

            public TResult SingleOrDefault()
                => selector(source.SingleOrDefault(), 0);
        }

        public static TResult? FirstOrNull<TSource, TResult>(this SelectEnumerable<TSource, TResult> source)
            where TResult : struct
        {
            var span = source.source;
            if (span.Length == 0) return null;

            return source.selector(span[0], 0);
        }

        public static TResult? SingleOrNull<TSource, TResult>(this SelectEnumerable<TSource, TResult> source)
            where TResult : struct
        {
            var span = source.source;
            var length = span.Length;
            if (length == 0) return null;
            if (length > 1) ThrowHelper.ThrowNotSingleSequence<TSource>();

            return source.selector(span[0], 0);
        }
    }
}

