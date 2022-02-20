import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.DisplayName;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.io.EOFException;
import java.io.StringReader;
import java.io.StringWriter;
import java.io.UncheckedIOException;
import java.util.Arrays;
import java.util.LinkedList;

public class GameTest {

    private StringReader in;
    private StringWriter out;
    private Game game;

    private void givenStubGame() {
        in = new StringReader("");
        out = new StringWriter();
        UserIo userIo = new UserIo(in, out);
        Deck deck = new Deck((cards) -> cards);
        game = new Game(deck, userIo);
    }

    private void givenInput(String input) {
        in = new StringReader(input);
        out = new StringWriter();
        UserIo userIo = new UserIo(in, out);
        Deck deck = new Deck((cards) -> cards);
        game = new Game(deck, userIo);
    }

    private void givenInput(String input, Card... customCards) {
        in = new StringReader(input);
        out = new StringWriter();
        UserIo userIo = new UserIo(in, out);
        LinkedList<Card> cardList = new LinkedList<>();
        cardList.addAll(Arrays.asList(customCards));
        Deck deck = new Deck((cards) -> cardList);
        game = new Game(deck, userIo);
    }

    @Test
    public void shouldQuitOnCtrlD() {
        // Given
        givenInput("\u2404"); // U+2404 is "End of Transmission" sent by CTRL+D (or CTRL+Z on Windows)

        // When
        Exception e = assertThrows(UncheckedIOException.class, game::run);

        // Then
        assertTrue(e.getCause() instanceof EOFException);
        assertEquals("!END OF INPUT", e.getMessage());
    }

    @Test
    @DisplayName("play() should end on STAY")
    public void playEndOnStay(){
        // Given
        Player player = new Player(1);
        player.dealCard(new Card(3, Card.Suit.CLUBS));
        player.dealCard(new Card(2, Card.Suit.SPADES));
        givenInput("S\n"); // "I also like to live dangerously."

        // When
        game.play(player);

        // Then
        assertEquals("PLAYER 1 ? ", out.toString());
    }

    @Test
    @DisplayName("play() should allow HIT until BUST")
    public void playHitUntilBust() {
        // Given
        Player player = new Player(1);
        player.dealCard(new Card(10, Card.Suit.HEARTS));
        player.dealCard(new Card(10, Card.Suit.SPADES));

        givenInput("H\nH\nH\n",
            new Card(1, Card.Suit.SPADES), // 20
            new Card(1, Card.Suit.HEARTS), // 21
            new Card(1, Card.Suit.CLUBS)); // 22 - D'oh!

        // When
        game.play(player);

        // Then
        assertTrue(out.toString().contains("BUSTED"));
    }

    @Test
    @DisplayName("Should allow double down on initial turn")
    public void playDoubleDown(){
        // Given
        Player player = new Player(1);
        player.setCurrentBet(100);
        player.dealCard(new Card(10, Card.Suit.HEARTS));
        player.dealCard(new Card(4, Card.Suit.SPADES));

        givenInput("D\n", new Card(7, Card.Suit.SPADES));

        // When
        game.play(player);

        // Then
        assertTrue(player.getCurrentBet() == 200);
        assertTrue(player.getHand().size() == 3);
    }

    @Test
    @DisplayName("Should NOT allow double down after initial deal")
    public void playDoubleDownLate(){
        // Given
        Player player = new Player(1);
        player.setCurrentBet(100);
        player.dealCard(new Card(10, Card.Suit.HEARTS));
        player.dealCard(new Card(2, Card.Suit.SPADES));

        givenInput("H\nD\nS\n", new Card(7, Card.Suit.SPADES));

        // When
        game.play(player);

        // Then
        assertTrue(out.toString().contains("TYPE H, OR S, PLEASE"));
    }

    @Test
    @DisplayName("scoreHand should sum non-ace values normally")
    public void scoreHandNormally() {
        // Given
        givenStubGame();
        LinkedList<Card> hand = new LinkedList<>();
        hand.add(new Card(4, Card.Suit.SPADES));
        hand.add(new Card(6, Card.Suit.SPADES));
        hand.add(new Card(10, Card.Suit.SPADES));

        // When
        int result = game.scoreHand(hand);

        // Then
        assertEquals(20, result);
    }

