import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of BatNum
 * <p>
 * Based on the Basic game of BatNum here
 * https://github.com/coding-horror/basic-computer-games/blob/main/08%20Batnum/batnum.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class BatNum {

    private enum GAME_STATE {
        STARTING,
        START_GAME,
        CHOOSE_PILE_SIZE,
        SELECT_WIN_OPTION,
        CHOOSE_MIN_AND_MAX,
        SELECT_WHO_STARTS_FIRST,
        PLAYERS_TURN,
        COMPUTERS_TURN,
        ANNOUNCE_WINNER,
        GAME_OVER
    }

    // Used for keyboard input
    private final Scanner kbScanner;

    // Current game state
    private GAME_STATE gameState;

    private int pileSize;

    // How to win the game options
    enum WIN_OPTION {
        TAKE_LAST,
        AVOID_LAST
    }

    // Tracking the winner
    enum WINNER {
        COMPUTER,
        PLAYER
    }

    private WINNER winner;

    private WIN_OPTION winOption;

    private int minSelection;
    private int maxSelection;

    // Used by computer for optimal move
    private int rangeOfRemovals;

    public BatNum() {

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
                    gameState = GAME_STATE.START_GAME;
                    break;

                // Start new game
                case START_GAME:
                    gameState = GAME_STATE.CHOOSE_PILE_SIZE;
                    break;

                case CHOOSE_PILE_SIZE:
                    System.out.println();
                    System.out.println();
                    pileSize = displayTextAndGetNumber("ENTER PILE SIZE ");
                    if (pileSize >= 1) {
                        gameState = GAME_STATE.SELECT_WIN_OPTION;
                    }
                    break;

                case SELECT_WIN_OPTION:
                    int winChoice = displayTextAndGetNumber("ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST: ");
                    if (winChoice == 1) {
                        winOption = WIN_OPTION.TAKE_LAST;
                        gameState = GAME_STATE.CHOOSE_MIN_AND_MAX;
                    } else if (winChoice == 2) {
                        winOption = WIN_OPTION.AVOID_LAST;
                        gameState = GAME_STATE.CHOOSE_MIN_AND_MAX;
                    }
                    break;

                case CHOOSE_MIN_AND_MAX:
                    String range = displayTextAndGetInput("ENTER MIN AND MAX ");
                    minSelection = getDelimitedValue(range, 0);
                    maxSelection = getDelimitedValue(range, 1);
                    if (maxSelection > minSelection && minSelection >= 1) {
                        gameState = GAME_STATE.SELECT_WHO_STARTS_FIRST;
                    }

                    // Used by computer in its turn
                    rangeOfRemovals = minSelection + maxSelection;
                    break;

                case SELECT_WHO_STARTS_FIRST:
                    int playFirstChoice = displayTextAndGetNumber("ENTER START OPTION - 1 COMPUTER FIRST, 2 YOU FIRST ");
                    if (playFirstChoice == 1) {
                        gameState = GAME_STATE.COMPUTERS_TURN;
                    } else if (playFirstChoice == 2) {
                        gameState = GAME_STATE.PLAYERS_TURN;
                    }
                    break;

                case PLAYERS_TURN:
                    int playersMove = displayTextAndGetNumber("YOUR MOVE ");

                    if (playersMove == 0) {
                        System.out.println("I TOLD YOU NOT TO USE ZERO! COMPUTER WINS BY FORFEIT.");
                        winner = WINNER.COMPUTER;
                        gameState = GAME_STATE.ANNOUNCE_WINNER;
                        break;
                    }

                    if (playersMove == pileSize && winOption == WIN_OPTION.AVOID_LAST) {
                        winner = WINNER.COMPUTER;
                        gameState = GAME_STATE.ANNOUNCE_WINNER;
                        break;
                    }

                    // Check if players move is with the min and max possible
                    if (playersMove >= minSelection && playersMove <= maxSelection) {
                        // Valid so reduce pileSize by amount player entered
                        pileSize -= playersMove;

                        // Did this move result in there being no more objects on pile?
                        if (pileSize == 0) {
                            // Was the game setup so the winner was whoever took the last object
                            if (winOption == WIN_OPTION.TAKE_LAST) {
                                // Player won
                                winner = WINNER.PLAYER;
                            } else {
                                // Computer one
                                winner = WINNER.COMPUTER;
                            }
                            gameState = GAME_STATE.ANNOUNCE_WINNER;
                        } else {
                            // There are still items left.
                            gameState = GAME_STATE.COMPUTERS_TURN;
                        }
                    } else {
                        // Invalid move
                        System.out.println("ILLEGAL MOVE, REENTER IT ");
                    }
                    break;

                case COMPUTERS_TURN:
                    int pileSizeLeft = pileSize;
                    if (winOption == WIN_OPTION.TAKE_LAST) {
                        if (pileSize > maxSelection) {

                            int objectsToRemove = calculateComputersTurn(pileSizeLeft);

                            pileSize -= objectsToRemove;
                            System.out.println("COMPUTER TAKES " + objectsToRemove + " AND LEAVES " + pileSize);
                            gameState = GAME_STATE.PLAYERS_TURN;
                        } else {
                            System.out.println("COMPUTER TAKES " + pileSize + " AND WINS.");
                            winner = WINNER.COMPUTER;
                            gameState = GAME_STATE.ANNOUNCE_WINNER;
                        }
                    } else {
                        pileSizeLeft--;
                        if (pileSize > minSelection) {
                            int objectsToRemove = calculateComputersTurn(pileSizeLeft);
                            pileSize -= objectsToRemove;
                            System.out.println("COMPUTER TAKES " + objectsToRemove + " AND LEAVES " + pileSize);
                            gameState = GAME_STATE.PLAYERS_TURN;
                        } else {
                            System.out.println("COMPUTER TAKES " + pileSize + " AND LOSES.");
                            winner = WINNER.PLAYER;
                            gameState = GAME_STATE.ANNOUNCE_WINNER;
                        }
                    }
                    break;

                case ANNOUNCE_WINNER:
                    switch (winner) {
                        case PLAYER:
                            System.out.println("CONGRATULATIONS, YOU WIN.");
                            break;
                        case COMPUTER:
                            System.out.println("TOUGH LUCK, YOU LOSE.");
                            break;
                    }
                    gameState = GAME_STATE.START_GAME;
                    break;
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    /**
     * Figure out the computers turn - i.e. how many objects to remove
     *
     * @param pileSizeLeft current size
     * @return the number of objects to remove.
     */
    private int calculateComputersTurn(int pileSizeLeft) {
        int computersNumberToRemove = pileSizeLeft - rangeOfRemovals * (pileSizeLeft / rangeOfRemovals);
        if (computersNumberToRemove < minSelection) {
            computersNumberToRemove = minSelection;
        }
        if (computersNumberToRemove > maxSelection) {
            computersNumberToRemove = maxSelection;
        }

        return computersNumberToRemove;
    }

    private void intro() {
        System.out.println(simulateTabs(33) + "BATNUM");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("THIS PROGRAM IS A 'BATTLE OF NUMBERS' GAME, WHERE THE");
        System.out.println("COMPUTER IS YOUR OPPONENT.");
        System.out.println();
        System.out.println("THE GAME STARTS WITH AN ASSUMED PILE OF OBJECTS. YOU");
        System.out.println("AND YOUR OPPONENT ALTERNATELY REMOVE OBJECTS FROM THE PILE.");
        System.out.println("WINNING IS DEFINED IN ADVANCE AS TAKING THE LAST OBJECT OR");
        System.out.println("NOT. YOU CAN ALSO SPECIFY SOME OTHER BEGINNING CONDITIONS.");
        System.out.println("DON'T USE ZERO, HOWEVER, IN PLAYING THE GAME.");
        System.out.println("ENTER A NEGATIVE NUMBER FOR NEW PILE SIZE TO STOP PLAYING.");
    }

    /*
     * Print a message on the screen, then accept input from Keyboard.
     * Converts input to Integer
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
}