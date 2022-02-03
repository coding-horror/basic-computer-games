import java.util.Collections;
import java.util.LinkedList;
import java.util.List;

public class Deck {

    private LinkedList<Card> cards;
    
    /**
     * Initialize the game deck with the given number of standard decks.
     * e.g. if you want to play with 2 decks, then {@code new Decks(2)} will
     * initialize 'cards' with 2 copies of a standard 52 card deck.
     * 
     * @param nDecks
     */
    public Deck() {
        cards = new LinkedList<>();
        for(Card.Suit suit : Card.Suit.values()) {
            for(int value = 1; value < 14; value++) {
                cards.add(new Card(value, suit));
            }
        }
    }

    /**
     * Deals one card from the deck, removing it from this object's state.
     * @return The card that was dealt.
     */
    public Card deal() {
        // TODO implement Deck.deal() -  new Card(10, Card.Suit.CLUBS) added temporarily
        return new Card(10, Card.Suit.CLUBS); 
    }

    /**
     * Shuffle the cards in this deck.
     */
    public void shuffle() {
        // TODO implement Deck.shuffle()
        // Probably just call Collections.shuffle(cards);
    }

    /**
     * Get the number of cards in this deck.
     * @return The number of cards in this deck. For example, 52 for a single deck.
     */
    public int size() {
        return cards.size();
    }

    /**
     * Returns the cards in this deck.
     * @return An immutable view of the cards in this deck.
     */
    public List<Card> getCards() {
        // The returned list is immutable because we don't want other code messing with the deck.
        return Collections.unmodifiableList(cards);
    }
}
