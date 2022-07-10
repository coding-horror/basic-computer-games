using BugGame.Resources;

namespace BugGame.Parts;

internal class Legs : PartCollection
{
    public Legs()
        : base(2, Message.LegAdded, Message.LegsFull)
    {
    }
}
