using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using War;



namespace WarTester
{
    [TestClass]
    public class CardTest
    {
        private Card c1 = new Card(Suit.clubs, Rank.two);
        private Card c2 = new Card(Suit.clubs, Rank.ten);
        private Card c3 = new Card(Suit.diamonds, Rank.ten);
        private Card c4 = new Card(Suit.diamonds, Rank.ten);

        // Test the relational operators.

        [TestMethod]
        public void LessThanIsValid()
        {
            Assert.IsTrue(c1 < c2, "c1 < c2");  // Same suit, different rank.
            Assert.IsFalse(c2 < c1, "c2 < c1"); // Same suit, different rank.

            Assert.IsFalse(c3 < c4, "c3 < c4"); // Same suit, same rank.

            Assert.IsTrue(c1 < c3, "c1 < c3");  // Different suit, different rank.
            Assert.IsFalse(c3 < c1, "c3 < c1"); // Different suit, different rank.

            Assert.IsFalse(c2 < c4, "c2 < c4"); // Different suit, same rank.
            Assert.IsFalse(c4 < c2, "c4 < c2"); // Different suit, same rank.
        }

        [TestMethod]
        public void GreaterThanIsValid()
        {
            Assert.IsFalse(c1 > c2, "c1 > c2"); // Same suit, different rank.
            Assert.IsTrue(c2 > c1, "c2 > c1");  // Same suit, different rank.

            Assert.IsFalse(c3 > c4, "c3 > c4"); // Same suit, same rank.

            Assert.IsFalse(c1 > c3, "c1 > c3"); // Different suit, different rank.
            Assert.IsTrue(c3 > c1, "c3 > c1");  // Different suit, different rank.

            Assert.IsFalse(c2 > c4, "c2 > c4"); // Different suit, same rank.
            Assert.IsFalse(c4 > c2, "c4 > c2"); // Different suit, same rank.
        }

        [TestMethod]
        public void LessThanEqualsIsValid()
        {
            Assert.IsTrue(c1 <= c2, "c1 <= c2");  // Same suit, different rank.
            Assert.IsFalse(c2 <= c1, "c2 <= c1"); // Same suit, different rank.

            Assert.IsTrue(c3 <= c4, "c3 <= c4");  // Same suit, same rank.

            Assert.IsTrue(c1 <= c3, "c1 <= c3");  // Different suit, different rank.
            Assert.IsFalse(c3 <= c1, "c3 <= c1"); // Different suit, different rank.

            Assert.IsTrue(c2 <= c4, "c2 <= c4");  // Different suit, same rank.
            Assert.IsTrue(c4 <= c2, "c4 <= c2");  // Different suit, same rank.
        }

        [TestMethod]
        public void GreaterThanEqualsIsValid()
        {
            Assert.IsFalse(c1 >= c2, "c1 >= c2"); // Same suit, different rank.
            Assert.IsTrue(c2 >= c1, "c2 >= c1");  // Same suit, different rank.

            Assert.IsTrue(c3 >= c4, "c3 >= c4");  // Same suit, same rank.

            Assert.IsFalse(c1 >= c3, "c1 >= c3"); // Different suit, different rank.
            Assert.IsTrue(c3 >= c1, "c3 >= c1");  // Different suit, different rank.

            Assert.IsTrue(c2 >= c4, "c2 >= c4");  // Different suit, same rank.
            Assert.IsTrue(c4 >= c2, "c4 >= c2");  // Different suit, same rank.
        }

        [TestMethod]
        public void ToStringIsValid()
        {
            var s1 = c1.ToString();
            var s2 = c3.ToString();
            var s3 = new Card(Suit.hearts, Rank.queen).ToString();
            var s4 = new Card(Suit.spades, Rank.ace).ToString();

            Assert.IsTrue(s1 == "C-2", "s1 invalid");
            Assert.IsTrue(s2 == "D-10", "s2 invalid");
            Assert.IsTrue(s3 == "H-Q", "s3 invalid");
            Assert.IsTrue(s4 == "S-A", "s4 invalid");
        }
    }

    [TestClass]
    public class DeckTest
    {
        private readonly string cardNamesInOrder = "C-2C-3C-4C-5C-6C-7C-8C-9C-10C-JC-QC-KC-AD-2D-3D-4D-5D-6D-7D-8D-9D-10D-JD-QD-KD-AH-2H-3H-4H-5H-6H-7H-8H-9H-10H-JH-QH-KH-AS-2S-3S-4S-5S-6S-7S-8S-9S-10S-JS-QS-KS-A";

        //Helper method. Adds the names of all the cards together into a single string.
        private string ConcatenateTheDeck(Deck d)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Deck.deckSize; i++)
            {
                sb.Append(d.GetCard(i));
            }

            return sb.ToString();
        }

        [TestMethod]
        public void InitialDeckContainsCardsInOrder()
        {
            Deck d = new Deck();
            string allTheCards = ConcatenateTheDeck(d);

            Assert.IsTrue(allTheCards == cardNamesInOrder);
        }

        [TestMethod]
        public void ShufflingChangesDeck()
        {
            // I'm not sure how to test that shuffling has worked other than to check that the cards aren't in the initial order.
            Deck d = new Deck();
            d.Shuffle();
            string allTheCards = ConcatenateTheDeck(d);

            Assert.IsTrue(allTheCards != cardNamesInOrder);
        }
    }
}
