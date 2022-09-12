using System.Collections;

internal class Life : IEnumerable<Generation>
{
    private readonly IReadWrite _io;

    public Life(IReadWrite io)
    {
        _io = io;
        FirstGeneration = Generation.Create(io);
    }

    public Generation FirstGeneration { get; }
    public string? Result { get; private set; }
    
    public IEnumerator<Generation> GetEnumerator()
    {
        var current = FirstGeneration;
        while (current.Result is null)
        {
            current = current.CalculateNextGeneration();
            yield return current;

            if (current.Result is null) { current.AddPieces(_io); }
        }

        Result = current.Result;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); 
}