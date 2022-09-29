namespace Poetry;

internal class Phrase
{
    private static Phrase[][] _phrases = new Phrase[][]
    {
        new Phrase[]
        {
            new("midnight dreary"),
            new("fiery eyes"),
            new("bird or fiend"),
            new("thing of evil"),
            new("prophet")
        },
        new Phrase[]
        {
            new("beguiling me", ctx => ctx.U = 2),
            new("thrilled me"),
            new("still sitting....", ctx => ctx.SkipComma = true),
            new("never flitting", ctx => ctx.U = 2),
            new("burned")
        },
        new Phrase[]
        {
            new("and my soul"),
            new("darkness there"),
            new("shall be lifted"),
            new("quoth the raven"),
            new(ctx => ctx.U != 0, "sign of parting")
        },
        new Phrase[]
        {
            new("nothing more"),
            new("yet again"),
            new("slowly creeping"),
            new("...evermore"),
            new("nevermore")
        }
    };

    private readonly Predicate<Context> _condition;
    private readonly string _text;
    private readonly Action<Context> _update;

    private Phrase(Predicate<Context> condition, string text)
        : this(condition, text, _ => { })
    {
    }

    private Phrase(string text, Action<Context> update)
        : this(_ => true, text, update)
    {
    }

    private Phrase(string text)
        : this(_ => true, text, _ => { })
    {
    }

    private Phrase(Predicate<Context> condition, string text, Action<Context> update)
    {
        _condition = condition;
        _text = text;
        _update = update;
    }

    public static Phrase GetPhrase(Context context)
    {
        var group = context.UseGroup2 ? _phrases[1] : _phrases[context.J];
        context.UseGroup2 = false;
        return group[context.I % 5];
    }

    public void Write(IReadWrite io, Context context)
    {
        if (_condition.Invoke(context))
        {
            io.Write(_text);
        }

        _update.Invoke(context);
    }
}