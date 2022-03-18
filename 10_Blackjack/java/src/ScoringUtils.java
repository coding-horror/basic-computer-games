import java.util.List;

public final class ScoringUtils {

	/**
	 * Calculates the value of a hand. When the hand contains aces, it will
	 * count one of them as 11 if that does not result in a bust.
	 * 
	 * @param hand the hand to evaluate
	 * @return The numeric value of a hand. A value over 21 indicates a bust.
	 */
	public static final int scoreHand(List<Card> hand) {
		int nAces = (int) hand.stream().filter(c -> c.value() == 1).count();
		int value = hand.stream()
				.mapToInt(Card::value)
				.filter(v -> v != 1) // start without aces
				.map(v -> v > 10 ? 10 : v) // all face cards are worth 10. The 'expr ? a : b' syntax is called the
											// 'ternary operator'
				.sum();
		value += nAces; // start by treating all aces as 1
		if (nAces > 0 && value <= 11) {
			value += 10; // We can use one of the aces to an 11
			// You can never use more than one ace as 11, since that would be 22 and a bust.
		}
		return value;
	}

	/**
	 * Compares two hands accounting for natural blackjacks and busting using the
	 * java.lang.Comparable convention of returning positive or negative integers
	 * 
	 * @param handA hand to compare
	 * @param handB other hand to compare
	 * @return a negative integer, zero, or a positive integer as handA is less
	 *         than, equal to, or greater than handB.
	 */
	public static final int compareHands(List<Card> handA, List<Card> handB) {
		int scoreA = scoreHand(handA);
		int scoreB = scoreHand(handB);
		if (scoreA == 21 && scoreB == 21) {
			if (handA.size() == 2 && handB.size() != 2) {
				return 1; // Hand A wins with a natural blackjack
			} else if (handA.size() != 2 && handB.size() == 2) {
				return -1; // Hand B wins with a natural blackjack
			} else {
				return 0; // Tie
			}
		} else if (scoreA > 21 || scoreB > 21) {
			if (scoreA > 21 && scoreB > 21) {
				return 0; // Tie, both bust
			} else if (scoreB > 21) {
				return 1; // A wins, B busted
			} else {
				return -1; // B wins, A busted
			}
		} else {
			return Integer.compare(scoreA, scoreB);
		}
	}

}
