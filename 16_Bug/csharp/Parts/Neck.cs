using BugGame.Resources;

namespace BugGame.Parts;

internal class Neck : ParentPart
{
    private Head _head = new();

    public Neck()
        : base(Message.NeckAdded, Message.NeckNotNeeded)
    {
    }

    public override bool IsComplete => _head.IsComplete;

    protected override bool TryAddCore(IPart part, out Message message)
        => part switch
        {
            Head => _head.TryAdd(out message),
            Feeler => _head.TryAdd(part, out message),
            _ => throw new NotSupportedException($"Can't add a {part.Name} to a {Name}.")
        };
}
