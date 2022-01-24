/**
 * This is an example of an "immutable" class in Java. That's just a fancy way
 * of saying the properties (value and suit) can't change after the object has
 * been created (it has no 'setter' methods and the properties are 'final').
 *
 * Immutability often makes it easier to reason about code logic and avoid
 * certain classes of bugs.
 *
 * Since it would never make sense for a card to change in the middle of a game,
 * this is a good candidate for immutability.
 *
 */
public final class Card {

	public enum Suit {
		HEARTS, DIAMONDS, SPADES, CLUBS;
	}

    // Since this class is immutable, there's no reason these couldn't be
    // 'public', but the pattern of using 'getters' is more consistent with
    // typical Java coding patterns.
	private final int value;
	private final Suit suit;

	public Card(int value, Suit suit) {
		this.value = value;
		this.suit = suit;
	}

	public int getValue() {
		return this.value;
    }

	public Suit getSuit() {
		return this.suit;
	}

    public String toString() {
        StringBuilder result = new StringBuilder(2); 
        if(value < 11) {
            result.append(value);
        } else if(value == 11) {
            result.append('J');
        } else if(value == 12) {
            result.append('Q');
        } else if(value == 13) {
            result.append('K');
        }
        // Uncomment to include the suit in output. Useful for debugging, but
        // doesn't match the original BASIC behavior.
        // result.append(suit.name().charAt(0));
        return result.toString();
    }
}