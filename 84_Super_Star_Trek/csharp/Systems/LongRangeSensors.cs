using System.Linq;
using Games.Common.IO;
using SuperStarTrek.Commands;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems;

internal class LongRangeSensors : Subsystem
{
    private readonly Galaxy _galaxy;
    private readonly IReadWrite _io;

    internal LongRangeSensors(Galaxy galaxy, IReadWrite io)
        : base("Long Range Sensors", Command.LRS, io)
    {
        _galaxy = galaxy;
        _io = io;
    }

    protected override bool CanExecuteCommand() => IsOperational("{name} are inoperable");

    protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
    {
        _io.WriteLine($"Long range scan for quadrant {quadrant.Coordinates}");
        _io.WriteLine("-------------------");
        foreach (var quadrants in _galaxy.GetNeighborhood(quadrant))
        {
            _io.WriteLine(": " + string.Join(" : ", quadrants.Select(q => q?.Scan() ?? "***")) + " :");
            _io.WriteLine("-------------------");
        }

        return CommandResult.Ok;
    }
}

