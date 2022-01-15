namespace BombsAwayConsole;

/// <summary>
/// Implements <see cref="BombsAwayGame.IUserInterface"/> by writing to and reading from <see cref="Console"/>.
/// </summary>
internal class ConsoleUserInterface : BombsAwayGame.IUserInterface
{
    /// <summary>
    /// Write message to console.
    /// </summary>
    /// <param name="message">Message to display.</param>
    public void Output(string message)
    {
        Console.WriteLine(message);
    }

    /// <summary>
    /// Write choices with affixed indexes, allowing the user to choose by index.
    /// </summary>
    /// <param name="message">Message to display.</param>
    /// <param name="choices">Choices to display.</param>
    /// <returns>Choice that user picked.</returns>
    public int Choose(string message, IList<string> choices)
    {
        IEnumerable<string> choicesWithIndexes = choices.Select((choice, index) => $"{choice}({index + 1})");
        string choiceText = string.Join(", ", choicesWithIndexes);
        Output($"{message} -- {choiceText}");

        ISet<ConsoleKey> allowedKeys = ConsoleKeysFromList(choices);
        ConsoleKey? choice;
        do
        {
            choice = ReadChoice(allowedKeys);
            if (choice is null)
            {
                Output("TRY AGAIN...");
            }
        }
        while (choice is null);

        return ListIndexFromConsoleKey(choice.Value);
    }

    /// <summary>
    /// Convert the given list to its <see cref="ConsoleKey"/> equivalents. This generates keys that map
    /// the first element to <see cref="ConsoleKey.D1"/>, the second element to <see cref="ConsoleKey.D2"/>,
    /// and so on, up to the last element of the list.
    /// </summary>
    /// <param name="list">List whose elements will be converted to <see cref="ConsoleKey"/> equivalents.</param>
    /// <returns><see cref="ConsoleKey"/> equivalents from <paramref name="list"/>.</returns>
    private ISet<ConsoleKey> ConsoleKeysFromList(IList<string> list)
    {
        IEnumerable<int> indexes = Enumerable.Range((int)ConsoleKey.D1, list.Count);
        return new HashSet<ConsoleKey>(indexes.Cast<ConsoleKey>());
    }

    /// <summary>
    /// Convert the given console key to its list index equivalent. This assumes the key was generated from
    /// <see cref="ConsoleKeysFromList(IList{string})"/>
    /// </summary>
    /// <param name="key">Key to convert to its list index equivalent.</param>
    /// <returns>List index equivalent of key.</returns>
    private int ListIndexFromConsoleKey(ConsoleKey key)
    {
        return key - ConsoleKey.D1;
    }

    /// <summary>
    /// Read a key from the console and return it if it is in the given allowed keys.
    /// </summary>
    /// <param name="allowedKeys">Allowed keys.</param>
    /// <returns>Key read from <see cref="Console"/>, if it is in <paramref name="allowedKeys"/>; null otherwise./></returns>
    private ConsoleKey? ReadChoice(ISet<ConsoleKey> allowedKeys)
    {
        ConsoleKeyInfo keyInfo = ReadKey();
        return allowedKeys.Contains(keyInfo.Key) ? keyInfo.Key : null;
    }

    /// <summary>
    /// Read key from <see cref="Console"/>.
    /// </summary>
    /// <returns>Key read from <see cref="Console"/>.</returns>
    private ConsoleKeyInfo ReadKey()
    {
        ConsoleKeyInfo result = Console.ReadKey(intercept: false);
        // Write a blank line to the console so the displayed key is on its own line.
        Console.WriteLine();
        return result;
    }

    /// <summary>
    /// Allow user to choose 'Y' or 'N' from <see cref="Console"/>.
    /// </summary>
    /// <param name="message">Message to display.</param>
    /// <returns>True if user chose 'Y', false if user chose 'N'.</returns>
    public bool ChooseYesOrNo(string message)
    {
        Output(message);
        ConsoleKey? choice;
        do
        {
            choice = ReadChoice(new HashSet<ConsoleKey>(new[] { ConsoleKey.Y, ConsoleKey.N }));
            if (choice is null)
            {
                Output("ENTER Y OR N");
            }
        }
        while (choice is null);

        return choice.Value == ConsoleKey.Y;
    }

    /// <summary>
    /// Get integer by reading a line from <see cref="Console"/>.
    /// </summary>
    /// <returns>Integer read from <see cref="Console"/>.</returns>
    public int InputInteger()
    {
        bool resultIsValid;
        int result;
        do
        {
            string? integerText = Console.ReadLine();
            resultIsValid = int.TryParse(integerText, out result);
            if (!resultIsValid)
            {
                Output("PLEASE ENTER A NUMBER");
            }
        }
        while (!resultIsValid);

        return result;
    }
}
