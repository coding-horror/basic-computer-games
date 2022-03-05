import org.junit.jupiter.api.Test;

import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;

import static org.junit.jupiter.api.Assertions.assertAll;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.io.EOFException;
import java.io.StringReader;
import java.io.StringWriter;
import java.io.UncheckedIOException;
import java.util.Arrays;
import java.util.Collections;
import java.util.LinkedList;
import java.util.List;

public class GameTest {

    private StringReader in;
    private StringWriter out;
    private Game game;

    private StringBuilder playerActions;
    private LinkedList<Card> cards;

    @BeforeEach
    public void resetIo() {
        in = null;
        out = null;
        game = null;
        playerActions = new StringBuilder();
        cards = new LinkedList<>();
    }

    private void playerGets(int value, Card.Suit suit) {
        cards.add(new Card(value, suit));
    }

    private void playerSays(String action) {
        playerActions.append(action).append(System.lineSeparator());
    }

    private void initGame() {
        System.out.printf("Running game with input: %s\tand cards: %s\n",playerActions.toString(), cards);
        in = new StringReader(playerActions.toString());
        out = new StringWriter();
        UserIo userIo = new UserIo(in, out);
        Deck deck = new Deck((c) -> cards);
        game = new Game(deck, userIo);
    }

    @AfterEach
    private void printOutput() {
        System.out.println(out.toString());
    }

    @Test
    public void shouldQuitOnCtrlD() {
        // Given
        playerSays("\u2404"); // U+2404 is "End of Transmission" sent by CTRL+D (or CTRL+Z on Windows)
        initGame();

        // When
        Exception e = assertThrows(UncheckedIOException.class, game::run);

        // Then
        assertTrue(e.getCause() instanceof EOFException);
        assertEquals("!END OF INPUT", e.getMessage());
    }

    @Test
    @DisplayName("collectInsurance() should not prompt on N")
    public void collectInsuranceNo(){
        // Given
        List<Player> players = Collections.singletonList(new Player(1));
        playerSays("N");
        initGame();

        // When
        game.collectInsurance(players);

        // Then
        assertAll(
            () -> assertTrue(out.toString().contains("ANY INSURANCE")),
            () -> assertFalse(out.toString().contains("INSURANCE BETS"))
        );
    }

    @Test
    @DisplayName("collectInsurance() should collect on Y")
    public void collectInsuranceYes(){
        // Given
        List<Player> players = Collections.singletonList(new Player(1));
        players.get(0).setCurrentBet(100);
        playerSays("Y");
        playerSays("50");
        initGame();

        // When
        game.collectInsurance(players);

        // Then
        assertAll(
            () -> assertTrue(out.toString().contains("ANY INSURANCE")),
            () -> assertTrue(out.toString().contains("INSURANCE BETS")),
            () -> assertEquals(50, players.get(0).getInsuranceBet())
        );
    }

    @Test
    @DisplayName("collectInsurance() should not allow more than 50% of current bet")
    public void collectInsuranceYesTooMuch(){
        // Given
        List<Player> players = Collections.singletonList(new Player(1));
        players.get(0).setCurrentBet(100);
        playerSays("Y");
        playerSays("51");
        playerSays("50");
        initGame();

        // When
        game.collectInsurance(players);

        // Then
        assertAll(
            () -> assertEquals(50, players.get(0).getInsuranceBet()),
            () -> assertTrue(out.toString().contains("# 1 ? # 1 ?"))
        );
    }

    @Test
    @DisplayName("collectInsurance() should not allow negative bets")
    public void collectInsuranceYesNegative(){
        // Given
        List<Player> players = Collections.singletonList(new Player(1));
        players.get(0).setCurrentBet(100);
        playerSays("Y");
        playerSays("-1");
        playerSays("1");
        initGame();

        // When
        game.collectInsurance(players);

        // Then
        assertAll(
            () -> assertEquals(1, players.get(0).getInsuranceBet()),
            () -> assertTrue(out.toString().contains("# 1 ? # 1 ?"))
        );
    }

    @Test
    @DisplayName("collectInsurance() should prompt all players")
    public void collectInsuranceYesTwoPlayers(){
        // Given
        List<Player> players = Arrays.asList(
            new Player(1),
            new Player(2)
        );
        players.get(0).setCurrentBet(100);
        players.get(1).setCurrentBet(100);

        playerSays("Y");
        playerSays("50");
        playerSays("25");
        initGame();

        // When
        game.collectInsurance(players);

        // Then
        assertAll(
            () -> assertEquals(50, players.get(0).getInsuranceBet()),
            () -> assertEquals(25, players.get(1).getInsuranceBet()),
            () -> assertTrue(out.toString().contains("# 1 ? # 2 ?"))
        );
    }

    @Test
    @DisplayName("play() should end on STAY")
    public void playEndOnStay(){
        // Given
        Player player = new Player(1);
        player.dealCard(new Card(3, Card.Suit.CLUBS));
        player.dealCard(new Card(2, Card.Suit.SPADES));
        playerSays("S"); // "I also like to live dangerously."
        initGame();

        // When
        game.play(player);

        // Then
        assertTrue(out.toString().startsWith("PLAYER 1 ? TOTAL IS 5"));
    }

