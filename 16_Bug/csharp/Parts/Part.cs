using BugGame.Resources;

namespace BugGame.Parts;

internal class Part
{
    private readonly Message _addedMessage;
    private readonly Message _duplicateMessage;

    public Part(Message addedMessage, Message duplicateMessage)
    {
        _addedMessage = addedMessage;
        _duplicateMessage = duplicateMessage;
    }

    public virtual bool IsComplete => IsPresent;

    protected bool IsPresent { get; private set; }

    public string Name => GetType().Name;

    public bool TryAdd(out Message message)
    {
        if (IsPresent)
        {
            message = _duplicateMessage;
            return false;
        }

        message = _addedMessage;
        IsPresent = true;
        return true;
    }
}
