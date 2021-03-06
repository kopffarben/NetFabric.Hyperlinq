using NetFabric.Assertive;
using System;
using Xunit;

namespace NetFabric.Hyperlinq.UnitTests.Partitioning.Skip
{
    public class ReadOnlySpanTests
    {
        [Theory]
        [MemberData(nameof(TestData.SkipEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SkipSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.SkipMultiple), MemberType = typeof(TestData))]
        public void Skip_With_ValidData_Must_Succeed(int[] source, int count)
        {
            // Arrange
            var expected = System.Linq.Enumerable.Skip(source, count);

            // Act
            var result = Array.Skip((ReadOnlySpan<int>)source.AsSpan(), count);

            // Assert
            using var expectedEnumerator = expected.GetEnumerator();
            for (var index = 0; true; index++)
            {
                var resultEnded = index == result.Length;
                var expectedEnded = !expectedEnumerator.MoveNext();

                if (resultEnded != expectedEnded)
                    throw new Exception("Not same size");

                if (resultEnded)
                    break;

                if (result[index] != expectedEnumerator.Current)
                    throw new Exception("Items are not equal");
            }
        }
    }
}