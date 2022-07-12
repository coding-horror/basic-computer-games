using BugGame.Resources;

namespace BugGame.Parts;

internal abstract class ParentPart : Part
{
    public ParentPart(Message addedMessage, Message duplicateMessage)
        : base(addedMessage, duplicateMessage)
    {
    }

    public bool TryAdd(IPart part, out Message message)
        => (part.GetType() == GetType(), IsPresent) switch
        {
            (true, _) => TryAdd(out message),
            (false, false) => ReportDoNotHave(out message),
            _ => TryAddCore(part, out message)
        };

    protected abstract bool TryAddCore(IPart part, out Message message);

    private bool ReportDoNotHave(out Message message)
    {
        message = Message.DoNotHaveA(this);
        return false;
    }
}
