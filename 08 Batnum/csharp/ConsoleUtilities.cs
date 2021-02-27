using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batnum
{
    public static class ConsoleUtilities
    {
        /// <summary>
        /// Ask the user a question and expects a comma separated pair of numbers representing a number range in response
        /// the range provided must have a maximum which is greater than the minimum 
        /// </summary>
        /// <param name="question">The question to ask</param>
        /// <param name="minimum">The minimum value expected</param>
        /// <param name="maximum">The maximum value expected</param>
        /// <returns>A pair of numbers representing the minimum and maximum of the range</returns>
        public static (int min, int max) AskNumberRangeQuestion(string question, Func<int, int, bool> Validate)
        {
            while (true)
            {
                Console.Write(question);
                Console.Write(" ");
                string[] rawInput = Console.ReadLine().Split(',');
                if (rawInput.Length == 2)
                {
                    if (int.TryParse(rawInput[0], out int min) && int.TryParse(rawInput[1], out int max))
                    {
                        if (Validate(min, max))
                        {
                            return (min, max);
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Ask the user a question and expects a number in response
        /// </summary>
        /// <param name="question">The question to ask</param>
        /// <param name="minimum">A minimum value expected</param>
        /// <param name="maximum">A maximum value expected</param>
        /// <returns>The number the user entered</returns>
        public static int AskNumberQuestion(string question, Func<int, bool> Validate)
        {
            while (true)
            {
                Console.Write(question);
                Console.Write(" ");
                string rawInput = Console.ReadLine();
                if (int.TryParse(rawInput, out int number))
                {
                    if (Validate(number))
                    {
                        return number;
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Align content to center of console.
        /// </summary>
        /// <param name="content">Content to center</param>
        /// <returns>Center aligned text</returns>
        public static string CenterText(string content)
        {
            int windowWidth = Console.WindowWidth;
            return String.Format("{0," + ((windowWidth / 2) + (content.Length / 2)) + "}", content);
        }

        /// <summary>
        ///     Writes the specified data, followed by the current line terminator, to the standard output stream, while wrapping lines that would otherwise break words.
        ///     source: https://stackoverflow.com/questions/20534318/make-console-writeline-wrap-words-instead-of-letters
        /// </summary>
        /// <param name="paragraph">The value to write.</param>
        /// <param name="tabSize">The value that indicates the column width of tab characters.</param>
        public static void WriteLineWordWrap(string paragraph, int tabSize = 4)
        {
            string[] lines = paragraph
                .Replace("\t", new String(' ', tabSize))
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                string process = lines[i];
                List<String> wrapped = new List<string>();

                while (process.Length > Console.WindowWidth)
                {
                    int wrapAt = process.LastIndexOf(' ', Math.Min(Console.WindowWidth - 1, process.Length));
                    if (wrapAt <= 0) break;

                    wrapped.Add(process.Substring(0, wrapAt));
                    process = process.Remove(0, wrapAt + 1);
                }

                foreach (string wrap in wrapped)
                {
                    Console.WriteLine(wrap);
                }

                Console.WriteLine(process);
            }
        }
    }
}
