import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertAll;
import org.junit.jupiter.api.Test;

public class DeckTest {

    @Test
    void testInit() {
        // When
        Deck deck = new Deck((cards) -> cards);
        deck.reshuffle();

        // Then
        long nCards = deck.size();
        long nSuits = deck.getCards().stream()
                .map(card -> card.getSuit())
                .distinct()
                .count();
        long nValues = deck.getCards().stream()
                .map(card -> card.getValue())
                .distinct()
                .count();

        assertAll("deck",
            () -> assertEquals(52, nCards, "Expected 52 cards in a deck, but got " + nCards),
            () -> assertEquals(4, nSuits, "Expected 4 suits, but got " + nSuits),
            () -> assertEquals(13, nValues, "Expected 13 values, but got " + nValues)
        );
        
    }

}
