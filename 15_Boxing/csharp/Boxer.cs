namespace Boxing;

public class Boxer
{
    private int _wins;

    private string Name { get; set; } = string.Empty;

    public Punch BestPunch { get; set; }

    public Punch Vulnerability { get; set; }

    public void SetName(string prompt)
    {
        Console.WriteLine(prompt);
        string? name;
        do
        {
            name = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(name));
        Name = name;
    }

    public int DamageTaken { get; set; }

    public void ResetForNewRound() => DamageTaken = 0;

    public void RecordWin() => _wins += 1;

    public bool IsWinner => _wins >= 2;

    public override string ToString() => Name;
}

public class Opponent : Boxer
{
    public void SetRandomPunches()
    {
        do
        {
            BestPunch = (Punch) GameUtils.Roll(4); // B1
            Vulnerability = (Punch) GameUtils.Roll(4); // D1
        } while (BestPunch == Vulnerability);
    }
}
