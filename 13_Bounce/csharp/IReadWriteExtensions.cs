namespace Bounce;

internal static class IReadWriteExtensions
{
    internal static float ReadParameter(this IReadWrite io, string parameter)
    {
        var value = io.ReadNumber(parameter);
        io.WriteLine();
        return value;
    }
}