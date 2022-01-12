namespace Reverse
{
    public class Reverser
    {
        public static void Reverse(int[] arrayToReverse, int indexToReverseTo)
        {
            for (int i = 0; i < indexToReverseTo / 2; i++)
            {
                int temp = arrayToReverse[i];
                int upperIndex = indexToReverseTo - 1 - i;
                arrayToReverse[i] = arrayToReverse[upperIndex];
                arrayToReverse[upperIndex] = temp;
            }
        }
    }
}
