import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Slots
 * <p>
 * Based on the Basic game of Slots here
 * https://github.com/coding-horror/basic-computer-games/blob/main/80%20Slots/slots.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Slots {

    public static final String[] SLOT_SYMBOLS = {"BAR", "BELL", "ORANGE", "LEMON", "PLUM", "CHERRY"};

    public static final int NUMBER_SYMBOLS = SLOT_SYMBOLS.length;

    // Jackpot symbol (BAR)
    public static final int BAR_SYMBOL = 0;

    // Indicator that the current spin won nothing
    public static final int NO_WINNER = -1;

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        START_GAME,
        ONE_SPIN,
        RESULTS,
        GAME_OVER
    }

    // Current game state
    private GAME_STATE gameState;

    // Different types of spin results
    private enum WINNINGS {
        JACKPOT(100),
        TOP_DOLLAR(10),
        DOUBLE_BAR(5),
        REGULAR(2),
        NO_WIN(0);

        private final int multiplier;

        WINNINGS(int mult) {
            multiplier = mult;
        }

        // No win returns the negative amount of net
        // otherwise calculate winnings based on
        // multiplier
        public int calculateWinnings(int bet) {

            if (multiplier == 0) {
                return -bet;
            } else {
                // Return original bet plus a multipler
                // of the win type
                return (multiplier * bet) + bet;
            }
        }
    }

    private int playerBalance;

    public Slots() {

        kbScanner = new Scanner(System.in);
        gameState = GAME_STATE.START_GAME;
    }

    /**
     * Main game loop
     */
    public void play() {

        int[] slotReel = new int[3];

        do {
            // Results of a single spin
            WINNINGS winnings;

            switch (gameState) {

                case START_GAME:
                    intro();
                    playerBalance = 0;
                    gameState = GAME_STATE.ONE_SPIN;
                    break;

                case ONE_SPIN:

                    int playerBet = displayTextAndGetNumber("YOUR BET? ");

                    slotReel[0] = randomSymbol();
                    slotReel[1] = randomSymbol();
                    slotReel[2] = randomSymbol();

                    // Store which symbol (if any) matches at least one other reel
                    int whichSymbolWon = winningSymbol(slotReel[0], slotReel[1], slotReel[2]);

                    // Display the three randomly drawn symbols
                    StringBuilder output = new StringBuilder();
                    for (int i = 0; i < 3; i++) {
                        if (i > 0) {
                            output.append(" ");
                        }
                        output.append(SLOT_SYMBOLS[slotReel[i]]);
                    }

                    System.out.println(output);

                    // Calculate results

                    if (whichSymbolWon == NO_WINNER) {
                        // No symbols match = nothing won
                        winnings = WINNINGS.NO_WIN;
                    } else if (slotReel[0] == slotReel[1] && slotReel[0] == slotReel[2]) {
                        // Top dollar, 3 matching symbols
                        winnings = WINNINGS.TOP_DOLLAR;
                        if (slotReel[0] == BAR_SYMBOL) {
                            // All 3 symbols are BAR. Jackpot!
                            winnings = WINNINGS.JACKPOT;
                        }
                    } else {
                        // At this point the remaining options are a regular win
                        // or a double, since the rest (including not winning) have already
                        // been checked above.
                        // Assume a regular win
                        winnings = WINNINGS.REGULAR;

                        // But if it was the BAR symbol that matched, its a double bar
                        if (slotReel[0] == BAR_SYMBOL) {
                            winnings = WINNINGS.DOUBLE_BAR;
                        }

                    }

                    // Update the players balance with the amount won or lost on this spin
                    playerBalance += winnings.calculateWinnings(playerBet);

                    System.out.println();

                    // Output what happened on this spin
                    switch (winnings) {
                        case NO_WIN:
                            System.out.println("YOU LOST.");
                            break;

                        case REGULAR:
                            System.out.println("DOUBLE!!");
                            System.out.println("YOU WON!");
                            break;

                        case DOUBLE_BAR:
                            System.out.println("*DOUBLE BAR*");
                            System.out.println("YOU WON!");
                            break;

                        case TOP_DOLLAR:
                            System.out.println();
                            System.out.println("**TOP DOLLAR**");
                            System.out.println("YOU WON!");
                            break;

                        case JACKPOT:
                            System.out.println();
                            System.out.println("***JACKPOT***");
                            System.out.println("YOU WON!");
                            break;

                    }

                    System.out.println("YOUR STANDINGS ARE $" + playerBalance);

                    // If player does not elect to play again, show results of session
                    if (!yesEntered(displayTextAndGetInput("AGAIN? "))) {
                        gameState = GAME_STATE.RESULTS;

                    }
                    break;

                case RESULTS:
                    if (playerBalance == 0) {
                        System.out.println("HEY, YOU BROKE EVEN.");
                    } else if (playerBalance > 0) {
                        System.out.println("COLLECT YOUR WINNINGS FROM THE H&M CASHIER.");
                    } else {
                        // Lost
                        System.out.println("PAY UP!  PLEASE LEAVE YOUR MONEY ON THE TERMINAL.");
                    }

                    gameState = GAME_STATE.GAME_OVER;
                    break;
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    private void intro() {
        System.out.println(simulateTabs(30) + "SLOTS");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("YOU ARE IN THE H&M CASINO,IN FRONT OF ONE OF OUR");
        System.out.println("ONE-ARM BANDITS. BET FROM $1 TO $100.");
        System.out.println("TO PULL THE ARM, PUNCH THE RETURN KEY AFTER MAKING YOUR BET.");
    }

    /*
     * Print a message on the screen, then accept input from Keyboard.
     * Converts input to an Integer
     *
     * @param text message to be displayed on screen.
     * @return what was typed by the player.
     */
    private int displayTextAndGetNumber(String text) {
        return Integer.parseInt(displayTextAndGetInput(text));
    }

    /*
     * Print a message on the screen, then accept input from Keyboard.
     *
     * @param text message to be displayed on screen.
     * @return what was typed by the player.
     */
    private String displayTextAndGetInput(String text) {
        System.out.print(text);
        return kbScanner.next();
    }

    /**
     * Checks whether player entered Y or YES to a question.
     *
     * @param text player string from kb
     * @return true of Y or YES was entered, otherwise false
     */
    private boolean yesEntered(String text) {
        return stringIsAnyValue(text, "Y", "YES");
    }

    /**
     * Check whether a string equals one of a variable number of values
     * Useful to check for Y or YES for example
     * Comparison is case insensitive.
     *
     * @param text   source string
     * @param values a range of values to compare against the source string
     * @return true if a comparison was found in one of the variable number of strings passed
     */
    private boolean stringIsAnyValue(String text, String... values) {

        return Arrays.stream(values).anyMatch(str -> str.equalsIgnoreCase(text));
    }

    /**
     * Simulate the old basic tab(xx) command which indented text by xx spaces.
     *
     * @param spaces number of spaces required
     * @return String with number of spaces
     */
    private String simulateTabs(int spaces) {
        char[] spacesTemp = new char[spaces];
        Arrays.fill(spacesTemp, ' ');
        return new String(spacesTemp);
    }

    /**
     * Find the symbol that won this round i.e. the first reel that matched another reel
     *
     * @param reel1 reel1 spin result
     * @param reel2 reel2 spin result
     * @param reel3 reel3 spin result
     * @return NO_WINNER if no reels match otherwise an int 0-2 to indicate the reel that matches another
     */
    private int winningSymbol(int reel1, int reel2, int reel3) {
        if (reel1 == reel2) {
            return 0;
        } else if (reel1 == reel3) {
            return 0;
        } else if (reel2 == reel3) {
            return 1;
        } else {
            return NO_WINNER;
        }
    }

    /**
     * Random symbol for a slot wheel
     *
     * @return number between 0-5
     */
    private int randomSymbol() {
        return (int) (Math.random() * NUMBER_SYMBOLS);
    }
}
