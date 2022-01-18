using System;

namespace Hammurabi
{
    /// <summary>
    /// Provides various methods for presenting information to the user.
    /// </summary>
    public static class View
    {
        /// <summary>
        /// Shows the introductory banner to the player.
        /// </summary>
        public static void ShowBanner()
        {
            Console.WriteLine("                                HAMURABI");
            Console.WriteLine("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("TRY YOUR HAND AT GOVERNING ANCIENT SUMERIA");
            Console.WriteLine("FOR A TEN-YEAR TERM OF OFFICE.");
        }

        /// <summary>
        /// Shows a summary of the current state of the city.
        /// </summary>
        public static void ShowCitySummary(GameState state)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("HAMURABI:  I BEG TO REPORT TO YOU,");
            Console.WriteLine($"IN YEAR {state.Year}, {state.Starvation} PEOPLE STARVED, {state.PopulationIncrease} CAME TO THE CITY,");

            if (state.IsPlagueYear)
            {
                Console.WriteLine("A HORRIBLE PLAGUE STRUCK!  HALF THE PEOPLE DIED.");
            }

            Console.WriteLine($"POPULATION IS NOW {state.Population}");
            Console.WriteLine($"THE CITY NOW OWNS {state.Acres} ACRES.");
            Console.WriteLine($"YOU HARVESTED {state.Productivity} BUSHELS PER ACRE.");
            Console.WriteLine($"THE RATS ATE {state.Spoilage} BUSHELS.");
            Console.WriteLine($"YOU NOW HAVE {state.Stores} BUSHELS IN STORE.");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows the current cost of land.
        /// </summary>
        /// <param name="state"></param>
        public static void ShowLandPrice(GameState state)
        {
            Console.WriteLine ($"LAND IS TRADING AT {state.LandPrice} BUSHELS PER ACRE.");
        }

        /// <summary>
        /// Displays a section separator.
        /// </summary>
        public static void ShowSeparator()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Inform the player that he or she has entered an invalid number.
        /// </summary>
        public static void ShowInvalidNumber()
        {
            Console.WriteLine("PLEASE ENTER A VALID NUMBER");
        }

        /// <summary>
        /// Inform the player that he or she has insufficient acreage.
        /// </summary>
        public static void ShowInsufficientLand(GameState state)
        {
            Console.WriteLine($"HAMURABI:  THINK AGAIN.  YOU OWN ONLY {state.Acres} ACRES.  NOW THEN,");
        }

        /// <summary>
        /// Inform the player that he or she has insufficient population.
        /// </summary>
        public static void ShowInsufficientPopulation(GameState state)
        {
            Console.WriteLine($"BUT YOU HAVE ONLY {state.Population} PEOPLE TO TEND THE FIELDS!  NOW THEN,");
        }

        /// <summary>
        /// Inform the player that he or she has insufficient grain stores.
        /// </summary>
        public static void ShowInsufficientStores(GameState state)
        {
            Console.WriteLine("HAMURABI:  THINK AGAIN.  YOU HAVE ONLY");
            Console.WriteLine($"{state.Stores} BUSHELS OF GRAIN.  NOW THEN,");
        }

        /// <summary>
        /// Show the player that he or she has caused great offence.
        /// </summary>
        public static void ShowGreatOffence()
        {
            Console.WriteLine();
            Console.WriteLine("HAMURABI:  I CANNOT DO WHAT YOU WISH.");
            Console.WriteLine("GET YOURSELF ANOTHER STEWARD!!!!!");
        }

        /// <summary>
        /// Shows the game's final result to the user.
        /// </summary>
        public static void ShowGameResult(GameResult result)
        {
            if (!result.WasPlayerImpeached)
            {
                Console.WriteLine($"IN YOUR 10-YEAR TERM OF OFFICE, {result.AverageStarvationRate} PERCENT OF THE");
                Console.WriteLine("POPULATION STARVED PER YEAR ON THE AVERAGE, I.E. A TOTAL OF");
                Console.WriteLine($"{result.TotalStarvation} PEOPLE DIED!!");

                Console.WriteLine("YOU STARTED WITH 10 ACRES PER PERSON AND ENDED WITH");
                Console.WriteLine($"{result.AcresPerPerson} ACRES PER PERSON.");
                Console.WriteLine();
            }

            switch (result.Rating)
            {
                case PerformanceRating.Disgraceful:
                    if (result.WasPlayerImpeached)
                        Console.WriteLine($"YOU STARVED {result.FinalStarvation} PEOPLE IN ONE YEAR!!!");

                    Console.WriteLine("DUE TO THIS EXTREME MISMANAGEMENT YOU HAVE NOT ONLY");
                    Console.WriteLine("BEEN IMPEACHED AND THROWN OUT OF OFFICE BUT YOU HAVE");
                    Console.WriteLine("ALSO BEEN DECLARED NATIONAL FINK!!!!");
                    break;
                case PerformanceRating.Bad:
                    Console.WriteLine("YOUR HEAVY-HANDED PERFORMANCE SMACKS OF NERO AND IVAN IV.");
                    Console.WriteLine("THE PEOPLE (REMIANING) FIND YOU AN UNPLEASANT RULER, AND,");
                    Console.WriteLine("FRANKLY, HATE YOUR GUTS!!");
                    break;
                case PerformanceRating.Ok:
                    Console.WriteLine("YOUR PERFORMANCE COULD HAVE BEEN SOMEWHAT BETTER, BUT");
                    Console.WriteLine($"REALLY WASN'T TOO BAD AT ALL. {result.Assassins} PEOPLE");
                    Console.WriteLine("WOULD DEARLY LIKE TO SEE YOU ASSASSINATED BUT WE ALL HAVE OUR");
                    Console.WriteLine("TRIVIAL PROBLEMS.");
                    break;
                case PerformanceRating.Terrific:
                    Console.WriteLine("A FANTASTIC PERFORMANCE!!!  CHARLEMANGE, DISRAELI, AND");
                    Console.WriteLine("JEFFERSON COMBINED COULD NOT HAVE DONE BETTER!");
                    break;
            }
        }

        /// <summary>
        /// Shows a farewell message to the user.
        /// </summary>
        public static void ShowFarewell()
        {
            Console.WriteLine("SO LONG FOR NOW.");
            Console.WriteLine();
        }

        /// <summary>
        /// Prompts the user to buy land.
        /// </summary>
        public static void PromptBuyLand()
        {
            Console.Write("HOW MANY ACRES DO YOU WISH TO BUY? ");
        }

        /// <summary>
        /// Prompts the user to sell land.
        /// </summary>
        public static void PromptSellLand()
        {
            Console.Write("HOW MANY ACRES DO YOU WISH TO SELL? ");
        }

        /// <summary>
        /// Prompts the user to feed the people.
        /// </summary>
        public static void PromptFeedPeople()
        {
            Console.Write("HOW MANY BUSHELS DO YOU WISH TO FEED YOUR PEOPLE? ");
        }

        /// <summary>
        /// Prompts the user to plant crops.
        /// </summary>
        public static void PromptPlantCrops()
        {
            Console.Write("HOW MANY ACRES DO YOU WISH TO PLANT WITH SEED? ");
        }
    }
}
