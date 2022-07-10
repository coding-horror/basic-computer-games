using BugGame.Resources;

namespace BugGame.Parts;

internal class Head : ParentPart
{
    private Feelers _feelers = new();

    public Head()
        : base(Message.HeadAdded, Message.HeadNotNeeded)
    {
    }

    protected override bool TryAddCore(IPart part, out Message message)
        => part switch
        {
            Feeler => _feelers.TryAddOne(out message),
            _ => throw new NotSupportedException($"Can't add a {part.Name} to a {Name}.")
        };
}
