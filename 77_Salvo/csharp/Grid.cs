using System.Collections.Immutable;

namespace Salvo;

internal class Grid
{
    private readonly List<Ship> _ships;
    private readonly Dictionary<Position, int> _shots = new();

    internal Grid()
    {
        _ships = new();
    }

    internal Grid(IReadWrite io)
    {
        io.WriteLine("ENTER COORDINATES FOR...");
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

    public float this[Position position] 
    {
        get => _shots.TryGetValue(position, out var value) 
                ? value + 10
                : _ships.FirstOrDefault(s => s.Positions.Contains(position))?.Value ?? 0;
        set
        {
            _ = _ships.FirstOrDefault(s => s.IsHit(position));
            _shots[position] = (int)value - 10;
        }
    }

    internal int UntriedSquareCount => 100 - _shots.Count;
    internal IEnumerable<Ship> Ships => _ships.AsEnumerable();

    internal bool WasTargetedAt(Position position, out int turnTargeted)
        => _shots.TryGetValue(position, out turnTargeted);

    internal bool IsHit(Position position, int turnNumber, out string? shipName)
    {
        shipName = null;
        _shots[position] = turnNumber;
        
        var ship = _ships.FirstOrDefault(s => s.IsHit(position));
        if (ship == null) { return false; }

        if (ship.IsDestroyed) { _ships.Remove(ship); }

        return true;
    }
}