    @Test
    @DisplayName("scoreHand should treat face cards as 10")
    public void scoreHandFaceCards() {
        // Given
        givenStubGame();
        LinkedList<Card> hand = new LinkedList<>();
        hand.add(new Card(11, Card.Suit.SPADES));
        hand.add(new Card(12, Card.Suit.SPADES));
        hand.add(new Card(13, Card.Suit.SPADES));

        // When
        int result = game.scoreHand(hand);

        // Then
        assertEquals(30, result);
    }

    @Test
    @DisplayName("scoreHand should score aces as 11 when possible")
    public void scoreHandSoftAce() {
        // Given
        givenStubGame();
        LinkedList<Card> hand = new LinkedList<>();
        hand.add(new Card(10, Card.Suit.SPADES));
        hand.add(new Card(1, Card.Suit.SPADES));

        // When
        int result = game.scoreHand(hand);

        // Then
        assertEquals(21, result);
    }

    @Test
    @DisplayName("scoreHand should score aces as 1 when using 11 would bust")
    public void scoreHandHardAce() {
        // Given
        givenStubGame();
        LinkedList<Card> hand = new LinkedList<>();
        hand.add(new Card(10, Card.Suit.SPADES));
        hand.add(new Card(9, Card.Suit.SPADES));
        hand.add(new Card(1, Card.Suit.SPADES));

        // When
        int result = game.scoreHand(hand);

        // Then
        assertEquals(20, result);
    }

    @Test
    @DisplayName("scoreHand should score 3 aces as 13")
    public void scoreHandMultipleAces() {
        // Given
        givenStubGame();
        LinkedList<Card> hand = new LinkedList<>();
        hand.add(new Card(1, Card.Suit.SPADES));
        hand.add(new Card(1, Card.Suit.CLUBS));
        hand.add(new Card(1, Card.Suit.HEARTS));

        // When
        int result = game.scoreHand(hand);

        // Then
        assertEquals(13, result);
    }

