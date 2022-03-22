using Games.Common.IO;
using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems;

internal class DamageControl : Subsystem
{
    private readonly Enterprise _enterprise;
    private readonly IReadWrite _io;

    internal DamageControl(Enterprise enterprise, IReadWrite io)
        : base("Damage Control", Command.DAM, io)
    {
        _enterprise = enterprise;
        _io = io;
    }

    protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
    {
        if (IsDamaged)
        {
            _io.WriteLine("Damage Control report not available");
        }
        else
        {
            _io.WriteLine();
            WriteDamageReport();
        }

        if (_enterprise.DamagedSystemCount > 0 && _enterprise.IsDocked)
        {
            if (quadrant.Starbase.TryRepair(_enterprise, out var repairTime))
            {
                WriteDamageReport();
                return CommandResult.Elapsed(repairTime);
            }
        }

        return CommandResult.Ok;
    }

    internal void WriteDamageReport()
    {
        _io.WriteLine();
        _io.WriteLine("Device             State of Repair");
        foreach (var system in _enterprise.Systems)
        {
            _io.Write(system.Name.PadRight(25));
            _io.WriteLine((int)(system.Condition * 100) * 0.01F);
        }
        _io.WriteLine();
    }
}
