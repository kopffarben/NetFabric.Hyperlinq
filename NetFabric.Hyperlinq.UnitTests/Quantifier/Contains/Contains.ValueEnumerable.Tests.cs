using System;
using System.Collections.Generic;
using NetFabric.Assertive;
using Xunit;

namespace NetFabric.Hyperlinq.UnitTests.Quantifier.Contains
{
    public class ValueEnumerableTests
    {
        [Theory]
        [MemberData(nameof(TestData.Empty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.Single), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.Multiple), MemberType = typeof(TestData))]
        public void Contains_With_Null_And_NotContains_Must_ReturnFalse(int[] source)
        {
            // Arrange
            var value = int.MaxValue;
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Contains<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, value, null);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.Single), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.Multiple), MemberType = typeof(TestData))]
        public void Contains_With_Null_And_Contains_Must_ReturnTrue(int[] source)
        {
            // Arrange
            var value = System.Linq.Enumerable.Last(source);
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Contains<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, value, null);

            // Assert
            _ = result.Must()
                .BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Empty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.Single), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.Multiple), MemberType = typeof(TestData))]
        public void Contains_With_Comparer_And_NotContains_Must_ReturnFalse(int[] source)
        {
            // Arrange
            var value = int.MaxValue;
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Contains<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, value, EqualityComparer<int>.Default);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.Single), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.Multiple), MemberType = typeof(TestData))]
        public void Contains_With_Comparer_And_Contains_Must_ReturnTrue(int[] source)
        {
            // Arrange
            var value = System.Linq.Enumerable.Last(source);
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Contains<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, value, EqualityComparer<int>.Default);

            // Assert
            _ = result.Must()
                .BeTrue();
        }

        ////////////////////
        // Predicate
        
        [Theory]
        [MemberData(nameof(TestData.PredicateEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateMultiple), MemberType = typeof(TestData))]
        public void Contains_Predicate_With_Null_And_NotContains_Must_ReturnFalse(int[] source, Predicate<int> predicate)
        {
            // Arrange
            var value = int.MaxValue;
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Contains(value, null);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.PredicateSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateMultiple), MemberType = typeof(TestData))]
        public void Contains_Predicate_With_Null_And_Contains_Must_ReturnTrue(int[] source, Predicate<int> predicate)
        {
            // Arrange
            var value = 
                System.Linq.Enumerable.Last(
                    System.Linq.Enumerable.Where(source, predicate.AsFunc()));
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Contains(value, null);

            // Assert
            _ = result.Must()
                .BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.PredicateEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateMultiple), MemberType = typeof(TestData))]
        public void Contains_Predicate_With_Comparer_And_NotContains_Must_ReturnFalse(int[] source, Predicate<int> predicate)
        {
            // Arrange
            var value = int.MaxValue;
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Contains(value, EqualityComparer<int>.Default);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.PredicateSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateMultiple), MemberType = typeof(TestData))]
        public void Contains_Predicate_With_Comparer_And_Contains_Must_ReturnTrue(int[] source, Predicate<int> predicate)
        {
            // Arrange
            var value = 
                System.Linq.Enumerable.Last(
                    System.Linq.Enumerable.Where(source, predicate.AsFunc()));
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Contains(value, EqualityComparer<int>.Default);

            // Assert
            _ = result.Must()
                .BeTrue();
        }

        ////////////////////
        // PredicateAt
        
        [Theory]
        [MemberData(nameof(TestData.PredicateAtEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateAtSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateAtMultiple), MemberType = typeof(TestData))]
        public void Contains_PredicateAt_With_Null_And_NotContains_Must_ReturnFalse(int[] source, PredicateAt<int> predicate)
        {
            // Arrange
            var value = int.MaxValue;
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Contains(value, null);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.PredicateAtSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateAtMultiple), MemberType = typeof(TestData))]
        public void Contains_PredicateAt_With_Null_And_Contains_Must_ReturnTrue(int[] source, PredicateAt<int> predicate)
        {
            // Arrange
            var value = 
                System.Linq.Enumerable.Last(
                    System.Linq.Enumerable.Where(source, predicate.AsFunc()));
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Contains(value, null);

            // Assert
            _ = result.Must()
                .BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.PredicateAtEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateAtSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateAtMultiple), MemberType = typeof(TestData))]
        public void Contains_PredicateAt_With_Comparer_And_NotContains_Must_ReturnFalse(int[] source, PredicateAt<int> predicate)
        {
            // Arrange
            var value = int.MaxValue;
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Contains(value, EqualityComparer<int>.Default);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.PredicateAtSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateAtMultiple), MemberType = typeof(TestData))]
        public void Contains_PredicateAt_With_Comparer_And_Contains_Must_ReturnTrue(int[] source, PredicateAt<int> predicate)
        {
            // Arrange
            var value = 
                System.Linq.Enumerable.Last(
                    System.Linq.Enumerable.Where(source, predicate.AsFunc()));
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Contains(value, EqualityComparer<int>.Default);

            // Assert
            _ = result.Must()
                .BeTrue();
        }

        ////////////////////
        // Selector
        
        [Theory]
        [MemberData(nameof(TestData.SelectorEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorMultiple), MemberType = typeof(TestData))]
        public void Contains_Selector_With_Null_And_NotContains_Must_ReturnFalse(int[] source, Selector<int, string> selector)
        {
            // Arrange
            var value = "!";
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Select<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int, string>(wrapped, selector)
                .Contains(value, null);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.SelectorSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorMultiple), MemberType = typeof(TestData))]
        public void Contains_Selector_With_Null_And_Contains_Must_ReturnTrue(int[] source, Selector<int, string> selector)
        {
            // Arrange
            var value = 
                System.Linq.Enumerable.Last(
                    System.Linq.Enumerable.Select(source, selector.AsFunc()));
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Select<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int, string>(wrapped, selector)
                .Contains(value, null);

            // Assert
            _ = result.Must()
                .BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.SelectorEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorMultiple), MemberType = typeof(TestData))]
        public void Contains_Selector_With_Comparer_And_NotContains_Must_ReturnFalse(int[] source, Selector<int, string> selector)
        {
            // Arrange
            var value = "!";
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Select<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int, string>(wrapped, selector)
                .Contains(value, EqualityComparer<string>.Default);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.SelectorSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorMultiple), MemberType = typeof(TestData))]
        public void Contains_Selector_With_Comparer_And_Contains_Must_ReturnTrue(int[] source, Selector<int, string> selector)
        {
            // Arrange
            var value = 
                System.Linq.Enumerable.Last(
                    System.Linq.Enumerable.Select(source, selector.AsFunc()));
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Select<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int, string>(wrapped, selector)
                .Contains(value, EqualityComparer<string>.Default);

            // Assert
            _ = result.Must()
                .BeTrue();
        }

        ////////////////////
        // SelectorAt
        
        [Theory]
        [MemberData(nameof(TestData.SelectorAtEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorAtSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorAtMultiple), MemberType = typeof(TestData))]
        public void Contains_SelectorAt_With_Null_And_NotContains_Must_ReturnFalse(int[] source, SelectorAt<int, string> selector)
        {
            // Arrange
            var value = "!";
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Select<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int, string>(wrapped, selector)
                .Contains(value, null);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.SelectorAtSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorAtMultiple), MemberType = typeof(TestData))]
        public void Contains_SelectorAt_With_Null_And_Contains_Must_ReturnFalse(int[] source, SelectorAt<int, string> selector)
        {
            // Arrange
            var value = 
                System.Linq.Enumerable.Last(
                    System.Linq.Enumerable.Select(source, selector.AsFunc()));
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Select<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int, string>(wrapped, selector)
                .Contains(value, null);

            // Assert
            _ = result.Must()
                .BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.SelectorAtEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorAtSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorAtMultiple), MemberType = typeof(TestData))]
        public void Contains_SelectorAt_With_Comparer_And_NotContains_Must_ReturnFalse(int[] source, SelectorAt<int, string> selector)
        {
            // Arrange
            var value = "!";
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Select<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int, string>(wrapped, selector)
                .Contains(value, EqualityComparer<string>.Default);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.SelectorAtSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SelectorAtMultiple), MemberType = typeof(TestData))]
        public void Contains_SelectorAt_With_Comparer_And_Contains_Must_ReturnTrue(int[] source, SelectorAt<int, string> selector)
        {
            // Arrange
            var value = 
                System.Linq.Enumerable.Last(
                    System.Linq.Enumerable.Select(source, selector.AsFunc()));
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Select<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int, string>(wrapped, selector)
                .Contains(value, EqualityComparer<string>.Default);

            // Assert
            _ = result.Must()
                .BeTrue();
        }

        ////////////////////
        // Predicate Selector
        
        [Theory]
        [MemberData(nameof(TestData.PredicateSelectorEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateSelectorSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateSelectorMultiple), MemberType = typeof(TestData))]
        public void Contains_Predicate_Selector_With_Null_And_NotContains_Must_ReturnFalse(int[] source, Predicate<int> predicate, Selector<int, string> selector)
        {
            // Arrange
            var value = "!";
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Select(selector)
                .Contains(value, null);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.PredicateSelectorSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateSelectorMultiple), MemberType = typeof(TestData))]
        public void Contains_Predicate_Selector_With_Null_And_Contains_Must_ReturnTrue(int[] source, Predicate<int> predicate, Selector<int, string> selector)
        {
            // Arrange
            var value = 
                System.Linq.Enumerable.Last(
                    System.Linq.Enumerable.Select(
                        System.Linq.Enumerable.Where(source, predicate.AsFunc()), selector.AsFunc()));
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Select(selector)
                .Contains(value, null);

            // Assert
            _ = result.Must()
                .BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.PredicateSelectorEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateSelectorSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateSelectorMultiple), MemberType = typeof(TestData))]
        public void Contains_Predicate_Selector_With_Comparer_And_NotContains_Must_ReturnFalse(int[] source, Predicate<int> predicate, Selector<int, string> selector)
        {
            // Arrange
            var value = "!";
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Select(selector)
                .Contains(value, EqualityComparer<string>.Default);

            // Assert
            _ = result.Must()
                .BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.PredicateSelectorSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateSelectorMultiple), MemberType = typeof(TestData))]
        public void Contains_Predicate_Selector_With_Comparer_And_Contains_Must_ReturnTrue(int[] source, Predicate<int> predicate, Selector<int, string> selector)
        {
            // Arrange
            var value = 
                System.Linq.Enumerable.Last(
                    System.Linq.Enumerable.Select(
                        System.Linq.Enumerable.Where(source, predicate.AsFunc()), selector.AsFunc()));
            var wrapped = Wrap.AsValueEnumerable(source);

            // Act
            var result = ValueEnumerable
                .Where<Wrap.ValueEnumerable<int>, Wrap.Enumerator<int>, int>(wrapped, predicate)
                .Select(selector)
                .Contains(value, EqualityComparer<string>.Default);

            // Assert
            _ = result.Must()
                .BeTrue();
        }
    }
}