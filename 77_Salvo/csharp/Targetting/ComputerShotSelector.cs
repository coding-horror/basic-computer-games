namespace Salvo.Targetting;

internal class ComputerShotSelector : ShotSelector
{
    private readonly bool _displayShots;

    internal ComputerShotSelector(Grid source, Grid target, bool displayShots) 
        : base(source, target)
    {
        _displayShots = displayShots;
    }

    private void DisplayShots(IEnumerable<Position> shots, IReadWrite io)
    {
        if (_displayShots)
        {
            foreach (var shot in shots)
            {
                io.WriteLine(shot);
            }
        }
    }
}
