import java.util.Scanner;

/**
 * Game of HiLo
 *
 * Based on the Basic game of Hi-Lo here
 * https://github.com/coding-horror/basic-computer-games/blob/main/47%20Hi-Lo/hi-lo.bas
 *
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 *        new features - no additional text, error checking, etc has been added.
 */
public class HiLo {

    public static final int LOW_NUMBER_RANGE = 1;
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

    // Players Winnings
    private int playerAmountWon;

    // Players guess count;
    private int playersGuesses;

    // Computers random number
    private int computersNumber;

    public HiLo() {

        gameState = GAME_STATE.STARTING;
        playerAmountWon = 0;

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
                    gameState = GAME_STATE.START_GAME;
                    break;

                // Generate computers number for player to guess, etc.
                case START_GAME:
                    init();
                    System.out.println("O.K.  I HAVE A NUMBER IN MIND.");
                    gameState = GAME_STATE.GUESSING;
                    break;

                // Player guesses the number until they get it or run out of guesses
                case GUESSING:
                    int guess = playerGuess();

                    // Check if the player guessed the number
                    if(validateGuess(guess)) {
                        System.out.println("GOT IT!!!!!!!!!!   YOU WIN " + computersNumber
                                + " DOLLARS.");
                        playerAmountWon += computersNumber;
                        System.out.println("YOUR TOTAL WINNINGS ARE NOW "
                                + playerAmountWon + " DOLLARS.");
                        gameState = GAME_STATE.PLAY_AGAIN;
                    } else {
                        // incorrect guess
                        playersGuesses++;
                        // Ran out of guesses?
                        if (playersGuesses == MAX_GUESSES) {
                            System.out.println("YOU BLEW IT...TOO BAD...THE NUMBER WAS "
                                    + computersNumber);
                            playerAmountWon = 0;
                            gameState = GAME_STATE.PLAY_AGAIN;
                        }
                    }
                    break;

                // Play again, or exit game?
                case PLAY_AGAIN:
                    System.out.println();
                    if(yesEntered(displayTextAndGetInput("PLAY AGAIN (YES OR NO) "))) {
                        gameState = GAME_STATE.START_GAME;
                    } else {
                        // Chose not to play again
                        System.out.println("SO LONG.  HOPE YOU ENJOYED YOURSELF!!!");
                        gameState = GAME_STATE.GAME_OVER;
                    }
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    /**
     * Checks the players guess against the computers randomly generated number
     *
     * @param theGuess the players guess
     * @return true if the player guessed correctly, false otherwise
     */
    private boolean validateGuess(int theGuess) {

        // Correct guess?
        if(theGuess == computersNumber) {
            return true;
        }

        if(theGuess > computersNumber) {
            System.out.println("YOUR GUESS IS TOO HIGH.");
        } else {
            System.out.println("YOUR GUESS IS TOO LOW.");
        }

        return false;
    }

    private void init() {
        playersGuesses = 0;
        computersNumber = randomNumber();
    }

    public void intro() {
        System.out.println("HI LO");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println();
        System.out.println("IS THE GAME OF HI LO.");
        System.out.println();
        System.out.println("YOU WILL HAVE 6 TRIES TO GUESS THE AMOUNT OF MONEY IN THE");
        System.out.println("HI LO JACKPOT, WHICH IS BETWEEN 1 AND 100 DOLLARS.  IF YOU");
        System.out.println("GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!");
        System.out.println("THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY.  HOWEVER,");
        System.out.println("IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS.");
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
     * Used as a single digit of the computer player
     *
     * @return random number
     */
    private int randomNumber() {
        return (int) (Math.random()
                * (HIGH_NUMBER_RANGE - LOW_NUMBER_RANGE + 1) + LOW_NUMBER_RANGE);
    }
}