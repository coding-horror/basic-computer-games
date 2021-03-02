using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal class ShieldControl : Subsystem
    {
        private readonly Enterprise _enterprise;
        private readonly Output _output;
        private readonly Input _input;

        public ShieldControl(Enterprise enterprise, Output output, Input input)
            : base("Shield Control", Command.SHE)
        {
            _enterprise = enterprise;
            _output = output;
            _input = input;
        }

        public double ShieldEnergy { get; private set; }

        public override CommandResult ExecuteCommand(Quadrant quadrant)
        {
            if (Condition < 0)
            {
                _output.WriteLine("Shield Control inoperable");
            }
            else
            {
                UpdateShields();
            }

            return CommandResult.Ok;
        }

        private void UpdateShields()
        {
            _output.WriteLine($"Energy available = {_enterprise.TotalEnergy}");
            var requested = _input.GetNumber($"Number of units to shields");

            if (Validate(requested))
            {
                ShieldEnergy = requested;
            }
            else
            {
                _output.WriteLine("<SHIELDS UNCHANGED>");
            }
        }

        private bool Validate(double requested)
        {
            if (requested > _enterprise.TotalEnergy)
            {
                _output.WriteLine("Shield Control reports, 'This is not the Federation Treasury.'");
                return false;
            }

            return requested >= 0 && requested != ShieldEnergy;
        }
    }
}
