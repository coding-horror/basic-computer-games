using Games.Common.IO;
using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems;

internal class ShieldControl : Subsystem
{
    private readonly Enterprise _enterprise;
    private readonly IReadWrite _io;

    internal ShieldControl(Enterprise enterprise, IReadWrite io)
        : base("Shield Control", Command.SHE, io)
    {
        _enterprise = enterprise;
        _io = io;
    }

    internal float ShieldEnergy { get; set; }

    protected override bool CanExecuteCommand() => IsOperational("{name} inoperable");

    protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
    {
        _io.WriteLine($"Energy available = {_enterprise.TotalEnergy}");
        var requested = _io.ReadNumber($"Number of units to shields");

        if (Validate(requested))
        {
            ShieldEnergy = requested;
            _io.Write(Strings.ShieldsSet, requested);
        }
        else
        {
            _io.WriteLine("<SHIELDS UNCHANGED>");
        }

        return CommandResult.Ok;
    }

    private bool Validate(float requested)
    {
        if (requested > _enterprise.TotalEnergy)
        {
            _io.WriteLine("Shield Control reports, 'This is not the Federation Treasury.'");
            return false;
        }

        return requested >= 0 && requested != ShieldEnergy;
    }

    internal void AbsorbHit(int hitStrength) => ShieldEnergy -= hitStrength;

    internal void DropShields() => ShieldEnergy = 0;
}
