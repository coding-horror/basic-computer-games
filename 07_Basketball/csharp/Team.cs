namespace Basketball;

internal record Team(string Name, Func<Scoreboard, bool> PlayResolution)
{
    public override string ToString() => Name;

    public bool ResolvePlay(Scoreboard scoreboard) => PlayResolution.Invoke(scoreboard);
}
