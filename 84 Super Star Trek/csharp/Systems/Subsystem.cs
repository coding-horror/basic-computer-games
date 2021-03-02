using SuperStarTrek.Commands;
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
        public double Condition { get; private set; }
        public bool IsDamaged => Condition < 0;
        public Command Command { get; }

        public abstract CommandResult ExecuteCommand(Quadrant quadrant);
        public void Repair()
        {
            if (Condition < 0) { Condition = 0; }
        }
    }
}
