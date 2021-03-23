using Microsoft.VisualStudio.TestTools.UnitTesting;
using War;



namespace WarTester
{
    [TestClass]
    public class CardTest
    {
        Card c1 = new Card(Suit.clubs, Rank.two);
        Card c2 = new Card(Suit.clubs, Rank.ten);
        Card c3 = new Card(Suit.diamonds, Rank.ten);
        Card c4 = new Card(Suit.diamonds, Rank.ten);

        [TestMethod]
        public void LessThan_IsValid()
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
        public void GreaterThan_IsValid()
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
        public void LessThanEquals_IsValid()
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
        public void GreaterThanEquals_IsValid()
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
        public void ToString_IsValid()
        {
            string s1 = c1.ToString();
            string s2 = c3.ToString();
            string s3 = new Card(Suit.hearts, Rank.queen).ToString();
            string s4 = new Card(Suit.spades, Rank.ace).ToString();

            Assert.IsTrue(s1 == "C-2", "s1 invalid");
            Assert.IsTrue(s2 == "D-10", "s2 invalid");
            Assert.IsTrue(s3 == "H-Q", "s3 invalid");
            Assert.IsTrue(s4 == "S-A", "s4 invalid");
        }
    }
}
