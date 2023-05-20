using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Salvo;

internal class Grid
{
    private readonly List<Ship> _ships;

    internal Grid(IReadWrite io)
    {
        io.WriteLine(Prompts.Coordinates);
        _ships = new()
        {
            new Battleship(io),
            new Cruiser(io),
            new Destroyer("A", io),
            new Destroyer("B", io)
        };
    }

    internal Grid(IRandom random)
    {
        _ships = new();
        while (true)
        {
            _ships.Add(new Battleship(random));
            if (TryPositionShip(() => new Cruiser(random)) &&
                TryPositionShip(() => new Destroyer("A", random)) &&
                TryPositionShip(() => new Destroyer("B", random)))
            {
                return;
            } 
            _ships.Clear();
        }

        bool TryPositionShip(Func<Ship> shipFactory)
        {
            var shipGenerationAttempts = 0;
            while (true)
            {
                var ship = shipFactory.Invoke();
                shipGenerationAttempts++;
                if (shipGenerationAttempts > 25) { return false; }
                if (_ships.Min(ship.DistanceTo) >= 3.59)
                {
                    _ships.Add(ship);
                    return true; 
                }
            }
        }
    }

    internal IEnumerable<Ship> Ships => _ships.AsEnumerable();

    internal bool IsHit(Position position, [NotNullWhen(true)] out Ship? ship)
    {
        ship = _ships.FirstOrDefault(s => s.IsHit(position));
        if (ship == null) { return false; }

        if (ship.IsDestroyed) { _ships.Remove(ship); }

        return true;
    }
}
