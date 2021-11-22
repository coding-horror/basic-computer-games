import java.util.ArrayList;
import java.util.Scanner;

/**
 * Game of Bug
 * <p>
 * Based on the Basic game of Bug here
 * https://github.com/coding-horror/basic-computer-games/blob/main/16%20Bug/bug.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Bug {

    // Dice roll
    public static final int SIX = 6;

    private enum GAME_STATE {
        START,
        PLAYER_TURN,
        COMPUTER_TURN,
        CHECK_FOR_WINNER,
        GAME_OVER
    }

    // Used for keyboard input
    private final Scanner kbScanner;

    // Current game state
    private GAME_STATE gameState;


    private final Insect playersBug;

    private final Insect computersBug;

    // Used to show the result of dice roll.
    private final String[] ROLLS = new String[]{"BODY", "NECK", "HEAD", "FEELERS", "TAIL", "LEGS"};

    public Bug() {

        playersBug = new PlayerBug();
        computersBug = new ComputerBug();

        gameState = GAME_STATE.START;

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
                // And optionally instructions.
                case START:
                    intro();
                    if (!noEntered(displayTextAndGetInput("DO YOU WANT INSTRUCTIONS? "))) {
                        instructions();
                    }

                    gameState = GAME_STATE.PLAYER_TURN;
                    break;

                case PLAYER_TURN:
                    int playersRoll = randomNumber();
                    System.out.println("YOU ROLLED A " + playersRoll + "=" + ROLLS[playersRoll - 1]);
                    switch (playersRoll) {
                        case 1:
                            System.out.println(playersBug.addBody());
                            break;
                        case 2:
                            System.out.println(playersBug.addNeck());
                            break;
                        case 3:
                            System.out.println(playersBug.addHead());
                            break;
                        case 4:
                            System.out.println(playersBug.addFeelers());
                            break;
                        case 5:
                            System.out.println(playersBug.addTail());
                            break;
                        case 6:
                            System.out.println(playersBug.addLeg());
                            break;
                    }

                    gameState = GAME_STATE.COMPUTER_TURN;
                    break;

                case COMPUTER_TURN:
                    int computersRoll = randomNumber();
                    System.out.println("I ROLLED A " + computersRoll + "=" + ROLLS[computersRoll - 1]);
                    switch (computersRoll) {
                        case 1:
                            System.out.println(computersBug.addBody());
                            break;
                        case 2:
                            System.out.println(computersBug.addNeck());
                            break;
                        case 3:
                            System.out.println(computersBug.addHead());
                            break;
                        case 4:
                            System.out.println(computersBug.addFeelers());
                            break;
                        case 5:
                            System.out.println(computersBug.addTail());
                            break;
                        case 6:
                            System.out.println(computersBug.addLeg());
                            break;
                    }

                    gameState = GAME_STATE.CHECK_FOR_WINNER;
                    break;

                case CHECK_FOR_WINNER:
                    boolean gameOver = false;

                    if (playersBug.complete()) {
                        System.out.println("YOUR BUG IS FINISHED.");
                        gameOver = true;
                    } else if (computersBug.complete()) {
                        System.out.println("MY BUG IS FINISHED.");
                        gameOver = true;
                    }

                    if (noEntered(displayTextAndGetInput("DO YOU WANT THE PICTURES? "))) {
                        gameState = GAME_STATE.PLAYER_TURN;
                    } else {
                        System.out.println("*****YOUR BUG*****");
                        System.out.println();
                        draw(playersBug);

                        System.out.println();
                        System.out.println("*****MY BUG*****");
                        System.out.println();
                        draw(computersBug);
                        gameState = GAME_STATE.PLAYER_TURN;
                    }
                    if (gameOver) {
                        System.out.println("I HOPE YOU ENJOYED THE GAME, PLAY IT AGAIN SOON!!");
                        gameState = GAME_STATE.GAME_OVER;
                    }
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    /**
     * Draw the bug (player or computer) based on what has
     * already been added to it.
     *
     * @param bug The bug to be drawn.
     */
    private void draw(Insect bug) {
        ArrayList<String> insectOutput = bug.draw();
        for (String s : insectOutput) {
            System.out.println(s);
        }
    }

    /**
     * Display an intro
     */
    private void intro() {
        System.out.println("BUG");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("THE GAME BUG");
        System.out.println("I HOPE YOU ENJOY THIS GAME.");
    }

    private void instructions() {
        System.out.println("THE OBJECT OF BUG IS TO FINISH YOUR BUG BEFORE I FINISH");
        System.out.println("MINE. EACH NUMBER STANDS FOR A PART OF THE BUG BODY.");
        System.out.println("I WILL ROLL THE DIE FOR YOU, TELL YOU WHAT I ROLLED FOR YOU");
        System.out.println("WHAT THE NUMBER STANDS FOR, AND IF YOU CAN GET THE PART.");
        System.out.println("IF YOU CAN GET THE PART I WILL GIVE IT TO YOU.");
        System.out.println("THE SAME WILL HAPPEN ON MY TURN.");
        System.out.println("IF THERE IS A CHANGE IN EITHER BUG I WILL GIVE YOU THE");
        System.out.println("OPTION OF SEEING THE PICTURES OF THE BUGS.");
        System.out.println("THE NUMBERS STAND FOR PARTS AS FOLLOWS:");
        System.out.println("NUMBER\tPART\tNUMBER OF PART NEEDED");
        System.out.println("1\tBODY\t1");
        System.out.println("2\tNECK\t1");
        System.out.println("3\tHEAD\t1");
        System.out.println("4\tFEELERS\t2");
        System.out.println("5\tTAIL\t1");
        System.out.println("6\tLEGS\t6");
        System.out.println();

    }

    /**
     * Checks whether player entered N or NO to a question.
     *
     * @param text player string from kb
     * @return true if N or NO was entered, otherwise false
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

    /**
     * Generate random number
     *
     * @return random number
     */
    private int randomNumber() {
        return (int) (Math.random()
                * (SIX) + 1);
    }
}