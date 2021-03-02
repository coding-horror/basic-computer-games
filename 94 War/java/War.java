import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Scanner;

/**
 * Converted FROM BASIC to Java by Nahid Mondol
 * 
 * Based on Trevor Hobsons approach
 */
public class War {

    private static final int CARD_DECK_SIZE = 52;
    private static int totalPlayerScore = 0;
    private static int totalComputerScore = 0;
    private static boolean directionPhase;
    static Scanner input = new Scanner(System.in);
    static ArrayList<String> cards = new ArrayList<String>(
            Arrays.asList("S-2", "H-2", "C-2", "D-2", "S-3", "H-3", "C-3", "D-3", "S-4", "H-4", "C-4", "D-4", "S-5",
                    "H-5", "C-5", "D-5", "S-6", "H-6", "C-6", "D-6", "S-7", "H-7", "C-7", "D-7", "S-8", "H-8", "C-8",
                    "D-8", "S-9", "H-9", "C-9", "D-9", "S-10", "H-10", "C-10", "D-10", "S-J", "H-J", "C-J", "D-J",
                    "S-Q", "H-Q", "C-Q", "D-Q", "S-K", "H-K", "C-K", "D-K", "S-A", "H-A", "C-A", "D-A"));

    public static void main(String[] args) {
        System.out.println("\t         WAR");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println("THIS IS THE CARD GAME OF WAR. EACH CARD IS GIVEN BY SUIT-#");
        System.out.print("AS S-7 FOR SPADE 7. DO YOU WANT DIRECTIONS? ");

        while (!directionPhase) {
            getDirections(input);
        }
        System.out.println();
        playGame();
    }

    private static void getDirections(Scanner userInput) {
        switch (userInput.nextLine().toLowerCase()) {
            case "yes":
                System.out.println("THE COMPUTER GIVES YOU AND IT A 'CARD'. THE HIGHER CARD");
                System.out.println("(NUMERICALLY) WINS. THE GAME ENDS WHEN YOU CHOOSE NOT TO ");
                System.out.println("CONTINUE OR WHEN YOU HAVE FINISHED THE PACK.");
                directionPhase = true;
                break;
            case "no":
                directionPhase = true;
                break;
            default:
                System.out.print("YES OR NO, PLEASE.   ");
        }
    }

    private static void playGame() {

        Collections.shuffle(cards);

        // Since the cards are already suffled, pull each card from the deck starting
        // until it is empty or until the user quits.
        for (int i = 1; i <= CARD_DECK_SIZE; i += 2) {
            System.out.println("YOU: " + cards.get(i - 1) + "\t " + "COMPUTER: " + cards.get(i));
            getWinner(cards.get(i - 1), cards.get(i));
            // if(endGame) {
            // break;
            // }
        }
        System.out.println("WE HAVE RUN OUT OF CARDS. FINAL SCORE:  YOU: " + getPlayerScore() + " COMPUTER: "
                + getComputerScore());
        System.out.println("THANKS FOR PLAYING.  IT WAS FUN.");
    }

    private static void getWinner(String playerCard, String computerCard) {

        String playerScore = (playerCard.length() == 3) ? Character.toString(playerCard.charAt(2))
                : playerCard.substring(2, 4);
        String computerScore = (computerCard.length() == 3) ? Character.toString(computerCard.charAt(2))
                : computerCard.substring(2, 4);

        if (Integer.parseInt(checkCourtCards(playerScore)) > Integer.parseInt(checkCourtCards(computerScore))) {
            System.out.println("YOU WIN.   YOU HAVE " + playerWonRound() + "   COMPUTER HAS " + getComputerScore());
        } else if (Integer.parseInt(checkCourtCards(playerScore)) < Integer.parseInt(checkCourtCards(computerScore))) {
            System.out.println(
                    "COMPUTER WINS!!!   YOU HAVE " + getPlayerScore() + "   COMPUTER HAS " + computerWonRound());
        } else {
            System.out.println("TIE.  NO SCORE CHANGE");
        }

        System.out.println("DO YOU WANT TO CONTINUE? ");
    }

    private static String checkCourtCards(String score) {
        switch (score) {
            case "J":
                return "11";
            case "Q":
                return "12";
            case "K":
                return "13";
            case "A":
                return "14";
            default:
                return score;
        }
    }

    private static int playerWonRound() {
        return totalPlayerScore += 1;
    }

    private static int getPlayerScore() {
        return totalPlayerScore;
    }

    private static int computerWonRound() {
        return totalComputerScore += 1;
    }

    private static int getComputerScore() {
        return totalComputerScore;
    }
}