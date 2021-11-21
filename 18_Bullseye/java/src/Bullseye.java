import java.util.ArrayList;
import java.util.Scanner;

/**
 * Game of Bullseye
 * <p>
 * Based on the Basic game of Bullseye here
 * https://github.com/coding-horror/basic-computer-games/blob/main/18%20Bullseye/bullseye.bas
 * <p>
 * Note:  The idea was to create a version of 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Bullseye {

    // Used for formatting output
    public static final int FIRST_IDENT = 10;
    public static final int SECOND_IDENT = 30;
    public static final int THIRD_INDENT = 30;

    // Used to decide throw result
    public static final double[] SHOT_ONE = new double[]{.65, .55, .5, .5};
    public static final double[] SHOT_TWO = new double[]{.99, .77, .43, .01};
    public static final double[] SHOT_THREE = new double[]{.95, .75, .45, .05};

    private enum GAME_STATE {
        STARTING,
        START_GAME,
        PLAYING,
        GAME_OVER
    }

    private GAME_STATE gameState;

    private final ArrayList<Player> players;

    private final Shot[] shots;

    // Used for keyboard input
    private final Scanner kbScanner;

    private int round;

    public Bullseye() {

        gameState = GAME_STATE.STARTING;
        players = new ArrayList<>();

        // Save the random chances of points based on shot type

        shots = new Shot[3];
        shots[0] = new Shot(SHOT_ONE);
        shots[1] = new Shot(SHOT_TWO);
        shots[2] = new Shot(SHOT_THREE);

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
                    gameState = GAME_STATE.START_GAME;
                    break;

                // Start the game, set the number of players, names and round
                case START_GAME:

                    int numberOfPlayers = chooseNumberOfPlayers();

                    for (int i = 0; i < numberOfPlayers; i++) {
                        String name = displayTextAndGetInput("NAME OF PLAYER #" + (i + 1) + "? ");
                        Player player = new Player(name);
                        this.players.add(player);
                    }

                    this.round = 1;

                    gameState = GAME_STATE.PLAYING;
                    break;

                // Playing round by round until we have a winner
                case PLAYING:
                    System.out.println();
                    System.out.println("ROUND " + this.round);
                    System.out.println("=======");

                    // Each player takes their turn
                    for (Player player : players) {
                        int playerThrow = getPlayersThrow(player);
                        int points = calculatePlayerPoints(playerThrow);
                        player.addScore(points);
                        System.out.println("TOTAL SCORE = " + player.getScore());
                    }

                    boolean foundWinner = false;

                    // Check if any player won
                    for (Player thePlayer : players) {
                        int score = thePlayer.getScore();
                        if (score >= 200) {
                            if (!foundWinner) {
                                System.out.println("WE HAVE A WINNER!!");
                                System.out.println();
                                foundWinner = true;
                            }
                            System.out.println(thePlayer.getName() + " SCORED "
                                    + thePlayer.getScore() + " POINTS");
                        }
                    }

                    if (foundWinner) {
                        System.out.println("THANKS FOR THE GAME.");
                        gameState = GAME_STATE.GAME_OVER;
                    } else {
                        // No winner found, continue on with the next round
                        this.round++;
                    }

                    break;
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    /**
     * Display info about the game
     */
    private void intro() {
        System.out.println("BULLSEYE");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("IN THIS GAME, UP TO 20 PLAYERS THROW DARTS AT A TARGET");
        System.out.println("WITH 10, 20, 30, AND 40 POINT ZONES.  THE OBJECTIVE IS");
        System.out.println("TO GET 200 POINTS.");
        System.out.println();
        System.out.println(paddedString("THROW", "DESCRIPTION", "PROBABLE SCORE"));
        System.out.println(paddedString("1", "FAST OVERARM", "BULLSEYE OR COMPLETE MISS"));
        System.out.println(paddedString("2", "CONTROLLED OVERARM", "10, 20 OR 30 POINTS"));
        System.out.println(paddedString("3", "UNDERARM", "ANYTHING"));
    }

    /**
     * Calculate the players score
     * Score is based on the type of shot plus a random factor
     *
     * @param playerThrow 1,2, or 3 indicating the type of shot
     * @return player score
     */
    private int calculatePlayerPoints(int playerThrow) {

        // -1 is because of 0 base Java array
        double p1 = this.shots[playerThrow - 1].getShot(0);
        double p2 = this.shots[playerThrow - 1].getShot(1);
        double p3 = this.shots[playerThrow - 1].getShot(2);
        double p4 = this.shots[playerThrow - 1].getShot(3);

        double random = Math.random();

        int points;

        if (random >= p1) {
            System.out.println("BULLSEYE!!  40 POINTS!");
            points = 40;
            // If the throw was 1 (bullseye or missed, then make it missed
            // N.B. This is a fix for the basic code which for shot type 1
            // allowed a bullseye but did not make the score zero if a bullseye
            // was not made (which it should have done).
        } else if (playerThrow == 1) {
            System.out.println("MISSED THE TARGET!  TOO BAD.");
            points = 0;
        } else if (random >= p2) {
            System.out.println("30-POINT ZONE!");
            points = 30;
        } else if (random >= p3) {
            System.out.println("20-POINT ZONE");
            points = 20;
        } else if (random >= p4) {
            System.out.println("WHEW!  10 POINTS.");
            points = 10;
        } else {
            System.out.println("MISSED THE TARGET!  TOO BAD.");
            points = 0;
        }

        return points;
    }

    /**
     * Get players shot 1,2, or 3 - ask again if invalid input
     *
     * @param player the player we are calculating the throw on
     * @return 1, 2, or 3 indicating the players shot
     */
    private int getPlayersThrow(Player player) {
        boolean inputCorrect = false;
        String theThrow;
        do {
            theThrow = displayTextAndGetInput(player.getName() + "'S THROW ");
            if (theThrow.equals("1") || theThrow.equals("2") || theThrow.equals("3")) {
                inputCorrect = true;
            } else {
                System.out.println("INPUT 1, 2, OR 3!");
            }

        } while (!inputCorrect);

        return Integer.parseInt(theThrow);
    }


    /**
     * Get players guess from kb
     *
     * @return players guess as an int
     */
    private int chooseNumberOfPlayers() {

        return Integer.parseInt((displayTextAndGetInput("HOW MANY PLAYERS? ")));
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
     * Format three strings to a given number of spaces
     * Replacing the original basic code which used tabs
     *
     * @param first  String to print in pos 1
     * @param second String to print in pos 2
     * @param third  String to print in pos 3
     * @return formatted string
     */
    private String paddedString(String first, String second, String third) {
        String output = String.format("%1$" + FIRST_IDENT + "s", first);
        output += String.format("%1$" + SECOND_IDENT + "s", second);
        output += String.format("%1$" + THIRD_INDENT + "s", third);
        return output;
    }
}
