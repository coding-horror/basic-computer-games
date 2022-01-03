using Train;
using Xunit;

namespace TrainTests
{
    public class TrainGameTests
    {
        [Fact]
        public void MiniumRandomNumber()
        {
            TrainGame game = new TrainGame();
            Assert.True(game.GenerateRandomNumber(10, 10) >= 10);
        }

        [Fact]
        public void MaximumRandomNumber()
        {
            TrainGame game = new TrainGame();
            Assert.True(game.GenerateRandomNumber(10, 10) <= 110);
        }

        [Fact]
        public void IsInputYesWhenY()
        {
            Assert.True(TrainGame.IsInputYes("y"));
        }

        [Fact]
        public void IsInputYesWhenNotY()
        {
            Assert.False(TrainGame.IsInputYes("a"));
        }

        [Fact]
        public void CarDurationTest()
        {
            Assert.Equal(1, TrainGame.CalculateCarJourneyDuration(30, 1, 15) );
        }

        [Fact]
        public void IsWithinAllowedDifference()
        {
            Assert.True(TrainGame.IsWithinAllowedDifference(5,5));
        }


        [Fact]
        public void IsNotWithinAllowedDifference()
        {
            Assert.False(TrainGame.IsWithinAllowedDifference(6, 5));
        }
    }
}
