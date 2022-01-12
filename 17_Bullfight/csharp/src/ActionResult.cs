using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// Enumerates the different possible outcomes of the player's action.
    /// </summary>
    public enum ActionResult
    {
        /// <summary>
        /// The fight continues.
        /// </summary>
        FightContinues,

        /// <summary>
        /// The player fled from the ring.
        /// </summary>
        PlayerFlees,

        /// <summary>
        /// The bull has gored the player.
        /// </summary>
        BullGoresPlayer,

        /// <summary>
        /// The bull killed the player.
        /// </summary>
        BullKillsPlayer,

        /// <summary>
        /// The player killed the bull.
        /// </summary>
        PlayerKillsBull,

        /// <summary>
        /// The player attempted to kill the bull and both survived.
        /// </summary>
        Draw
    }
}
