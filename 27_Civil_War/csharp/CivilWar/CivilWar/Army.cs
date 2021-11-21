using System;
using System.Collections.Generic;
using System.Linq;

namespace CivilWar
{
    public class Army
    {
        private enum Resource
        {
            Food,
            Salaries,
            Ammunition
        }

        public Army(Side side)
        {
            Side = side;
        }

        public Side Side { get; }

        // Cumulative
        public int Wins { get; private set; } // W, L
        public int Losses { get; private set; } // L, W
        public int Draws { get; private set; } // W0
        public int BattlesFought => Wins + Draws + Losses;
        public bool Surrendered { get; private set; } // Y, Y2 == 5

        public int CumulativeHistoricCasualties { get; private set; } // P1, P2
        public int CumulativeSimulatedCasualties { get; private set; } // T1, T2
        public int CumulativeHistoricMen { get; private set; } // M3, M4

        private int income; // R1, R2
        private int moneySpent; // Q1, Q2

        private bool IsFirstBattle => income == 0;

        // This battle
        private int historicMen; // M1, M2
        public int HistoricCasualties { get; private set; }

        public int Money { get; private set; } // D(n)
        public int Men { get; private set; } // M5, M6
        public int Inflation { get; private set; } // I1, I2
        public int InflationDisplay => Side == Side.Confederate ? Inflation + 15 : Inflation; // Confederate inflation is shown with 15 added - no idea why!

        private readonly Dictionary<Resource, int> allocations = new(); // F(n), H(n), B(n) for food, salaries, ammunition

        public int Strategy { get; protected set; } // Y1, Y2

        public double Morale => (2.0 * allocations[Resource.Food] * allocations[Resource.Food] + allocations[Resource.Salaries] * allocations[Resource.Salaries]) / (reducedAvailableMen * reducedAvailableMen + 1); // O, O2

        public int Casualties { get; protected set; } // C5, C6
        public int Desertions { get; protected set; } // E, E2
        public int MenLost => Casualties + Desertions;
        public bool AllLost { get; private set; } // U, U2

        private double reducedAvailableMen; // F1

        protected virtual double FractionUnspent => (income - moneySpent) / (income + 1.0);

        public void PrepareBattle(int men, int casualties)
        {
            historicMen = men;
            HistoricCasualties = casualties;
            Inflation = 10 + (Losses - Wins) * 2;
            Money = 100 * (int)(men * (100 - Inflation) / 2000.0 * (1 + FractionUnspent) + 0.5);
            Men = (int)(men * 1 + (CumulativeHistoricCasualties - CumulativeSimulatedCasualties) / (CumulativeHistoricMen + 1.0));
            reducedAvailableMen = men * 5.0 / 6.0;
        }

        public virtual void AllocateResources()
        {
            Console.WriteLine($"{Side} General ---\nHow much do you wish to spend for");
            while (true)
            {
                foreach (Resource resource in Enum.GetValues<Resource>())
                {
                    if (EnterResource(resource))
                        break;
                }
                if (allocations.Values.Sum() <= Money)
                    return;
                Console.WriteLine($"Think again! You have only ${Money}");
            }
        }

        private bool EnterResource(Resource resource)
        {
            while (true)
            {
                Console.WriteLine($" - {resource}");
                switch ((int.TryParse(Console.ReadLine(), out int val), val))
                {
                    case (false, _):
                        Console.WriteLine("Not a valid number");
                        break;
                    case (_, < 0):
                        Console.WriteLine("Negative values not allowed");
                        break;
                    case (_, 0) when IsFirstBattle:
                        Console.WriteLine("No previous entries");
                        break;
                    case (_, 0):
                        Console.WriteLine("Assume you want to keep same allocations");
                        return true;
                    case (_, > 0):
                        allocations[resource] = val;
                        return false;
                }
            }
        }

        public virtual void DisplayMorale()
        {
            Console.WriteLine($"{Side} morale is {Morale switch { < 5 => "Poor", < 10 => "Fair", _ => "High" }}");
        }

        public virtual bool ChooseStrategy(bool isReplay) => EnterStrategy(true, "(1-5)");

