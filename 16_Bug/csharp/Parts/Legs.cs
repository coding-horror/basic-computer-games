using System.Text;
using BugGame.Resources;

namespace BugGame.Parts;

internal class Legs : PartCollection
{
    public Legs()
        : base(6, Message.LegAdded, Message.LegsFull)
    {
    }

    public void AppendTo(StringBuilder builder) => AppendTo(builder, 6, 2, 'L');
}
