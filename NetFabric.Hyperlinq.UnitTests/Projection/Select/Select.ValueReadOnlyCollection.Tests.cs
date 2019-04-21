using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NetFabric.Hyperlinq.UnitTests
{
    public class SelectValueReadOnlyCollectionTests
    {
        [Fact]
        public void Select_With_NullSelector_Should_Throw()
        {
            // Arrange
            var source = Enumerable.Empty<int>();

            // Act
            Action action = () => ValueReadOnlyCollection.Select<
                Enumerable.EmptyEnumerable<int>, 
                Enumerable.EmptyEnumerable<int>.ValueEnumerator,
                int, string>(source.AsValueEnumerable(), null);

            // Assert
            action.Should()
                .ThrowExactly<ArgumentNullException>()
                .And
                .ParamName.Should()
                    .Be("selector");
        }

        [Theory]
        [MemberData(nameof(TestData.Select), MemberType = typeof(TestData))]
        public void Select_With_ValidData_Should_Succeed(IReadOnlyCollection<int> source, Func<int, int, string> selector, IReadOnlyCollection<string> expected)
        {
            // Arrange

            // Act
            var result = ValueReadOnlyCollection.Select<
                ReadOnlyCollection.AsValueEnumerableEnumerable<IReadOnlyCollection<int>, IEnumerator<int>, int>, 
                ReadOnlyCollection.AsValueEnumerableEnumerable<IReadOnlyCollection<int>, IEnumerator<int>, int>.ValueEnumerator, 
                int, string>(source.AsValueEnumerable(), selector);

            // Assert
            result.Should().Generate(expected);

            var index = 0;
            using(var resultEnumerator = result.GetEnumerator())
            using(var expectedEnumerator = expected.GetEnumerator())
            {
                while (true)
                {
                    var isResultCompleted = !resultEnumerator.MoveNext();
                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isResultCompleted && isExpectedCompleted)
                        break;

                    if (isResultCompleted ^ isExpectedCompleted)
                        throw new Exception($"foreach enumeration expected complete {isExpectedCompleted} but found {isResultCompleted} at index {index}.");

                    if(!resultEnumerator.Current.Equals(expectedEnumerator.Current))
                        throw new Exception($"foreach enumeration expected value {expectedEnumerator.Current} but found {resultEnumerator.Current} at index {index}.");

                    index++;
                }
            }
        }
    }
}