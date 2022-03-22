using System.Text;

namespace Love;

internal static class StringBuilderExtensions
{
    internal static StringBuilder AppendLines(this StringBuilder builder, int count)
    {
        for (int i = 0; i < count; i++)
        {
            builder.AppendLine();
        }

        return builder;
    }
}
