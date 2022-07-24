namespace Chomp;

internal static class IOExtensions
{
    public static (float, int, int) ReadParameters(this IReadWrite io)
        => (
            (int)io.ReadNumber(Resource.Prompts.HowManyPlayers),
            io.ReadNumberWithMax(Resource.Prompts.HowManyRows, 9, Resource.Strings.TooManyRows),
            io.ReadNumberWithMax(Resource.Prompts.HowManyColumns, 9, Resource.Strings.TooManyColumns)
        );

    private static int ReadNumberWithMax(this IReadWrite io, string initialPrompt, int max, string reprompt)
    {
        var prompt = initialPrompt;

        while (true)
        {
            var response = io.ReadNumber(prompt);
            if (response <= 9) { return (int)response; }

            prompt = $"{reprompt} {initialPrompt.ToLowerInvariant()}";
        }
    }
}