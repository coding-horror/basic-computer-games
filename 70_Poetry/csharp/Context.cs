namespace Poetry;

internal class Context
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Context(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    public int I { get; set; }
    public int J { get; set; }
    public int K { get; set; }
    public int U { get; set; }
    public bool SkipComma { get; set; }
    public bool UseGroup2 { get; set; }
    public bool ShouldIndent => U == 0 && J % 2 == 0;
    public bool GroupNumberIsValid => J < 5;

    public void WritePhrase()
    {
        Phrase.GetPhrase(this).Write(_io, this);
    }

    public void MaybeWriteComma()
    {
        if (!SkipComma && _random.NextFloat() <= 0.19F && U != 0)
        {
            _io.Write(",");
            U = 2;
        }
        SkipComma = false;
    }

    public void WriteSpaceOrNewLine()
    {
        if (_random.NextFloat() <= 0.65F)
        {
            _io.Write(" ");
            U += 1;
        }
        else
        {
            _io.WriteLine();
            U = 0;
        }
    }

    public void Update(IRandom random)
    {
        I = random.Next(1, 6);
        J += 1;
        K += 1;
    }

    public void MaybeIndent()
    {
        if (U == 0 && J % 2 == 0)
        {
            _io.Write("     ");
        }
    }
    
    public void ResetGroup(IReadWrite io)
    {
        J = 0;
        io.WriteLine();
    }

    public bool MaybeCompleteStanza()
    {
        if (K > 20)
        {
            _io.WriteLine();
            U = K = 0;
            UseGroup2 = true;
            return true;
        }

        return false;
    }

    public void SkipNextComma() => SkipComma = true;
}
