import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Nichomachus
 * <p>
 * Based on the Basic game of Nichomachus here
 * https://github.com/coding-horror/basic-computer-games/blob/main/64%20Nicomachus/nicomachus.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */

public class Nicomachus {

    public static final long TWO_SECONDS = 2000;

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        START_GAME,
        GET_INPUTS,
        RESULTS,
        PLAY_AGAIN
    }

    int remainderNumberDividedBy3;
    int remainderNumberDividedBy5;
    int remainderNumberDividedBy7;

    // Current game state
    private GAME_STATE gameState;

    public Nicomachus() {
        kbScanner = new Scanner(System.in);
        gameState = GAME_STATE.START_GAME;
    }

    /**
     * Main game loop
     */
    public void play() throws Exception {

        do {
            switch (gameState) {

                case START_GAME:
                    intro();
                    gameState = GAME_STATE.GET_INPUTS;
                    break;

                case GET_INPUTS:

                    System.out.println("PLEASE THINK OF A NUMBER BETWEEN 1 AND 100.");
                    remainderNumberDividedBy3 = displayTextAndGetNumber("YOUR NUMBER DIVIDED BY 3 HAS A REMAINDER OF? ");
                    remainderNumberDividedBy5 = displayTextAndGetNumber("YOUR NUMBER DIVIDED BY 5 HAS A REMAINDER OF? ");
                    remainderNumberDividedBy7 = displayTextAndGetNumber("YOUR NUMBER DIVIDED BY 7 HAS A REMAINDER OF? ");

                    gameState = GAME_STATE.RESULTS;

                case RESULTS:
                    System.out.println("LET ME THINK A MOMENT...");
                    // Simulate the basic programs for/next loop to delay things.
                    // Here we are sleeping for one second.
                    Thread.sleep(TWO_SECONDS);

                    // Calculate the number the player was thinking of.
                    int answer = (70 * remainderNumberDividedBy3) + (21 * remainderNumberDividedBy5)
                            + (15 * remainderNumberDividedBy7);

                    // Something similar was in the original basic program
                    // (to test if the answer was 105 and deducting 105 until it was <= 105
                    while (answer > 105) {
                        answer -= 105;
                    }

                    do {
                        String input = displayTextAndGetInput("YOUR NUMBER WAS " + answer + ", RIGHT? ");
                        if (yesEntered(input)) {
                            System.out.println("HOW ABOUT THAT!!");
                            break;
                        } else if (noEntered(input)) {
                            System.out.println("I FEEL YOUR ARITHMETIC IS IN ERROR.");
                            break;
                        } else {
                            System.out.println("EH?  I DON'T UNDERSTAND '" + input + "'  TRY 'YES' OR 'NO'.");
                        }
                    } while (true);

                    gameState = GAME_STATE.PLAY_AGAIN;
                    break;

                case PLAY_AGAIN:
                    System.out.println("LET'S TRY ANOTHER");
                    gameState = GAME_STATE.GET_INPUTS;
                    break;
            }

            // Original basic program looped until CTRL-C
        } while (true);
    }

    private void intro() {
        System.out.println(addSpaces(33) + "NICOMA");
        System.out.println(addSpaces(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("BOOMERANG PUZZLE FROM ARITHMETICA OF NICOMACHUS -- A.D. 90!");
        System.out.println();
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
        return kbScanner.nextLine();
    }

    /**
     * Return a string of x spaces
     *
     * @param spaces number of spaces required
     * @return String with number of spaces
     */
    private String addSpaces(int spaces) {
        char[] spacesTemp = new char[spaces];
        Arrays.fill(spacesTemp, ' ');
        return new String(spacesTemp);
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
     * Checks whether player entered N or NO to a question.
     *
     * @param text player string from kb
     * @return true of N or NO was entered, otherwise false
     */
    private boolean noEntered(String text) {
        return stringIsAnyValue(text, "N", "NO");
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

    public static void main(String[] args) throws Exception {

        Nicomachus nicomachus = new Nicomachus();
        nicomachus.play();
    }
}
