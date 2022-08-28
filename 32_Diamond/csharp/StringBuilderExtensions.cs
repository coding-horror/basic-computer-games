using System.Text;

namespace Diamond;

internal static class StringBuilderExtensions
{
    internal static StringBuilder PadToLength(this StringBuilder builder, int length) => 
        builder.Append(' ', length - builder.Length);
}