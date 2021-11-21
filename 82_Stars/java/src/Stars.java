import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Stars
 *
 * Based on the Basic game of Stars here
 * https://github.com/coding-horror/basic-computer-games/blob/main/82%20Stars/stars.bas
 *
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 *        new features - no additional text, error checking, etc has been added.
 */
public class Stars {

    public static final int HIGH_NUMBER_RANGE = 100;
    public static final int MAX_GUESSES = 7;

    private enum GAME_STATE {
        STARTING,
        INSTRUCTIONS,
        START_GAME,
        GUESSING,
        WON,
        LOST,
        GAME_OVER
    }

    // Used for keyboard input
    private final Scanner kbScanner;

    // Current game state
    private GAME_STATE gameState;

    // Players guess count;
    private int playerTotalGuesses;

    // Players current guess
    private int playerCurrentGuess;

    // Computers random number
    private int computersNumber;

    public Stars() {

        gameState = GAME_STATE.STARTING;

        // Initialise kb scanner
        kbScanner = new Scanner(System.in);
    }

    /**
     * Main game loop
     *
     */
    public void play() {

        do {
            switch (gameState) {

                // Show an introduction the first time the game is played.
                case STARTING:
                    intro();
                    gameState = GAME_STATE.INSTRUCTIONS;
                    break;

                // Ask if instructions are needed and display if yes
                case INSTRUCTIONS:
                    if(yesEntered(displayTextAndGetInput("DO YOU WANT INSTRUCTIONS? "))) {
                        instructions();
                    }
                    gameState = GAME_STATE.START_GAME;
                    break;

                // Generate computers number for player to guess, etc.
                case START_GAME:
                    init();
                    System.out.println("OK, I AM THINKING OF A NUMBER, START GUESSING.");
                    gameState = GAME_STATE.GUESSING;
                    break;

                // Player guesses the number until they get it or run out of guesses
                case GUESSING:
                    playerCurrentGuess = playerGuess();

                    // Check if the player guessed the number
                    if(playerCurrentGuess == computersNumber) {
                        gameState = GAME_STATE.WON;
                    } else {
                        // incorrect guess
                        showStars();
                        playerTotalGuesses++;
                        // Ran out of guesses?
                        if (playerTotalGuesses > MAX_GUESSES) {
                            gameState = GAME_STATE.LOST;
                        }
                    }
                    break;

                // Won game.
                case WON:

                    System.out.println(stars(79));
                    System.out.println("YOU GOT IT IN " + playerTotalGuesses
                            + " GUESSES!!!  LET'S PLAY AGAIN...");
                    gameState = GAME_STATE.START_GAME;
                    break;

                // Lost game by running out of guesses
                case LOST:
                    System.out.println("SORRY, THAT'S " + MAX_GUESSES
                            + " GUESSES. THE NUMBER WAS " + computersNumber);
                    gameState = GAME_STATE.START_GAME;
                    break;
            }
            // Endless loop since the original code did not allow the player to exit
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    /**
     * Shows how close a players guess is to the computers number by
     * showing a series of stars - the more stars the closer to the
     * number.
     *
     */
    private void showStars() {
        int d = Math.abs(playerCurrentGuess - computersNumber);
        int starsToShow;
        if(d >=64) {
            starsToShow = 1;
        } else if(d >=32) {
            starsToShow = 2;
        } else if (d >= 16) {
            starsToShow = 3;
        } else if (d >=8) {
            starsToShow = 4;
        } else if( d>= 4) {
            starsToShow = 5;
        } else if(d>= 2) {
            starsToShow = 6;
        } else {
            starsToShow = 7;
        }
        System.out.println(stars(starsToShow));
    }

    /**
     * Show a number of stars (asterisks)
     * @param number the number of stars needed
     * @return the string encoded with the number of required stars
     */
    private String stars(int number) {
        char[] stars = new char[number];
        Arrays.fill(stars, '*');
        return new String(stars);
    }

    /**
     * Initialise variables before each new game
     *
     */
    private void init() {
        playerTotalGuesses = 1;
        computersNumber = randomNumber();
    }

    public void instructions() {
        System.out.println("I AM THINKING OF A WHOLE NUMBER FROM 1 TO " + HIGH_NUMBER_RANGE);
        System.out.println("TRY TO GUESS MY NUMBER.  AFTER YOU GUESS, I");
        System.out.println("WILL TYPE ONE OR MORE STARS (*).  THE MORE");
        System.out.println("STARS I TYPE, THE CLOSER YOU ARE TO MY NUMBER.");
        System.out.println("ONE STAR (*) MEANS FAR AWAY, SEVEN STARS (*******)");
        System.out.println("MEANS REALLY CLOSE!  YOU GET " + MAX_GUESSES + " GUESSES.");
    }

    public void intro() {
        System.out.println("STARS");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
    }

    /**
     * Get players guess from kb
     *
     * @return players guess as an int
     */
    private int playerGuess() {
        return Integer.parseInt((displayTextAndGetInput("YOUR GUESS? ")));
    }

    /**
     * Checks whether player entered Y or YES to a question.
     *
     * @param text  player string from kb
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
     * @param text source string
     * @param values a range of values to compare against the source string
     * @return true if a comparison was found in one of the variable number of strings passed
     */
    private boolean stringIsAnyValue(String text, String... values) {

        // Cycle through the variable number of values and test each
        for(String val:values) {
            if(text.equalsIgnoreCase(val)) {
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

    /**
     * Generate random number
     *
     * @return random number
     */
    private int randomNumber() {
        return (int) (Math.random()
                * (HIGH_NUMBER_RANGE) + 1);
    }
}