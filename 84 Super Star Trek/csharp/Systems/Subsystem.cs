using SuperStarTrek.Commands;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal abstract class Subsystem
    {
        private readonly Output _output;

        protected Subsystem(string name, Command command, Output output)
        {
            Name = name;
            Command = command;
            Condition = 0;
            _output = output;
        }

        internal string Name { get; }
        internal float Condition { get; private set; }
        internal bool IsDamaged => Condition < 0;
        internal Command Command { get; }

        protected virtual bool CanExecuteCommand() => true;

        protected bool IsOperational(string notOperationalMessage)
        {
            if (IsDamaged)
            {
                _output.WriteLine(notOperationalMessage.Replace("{name}", Name));
                return false;
            }

            return true;
        }

        internal CommandResult ExecuteCommand(Quadrant quadrant)
            => CanExecuteCommand() ? ExecuteCommandCore(quadrant) : CommandResult.Ok;

        protected abstract CommandResult ExecuteCommandCore(Quadrant quadrant);

        internal virtual void Repair()
        {
            if (IsDamaged)
            {
                Condition = 0;
            }
        }

        internal virtual bool Repair(float repairWorkDone)
        {
            if (IsDamaged)
            {
                Condition += repairWorkDone;
                if (Condition > -0.1f && Condition < 0)
                {
                    Condition = -0.1f;
                }
            }

            return !IsDamaged;
        }

        internal void TakeDamage(float damage) => Condition -= damage;
    }
}
