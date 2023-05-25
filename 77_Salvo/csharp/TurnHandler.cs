using Salvo.Targetting;

namespace Salvo;

internal class TurnHandler
{
    private readonly IReadWrite _io;
    private readonly Fleet _humanFleet;
    private readonly Fleet _computerFleet;
    private readonly bool _humanStarts;
    private readonly HumanShotSelector _humanShotSelector;
    private readonly ComputerShotSelector _computerShotSelector;
    private readonly Func<Winner?> _turnAction;
    private int _turnNumber;

    public TurnHandler(IReadWrite io, IRandom random)
    {
        _io = io;
        _computerFleet = new Fleet(random);
        _humanFleet = new Fleet(io);
        _turnAction = AskWhoStarts()
            ? () => PlayHumanTurn() ?? PlayComputerTurn()
            : () => PlayComputerTurn() ?? PlayHumanTurn();
        _humanShotSelector = new HumanShotSelector(_humanFleet, io);
        _computerShotSelector = new ComputerShotSelector(_computerFleet, random, io);
    }

    public Winner? PlayTurn()
    {
        _io.Write(Strings.Turn(++_turnNumber));
        return _turnAction.Invoke();
    }

    private bool AskWhoStarts()
    {
        while (true)
        {
            var startResponse = _io.ReadString(Prompts.Start);
            if (startResponse.Equals(Strings.WhereAreYourShips, StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (var ship in _computerFleet.Ships)
                {
                    _io.WriteLine(ship);
                }
            }
            else
            {
                return startResponse.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }

    private Winner? PlayComputerTurn()
    {
        var numberOfShots = _computerShotSelector.NumberOfShots;
        _io.Write(Strings.IHaveShots(numberOfShots));
        if (numberOfShots == 0) { return Winner.Human; }
        if (_computerShotSelector.CanTargetAllRemainingSquares)
        {
            _io.Write(Streams.IHaveMoreShotsThanSquares);
            return Winner.Computer;
        }

        _humanFleet.ReceiveShots(
            _computerShotSelector.GetShots(_turnNumber),
            ship =>
            { 
                _io.Write(Strings.IHit(ship.Name));
                _computerShotSelector.RecordHit(ship, _turnNumber);
            });

        return null;
    }

    private Winner? PlayHumanTurn()
    {
        var numberOfShots = _humanShotSelector.NumberOfShots;
        _io.Write(Strings.YouHaveShots(numberOfShots));
        if (numberOfShots == 0) { return Winner.Computer; }
        if (_humanShotSelector.CanTargetAllRemainingSquares) 
        { 
            _io.WriteLine(Streams.YouHaveMoreShotsThanSquares);
            return Winner.Human;
        }
        
        _computerFleet.ReceiveShots(
            _humanShotSelector.GetShots(_turnNumber), 
            ship => _io.Write(Strings.YouHit(ship.Name)));
        
        return null;
    }
}
