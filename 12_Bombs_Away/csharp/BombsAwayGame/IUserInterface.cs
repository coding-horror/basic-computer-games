namespace BombsAwayGame;

/// <summary>
/// Represents an interface for supplying data to the game.
/// </summary>
/// <remarks>
/// Abstracting the UI allows us to concentrate its concerns in one part of our code and to change UI behavior
/// without creating any risk of changing the game logic. It also allows us to supply an automated UI for tests.
/// </remarks>
public interface IUserInterface
{
    /// <summary>
    /// Display the given message.
    /// </summary>
    /// <param name="message">Message to display.</param>
    void Output(string message);

    /// <summary>
    /// Choose an item from the given choices.
    /// </summary>
    /// <param name="message">Message to display.</param>
    /// <param name="choices">Choices to choose from.</param>
    /// <returns>Index of choice in <paramref name="choices"/> that user chose.</returns>
    int Choose(string message, IList<string> choices);

    /// <summary>
    /// Allow user to choose Yes or No.
    /// </summary>
    /// <param name="message">Message to display.</param>
    /// <returns>True if user chose Yes, false if user chose No.</returns>
    bool ChooseYesOrNo(string message);

    /// <summary>
    /// Get integer from user.
    /// </summary>
    /// <returns>Integer supplied by user.</returns>
    int InputInteger();
}
