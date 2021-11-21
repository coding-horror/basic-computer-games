using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal class DamageControl : Subsystem
    {
        private readonly Enterprise _enterprise;
        private readonly Output _output;

        internal DamageControl(Enterprise enterprise, Output output)
            : base("Damage Control", Command.DAM, output)
        {
            _enterprise = enterprise;
            _output = output;
        }

        protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
        {
            if (IsDamaged)
            {
                _output.WriteLine("Damage Control report not available");
            }
            else
            {
                _output.NextLine();
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
            _output.NextLine().WriteLine("Device             State of Repair");
            foreach (var system in _enterprise.Systems)
            {
                _output.Write(system.Name.PadRight(25))
                    .WriteLine(((int)(system.Condition * 100) * 0.01).ToString(" 0.##;-0.##"));
            }
            _output.NextLine();
        }
    }
}
