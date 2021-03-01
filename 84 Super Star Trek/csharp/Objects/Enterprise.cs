using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;
using SuperStarTrek.Systems;

namespace SuperStarTrek.Objects
{
    internal class Enterprise
    {
        private readonly int _maxEnergy;
        private readonly List<Subsystem> _systems;
        private readonly Dictionary<Command, Subsystem> _commandExecutors;
        private Quadrant _quadrant;

        public Enterprise(int maxEnergy, Coordinates sector)
        {
            Sector = sector;
            TotalEnergy = _maxEnergy = maxEnergy;

            _systems = new List<Subsystem>();
            _commandExecutors = new Dictionary<Command, Subsystem>();
        }

        public Coordinates Quadrant => _quadrant.Coordinates;
        public Coordinates Sector { get; }
        public string Condition => GetCondition();
        public ShieldControl Shields => (ShieldControl)_commandExecutors[Command.SHE];
        public double Energy => TotalEnergy - Shields.Energy;
        public double TotalEnergy { get; private set; }
        public int DamagedSystemCount => _systems.Count(s => s.IsDamaged);
        public IEnumerable<Subsystem> Systems => _systems;
        public int TorpedoCount { get; }
        public bool IsDocked { get; private set; }

        public Enterprise Add(Subsystem system)
        {
            _systems.Add(system);
            _commandExecutors[system.Command] = system;

            return this;
        }

        public void Enter(Quadrant quadrant, string entryTextFormat)
        {
            _quadrant = quadrant;

            var _output = new Output();
            _output.Write(entryTextFormat, quadrant);

            if (quadrant.HasKlingons)
            {
                _output.Write(Strings.CombatArea);
                if (Shields.Energy <= 200) { _output.Write(Strings.LowShields); }
            }

            IsDocked = quadrant.EnterpriseIsNextToStarbase;

            Execute(Command.SRS);
        }

        private string GetCondition() =>
            (_quadrant.HasKlingons, Energy / _maxEnergy) switch
            {
                (true, _) => "*Red*",
                (_, < 0.1) => "Yellow",
                _ => "Green"
            };

        public bool Execute(Command command)
        {
            _commandExecutors[command].ExecuteCommand(_quadrant);
            return false;
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
    }
}
