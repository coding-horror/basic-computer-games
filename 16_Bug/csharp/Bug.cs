using System.Text;
using BugGame.Parts;
using BugGame.Resources;

namespace BugGame;

internal class Bug
{
    private readonly Body _body = new();

    public bool IsComplete => _body.IsComplete;

    public bool TryAdd(IPart part, out Message message) => _body.TryAdd(part, out message);

    public string ToString(string pronoun, char feelerCharacter)
    {
        var builder = new StringBuilder($"*****{pronoun} Bug*****").AppendLine().AppendLine().AppendLine();
        _body.AppendTo(builder, feelerCharacter);
        return builder.ToString();
    }
}