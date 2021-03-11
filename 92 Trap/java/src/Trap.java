import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Trap
 * <p>
 * Based on the Basic game of Trap here
 * https://github.com/coding-horror/basic-computer-games/blob/main/92%20Trap/trap.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Trap {

    public static final int HIGH_NUMBER_RANGE = 100;
    public static final int MAX_GUESSES = 6;

    private enum GAME_STATE {
        STARTING,
        START_GAME,
        GUESSING,
        PLAY_AGAIN,
        GAME_OVER
    }

    // Used for keyboard input
    private final Scanner kbScanner;

    // Current game state
    private GAME_STATE gameState;

    // Players guess count;
    private int currentPlayersGuess;

    // Computers random number
    private int computersNumber;

    public Trap() {

        gameState = GAME_STATE.STARTING;

        // Initialise kb scanner
        kbScanner = new Scanner(System.in);
    }

    /**
     * Main game loop
     */
    public void play() {

        do {
            switch (gameState) {

                // Show an introduction and optional instructions the first time the game is played.
                case STARTING:
                    intro();
                    if (yesEntered(displayTextAndGetInput("INSTRUCTIONS? "))) {
                        instructions();
                    }
                    gameState = GAME_STATE.START_GAME;
                    break;

                // Start new game
                case START_GAME:
                    computersNumber = randomNumber();
                    currentPlayersGuess = 1;
                    gameState = GAME_STATE.GUESSING;
                    break;

                // Player guesses the number until they get it or run out of guesses
                case GUESSING:
                    System.out.println();
                    String playerRangeGuess = displayTextAndGetInput("GUESS # " + currentPlayersGuess + "? ");
                    int startRange = getDelimitedValue(playerRangeGuess, 0);
                    int endRange = getDelimitedValue(playerRangeGuess, 1);

                    // Has the player won?
                    if (startRange == computersNumber && endRange == computersNumber) {
                        System.out.println("YOU GOT IT!!!");
                        System.out.println();
                        gameState = GAME_STATE.PLAY_AGAIN;
                    } else {
                        // show where the guess is at
                        System.out.println(showGuessResult(startRange, endRange));
                        currentPlayersGuess++;
                        if (currentPlayersGuess > MAX_GUESSES) {
                            System.out.println("SORRY, THAT'S " + MAX_GUESSES + " GUESSES. THE NUMBER WAS "
                                    + computersNumber);
                            gameState = GAME_STATE.PLAY_AGAIN;
                        }
                    }
                    break;

                // Play again, or exit game?
                case PLAY_AGAIN:
                    System.out.println("TRY AGAIN");
                    gameState = GAME_STATE.START_GAME;
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    /**
     * Show the players guess result
     *
     * @param start start range entered by player
     * @param end   end range
     * @return text to indicate their progress.
     */
    private String showGuessResult(int start, int end) {

        String status;
        if (start <= computersNumber && computersNumber <= end) {
            status = "YOU HAVE TRAPPED MY NUMBER.";
        } else if (computersNumber < start) {
            status = "MY NUMBER IS SMALLER THAN YOUR TRAP NUMBERS.";
        } else {
            status = "MY NUMBER IS LARGER THAN YOUR TRAP NUMBERS.";
        }

        return status;
    }

    private void instructions() {
        System.out.println("I AM THINKING OF A NUMBER BETWEEN 1 AND " + HIGH_NUMBER_RANGE);
        System.out.println("TRY TO GUESS MY NUMBER. ON EACH GUESS,");
        System.out.println("YOU ARE TO ENTER 2 NUMBERS, TRYING TO TRAP");
        System.out.println("MY NUMBER BETWEEN THE TWO NUMBERS. I WILL");
        System.out.println("TELL YOU IF YOU HAVE TRAPPED MY NUMBER, IF MY");
        System.out.println("NUMBER IS LARGER THAN YOUR TWO NUMBERS, OR IF");
        System.out.println("MY NUMBER IS SMALLER THAN YOUR TWO NUMBERS.");
        System.out.println("IF YOU WANT TO GUESS ONE SINGLE NUMBER, TYPE");
        System.out.println("YOUR GUESS FOR BOTH YOUR TRAP NUMBERS.");
        System.out.println("YOU GET " + MAX_GUESSES + " GUESSES TO GET MY NUMBER.");
    }

    private void intro() {
        System.out.println("TRAP");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println();
    }

    /**
     * Accepts a string delimited by comma's and returns the nth delimited
     * value (starting at count 0).
     *
     * @param text - text with values separated by comma's
     * @param pos  - which position to return a value for
     * @return the int representation of the value
     */
    private int getDelimitedValue(String text, int pos) {
        String[] tokens = text.split(",");
        return Integer.parseInt(tokens[pos]);
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
     * Generate random number
     * Used as a single digit of the computer player
     *
     * @return random number
     */
    private int randomNumber() {
        return (int) (Math.random()
                * (HIGH_NUMBER_RANGE) + 1);
    }
}