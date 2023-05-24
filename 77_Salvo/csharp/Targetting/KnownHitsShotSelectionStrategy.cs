namespace Salvo.Targetting;

internal class KnownHitsShotSelectionStrategy : ShotSelectionStrategy
{
    private readonly List<(int Turn, Ship Ship)> _damagedShips = new();

    internal KnownHitsShotSelectionStrategy(ShotSelector shotSelector)
        : base(shotSelector)
    {
    }

    internal bool KnowsOfDamagedShips => _damagedShips.Any();

    internal override IEnumerable<Position> GetShots(int numberOfShots)
    {
        var tempGrid = Position.All.ToDictionary(x => x, _ => 0);
        var shots = Enumerable.Range(1, numberOfShots).Select(x => new Position(x, x)).ToArray();

        foreach (var (hitTurn, ship) in _damagedShips)
        {
            foreach (var position in Position.All)
            {
                if (WasSelectedPreviously(position))
                {  
                    tempGrid[position]=-10000000;
                    continue;
                }

                foreach (var neighbour in position.Neighbours)    
                {
                    if (WasSelectedPreviously(neighbour, out var turn) && turn == hitTurn)
                    {
                        tempGrid[position] += hitTurn + 10 - position.Y * ship.Shots;
                    }
                }
            }
        }

        foreach (var position in Position.All)
        {
            var Q9=0;
            for (var i = 0; i < numberOfShots; i++)
            {
                if (tempGrid[shots[i]] < tempGrid[shots[Q9]]) 
                { 
                    Q9 = i;
                }
            }
            if (position.X <= numberOfShots && position.IsOnDiagonal) { continue; }
            if (tempGrid[position]<tempGrid[shots[Q9]]) { continue; }
            if (!shots.Contains(position))
            {
                shots[Q9] = position;
            }
        }

        return shots;
    } 

    internal void RecordHit(Ship ship, int turn)
    {
        if (ship.IsDestroyed) 
        {
            _damagedShips.RemoveAll(x => x.Ship == ship);
        }
        else
        {
            _damagedShips.Add((turn, ship));
        }
    }
}