    @Test
    @DisplayName("compareHands should return 1 meaning A beat B, 20 to 12")
    public void compareHandsAWins() {

        givenStubGame();
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(10, Card.Suit.SPADES));
        handA.add(new Card(10, Card.Suit.CLUBS));

        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(1, Card.Suit.SPADES));
        handB.add(new Card(1, Card.Suit.CLUBS));

        int result = game.compareHands(handA,handB);

        assertEquals(1, result);
    }

    @Test
    @DisplayName("compareHands should return -1 meaning B beat A, 18 to 4")
    public void compareHandsBwins() {
        givenStubGame();
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(2, Card.Suit.SPADES));
        handA.add(new Card(2, Card.Suit.CLUBS));

        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(5, Card.Suit.SPADES));
        handB.add(new Card(6, Card.Suit.HEARTS));
        handB.add(new Card(7, Card.Suit.CLUBS));

        int result = game.compareHands(handA,handB);

        assertEquals(-1, result);
    }

    @Test
    @DisplayName("compareHands should return 1 meaning A beat B, natural Blackjack to Blackjack")
    public void compareHandsAWinsWithNaturalBlackJack() {
        //Hand A wins with natural BlackJack, B with Blackjack
        givenStubGame();
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(10, Card.Suit.SPADES));
        handA.add(new Card(1, Card.Suit.CLUBS));

        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(6, Card.Suit.SPADES));
        handB.add(new Card(7, Card.Suit.HEARTS));
        handB.add(new Card(8, Card.Suit.CLUBS));

        int result = game.compareHands(handA,handB);

        assertEquals(1, result);
    }

    @Test
    @DisplayName("compareHands should return -1 meaning B beat A, natural Blackjack to Blackjack")
    public void compareHandsBWinsWithNaturalBlackJack() {
        givenStubGame();
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(6, Card.Suit.SPADES));
        handA.add(new Card(7, Card.Suit.HEARTS));
        handA.add(new Card(8, Card.Suit.CLUBS));
        
        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(10, Card.Suit.SPADES));
        handB.add(new Card(1, Card.Suit.CLUBS));

        int result = game.compareHands(handA,handB);

        assertEquals(-1, result);
    }

    @Test
    @DisplayName("compareHands should return 0, hand A and B tied with a Blackjack")
    public void compareHandsTieBothBlackJack() {
        givenStubGame();
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(11, Card.Suit.SPADES));
        handA.add(new Card(10, Card.Suit.CLUBS));
        
        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(10, Card.Suit.SPADES));
        handB.add(new Card(11, Card.Suit.CLUBS));

        int result = game.compareHands(handA,handB);

        assertEquals(0, result);
    }

    @Test
    @DisplayName("compareHands should return 0, hand A and B tie without a Blackjack")
    public void compareHandsTieNoBlackJack() {
        givenStubGame();
        LinkedList<Card> handA = new LinkedList<>();
        handA.add(new Card(10, Card.Suit.DIAMONDS));
        handA.add(new Card(10, Card.Suit.HEARTS));
        
        LinkedList<Card> handB = new LinkedList<>();
        handB.add(new Card(10, Card.Suit.SPADES));
        handB.add(new Card(10, Card.Suit.CLUBS));

        int result = game.compareHands(handA,handB);

        assertEquals(0, result);
    }

    //@Test
    @DisplayName("playSplit() should end on STAY")
    public void playSplitEndOnStay(){
        // Given
        Player player = new Player(1);
        player.dealCard(new Card(1, Card.Suit.CLUBS));
        player.dealCard(new Card(1, Card.Suit.SPADES));
        givenInput("S\nS\n"); 

        // When
        game.playSplit(player);

        // Then
        assertTrue(out.toString().contains("FIRST HAND RECEIVES"));
        assertTrue(out.toString().contains("SECOND HAND RECEIVES"));
        assertEquals("PLAYER 1 ? ", out.toString());
    }

    //@Test
    @DisplayName("playSplit() should allow HIT until BUST")
    public void playSplitHitUntilBust() {
        // Given
        Player player = new Player(1);
        player.dealCard(new Card(10, Card.Suit.HEARTS));
        player.dealCard(new Card(10, Card.Suit.SPADES));

        givenInput("H\nH\n",
            new Card(12, Card.Suit.SPADES), // 20
            new Card(12, Card.Suit.HEARTS), // Split hand 20
            new Card(12, Card.Suit.DIAMONDS), // 30
            new Card(12, Card.Suit.CLUBS)); // Split hand 30

        // When
        game.playSplit(player);

        // Then
        assertTrue(out.toString().contains("BUSTED"));
    }

    //@Test
    @DisplayName("playSplit should allow double down")
    public void playSplitDoubleDown(){
        // Given
        Player player = new Player(1);
        player.setCurrentBet(100);
        player.dealCard(new Card(9, Card.Suit.HEARTS));
        player.dealCard(new Card(9, Card.Suit.SPADES));

        givenInput("D\nD\n", 
            new Card(6, Card.Suit.HEARTS),
            new Card(7, Card.Suit.HEARTS),
            new Card(6, Card.Suit.CLUBS),
            new Card(7, Card.Suit.CLUBS));

        // When
        game.playSplit(player);

        // Then
        assertTrue(player.getCurrentBet() == 200);
        assertTrue(player.getSplitBet() == 200);
        assertTrue(player.getHand().size() == 3);
        assertTrue(player.getSplitHand().size() == 3);
    }

    //@Test
    @DisplayName("playSplit should NOT allow re-splitting")
    public void playSplitDoubleDownLate(){
        // Given
        Player player = new Player(1);
        player.setCurrentBet(100);
        player.dealCard(new Card(1, Card.Suit.HEARTS));
        player.dealCard(new Card(1, Card.Suit.SPADES));

        givenInput("\\\nS\nS\n", 
            new Card(13, Card.Suit.HEARTS),
            new Card(13, Card.Suit.SPADES));

        // When
        game.playSplit(player);

        // Then
        assertTrue(out.toString().contains("TYPE H, S OR D, PLEASE"));
    }

}
