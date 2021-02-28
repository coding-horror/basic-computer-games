using System;
using System.Linq;
using static System.StringComparison;

namespace SuperStarTrek
{
    internal class Input
    {
        private readonly Output _output;

        public Input(Output output)
        {
            _output = output;
        }

        public void WaitForAnyKeyButEnter(string prompt)
        {
            _output.Write($"Hit any key but Enter {prompt} ");
            while (Console.ReadKey(intercept: true).Key == ConsoleKey.Enter);
        }

        public string GetString(string prompt)
        {
            _output.Prompt(prompt);
            return Console.ReadLine();
        }

        public double GetNumber(string prompt)
        {
            _output.Prompt(prompt);

            while (true)
            {
                var response = Console.ReadLine();
                if (double.TryParse(response, out var value))
                {
                    return value;
                }

                _output.WriteLine("!Number expected - retry input line");
                _output.Prompt();
            }
        }

        public bool TryGetNumber(string prompt, double minValue, double maxValue, out double value)
        {
            value = GetNumber($"{prompt} ({minValue}-{maxValue})");

            return value >= minValue && value <= maxValue;
        }

        internal bool GetString(string replayPrompt, string trueValue)
            => GetString(replayPrompt).Equals(trueValue, InvariantCultureIgnoreCase);

        public Command GetCommand()
        {
            while(true)
            {
                var response = GetString("Command");

                if (response != "" &&
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
    }
}
