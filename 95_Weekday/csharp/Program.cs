using System.Text;

namespace Weekday
{
    class Weekday
    {
        private void DisplayIntro()
        {
            Console.WriteLine("");
            Console.WriteLine("SYNONYM".PadLeft(23));
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine("");
            Console.WriteLine("Weekday is a computer demonstration that");
            Console.WriteLine("gives facts about a date of interest to you.");
            Console.WriteLine("");
        }

        private bool ValidateDate(string InputDate, out DateTime ReturnDate)
        {
            // The expectation is that the input is in the format D,M,Y
            // but any valid date format (other than with commas) will work
            string DateString = InputDate.Replace(",", "/");

            return (DateTime.TryParse(DateString, out ReturnDate));
        }

        private DateTime PromptForADate(string Prompt)
        {
            bool Success = false;
            string LineInput = String.Empty;
            DateTime TodaysDate = DateTime.MinValue;

            // Get the date for input and validate it
            while (!Success)
            {
                Console.Write(Prompt);
                LineInput = Console.ReadLine().Trim().ToLower();

                Success = ValidateDate(LineInput, out TodaysDate);

                if (!Success)
                {
                    Console.WriteLine("*** Invalid date.  Please try again.");
                    Console.WriteLine("");
                }
            }

            return TodaysDate;
        }

        private void CalculateDateDiff(DateTime TodaysDate, DateTime BirthDate, Double Factor, out int AgeInYears, out int AgeInMonths, out int AgeInDays)
        {
            // leveraged Stack Overflow answer: https://stackoverflow.com/a/3055445

            // Convert to number of days since Birth Date, multiple by factor then store as new FactorDate
            TimeSpan TimeDiff = TodaysDate.Subtract(BirthDate);
            Double NumberOfDays = TimeDiff.Days * Factor;
            DateTime FactorDate = BirthDate.AddDays(NumberOfDays);

            // Compute difference between FactorDate (which is TodaysDate * Factor) and BirthDate
            AgeInMonths = FactorDate.Month - BirthDate.Month;
            AgeInYears = FactorDate.Year - BirthDate.Year;

            if (FactorDate.Day < BirthDate.Day)
            {
                AgeInMonths--;
            }

            if (AgeInMonths < 0)
            {
                AgeInYears--;
                AgeInMonths += 12;
            }

            AgeInDays = (FactorDate - BirthDate.AddMonths((AgeInYears * 12) + AgeInMonths)).Days;

        }

        private void WriteColumnOutput(string Message, int Years, int Months, int Days)
        {

            Console.WriteLine("{0,-25} {1,-10:N0} {2,-10:N0} {3,-10:N0}", Message, Years, Months, Days);

        }

        private void DisplayOutput(DateTime TodaysDate, DateTime BirthDate)
        {
            Console.WriteLine("");

            // Not allowed to play if the current year is before 1582
            if (TodaysDate.Year < 1582)
            {
                Console.WriteLine("Not prepared to give day of week prior to MDLXXXII.");
                return;
            }

            // Share which day of the week the BirthDate was on
            Console.Write(" {0} ", BirthDate.ToString("d"));

            string DateVerb = "";
            if (BirthDate.CompareTo(TodaysDate) < 0)
            {
                DateVerb = "was a ";
            }
            else if (BirthDate.CompareTo(TodaysDate) == 0)
            {
                DateVerb = "is a ";
            }
            else
            {
                DateVerb = "will be a ";
            }
            Console.Write("{0}", DateVerb);

            // Special warning if their birth date was on a Friday the 13th!
            if (BirthDate.DayOfWeek.ToString().Equals("Friday") && BirthDate.Day == 13)
            {
                Console.WriteLine("{0} the Thirteenth---BEWARE", BirthDate.DayOfWeek.ToString());
            }
            else
            {
                Console.WriteLine("{0}", BirthDate.DayOfWeek.ToString());
            }

            // If today's date is the same month & day as the birth date then wish them a happy birthday!
            if (BirthDate.Month == TodaysDate.Month && BirthDate.Day == TodaysDate.Day)
            {
                Console.WriteLine("");
                Console.Write("***Happy Birthday***");
            }

            Console.WriteLine("");

            // Only show the date calculations if BirthDate is before TodaysDate
            if (DateVerb.Trim().Equals("was a"))
            {

                Console.WriteLine("{0,-24} {1,-10} {2,-10} {3,-10}", " ", "Years", "Months", "Days");

                int TheYears = 0, TheMonths = 0, TheDays = 0;
                int FlexYears = 0, FlexMonths = 0, FlexDays = 0;

                CalculateDateDiff(TodaysDate, BirthDate, 1, out TheYears, out TheMonths, out TheDays);
                WriteColumnOutput("Your age if birthdate", TheYears, TheMonths, TheDays);

                FlexYears = TheYears;
                FlexMonths = TheMonths;
                FlexDays = TheDays;
                CalculateDateDiff(TodaysDate, BirthDate, .35, out FlexYears, out FlexMonths, out FlexDays);
                WriteColumnOutput("You have slept", FlexYears, FlexMonths, FlexDays);

                FlexYears = TheYears;
                FlexMonths = TheMonths;
                FlexDays = TheDays;
                CalculateDateDiff(TodaysDate, BirthDate, .17, out FlexYears, out FlexMonths, out FlexDays);
                WriteColumnOutput("You have eaten", FlexYears, FlexMonths, FlexDays);

                FlexYears = TheYears;
                FlexMonths = TheMonths;
                FlexDays = TheDays;
                CalculateDateDiff(TodaysDate, BirthDate, .23, out FlexYears, out FlexMonths, out FlexDays);
                string FlexPhrase = "You have played";
                if (TheYears > 3)
                    FlexPhrase = "You have played/studied";
                if (TheYears > 9)
                    FlexPhrase = "You have worked/played";
                WriteColumnOutput(FlexPhrase, FlexYears, FlexMonths, FlexDays);

                FlexYears = TheYears;
                FlexMonths = TheMonths;
                FlexDays = TheDays;
                CalculateDateDiff(TodaysDate, BirthDate, .25, out FlexYears, out FlexMonths, out FlexDays);
                WriteColumnOutput("You have relaxed", FlexYears, FlexMonths, FlexDays);

                Console.WriteLine("");
                Console.WriteLine("* You may retire in {0} *".PadLeft(38), BirthDate.Year + 65);
            }
        }

        public void PlayTheGame()
        {
            DateTime TodaysDate = DateTime.MinValue;
            DateTime BirthDate = DateTime.MinValue;

            DisplayIntro();

            TodaysDate = PromptForADate("Enter today's date in the form: 3,24,1978  ? ");
            BirthDate = PromptForADate("Enter day of birth (or other day of interest)? ");

            DisplayOutput(TodaysDate, BirthDate);   

        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            new Weekday().PlayTheGame();

        }
    }
}