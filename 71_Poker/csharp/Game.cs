using Poker.Cards;
using Poker.Players;
using Poker.Resources;

namespace Poker;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    internal void Play()
    {
        _io.Write(Resource.Streams.Title);
        _io.Write(Resource.Streams.Instructions);

        var deck = new Deck();
        var human = new Human(200, _io);
        var computer = new Computer(200, _io, _random);
        var table = new Table(_io, _random, deck, human, computer);

        do
        {
            table.PlayHand();
        } while (table.ShouldPlayAnotherHand());
    }
}
