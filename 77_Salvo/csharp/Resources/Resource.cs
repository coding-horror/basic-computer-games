using System.Reflection;
using System.Runtime.CompilerServices;

namespace Salvo.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Title => GetStream();
        public static Stream YouHaveMoreShotsThanSquares => GetStream();
        public static Stream YouWon => GetStream();
        public static Stream IHaveMoreShotsThanSquares => GetStream();
        public static Stream IWon => GetStream();
        public static Stream Illegal => GetStream();
    }

    internal static class Strings
    {
        public static string WhereAreYourShips => GetString();
        public static string YouHaveShots(int number) => Format(number);
        public static string IHaveShots(int number) => Format(number);
        public static string YouHit(string shipName) => Format(shipName);
        public static string ShotBefore(int turnNumber) => Format(turnNumber);
        public static string Turn(int number) => Format(number);
    }

    internal static class Prompts
    {
        public static string Coordinates => GetString();
        public static string Start => GetString();
        public static string SeeShots => GetString();
    }

    private static string Format<T>(T value, [CallerMemberName] string? name = null) 
        => string.Format(GetString(name), value);

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