import java.util.Scanner;

public class Blackjack {
	public static void main(String[] args) {
		System.out.println("BLACK JACK");
		System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n");
		System.out.println("Do you want instructions?");
		String input = getInput();
		if(input.toLowerCase().equals("y")){
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
			System.out.println("NUMBER OF PLAYERS");
		}
		System.out.println(input);
	}

	// TODO copied from Craps. Clean this up.
	public static String getInput() {
		Scanner scanner = new Scanner(System.in);
		System.out.print("> ");
		while (true) {
		  try {
			return scanner.nextLine();
		  } catch (Exception ex) {
			try {
			  scanner.nextLine(); // flush whatever this non number stuff is.
			} catch (Exception ns_ex) { // received EOF (ctrl-d or ctrl-z if windows)
			  System.out.println("END OF INPUT, STOPPING PROGRAM.");
			  System.exit(1);
			}
		  }
		  System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE");
		  System.out.print("> ");
		}
	}
}
