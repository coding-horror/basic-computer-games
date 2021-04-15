using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Objects
{
    internal class Starbase
    {
        private readonly Input _input;
        private readonly Output _output;
        private readonly float _repairDelay;

        internal Starbase(Coordinates sector, Random random, Input input, Output output)
        {
            Sector = sector;
            _repairDelay = random.GetFloat() * 0.5f;
            _input = input;
            _output = output;
        }

        internal Coordinates Sector { get; }
        public override string ToString() => ">!<";

        internal bool TryRepair(Enterprise enterprise, out float repairTime)
        {
            repairTime = enterprise.DamagedSystemCount * 0.1f + _repairDelay;
            if (repairTime >= 1) { repairTime = 0.9f; }

            _output.Write(Strings.RepairEstimate, repairTime);
            if (_input.GetYesNo(Strings.RepairPrompt, Input.YesNoMode.TrueOnY))
            {
                foreach (var system in enterprise.Systems)
                {
                    system.Repair();
                }
                return true;
            }

            repairTime = 0;
            return false;
        }

        internal void ProtectEnterprise() => _output.WriteLine(Strings.Protected);
    }
}
