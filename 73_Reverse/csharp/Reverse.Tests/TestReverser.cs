namespace Reverse.Tests
{
    internal class TestReverser : Reverser
    {
        public TestReverser(int arraySize) : base(arraySize) { }

        public int[] GetArray()
        {
            return _array;
        }

        public void SetArray(int[] array)
        {
            _array = array;
        }
    }
}
