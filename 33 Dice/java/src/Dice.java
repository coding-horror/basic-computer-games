import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Dice
 * <p>
 * Based on the Basic game of Dice here
 * https://github.com/coding-horror/basic-computer-games/blob/main/33%20Dice/dice.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Dice {

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        START_GAME,
        INPUT_AND_CALCULATE,
        RESULTS,
        GAME_OVER
    }

    // Current game state
    private GAME_STATE gameState;

    private int[] spots;

    public Dice() {
        kbScanner = new Scanner(System.in);

        gameState = GAME_STATE.START_GAME;
    }

    /**
     * Main game loop
     */
    public void play() {

        do {
            switch (gameState) {

                case START_GAME:
                    intro();
                    spots = new int[12];
                    gameState = GAME_STATE.INPUT_AND_CALCULATE;
                    break;

                case INPUT_AND_CALCULATE:

                    int howManyRolls = displayTextAndGetNumber("HOW MANY ROLLS? ");
                    for (int i = 0; i < howManyRolls; i++) {
                        int diceRoll = (int) (Math.random() * 6 + 1) + (int) (Math.random() * 6 + 1);
                        // save dice roll in zero based array
                        spots[diceRoll - 1]++;
                    }
                    gameState = GAME_STATE.RESULTS;
                    break;

                case RESULTS:
                    System.out.println("TOTAL SPOTS" + simulateTabs(8) + "NUMBER OF TIMES");
                    for (int i = 1; i < 12; i++) {
                        // show output using zero based array
                        System.out.println(simulateTabs(5) + (i + 1) + simulateTabs(20) + spots[i]);
                    }
                    System.out.println();
                    if (yesEntered(displayTextAndGetInput("TRY AGAIN? "))) {
                        gameState = GAME_STATE.START_GAME;
                    } else {
                        gameState = GAME_STATE.GAME_OVER;
                    }
                    break;
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    private void intro() {
        System.out.println(simulateTabs(34) + "DICE");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("THIS PROGRAM SIMULATES THE ROLLING OF A");
        System.out.println("PAIR OF DICE.");
        System.out.println("YOU ENTER THE NUMBER OF TIMES YOU WANT THE COMPUTER TO");
        System.out.println("'ROLL' THE DICE.  WATCH OUT, VERY LARGE NUMBERS TAKE");
        System.out.println("A LONG TIME.  IN PARTICULAR, NUMBERS OVER 5000.");
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
}
