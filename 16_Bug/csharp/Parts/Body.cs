using BugGame.Resources;

namespace BugGame.Parts;

internal class Body : ParentPart
{
    private readonly Neck _neck = new();
    private readonly Tail _tail = new();
    private readonly Legs _legs = new();

    public Body()
        : base(Message.BodyAdded, Message.BodyNotNeeded)
    {
    }

    protected override bool TryAddCore(IPart part, out Message message)
        => part switch
        {
            Neck => _neck.TryAdd(out message),
            Head or Feeler => _neck.TryAdd(part, out message),
            Tail => _tail.TryAdd(out message),
            Leg => _legs.TryAddOne(out message),
            _ => throw new NotSupportedException($"Can't add a {part.Name} to a {Name}.")
        };
}
