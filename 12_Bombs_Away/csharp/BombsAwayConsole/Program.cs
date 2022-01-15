using BombsAwayConsole;
using BombsAwayGame;

/// Create and play <see cref="Game"/>s using a <see cref="ConsoleUserInterface"/>.
PlayGameWhileUserWantsTo(new ConsoleUserInterface());

void PlayGameWhileUserWantsTo(ConsoleUserInterface ui)
{
    do
    {
        new Game(ui).Play();
    }
    while (UserWantsToPlayAgain(ui));
}

bool UserWantsToPlayAgain(IUserInterface ui)
{
    bool result = ui.ChooseYesOrNo("ANOTHER MISSION (Y OR N)?");
    if (!result)
    {
        Console.WriteLine("CHICKEN !!!");
    }

    return result;
}

