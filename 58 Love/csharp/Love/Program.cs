using System;
using System.Reflection;

namespace Love
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DisplayIntro();

            var message = Input.ReadLine("Your message, please");
            var pattern = new LovePattern();

            var source = new SourceCharacters(pattern.LineLength, message);

            using var destination = Console.OpenStandardOutput();

            pattern.Write(source, destination);
        }

        private static void DisplayIntro()
        {
            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Love.Strings.Intro.txt");
            using var stdout = Console.OpenStandardOutput();

            stream.CopyTo(stdout);
        }
    }
}
