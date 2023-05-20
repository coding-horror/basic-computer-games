namespace Salvo.Targetting;

internal class HumanShotSelector : ShotSelector
{
    private readonly IReadWrite _io;

    internal HumanShotSelector(Grid source, IReadWrite io) 
        : base(source)
    {
        _io = io;
    }

    protected override IEnumerable<Position> GetShots()
    {
        var shots = new Position[NumberOfShots];
        
        for (var i = 0; i < shots.Length; i++)
        {
            while (true)
            {
                var position = _io.ReadValidPosition();
                if (WasSelectedPreviously(position, out var turnTargeted)) 
                { 
                    _io.WriteLine($"YOU SHOT THERE BEFORE ON TURN {turnTargeted}");
                    continue;
                }
                shots[i] = position;
                break;
            }
        }

        return shots;
    }
}
