namespace OneCheck;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly Board _board;
    private int _moveCount;

    public Game(IReadWrite io)
    {
        _io = io;
        _board = new Board();
    }

    public void Play()
    {
        _io.Write(Streams.Introduction);
        
        do
        {
            _io.WriteLine(_board);
            _io.WriteLine();
        } while (PlayMove());

        _io.WriteLine(Formats.Results, _moveCount, _board.Count);
    }

    private bool PlayMove()
    {
        while (true)
        {
            var from = (int)_io.ReadNumber(Prompts.From);
            if (from == 0) { return false; }

            var move = new Move { From = from, To = (int)_io.ReadNumber(Prompts.To) };

            if (_board.TryMove(move)) 
            { 
                _moveCount++;
                return true; 
            }

            _io.Write(Streams.IllegalMove);
        }
    }
}
