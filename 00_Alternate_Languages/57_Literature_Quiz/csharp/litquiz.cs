using System;

namespace litquiz
{
    class litquiz
    {
        public static int Score = 0;


        public static void Main(string[] args)
        {

            //Print the title and intro

            Console.WriteLine("                         LITERATURE QUIZ");
            Console.WriteLine("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("TEST YOUR KNOWLEDGE OF CHILDREN'S LITERATURE");
            Console.WriteLine();
            Console.WriteLine("THIS IS A MULTIPLE-CHOICE QUIZ");
            Console.WriteLine("TYPE A 1, 2, 3, OR 4 AFTER THE QUESTION MARK.");
            Console.WriteLine();
            Console.WriteLine("GOOD LUCK!");
            Console.WriteLine();
            Console.WriteLine();
            One();



        }

        public static void One() {
            Console.WriteLine("IN PINOCCHIO, WHAT WAS THE NAME OF THE CAT");
            Console.WriteLine("1)TIGGER, 2)CICERO, 3)FIGARO, 4)GUIPETTO");

            string answerOne;
            answerOne = Console.ReadLine();

            if(answerOne == "4")
            {
                Console.WriteLine("VERY GOOD! HERE'S ANOTHER.");
                Score = Score + 1;
                Two();
            }
            else
            {
                Console.WriteLine("SORRY...FIGARO WAS HIS NAME.");
                Two();
            }

        }

        public static void Two()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("FROM WHOSE GARDEN DID BUGS BUNNY STEAL THE CARROTS?");
            Console.WriteLine("1)MR. NIXON'S, 2)ELMER FUDD'S, 3)CLEM JUDD'S, 4)STROMBOLI'S");

            string answerTwo;
            answerTwo = Console.ReadLine();

            if(answerTwo == "2")
            {
                Console.WriteLine("PRETTY GOOD!");
                Score = Score + 1;
                Three();
            }
            else
            {
                Console.WriteLine("TOO BAD...IT WAS ELMER FUDD'S GARDEN.");
                Three();
            }
        }

        public static void Three()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("IN THE WIZARD OF OS, DOROTHY'S DOG WAS NAMED");
            Console.WriteLine("1)CICERO, 2)TRIXIA, 3)KING, 4)TOTO");

            string answerThree;
            answerThree = Console.ReadLine();

            if(answerThree == "4")
            {
                Console.WriteLine("YEA!  YOU'RE A REAL LITERATURE GIANT.");
                Score = Score + 1;
                Four();
            }
            else
            {
                Console.WriteLine("BACK TO THE BOOKS,...TOTO WAS HIS NAME.");
                Four();
            }




        }

        public static void Four()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("WHO WAS THE FAIR MAIDEN WHO ATE THE POISON APPLE");
            Console.WriteLine("1)SLEEPING BEAUTY, 2)CINDERELLA, 3)SNOW WHITE, 4)WENDY");

            string answerFour;
            answerFour = Console.ReadLine();

            if(answerFour == "3")
            {
                Console.WriteLine("GOOD MEMORY!");
                Score = Score + 1;
                End();
            }
            else
            {
                Console.WriteLine("OH, COME ON NOW...IT WAS SNOW WHITE.");
                End();
            }

        }

        public static void End()
        {
            Console.WriteLine();
            Console.WriteLine();
            if(Score == 4)
            {
                Console.WriteLine("WOW!  THAT'S SUPER!  YOU REALLY KNOW YOUR NURSERY");
                Console.WriteLine("YOUR NEXT QUIZ WILL BE ON 2ND CENTURY CHINESE");
                Console.WriteLine("LITERATURE (HA, HA, HA)");
                return;
            }
            else if(Score < 2)
            {
                Console.WriteLine("UGH.  THAT WAS DEFINITELY NOT TOO SWIFT.  BACK TO");
                Console.WriteLine("NURSERY SCHOOL FOR YOU, MY FRIEND.");
                return;
            }
            else
            {
                Console.WriteLine("NOT BAD, BUT YOU MIGHT SPEND A LITTLE MORE TIME");
                Console.WriteLine("READING THE NURSERY GREATS.");
                return;
            }
        }

	}
}
