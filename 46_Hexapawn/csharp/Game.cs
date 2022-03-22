using System;
using Games.Common.IO;

namespace Hexapawn;

// A single game of Hexapawn
internal class Game
{
    private readonly TextIO _io;
    private readonly Board _board;

    public Game(TextIO io)
    {
        _board = new Board();
        _io = io;
    }

    public object Play(Human human, Computer computer)
    {
        _io.WriteLine(_board);
        while(true)
        {
            human.Move(_board);
            _io.WriteLine(_board);
            if (!computer.TryMove(_board))
            {
                return human;
            }
            _io.WriteLine(_board);
            if (computer.IsFullyAdvanced(_board) || human.HasNoPawns(_board))
            {
                return computer;
            }
            if (!human.HasLegalMove(_board))
            {
                _io.Write("You can't move, so ");
                return computer;
            }
        }
    }
}
