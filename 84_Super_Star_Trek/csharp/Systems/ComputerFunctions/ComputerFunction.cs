using Games.Common.IO;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions;

internal abstract class ComputerFunction
{
    protected ComputerFunction(string description, IReadWrite io)
    {
        Description = description;
        IO = io;
    }

    internal string Description { get; }

    protected IReadWrite IO { get; }

    internal abstract void Execute(Quadrant quadrant);
}
