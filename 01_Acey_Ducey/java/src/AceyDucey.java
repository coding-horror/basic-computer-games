import java.util.Scanner;

/**
 * Game of AceyDucey
 * <p>
 * Based on the Basic game of AceyDucey here
 * https://github.com/coding-horror/basic-computer-games/blob/main/01%20Acey%20Ducey/aceyducey.bas
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class AceyDucey {

    // Current amount of players cash
    private int playerAmount;

    // First drawn dealer card
    private Card firstCard;

    // Second drawn dealer card
    private Card secondCard;

    // Players drawn card
    private Card playersCard;

    // User to display game intro/instructions
    private boolean firstTimePlaying = true;

    // game state to determine if game over
    private boolean gameOver = false;

    // Used for keyboard input
    private final Scanner kbScanner;

    // Constant value for cards from a deck - 2 lowest, 14 (Ace) highest
    public static final int LOW_CARD_RANGE = 2;
    public static final int HIGH_CARD_RANGE = 14;

    public AceyDucey() {
        // Initialise players cash
        playerAmount = 100;

        // Initialise kb scanner
        kbScanner = new Scanner(System.in);
    }

    // Play again method - public method called from class invoking game
    // If player enters YES then the game can be played again (true returned)
    // otherwise not (false)
    public boolean playAgain() {
        System.out.println();
        System.out.println("SORRY, FRIEND, BUT YOU BLEW YOUR WAD.");
        System.out.println();
        System.out.println();
        System.out.print("TRY AGAIN (YES OR NO) ");
        String playAgain = kbScanner.next().toUpperCase();
        System.out.println();
        System.out.println();
        if (playAgain.equals("YES")) {
            return true;
        } else {
            System.out.println("O.K., HOPE YOU HAD FUN!");
            return false;
        }
    }

    // game loop method

    public void play() {

        // Keep playing hands until player runs out of cash
        do {
            if (firstTimePlaying) {
                intro();
                firstTimePlaying = false;
            }
            displayBalance();
            drawCards();
            displayCards();
            int betAmount = getBet();
            playersCard = randomCard();
            displayPlayerCard();
            if (playerWon()) {
                System.out.println("YOU WIN!!");
                playerAmount += betAmount;
            } else {
                System.out.println("SORRY, YOU LOSE");
                playerAmount -= betAmount;
                // Player run out of money?
                if (playerAmount <= 0) {
                    gameOver = true;
                }
            }

        } while (!gameOver); // Keep playing until player runs out of cash
    }

    // Method to determine if player won (true returned) or lost (false returned)
    // to win a players card has to be in the range of the first and second dealer
    // drawn cards inclusive of first and second cards.
    private boolean playerWon() {
        // winner
        return (playersCard.getValue() >= firstCard.getValue())
                && playersCard.getValue() <= secondCard.getValue();

    }

    private void displayPlayerCard() {
        System.out.println(playersCard.getName());
    }

    // Get the players bet, and return the amount
    // 0 is considered a valid bet, but better more than the player has available is not
    // method will loop until a valid bet is entered.
    private int getBet() {
        boolean validBet = false;
        int amount;
        do {
            System.out.print("WHAT IS YOUR BET ");
            amount = kbScanner.nextInt();
            if (amount == 0) {
                System.out.println("CHICKEN!!");
                validBet = true;
            } else if (amount > playerAmount) {
                System.out.println("SORRY, MY FRIEND, BUT YOU BET TOO MUCH.");
                System.out.println("YOU HAVE ONLY " + playerAmount + " DOLLARS TO BET.");
            } else {
                validBet = true;
            }
        } while (!validBet);

        return amount;
    }

    private void displayBalance() {
        System.out.println("YOU NOW HAVE " + playerAmount + " DOLLARS.");
    }

    private void displayCards() {
        System.out.println("HERE ARE YOUR NEXT TWO CARDS: ");
        System.out.println(firstCard.getName());
        System.out.println(secondCard.getName());
    }

    // Draw two dealer cards, and save them for later use.
    // ensure that the first card is a smaller value card than the second one
    private void drawCards() {

        do {
            firstCard = randomCard();
            secondCard = randomCard();
        } while (firstCard.getValue() >= secondCard.getValue());
    }

    // Creates a random card
    private Card randomCard() {
        return new Card((int) (Math.random()
                * (HIGH_CARD_RANGE - LOW_CARD_RANGE + 1) + LOW_CARD_RANGE));
    }

    public void intro() {
        System.out.println("ACEY DUCEY CARD GAME");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println();
        System.out.println("ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER");
        System.out.println("THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP");
        System.out.println("YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING");
        System.out.println("ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE");
        System.out.println("A VALUE BETWEEN THE FIRST TWO.");
        System.out.println("IF YOU DO NOT WANT TO BET, INPUT: 0");
    }
}