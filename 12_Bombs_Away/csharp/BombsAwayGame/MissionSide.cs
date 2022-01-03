namespace BombsAwayGame;

/// <summary>
/// Represents a protagonist that chooses a standard (non-kamikaze) mission.
/// </summary>
internal abstract class MissionSide : Side
{
    /// <summary>
    /// Create instance using the given UI.
    /// </summary>
    /// <param name="ui">UI to use.</param>
    public MissionSide(IUserInterface ui)
        : base(ui)
    {
    }

    /// <summary>
    /// Reasonable upper bound for missions flown previously.
    /// </summary>
    private const int MaxMissionCount = 160;

    /// <summary>
    /// Choose a mission and attempt it. If attempt fails, perform an enemy counterattack.
    /// </summary>
    public override void Play()
    {
        Mission mission = ChooseMission();
        UI.Output(mission.Description);

        int missionCount = MissionCountFromUI();
        CommentOnMissionCount(missionCount);

        AttemptMission(missionCount);
    }

    /// <summary>
    /// Choose a mission.
    /// </summary>
    /// <returns>Mission chosen.</returns>
    private Mission ChooseMission()
    {
        IList<Mission> missions = AllMissions;
        string[] missionNames = missions.Select(a => a.Name).ToArray();
        int index = UI.Choose(ChooseMissionMessage, missionNames);
        return missions[index];
    }

    /// <summary>
    /// Message to display when choosing a mission.
    /// </summary>
    protected abstract string ChooseMissionMessage { get; }

    /// <summary>
    /// All aviailable missions to choose from.
    /// </summary>
    protected abstract IList<Mission> AllMissions { get; }

    /// <summary>
    /// Get mission count from UI. If mission count exceeds a reasonable maximum, ask UI again.
    /// </summary>
    /// <returns>Mission count from UI.</returns>
    private int MissionCountFromUI()
    {
        const string HowManyMissions = "HOW MANY MISSIONS HAVE YOU FLOWN?";
        string inputMessage = HowManyMissions;

        bool resultIsValid;
        int result;
        do
        {
            UI.Output(inputMessage);
            result = UI.InputInteger();
            if (result < 0)
            {
                UI.Output($"NUMBER OF MISSIONS CAN'T BE NEGATIVE.");
                resultIsValid = false;
            }
            else if (result > MaxMissionCount)
            {
                resultIsValid = false;
                UI.Output($"MISSIONS, NOT MILES...{MaxMissionCount} MISSIONS IS HIGH EVEN FOR OLD-TIMERS.");
                inputMessage = "NOW THEN, " + HowManyMissions;
            }
            else
            {
                resultIsValid = true;
            }
        }
        while (!resultIsValid);

        return result;
    }

    /// <summary>
    /// Display a message about the given mission count, if it is unusually high or low.
    /// </summary>
    /// <param name="missionCount">Mission count to comment on.</param>
    private void CommentOnMissionCount(int missionCount)
    {
        if (missionCount >= 100)
        {
            UI.Output("THAT'S PUSHING THE ODDS!");
        }
        else if (missionCount < 25)
        {
            UI.Output("FRESH OUT OF TRAINING, EH?");
        }
    }

    /// <summary>
    /// Attempt mission.
    /// </summary>
    /// <param name="missionCount">Number of missions previously flown. Higher mission counts will yield a higher probability of success.</param>
    private void AttemptMission(int missionCount)
    {
        if (missionCount < RandomInteger(0, MaxMissionCount))
        {
            MissedTarget();
        }
        else
        {
            MissionSucceeded();
        }
    }

    /// <summary>
    /// Display message indicating that target was missed. Choose enemy artillery and perform a counterattack.
    /// </summary>
    private void MissedTarget()
    {
        UI.Output("MISSED TARGET BY " + (2 + RandomInteger(0, 30)) + " MILES!");
        UI.Output("NOW YOU'RE REALLY IN FOR IT !!");

        // Choose enemy and counterattack.
        EnemyArtillery enemyArtillery = ChooseEnemyArtillery();

        if (enemyArtillery == Missiles)
        {
            EnemyCounterattack(enemyArtillery, hitRatePercent: 0);
        }
        else
        {
            int hitRatePercent = EnemyHitRatePercentFromUI();
            if (hitRatePercent < MinEnemyHitRatePercent)
            {
                UI.Output("YOU LIE, BUT YOU'LL PAY...");
                MissionFailed();
            }
            else
            {
                EnemyCounterattack(enemyArtillery, hitRatePercent);
            }
        }
    }

    /// <summary>
    /// Choose enemy artillery from UI.
    /// </summary>
    /// <returns>Artillery chosen.</returns>
    private EnemyArtillery ChooseEnemyArtillery()
    {
        EnemyArtillery[] artilleries = new EnemyArtillery[] { Guns, Missiles, Both };
        string[] artilleryNames = artilleries.Select(a => a.Name).ToArray();
        int index = UI.Choose("DOES THE ENEMY HAVE", artilleryNames);
        return artilleries[index];
    }

    /// <summary>
    /// Minimum allowed hit rate percent.
    /// </summary>
    private const int MinEnemyHitRatePercent = 10;

    /// <summary>
    /// Maximum allowed hit rate percent.
    /// </summary>
    private const int MaxEnemyHitRatePercent = 50;

    /// <summary>
    /// Get the enemy hit rate percent from UI. Value must be between zero and <see cref="MaxEnemyHitRatePercent"/>.
    /// If value is less than <see cref="MinEnemyHitRatePercent"/>, mission fails automatically because the user is
    /// assumed to be untruthful.
    /// </summary>
    /// <returns>Enemy hit rate percent from UI.</returns>
    private int EnemyHitRatePercentFromUI()
    {
        UI.Output($"WHAT'S THE PERCENT HIT RATE OF ENEMY GUNNERS ({MinEnemyHitRatePercent} TO {MaxEnemyHitRatePercent})");

        bool resultIsValid;
        int result;
        do
        {
            result = UI.InputInteger();
            // Let them enter a number below the stated minimum, as they will be caught and punished.
            if (0 <= result && result <= MaxEnemyHitRatePercent)
            {
                resultIsValid = true;
            }
            else
            {
                resultIsValid = false;
                UI.Output($"NUMBER MUST BE FROM {MinEnemyHitRatePercent} TO {MaxEnemyHitRatePercent}");
            }
        }
        while (!resultIsValid);

        return result;
    }
}
