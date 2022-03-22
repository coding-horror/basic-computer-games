using Games.Common.Randomness;

namespace SuperStarTrek.Space;

internal class QuadrantInfo
{
    private bool _isKnown;

    private QuadrantInfo(Coordinates coordinates, string name, int klingonCount, int starCount, bool hasStarbase)
    {
        Coordinates = coordinates;
        Name = name;
        KlingonCount = klingonCount;
        StarCount = starCount;
        HasStarbase = hasStarbase;
    }

    internal Coordinates Coordinates { get; }

    internal string Name { get; }

    internal int KlingonCount { get; private set; }

    internal bool HasStarbase { get; private set; }

    internal int StarCount { get; }

    internal static QuadrantInfo Create(Coordinates coordinates, string name, IRandom random)
    {
        var klingonCount = random.NextFloat() switch
        {
            > 0.98f => 3,
            > 0.95f => 2,
            > 0.80f => 1,
            _ => 0
        };
        var hasStarbase = random.NextFloat() > 0.96f;
        var starCount = random.Next1To8Inclusive();

        return new QuadrantInfo(coordinates, name, klingonCount, starCount, hasStarbase);
    }

    internal void AddKlingon() => KlingonCount += 1;

    internal void AddStarbase() => HasStarbase = true;

    internal void MarkAsKnown() => _isKnown = true;

    internal string Scan()
    {
        _isKnown = true;
        return ToString();
    }

    public override string ToString() => _isKnown ? $"{KlingonCount}{(HasStarbase ? 1 : 0)}{StarCount}" : "***";

    internal void RemoveKlingon()
    {
        if (KlingonCount > 0)
        {
            KlingonCount -= 1;
        }
    }

    internal void RemoveStarbase() => HasStarbase = false;
}
