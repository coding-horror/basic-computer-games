using Games.Common.IO;
using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems;

internal class PhotonTubes : Subsystem
{
    private readonly int _tubeCount;
    private readonly Enterprise _enterprise;
    private readonly IReadWrite _io;

    internal PhotonTubes(int tubeCount, Enterprise enterprise, IReadWrite io)
        : base("Photon Tubes", Command.TOR, io)
    {
        TorpedoCount = _tubeCount = tubeCount;
        _enterprise = enterprise;
        _io = io;
    }

    internal int TorpedoCount { get; private set; }

    protected override bool CanExecuteCommand() => HasTorpedoes() && IsOperational("{name} are not operational");

    private bool HasTorpedoes()
    {
        if (TorpedoCount > 0) { return true; }

        _io.WriteLine("All photon torpedoes expended");
        return false;
    }

    protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
    {
        if (!_io.TryReadCourse("Photon torpedo course", "Ensign Chekov", out var course))
        {
            return CommandResult.Ok;
        }

        TorpedoCount -= 1;

        var isHit = false;
        _io.WriteLine("Torpedo track:");
        foreach (var sector in course.GetSectorsFrom(_enterprise.SectorCoordinates))
        {
            _io.WriteLine($"                {sector}");

            if (quadrant.TorpedoCollisionAt(sector, out var message, out var gameOver))
            {
                _io.WriteLine(message);
                isHit = true;
                if (gameOver) { return CommandResult.GameOver; }
                break;
            }
        }

        if (!isHit) { _io.WriteLine("Torpedo missed!"); }

        return quadrant.KlingonsFireOnEnterprise();
    }

    internal void ReplenishTorpedoes() => TorpedoCount = _tubeCount;
}
