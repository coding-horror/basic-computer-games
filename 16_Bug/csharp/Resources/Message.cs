using BugGame.Parts;

namespace BugGame.Resources;

internal class Message
{
    public static Message Rolled = new("rolled a {0}");

    public static Message BodyAdded = new("now have a body.");
    public static Message BodyNotNeeded = new("do not need a body.");

    public static Message NeckAdded = new("now have a neck.");
    public static Message NeckNotNeeded = new("do not need a neck.");

    public static Message HeadAdded = new("needed a head.");
    public static Message HeadNotNeeded = new("I do not need a head.", "You have a head.");

    public static Message TailAdded = new("I now have a tail.", "I now give you a tail.");
    public static Message TailNotNeeded = new("I do not need a tail.", "You already have a tail.");

    public static Message FeelerAdded = new("I get a feeler.", "I now give you a feeler");
    public static Message FeelersFull = new("I have 2 feelers already.", "You have two feelers already");

    public static Message LegAdded = new("now have {0} legs");
    public static Message LegsFull = new("I have 6 feet.", "You have 6 feet already");

    private Message(string common)
        : this("I " + common, "You" + common)
    {
    }

    private Message(string i, string you)
    {
        I = i;
        You = you;
    }

    public string I { get; }
    public string You { get; }

    public static Message DoNotHaveA(Part part) => new($"do no have a {part.Name}");

    public Message ForQuantity(int quantity) => new(string.Format(I, quantity), string.Format(You, quantity));
}