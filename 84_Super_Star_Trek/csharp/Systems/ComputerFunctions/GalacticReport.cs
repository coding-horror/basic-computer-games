using System.Collections.Generic;
using System.Linq;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions
{
    internal abstract class GalacticReport : ComputerFunction
    {
        internal GalacticReport(string description, Output output, Galaxy galaxy)
            : base(description, output)
        {
            Galaxy = galaxy;
        }

        protected Galaxy Galaxy { get; }

        protected abstract void WriteHeader(Quadrant quadrant);

        protected abstract IEnumerable<string> GetRowData();

        internal sealed override void Execute(Quadrant quadrant)
        {
            WriteHeader(quadrant);
            Output.WriteLine("       1     2     3     4     5     6     7     8")
                .WriteLine("     ----- ----- ----- ----- ----- ----- ----- -----");

            foreach (var (row, index) in GetRowData().Select((r, i) => (r, i)))
            {
                Output.WriteLine($" {index+1}   {row}")
                    .WriteLine("     ----- ----- ----- ----- ----- ----- ----- -----");
            }
        }
    }
}