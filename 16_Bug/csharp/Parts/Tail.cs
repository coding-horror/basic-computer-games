using System.Text;
using BugGame.Resources;

namespace BugGame.Parts;

internal class Tail : Part
{
    public Tail()
        : base(Message.TailAdded, Message.TailNotNeeded)
    {
    }

    public void AppendTo(StringBuilder builder) => builder.AppendLine("TTTTTB          B");
}