using BugGame.Parts;
using BugGame.Resources;
using Games.Common.IO;
using Games.Common.Randomness;
using static System.StringComparison;
namespace BugGame;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    public void Play()
    {
        _io.Write(Resource.Streams.Introduction);
        if (!_io.ReadString("Do you want instructions").Equals("no", InvariantCultureIgnoreCase))
        {
            _io.Write(Resource.Streams.Instructions);
        }

        BuildBugs();

        _io.Write(Resource.Streams.PlayAgain);
    }

    private void BuildBugs()
    {
        var yourBug = new Bug();
        var myBug = new Bug();

        while (true)
        {
            if (TryBuild(yourBug, m => m.You) || TryBuild(myBug, m => m.I))
            {
                if (yourBug.IsComplete) { _io.WriteLine(Message.Complete.You); }
                if (myBug.IsComplete) { _io.WriteLine(Message.Complete.I); }

                if (!_io.ReadString("Do you want the picture").Equals("no", InvariantCultureIgnoreCase))
                {
                    _io.WriteLine(yourBug.ToString("Your", 'A'));
                    _io.WriteLine(myBug.ToString("My", 'F'));
                }
            }

            if (yourBug.IsComplete || myBug.IsComplete) { break; }
        }
    }

    private bool TryBuild(Bug bug, Func<Message, string> messageTransform)
    {
        var roll = _random.Next(6) + 1;
        _io.WriteLine(messageTransform(Message.Rolled.ForValue(roll)));

        IPart part = roll switch
        {
            1 => new Body(),
            2 => new Neck(),
            3 => new Head(),
            4 => new Feeler(),
            5 => new Tail(),
            6 => new Leg(),
            _ => throw new Exception("Unexpected roll value")
        };
        _io.WriteLine($"{roll}={part.GetType().Name}");

        var partAdded = bug.TryAdd(part, out var message);
        _io.WriteLine(messageTransform.Invoke(message));

        return partAdded;
    }
}