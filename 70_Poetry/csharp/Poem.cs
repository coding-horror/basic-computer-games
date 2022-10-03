using static Poetry.Resources.Resource;

namespace Poetry;

internal class Poem
{
    internal static void Compose(IReadWrite io, IRandom random)
    {
        io.Write(Streams.Title);

        var context = new Context(io, random);

        while (true)
        {
            context.WritePhrase();
            context.MaybeWriteComma();
            context.WriteSpaceOrNewLine();

            while (true)
            {
                context.Update(random);
                context.MaybeIndent();

                if (context.GroupNumberIsValid) { break; }

                context.ResetGroup(io);

                if (context.MaybeCompleteStanza()) { break; }
            }
        }
    }
}