    @Test
    @DisplayName("play() should allow HIT until BUST")
    public void playHitUntilBust() {
        // Given
        Player player = new Player(1);
        player.dealCard(new Card(10, Card.Suit.HEARTS));
        player.dealCard(new Card(10, Card.Suit.SPADES));

        playerSays("H");
        playerGets(1, Card.Suit.SPADES); // 20
        playerSays("H");
        playerGets(1, Card.Suit.HEARTS); // 21
        playerSays("H");
        playerGets(1, Card.Suit.CLUBS); // 22 - D'oh!
        initGame();

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

        playerSays("D");
        playerGets(7, Card.Suit.SPADES);
        initGame();

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

        playerSays("H");
        playerGets(7, Card.Suit.SPADES);
        playerSays("D");
        playerSays("S");
        initGame();

        // When
        game.play(player);

        // Then
        assertTrue(out.toString().contains("TYPE H, OR S, PLEASE"));
    }

    @Test
    @DisplayName("play() should end on STAY after split")
    public void playSplitEndOnStay(){
        // Given
        Player player = new Player(1);
        player.dealCard(new Card(1, Card.Suit.CLUBS));
        player.dealCard(new Card(1, Card.Suit.SPADES));

        playerSays("/");
        playerGets(2, Card.Suit.SPADES); // First hand
        playerSays("S");
        playerGets(2, Card.Suit.SPADES); // Second hand
        playerSays("S");
        initGame();

        // When
        game.play(player);

        // Then
        assertTrue(out.toString().contains("FIRST HAND RECEIVES"));
        assertTrue(out.toString().contains("SECOND HAND RECEIVES"));
    }

    @Test
    @DisplayName("play() should allow HIT until BUST after split")
    public void playSplitHitUntilBust() {
        // Given
        Player player = new Player(1);
        player.dealCard(new Card(10, Card.Suit.HEARTS));
        player.dealCard(new Card(10, Card.Suit.SPADES));

        playerSays("/");
        playerGets(12, Card.Suit.SPADES); // First hand has 20
        playerSays("H");
        playerGets(12, Card.Suit.HEARTS); // First hand busted
        playerGets(10, Card.Suit.HEARTS); // Second hand gets a 10
        playerSays("S");
        initGame();

        // When
        game.play(player);

        // Then
        assertTrue(out.toString().contains("BUSTED"));
    }

    @Test
    @DisplayName("play() should allow HIT on split hand until BUST")
    public void playSplitHitUntilBustHand2() {
        // Given
        Player player = new Player(1);
        player.dealCard(new Card(10, Card.Suit.HEARTS));
        player.dealCard(new Card(10, Card.Suit.SPADES));

        playerSays("/");
        playerGets(1, Card.Suit.CLUBS); // First hand is 21
        playerSays("S");
        playerGets(12, Card.Suit.SPADES); // Second hand is 20
        playerSays("H");
        playerGets(12, Card.Suit.HEARTS); // Busted
        playerSays("H");
        initGame();

        // When
        game.play(player);

        // Then
        assertTrue(out.toString().contains("BUSTED"));
    }

    @Test
    @DisplayName("play() should allow double down on split hands")
    public void playSplitDoubleDown(){
        // Given
        Player player = new Player(1);
        player.setCurrentBet(100);
        player.dealCard(new Card(9, Card.Suit.HEARTS));
        player.dealCard(new Card(9, Card.Suit.SPADES));

        playerSays("/");
        playerGets(5, Card.Suit.DIAMONDS); // First hand is 14
        playerSays("D");
        playerGets(6, Card.Suit.HEARTS); // First hand is 20
        playerGets(7, Card.Suit.CLUBS); // Second hand is 16
        playerSays("D");
        playerGets(4, Card.Suit.CLUBS); // Second hand is 20
        initGame();

        // When
        game.play(player);

        // Then
        assertAll(
            () -> assertEquals(200, player.getCurrentBet(), "Current bet should be doubled"),
            () -> assertEquals(200, player.getSplitBet(), "Split bet should be doubled"),
            () -> assertEquals(3, player.getHand(1).size(), "First hand should have exactly three cards"),
            () -> assertEquals(3, player.getHand(2).size(), "Second hand should have exactly three cards")
        );
    }

    @Test
    @DisplayName("play() should NOT allow re-splitting first split hand")
    public void playSplitTwice(){
        // Given
        Player player = new Player(1);
        player.setCurrentBet(100);
        player.dealCard(new Card(1, Card.Suit.HEARTS));
        player.dealCard(new Card(1, Card.Suit.SPADES));

        playerSays("/");
        playerGets(13, Card.Suit.CLUBS); // First hand
        playerSays("/"); // Not allowed
        playerSays("S");
        playerGets(13, Card.Suit.SPADES); // Second hand
        playerSays("S");
        initGame();

        // When
        game.play(player);

        // Then
        assertTrue(out.toString().contains("TYPE H, S OR D, PLEASE"));
    }

    @Test
    @DisplayName("play() should NOT allow re-splitting second split hand")
    public void playSplitTwiceHand2(){
        // Given
        Player player = new Player(1);
        player.setCurrentBet(100);
        player.dealCard(new Card(1, Card.Suit.HEARTS));
        player.dealCard(new Card(1, Card.Suit.SPADES));

        playerSays("/");
        playerGets(13, Card.Suit.CLUBS); // First hand
        playerSays("S");
        playerGets(13, Card.Suit.SPADES); // Second hand
        playerSays("/"); // Not allowed
        playerSays("S");
        initGame();

        // When
        game.play(player);

        // Then
        assertTrue(out.toString().contains("TYPE H, S OR D, PLEASE"));
    }
}
