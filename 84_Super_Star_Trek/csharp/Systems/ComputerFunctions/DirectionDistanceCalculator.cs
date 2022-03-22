using Games.Common.IO;
using SuperStarTrek.Objects;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions;

internal class DirectionDistanceCalculator : NavigationCalculator
{
    private readonly Enterprise _enterprise;
    private readonly IReadWrite _io;

    internal DirectionDistanceCalculator(Enterprise enterprise, IReadWrite io)
        : base("Direction/distance calculator", io)
    {
        _enterprise = enterprise;
        _io = io;
    }

    internal override void Execute(Quadrant quadrant)
    {
        IO.WriteLine("Direction/distance calculator:");
        IO.Write($"You are at quadrant {_enterprise.QuadrantCoordinates}");
        IO.WriteLine($" sector {_enterprise.SectorCoordinates}");
        IO.WriteLine("Please enter");

        WriteDirectionAndDistance(
            _io.GetCoordinates("  Initial coordinates"),
            _io.GetCoordinates("  Final coordinates"));
    }
}
