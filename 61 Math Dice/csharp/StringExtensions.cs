namespace MathDice
{
    public static class StringExtensions
    {
        private const int ConsoleWidth = 120; // default console width

        public static string CentreAlign(this string value)
        {
            int spaces = ConsoleWidth - value.Length;
            int leftPadding = spaces / 2 + value.Length;

            return value.PadLeft(leftPadding).PadRight(ConsoleWidth);
        }
    }
}
