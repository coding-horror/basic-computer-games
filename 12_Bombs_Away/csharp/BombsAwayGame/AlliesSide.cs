namespace BombsAwayGame;

/// <summary>
/// Allies protagonist. Can fly missions in a Liberator, B-29, B-17, or Lancaster.
/// </summary>
internal class AlliesSide : MissionSide
{
    public AlliesSide(IUserInterface ui)
        : base(ui)
    {
    }

    protected override string ChooseMissionMessage => "AIRCRAFT";

    protected override IList<Mission> AllMissions => new Mission[]
    {
        new("LIBERATOR", "YOU'VE GOT 2 TONS OF BOMBS FLYING FOR PLOESTI."),
        new("B-29", "YOU'RE DUMPING THE A-BOMB ON HIROSHIMA."),
        new("B-17", "YOU'RE CHASING THE BISMARK IN THE NORTH SEA."),
        new("LANCASTER", "YOU'RE BUSTING A GERMAN HEAVY WATER PLANT IN THE RUHR.")
    };
}
