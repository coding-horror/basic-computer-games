namespace BombsAwayGame;

/// <summary>
/// Japan protagonist. Flies a kamikaze mission, which has a different logic from <see cref="MissionSide"/>s.
/// </summary>
internal class JapanSide : Side
{
    public JapanSide(IUserInterface ui)
        : base(ui)
    {
    }

    /// <summary>
    /// Perform a kamikaze mission. If first kamikaze mission, it will succeed 65% of the time. If it's not
    /// first kamikaze mission, perform an enemy counterattack.
    /// </summary>
    public override void Play()
    {
        UI.Output("YOU'RE FLYING A KAMIKAZE MISSION OVER THE USS LEXINGTON.");

        bool isFirstMission = UI.ChooseYesOrNo("YOUR FIRST KAMIKAZE MISSION(Y OR N)?");
        if (!isFirstMission)
        {
            // LINE 207 of original BASIC: hitRatePercent is initialized to 0,
            // but R, the type of artillery, is not initialized at all. Setting
            // R = 1, which is to say EnemyArtillery = Guns, gives the same result.
            EnemyCounterattack(Guns, hitRatePercent: 0);
        }
        else if (RandomFrac() > 0.65)
        {
            MissionSucceeded();
        }
        else
        {
            MissionFailed();
        }
    }
}
