using Craps;
using Microsoft.VisualStudio.TestTools.UnitTesting;



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

        [TestMethod]
        public void DiceRollsAreRandom()
        {
            // Roll 600,000 dice and count how many rolls there are for each side.

            var dice = new Dice();

            int numOnes = 0;
            int numTwos = 0;
            int numThrees = 0;
            int numFours = 0;
            int numFives = 0;
            int numSixes = 0;
            int numErrors = 0;

            for (int i = 0; i < 600000; i++)
            {
                switch (dice.Roll())
                {
                    case 1:
                        numOnes++;
                        break;

                    case 2:
                        numTwos++;
                        break;

                    case 3:
                        numThrees++;
                        break;

                    case 4:
                        numFours++;
                        break;

                    case 5:
                        numFives++;
                        break;

                    case 6:
                        numSixes++;
                        break;

                    default:
                        numErrors++;
                        break;
                }
            }

            // We'll assume that a variation of 10% in rolls for the different numbers is random enough.
            // Perfectly random rolling would produce 100000 rolls per side, +/- 5% of this gives the 
            // range 90000..110000.
            const int minRolls = 95000;
            const int maxRolls = 105000;
            Assert.IsTrue(numOnes >= minRolls && numOnes <= maxRolls);
            Assert.IsTrue(numTwos >= minRolls && numTwos <= maxRolls);
            Assert.IsTrue(numThrees >= minRolls && numThrees <= maxRolls);
            Assert.IsTrue(numFours >= minRolls && numFours <= maxRolls);
            Assert.IsTrue(numFives >= minRolls && numFives <= maxRolls);
            Assert.IsTrue(numSixes >= minRolls && numSixes <= maxRolls);
            Assert.AreEqual(numErrors, 0);
        }
    }
}