        protected bool EnterStrategy(bool canSurrender, string hint)
        {
            Console.WriteLine($"{Side} strategy {hint}");
            while (true)
            {
                switch ((int.TryParse(Console.ReadLine(), out int val), val))
                {
                    case (false, _):
                        Console.WriteLine("Not a valid number");
                        break;
                    case (_, 5) when canSurrender:
                        Surrendered = true;
                        Console.WriteLine($"The {Side} general has surrendered");
                        return true;
                    case (_, < 1 or >= 5):
                        Console.WriteLine($"Strategy {val} not allowed.");
                        break;
                    default:
                        Strategy = val;
                        return false;
                }
            }
        }

        public virtual void CalculateLosses(Army opponent)
        {
            AllLost = false;
            int stratFactor = 2 * (Math.Abs(Strategy - opponent.Strategy) + 1);
            Casualties = (int)Math.Round(HistoricCasualties * 0.4 * (1 + 1.0 / stratFactor) * (1 + 1 / Morale) * (1.28 + reducedAvailableMen / (allocations[Resource.Ammunition] + 1)));
            Desertions = (int)Math.Round(100 / Morale);

            // If losses > men present, rescale losses
            if (MenLost > Men)
            {
                Casualties = 13 * Men / 20;
                Desertions = Men - Casualties;
                AllLost = true;
            }
        }

        public void RecordResult(Side winner)
        {
            if (winner == Side)
                Wins++;
            else if (winner == Side.Both)
                Draws++;
            else
                Losses++;

            CumulativeSimulatedCasualties += MenLost;
            CumulativeHistoricCasualties += HistoricCasualties;
            moneySpent += allocations.Values.Sum();
            income += historicMen * (100 - Inflation) / 20;
            CumulativeHistoricMen += historicMen;

            LearnStrategy();
        }

        protected virtual void LearnStrategy() { }

        public void DisplayWarResult(Army opponent)
        {
            Console.WriteLine("\n\n\n\n");
            Console.WriteLine($"The {Side} general has won {Wins} battles and lost {Losses}");
            Side winner = (Surrendered, opponent.Surrendered, Wins < Losses) switch
            {
                (_, true, _) => Side,
                (true, _, _) or (_, _, true) => opponent.Side,
                _ => Side
            };
            Console.WriteLine($"The {winner} general has won the war\n");
        }

        public virtual void DisplayStrategies() { }
    }

    class ComputerArmy : Army
    {
        public int[] StrategyProb { get; } = { 25, 25, 25, 25 }; // S(n)
        private readonly Random strategyRng = new();

        public ComputerArmy(Side side) : base(side) { }

        protected override double FractionUnspent => 0.0;

        public override void AllocateResources() { }

        public override void DisplayMorale() { }

        public override bool ChooseStrategy(bool isReplay)
        {
            if (isReplay)
                return EnterStrategy(false, $"(1-4; usually previous {Side} strategy)");

            // Basic code comments say "If actual strategy info is in  data then r-100 is extra weight given to that strategy" but there's no data or code to do it.
            int strategyChosenProb = strategyRng.Next(100); // 0-99
            int sumProbs = 0;
            for (int i = 0; i < 4; i++)
            {
                sumProbs += StrategyProb[i];
                if (strategyChosenProb < sumProbs)
                {
                    Strategy = i + 1;
                    break;
                }
            }
            Console.WriteLine($"{Side} strategy is {Strategy}");
            return false;
        }

        protected override void LearnStrategy()
        {
            // Learn  present strategy, start forgetting old ones
            // - present strategy gains 3 * s, others lose s probability points, unless a strategy falls below 5 %.
            const int s = 3;
            int presentGain = 0;
            for (int i = 0; i < 4; i++)
            {
                if (StrategyProb[i] >= 5)
                {
                    StrategyProb[i] -= s;
                    presentGain += s;
                }
            }
            StrategyProb[Strategy - 1] += presentGain;
        }

        public override void CalculateLosses(Army opponent)
        {
            Casualties = (int)(17.0 * HistoricCasualties * opponent.HistoricCasualties / (opponent.Casualties * 20));
            Desertions = (int)(5 * opponent.Morale);
        }

        public override void DisplayStrategies()
        {
            ConsoleUtils.WriteWordWrap($"\nIntelligence suggests that the {Side} general used strategies 1, 2, 3, 4 in the following percentages:");
            Console.WriteLine(string.Join(", ", StrategyProb));
        }
    }
}
