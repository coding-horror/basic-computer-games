using Games.Common.IO;
using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions;

internal class StatusReport : ComputerFunction
{
    private readonly Game _game;
    private readonly Galaxy _galaxy;
    private readonly Enterprise _enterprise;

    internal StatusReport(Game game, Galaxy galaxy, Enterprise enterprise, IReadWrite io)
        : base("Status report", io)
    {
        _game = game;
        _galaxy = galaxy;
        _enterprise = enterprise;
    }

    internal override void Execute(Quadrant quadrant)
    {
        IO.WriteLine("   Status report:");
        IO.Write("Klingon".Pluralize(_galaxy.KlingonCount));
        IO.WriteLine($" left:  {_galaxy.KlingonCount}");
        IO.WriteLine($"Mission must be completed in {_game.StardatesRemaining:0.#} stardates.");

        if (_galaxy.StarbaseCount > 0)
        {
            IO.Write($"The Federation is maintaining {_galaxy.StarbaseCount} ");
            IO.Write("starbase".Pluralize(_galaxy.StarbaseCount));
            IO.WriteLine(" in the galaxy.");
        }
        else
        {
            IO.WriteLine("Your stupidity has left you on your own in");
            IO.WriteLine("  the galaxy -- you have no starbases left!");
        }

        _enterprise.Execute(Command.DAM);
    }
}
