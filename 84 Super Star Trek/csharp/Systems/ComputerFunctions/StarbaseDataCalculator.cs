using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions
{
    internal class StarbaseDataCalculator : NavigationCalculator
    {
        private readonly Enterprise _enterprise;

        public StarbaseDataCalculator(Enterprise enterprise, Output output)
            : base("Starbase nav data", output)
        {
            _enterprise = enterprise;
        }

        internal override void Execute(Quadrant quadrant)
        {
            if (!quadrant.HasStarbase)
            {
                Output.WriteLine(Strings.NoStarbase);
                return;
            }

            Output.WriteLine("From Enterprise to Starbase:");

            WriteDirectionAndDistance(_enterprise.Sector, quadrant.Starbase.Sector);
        }
    }
}