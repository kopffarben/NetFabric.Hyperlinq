using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerable
    {
        [Pure]
        public static EmptyEnumerable<TSource> Empty<TSource>() =>
            EmptyEnumerable<TSource>.Instance;

        public partial class EmptyEnumerable<TSource>
            : IValueReadOnlyList<TSource, EmptyEnumerable<TSource>.DisposableEnumerator>
            , IList<TSource>
        {
            public static readonly EmptyEnumerable<TSource> Instance = new EmptyEnumerable<TSource>();

            private EmptyEnumerable() { }

            public int Count 
                => 0;

            public TSource this[int index] 
                => Throw.IndexOutOfRangeException<TSource>(); 

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator GetEnumerator() => new Enumerator();
            DisposableEnumerator IValueEnumerable<TSource, EmptyEnumerable<TSource>.DisposableEnumerator>.GetEnumerator() => new DisposableEnumerator();
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => new DisposableEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => new DisposableEnumerator();

            [MaybeNull]
            TSource IList<TSource>.this[int index]
            {
                get => this[index];
                set => throw new NotImplementedException();
            }

            bool ICollection<TSource>.IsReadOnly  
                => true;

            void ICollection<TSource>.CopyTo(TSource[] array, int arrayIndex) 
            { }
            void ICollection<TSource>.Add(TSource item) 
                => throw new NotImplementedException();
            void ICollection<TSource>.Clear() 
                => throw new NotImplementedException();
            bool ICollection<TSource>.Contains(TSource item) 
                => false;
            bool ICollection<TSource>.Remove(TSource item) 
                => throw new NotImplementedException();
            int IList<TSource>.IndexOf(TSource item)
                => -1;
            void IList<TSource>.Insert(int index, TSource item)
                => throw new NotImplementedException();
            void IList<TSource>.RemoveAt(int index)
                => throw new NotImplementedException();

            public readonly struct Enumerator
            {
                [ExcludeFromCodeCoverage]
                [MaybeNull]                
                public readonly TSource Current
                    => default!;

                public readonly bool MoveNext() 
                    => false;
            }

            public readonly struct DisposableEnumerator
                : IEnumerator<TSource>
            {
                [ExcludeFromCodeCoverage]
                [MaybeNull]
                public readonly TSource Current
                    => default!;

                [ExcludeFromCodeCoverage]
                readonly object? IEnumerator.Current 
                    => default;

                public readonly bool MoveNext()
                    => false;

                [ExcludeFromCodeCoverage]
                public readonly void Reset() { }

                public readonly void Dispose() { }
            }
        }
    }
}

