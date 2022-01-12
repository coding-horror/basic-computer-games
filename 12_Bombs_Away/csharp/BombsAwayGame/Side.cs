namespace BombsAwayGame;

/// <summary>
/// Represents a protagonist in the game.
/// </summary>
internal abstract class Side
{
    /// <summary>
    /// Create instance using the given UI.
    /// </summary>
    /// <param name="ui">UI to use.</param>
    public Side(IUserInterface ui)
    {
        UI = ui;
    }

    /// <summary>
    /// Play this side.
    /// </summary>
    public abstract void Play();

    /// <summary>
    /// User interface supplied to ctor.
    /// </summary>
    protected IUserInterface UI { get; }

    /// <summary>
    /// Random-number generator for this play-through.
    /// </summary>
    private readonly Random _random = new();

    /// <summary>
    /// Gets a random floating-point number greater than or equal to zero, and less than one.
    /// </summary>
    /// <returns>Random floating-point number greater than or equal to zero, and less than one.</returns>
    protected double RandomFrac() => _random.NextDouble();

    /// <summary>
    /// Gets a random integer in a range.
    /// </summary>
    /// <param name="minValue">The inclusive lower bound of the number returned.</param>
    /// <param name="maxValue">The exclusive upper bound of the number returned.</param>
    /// <returns>Random integer in a range.</returns>
    protected int RandomInteger(int minValue, int maxValue) => _random.Next(minValue: minValue, maxValue: maxValue);

    /// <summary>
    /// Display messages indicating the mission succeeded.
    /// </summary>
    protected void MissionSucceeded()
    {
        UI.Output("DIRECT HIT!!!! " + RandomInteger(0, 100) + " KILLED.");
        UI.Output("MISSION SUCCESSFUL.");
    }

    /// <summary>
    /// Gets the Guns type of enemy artillery.
    /// </summary>
    protected EnemyArtillery Guns { get; } = new("GUNS", 0);

    /// <summary>
    /// Gets the Missiles type of enemy artillery.
    /// </summary>
    protected EnemyArtillery Missiles { get; } = new("MISSILES", 35);

    /// <summary>
    /// Gets the Both Guns and Missiles type of enemy artillery.
    /// </summary>
    protected EnemyArtillery Both { get; } = new("BOTH", 35);

    /// <summary>
    /// Perform enemy counterattack using the given artillery and hit rate percent.
    /// </summary>
    /// <param name="artillery">Enemy artillery to use.</param>
    /// <param name="hitRatePercent">Hit rate percent for enemy.</param>
    protected void EnemyCounterattack(EnemyArtillery artillery, int hitRatePercent)
    {
        if (hitRatePercent + artillery.Accuracy > RandomInteger(0, 100))
        {
            MissionFailed();
        }
        else
        {
            UI.Output("YOU MADE IT THROUGH TREMENDOUS FLAK!!");
        }
    }

    /// <summary>
    /// Display messages indicating the mission failed.
    /// </summary>
    protected void MissionFailed()
    {
        UI.Output("* * * * BOOM * * * *");
        UI.Output("YOU HAVE BEEN SHOT DOWN.....");
        UI.Output("DEARLY BELOVED, WE ARE GATHERED HERE TODAY TO PAY OUR");
        UI.Output("LAST TRIBUTE...");
    }
}
