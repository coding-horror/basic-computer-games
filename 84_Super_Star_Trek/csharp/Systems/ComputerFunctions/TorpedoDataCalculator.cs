using Games.Common.IO;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions;

internal class TorpedoDataCalculator : NavigationCalculator
{
    private readonly Enterprise _enterprise;

    internal TorpedoDataCalculator(Enterprise enterprise, IReadWrite io)
        : base("Photon torpedo data", io)
    {
        _enterprise = enterprise;
    }

    internal override void Execute(Quadrant quadrant)
    {
        if (!quadrant.HasKlingons)
        {
            IO.WriteLine(Strings.NoEnemyShips);
            return;
        }

        IO.WriteLine("From Enterprise to Klingon battle cruiser".Pluralize(quadrant.KlingonCount));

        foreach (var klingon in quadrant.Klingons)
        {
            WriteDirectionAndDistance(_enterprise.SectorCoordinates, klingon.Sector);
        }
    }
}
