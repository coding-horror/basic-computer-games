namespace Digits;

internal static class IOExtensions
{
    internal static IEnumerable<int> Read10Digits(this IReadWrite io, string prompt, Stream retryText)
    {
        while (true)
        {
            var numbers = new float[10];
            io.ReadNumbers(prompt, numbers);

            if (numbers.All(n => n == 0 || n == 1 || n == 2))
            {
                return numbers.Select(n => (int)n);
            }    

            io.Write(retryText);
        }
    }
}