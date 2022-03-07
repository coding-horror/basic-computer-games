import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Rock Scissors Paper
 * <p>
 * Based on the Basic game of Rock Scissors here
 * https://github.com/coding-horror/basic-computer-games/blob/main/74%20Rock%20Scissors%20Paper/rockscissors.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */

public class RockScissors {

    public static final int MAX_GAMES = 10;

    public static final int PAPER = 1;
    public static final int SCISSORS = 2;
    public static final int ROCK = 3;

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        START_GAME,
        GET_NUMBER_GAMES,
        START_ROUND,
        PLAY_ROUND,
        GAME_RESULT,
        GAME_OVER
    }

    // Current game state
    private GAME_STATE gameState;

    private enum WINNER {
        COMPUTER,
        PLAYER
    }

    private WINNER gameWinner;

    int playerWins;
    int computerWins;
    int numberOfGames;
    int currentGameCount;
    int computersChoice;

    public RockScissors() {
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
                    currentGameCount = 0;
                    gameState = GAME_STATE.GET_NUMBER_GAMES;

                    break;

                case GET_NUMBER_GAMES:
                    numberOfGames = displayTextAndGetNumber("HOW MANY GAMES? ");
                    if (numberOfGames <= MAX_GAMES) {
                        gameState = GAME_STATE.START_ROUND;
                    } else {
                        System.out.println("SORRY, BUT WE AREN'T ALLOWED TO PLAY THAT MANY.");
                    }
                    break;

                case START_ROUND:
                    currentGameCount++;
                    if (currentGameCount > numberOfGames) {
                        gameState = GAME_STATE.GAME_RESULT;
                        break;
                    }
                    System.out.println("GAME NUMBER: " + (currentGameCount));
                    computersChoice = (int) (Math.random() * 3) + 1;
                    gameState = GAME_STATE.PLAY_ROUND;

                case PLAY_ROUND:
                    System.out.println("3=ROCK...2=SCISSORS...1=PAPER");
                    int playersChoice = displayTextAndGetNumber("1...2...3...WHAT'S YOUR CHOICE? ");
                    if (playersChoice >= PAPER && playersChoice <= ROCK) {
                        switch (computersChoice) {
                            case PAPER:
                                System.out.println("...PAPER");
                                break;
                            case SCISSORS:
                                System.out.println("...SCISSORS");
                                break;
                            case ROCK:
                                System.out.println("...ROCK");
                                break;
                        }

                        if (playersChoice == computersChoice) {
                            System.out.println("TIE GAME.  NO WINNER.");
                        } else {
                            switch (playersChoice) {
                                case PAPER:
                                    if (computersChoice == SCISSORS) {
                                        gameWinner = WINNER.COMPUTER;
                                    } else if (computersChoice == ROCK) {
                                        // Don't need to re-assign here, as its initialized to
                                        // false I'd argue this aids readability.
                                        gameWinner = WINNER.PLAYER;
                                    }
                                    break;
                                case SCISSORS:
                                    if (computersChoice == ROCK) {
                                        gameWinner = WINNER.COMPUTER;
                                    } else if (computersChoice == PAPER) {
                                        // Don't need to re-assign here, as its initialized to
                                        // false I'd argue this aids readability.
                                        gameWinner = WINNER.PLAYER;
                                    }
                                    break;
                                case ROCK:
                                    if (computersChoice == PAPER) {
                                        gameWinner = WINNER.COMPUTER;
                                    } else if (computersChoice == SCISSORS) {
                                        // Don't need to re-assign here, as its initialized to
                                        // false I'd argue this aids readability.
                                        gameWinner = WINNER.PLAYER;
                                    }
                                    break;
                            }

                            if (gameWinner == WINNER.COMPUTER) {
                                System.out.println("WOW!  I WIN!!!");
                                computerWins++;
                            } else {
                                System.out.println("YOU WIN!!!");
                                playerWins++;
                            }
                        }
                        gameState = GAME_STATE.START_ROUND;
                    } else {
                        System.out.println("INVALID.");
                    }

                    break;

                case GAME_RESULT:
                    System.out.println();
                    System.out.println("HERE IS THE FINAL GAME SCORE:");
                    System.out.println("I HAVE WON " + computerWins + " GAME" + (computerWins != 1 ? "S." : "."));
                    System.out.println("YOU HAVE WON " + playerWins + " GAME" + (playerWins != 1 ? "S." : "."));
                    int tiedGames = numberOfGames - (computerWins + playerWins);
                    System.out.println("AND " + tiedGames + " GAME" + (tiedGames != 1 ? "S " : " ") + "ENDED IN A TIE.");
                    System.out.println();
                    System.out.println("THANKS FOR PLAYING!!");
                    gameState = GAME_STATE.GAME_OVER;
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    private void intro() {
        System.out.println(addSpaces(21) + "GAME OF ROCK, SCISSORS, PAPER");
        System.out.println(addSpaces(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
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

    public static void main(String[] args) {

        RockScissors rockScissors = new RockScissors();
        rockScissors.play();
    }
}
