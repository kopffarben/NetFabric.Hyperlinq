using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class Enumerable
    {
        public static SkipEnumerable<TEnumerable, TEnumerator, TSource> Skip<TEnumerable, TEnumerator, TSource>(this TEnumerable source, long count)
            where TEnumerable : IEnumerable<TSource>
            where TEnumerator : IEnumerator<TSource>
            => new SkipEnumerable<TEnumerable, TEnumerator, TSource>(in source, count);

        [GenericsTypeMapping("TEnumerable", typeof(SkipEnumerable<,,>))]
        [GenericsTypeMapping("TEnumerator", typeof(SkipEnumerable<,,>.Enumerator))]
        public readonly struct SkipEnumerable<TEnumerable, TEnumerator, TSource>
            : IValueEnumerable<TSource, SkipEnumerable<TEnumerable, TEnumerator, TSource>.Enumerator>
            where TEnumerable : IEnumerable<TSource>
            where TEnumerator : IEnumerator<TSource>
        {
            readonly TEnumerable source;
            readonly long count;

            internal SkipEnumerable(in TEnumerable source, long count)
            {
                this.source = source;
                this.count = count;
            }

            public Enumerator GetEnumerator() => new Enumerator(in this);

            public struct Enumerator
                : IValueEnumerator<TSource>
            {
                TEnumerator enumerator;
                long counter;

                internal Enumerator(in SkipEnumerable<TEnumerable, TEnumerator, TSource> enumerable)
                {
                    enumerator = (TEnumerator)enumerable.source.GetEnumerator();
                    counter = enumerable.count;
                }

                public TSource Current
                    => enumerator.Current;

                public bool MoveNext()
                {
                    while (counter > 0)
                    {
                        if (!enumerator.MoveNext())
                        {
                            counter = 0;
                            return false;
                        }

                        counter--;
                    }

                    return enumerator.MoveNext();                    
                }

                public void Dispose() => enumerator.Dispose();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SkipEnumerable<TEnumerable, TEnumerator, TSource> Skip(long count)
                => Enumerable.Skip<TEnumerable, TEnumerator, TSource>(source, this.count + count);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SkipTakeEnumerable<TEnumerable, TEnumerator, TSource> Take(long count)
                => Enumerable.SkipTake<TEnumerable, TEnumerator, TSource>(source, this.count, count);
        }
    }
}