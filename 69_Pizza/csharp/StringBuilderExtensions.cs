using System.Text;

namespace Pizza
{
    internal static class StringBuilderExtensions
    {
        /// <summary>
        /// Extensions for adding new lines of specific value.
        /// </summary>
        /// <param name="stringBuilder">Extended class.</param>
        /// <param name="value">Value which will be repeated.</param>
        /// <param name="numberOfLines">Number of lines that will be appended.</param>
        public static void AppendLine(this StringBuilder stringBuilder, string value, int numberOfLines)
        {
            for (int i = 0; i < numberOfLines; i++)
            {
                stringBuilder.AppendLine(value);
            }
        }
    }
}