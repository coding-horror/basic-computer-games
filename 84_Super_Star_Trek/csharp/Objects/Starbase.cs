using Games.Common.IO;
using Games.Common.Randomness;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Objects;

internal class Starbase
{
    private readonly IReadWrite _io;
    private readonly float _repairDelay;

    internal Starbase(Coordinates sector, IRandom random, IReadWrite io)
    {
        Sector = sector;
        _repairDelay = random.NextFloat(0.5f);
        _io = io;
    }

    internal Coordinates Sector { get; }

    public override string ToString() => ">!<";

    internal bool TryRepair(Enterprise enterprise, out float repairTime)
    {
        repairTime = enterprise.DamagedSystemCount * 0.1f + _repairDelay;
        if (repairTime >= 1) { repairTime = 0.9f; }

        _io.Write(Strings.RepairEstimate, repairTime);
        if (_io.GetYesNo(Strings.RepairPrompt, IReadWriteExtensions.YesNoMode.TrueOnY))
        {
            foreach (var system in enterprise.Systems)
            {
                system.Repair();
            }
            return true;
        }

        repairTime = 0;
        return false;
    }

    internal void ProtectEnterprise() => _io.WriteLine(Strings.Protected);
}
