using System;
using System.Linq;
using SuperStarTrek.Commands;
using SuperStarTrek.Space;
using static System.StringComparison;

namespace SuperStarTrek
{
    internal class Input
    {
        private readonly Output _output;

        internal Input(Output output)
        {
            _output = output;
        }

        internal void WaitForAnyKeyButEnter(string prompt)
        {
            _output.Write($"Hit any key but Enter {prompt} ");
            while (Console.ReadKey(intercept: true).Key == ConsoleKey.Enter);
        }

        internal string GetString(string prompt)
        {
            _output.Prompt(prompt);
            return Console.ReadLine();
        }

        internal float GetNumber(string prompt)
        {
            _output.Prompt(prompt);

            while (true)
            {
                var response = Console.ReadLine();
                if (float.TryParse(response, out var value))
                {
                    return value;
                }

                _output.WriteLine("!Number expected - retry input line");
                _output.Prompt();
            }
        }

        internal (float X, float Y) GetCoordinates(string prompt)
        {
            _output.Prompt($"{prompt} (X,Y)");
            var responses = ReadNumbers(2);
            return (responses[0], responses[1]);
        }

        internal bool TryGetNumber(string prompt, float minValue, float maxValue, out float value)
        {
            value = GetNumber($"{prompt} ({minValue}-{maxValue})");

            return value >= minValue && value <= maxValue;
        }

        internal bool GetString(string replayPrompt, string trueValue) =>
            GetString(replayPrompt).Equals(trueValue, InvariantCultureIgnoreCase);

        internal Command GetCommand()
        {
            while(true)
            {
                var response = GetString("Command");

                if (response.Length >= 3 &&
                    Enum.TryParse(response.Substring(0, 3), ignoreCase: true, out Command parsedCommand))
                {
                    return parsedCommand;
                }

                _output.WriteLine("Enter one of the following:");
                foreach (var command in Enum.GetValues(typeof(Command)).OfType<Command>())
                {
                    _output.WriteLine($"  {command}  ({command.GetDescription()})");
                }
                _output.WriteLine();
            }
        }

        internal bool TryGetCourse(string prompt, string officer, out Course course)
        {
            if (!TryGetNumber(prompt, 1, 9, out var direction))
            {
                _output.WriteLine($"{officer} reports, 'Incorrect course data, sir!'");
                course = default;
                return false;
            }

            course = new Course(direction);
            return true;
        }

        internal bool GetYesNo(string prompt, YesNoMode mode)
        {
            _output.Prompt($"{prompt} (Y/N)");
            var response = Console.ReadLine().ToUpperInvariant();

            return (mode, response) switch
            {
                (YesNoMode.FalseOnN, "N") => false,
                (YesNoMode.FalseOnN, _) => true,
                (YesNoMode.TrueOnY, "Y") => true,
                (YesNoMode.TrueOnY, _) => false,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Invalid value")
            };
        }

        private float[] ReadNumbers(int quantity)
        {
            var numbers = new float[quantity];
            var index = 0;
            bool tryAgain;

            do
            {
                tryAgain = false;
                var responses = Console.ReadLine().Split(',');
                if (responses.Length > quantity)
                {
                    _output.WriteLine("!Extra input ingored");
                }

                for (; index < responses.Length; index++)
                {
                    if (!float.TryParse(responses[index], out numbers[index]))
                    {
                        _output.WriteLine("!Number expected - retry input line");
                        _output.Prompt();
                        tryAgain = true;
                        break;
                    }
                }
            } while (tryAgain);

            if (index < quantity)
            {
                _output.Prompt("?");
                var responses = ReadNumbers(quantity - index);
                for (int i = 0; i < responses.Length; i++, index++)
                {
                    numbers[index] = responses[i];
                }
            }

            return numbers;
        }

        internal enum YesNoMode
        {
            TrueOnY,
            FalseOnN
        }
    }
}
