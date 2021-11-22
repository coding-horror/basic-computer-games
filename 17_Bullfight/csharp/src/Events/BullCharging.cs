namespace Game.Events
{
    /// <summary>
    /// Indicates that the bull is charing the player.
    /// </summary>
    public sealed record BullCharging(int PassNumber) : Event;
}
