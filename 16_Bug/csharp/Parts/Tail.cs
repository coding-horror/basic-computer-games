using BugGame.Resources;

namespace BugGame.Parts;

internal class Tail : Part
{
    public Tail()
        : base(Message.TailAdded, Message.TailNotNeeded)
    {
    }
}