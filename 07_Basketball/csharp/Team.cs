namespace Basketball;

internal record Team(string Name, Action<Scoreboard> PlayResolution)
{
    public override string ToString() => Name;

    public void ResolvePlay(Scoreboard scoreboard) => PlayResolution.Invoke(scoreboard);
}
