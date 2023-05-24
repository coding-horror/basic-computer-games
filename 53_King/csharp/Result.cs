namespace King;

internal record struct Result (bool IsGameOver, string Message)
{
    internal static Result GameOver(string message) => new(true, message);
    internal static Result Continue => new(false, "");
}
