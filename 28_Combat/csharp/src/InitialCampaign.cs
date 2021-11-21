namespace Game
{
    /// <summary>
    /// Represents the state of the game during the initial campaign of the war.
    /// </summary>
    public sealed class InitialCampaign : WarState
    {
        /// <summary>
        /// Initializes a new instance of the InitialCampaign class.
        /// </summary>
        /// <param name="computerForces">
        /// The computer's forces.
        /// </param>
        /// <param name="playerForces">
        /// The player's forces.
        /// </param>
        public InitialCampaign(ArmedForces computerForces, ArmedForces playerForces)
            : base(computerForces, playerForces)
        {
        }

        protected override (WarState nextState, string message) AttackWithArmy(int attackSize)
        {
            // BUG: Why are we comparing attack size to the size of our own
            //   military?  This leads to some truly absurd results if our
            //   army is tiny.
            if (attackSize < PlayerForces.Army / 3)
            {
                return
                (
                    new FinalCampaign(
                        ComputerForces,
                        PlayerForces with
                        {
                            Army = PlayerForces.Army - attackSize
                        }),
                    $"YOU LOST {attackSize} MEN FROM YOUR ARMY."
                );
            }
            else
            if (attackSize < 2 * PlayerForces.Army / 3)
            {
                return
                (
                    new FinalCampaign(
                        ComputerForces with
                        {
                            // BUG: Clearly not what we claim below...
                            Army = 0
                        },
                        PlayerForces with
                        {
                            Army = PlayerForces.Army - attackSize / 3
                        }),
                    $"YOU LOST {attackSize / 3} MEN, BUT I LOST {2 * ComputerForces.Army / 3}"
                );
            }
            else
            {
                // BUG? This is identical to the third outcome when attacking
                //  with the navy.  It seems unlikely that this was the
                //  intent.  Probably line 115 in the original source was
                //  supposed to say "GOTO 170" instead of "GOTO 270".
                //  (Line 170 is conspicuously absent.)
                return
                (
                    new FinalCampaign(
                        ComputerForces with
                        {
                            Navy = 2 * ComputerForces.Navy / 3
                        },
                        PlayerForces with
                        {
                            Army     = PlayerForces.Army / 3,
                            AirForce = PlayerForces.AirForce / 3
                        }),
                    "YOU SUNK ONE OF MY PATROL BOATS, BUT I WIPED OUT TWO\n" +
                    "OF YOUR AIR FORCE BASES AND 3 ARMY BASES."
                );
            }
        }

        protected override (WarState nextState, string message) AttackWithNavy(int attackSize)
        {
            if (attackSize < ComputerForces.Navy / 3)
            {
                return
                (
                    new FinalCampaign(
                        ComputerForces,
                        PlayerForces with
                        {
                            Navy = PlayerForces.Navy - attackSize
                        }),
                    "YOUR ATTACK WAS STOPPED!"
                );
            }
            else
            if (attackSize < 2 * ComputerForces.Navy / 3)
            {
                return
                (
                    new FinalCampaign(
                        ComputerForces with
                        {
                            Navy = ComputerForces.Navy / 3
                        },
                        PlayerForces),
                    $"YOU DESTROYED {2 * ComputerForces.Navy / 3} OF MY ARMY."
                );
            }
            else
            {
                return
                (
                    new FinalCampaign(
                        ComputerForces with
                        {
                            Navy = 2 * ComputerForces.Navy / 3
                        },
                        PlayerForces with
                        {
                            Army     = PlayerForces.Army / 3,
                            AirForce = PlayerForces.AirForce / 3
                        }),
                    "YOU SUNK ONE OF MY PATROL BOATS, BUT I WIPED OUT TWO\n" +
                    "OF YOUR AIR FORCE BASES AND 3 ARMY BASES."
                );
            }
        }

        protected override (WarState nextState, string message) AttackWithAirForce(int attackSize)
        {
            // BUG: Why are we comparing the attack size to the size of
            //  our own air force?  Surely we meant to compare to the
            //  computer's air force.
            if (attackSize < PlayerForces.AirForce / 3)
            {
                return
                (
                    new FinalCampaign(
                        ComputerForces,
                        PlayerForces with
                        {
                            AirForce = PlayerForces.AirForce - attackSize
                        }),
                    "YOUR ATTACK WAS WIPED OUT."
                );
            }
            else
            if (attackSize < 2 * PlayerForces.AirForce / 3)
            {
                return
                (
                    new FinalCampaign(
                        ComputerForces with
                        {
                            Army     = 2 * ComputerForces.Army / 3,
                            Navy     = ComputerForces.Navy / 3,
                            AirForce = ComputerForces.AirForce / 3
                        },
                        PlayerForces),
                    "WE HAD A DOGFIGHT. YOU WON - AND FINISHED YOUR MISSION."
                );
            }
            else
            {

                return
                (
                    new FinalCampaign(
                        ComputerForces with
                        {
                            Army = 2 * ComputerForces.Army / 3
                        },
                        PlayerForces with
                        {
                            Army = PlayerForces.Army / 4,
                            Navy = PlayerForces.Navy / 3
                        }),
                    "YOU WIPED OUT ONE OF MY ARMY PATROLS, BUT I DESTROYED" +
                    "TWO NAVY BASES AND BOMBED THREE ARMY BASES."
                );
            }
        }
    }
}
