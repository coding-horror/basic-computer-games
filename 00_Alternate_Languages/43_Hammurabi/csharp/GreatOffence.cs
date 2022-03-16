using System;

namespace Hammurabi
{
    /// <summary>
    /// Indicates that the game cannot continue due to the player's extreme
    /// incompetance and/or unserious attitude!
    /// </summary>
    public class GreatOffence : InvalidOperationException
    {
    }
}
