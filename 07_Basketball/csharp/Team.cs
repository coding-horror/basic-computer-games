using Basketball.Plays;

namespace Basketball;

internal record Team(string Name, Play PlayResolver)
{
    public override string ToString() => Name;

    public bool ResolvePlay(Scoreboard scoreboard) => PlayResolver.Resolve(scoreboard);
}
