import java.util.Collections;
import java.util.LinkedList;
import java.util.List;
import java.util.function.Function;

public class Deck {

    private LinkedList<Card> cards;
    private Function<LinkedList<Card>, LinkedList<Card>> shuffleAlgorithm;
    
    /**
     * Initialize the game deck with the given number of standard decks.
     * e.g. if you want to play with 2 decks, then {@code new Decks(2)} will
     * initialize 'cards' with 2 copies of a standard 52 card deck.
     * 
     * @param shuffleAlgorithm A function that takes the initial sorted card
     * list and returns a shuffled list ready to deal.
     * 
     */
    public Deck(Function<LinkedList<Card>, LinkedList<Card>> shuffleAlgorithm) {
        this.shuffleAlgorithm = shuffleAlgorithm;
    }

    /**
     * Deals one card from the deck, removing it from this object's state. If
     * the deck is empty, it will be reshuffled before dealing a new card.
     * 
     * @return The card that was dealt.
     */
    public Card deal() {
        if(cards == null || cards.isEmpty()) {
            reshuffle();
        }
        return cards.pollFirst();
    }

    /**
     * Shuffle the cards in this deck using the shuffleAlgorithm.
     */
    public void reshuffle() {
        LinkedList<Card> newCards = new LinkedList<>();
        for(Card.Suit suit : Card.Suit.values()) {
            for(int value = 1; value < 14; value++) {
                newCards.add(new Card(value, suit));
            }
        }
        this.cards = this.shuffleAlgorithm.apply(newCards);
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
