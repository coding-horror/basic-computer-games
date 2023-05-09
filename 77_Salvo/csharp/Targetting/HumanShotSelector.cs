namespace Salvo.Targetting;

internal class HumanShotSelector : ShotSelector
{
    private readonly IReadWrite _io;

    internal HumanShotSelector(Grid source, Grid target, IReadWrite io) 
        : base(source, target)
    {
        _io = io;
    }

    internal override IEnumerable<Position> GetShots()
    {
        var shots = new Position[NumberOfShots];
        
        for (var i = 0; i < shots.Length; i++)
        {
            while (true)
            {
                var position = _io.ReadValidPosition();
                if (Target.WasTargetedAt(position, out var turnTargeted)) 
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
