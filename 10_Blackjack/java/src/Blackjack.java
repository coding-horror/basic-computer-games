public class Blackjack {
	public static void main(String[] args) {
		System.out.println("BLACK JACK");
		System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n");
		if(promptBoolean("DO YOU WANT INSTRUCTIONS ")){
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
		int nPlayers = promptInt("NUMBER OF PLAYERS ", 1, 7);
		System.out.println("You picked " + nPlayers);
	}

	/**
	 * Prompts the user for a "Yes" or "No" answer.
	 * @param prompt The prompt to display to the user on STDOUT.
	 * @return false if the user enters a value beginning with "N" (case insensitive), or true otherwise.
	 */
	public static boolean promptBoolean(String prompt) {
		System.out.print(prompt);

		// Other ways to read input are
		// new BufferedReader(new InputStreamReader(System.in)).readLine();
		// and new Scanner(System.in)
		// But those are less expressive and care must be taken to close the
		// Reader or Scanner resource.
		String input = System.console().readLine();

		// input will be null if the user presses CTRL+D or CTRL+Z in Windows
		if(input != null && input.toLowerCase().startsWith("n")) {
			return false;
		} else {
			return true;
		}
	}

	/**
	 * Prompts the user for an integer. Re-prompts if the input is not an int or outside the given range.
	 * @param prompt The prompt to display to the user on STDIN
	 * @param min The minimum allowed value (inclusive)
	 * @param max The maximum allowed value (inclusive)
	 * @return The number given by the user, or -1 for any non-numeric input.
	 */
	public static int promptInt(String prompt, int min, int max) {
		while(true) {
			System.out.print(prompt);

			String input = System.console().readLine();
			int numericInput;
			try {
				numericInput = Integer.parseInt(input);
			} catch(NumberFormatException e) {
				// Non-int input (including CTRL+D/CTRL+Z)
				System.out.println();
				continue;
			}
			if(numericInput < min || numericInput > max) {
				// Out of range. Clear input and re-prompt
				System.out.println();
				continue;
			} else {
				return numericInput;
			}
		}
	}
}
