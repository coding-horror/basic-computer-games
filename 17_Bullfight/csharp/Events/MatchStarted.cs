namespace Game.Events
{
    /// <summary>
    /// Indicates that a new match has started.
    /// </summary>
    public sealed record MatchStarted(
        Quality BullQuality,
        Quality ToreadorePerformance,
        Quality PicadorePerformance,
        int ToreadoresKilled,
        int PicadoresKilled,
        int HorsesKilled) : Event;
}
