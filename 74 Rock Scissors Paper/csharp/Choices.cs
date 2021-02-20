using System;

namespace RockScissorsPaper
{
    public class Choices
    {
        public static readonly Choice Rock = new Choice("3", "Rock");
        public static readonly Choice Scissors = new Choice("2", "Scissors");
        public static readonly Choice Paper = new Choice("1", "Paper");

        private static readonly Choice[] _allChoices;
        private static readonly Random _random = new Random();

        static Choices()
        {
            Rock.CanBeat = Scissors;
            Scissors.CanBeat = Paper;
            Paper.CanBeat = Rock;

            _allChoices = new[] { Rock, Scissors, Paper };
        }

        public static Choice GetRandom()
        {
            return _allChoices[_random.Next(_allChoices.GetLength(0))];
        }

        public static bool TryGetBySelector(string selector, out Choice choice)
        {
            foreach (var possibleChoice in _allChoices)
            {
                if (string.Equals(possibleChoice.Selector, selector))
                {
                    choice = possibleChoice;
                    return true;
                }
            }
            choice = null;
            return false;
        }
    }
}
