using System;
using System.Collections.Generic;
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
        private ShieldControl _shieldControl;

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
        public double Shields => _shieldControl.Energy;
        public double Energy => TotalEnergy - Shields;
        public double TotalEnergy { get; private set; }
        public int TorpedoCount { get; }

        public bool IsDocked { get; private set; }

        public Enterprise Add(Subsystem system)
        {
            _systems.Add(system);
            _commandExecutors[system.Command] = system;

            if (system is ShieldControl shieldControl) { _shieldControl = shieldControl; }

            return this;
        }

        public string GetDamageReport()
        {
            var report = new StringBuilder();
            report.AppendLine().AppendLine().AppendLine("Device             State of Repair");
            foreach (var system in _systems)
            {
                report.Append(system.Name.PadRight(25)).AppendLine(system.Condition.ToString(" 0.00;-0.00"));
            }
            report.AppendLine();
            return report.ToString();
        }

        public void Enter(Quadrant quadrant, string entryTextFormat)
        {
            _quadrant = quadrant;

            var _output = new Output();
            _output.Write(entryTextFormat, quadrant);

            if (quadrant.HasKlingons)
            {
                _output.Write(Strings.CombatArea);
                if (Shields <= 200) { _output.Write(Strings.LowShields); }
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
