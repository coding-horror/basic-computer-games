using System.Linq;
using Games.Common.IO;
using Games.Common.Randomness;
using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems;

internal class PhaserControl : Subsystem
{
    private readonly Enterprise _enterprise;
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    internal PhaserControl(Enterprise enterprise, IReadWrite io, IRandom random)
        : base("Phaser Control", Command.PHA, io)
    {
        _enterprise = enterprise;
        _io = io;
        _random = random;
    }

    protected override bool CanExecuteCommand() => IsOperational("Phasers inoperative");

    protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
    {
        if (!quadrant.HasKlingons)
        {
            _io.WriteLine(Strings.NoEnemyShips);
            return CommandResult.Ok;
        }

        if (_enterprise.Computer.IsDamaged)
        {
            _io.WriteLine("Computer failure hampers accuracy");
        }

        _io.Write($"Phasers locked on target;  ");

        var phaserStrength = GetPhaserStrength();
        if (phaserStrength < 0) { return CommandResult.Ok; }

        _enterprise.UseEnergy(phaserStrength);

        var perEnemyStrength = GetPerTargetPhaserStrength(phaserStrength, quadrant.KlingonCount);

        foreach (var klingon in quadrant.Klingons.ToList())
        {
            ResolveHitOn(klingon, perEnemyStrength, quadrant);
        }

        return quadrant.KlingonsFireOnEnterprise();
    }

    private float GetPhaserStrength()
    {
        while (true)
        {
            _io.WriteLine($"Energy available = {_enterprise.Energy} units");
            var phaserStrength = _io.ReadNumber("Number of units to fire");

            if (phaserStrength <= _enterprise.Energy) { return phaserStrength; }
        }
    }

    private float GetPerTargetPhaserStrength(float phaserStrength, int targetCount)
    {
        if (_enterprise.Computer.IsDamaged)
        {
            phaserStrength *= _random.NextFloat();
        }

        return phaserStrength / targetCount;
    }

    private void ResolveHitOn(Klingon klingon, float perEnemyStrength, Quadrant quadrant)
    {
        var distance = _enterprise.SectorCoordinates.GetDistanceTo(klingon.Sector);
        var hitStrength = (int)(perEnemyStrength / distance * (2 + _random.NextFloat()));

        if (klingon.TakeHit(hitStrength))
        {
            _io.WriteLine($"{hitStrength} unit hit on Klingon at sector {klingon.Sector}");
            _io.WriteLine(
                klingon.Energy <= 0
                    ? quadrant.Remove(klingon)
                    : $"   (sensors show {klingon.Energy} units remaining)");
        }
        else
        {
            _io.WriteLine($"Sensors show no damage to enemy at {klingon.Sector}");
        }
    }
}
