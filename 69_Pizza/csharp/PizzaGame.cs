namespace Pizza
{
    internal class PizzaGame
    {
        private const int CustomerMapSize = 4;
        private readonly CustomerMap _customerMap = new CustomerMap(CustomerMapSize);

        /// <summary>
        /// Starts game. Main coordinator for pizza game.
        /// It is responsible for showing information, getting data from user and starting to delivery pizza.
        /// </summary>
        public void Play()
        {
            ShowHeader();

            string playerName = GetPlayerName();

            ShowIntroduction(playerName);
            ShowMap();

            if (AskForMoreDirections())
            {
                ShowMoreDirections(playerName);

                var playerUnderstands = AskIfPlayerUnderstand();
                if (!playerUnderstands)
                {
                    return;
                }
            }

            StartDelivery(playerName);
            EndDelivery(playerName);
        }

        /// <summary>
        /// Starts with pizza delivering to customers.
        /// Every 5 deliveries it is asking user whether want to continue in delivering.
        /// </summary>
        /// <param name="playerName">Player name which was filled by user.</param>
        private void StartDelivery(string playerName)
        {
            var numberOfDeliveredPizzas = 0;
            while (true)
            {
                numberOfDeliveredPizzas++;
                string deliverPizzaToCustomer = GetRandomCustomer();

                WriteEmptyLine();
                Console.WriteLine($"HELLO {playerName}'S PIZZA.  THIS IS {deliverPizzaToCustomer}.");
                Console.WriteLine("\tPLEASE SEND A PIZZA.");

                DeliverPizzaByPlayer(playerName, deliverPizzaToCustomer);

                if (numberOfDeliveredPizzas % 5 == 0)
                {
                    bool playerWantToDeliveryMorePizzas = AskQuestionWithYesNoResponse("DO YOU WANT TO DELIVER MORE PIZZAS?");
                    if (!playerWantToDeliveryMorePizzas)
                    {
                        WriteEmptyLine();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets random customer for which pizza should be delivered.
        /// </summary>
        /// <returns>Customer name.</returns>
        private string GetRandomCustomer()
        {
            int randomPositionOnX = Random.Shared.Next(0, CustomerMapSize);
            int randomPositionOnY = Random.Shared.Next(0, CustomerMapSize);

            return _customerMap.GetCustomerOnPosition(randomPositionOnX, randomPositionOnY);
        }

        /// <summary>
        /// Delivers pizza to customer by player. It verifies whether player was delivering pizza to correct customer.
        /// </summary>
        /// <param name="playerName">Player name which was filled by user.</param>
        /// <param name="deliverPizzaToCustomer">Customer name which order pizza.</param>
        private void DeliverPizzaByPlayer(string playerName, string deliverPizzaToCustomer)
        {
            while (true)
            {
                string userInput = GetPlayerInput($"\tDRIVER TO {playerName}:  WHERE DOES {deliverPizzaToCustomer} LIVE?");
                var deliveredToCustomer = GetCustomerFromPlayerInput(userInput);
                if (string.IsNullOrEmpty(deliveredToCustomer))
                {
                    deliveredToCustomer = "UNKNOWN CUSTOMER";
                }

                if (deliveredToCustomer.Equals(deliverPizzaToCustomer))
                {
                    Console.WriteLine($"HELLO {playerName}.  THIS IS {deliverPizzaToCustomer}, THANKS FOR THE PIZZA.");
                    break;
                }

                Console.WriteLine($"THIS IS {deliveredToCustomer}.  I DID NOT ORDER A PIZZA.");
                Console.WriteLine($"I LIVE AT {userInput}");
            }
        }

        /// <summary>
        /// Gets customer name by user input with customer coordinations.
        /// </summary>
        /// <param name="userInput">Input from users - it should represent customer coordination separated by ','.</param>
        /// <returns>If coordinations are correct and customer exists then returns true otherwise false.</returns>
        private string GetCustomerFromPlayerInput(string userInput)
        {
            var pizzaIsDeliveredToPosition = userInput?
                .Split(',')
                .Select(i => int.TryParse(i, out var customerPosition) ? (customerPosition - 1) : -1)
                .Where(i => i != -1)
                .ToArray() ?? Array.Empty<int>();
            if (pizzaIsDeliveredToPosition.Length != 2)
            {
                return string.Empty;
            }

            return _customerMap.GetCustomerOnPosition(pizzaIsDeliveredToPosition[0], pizzaIsDeliveredToPosition[1]);
        }

        /// <summary>
        /// Shows game header in console.
        /// </summary>
        private void ShowHeader()
        {
            Console.WriteLine("PIZZA".PadLeft(22));
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            WriteEmptyLine(3);
            Console.WriteLine("PIZZA DELIVERY GAME");
            WriteEmptyLine();
        }

        /// <summary>
        /// Asks user for name which will be used in game.
        /// </summary>
        /// <returns>Player name.</returns>
        private string GetPlayerName()
        {
            return GetPlayerInput("WHAT IS YOUR FIRST NAME:");
        }

        /// <summary>
        /// Shows game introduction in console
        /// </summary>
        /// <param name="playerName">Player name which was filled by user.</param>
        private void ShowIntroduction(string playerName)
        {
            Console.WriteLine($"HI, {playerName}.  IN THIS GAME YOU ARE TO TAKE ORDERS");
            Console.WriteLine("FOR PIZZAS.  THEN YOU ARE TO TELL A DELIVERY BOY");
            Console.WriteLine("WHERE TO DELIVER THE ORDERED PIZZAS.");
            WriteEmptyLine(2);
        }

        /// <summary>
        /// Shows customers map in console. In this method is used overridden method 'ToString' for getting text representation of customers map.
        /// </summary>
        private void ShowMap()
        {
            Console.WriteLine("MAP OF THE CITY OF HYATTSVILLE");
            WriteEmptyLine();

            Console.WriteLine(_customerMap.ToString());

            Console.WriteLine("THE OUTPUT IS A MAP OF THE HOMES WHERE");
            Console.WriteLine("YOU ARE TO SEND PIZZAS.");
            WriteEmptyLine();
            Console.WriteLine("YOUR JOB IS TO GIVE A TRUCK DRIVER");
            Console.WriteLine("THE LOCATION OR COORDINATES OF THE");
            Console.WriteLine("HOME ORDERING THE PIZZA.");
            WriteEmptyLine();
        }

        /// <summary>
        /// Asks user if needs more directions.
        /// </summary>
        /// <returns>True if user need more directions otherwise false.</returns>
        private bool AskForMoreDirections()
        {
            var playerNeedsMoreDirections = AskQuestionWithYesNoResponse("DO YOU NEED MORE DIRECTIONS?");
            WriteEmptyLine();

            return playerNeedsMoreDirections;
        }

        /// <summary>
        /// Shows more directions.
        /// </summary>
        /// <param name="playerName">Player name which was filled by user.</param>
        private void ShowMoreDirections(string playerName)
        {
            Console.WriteLine("SOMEBODY WILL ASK FOR A PIZZA TO BE");
            Console.WriteLine("DELIVERED.  THEN A DELIVERY BOY WILL");
            Console.WriteLine("ASK YOU FOR THE LOCATION.");
            Console.WriteLine("\tEXAMPLE:");
            Console.WriteLine("THIS IS J.  PLEASE SEND A PIZZA.");
            Console.WriteLine($"DRIVER TO {playerName}.  WHERE DOES J LIVE?");
            Console.WriteLine("YOUR ANSWER WOULD BE 2,3");
        }

        /// <summary>
        /// Asks user if understands to instructions.
        /// </summary>
        /// <returns>True if user understand otherwise false.</returns>
        private bool AskIfPlayerUnderstand()
        {
            var playerUnderstands = AskQuestionWithYesNoResponse("UNDERSTAND?");
            if (!playerUnderstands)
            {
                Console.WriteLine("THIS JOB IS DEFINITELY TOO DIFFICULT FOR YOU. THANKS ANYWAY");
                return false;
            }

            WriteEmptyLine();
            Console.WriteLine("GOOD.  YOU ARE NOW READY TO START TAKING ORDERS.");
            WriteEmptyLine();
            Console.WriteLine("GOOD LUCK!!");
            WriteEmptyLine();

            return true;
        }

        /// <summary>
        /// Shows message about ending delivery in console.
        /// </summary>
        /// <param name="playerName">Player name which was filled by user.</param>
        private void EndDelivery(string playerName)
        {
            Console.WriteLine($"O.K. {playerName}, SEE YOU LATER!");
            WriteEmptyLine();
        }

        /// <summary>
        /// Gets input from user.
        /// </summary>
        /// <param name="question">Question which is displayed in console.</param>
        /// <returns>User input.</returns>
        private string GetPlayerInput(string question)
        {
            Console.Write($"{question} ");

            while (true)
            {
                var userInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(userInput))
                {
                    return userInput;
                }
            }
        }

        /// <summary>
        /// Asks user with required resposne 'YES', 'Y, 'NO', 'N'.
        /// </summary>
        /// <param name="question">Question which is displayed in console.</param>
        /// <returns>True if user write 'YES', 'Y'. False if user write 'NO', 'N'.</returns>
        private static bool AskQuestionWithYesNoResponse(string question)
        {
            var possitiveResponse = new string[] { "Y", "YES" };
            var negativeResponse = new string[] { "N", "NO" };
            var validUserInputs = possitiveResponse.Concat(negativeResponse);

            Console.Write($"{question} ");

            string? userInput;
            while (true)
            {
                userInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(userInput) && validUserInputs.Contains(userInput.ToUpper()))
                {
                    break;
                }

                Console.Write($"'YES' OR 'NO' PLEASE, NOW THEN, {question} ");
            }

            return possitiveResponse.Contains(userInput.ToUpper());
        }

        /// <summary>
        /// Writes empty line in console.
        /// </summary>
        /// <param name="numberOfEmptyLines">Number of empty lines which will be written in console. Parameter is optional and default value is 1.</param>
        private void WriteEmptyLine(int numberOfEmptyLines = 1)
        {
            for (int i = 0; i < numberOfEmptyLines; i++)
            {
                Console.WriteLine();
            }
        }
    }
}