using SuperStarTrek.Commands;
using SuperStarTrek.Space;
using SuperStarTrek.Systems.ComputerFunctions;

namespace SuperStarTrek.Systems
{
    internal class LibraryComputer : Subsystem
    {
        private readonly Output _output;
        private readonly Input _input;
        private readonly ComputerFunction[] _functions;

        internal LibraryComputer(Output output, Input input, params ComputerFunction[] functions)
            : base("Library-Computer", Command.COM, output)
        {
            _output = output;
            _input = input;
            _functions = functions;
        }

        protected override bool CanExecuteCommand() => IsOperational("Computer disabled");

        protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
        {
            var index = GetFunctionIndex();
            _output.NextLine();

            _functions[index].Execute(quadrant);

            return CommandResult.Ok;
        }

        private int GetFunctionIndex()
        {
            while (true)
            {
                var index = (int)_input.GetNumber("Computer active and waiting command");
                if (index >= 0 && index <= 5) { return index; }

                for (int i = 0; i < _functions.Length; i++)
                {
                    _output.WriteLine($"   {i} = {_functions[i].Description}");
                }
            }
        }
    }
}
