using System;
using System.Collections.Immutable;
using System.Linq;

namespace Game
{
    /// <summary>
    /// Contains functions for getting input from the end user.
    /// </summary>
    public static class Controller
    {
        /// <summary>
        /// Maps the letters for each color to the integer value representing
        /// that color.
        /// </summary>
        /// <remarks>
        /// We derive this map from the Colors list rather than defining the
        /// entries directly in order to keep all color related information
        /// in one place.  (This makes it easier to change the color options
        /// later.)
        /// </remarks>
        private static ImmutableDictionary<char, int> ColorsByKey = Colors.List
            .Select((info, index) => (key: info.ShortName, index))
            .ToImmutableDictionary(entry => entry.key, entry => entry.index);

        /// <summary>
        /// Gets the number of colors to use in the secret code.
        /// </summary>
        public static int GetNumberOfColors()
        {
            var maximumColors = Colors.List.Length;
            var colors = 0;

            while (colors < 1 || colors > maximumColors)
            {
                colors = GetInteger(View.PromptNumberOfColors);
                if (colors > maximumColors)
                    View.NotifyTooManyColors(maximumColors);
            }

            return colors;
        }

        /// <summary>
        /// Gets the number of positions in the secret code.
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfPositions()
        {
            // Note: We should probably ensure that the user enters a sane
            //  number of positions here.  (Things go south pretty quickly
            //  with a large number of positions.)  But since the original
            //  program did not, neither will we.
            return GetInteger(View.PromptNumberOfPositions);
        }

        /// <summary>
        /// Gets the number of rounds to play.
        /// </summary>
        public static int GetNumberOfRounds()
        {
            // Note: Silly numbers of rounds (like 0, or a negative number)
            //  are harmless, but it would still make sense to validate.
            return GetInteger(View.PromptNumberOfRounds);
        }

        /// <summary>
        /// Gets a command from the user.
        /// </summary>
        /// <param name="moveNumber">
        /// The current move number.
        /// </param>
        /// <param name="positions">
        /// The number of code positions.
        /// </param>
        /// <param name="colors">
        /// The maximum number of code colors.
        /// </param>
        /// <returns>
        /// The entered command and guess (if applicable).
        /// </returns>
        public static (Command command, Code? guess) GetCommand(int moveNumber, int positions, int colors)
        {
            while (true)
            {
                View.PromptGuess (moveNumber);

                var input = Console.ReadLine();
                if (input is null)
                    Environment.Exit(0);

                switch (input.ToUpperInvariant())
                {
                    case "BOARD":
                        return (Command.ShowBoard, null);
                    case "QUIT":
                        return (Command.Quit, null);
                    default:
                        if (input.Length != positions)
                            View.NotifyBadNumberOfPositions();
                        else
                        if (input.FindFirstIndex(c => !TranslateColor(c).HasValue) is int invalidPosition)
                            View.NotifyInvalidColor(input[invalidPosition]);
                        else
                            return (Command.MakeGuess, new Code(input.Select(c => TranslateColor(c)!.Value)));

                        break;
                }
            }
        }

        /// <summary>
        /// Waits until the user indicates that he or she is ready to continue.
        /// </summary>
        public static void WaitUntilReady()
        {
            View.PromptReady();
            var input = Console.ReadLine();
            if (input is null)
                Environment.Exit(0);
        }

        /// <summary>
        /// Gets the number of blacks and whites for the given code from the
        /// user.
        /// </summary>
        public static (int blacks, int whites) GetBlacksWhites(Code code)
        {
            while (true)
            {
                View.PromptBlacksWhites(code);

                var input = Console.ReadLine();
                if (input is null)
                    Environment.Exit(0);

                var parts = input.Split(',');

                if (parts.Length != 2)
                    View.PromptTwoValues();
                else
                if (!Int32.TryParse(parts[0], out var blacks) || !Int32.TryParse(parts[1], out var whites))
                    View.PromptValidInteger();
                else
                    return (blacks, whites);
            }
        }

        /// <summary>
        /// Gets an integer value from the user.
        /// </summary>
        private static int GetInteger(Action prompt)
        {
            while (true)
            {
                prompt();

                var input = Console.ReadLine();
                if (input is null)
                    Environment.Exit(0);

                if (Int32.TryParse(input, out var result))
                    return result;
                else
                    View.PromptValidInteger();
            }
        }

        /// <summary>
        /// Translates the given character into the corresponding color.
        /// </summary>
        private static int? TranslateColor(char c) =>
            ColorsByKey.TryGetValue(c, out var index) ? index : null;
    }
}
