using System;
using System.Collections.Generic;
using System.Text;

namespace AceyDucey
{
    /// <summary>
    /// The GameState class keeps track of all the game variables while the game is being played
    /// </summary>
    internal class GameState
    {
        
        /// <summary>
        /// How much money does the player have at the moment?
        /// </summary>
        internal int Money { get; set; }

        /// <summary>
        /// What's the highest amount of money they had at any point in the game?
        /// </summary>
        internal int MaxMoney { get; set; }

        /// <summary>
        /// How many turns have they played?
        /// </summary>
        internal int TurnCount { get; set; }

        /// <summary>
        /// Class constructor -- initialise all values to their defaults.
        /// </summary>
        internal GameState()
        {
            // Setting Money to 100 gives the player their starting balance. Changing this will alter how much they have to begin with.
            Money = 100;
            MaxMoney = Money;
            TurnCount = 0;
        }
    }
}
