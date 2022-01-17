namespace Game.Events
{
    /// <summary>
    /// Indicates that the fight has completed.
    /// </summary>
    public sealed record MatchCompleted(ActionResult Result, bool ExtremeBravery, Reward Reward) : Event;
}
