using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperStarTrek.Commands;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;
using SuperStarTrek.Systems;

namespace SuperStarTrek.Objects
{
    internal class Enterprise
    {
        private readonly int _maxEnergy;
        private readonly Output _output;
        private readonly List<Subsystem> _systems;
        private readonly Dictionary<Command, Subsystem> _commandExecutors;
        private readonly Random _random;
        private readonly Input _input;
        private Quadrant _quadrant;

        public Enterprise(int maxEnergy, Coordinates sector, Output output, Random random, Input input)
        {
            SectorCoordinates = sector;
            TotalEnergy = _maxEnergy = maxEnergy;

            _systems = new List<Subsystem>();
            _commandExecutors = new Dictionary<Command, Subsystem>();
            _output = output;
            _random = random;
            _input = input;
        }

        public Quadrant Quadrant => _quadrant;
        public Coordinates QuadrantCoordinates => _quadrant.Coordinates;
        public Coordinates SectorCoordinates { get; private set; }

        public string Condition => GetCondition();
        public LibraryComputer Computer => (LibraryComputer)_commandExecutors[Command.COM];
        public ShieldControl ShieldControl => (ShieldControl)_commandExecutors[Command.SHE];
        public float Energy => TotalEnergy - ShieldControl.ShieldEnergy;
        public float TotalEnergy { get; private set; }
        public int DamagedSystemCount => _systems.Count(s => s.IsDamaged);
        public IEnumerable<Subsystem> Systems => _systems;
        public int TorpedoCount { get; }
        public bool IsDocked => _quadrant.EnterpriseIsNextToStarbase;
        public bool IsStranded => TotalEnergy < 10 || Energy < 10 && ShieldControl.IsDamaged;

        public Enterprise Add(Subsystem system)
        {
            _systems.Add(system);
            _commandExecutors[system.Command] = system;

            return this;
        }

        public void StartIn(Quadrant quadrant)
        {
            _quadrant = quadrant;
            quadrant.Display(Strings.StartText);
        }

        private string GetCondition() =>
            (_quadrant.HasKlingons, Energy / _maxEnergy) switch
            {
                (true, _) => "*Red*",
                (_, < 0.1f) => "Yellow",
                _ => "Green"
            };

        public CommandResult Execute(Command command)
        {
            if (command == Command.XXX) { return CommandResult.GameOver; }

            return _commandExecutors[command].ExecuteCommand(_quadrant);
        }

        internal bool Recognises(string command)
        {
            throw new NotImplementedException();
        }

        internal string GetCommandList()
        {
            throw new NotImplementedException();
        }

        public override string ToString() => "<*>";

        internal void UseEnergy(float amountUsed)
        {
            TotalEnergy -= amountUsed;
        }

        internal CommandResult TakeHit(Coordinates sector, int hitStrength)
        {
            _output.WriteLine($"{hitStrength} unit hit on Enterprise from sector {sector}");
            ShieldControl.AbsorbHit(hitStrength);

            if (ShieldControl.ShieldEnergy <= 0)
            {
                _output.WriteLine(Strings.Destroyed);
                return CommandResult.GameOver;
            }

            _output.WriteLine($"      <Shields down to {ShieldControl.ShieldEnergy} units>");

            if (hitStrength >= 20)
            {
                TakeDamage(hitStrength);
            }

            return CommandResult.Ok;
        }

        private void TakeDamage(float hitStrength)
        {
            var hitShieldRatio = hitStrength / ShieldControl.ShieldEnergy;
            if (_random.GetFloat() > 0.6 || hitShieldRatio <= 0.02f)
            {
                return;
            }

            var system = _systems[_random.Get1To8Inclusive() - 1];
            system.TakeDamage(hitShieldRatio + 0.5f * _random.GetFloat());
            _output.WriteLine($"Damage Control reports, '{system.Name} damaged by the hit.'");
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
                _output.WriteLine("Damage Control report:");
                foreach (var systemName in repairedSystems)
                {
                    _output.WriteLine($"        {systemName} repair completed.");
                }
            }
        }

        internal void VaryConditionOfRandomSystem()
        {
            if (_random.GetFloat() > 0.2f) { return; }

            var system = _systems[_random.Get1To8Inclusive() - 1];
            _output.Write($"Damage Control report:  {system.Name} ");
            if (_random.GetFloat() >= 0.6)
            {
                system.Repair(_random.GetFloat() * 3 + 1);
                _output.WriteLine("state of repair improved");
            }
            else
            {
                system.TakeDamage(_random.GetFloat() * 5 + 1);
                _output.WriteLine("damaged");
            }
        }

        internal float Move(Course course, float warpFactor, int distance)
        {
            var (quadrant, sector) = MoveWithinQuadrant(course, distance) ?? MoveBeyondQuadrant(course, distance);

            if (quadrant != _quadrant.Coordinates)
            {
                _quadrant = new Quadrant(_quadrant.Galaxy[quadrant], this, _random, _quadrant.Galaxy, _input, _output);
            }
            SectorCoordinates = sector;

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
                    _output.WriteLine($"Warp engines shut down at sector {currentSector} dues to bad navigation");
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
                _output.Write(Strings.PermissionDenied, sector, quadrant);
            }

            return (quadrant, sector);
        }

        private float GetTimeElapsed(Coordinates finalQuadrant, float warpFactor) =>
            finalQuadrant == _quadrant.Coordinates
                ? Math.Min(1, (float)Math.Round(warpFactor, 1, MidpointRounding.ToZero))
                : 1;
    }
}
