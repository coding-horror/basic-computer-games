namespace Game
{
    /// <summary>
    /// Represents the state of the game during the final campaign of the war.
    /// </summary>
    public sealed class FinalCampaign : WarState
    {
        /// <summary>
        /// Initializes a new instance of the FinalCampaign class.
        /// </summary>
        /// <param name="computerForces">
        /// The computer's forces.
        /// </param>
        /// <param name="playerForces">
        /// The player's forces.
        /// </param>
        public FinalCampaign(ArmedForces computerForces, ArmedForces playerForces)
            : base(computerForces, playerForces)
        {
        }

        protected override (WarState nextState, string message) AttackWithArmy(int attackSize)
        {
            if (attackSize < ComputerForces.Army / 2)
            {
                return
                (
                    new Ceasefire(
                        ComputerForces,
                        PlayerForces with
                        {
                            Army = PlayerForces.Army - attackSize
                        }),
                    "I WIPED OUT YOUR ATTACK!"
                );
            }
            else
            {
                return
                (
                    new Ceasefire(
                        ComputerForces with
                        {
                            Army = 0
                        },
                        PlayerForces),
                    "YOU DESTROYED MY ARMY!"
                );
            }
        }

        protected override (WarState nextState, string message) AttackWithNavy(int attackSize)
        {
            if (attackSize < ComputerForces.Navy / 2)
            {
                return
                (
                    new Ceasefire(
                        ComputerForces,
                        PlayerForces with
                        {
                            Army = PlayerForces.Army / 4,
                            Navy = PlayerForces.Navy / 2
                        }),
                    "I SUNK TWO OF YOUR BATTLESHIPS, AND MY AIR FORCE\n" +
                    "WIPED OUT YOUR UNGAURDED CAPITOL."
                );
            }
            else
            {
                return
                (
                    new Ceasefire(
                        ComputerForces with
                        {
                            AirForce = 2 * ComputerForces.AirForce / 3,
                            Navy     = ComputerForces.Navy / 2
                        },
                        PlayerForces),
                    "YOUR NAVY SHOT DOWN THREE OF MY XIII PLANES,\n" +
                    "AND SUNK THREE BATTLESHIPS."
                );
            }
        }

        protected override (WarState nextState, string message) AttackWithAirForce(int attackSize)
        {
            // BUG? Usually, larger attacks lead to better outcomes.
            //  It seems odd that the logic is suddenly reversed here,
            //  but this could be intentional.
            if (attackSize > ComputerForces.AirForce / 2)
            {
                return
                (
                    new Ceasefire(
                        ComputerForces,
                        PlayerForces with
                        {
                            Army     = PlayerForces.Army  / 3,
                            Navy     = PlayerForces.Navy / 3,
                            AirForce = PlayerForces.AirForce / 3
                        }),
                    "MY NAVY AND AIR FORCE IN A COMBINED ATTACK LEFT\n" +
                    "YOUR COUNTRY IN SHAMBLES."
                );
            }
            else
            {
                return
                (
                    new Ceasefire(
                        ComputerForces,
                        PlayerForces,
                        absoluteVictory: true),
                    "ONE OF YOUR PLANES CRASHED INTO MY HOUSE. I AM DEAD.\n" +
                    "MY COUNTRY FELL APART."
                );
            }
        }
    }
}
