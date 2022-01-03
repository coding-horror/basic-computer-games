namespace BombsAwayGame;

/// <summary>
/// Plays the Bombs Away game using a supplied <see cref="IUserInterface"/>.
/// </summary>
public class Game
{
    private readonly IUserInterface _ui;

    /// <summary>
    /// Create game instance using the given UI.
    /// </summary>
    /// <param name="ui">UI to use for game.</param>
    public Game(IUserInterface ui)
    {
        _ui = ui;
    }

    /// <summary>
    /// Play game. Choose a side and play the side's logic.
    /// </summary>
    public void Play()
    {
        _ui.Output("YOU ARE A PILOT IN A WORLD WAR II BOMBER.");
        Side side = ChooseSide();
        side.Play();
    }

    /// <summary>
    /// Represents a <see cref="Side"/>.
    /// </summary>
    /// <param name="Name">Name of side.</param>
    /// <param name="CreateSide">Create instance of side that this descriptor represents.</param>
    private record class SideDescriptor(string Name, Func<Side> CreateSide);

    /// <summary>
    /// Choose side and return a new instance of that side.
    /// </summary>
    /// <returns>New instance of side that was chosen.</returns>
    private Side ChooseSide()
    {
        SideDescriptor[] sides = AllSideDescriptors;
        string[] sideNames = sides.Select(a => a.Name).ToArray();
        int index = _ui.Choose("WHAT SIDE", sideNames);
        return sides[index].CreateSide();
    }

    /// <summary>
    /// All side descriptors.
    /// </summary>
    private SideDescriptor[] AllSideDescriptors => new SideDescriptor[]
    {
        new("ITALY", () => new ItalySide(_ui)),
        new("ALLIES", () => new AlliesSide(_ui)),
        new("JAPAN", () => new JapanSide(_ui)),
        new("GERMANY", () => new GermanySide(_ui)),
    };
}
