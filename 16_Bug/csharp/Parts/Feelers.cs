using BugGame.Resources;

namespace BugGame.Parts;

internal class Feelers : PartCollection
{
    public Feelers()
        : base(6, Message.FeelerAdded, Message.FeelersFull)
    {
    }
}
