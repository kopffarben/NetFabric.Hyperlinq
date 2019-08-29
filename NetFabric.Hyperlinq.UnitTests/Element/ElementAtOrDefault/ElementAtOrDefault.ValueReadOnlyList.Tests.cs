using FluentAssertions;
using System;
using Xunit;

namespace NetFabric.Hyperlinq.UnitTests
{
    public class ElementAtOrDefaultValueReadOnlyListTests
    {
        [Theory]
        [MemberData(nameof(TestData.ElementAtOutOfRange), MemberType = typeof(TestData))]
        public void ElementAtOrDefault_With_OutOfRange_Should_ReturnDefault(int[] source, int index)
        {
            // Arrange
            var wrapped = Wrap.AsValueReadOnlyList(source);

            // Act
            var result = ValueReadOnlyList.ElementAtOrDefault<Wrap.ValueReadOnlyList<int>, Wrap.Enumerator<int>, int>(wrapped, index);

            // Assert
            result.Should().Be(default);
        }

        [Theory]
        [MemberData(nameof(TestData.ElementAt), MemberType = typeof(TestData))]
        public void ElementAtOrDefault_With_ValidData_Should_Succeed(int[] source, int index)
        {
            // Arrange
            var wrapped = Wrap.AsValueReadOnlyList(source);
            var expected = System.Linq.Enumerable.ElementAtOrDefault(wrapped, index);

            // Act
            var result = ValueReadOnlyList.ElementAtOrDefault<Wrap.ValueReadOnlyList<int>, Wrap.Enumerator<int>, int>(wrapped, index);

            // Assert
            result.Should().Be(expected);
        }
    }
}