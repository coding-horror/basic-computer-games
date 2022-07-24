using System.Text;
using BugGame.Resources;

namespace BugGame.Parts;

internal class Feelers : PartCollection
{
    public Feelers()
        : base(2, Message.FeelerAdded, Message.FeelersFull)
    {
    }

    public void AppendTo(StringBuilder builder, char character) => AppendTo(builder, 10, 4, character);
}
