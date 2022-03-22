using Games.Common.IO;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions;

internal class StarbaseDataCalculator : NavigationCalculator
{
    private readonly Enterprise _enterprise;

    internal StarbaseDataCalculator(Enterprise enterprise, IReadWrite io)
        : base("Starbase nav data", io)
    {
        _enterprise = enterprise;
    }

    internal override void Execute(Quadrant quadrant)
    {
        if (!quadrant.HasStarbase)
        {
            IO.WriteLine(Strings.NoStarbase);
            return;
        }

        IO.WriteLine("From Enterprise to Starbase:");

        WriteDirectionAndDistance(_enterprise.SectorCoordinates, quadrant.Starbase.Sector);
    }
}
