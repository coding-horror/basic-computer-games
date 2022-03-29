using System.Collections.Generic;
using System.Linq;
using Games.Common.IO;
using Games.Common.Randomness;
using Hexapawn.Resources;

namespace Hexapawn;

// Runs series of games between the computer and the human player
internal class GameSeries
{
    private readonly TextIO _io;
    private readonly Computer _computer;
    private readonly Human _human;
    private readonly Dictionary<object, int> _wins;

    public GameSeries(TextIO io, IRandom random)
    {
        _io = io;
        _computer = new(io, random);
        _human = new(io);
        _wins = new() { [_computer] = 0, [_human] = 0 };
    }

    public void Play()
    {
        _io.Write(Resource.Streams.Title);

        if (_io.GetYesNo("Instructions") == 'Y')
        {
            _io.Write(Resource.Streams.Instructions);
        }

        while (true)
        {
            var game = new Game(_io);

            var winner = game.Play(_human, _computer);
            _wins[winner]++;
            _io.WriteLine(winner == _computer ? "I win." : "You win.");

            _io.Write($"I have won {_wins[_computer]} and you {_wins[_human]}");
            _io.WriteLine($" out of {_wins.Values.Sum()} games.");
            _io.WriteLine();
        }
    }
}
