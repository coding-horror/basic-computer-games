import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Guess
 * <p>
 * Based on the Basic game of Guess here
 * https://github.com/coding-horror/basic-computer-games/blob/main/41%20Guess/guess.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Guess {

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        STARTUP,
        INPUT_RANGE,
        DEFINE_COMPUTERS_NUMBER,
        GUESS,
        GAME_OVER
    }

    // Current game state
    private GAME_STATE gameState;

    // User supplied maximum number to guess
    private int limit;

    // Computers calculated number for the player to guess

    private int computersNumber;

    // Number of turns the player has had guessing
    private int tries;

    // Optimal number of turns it should take to guess
    private int calculatedTurns;

    public Guess() {
        kbScanner = new Scanner(System.in);

        gameState = GAME_STATE.STARTUP;
    }

    /**
     * Main game loop
     */
    public void play() {

        do {
            switch (gameState) {

                case STARTUP:
                    intro();
                    gameState = GAME_STATE.INPUT_RANGE;
                    break;

                case INPUT_RANGE:

                    limit = displayTextAndGetNumber("WHAT LIMIT DO YOU WANT? ");
                    calculatedTurns = (int) (Math.log(limit) / Math.log(2)) + 1;
                    gameState = GAME_STATE.DEFINE_COMPUTERS_NUMBER;
                    break;

                case DEFINE_COMPUTERS_NUMBER:

                    tries = 1;
                    System.out.println("I'M THINKING OF A NUMBER BETWEEN 1 AND " + limit);
                    computersNumber = (int) (Math.random() * limit + 1);

                    gameState = GAME_STATE.GUESS;
                    break;

                case GUESS:
                    int playersGuess = displayTextAndGetNumber("NOW YOU TRY TO GUESS WHAT IT IS ");

                    // Allow player to restart game with entry of 0
                    if (playersGuess == 0) {
                        linePadding();
                        gameState = GAME_STATE.STARTUP;
                        break;
                    }

                    if (playersGuess == computersNumber) {
                        System.out.println("THAT'S IT! YOU GOT IT IN " + tries + " TRIES.");
                        if (tries < calculatedTurns) {
                            System.out.println("VERY ");
                        }
                        System.out.println("GOOD.");
                        System.out.println("YOU SHOULD HAVE BEEN ABLE TO GET IT IN ONLY " + calculatedTurns);
                        linePadding();
                        gameState = GAME_STATE.DEFINE_COMPUTERS_NUMBER;
                        break;
                    } else if (playersGuess < computersNumber) {
                        System.out.println("TOO LOW. TRY A BIGGER ANSWER.");
                    } else {
                        System.out.println("TOO HIGH. TRY A SMALLER ANSWER.");
                    }
                    tries++;
                    break;
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    private void intro() {
        System.out.println(simulateTabs(33) + "GUESS");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("THIS IS A NUMBER GUESSING GAME. I'LL THINK");
        System.out.println("OF A NUMBER BETWEEN 1 AND ANY LIMIT YOU WANT.");
        System.out.println("THEN YOU HAVE TO GUESS WHAT IT IS.");
    }

    /**
     * Print a predefined number of blank lines
     *
     */
    private void linePadding() {
        for (int i = 1; i <= 5; i++) {
            System.out.println();
        }
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
