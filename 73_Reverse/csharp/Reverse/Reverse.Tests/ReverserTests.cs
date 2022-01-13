using FsCheck.Xunit;
using Reverse.Tests.Generators;
using System;
using System.Linq;
using Xunit;

namespace Reverse.Tests
{
    public class ReverserTests
    {
        [Fact]
        public void Constructor_CannotAcceptNumberLessThanZero()
        {
            Action action = () => new Reverser(0);

            Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Property(Arbitrary = new[] { typeof(PositiveIntegerGenerator) })]
        public void Constructor_CreatesRandomArrayOfSpecifiedLength(int size)
        {
            var sut = new TestReverser(size);

            Assert.Equal(size, sut.GetArray().Length);
        }

        [Property(Arbitrary = new[] { typeof(PositiveIntegerGenerator) })]
        public void ConstructorArray_MaxElementValueIsEqualToSize(int size)
        {
            var sut = new TestReverser(size);

            Assert.Equal(size, sut.GetArray().Max());
        }

        [Property(Arbitrary = new[] { typeof(PositiveIntegerGenerator) })]
        public void ConstructorArray_ReturnsRandomArrayWithDistinctElements(int size)
        {
            var sut = new TestReverser(size);
            var array = sut.GetArray();
            var arrayGroup = array.GroupBy(x => x);
            var duplicateFound = arrayGroup.Any(x => x.Count() > 1);

            Assert.False(duplicateFound);
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 })]
        [InlineData(new int[] { 1, 2 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 3, 2, 1 })]
        public void Reverse_WillReverseEntireArray(int[] input, int[] output)
        {
            var sut = new TestReverser(1);
            sut.SetArray(input);

            sut.Reverse(input.Length);

            Assert.True(sut.GetArray().SequenceEqual(output));
        }

        [Fact]
        public void Reverse_WithSpecifiedIndex_ReversesItemsUpToThatIndex()
        {
            var input = new int[] { 1, 2, 3, 4 };
            var output = new int[] { 2, 1, 3, 4 };
            var sut = new TestReverser(1);
            sut.SetArray(input);

            sut.Reverse(2);

            Assert.True(sut.GetArray().SequenceEqual(output));
        }

        [Fact]
        public void Reverse_WithIndexOne_DoesNothing()
        {
            var input = new int[] { 1, 2 };
            var output = new int[] { 1, 2 };
            var sut = new TestReverser(1);
            sut.SetArray(input);

            sut.Reverse(1);

            Assert.True(sut.GetArray().SequenceEqual(output));
        }

        [Fact]
        public void Reverse_WithIndexGreaterThanArrayLength_DoesNothing()
        {
            var input = new int[] { 1, 2 };
            var output = new int[] { 1, 2 };
            var sut = new TestReverser(1);
            sut.SetArray(input);

            sut.Reverse(sut.GetArray().Length + 1);

            Assert.True(sut.GetArray().SequenceEqual(output));
        }

        [Fact]
        public void Reverse_WithIndexLessThanZero_DoesNothing()
        {
            var input = new int[] { 1, 2 };
            var output = new int[] { 1, 2 };
            var sut = new TestReverser(1);
            sut.SetArray(input);

            sut.Reverse(-1);

            Assert.True(sut.GetArray().SequenceEqual(output));
        }

        [Theory]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 1 })]
        public void IsArrayInAscendingOrder_WhenArrayElementsAreInNumericAscendingOrder_ReturnsTrue(int[] input)
        {
            var sut = new TestReverser(1);
            sut.SetArray(input);

            var result = sut.IsArrayInAscendingOrder();

            Assert.True(result);
        }

        [Fact]
        public void IsArrayInOrder_WhenArrayElementsAreNotInNumericAscendingOrder_ReturnsFalse()
        {
            var sut = new TestReverser(1);
            sut.SetArray(new int[] { 2, 1 });

            var result = sut.IsArrayInAscendingOrder();

            Assert.False(result);
        }

        [Fact]
        public void GetArrayString_ReturnsSpaceSeparatedElementsOfArrayInStringFormat()
        {
            var sut = new TestReverser(1);
            sut.SetArray(new int[] { 1, 2 });

            var result = sut.GetArrayString();

            Assert.Equal(" 1  2 ", result);
        }
    }
}
