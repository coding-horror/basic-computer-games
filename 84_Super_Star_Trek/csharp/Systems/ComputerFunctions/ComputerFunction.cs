using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions
{
    internal abstract class ComputerFunction
    {
        protected ComputerFunction(string description, Output output)
        {
            Description = description;
            Output = output;
        }

        internal string Description { get; }

        protected Output Output { get; }

        internal abstract void Execute(Quadrant quadrant);
    }
}
