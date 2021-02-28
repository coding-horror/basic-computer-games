using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal abstract class Subsystem
    {
        protected Subsystem(string name, Command command)
        {
            Name = name;
            Command = command;
            Condition = 0;
        }

        public string Name { get; }
        public double Condition { get; }
        public Command Command { get; }

        public abstract void ExecuteCommand(Quadrant quadrant);
    }
}
