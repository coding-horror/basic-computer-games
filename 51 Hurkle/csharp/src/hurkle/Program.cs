using System;

namespace hurkle
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Original source transscription
            10 PRINT TAB(33);"HURKLE"
            20 PRINT TAB(15);"CREATIVE COMPUTING NORRISTOWN, NEW JERSEY"
            30 PRINT;PRINT;PRINT
            */
            Console.WriteLine(new string(' ', 33) + @"HURKLE");
            Console.WriteLine(new string(' ', 15) + @"CREATIVE COMPUTING NORRISTOWN, NEW JERSEY");
            /*
            110 N=5
            120 G=10
            */
            var N=5;
            var G=10;
            /*
            210 PRINT
            220 PRINT "A HURKLE IS HIDING ON A";G;"BY";G;"GRID. HOMEBASE"
            230 PRINT "ON THE GRID IS POINT 0,0 AND ANY GRIDPOINT IS A"
            240 PRINT "PAIR OF WHOLE NUMBERS SEPERATED BY A COMMA. TRY TO"
            250 PRINT "GUESS THE HURKLE'S GRIDPOINT. YOU GET";N;"TRIES."
            260 PRINT "AFTER EACH TRY, I WILL TELL YOU THE APPROXIMATE"
            270 PRINT "DIRECTION TO GO TO LOOK FOR THE HURKLE."
            280 PRINT
            */
            // Using string formatting via the '$' string
            Console.WriteLine();
            Console.WriteLine($"A HURKLE IS HIDING ON A {G} BY {G} GRID. HOMEBASE");
            Console.WriteLine(@"ON THE GRID IS POINT 0,0 AND ANY GRIDPOINT IS A");
            Console.WriteLine(@"PAIR OF WHOLE NUMBERS SEPERATED BY A COMMA. TRY TO");
            Console.WriteLine($"GUESS THE HURKLE'S GRIDPOINT. YOU GET {N} TRIES.");
            Console.WriteLine(@"AFTER EACH TRY, I WILL TELL YOU THE APPROXIMATE");
            Console.WriteLine(@"DIRECTION TO GO TO LOOK FOR THE HURKLE.");
            Console.WriteLine();
            
            /*
            285 A=INT(G*RND(1))
            286 B=INT(G*RND(1))
            */
            // Get a pseudo number generator
            var randomSource = new Random();
            START:
            // BASIC program was generating a float between 0 and 1
            // then multiplying by the size of the grid to to a number
            // between 1 and 10. C# allows you to do that directly.
            var A = randomSource.Next(0,G);
            var B = randomSource.Next(0,G);
            
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
            for(var K=1;K<=N;K++)
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
                    goto END;
                }

                PrintInfo(X,Y,A,B);
            }
            
            /*
            410 PRINT
            420 PRINT "SORRY, THAT'S;N;"GUESSES."
            430 PRINT "THE HURKLE IS AT ";A;",";B
            */
            Console.WriteLine();
            Console.WriteLine($"SORRY, THAT'S {N} GUESSES");
            Console.WriteLine($"THE HURKLE IS AT {A},{B}");
            /*
            440 PRINT
            450 PRINT "LET'S PLAY AGAIN. HURKLE IS HIDING."
            460 PRINT
            470 GOTO 285
            */
            END:
            Console.WriteLine();
            Console.WriteLine("LET'S PLAY AGAIN. HURKLE IS HIDING");
            Console.WriteLine();
            goto START;
            
            /*
            999 END
            */
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
