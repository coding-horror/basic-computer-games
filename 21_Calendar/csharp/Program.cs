using System;

/*
 21_Calendar in C# for basic-computer-games
 Converted by luminoso-256
*/

namespace _21_calendar
{
    class Program
    {
        //basic has a TAB function. We do not by default, so we make our own!
        static string Tab(int numspaces)
        {
            string space = "";
            //loop as many times as there are spaces specified, and add a space each time
            while (numspaces > 0)
            {
                //add the space
                space += " ";
                //decrement the loop variable so we don't keep going forever!
                numspaces--;
            }
            return space;
        }

        static void Main(string[] args)
        {
            // print the "title" of our program
            // the usage of Write*Line* means we do not have to specify a newline (\n)
            Console.WriteLine(Tab(32) + "CALENDAR");
            Console.WriteLine(Tab(15) + "CREATE COMPUTING  MORRISTOWN, NEW JERSEY");
            //give us some space. 
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");

            //establish some variables needed to print out a calculator

            //the length of each month in days. On a leap year, the start of this would be 
            // 0, 31, 29 to account for Feb. the 0 at the start is for days elapsed to work right in Jan.
            int[] monthLengths = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}; // m in original source

            //the starting day of the month. in 1979 this was monday
            // 0 = sun, -1 = mon, -2 = tue, -3 = wed, etc.
            int day = -1; // called d in original source

            //how much time in the year has gone by?
            int elapsed = 0; // called s in original source

            //loop through printing all the months.
            for (int month = 1; month <= 12; month++) //month is called n in original source
            {
                //pad some space
                Console.WriteLine("");
                Console.WriteLine("");
                //increment days elapsed
                elapsed += monthLengths[month - 1];
                //build our header for this month of the calendar
                string header = "** " + elapsed;
                //add padding as needed
                while (header.Length < 7)
                {
                    header += " ";
                }
                for (int i = 1; i <= 18; i++)
                {
                    header += "*";
                }
                //determine what month it is, add text accordingly
                switch (month) {
                    case 1: header += " JANUARY "; break;
                    case 2: header += " FEBRUARY"; break;
                    case 3: header += "  MARCH  "; break;
                    case 4: header += "  APRIL  "; break;
                    case 5: header += "   MAY   "; break;
                    case 6: header += "   JUNE  "; break;
                    case 7: header += "   JULY  "; break;
                    case 8: header += "  AUGUST "; break;
                    case 9: header += "SEPTEMBER"; break;
                    case 10: header += " OCTOBER "; break;
                    case 11: header += " NOVEMBER"; break;
                    case 12: header += " DECEMBER"; break;
                }
                //more padding
                for (int i = 1; i <= 18; i++)
                {
                    header += "*";
                }
                header += "  ";
                // how many days left till the year's over?
                header += (365 - elapsed) + " **"; // on leap years 366 
                Console.WriteLine(header);
                //dates 
                Console.WriteLine("     S       M       T       W       T       F       S");
                Console.WriteLine(" ");

                string weekOutput = "";
                for (int i = 1; i <= 59; i++)
                {
                    weekOutput += "*";
                }
                //init some vars ahead of time
                int g = 0;
                int d2 = 0;
                //go through the weeks and days
                for (int week = 1; week <= 6; week++)
                {
                    Console.WriteLine(weekOutput);
                    weekOutput = "    "; 
                    for (g = 1; g <= 7; g++)
                    {
                        //add one to the day
                        day++;
                        d2 = day - elapsed;
                        //check if we're done with this month
                        if (d2 > monthLengths[month])
                        {
                            week = 6;
                            break;
                        }
                        //should we print this day?
                        if (d2 > 0)
                        {
                            weekOutput += d2;
                        }
                        //padding
                        while (weekOutput.Length < 4 + 8 * g)
                        {
                            weekOutput += " ";
                        }
                    }
                    if (d2 == monthLengths[month])
                    {
                        day += g;
                        break;
                    }
                }
                day -= g;
                Console.WriteLine(weekOutput);
            }
        }
    }
}
