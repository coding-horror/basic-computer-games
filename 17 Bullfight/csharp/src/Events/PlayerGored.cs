namespace Game.Events
{
    /// <summary>
    /// Indicates that the player has been gored by the bull.
    /// </summary>
    public sealed record PlayerGored(bool Panicked, bool FirstGoring) : Event;
}
