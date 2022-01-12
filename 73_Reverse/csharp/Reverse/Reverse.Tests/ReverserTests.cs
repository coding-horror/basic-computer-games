using System.Linq;
using Xunit;

namespace Reverse.Tests
{
    public class ReverserTests
    {
        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 })]
        [InlineData(new int[] { 1, 2 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 3, 2, 1 })]
        public void Reverse_WillReverseEntireArray(int[] input, int[] output)
        {
            Reverser.Reverse(input, input.Length);

            Assert.True(input.SequenceEqual(output));
        }

        [Fact]
        public void Reverse_WithSpecifiedIndex_ReversesItemsUpToThatIndex()
        {
            var input = new int[] { 1, 2, 3, 4 };
            var output = new int[] { 2, 1, 3, 4 };

            Reverser.Reverse(input, 2);

            Assert.True(input.SequenceEqual(output));
        }

        [Fact]
        public void Reverse_WithIndexOne_DoesNothing()
        {
            var input = new int[] { 1, 2 };
            var output = new int[] { 1, 2 };

            Reverser.Reverse(input, 1);

            Assert.True(input.SequenceEqual(output));
        }

        [Fact]
        public void Reverse_WithIndexGreaterThanArrayLength_DoesNothing()
        {
            var input = new int[] { 1, 2 };
            var output = new int[] { 1, 2 };

            Reverser.Reverse(input, input.Length + 1);

            Assert.True(input.SequenceEqual(output));
        }
        [Theory]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 1 })]
        public void IsArrayInAscendingOrder_WhenArrayElementsAreInNumericAscendingOrder_ReturnsTrue(int[] input)
        {
            var result = Reverser.IsArrayInAscendingOrder(input);

            Assert.True(result);
        }

        [Fact]
        public void IsArrayInOrder_WhenArrayElementsAreNotInNumericAscendingOrder_ReturnsFalse()
        {
            var result = Reverser.IsArrayInAscendingOrder(new int[] { 2, 1 });

            Assert.False(result);
        }
    }
}
