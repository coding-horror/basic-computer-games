using Microsoft.VisualStudio.TestTools.UnitTesting;
using Craps;

namespace CrapsTester
{
    [TestClass]
    public class DiceTests
    {
        [TestMethod]
        public void SixSidedDiceReturnsValidRolls()
        {
            var dice = new Dice();
            for (int i = 0; i < 100000; i++)
            {
                var roll = dice.Roll();
                Assert.IsTrue(roll >= 1 && roll <= dice.sides);
            }
        }

        [TestMethod]
        public void TwentySidedDiceReturnsValidRolls()
        {
            var dice = new Dice(20);
            for (int i = 0; i < 100000; i++)
            {
                var roll = dice.Roll();
                Assert.IsTrue(roll >= 1 && roll <= dice.sides);
            }
        }
    }
}
