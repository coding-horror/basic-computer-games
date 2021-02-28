using System;

namespace hurkle
{
    public class HurkleGame
    {
        private readonly Random _random = new Random();
        private readonly int guesses;
        private readonly int gridSize;

        public HurkleGame(int guesses, int gridSize)
        {
            this.guesses = guesses;
            this.gridSize = gridSize;
        }

        public void PlayGame()
        {
            // BASIC program was generating a float between 0 and 1
            // then multiplying by the size of the grid to to a number
            // between 1 and 10. C# allows you to do that directly.
            var A = _random.Next(0,gridSize);
            var B = _random.Next(0,gridSize);
            
            /*
            310 FOR K=1 TO N
            320 PRINT "GUESS #";K;
            330 INPUT X,Y
            340 IF ABS(X-A)+ABS(Y-B)=0 THEN 500
            350 REM PRINT INFO
            360 GOSUB 610
            370 PRINT
            380 NEXT K
            */
            for(var K=1;K<=guesses;K++)
            {
                Console.WriteLine($"GUESS #{K}");
                var inputLine = Console.ReadLine();
                var seperateStrings = inputLine.Split(',', 2, StringSplitOptions.TrimEntries);
                var X = int.Parse(seperateStrings[0]);
                var Y = int.Parse(seperateStrings[1]);
                if(Math.Abs(X-A) + Math.Abs(Y-B) == 0)
                {
                    /*
                    500 REM
                    510 PRINT
                    520 PRINT "YOU FOUND HIM IN";K;GUESSES!"
                    540 GOTO 440
                    */
                    Console.WriteLine();
                    Console.WriteLine($"YOU FOUND HIM IN {K} GUESSES!");
                    return;
                }

                PrintInfo(X,Y,A,B);
            }
            
            /*
            410 PRINT
            420 PRINT "SORRY, THAT'S;N;"GUESSES."
            430 PRINT "THE HURKLE IS AT ";A;",";B
            */
            Console.WriteLine();
            Console.WriteLine($"SORRY, THAT'S {guesses} GUESSES");
            Console.WriteLine($"THE HURKLE IS AT {A},{B}");
        }

        private static void PrintInfo(int X, int Y, int A, int B)
        {
            
            /*
            610 PRINT "GO ";
            */
            Console.Write("GO ");
            /*
            620 IF Y=B THEN 670
            630 IF Y<B THEN 660
            640 PRINT "SOUTH";
            650 GO TO 670
            660 PRINT "NORTH";
            */
            if(Y>B)
            {
                Console.Write("SOUTH");
            }else if(Y<B)
            {
                Console.Write("NORTH");
            }
            /*
            670 IF X=A THEN 720
            680 IF X<A THEN 710
            690 PRINT "WEST";
            700 GO TO 720
            710 PRINT "EAST";
            */
            if(X<A)
            {
                Console.Write("EAST");
            }else if(X>A)
            {
                Console.Write("WEST");
            }

            Console.WriteLine();
            /*
            720 PRINT
            730 RETURN
            */
        }
    }
}