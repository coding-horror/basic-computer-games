namespace Poetry;

internal class Poem
{
    internal static void Compose(IReadWrite io, IRandom random)
    {
        var context = new Context();

        while (true)
        {
            io.WritePhrase(context);

            if (!context.SkipComma && random.NextFloat() <= 0.19F && context.U != 0)
            {
                io.Write(",");
                context.U = 2;
            }
            context.SkipComma = false;

            if (random.NextFloat() <= 0.65F)
            {
                io.Write(" ");
                context.U += 1;
            }
            else
            {
                io.WriteLine();
                context.U = 0;
            }

            while (true)
            {
                context.I = random.Next(1, 6);
                context.J += 1;
                context.K += 1;

                if (context.U == 0 && context.J % 2 == 0)
                {
                    io.Write("     ");
                }

                if (context.J < 5) { break; }

                context.J = 0;
                io.WriteLine();

                if (context.K > 20)
                {
                    io.WriteLine();
                    context.U = context.K = 0;
                    context.UseGroup2 = true;
                    break;
                }
            }
        }
    }
}