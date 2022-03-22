import org.junit.jupiter.api.Test;

import org.junit.jupiter.api.DisplayName;

import static org.junit.jupiter.api.Assertions.assertEquals;

import java.util.LinkedList;

public class ScoringUtilsTest {

    @Test
    @DisplayName("scoreHand should score aces as 1 when using 11 would bust")
    public void scoreHandHardAce() {
        // Given
        LinkedList<Card> hand = new LinkedList<>();
        hand.add(new Card(10, Card.Suit.SPADES));
        hand.add(new Card(9, Card.Suit.SPADES));
        hand.add(new Card(1, Card.Suit.SPADES));

        // When
        int result = ScoringUtils.scoreHand(hand);

        // Then
        assertEquals(20, result);
    }

    @Test
    @DisplayName("scoreHand should score 3 aces as 13")
    public void scoreHandMultipleAces() {
        // Given
        LinkedList<Card> hand = new LinkedList<>();
        hand.add(new Card(1, Card.Suit.SPADES));
        hand.add(new Card(1, Card.Suit.CLUBS));
        hand.add(new Card(1, Card.Suit.HEARTS));

        // When
        int result = ScoringUtils.scoreHand(hand);

        // Then
        assertEquals(13, result);
    }

    @Test
    @DisplayName("compareHands should return 1 meaning A beat B, 20 to 12")
    public void compareHandsAWins() {
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(10, Card.Suit.SPADES));
        handA.add(new Card(10, Card.Suit.CLUBS));

        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(1, Card.Suit.SPADES));
        handB.add(new Card(1, Card.Suit.CLUBS));

        int result = ScoringUtils.compareHands(handA,handB);

        assertEquals(1, result);
    }

    @Test
    @DisplayName("compareHands should return -1 meaning B beat A, 18 to 4")
    public void compareHandsBwins() {
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(2, Card.Suit.SPADES));
        handA.add(new Card(2, Card.Suit.CLUBS));

        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(5, Card.Suit.SPADES));
        handB.add(new Card(6, Card.Suit.HEARTS));
        handB.add(new Card(7, Card.Suit.CLUBS));

        int result = ScoringUtils.compareHands(handA,handB);

        assertEquals(-1, result);
    }

    @Test
    @DisplayName("compareHands should return 1 meaning A beat B, natural Blackjack to Blackjack")
    public void compareHandsAWinsWithNaturalBlackJack() {
        //Hand A wins with natural BlackJack, B with Blackjack
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(10, Card.Suit.SPADES));
        handA.add(new Card(1, Card.Suit.CLUBS));

        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(6, Card.Suit.SPADES));
        handB.add(new Card(7, Card.Suit.HEARTS));
        handB.add(new Card(8, Card.Suit.CLUBS));

        int result = ScoringUtils.compareHands(handA,handB);

        assertEquals(1, result);
    }

    @Test
    @DisplayName("compareHands should return -1 meaning B beat A, natural Blackjack to Blackjack")
    public void compareHandsBWinsWithNaturalBlackJack() {
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(6, Card.Suit.SPADES));
        handA.add(new Card(7, Card.Suit.HEARTS));
        handA.add(new Card(8, Card.Suit.CLUBS));
        
        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(10, Card.Suit.SPADES));
        handB.add(new Card(1, Card.Suit.CLUBS));

        int result = ScoringUtils.compareHands(handA,handB);

        assertEquals(-1, result);
    }

    @Test
    @DisplayName("compareHands should return 0, hand A and B tied with a Blackjack")
    public void compareHandsTieBothBlackJack() {
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(11, Card.Suit.SPADES));
        handA.add(new Card(10, Card.Suit.CLUBS));
        
        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(10, Card.Suit.SPADES));
        handB.add(new Card(11, Card.Suit.CLUBS));

        int result = ScoringUtils.compareHands(handA,handB);

        assertEquals(0, result);
    }

    @Test
    @DisplayName("compareHands should return 0, hand A and B tie without a Blackjack")
    public void compareHandsTieNoBlackJack() {
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(10, Card.Suit.DIAMONDS));
        handA.add(new Card(10, Card.Suit.HEARTS));
        
        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(10, Card.Suit.SPADES));
        handB.add(new Card(10, Card.Suit.CLUBS));

        int result = ScoringUtils.compareHands(handA,handB);

        assertEquals(0, result);
    }

    @Test
    @DisplayName("compareHands should return 0, hand A and B tie when both bust")
    public void compareHandsTieBust() {
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(10, Card.Suit.DIAMONDS));
        handA.add(new Card(10, Card.Suit.HEARTS));
        handA.add(new Card(3, Card.Suit.HEARTS));
        
        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(10, Card.Suit.SPADES));
        handB.add(new Card(11, Card.Suit.SPADES));
        handB.add(new Card(4, Card.Suit.SPADES));

        int result = ScoringUtils.compareHands(handA,handB);

        assertEquals(0, result);
    }
    @Test
    @DisplayName("compareHands should return -1, meaning B beat A, A busted")
    public void compareHandsABusted() {
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(10, Card.Suit.DIAMONDS));
        handA.add(new Card(10, Card.Suit.HEARTS));
        handA.add(new Card(3, Card.Suit.HEARTS));
        
        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(10, Card.Suit.SPADES));
        handB.add(new Card(10, Card.Suit.SPADES));

        int result = ScoringUtils.compareHands(handA,handB);

        assertEquals(-1, result);
    }

    @Test
    @DisplayName("compareHands should return 1, meaning A beat B, B busted")
    public void compareHandsBBusted() {
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(10, Card.Suit.DIAMONDS));
        handA.add(new Card(3, Card.Suit.HEARTS));
        
        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(10, Card.Suit.SPADES));
        handB.add(new Card(10, Card.Suit.SPADES));
        handB.add(new Card(5, Card.Suit.SPADES));

        int result = ScoringUtils.compareHands(handA,handB);

        assertEquals(1, result);
    }
}
