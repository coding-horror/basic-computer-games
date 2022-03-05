namespace Bunny
{
    internal class Bunny
    {
        private const int asciiBase = 64;
        private readonly int[] bunnyData = {
            2,21,14,14,25,
            1,2,-1,0,2,45,50,-1,0,5,43,52,-1,0,7,41,52,-1,
            1,9,37,50,-1,2,11,36,50,-1,3,13,34,49,-1,4,14,32,48,-1,
            5,15,31,47,-1,6,16,30,45,-1,7,17,29,44,-1,8,19,28,43,-1,
            9,20,27,41,-1,10,21,26,40,-1,11,22,25,38,-1,12,22,24,36,-1,
            13,34,-1,14,33,-1,15,31,-1,17,29,-1,18,27,-1,
            19,26,-1,16,28,-1,13,30,-1,11,31,-1,10,32,-1,
            8,33,-1,7,34,-1,6,13,16,34,-1,5,12,16,35,-1,
            4,12,16,35,-1,3,12,15,35,-1,2,35,-1,1,35,-1,
            2,34,-1,3,34,-1,4,33,-1,6,33,-1,10,32,34,34,-1,
            14,17,19,25,28,31,35,35,-1,15,19,23,30,36,36,-1,
            14,18,21,21,24,30,37,37,-1,13,18,23,29,33,38,-1,
            12,29,31,33,-1,11,13,17,17,19,19,22,22,24,31,-1,
            10,11,17,18,22,22,24,24,29,29,-1,
            22,23,26,29,-1,27,29,-1,28,29,-1,4096
        };

        public void Run()
        {
            PrintString(33, "BUNNY");
            PrintString(15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            PrintLines(3);

            // Set up a BASIC-ish data object
            BasicData data = new (bunnyData);

            // Get the first five data values into an array.
            // These are the characters we are going to print.
            // Unlike the original program, we are only converting
            // them to ASCII once.
            var a = new char[5];
            for (var i = 0; i < 5; ++i)
            {
                a[i] = (char)(asciiBase + data.Read());
            }
            PrintLines(6);

            PrintLines(1);
            var col = 0;
            while (true)
            {
                var x = data.Read();
                if (x < 0) // Start a new line
                {
                    PrintLines(1);
                    col = 0;
                    continue;
                }
                if (x > 128) break; // End processing
                col += PrintSpaces(x - col); // Move to TAB position x (sort of)
                var y = data.Read(); // Read the next value
                for (var i = x; i <= y; ++i)
                {
                    // var j = i - 5 * (i / 5); // BASIC didn't have a modulus operator
                    Console.Write(a[i % 5]);
                    // Console.Write(a[col % 5]); // This works, too
                    ++col;
                }
            }
            PrintLines(6);
        }
        private static void PrintLines(int count)
        {
            for (var i = 0; i < count; ++i)
                Console.WriteLine();
        }
        private static int PrintSpaces(int count)
        {
            for (var i = 0; i < count; ++i)
                Console.Write(' ');
            return count;
        }
        public static void PrintString(int tab, string value, bool newLine = true)
        {
            PrintSpaces(tab);
            Console.Write(value);
            if (newLine) Console.WriteLine();
        }

    }
}
