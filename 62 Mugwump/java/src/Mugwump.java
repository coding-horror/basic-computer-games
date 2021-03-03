import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Mugwump
 * <p>
 * Based on the Basic game of Mugwump here
 * https://github.com/coding-horror/basic-computer-games/blob/main/62%20Mugwump/mugwump.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */

public class Mugwump {

    public static final int NUMBER_OF_MUGWUMPS = 4;

    public static final int MAX_TURNS = 10;

    public static final int FOUND = -1;

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        INIT,
        GAME_START,
        PLAY_TURN
    }

    // Current game state
    private GAME_STATE gameState;

    int[][] mugwumpLocations;

    int turn;

    public Mugwump() {
        kbScanner = new Scanner(System.in);
        gameState = GAME_STATE.INIT;
    }

    /**
     * Main game loop
     */
    public void play() {

        do {
            switch (gameState) {

                case INIT:
                    intro();
                    gameState = GAME_STATE.GAME_START;

                    break;

                case GAME_START:

                    turn = 0;

                    // initialise all array elements with 0
                    mugwumpLocations = new int[NUMBER_OF_MUGWUMPS][2];

                    // Place 4 mugwumps
                    for (int i = 0; i < NUMBER_OF_MUGWUMPS; i++) {
                        for (int j = 0; j < 2; j++) {
                            mugwumpLocations[i][j] = (int) (Math.random() * 10);
                        }
                    }
                    gameState = GAME_STATE.PLAY_TURN;
                    break;

                case PLAY_TURN:
                    turn++;
                    String locations = displayTextAndGetInput("TURN NO." + turn + " -- WHAT IS YOUR GUESS? ");
                    int distanceRightGuess = getDelimitedValue(locations, 0);
                    int distanceUpGuess = getDelimitedValue(locations, 1);

                    int numberFound = 0;
                    for (int i = 0; i < NUMBER_OF_MUGWUMPS; i++) {

                        if (mugwumpLocations[i][0] == FOUND) {
                            numberFound++;
                        }

                        int right = mugwumpLocations[i][0];
                        int up = mugwumpLocations[i][1];

                        if (right == distanceRightGuess && up == distanceUpGuess) {
                            if (right != FOUND) {
                                System.out.println("YOU HAVE FOUND MUGWUMP " + (i + 1));
                                mugwumpLocations[i][0] = FOUND;
                            }
                            numberFound++;
                        } else {
                            // Not found so show distance
                            if (mugwumpLocations[i][0] != FOUND) {
                                double distance = Math.sqrt((Math.pow(right - distanceRightGuess, 2.0d))
                                        + (Math.pow(up - distanceUpGuess, 2.0d)));

                                System.out.println("YOU ARE " + (int) ((distance * 10) / 10) + " UNITS FROM MUGWUMP");
                            }
                        }
                    }

                    if (numberFound == NUMBER_OF_MUGWUMPS) {
                        System.out.println("YOU GOT THEM ALL IN " + turn + " TURNS!");
                        gameState = GAME_STATE.GAME_START;
                    } else if (turn >= MAX_TURNS) {
                        System.out.println("SORRY, THAT'S " + MAX_TURNS + " TRIES.  HERE IS WHERE THEY'RE HIDING");
                        for (int i = 0; i < NUMBER_OF_MUGWUMPS; i++) {
                            if (mugwumpLocations[i][0] != FOUND) {
                                System.out.println("MUGWUMP " + (i + 1) + " IS AT ("
                                        + mugwumpLocations[i][0] + "," + mugwumpLocations[i][1] + ")");
                            }
                        }
                        gameState = GAME_STATE.GAME_START;
                    }

                    // Game ended?
                    if (gameState != GAME_STATE.PLAY_TURN) {
                        System.out.println("THAT WAS FUN! LET'S PLAY AGAIN.......");
                        System.out.println("FOUR MORE MUGWUMPS ARE NOW IN HIDING.");
                    }
            }
            // Infinite loop - based on original basic version
        } while (true);
    }

    private void intro() {
        System.out.println(addSpaces(33) + "MUGWUMP");
        System.out.println(addSpaces(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("THE OBJECT OF THIS GAME IS TO FIND FOUR MUGWUMPS");
        System.out.println("HIDDEN ON A 10 BY 10 GRID.  HOMEBASE IS POSITION 0,0.");
        System.out.println("ANY GUESS YOU MAKE MUST BE TWO NUMBERS WITH EACH");
        System.out.println("NUMBER BETWEEN 0 AND 9, INCLUSIVE.  FIRST NUMBER");
        System.out.println("IS DISTANCE TO RIGHT OF HOMEBASE AND SECOND NUMBER");
        System.out.println("IS DISTANCE ABOVE HOMEBASE.");
        System.out.println();
        System.out.println("YOU GET 10 TRIES.  AFTER EACH TRY, I WILL TELL");
        System.out.println("YOU HOW FAR YOU ARE FROM EACH MUGWUMP.");
    }

    /**
     * Accepts a string delimited by comma's and returns the pos'th delimited
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

        Mugwump mugwump = new Mugwump();
        mugwump.play();
    }
}