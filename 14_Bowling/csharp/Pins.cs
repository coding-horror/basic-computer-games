using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling
{
    public class Pins
    {
        public enum State { Up, Down };
        public static readonly int TotalPinCount = 10;
        private readonly Random random = new();

        private State[] PinSet { get; set; }

        public Pins()
        {
            PinSet = new State[TotalPinCount];
        }
        public State this[int i]
        {
            get { return PinSet[i]; }
            set { PinSet[i] = value; }
        }
        public int Roll()
        {
            // REM ARK BALL GENERATOR USING MOD '15' SYSTEM
            for (int i = 0; i < 20; ++i)
            {
                var x = random.Next(100) + 1;
                int j;
                for (j = 1; j <= 10; ++j)
                {
                    if (x < 15 * j)
                        break;
                }
                var pindex = 15 * j - x;
                if (pindex > 0 && pindex <= TotalPinCount)
                    PinSet[--pindex] = State.Down;
            }
            return GetPinsDown();
        }
        public void Reset()
        {
            for (int i = 0; i < PinSet.Length; ++i)
            {
                PinSet[i] = State.Up;
            }
        }
        public int GetPinsDown()
        {
            return PinSet.Count(p => p == State.Down);
        }
    }
}
