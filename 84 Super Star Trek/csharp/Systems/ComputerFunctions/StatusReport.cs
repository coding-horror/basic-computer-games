using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions
{
    internal class StatusReport : ComputerFunction
    {
        private readonly Game _game;
        private readonly Galaxy _galaxy;
        private readonly Enterprise _enterprise;

        public StatusReport(Game game, Galaxy galaxy, Enterprise enterprise, Output output)
            : base("Status report", output)
        {
            _game = game;
            _galaxy = galaxy;
            _enterprise = enterprise;
        }

        internal override void Execute(Quadrant quadrant)
        {
            Output.WriteLine("   Status report:")
                .Write("Klingon".Pluralize(_galaxy.KlingonCount)).WriteLine($" left:  {_galaxy.KlingonCount}")
                .WriteLine($"Mission must be completed in {_game.StardatesRemaining:0.#} stardates.");

            if (_galaxy.StarbaseCount > 0)
            {
                Output.Write($"The Federation is maintaining {_galaxy.StarbaseCount} ")
                   .Write("starbase".Pluralize(_galaxy.StarbaseCount)).WriteLine(" in the galaxy.");
            }
            else
            {
                Output.WriteLine("Your stupidity has left you on your own in")
                    .WriteLine("  the galaxy -- you have no starbases left!");
            }

            _enterprise.Execute(Command.DAM);
        }
    }
}