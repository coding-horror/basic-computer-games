using System;
using System.Collections.Generic;
using System.Linq;
using Games.Common.IO;
using Games.Common.Randomness;
using SuperStarTrek.Commands;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;
using SuperStarTrek.Systems;

namespace SuperStarTrek.Objects;

internal class Enterprise
{
    private readonly int _maxEnergy;
    private readonly IReadWrite _io;
    private readonly List<Subsystem> _systems;
    private readonly Dictionary<Command, Subsystem> _commandExecutors;
    private readonly IRandom _random;
    private Quadrant _quadrant;

    public Enterprise(int maxEnergy, Coordinates sector, IReadWrite io, IRandom random)
    {
        SectorCoordinates = sector;
        TotalEnergy = _maxEnergy = maxEnergy;

        _systems = new List<Subsystem>();
        _commandExecutors = new Dictionary<Command, Subsystem>();
        _io = io;
        _random = random;
    }

    internal Quadrant Quadrant => _quadrant;

    internal Coordinates QuadrantCoordinates => _quadrant.Coordinates;

    internal Coordinates SectorCoordinates { get; private set; }

    internal string Condition => GetCondition();

    internal LibraryComputer Computer => (LibraryComputer)_commandExecutors[Command.COM];

    internal ShieldControl ShieldControl => (ShieldControl)_commandExecutors[Command.SHE];

    internal float Energy => TotalEnergy - ShieldControl.ShieldEnergy;

    internal float TotalEnergy { get; private set; }

    internal int DamagedSystemCount => _systems.Count(s => s.IsDamaged);

    internal IEnumerable<Subsystem> Systems => _systems;

    internal PhotonTubes PhotonTubes => (PhotonTubes)_commandExecutors[Command.TOR];

    internal bool IsDocked => _quadrant.EnterpriseIsNextToStarbase;

    internal bool IsStranded => TotalEnergy < 10 || Energy < 10 && ShieldControl.IsDamaged;

    internal Enterprise Add(Subsystem system)
    {
        _systems.Add(system);
        _commandExecutors[system.Command] = system;

        return this;
    }

    internal void StartIn(Quadrant quadrant)
    {
        _quadrant = quadrant;
        quadrant.Display(Strings.StartText);
    }

    private string GetCondition() =>
        IsDocked switch
        {
            true => "Docked",
            false when _quadrant.HasKlingons => "*Red*",
            false when Energy / _maxEnergy < 0.1f => "Yellow",
            false => "Green"
        };

    internal CommandResult Execute(Command command)
    {
        if (command == Command.XXX) { return CommandResult.GameOver; }

        return _commandExecutors[command].ExecuteCommand(_quadrant);
    }

    internal void Refuel() => TotalEnergy = _maxEnergy;

    public override string ToString() => "<*>";

    internal void UseEnergy(float amountUsed)
    {
        TotalEnergy -= amountUsed;
    }

    internal CommandResult TakeHit(Coordinates sector, int hitStrength)
    {
        _io.WriteLine($"{hitStrength} unit hit on Enterprise from sector {sector}");
        ShieldControl.AbsorbHit(hitStrength);

        if (ShieldControl.ShieldEnergy <= 0)
        {
            _io.WriteLine(Strings.Destroyed);
            return CommandResult.GameOver;
        }

        _io.WriteLine($"      <Shields down to {ShieldControl.ShieldEnergy} units>");

        if (hitStrength >= 20)
        {
            TakeDamage(hitStrength);
        }

        return CommandResult.Ok;
    }

    private void TakeDamage(float hitStrength)
    {
        var hitShieldRatio = hitStrength / ShieldControl.ShieldEnergy;
        if (_random.NextFloat() > 0.6 || hitShieldRatio <= 0.02f)
        {
            return;
        }

        var system = _systems[_random.Next1To8Inclusive() - 1];
        system.TakeDamage(hitShieldRatio + 0.5f * _random.NextFloat());
        _io.WriteLine($"Damage Control reports, '{system.Name} damaged by the hit.'");
    }

    internal void RepairSystems(float repairWorkDone)
    {
        var repairedSystems = new List<string>();

        foreach (var system in _systems.Where(s => s.IsDamaged))
        {
            if (system.Repair(repairWorkDone))
            {
                repairedSystems.Add(system.Name);
            }
        }

        if (repairedSystems.Any())
        {
            _io.WriteLine("Damage Control report:");
            foreach (var systemName in repairedSystems)
            {
                _io.WriteLine($"        {systemName} repair completed.");
            }
        }
    }

    internal void VaryConditionOfRandomSystem()
    {
        if (_random.NextFloat() > 0.2f) { return; }

        var system = _systems[_random.Next1To8Inclusive() - 1];
        _io.Write($"Damage Control report:  {system.Name} ");
        if (_random.NextFloat() >= 0.6)
        {
            system.Repair(_random.NextFloat() * 3 + 1);
            _io.WriteLine("state of repair improved");
        }
        else
        {
            system.TakeDamage(_random.NextFloat() * 5 + 1);
            _io.WriteLine("damaged");
        }
    }

    internal float Move(Course course, float warpFactor, int distance)
    {
        var (quadrant, sector) = MoveWithinQuadrant(course, distance) ?? MoveBeyondQuadrant(course, distance);

        if (quadrant != _quadrant.Coordinates)
        {
            _quadrant = new Quadrant(_quadrant.Galaxy[quadrant], this, _random, _quadrant.Galaxy, _io);
        }
        _quadrant.SetEnterpriseSector(sector);
        SectorCoordinates = sector;

        TotalEnergy -= distance + 10;
        if (Energy < 0)
        {
            _io.WriteLine("Shield Control supplies energy to complete the maneuver.");
            ShieldControl.ShieldEnergy = Math.Max(0, TotalEnergy);
        }

        return GetTimeElapsed(quadrant, warpFactor);
    }

    private (Coordinates, Coordinates)? MoveWithinQuadrant(Course course, int distance)
    {
        var currentSector = SectorCoordinates;
        foreach (var (sector, index) in course.GetSectorsFrom(SectorCoordinates).Select((s, i) => (s, i)))
        {
            if (distance == 0) { break; }

            if (_quadrant.HasObjectAt(sector))
            {
                _io.WriteLine($"Warp engines shut down at sector {currentSector} dues to bad navigation");
                distance = 0;
                break;
            }

            currentSector = sector;
            distance -= 1;
        }

        return distance == 0 ? (_quadrant.Coordinates, currentSector) : null;
    }

    private (Coordinates, Coordinates) MoveBeyondQuadrant(Course course, int distance)
    {
        var (complete, quadrant, sector) = course.GetDestination(QuadrantCoordinates, SectorCoordinates, distance);

        if (!complete)
        {
            _io.Write(Strings.PermissionDenied, sector, quadrant);
        }

        return (quadrant, sector);
    }

    private float GetTimeElapsed(Coordinates finalQuadrant, float warpFactor) =>
        finalQuadrant == _quadrant.Coordinates
            ? Math.Min(1, (float)Math.Round(warpFactor, 1, MidpointRounding.ToZero))
            : 1;
}
