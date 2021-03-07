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
        private Quadrant _quadrant;

        public Enterprise(int maxEnergy, Coordinates sector, Output output, Random random)
        {
            Sector = sector;
            TotalEnergy = _maxEnergy = maxEnergy;

            _systems = new List<Subsystem>();
            _commandExecutors = new Dictionary<Command, Subsystem>();
            _output = output;
            _random = random;
        }

        public Coordinates Quadrant => _quadrant.Coordinates;
        public Coordinates Sector { get; }
        public string Condition => GetCondition();
        public ShieldControl ShieldControl => (ShieldControl)_commandExecutors[Command.SHE];
        public double Energy => TotalEnergy - ShieldControl.ShieldEnergy;
        public double TotalEnergy { get; private set; }
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

        public void Enter(Quadrant quadrant, string entryTextFormat)
        {
            _quadrant = quadrant;

            _output.Write(entryTextFormat, quadrant);

            if (quadrant.HasKlingons)
            {
                _output.Write(Strings.CombatArea);
                if (ShieldControl.ShieldEnergy <= 200) { _output.Write(Strings.LowShields); }
            }

            Execute(Command.SRS);
        }

        private string GetCondition() =>
            (_quadrant.HasKlingons, Energy / _maxEnergy) switch
            {
                (true, _) => "*Red*",
                (_, < 0.1) => "Yellow",
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

        private void TakeDamage(double hitStrength)
        {
            var hitShieldRatio = hitStrength / ShieldControl.ShieldEnergy;
            if (_random.GetDouble() > 0.6 || hitShieldRatio <= 0.02)
            {
                return;
            }

            _systems[_random.Get1To8Inclusive() - 1].TakeDamage(hitShieldRatio + 0.5 * _random.GetDouble());
        }
    }
}
