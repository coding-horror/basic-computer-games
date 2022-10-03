namespace Poetry;

internal class Context
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private int _phraseNumber;
    private int _groupNumber;
    private bool _skipComma;
    private int _lineCount;
    private bool _useGroup2;

    public Context(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    public int PhraseNumber => Math.Max(_phraseNumber - 1, 0); 

    public int GroupNumber 
    { 
        get
        {
            var value = _useGroup2 ? 2 : _groupNumber;
            _useGroup2 = false;
            return Math.Max(value - 1, 0);
        }
    }

    public int PhraseCount { get; set; }
    public bool GroupNumberIsValid => _groupNumber < 5;

    public void WritePhrase()
    {
        Phrase.GetPhrase(this).Write(_io, this);
    }

    public void MaybeWriteComma()
    {
        if (!_skipComma && _random.NextFloat() <= 0.19F && PhraseCount != 0)
        {
            _io.Write(",");
            PhraseCount = 2;
        }
        _skipComma = false;
    }

    public void WriteSpaceOrNewLine()
    {
        if (_random.NextFloat() <= 0.65F)
        {
            _io.Write(" ");
            PhraseCount += 1;
        }
        else
        {
            _io.WriteLine();
            PhraseCount = 0;
        }
    }

    public void Update(IRandom random)
    {
        _phraseNumber = random.Next(1, 6);
        _groupNumber += 1;
        _lineCount += 1;
    }

    public void MaybeIndent()
    {
        if (PhraseCount == 0 && _groupNumber % 2 == 0)
        {
            _io.Write("     ");
        }
    }
    
    public void ResetGroup(IReadWrite io)
    {
        _groupNumber = 0;
        io.WriteLine();
    }

    public bool MaybeCompleteStanza()
    {
        if (_lineCount > 20)
        {
            _io.WriteLine();
            PhraseCount = _lineCount = 0;
            _useGroup2 = true;
            return true;
        }

        return false;
    }

    public void SkipNextComma() => _skipComma = true;
}
