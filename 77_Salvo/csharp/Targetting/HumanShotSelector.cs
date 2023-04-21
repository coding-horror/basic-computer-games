namespace Salvo.Targetting;

internal class HumanShotSelector : ShotSelector
{
    public HumanShotSelector(Grid source, Grid target) 
        : base(source, target)
    {
    }

    public IEnumerable<Position> GetShots(IReadWrite io)
    {
        var shots = new Position[GetShotCount()];
        
        for (var i = 0; i < shots.Length; i++)
        {
            while (true)
            {
                var position = io.ReadValidPosition();
                if (Target.WasTargetedAt(position, out var turnTargeted)) 
                { 
                    io.WriteLine($"YOU SHOT THERE BEFORE ON TURN {turnTargeted}");
                    continue;
                }
                shots[i] = position;
                break;
            }
        }

        return shots;
    }
}
