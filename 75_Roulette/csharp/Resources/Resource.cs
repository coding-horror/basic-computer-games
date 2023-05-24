using System.Reflection;
using System.Runtime.CompilerServices;
using Games.Common.Randomness;

namespace Roulette.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Title => GetStream();
        public static Stream Instructions => GetStream();
        public static Stream BetAlready => GetStream();
        public static Stream Spinning => GetStream();
        public static Stream LastDollar => GetStream();
        public static Stream BrokeHouse => GetStream();
        public static Stream Thanks => GetStream();
    }

    internal static class Strings
    {
        public static string Black(int number) => Slot(number);
        public static string Red(int number) => Slot(number);
        private static string Slot(int number, [CallerMemberName] string? colour = null)
            => string.Format(GetString(), number, colour);

        public static string Lose(Bet bet) => Outcome(bet.Wager, bet.Number);
        public static string Win(Bet bet) => Outcome(bet.Payout, bet.Number);
        private static string Outcome(int amount, int number, [CallerMemberName] string? winlose = null)
            => string.Format(GetString(), winlose, amount, number);

        public static string Totals(int me, int you) => string.Format(GetString(), me, you);

        public static string Check(IRandom random, string payee, int amount)
            => string.Format(GetString(), random.Next(100), DateTime.Now, payee, amount);
    }

    internal static class Prompts
    {
        public static string Instructions => GetPrompt();
        public static string HowManyBets => GetPrompt();
        public static string Bet(int number) => string.Format(GetPrompt(), number);
        public static string Again => GetPrompt();
        public static string Check => GetPrompt();
    }

    private static string GetPrompt([CallerMemberName] string? name = null) => GetString($"{name}Prompt");

    private static string GetString([CallerMemberName] string? name = null)
    {
        using var stream = GetStream(name);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static Stream GetStream([CallerMemberName] string? name = null) =>
        Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(Resource).Namespace}.{name}.txt")
            ?? throw new Exception($"Could not find embedded resource stream '{name}'.");
}