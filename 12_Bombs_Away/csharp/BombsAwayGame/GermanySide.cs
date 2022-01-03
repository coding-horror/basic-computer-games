namespace BombsAwayGame;

/// <summary>
/// Germany protagonist. Can fly missions to Russia, England, and France.
/// </summary>
internal class GermanySide : MissionSide
{
    public GermanySide(IUserInterface ui)
        : base(ui)
    {
    }

    protected override string ChooseMissionMessage => "A NAZI, EH?  OH WELL.  ARE YOU GOING FOR";

    protected override IList<Mission> AllMissions => new Mission[]
    {
        new("RUSSIA", "YOU'RE NEARING STALINGRAD."),
        new("ENGLAND", "NEARING LONDON.  BE CAREFUL, THEY'VE GOT RADAR."),
        new("FRANCE", "NEARING VERSAILLES.  DUCK SOUP.  THEY'RE NEARLY DEFENSELESS.")
    };
}
