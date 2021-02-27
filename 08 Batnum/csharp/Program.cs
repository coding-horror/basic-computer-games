using Batnum;
using Batnum.Properties;
using System;

Console.WriteLine(ConsoleUtilities.CenterText(Resources.GAME_NAME));
Console.WriteLine(ConsoleUtilities.CenterText(Resources.INTRO_HEADER));
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
ConsoleUtilities.WriteLineWordWrap(Resources.INTRO_PART1);
Console.WriteLine();
ConsoleUtilities.WriteLineWordWrap(Resources.INTRO_PART2);

while (true)
{
    Console.WriteLine();
    int pileSize = ConsoleUtilities.AskNumberQuestion(Resources.START_QUESTION_PILESIZE, (n) => n > 1);
    WinOptions winOption = (WinOptions)ConsoleUtilities.AskNumberQuestion(Resources.START_QUESTION_WINOPTION, (n) => Enum.IsDefined(typeof(WinOptions), n));
    (int minTake, int maxTake) = ConsoleUtilities.AskNumberRangeQuestion(Resources.START_QUESTION_DRAWMINMAX, (min,max) => min >= 1 && max < pileSize && max > min);
    Players currentPlayer = (Players)ConsoleUtilities.AskNumberQuestion(Resources.START_QUESTION_WHOSTARTS, (n) => Enum.IsDefined(typeof(Players), n));

    BatnumGame game = new BatnumGame(pileSize, winOption, minTake, maxTake, currentPlayer, (question) => ConsoleUtilities.AskNumberQuestion(question, (c) => true));
    while(game.IsRunning)
    {
        string message = game.TakeTurn();
        Console.WriteLine(message);
    }

}

