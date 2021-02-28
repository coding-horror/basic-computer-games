using System;

namespace SuperStarTrek
{
    internal class Output
    {
        public void Write(string text) => Console.Write(text);
        public void Write(string format, params object[] args) => Console.Write(format, args);
        public void WriteLine(string text = "") => Console.WriteLine(text);

        public void NextLine() => Console.WriteLine();

        public void Prompt(string text = "") => Console.Write($"{text}? ");
    }
}
