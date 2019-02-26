using System;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueReadOnlyList
    {
        public static TSource? SingleOrNull<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueReadOnlyList<TSource, TEnumerator>
            where TEnumerator : struct, IValueEnumerator<TSource>
            where TSource : struct
        {
            if (source == null) ThrowHelper.ThrowArgumentNullException(nameof(source));

            var count = source.Count();
            if (count == 0) return null;
            if (count > 1) ThrowHelper.ThrowNotSingleSequence<TSource>();

            return source[0];
        }

        public static TSource? SingleOrNull<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueReadOnlyList<TSource, TEnumerator>
            where TEnumerator : struct, IValueEnumerator<TSource>
            where TSource : struct
        {
            if (source == null) ThrowHelper.ThrowArgumentNullException(nameof(source));

            var count = source.Count();
            if (count == 0) return null;

            for (var index = 0; index < count; index++)
            {
                var first = source[index];
                if (predicate(first))
                {
                    // found first, keep going until end or find second
                    for(var index2 = index + 1; index2 < count; index2++)
                    {
                        if (predicate(source[index2]))
                            ThrowHelper.ThrowNotSingleSequence<TSource>();      
                    }
                    return first;
                }
            }
            return null;
        }
    }
}
