import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Chief
 * <p>
 * Based on the Basic game of Hurkle here
 * https://github.com/coding-horror/basic-computer-games/blob/main/25%20Chief/chief.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Chief {

    private enum GAME_STATE {
        STARTING,
        READY_TO_START,
        ENTER_NUMBER,
        CALCULATE_AND_SHOW,
        END_GAME,
        GAME_OVER
    }

    private GAME_STATE gameState;

    // The number the computer determines to be the players starting number
    private double calculatedNumber;

    // Used for keyboard input
    private final Scanner kbScanner;

    public Chief() {

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

                // Show an introduction the first time the game is played.
                case STARTING:
                    intro();
                    gameState = GAME_STATE.READY_TO_START;
                    break;

                // show an message to start
                case READY_TO_START:
                    if (!yesEntered(displayTextAndGetInput("ARE YOU READY TO TAKE THE TEST YOU CALLED ME OUT FOR? "))) {
                        System.out.println("SHUT UP, PALE FACE WITH WISE TONGUE.");
                    }

                    instructions();
                    gameState = GAME_STATE.ENTER_NUMBER;
                    break;

                // Enter the number to be used to calculate
                case ENTER_NUMBER:
                    double playerNumber = Double.parseDouble(
                            displayTextAndGetInput(" WHAT DO YOU HAVE? "));

                    // Exact same formula used in the original game to calculate the players original number
                    calculatedNumber = (playerNumber + 1 - 5) * 5 / 8 * 5 - 3;

                    gameState = GAME_STATE.CALCULATE_AND_SHOW;
                    break;

                // Enter the number to be used to calculate
                case CALCULATE_AND_SHOW:
                    if (yesEntered(
                            displayTextAndGetInput("I BET YOUR NUMBER WAS " + calculatedNumber
                                    + ". AM I RIGHT? "))) {
                        gameState = GAME_STATE.END_GAME;

                    } else {
                        // Player did not agree, so show the breakdown
                        double number = Double.parseDouble(
                                displayTextAndGetInput(" WHAT WAS YOUR ORIGINAL NUMBER? "));
                        double f = number + 3;
                        double g = f / 5;
                        double h = g * 8;
                        double i = h / 5 + 5;
                        double j = i - 1;
                        System.out.println("SO YOU THINK YOU'RE SO SMART, EH?");
                        System.out.println("NOW WATCH.");
                        System.out.println(number + " PLUS 3 EQUALS " + f + ". DIVIDED BY 5 EQUALS " + g);
                        System.out.println("TIMES 8 EQUALS " + h + ". IF WE DIVIDE BY 5 AND ADD 5,");
                        System.out.println("WE GET " + i + ", WHICH, MINUS 1, EQUALS " + j + ".");
                        if (yesEntered(displayTextAndGetInput("NOW DO YOU BELIEVE ME? "))) {
                            gameState = GAME_STATE.END_GAME;
                        } else {
                            // Time for a lightning bolt.
                            System.out.println("YOU HAVE MADE ME MAD!!!");
                            System.out.println("THERE MUST BE A GREAT LIGHTNING BOLT!");
                            System.out.println();
                            for (int x = 30; x >= 22; x--) {
                                System.out.println(tabbedSpaces(x) + "X X");
                            }
                            System.out.println(tabbedSpaces(21) + "X XXX");
                            System.out.println(tabbedSpaces(20) + "X   X");
                            System.out.println(tabbedSpaces(19) + "XX X");
                            for (int y = 20; y >= 13; y--) {
                                System.out.println(tabbedSpaces(y) + "X X");
                            }
                            System.out.println(tabbedSpaces(12) + "XX");
                            System.out.println(tabbedSpaces(11) + "X");
                            System.out.println(tabbedSpaces(10) + "*");
                            System.out.println();
                            System.out.println("#########################");
                            System.out.println();
                            System.out.println("I HOPE YOU BELIEVE ME NOW, FOR YOUR SAKE!!");
                            gameState = GAME_STATE.GAME_OVER;
                        }

                    }
                    break;

                // Sign off message for cases where the Chief is not upset
                case END_GAME:
                    System.out.println("BYE!!!");
                    gameState = GAME_STATE.GAME_OVER;
                    break;

                // GAME_OVER State does not specifically have a case
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    /**
     * Simulate tabs by building up a string of spaces
     *
     * @param spaces how many spaces are there to be
     * @return a string with the requested number of spaces
     */
    private String tabbedSpaces(int spaces) {
        char[] repeat = new char[spaces];
        Arrays.fill(repeat, ' ');
        return new String(repeat);
    }

    private void instructions() {
        System.out.println(" TAKE A NUMBER AND ADD 3. DIVIDE NUMBER BY 5 AND");
        System.out.println("MULTIPLY BY 8. DIVIDE BY 5 AND ADD THE SAME. SUBTRACT 1.");
    }

    /**
     * Basic information about the game
     */
    private void intro() {
        System.out.println("CHIEF");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("I AM CHIEF NUMBERS FREEK, THE GREAT INDIAN MATH GOD.");
    }

    /**
     * Returns true if a given string is equal to at least one of the values specified in the call
     * to the stringIsAnyValue method
     *
     * @param text string to search
     * @return true if string is equal to one of the varargs
     */
    private boolean yesEntered(String text) {
        return stringIsAnyValue(text, "Y", "YES");
    }

    /**
     * Returns true if a given string contains at least one of the varargs (2nd parameter).
     * Note: Case insensitive comparison.
     *
     * @param text   string to search
     * @param values varargs of type string containing values to compare
     * @return true if one of the varargs arguments was found in text
     */
    private boolean stringIsAnyValue(String text, String... values) {

        // Cycle through the variable number of values and test each
        for (String val : values) {
            if (text.equalsIgnoreCase(val)) {
                return true;
            }
        }

        // no matches
        return false;
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

}