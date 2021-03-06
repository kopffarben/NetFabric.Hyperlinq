﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace NetFabric.Hyperlinq
{
    public static partial class AsyncValueEnumerable
    {
        [Pure]
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TEnumerable, TEnumerator, TSource, TKey>(this TEnumerable source, AsyncSelector<TSource, TKey> keySelector, CancellationToken cancellationToken = default)
            where TEnumerable : notnull, IAsyncValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IAsyncEnumerator<TSource>
            => ToDictionaryAsync<TEnumerable, TEnumerator, TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default, cancellationToken);

        [Pure]
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TEnumerable, TEnumerator, TSource, TKey>(this TEnumerable source, AsyncSelector<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default)
            where TEnumerable : notnull, IAsyncValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IAsyncEnumerator<TSource>
        {
            if (keySelector is null) Throw.ArgumentNullException(nameof(keySelector));

            cancellationToken.ThrowIfCancellationRequested();
            return ExecuteAsync(source, keySelector, comparer, cancellationToken);

            static async ValueTask<Dictionary<TKey, TSource>> ExecuteAsync(TEnumerable source, AsyncSelector<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken)
            {
                var enumerator = source.GetAsyncEnumerator(cancellationToken);
                await using (enumerator.ConfigureAwait(false))
                {
                    var dictionary = new Dictionary<TKey, TSource>(0, comparer);
                    while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                    {
                        var item = enumerator.Current;
                        dictionary.Add(
                            await keySelector(item, cancellationToken).ConfigureAwait(false),
                            item);
                    }
                    return dictionary;
                }
            }
        }

        [Pure]
        static async ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TEnumerable, TEnumerator, TSource, TKey>(this TEnumerable source, AsyncSelector<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer, AsyncPredicate<TSource> predicate, CancellationToken cancellationToken)
            where TEnumerable : notnull, IAsyncValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IAsyncEnumerator<TSource>
        {
            var enumerator = source.GetAsyncEnumerator(cancellationToken);
            await using (enumerator.ConfigureAwait(false))
            {
                var dictionary = new Dictionary<TKey, TSource>(0, comparer);
                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    var item = enumerator.Current;
                    if (await predicate(item, cancellationToken).ConfigureAwait(false))
                        dictionary.Add(
                            await keySelector(item, cancellationToken).ConfigureAwait(false),
                            item);
                }
                return dictionary;
            }
        }

        [Pure]
        static async ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TEnumerable, TEnumerator, TSource, TKey>(this TEnumerable source, AsyncSelector<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer, AsyncPredicateAt<TSource> predicate, CancellationToken cancellationToken)
            where TEnumerable : notnull, IAsyncValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IAsyncEnumerator<TSource>
        {
            var enumerator = source.GetAsyncEnumerator(cancellationToken);
            await using (enumerator.ConfigureAwait(false))
            {
                checked
                {
                    var dictionary = new Dictionary<TKey, TSource>(0, comparer);
                    for (var index = 0; await enumerator.MoveNextAsync().ConfigureAwait(false); index++)
                    {
                        var item = enumerator.Current;
                        if (await predicate(item, index, cancellationToken).ConfigureAwait(false))
                            dictionary.Add(
                                await keySelector(item, cancellationToken).ConfigureAwait(false),
                                item);
                    }
                    return dictionary;
                }
            }
        }

        [Pure]
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TEnumerable, TEnumerator, TSource, TKey, TElement>(this TEnumerable source, AsyncSelector<TSource, TKey> keySelector, AsyncSelector<TSource, TElement> elementSelector, CancellationToken cancellationToken = default)
            where TEnumerable : notnull, IAsyncValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IAsyncEnumerator<TSource>
            => ToDictionaryAsync<TEnumerable, TEnumerator, TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);

        [Pure]
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TEnumerable, TEnumerator, TSource, TKey, TElement>(this TEnumerable source, AsyncSelector<TSource, TKey> keySelector, AsyncSelector<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default)
            where TEnumerable : notnull, IAsyncValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IAsyncEnumerator<TSource>
        {
            if (keySelector is null) Throw.ArgumentNullException(nameof(keySelector));
            if (elementSelector is null) Throw.ArgumentNullException(nameof(elementSelector));

            cancellationToken.ThrowIfCancellationRequested();
            return ExecuteAsync(source, keySelector, elementSelector, comparer, cancellationToken);

            static async ValueTask<Dictionary<TKey, TElement>> ExecuteAsync(TEnumerable source, AsyncSelector<TSource, TKey> keySelector, AsyncSelector<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken)
            {
                var enumerator = source.GetAsyncEnumerator(cancellationToken);
                await using (enumerator.ConfigureAwait(false))
                {
                    var dictionary = new Dictionary<TKey, TElement>(0, comparer);
                    while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                    {
                        var item = enumerator.Current;
                        dictionary.Add(
                            await keySelector(item, cancellationToken).ConfigureAwait(false),
                            await elementSelector(item, cancellationToken).ConfigureAwait(false));
                    }
                    return dictionary;
                }
            }
        }

        [Pure]
        static async ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TEnumerable, TEnumerator, TSource, TKey, TElement>(this TEnumerable source, AsyncSelector<TSource, TKey> keySelector, AsyncSelector<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer, AsyncPredicate<TSource> predicate, CancellationToken cancellationToken)
            where TEnumerable : notnull, IAsyncValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IAsyncEnumerator<TSource>
        {
            var enumerator = source.GetAsyncEnumerator(cancellationToken);
            await using (enumerator.ConfigureAwait(false))
            {
                var dictionary = new Dictionary<TKey, TElement>(0, comparer);
                while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    var item = enumerator.Current;
                    if (await predicate(item, cancellationToken).ConfigureAwait(false))
                        dictionary.Add(
                            await keySelector(item, cancellationToken).ConfigureAwait(false), 
                            await elementSelector(item, cancellationToken).ConfigureAwait(false));
                }
                return dictionary;
            }
        }

        [Pure]
        static async ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TEnumerable, TEnumerator, TSource, TKey, TElement>(this TEnumerable source, AsyncSelector<TSource, TKey> keySelector, AsyncSelector<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer, AsyncPredicateAt<TSource> predicate, CancellationToken cancellationToken)
            where TEnumerable : notnull, IAsyncValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IAsyncEnumerator<TSource>
        {
            var enumerator = source.GetAsyncEnumerator(cancellationToken);
            await using (enumerator.ConfigureAwait(false))
            {
                checked
                {
                    var dictionary = new Dictionary<TKey, TElement>(0, comparer);
                    for (var index = 0; await enumerator.MoveNextAsync().ConfigureAwait(false); index++)
                    {
                        var item = enumerator.Current;
                        if (await predicate(item, index, cancellationToken).ConfigureAwait(false))
                            dictionary.Add(
                                await keySelector(item, cancellationToken).ConfigureAwait(false), 
                                await elementSelector(item, cancellationToken).ConfigureAwait(false));
                    }
                    return dictionary;
                }
            }
        }
    }
}
