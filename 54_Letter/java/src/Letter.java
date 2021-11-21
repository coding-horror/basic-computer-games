import java.awt.*;
import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Letter
 * <p>
 * Based on the Basic game of Letter here
 * https://github.com/coding-horror/basic-computer-games/blob/main/54%20Letter/letter.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Letter {

    public static final int OPTIMAL_GUESSES = 5;
    public static final int ASCII_A = 65;
    public static final int ALL_LETTERS = 26;

    private enum GAME_STATE {
        STARTUP,
        INIT,
        GUESSING,
        RESULTS,
        GAME_OVER
    }

    // Used for keyboard input
    private final Scanner kbScanner;

    // Current game state
    private GAME_STATE gameState;

    // Players guess count;
    private int playerGuesses;

    // Computers ascii code for a random letter between A..Z
    private int computersLetter;

    public Letter() {

        gameState = GAME_STATE.STARTUP;

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
                case STARTUP:
                    intro();
                    gameState = GAME_STATE.INIT;
                    break;

                case INIT:
                    playerGuesses = 0;
                    computersLetter = ASCII_A + (int) (Math.random() * ALL_LETTERS);
                    System.out.println("O.K., I HAVE A LETTER.  START GUESSING.");
                    gameState = GAME_STATE.GUESSING;
                    break;

                // Player guesses the number until they get it or run out of guesses
                case GUESSING:
                    String playerGuess = displayTextAndGetInput("WHAT IS YOUR GUESS? ").toUpperCase();

                    // Convert first character of input string to ascii
                    int toAscii = playerGuess.charAt(0);
                    playerGuesses++;
                    if (toAscii == computersLetter) {
                        gameState = GAME_STATE.RESULTS;
                        break;
                    }

                    if (toAscii > computersLetter) {
                        System.out.println("TOO HIGH.  TRY A LOWER LETTER.");
                    } else {
                        System.out.println("TOO LOW.  TRY A HIGHER LETTER.");
                    }
                    break;

                // Play again, or exit game?
                case RESULTS:
                    System.out.println();
                    System.out.println("YOU GOT IT IN " + playerGuesses + " GUESSES!!");
                    if (playerGuesses <= OPTIMAL_GUESSES) {
                        System.out.println("GOOD JOB !!!!!");
                        // Original game beeped 15 tims if you guessed in the optimal guesses or less
                        // Changed this to do a single beep only
                        Toolkit.getDefaultToolkit().beep();
                    } else {
                        // Took more than optimal number of guesses
                        System.out.println("BUT IT SHOULDN'T TAKE MORE THAN " + OPTIMAL_GUESSES + " GUESSES!");
                    }
                    System.out.println();
                    System.out.println("LET'S PLAN AGAIN.....");
                    gameState = GAME_STATE.INIT;
                    break;
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    public void intro() {
        System.out.println(simulateTabs(33) + "LETTER");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("LETTER GUESSING GAME");
        System.out.println();
        System.out.println("I'LL THINK OF A LETTER OF THE ALPHABET, A TO Z.");
        System.out.println("TRY TO GUESS MY LETTER AND I'LL GIVE YOU CLUES");
        System.out.println("AS TO HOW CLOSE YOU'RE GETTING TO MY LETTER.");
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