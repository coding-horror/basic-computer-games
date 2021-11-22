using System;

namespace FurTrader
{
    public class Game
    {
        /// <summary>
        /// random number generator; no seed to be faithful to original implementation
        /// </summary>
        private Random Rnd { get; } = new Random();

        /// <summary>
        /// Generate a price for pelts based off a factor and baseline value
        /// </summary>
        /// <param name="factor">Multiplier for the price</param>
        /// <param name="baseline">The baseline price</param>
        /// <returns>A randomised price for pelts</returns>
        internal double RandomPriceGenerator(double factor, double baseline)
        {
            var price = (Convert.ToInt32((factor * Rnd.NextDouble() + baseline) * 100d) + 5) / 100d;
            return price;
        }

        /// <summary>
        /// Main game loop function. This will play the game endlessly until the player chooses to quit or a GameOver event occurs
        /// </summary>
        /// <remarks>
        /// General structure followed from Adam Dawes (@AdamDawes575) implementation of Acey Ducey.");
        /// </remarks>
        internal void GameLoop()
        {
            // display instructions to the player
            DisplayIntroText();

            var state = new GameState();

            // loop for each turn until the player decides not to continue (or has a Game Over event)
            while ((!state.GameOver) && ContinueGame())
            {
                // clear display at start of each turn
                Console.Clear();

                // play the next turn; pass game state for details and updates from the turn
                PlayTurn(state);
            }

            // end screen; show some statistics to the player
            Console.Clear();
            Console.WriteLine("Thanks for playing!");
            Console.WriteLine("");
            Console.WriteLine($"Total Expeditions: {state.ExpeditionCount}");
            Console.WriteLine($"Final Amount:      {state.Savings.ToString("c")}");
        }

        /// <summary>
        /// Display instructions on how to play the game and wait for the player to press a key.
        /// </summary>
        private void DisplayIntroText()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Fur Trader.");
            Console.WriteLine("Creating Computing, Morristown, New Jersey.");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Originally published in 1978 in the book 'Basic Computer Games' by David Ahl.");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("You are the leader of a French fur trading expedition in 1776 leaving the Lake Ontario area to sell furs and get supplies for the next year.");
            Console.WriteLine("");
            Console.WriteLine("You have a choice of three forts at which you may trade. The cost of supplies and the amount you receive for your furs will depend on the fort that you choose.");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Press any key start the game.");
            Console.ReadKey(true);

        }

        /// <summary>
        /// Prompt the player to try again, and wait for them to press Y or N.
        /// </summary>
        /// <returns>Returns true if the player wants to try again, false if they have finished playing.</returns>
        private bool ContinueGame()
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Do you wish to trade furs? ");
            Console.Write("Answer (Y)es or (N)o ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("> ");

            char pressedKey;
            // Keep looping until we get a recognised input
            do
            {
                // Read a key, don't display it on screen
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Convert to upper-case so we don't need to care about capitalisation
                pressedKey = Char.ToUpper(key.KeyChar);
                // Is this a key we recognise? If not, keep looping
            } while (pressedKey != 'Y' && pressedKey != 'N');

            // Display the result on the screen
            Console.WriteLine(pressedKey);

            // Return true if the player pressed 'Y', false for anything else.
            return (pressedKey == 'Y');
        }

