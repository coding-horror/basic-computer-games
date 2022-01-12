using System;
using System.Collections.Generic;
using System.Linq;
using CivilWar;

var options = GameOptions.Input();
var armies = new List<Army> { new Army(Side.Confederate), options.TwoPlayers ? new Army(Side.Union) : new ComputerArmy(Side.Union) };

Battle? battle = null;
while (OneBattle(ref battle)) { }
DisplayResult();

bool OneBattle(ref Battle? previous)
{
    var (option, selected) = Battle.SelectBattle();
    var (battle, isReplay, quit) = option switch
    {
        Option.Battle => (selected!, false, false),
        Option.Replay when previous != null => (previous, true, false), // can't replay if no previous battle
        _ => (null!, false, true),
    };
    if (quit)
        return false;

    if (!isReplay)
    {
        Console.WriteLine($"This is the battle of {battle.Name}.");
        if (options.ShowDescriptions)
            ConsoleUtils.WriteWordWrap(battle.Description);
        armies.ForEach(a => a.PrepareBattle(battle.Men[(int)a.Side], battle.Casualties[(int)a.Side]));
    }

    ConsoleUtils.WriteTable(armies, new()
    {
        new("", a => a.Side),
        new("Men", a => a.Men),
        new("Money", a => a.Money, Before: "$"),
        new("Inflation", a => a.InflationDisplay, After: "%")
    });

    armies.ForEach(a => a.AllocateResources());
    armies.ForEach(a => a.DisplayMorale());

    string offensive = battle.Offensive switch
    {
        Side.Confederate => "You are on the offensive",
        Side.Union => "You are on the defensive",
        _ => "Both sides are on the offensive"
    };
    Console.WriteLine($"Confederate general---{offensive}");

    if (armies.Any(a => a.ChooseStrategy(isReplay)))
    {
        return false; // someone surrendered
    }
    armies[0].CalculateLosses(armies[1]);
    armies[1].CalculateLosses(armies[0]);

    ConsoleUtils.WriteTable(armies, new()
    {
        new("", a => a.Side),
        new("Casualties", a => a.Casualties),
        new("Desertions", a => a.Desertions),
    });
    if (options.TwoPlayers)
    {
        var oneDataCol = new[] { 1 };
        Console.WriteLine($"Compared to the actual casualties at {battle.Name}");
        ConsoleUtils.WriteTable(oneDataCol, armies.Select(a => new ConsoleUtils.TableRow<int>(
            a.Side.ToString(),
            _ => $"{(double)a.Casualties / battle.Casualties[(int)a.Side]}", After: "% of the original")
        ).ToList());
    }

    Side winner;
    switch (armies[0].AllLost, armies[1].AllLost, armies[0].MenLost - armies[1].MenLost)
    {
        case (true, true, _) or (false, false, 0):
            Console.WriteLine("Battle outcome unresolved");
            winner = Side.Both; // Draw
            break;
        case (false, true, _) or (false, false, < 0):
            Console.WriteLine($"The Confederacy wins {battle.Name}");
            winner = Side.Confederate;
            break;
        case (true, false, _) or (false, false, > 0):
            Console.WriteLine($"The Union wins {battle.Name}");
            winner = Side.Union;
            break;
    }
    if (!isReplay)
    {
        armies.ForEach(a => a.RecordResult(winner));
    }
    Console.WriteLine("---------------");
    previous = battle;
    return true;
}

void DisplayResult()
{
    armies[0].DisplayWarResult(armies[1]);

    int battles = armies[0].BattlesFought;
    if (battles > 0)
    {
        Console.WriteLine($"For the {battles} battles fought (excluding reruns)");

        ConsoleUtils.WriteTable(armies, new()
        {
            new("", a => a.Side),
            new("Historical Losses", a => a.CumulativeHistoricCasualties),
            new("Simulated Losses", a => a.CumulativeSimulatedCasualties),
            new("  % of original", a => ((double)a.CumulativeSimulatedCasualties / a.CumulativeHistoricCasualties).ToString("p2"))
        }, transpose: true);

        armies[1].DisplayStrategies();
    }
}