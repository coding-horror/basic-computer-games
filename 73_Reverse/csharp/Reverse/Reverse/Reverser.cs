using System;
using System.Text;

namespace Reverse
{
    public class Reverser
    {
        protected int[] _array;

        public Reverser(int arraySize)
        {
            _array = CreateRandomArray(arraySize);
        }

        public void Reverse(int index)
        {
            Reverse(_array, index);
        }

        public bool IsArrayInAscendingOrder()
        {
            return IsArrayInAscendingOrder(_array);
        }

        public static void Reverse(int[] arrayToReverse, int indexToReverseTo)
        {
            if (indexToReverseTo > arrayToReverse.Length)
            {
                return;
            }

            for (int i = 0; i < indexToReverseTo / 2; i++)
            {
                int temp = arrayToReverse[i];
                int upperIndex = indexToReverseTo - 1 - i;
                arrayToReverse[i] = arrayToReverse[upperIndex];
                arrayToReverse[upperIndex] = temp;
            }
        }

        public static int[] CreateRandomArray(int size)
        {
            var array = new int[size];
            for (int i = 1; i <= size; i++)
            {
                array[i - 1] = i;
            }

            var rnd = new Random();

            for (int i = size; i > 1;)
            {
                int k = rnd.Next(i);
                --i;
                int temp = array[i];
                array[i] = array[k];
                array[k] = temp;
            }
            return array;
        }

        public static bool IsArrayInAscendingOrder(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < array[i - 1])
                {
                    return false;
                }
            }

            return true;
        }

        public string GetArrayString()
        {
            var sb = new StringBuilder();

            foreach (int i in _array)
            {
                sb.Append(" " + i + " ");
            }

            return sb.ToString();
        }
    }
}
