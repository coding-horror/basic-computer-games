namespace Games.Common.IO;

public class InsufficientInputException : Exception
{
    public InsufficientInputException()
        : base("Insufficient input was supplied")
    {
    }
}