        /// <summary>
        /// Play a turn
        /// </summary>
        /// <param name="state">The current game state</param>
        private void PlayTurn(GameState state)
        {
            state.UnasignedFurCount = 190;      /// start with 190 furs each turn
            
            // provide current status to user
            Console.WriteLine(new string('_', 70));
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine($"You have {state.Savings.ToString("c")} savings and {state.UnasignedFurCount} furs to begin the expedition.");
            Console.WriteLine("");
            Console.WriteLine($"Your {state.UnasignedFurCount} furs are distributed among the following kinds of pelts: Mink, Beaver, Ermine, and Fox");
            Console.WriteLine("");

            // get input on number of pelts
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("How many Mink pelts do you have? ");
            state.MinkPelts = GetPelts(state.UnasignedFurCount);
            state.UnasignedFurCount -= state.MinkPelts;
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"You have {state.UnasignedFurCount} furs remaining for distribution");            
            Console.Write("How many Beaver pelts do you have? ");
            state.BeaverPelts = GetPelts(state.UnasignedFurCount);
            state.UnasignedFurCount -= state.BeaverPelts;
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"You have {state.UnasignedFurCount} furs remaining for distribution");
            Console.Write("How many Ermine pelts do you have? ");
            state.ErminePelts = GetPelts(state.UnasignedFurCount);
            state.UnasignedFurCount -= state.ErminePelts;
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"You have {state.UnasignedFurCount} furs remaining for distribution");
            Console.Write("How many Fox pelts do you have? ");
            state.FoxPelts = GetPelts(state.UnasignedFurCount);
            state.UnasignedFurCount -= state.FoxPelts;            

            // get input on which fort to trade with; user gets an opportunity to evaluate and re-select fort after selection until user confirms selection
            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("");
                Console.WriteLine("Do you want to trade your furs at Fort 1, Fort 2, or Fort 3");
                Console.WriteLine("Fort 1 is Fort Hochelaga (Montreal) and is under the protection of the French army.");
                Console.WriteLine("Fort 2 is Fort Stadacona (Quebec) and is under the protection of the French army. However, you must make a portage and cross the Lachine rapids.");
                Console.WriteLine("Fort 3 is Fort New York and is under Dutch control. You must cross through Iroquois land.");
                Console.WriteLine("");
                state.SelectedFort = GetSelectedFort();

                DisplaySelectedFortInformation(state.SelectedFort);

            } while (TradeAtAnotherFort());

            // process the travel to the fort
            ProcessExpeditionOutcome(state);

            // display results of expedition (savings change) to the user
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("You now have ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{state.Savings.ToString("c")}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" including your previous savings.");

            // update the turn count now that another turn has been played
            state.ExpeditionCount += 1;
        }

        /// <summary>
        /// Method to show the expedition costs to the player with some standard formatting
        /// </summary>
        /// <param name="fortname">The name of the fort traded with</param>
        /// <param name="supplies">The cost of the supplies at the fort</param>
        /// <param name="expenses">The travel expenses for the expedition</param>
        internal void DisplayCosts(string fortname, double supplies, double expenses)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Supplies at {fortname} cost".PadLeft(55));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{supplies.ToString("c").PadLeft(10)}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Your travel expenses to {fortname} were".PadLeft(55));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{expenses.ToString("c").PadLeft(10)}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Process the results of the expedition
        /// </summary>
        /// <param name="state">the game state</param>
        private void ProcessExpeditionOutcome(GameState state)
        {
            var beaverPrice = RandomPriceGenerator(0.25d, 1.00d);
            var foxPrice =    RandomPriceGenerator(0.2d , 0.80d);
            var erminePrice = RandomPriceGenerator(0.15d, 0.95d);
            var minkPrice =   RandomPriceGenerator(0.2d , 0.70d);

            var fortname = String.Empty;
            var suppliesCost = 0.0d;
            var travelExpenses = 0.0d;

            // create a random value 1 to 10 for the different outcomes at each fort
            var p = ((int)(10 * Rnd.NextDouble())) + 1;
            Console.WriteLine("");

            switch (state.SelectedFort)
            {
                case 1:     // outcome for expedition to Fort Hochelaga
                    beaverPrice = RandomPriceGenerator(0.2d, 0.75d);
                    foxPrice =    RandomPriceGenerator(0.2d, 0.80d);
                    erminePrice = RandomPriceGenerator(0.2d, 0.65d);
                    minkPrice =   RandomPriceGenerator(0.2d, 0.70d);
                    fortname = "Fort Hochelaga";
                    suppliesCost = 150.0d;
                    travelExpenses = 10.0d;
                    break;
                case 2:     // outcome for expedition to Fort Stadacona
                    beaverPrice = RandomPriceGenerator(0.2d , 0.90d);
                    foxPrice =    RandomPriceGenerator(0.2d , 0.80d);
                    erminePrice = RandomPriceGenerator(0.15d, 0.80d);
                    minkPrice =   RandomPriceGenerator(0.3d , 0.85d);
                    fortname = "Fort Stadacona";
                    suppliesCost = 125.0d;
                    travelExpenses = 15.0d;
                    if (p <= 2)
                    {
                        state.BeaverPelts = 0;
                        Console.WriteLine("Your beaver were to heavy to carry across the portage.");
                        Console.WriteLine("You had to leave the pelts but found them stolen when you returned");
                    }
                    else if (p <= 6)
                    {
                        Console.WriteLine("You arrived safely at Fort Stadacona");
                    }
                    else if (p <= 8)
                    {
                        state.BeaverPelts = 0;
                        state.FoxPelts = 0;
                        state.ErminePelts = 0;
                        state.MinkPelts = 0;
                        Console.WriteLine("Your canoe upset in the Lachine Rapids.");
                        Console.WriteLine("Your lost all your furs");
                    }
                    else if (p <= 10)
                    {
                        state.FoxPelts = 0;
                        Console.WriteLine("Your fox pelts were not cured properly.");
                        Console.WriteLine("No one will buy them.");
                    }
                    else
                    {
                        throw new Exception($"Unexpected Outcome p = {p}");
                    }
                    break;
                case 3:     // outcome for expedition to Fort New York
                    beaverPrice = RandomPriceGenerator(0.2d , 1.00d);
                    foxPrice =    RandomPriceGenerator(0.25d, 1.10d);
                    erminePrice = RandomPriceGenerator(0.2d , 0.95d);
                    minkPrice =   RandomPriceGenerator(0.15d, 1.05d); 
                    fortname = "Fort New York";
                    suppliesCost = 80.0d;
                    travelExpenses = 25.0d;
                    if (p <= 2)
                    {
                        state.BeaverPelts = 0;
                        state.FoxPelts = 0;
                        state.ErminePelts = 0;
                        state.MinkPelts = 0;
                        suppliesCost = 0.0d;
                        travelExpenses = 0.0d;
                        Console.WriteLine("You were attacked by a party of Iroquois.");
                        Console.WriteLine("All people in your trading group were killed.");
                        Console.WriteLine("This ends the game (press any key).");
                        Console.ReadKey(true);
                        state.GameOver = true;
                    }
                    else if (p <= 6)
                    {
                        Console.WriteLine("You were lucky. You arrived safely at Fort New York.");
                    }
                    else if (p <= 8)
                    {
                        state.BeaverPelts = 0;
                        state.FoxPelts = 0;
                        state.ErminePelts = 0;
                        state.MinkPelts = 0;
                        Console.WriteLine("You narrowly escaped an Iroquois raiding party.");
                        Console.WriteLine("However, you had to leave all your furs behind.");
                    }
                    else if (p <= 10)
                    {
                        beaverPrice = beaverPrice / 2;
                        minkPrice = minkPrice / 2;
                        Console.WriteLine("Your mink and beaver were damaged on your trip.");
                        Console.WriteLine("You receive only half the current price for these furs.");
                    }
                    else
                    {
                        throw new Exception($"Unexpected Outcome p = {p}");
                    }
                    break;
                default:
                    break;
            }

            var beaverSale = state.BeaverPelts * beaverPrice;
            var foxSale = state.FoxPelts * foxPrice;
            var ermineSale = state.ErminePelts * erminePrice;
            var minkSale = state.MinkPelts * minkPrice;
            var profit = beaverSale + foxSale + ermineSale + minkSale - suppliesCost - travelExpenses;
            state.Savings += profit;

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Your {state.BeaverPelts.ToString().PadLeft(3, ' ')} beaver pelts sold @ {beaverPrice.ToString("c")} per pelt for a total");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{beaverSale.ToString("c").PadLeft(10)}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Your {state.FoxPelts.ToString().PadLeft(3, ' ')} fox    pelts sold @ {foxPrice.ToString("c")} per pelt for a total");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{foxSale.ToString("c").PadLeft(10)}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Your {state.ErminePelts.ToString().PadLeft(3, ' ')} ermine pelts sold @ {erminePrice.ToString("c")} per pelt for a total");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{ermineSale.ToString("c").PadLeft(10)}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Your {state.MinkPelts.ToString().PadLeft(3, ' ')} mink   pelts sold @ {minkPrice.ToString("c")} per pelt for a total");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{minkSale.ToString("c").PadLeft(10)}");
            Console.WriteLine("");
            DisplayCosts(fortname, suppliesCost, travelExpenses);
            Console.WriteLine("");
            Console.Write($"Profit / Loss".PadLeft(55));
            Console.ForegroundColor = profit >= 0.0d ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"{profit.ToString("c").PadLeft(10)}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
        }

        private void DisplaySelectedFortInformation(int selectedFort)
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;

            switch (selectedFort)
            {
                case 1:    // selected fort details for Fort Hochelaga
                    Console.WriteLine("You have chosen the easiest route.");
                    Console.WriteLine("However, the fort is far from any seaport.");
                    Console.WriteLine("The value you receive for your furs will be low.");
                    Console.WriteLine("The cost of supplies will be higher than at Forts Stadacona or New York");
                    break;
                case 2:    // selected fort details for Fort Stadacona
                    Console.WriteLine("You have chosen a hard route.");
                    Console.WriteLine("It is, in comparsion, harder than the route to Hochelaga but easier than the route to New York.");
                    Console.WriteLine("You will receive an average value for your furs.");
                    Console.WriteLine("The cost of your supplies will be average.");
                    break;
                case 3:    // selected fort details for Fort New York
                    Console.WriteLine("You have chosen the most difficult route.");
                    Console.WriteLine("At Fort New York you will receive the higest value for your furs.");
                    Console.WriteLine("The cost of your supplies will be lower than at all the other forts.");
                    break;
                default:
                    break;
            }
        }

        private bool TradeAtAnotherFort()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("Do you want to trade at another fort?");
            Console.Write("Answer (Y)es or (N)o ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("> ");

            char pressedKey;
            // Keep looping until we get a recognised input
            do
            {
                // Read a key, don't display it on screen
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Convert to upper-case so we don't need to care about capitalisation
                pressedKey = Char.ToUpper(key.KeyChar);
                // Is this a key we recognise? If not, keep looping
            } while (pressedKey != 'Y' && pressedKey != 'N');

            // Display the result on the screen
            Console.WriteLine(pressedKey);

            // Return true if the player pressed 'Y', false for anything else.
            return (pressedKey == 'Y');
        }

        /// <summary>
        /// Get an amount of pelts from the user
        /// </summary>
        /// <param name="currentMoney">The total pelts available</param>
        /// <returns>Returns the amount the player selects</returns>
        private int GetPelts(int furCount)
        {
            int peltCount;
            
            // loop until the user enters a valid value
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("> ");
                string input = Console.ReadLine();

                // parse user information to check if it is a valid entry
                if (!int.TryParse(input, out peltCount))
                {
                    // invalid entry; message back to user
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sorry, I didn't understand. Please enter the number of pelts.");
                    
                    // continue looping
                    continue;
                }

                // check if plet amount is more than the available pelts
                if (peltCount > furCount)
                {
                    // too many pelts selected
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"You may not have that many furs. Do not try to cheat. I can add.");
                    
                    // continue looping
                    continue;
                }

                // valid pelt amount entered
                break;
            } while (true);

            // return pelt count to the user
            return peltCount;
        }

        /// <summary>
        /// Prompt the user for their selected fort
        /// </summary>
        /// <returns>returns the fort the user has selected</returns>
        private int GetSelectedFort()
        {
            int selectedFort;

            // loop until the user enters a valid value
            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Answer 1, 2, or 3 ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("> ");
                string input = Console.ReadLine();

                // is the user entry valid
                if (!int.TryParse(input, out selectedFort))
                {
                    // no, invalid data
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sorry, I didn't understand. Please answer 1, 2, or 3.");
                    
                    // continue looping
                    continue;
                }

                // is the selected fort an option (one, two or three only)
                if (selectedFort != 1 && selectedFort != 2 && selectedFort != 3)
                {
                    // no, invalid for selected
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Please answer 1, 2, or 3.");
                    
                    // continue looping
                    continue;
                }

                // valid fort selected, stop looping
                break;
            } while (true);

            // return the players selected fort
            return selectedFort;
        }
    }
}
