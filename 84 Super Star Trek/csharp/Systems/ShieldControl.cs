using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal class ShieldControl : Subsystem
    {
        private readonly Enterprise _enterprise;
        private readonly Output _output;
        private readonly Input _input;

        internal ShieldControl(Enterprise enterprise, Output output, Input input)
            : base("Shield Control", Command.SHE, output)
        {
            _enterprise = enterprise;
            _output = output;
            _input = input;
        }

        internal float ShieldEnergy { get; set; }

        protected override bool CanExecuteCommand() => IsOperational("{name} inoperable");

        protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
        {
            _output.WriteLine($"Energy available = {_enterprise.TotalEnergy}");
            var requested = _input.GetNumber($"Number of units to shields");

            if (Validate(requested))
            {
                ShieldEnergy = requested;
                _output.Write(Strings.ShieldsSet, requested);
            }
            else
            {
                _output.WriteLine("<SHIELDS UNCHANGED>");
            }

            return CommandResult.Ok;
        }

        private bool Validate(float requested)
        {
            if (requested > _enterprise.TotalEnergy)
            {
                _output.WriteLine("Shield Control reports, 'This is not the Federation Treasury.'");
                return false;
            }

            return requested >= 0 && requested != ShieldEnergy;
        }

        internal void AbsorbHit(int hitStrength) => ShieldEnergy -= hitStrength;

        internal void DropShields() => ShieldEnergy = 0;
    }
}
