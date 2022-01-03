namespace BombsAwayGame;

/// <summary>
/// Italy protagonist. Can fly missions to Albania, Greece, and North Africa.
/// </summary>
internal class ItalySide : MissionSide
{
    public ItalySide(IUserInterface ui)
        : base(ui)
    {
    }

    protected override string ChooseMissionMessage => "YOUR TARGET";

    protected override IList<Mission> AllMissions => new Mission[]
    {
        new("ALBANIA", "SHOULD BE EASY -- YOU'RE FLYING A NAZI-MADE PLANE."),
        new("GREECE", "BE CAREFUL!!!"),
        new("NORTH AFRICA", "YOU'RE GOING FOR THE OIL, EH?")
    };
}
