using BugGame.Resources;

namespace BugGame.Parts;

internal class PartCollection
{
    private readonly int _maxCount;
    private readonly Message _addedMessage;
    private readonly Message _fullMessage;
    private int _count;

    public PartCollection(int maxCount, Message addedMessage, Message fullMessage)
    {
        _maxCount = maxCount;
        _addedMessage = addedMessage;
        _fullMessage = fullMessage;
    }

    public bool IsComplete => _count == _maxCount;

    public bool TryAddOne(out Message message)
    {
        if (_count < _maxCount)
        {
            _count++;
            message = _addedMessage.ForQuantity(_count);
            return true;
        }

        message = _fullMessage;
        return false;
    }
}
