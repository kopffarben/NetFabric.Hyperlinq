﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class Array
    {
        [Pure]
        public static MemorySelectIndexEnumerable<TSource, TResult> Select<TSource, TResult>(
            this ReadOnlyMemory<TSource> source, 
            SelectorAt<TSource, TResult> selector)
        {
            if (selector is null) Throw.ArgumentNullException(nameof(selector));

            return new MemorySelectIndexEnumerable<TSource, TResult>(in source, selector);
        }

        [GeneratorMapping("TSource", "TResult")]
        public readonly partial struct MemorySelectIndexEnumerable<TSource, TResult>
            : IValueReadOnlyList<TResult, MemorySelectIndexEnumerable<TSource, TResult>.DisposableEnumerator>
            , IList<TResult>
        {
            internal readonly ReadOnlyMemory<TSource> source;
            internal readonly SelectorAt<TSource, TResult> selector;

            internal MemorySelectIndexEnumerable(in ReadOnlyMemory<TSource> source, SelectorAt<TSource, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            public readonly int Count => source.Length;

            [MaybeNull]
            public readonly TResult this[int index]
            {
                get
                {
                    if (index < 0 || index >= source.Length)
                        Throw.ArgumentOutOfRangeException(nameof(index));

                    return selector(source.Span[index], index);
                }
            }

            public readonly Enumerator GetEnumerator() 
                => new Enumerator(in this);
            readonly DisposableEnumerator IValueEnumerable<TResult, MemorySelectIndexEnumerable<TSource, TResult>.DisposableEnumerator>.GetEnumerator() 
                => new DisposableEnumerator(in this);
            readonly IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() 
                => new DisposableEnumerator(in this);
            readonly IEnumerator IEnumerable.GetEnumerator() 
                => new DisposableEnumerator(in this);

            [MaybeNull]
            TResult IList<TResult>.this[int index]
            {
                get => this[index];
                set => throw new NotImplementedException();
            }

            bool ICollection<TResult>.IsReadOnly  
                => true;

            void ICollection<TResult>.CopyTo(TResult[] array, int arrayIndex) 
            {
                var span = source.Span;
                for (var index = 0; index < source.Length; index++)
                    array[index + arrayIndex] = selector(span[index], index);
            }
            void ICollection<TResult>.Add(TResult item) 
                => throw new NotImplementedException();
            void ICollection<TResult>.Clear() 
                => throw new NotImplementedException();
            bool ICollection<TResult>.Contains(TResult item) 
            {
                var span = source.Span;
                for (var index = 0; index < source.Length; index++)
                {
                    if (EqualityComparer<TResult>.Default.Equals(selector(span[index], index), item))
                        return true;
                }
                return false;
            }
            bool ICollection<TResult>.Remove(TResult item) 
                => throw new NotImplementedException();
            int IList<TResult>.IndexOf(TResult item)
            {
                var span = source.Span;
                for (var index = 0; index < source.Length; index++)
                {
                    if (EqualityComparer<TResult>.Default.Equals(selector(span[index], index), item))
                        return index;
                }
                return -1;
            }
            void IList<TResult>.Insert(int index, TResult item)
                => throw new NotImplementedException();
            void IList<TResult>.RemoveAt(int index)
                => throw new NotImplementedException();

            public ref struct Enumerator
            {
                readonly ReadOnlySpan<TSource> source;
                readonly SelectorAt<TSource, TResult> selector;
                int index;

                internal Enumerator(in MemorySelectIndexEnumerable<TSource, TResult> enumerable)
                {
                    source = enumerable.source.Span;
                    selector = enumerable.selector;
                    index = -1;
                }

                public readonly TResult Current 
                    => selector(source[index], index);

                public bool MoveNext() 
                    => ++index < source.Length;
            }

            public struct DisposableEnumerator
                : IEnumerator<TResult>
            {
                readonly ReadOnlyMemory<TSource> source;
                readonly SelectorAt<TSource, TResult> selector;
                int index;

                internal DisposableEnumerator(in MemorySelectIndexEnumerable<TSource, TResult> enumerable)
                {
                    source = enumerable.source;
                    selector = enumerable.selector;
                    index = -1;
                }
 
                [MaybeNull] 
                public readonly TResult Current 
                    => selector(source.Span[index], index);
                readonly object? IEnumerator.Current 
                    => selector(source.Span[index], index);

                public bool MoveNext() 
                    => ++index < source.Length;

                [ExcludeFromCodeCoverage]
                public readonly void Reset() 
                    => throw new NotSupportedException();

                public void Dispose() { }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => source.Length != 0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Contains(TResult value, IEqualityComparer<TResult>? comparer = null)
                => Array.Contains(source.Span, value, comparer, selector);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TResult> ElementAt(int index)
                => Array.ElementAt<TSource, TResult>(source.Span, index, selector);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TResult> First()
                => Array.First<TSource, TResult>(source.Span, selector);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<TResult> Single()
                => Array.Single<TSource, TResult>(source.Span, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TResult[] ToArray()
                => Array.ToArray(source.Span, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<TResult> ToList()
                => Array.ToList(source, selector); // memory performs best
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource, TResult>(this MemorySelectIndexEnumerable<TSource, TResult> source)
            => source.Count;
    }
}

