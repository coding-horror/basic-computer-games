import static java.util.stream.Collectors.joining;

import java.util.Arrays;
import java.util.Collections;
import java.util.LinkedList;

public class Blackjack {
	public static void main(String[] args) {
		System.out.println("BLACK JACK");
		System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n");
		if(promptBoolean("DO YOU WANT INSTRUCTIONS? ")){
			System.out.println("THIS IS THE GAME OF 21. AS MANY AS 7 PLAYERS MAY PLAY THE");
			System.out.println("GAME. ON EACH DEAL, BETS WILL BE ASKED FOR, AND THE");
			System.out.println("PLAYERS' BETS SHOULD BE TYPED IN. THE CARDS WILL THEN BE");
			System.out.println("DEALT, AND EACH PLAYER IN TURN PLAYS HIS HAND. THE");
			System.out.println("FIRST RESPONSE SHOULD BE EITHER 'D', INDICATING THAT THE");
			System.out.println("PLAYER IS DOUBLING DOWN, 'S', INDICATING THAT HE IS");
			System.out.println("STANDING, 'H', INDICATING HE WANTS ANOTHER CARD, OR '/',");
			System.out.println("INDICATING THAT HE WANTS TO SPLIT HIS CARDS. AFTER THE");
			System.out.println("INITIAL RESPONSE, ALL FURTHER RESPONSES SHOULD BE 'S' OR");
			System.out.println("'H', UNLESS THE CARDS WERE SPLIT, IN WHICH CASE DOUBLING");
			System.out.println("DOWN IS AGAIN PERMITTED. IN ORDER TO COLLECT FOR");
			System.out.println("BLACKJACK, THE INITIAL RESPONSE SHOULD BE 'S'.");
		}

		int nPlayers = 0;
		while(nPlayers < 1 || nPlayers > 7) {
			nPlayers = promptInt("NUMBER OF PLAYERS");
		}

		System.out.println("RESHUFFLING");
		LinkedList<Card> deck = new LinkedList<>();
		for(Card.Suit suit : Card.Suit.values()) {
			for(int value = 1; value < 14; value++) {
				deck.add(new Card(value, suit));
			}
		}
		Collections.shuffle(deck);

		int[] bets = new int[nPlayers]; // empty array initialized with all '0' valuses.
		while(!betsAreValid(bets)){
			System.out.println("BETS:");
			for(int i = 0; i < nPlayers; i++) {
				// Note that the bet for player "1" is at index "0" in the bets
				// array and take care to avoid off-by-one errors.
				bets[i] = promptInt("#" + (i + 1));
			}
		}

		/*
		Note that LinkedList is a Deque: https://docs.oracle.com/javase/8/docs/api/java/util/Deque.html
		Player
			CurrentBet
			Total
			Hand
		Hand
			cards LinkedList<Card>
			evaluate() // see 300 in blackjack.bas for eval subroutine logic
		Deck // note the game is played with more than one deck
			cards LinkedList<Card> // instantiate cards and randomize in constructor via Collections.shuffle()
			List<Hand> dealHands(n)
		discardPile Queue<Card>
		Card
			Value
			Suit
		*/
	}

	/**
	 * Prompts the user for a "Yes" or "No" answer.
	 * @param prompt The prompt to display to the user on STDOUT.
	 * @return false if the user enters a value beginning with "N" or "n"; true otherwise.
	 */
	public static boolean promptBoolean(String prompt) {
		System.out.print(prompt);

		// Other ways to read input are
		// new BufferedReader(new InputStreamReader(System.in)).readLine();
		// and new Scanner(System.in)
		// But those are less expressive and care must be taken to close the
		// Reader or Scanner resource.
		String input = System.console().readLine();
		if(input == null) {
			// readLine returns null on CTRL-D or CTRL-Z
			// this is how the original basic handled that.
			System.out.println("!END OF INPUT");
			System.exit(0);
		}

		if(input.toLowerCase().startsWith("n")) {
			return false;
		} else {
			return true;
		}
	}

	/**
	 * Prompts the user for an integer.  As in Vintage Basic, "the optional
	 * prompt string is followed by a question mark and a space." and if the
	 * input is non-numeric, "an error will be generated and the user will be
	 * re-prompted.""
	 *
	 * @param prompt The prompt to display to the user.
	 * @return the number given by the user.
	 */
	public static int promptInt(String prompt) {
		System.out.print(prompt + "? ");

		while(true) {
			String input = System.console().readLine();
			if(input == null) {
				// readLine returns null on CTRL-D or CTRL-Z
				// this is how the original basic handled that.
				System.out.println("!END OF INPUT");
				System.exit(0);
			}
			try {
				return Integer.parseInt(input);
			} catch(NumberFormatException e) {
				// Input was not numeric.
				System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE");
				System.out.print("? ");
				continue;
			}
		}
	}

	/**
	 * Validates that all bets are between 1 and 500 (inclusive).
	 * 
	 * @param bets The array of bets for each player.
	 * @return true if all bets are valid, false otherwise.
	 */
	public static boolean betsAreValid(int[] bets) {
		return Arrays.stream(bets)
			.allMatch(bet -> bet >= 1 && bet <= 500);
	}

}
