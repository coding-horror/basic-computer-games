namespace Bullseye
{
    /// <summary>
    /// Class encompassing the game
    /// </summary>
    public class BullseyeGame
    {
        private readonly List<Player> _players;

        // define a constant for the winning score so that it is
        // easy to change again in the future
        private const int WinningScore = 200;

        public BullseyeGame()
        {
            // create the initial list of players; list is empty, but
            // the setup of the game will add items to this list
            _players = new List<Player>();
        }

        public void Run()
        {
            PrintIntroduction();

            SetupGame();

            PlayGame();

            PrintResults();
        }

        private void SetupGame()
        {
            // First, allow the user to enter how many players are going
            // to play. This could be weird if the user enters negative
            // numbers, words, or too many players, so there are some
            // extra checks on the input to make sure the user didn't do
            // anything too crazy. Loop until the user enters valid input.
            bool validPlayerCount;
            int playerCount;
            do
            {
                Console.WriteLine();
                Console.Write("HOW MANY PLAYERS? ");
                string? input = Console.ReadLine();

                // assume the user has entered something incorrect - the
                // next steps will validate the input
                validPlayerCount = false;

                if (Int32.TryParse(input, out playerCount))
                {
                    if (playerCount > 0 && playerCount <= 20)
                    {
                        validPlayerCount = true;
                    }
                    else
                    {
                        Console.WriteLine("YOU MUST ENTER A NUMBER BETWEEN 1 AND 20!");
                    }
                }
                else
                {
                    Console.WriteLine("YOU MUST ENTER A NUMBER");
                }

            }
            while (!validPlayerCount);

            // Next, allow the user to enter names for the players; as each
            // name is entered, create a Player object to track the name
            // and their score, and save the object to the list in this class
            // so the rest of the game has access to the set of players
            for (int i = 0; i < playerCount; i++)
            {
                string? playerName = String.Empty;
                do
                {
                    Console.Write($"NAME OF PLAYER #{i+1}? ");
                    playerName = Console.ReadLine();

                    // names can be any sort of text, so allow whatever the user
                    // enters as long as it isn't a blank space
                }
                while (String.IsNullOrWhiteSpace(playerName));

                _players.Add(new Player(playerName));
            }
        }

        private void PlayGame()
        {
            Random random = new Random(DateTime.Now.Millisecond);

            int round = 0;
            bool isOver = false;
            do
            {
                // starting a new round, increment the counter
                round++;
                Console.WriteLine($"ROUND {round}");
                Console.WriteLine("--------------");

                foreach (Player player in _players)
                {
                    // ask the user how they want to throw
                    Console.Write($"{player.Name.ToUpper()}'S THROW: ");
                    string? input = Console.ReadLine();

                    // based on the input, figure out the probabilities
                    int[] probabilities;
                    switch (input)
                    {
                        case "1":
                        {
                            probabilities = new int[] { 65, 55, 50, 50 };
                            break;
                        }
                        case "2":
                        {
                            probabilities = new int[] { 99, 77, 43, 1 };
                            break;
                        }
                        case "3":
                        {
                            probabilities = new int[] { 95, 75, 45, 5 };
                            break;
                        }
                        default:
                        {
                            // in case the user types something bad, pretend it's
                            // as if they tripped over themselves while throwing
                            // the dart - they'll either hit a bullseye or completely
                            // miss
                            probabilities = new int[] { 95, 95, 95, 95 };
                            Console.Write("TRIP! ");
                            break;
                        }
                    }


                    // Next() returns a number in the range: min <= num < max, so specify 101
                    // as the maximum so that 100 is a number that could be returned
                    int chance = random.Next(0, 101);

                    if (chance > probabilities[0])
                    {
                        player.Score += 40;
                        Console.WriteLine("BULLSEYE!!  40 POINTS!");
                    }
                    else if (chance > probabilities[1])
                    {
                        player.Score += 30;
                        Console.WriteLine("30-POINT ZONE!");
                    }
                    else if (chance > probabilities[2])
                    {
                        player.Score += 20;
                        Console.WriteLine("20-POINT ZONE");
                    }
                    else if (chance > probabilities[3])
                    {
                        player.Score += 10;
                        Console.WriteLine("WHEW!  10 POINTS.");
                    }
                    else
                    {
                        // missed it
                        Console.WriteLine("MISSED THE TARGET!  TOO BAD.");
                    }

                    // check to see if the player has won - if they have, then
                    // break out of the loops
                    if (player.Score > WinningScore)
                    {
                        Console.WriteLine();
                        Console.WriteLine("WE HAVE A WINNER!!");
                        Console.WriteLine($"{player.Name.ToUpper()} SCORED {player.Score} POINTS.");
                        Console.WriteLine();

                        isOver = true; // out of the do/while round loop
                        break; // out of the foreach (player) loop
                    }

                    Console.WriteLine();
                }
            }
            while (!isOver);
        }

        private void PrintResults()
        {
            // For bragging rights, print out all the scores, but sort them
            // by who had the highest score
            var sorted = _players.OrderByDescending(p => p.Score);

            // padding is used to get things to line up nicely - the results
            // should look something like:
            //      PLAYER       SCORE
            //      Bravo          210
            //      Charlie         15
            //      Alpha            1
            Console.WriteLine("PLAYER       SCORE");
            foreach (var player in sorted)
            {
                Console.WriteLine($"{player.Name.PadRight(12)} {player.Score.ToString().PadLeft(5)}");
            }

            Console.WriteLine();
            Console.WriteLine("THANKS FOR THE GAME.");
        }

        private void PrintIntroduction()
        {
            Console.WriteLine(Title);
            Console.WriteLine();
            Console.WriteLine(Introduction);
            Console.WriteLine();
            Console.WriteLine(Operations);
        }

        private const string Title = @"
                    BULLSEYE
    CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY";

        private const string Introduction = @"
IN THIS GAME, UP TO 20 PLAYERS THROW DARTS AT A TARGET
WITH 10, 20, 30, AND 40 POINT ZONES.  THE OBJECTIVE IS
TO GET 200 POINTS.";

        private const string Operations = @"
THROW   DESCRIPTION         PROBABLE SCORE
  1     FAST OVERARM        BULLSEYE OR COMPLETE MISS
  2     CONTROLLED OVERARM  10, 20, OR 30 POINTS
  3     UNDERARM            ANYTHING";
    }
}
