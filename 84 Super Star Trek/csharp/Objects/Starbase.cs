using SuperStarTrek.Resources;

namespace SuperStarTrek.Objects
{
    internal class Starbase
    {
        private readonly Input _input;
        private readonly Output _output;
        private readonly double _repairDelay;

        public Starbase(Random random, Input input)
        {
            _repairDelay = random.GetDouble() * 0.5;
            _input = input;
        }

        public override string ToString() => ">!<";

        internal bool TryRepair(Enterprise enterprise, out double repairTime)
        {
            repairTime = enterprise.DamagedSystemCount * 0.1 + _repairDelay;
            if (repairTime >= 1) { repairTime = 0.9; }

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
    }
}
