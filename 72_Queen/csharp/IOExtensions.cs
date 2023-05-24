namespace Queen;

internal static class IOExtensions
{
    internal static bool ReadYesNo(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var answer = io.ReadString(prompt).ToLower();
            if (answer == "yes") { return true; }
            if (answer == "no") { return false; }

            io.Write(Streams.YesOrNo);
        }
    }

    internal static Position ReadPosition(
        this IReadWrite io,
        string prompt,
        Predicate<Position> isValid,
        Stream error,
        bool repeatPrompt = false)
    {
        while (true)
        {
            var response = io.ReadNumber(prompt);
            var number = (int)response;
            var position = new Position(number);
            if (number == response && (position.IsZero || isValid(position)))
            {
                return position;
            }

            io.Write(error);
            if (!repeatPrompt) { prompt = ""; }
        }
    }
}
