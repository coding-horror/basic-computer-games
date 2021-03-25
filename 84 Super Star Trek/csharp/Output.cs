using System;

namespace SuperStarTrek
{
    internal class Output
    {
        public Output Write(string text)
        {
            Console.Write(text);
            return this;
        }

        public Output Write(string format, params object[] args)
        {
            Console.Write(format, args);
            return this;
        }

        public Output WriteLine(string text = "")
        {
            Console.WriteLine(text);
            return this;
        }


        public Output NextLine()
        {
            Console.WriteLine();
            return this;
        }


        public Output Prompt(string text = "")
        {
            Console.Write($"{text}? ");
            return this;
        }

    }
}
