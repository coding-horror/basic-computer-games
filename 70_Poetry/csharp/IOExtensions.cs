namespace Poetry;

internal static class IOExtensions
{

    internal static void WritePhrase(this IReadWrite io, Context context)
        => Phrase.GetPhrase(context).Write(io, context);
}
