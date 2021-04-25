using System;

namespace SuperStarTrek
{
    internal class Output
    {
        internal Output Write(string text)
        {
            Console.Write(text);
            return this;
        }

        internal Output Write(string format, params object[] args)
        {
            Console.Write(format, args);
            return this;
        }

        internal Output WriteLine(string text = "")
        {
            Console.WriteLine(text);
            return this;
        }


        internal Output NextLine()
        {
            Console.WriteLine();
            return this;
        }


        internal Output Prompt(string text = "")
        {
            Console.Write($"{text}? ");
            return this;
        }

    }
}
