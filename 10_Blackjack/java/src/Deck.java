import java.util.LinkedList;

public class Deck {

    LinkedList<Card> cards;
    
    /**
     * Initialize the game deck with the given number of standard decks.
     * e.g. if you want to play with 2 decks, then {@code new Decks(2)} will
     * initialize 'cards' with 2 copies of a standard 52 card deck.
     * 
     * @param nDecks
     */
    public Deck(int nDecks) {
        // TODO implement Deck constructor
        // See line 33 of Blackjack.java for the current version of this code
        /* for each suit
         *   for each value 1-13
         *     add new Card(value, suit) to cards
         */
    }

    /**
     * Deals one card from the deck, removing it from this object's state.
     * @return The card that was dealt.
     */
    public Card deal() {
        // TODO implement Deck.deal()
        return null;
    }

    /**
     * Shuffle the cards in this deck.
     */
    public void shuffle() {
        // TODO implement Deck.shuffle()
        // Probably just call Collections.shuffle(cards);
    }

}
