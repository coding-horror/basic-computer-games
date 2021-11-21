import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Chemist
 * <p>
 * Based on the Basic game of Chemist here
 * https://github.com/coding-horror/basic-computer-games/blob/main/24%20Chemist/chemist.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Chemist {

    public static final int MAX_LIVES = 9;

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        START_GAME,
        INPUT,
        BLOWN_UP,
        SURVIVED,
        GAME_OVER
    }

    // Current game state
    private GAME_STATE gameState;

    private int timesBlownUp;

    public Chemist() {
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
                    timesBlownUp = 0;
                    gameState = GAME_STATE.INPUT;
                    break;

                case INPUT:

                    int amountOfAcid = (int) (Math.random() * 50);
                    int correctAmountOfWater = (7 * amountOfAcid) / 3;
                    int water = displayTextAndGetNumber(amountOfAcid + " LITERS OF KRYPTOCYANIC ACID.  HOW MUCH WATER? ");

                    // Calculate if the player mixed enough water
                    int result = Math.abs(correctAmountOfWater - water);

                    // Ratio of water wrong?
                    if (result > (correctAmountOfWater / 20)) {
                        gameState = GAME_STATE.BLOWN_UP;
                    } else {
                        // Got the ratio correct
                        gameState = GAME_STATE.SURVIVED;
                    }
                    break;

                case BLOWN_UP:
                    System.out.println(" SIZZLE!  YOU HAVE JUST BEEN DESALINATED INTO A BLOB");
                    System.out.println(" OF QUIVERING PROTOPLASM!");

                    timesBlownUp++;

                    if (timesBlownUp < MAX_LIVES) {
                        System.out.println(" HOWEVER, YOU MAY TRY AGAIN WITH ANOTHER LIFE.");
                        gameState = GAME_STATE.INPUT;
                    } else {
                        System.out.println(" YOUR " + MAX_LIVES + " LIVES ARE USED, BUT YOU WILL BE LONG REMEMBERED FOR");
                        System.out.println(" YOUR CONTRIBUTIONS TO THE FIELD OF COMIC BOOK CHEMISTRY.");
                        gameState = GAME_STATE.GAME_OVER;
                    }

                    break;

                case SURVIVED:
                    System.out.println(" GOOD JOB! YOU MAY BREATHE NOW, BUT DON'T INHALE THE FUMES!");
                    System.out.println();
                    gameState = GAME_STATE.INPUT;
                    break;

            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    private void intro() {
        System.out.println(simulateTabs(33) + "CHEMIST");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("THE FICTITIOUS CHEMICAL KRYPTOCYANIC ACID CAN ONLY BE");
        System.out.println("DILUTED BY THE RATIO OF 7 PARTS WATER TO 3 PARTS ACID.");
        System.out.println("IF ANY OTHER RATIO IS ATTEMPTED, THE ACID BECOMES UNSTABLE");
        System.out.println("AND SOON EXPLODES.  GIVEN THE AMOUNT OF ACID, YOU MUST");
        System.out.println("DECIDE WHO MUCH WATER TO ADD FOR DILUTION.  IF YOU MISS");
        System.out.println("YOU FACE THE CONSEQUENCES.");
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
