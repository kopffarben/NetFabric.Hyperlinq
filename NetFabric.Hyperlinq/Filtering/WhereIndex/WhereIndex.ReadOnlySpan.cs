﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class Array
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanWhereIndexEnumerable<TSource> Where<TSource>(this ReadOnlySpan<TSource> source, PredicateAt<TSource> predicate) 
        {
            if (predicate is null) Throw.ArgumentNullException(nameof(predicate));

            return new SpanWhereIndexEnumerable<TSource>(source, predicate);
        }

        public readonly ref struct SpanWhereIndexEnumerable<TSource>
        {
            internal readonly ReadOnlySpan<TSource> source;
            internal readonly PredicateAt<TSource> predicate;

            internal SpanWhereIndexEnumerable(in ReadOnlySpan<TSource> source, PredicateAt<TSource> predicate)
            {
                this.source = source;
                this.predicate = predicate;
            }

            [Pure]
            public readonly Enumerator GetEnumerator() => new Enumerator(in this);

            public ref struct Enumerator 
            {
                readonly ReadOnlySpan<TSource> source;
                readonly PredicateAt<TSource> predicate;
                int index;

                internal Enumerator(in SpanWhereIndexEnumerable<TSource> enumerable)
                {
                    source = enumerable.source;
                    predicate = enumerable.predicate;
                    index = -1;
                }

                [MaybeNull]
                public readonly ref readonly TSource Current 
                    => ref source[index];

                public bool MoveNext()
                {
                    while (++index < source.Length)
                    {
                        if (predicate(source[index], index))
                            return true;
                    }
                    return false;
                }
            }

            public int Count()
                => source.Count(predicate);

            public bool Any()
                => Array.Any(source, predicate);
                
            public bool Contains(TSource value, IEqualityComparer<TSource>? comparer = null)
                => Array.Contains(source, value, comparer, predicate);

            public SpanWhereIndexEnumerable<TSource> Where(Predicate<TSource> predicate)
                => Where<TSource>(source, Utils.Combine(this.predicate, predicate));

            public SpanWhereIndexEnumerable<TSource> Where(PredicateAt<TSource> predicate)
                => Where<TSource>(source, Utils.Combine(this.predicate, predicate));

            public Option<TSource> ElementAt(int index)
                => Array.ElementAt<TSource>(source, index, predicate);

            public Option<TSource> First()
                => Array.First<TSource>(source, predicate);

            public Option<TSource> Single()
                => Array.Single<TSource>(source, predicate);

            public TSource[] ToArray()
                => Array.ToArray(source, predicate);

            public List<TSource> ToList()
                => Array.ToList(source, predicate);
        }
    }
}

