import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Russian Roulette Paper
 * <p>
 * Based on the Basic game of Russian Roulette here
 * https://github.com/coding-horror/basic-computer-games/blob/main/76%20Russian%20Roulette/russianroulette.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */

public class RussianRoulette {

    public static final int BULLETS_IN_CHAMBER = 10;
    public static final double CHANCE_OF_GETTING_SHOT = .833333d;

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        INIT,
        GAME_START,
        FIRE_BULLET,
        NEXT_VICTIM
    }

    // Current game state
    private GAME_STATE gameState;

    int bulletsShot;

    public RussianRoulette() {
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
                    bulletsShot = 0;
                    System.out.println();
                    System.out.println("HERE IS A REVOLVER.");
                    System.out.println("TYPE '1' TO SPIN CHAMBER AND PULL TRIGGER.");
                    System.out.println("TYPE '2' TO GIVE UP.");
                    gameState = GAME_STATE.FIRE_BULLET;
                    break;

                case FIRE_BULLET:

                    int choice = displayTextAndGetNumber("GO ");

                    // Anything but selecting give up = have a shot
                    if (choice != 2) {
                        bulletsShot++;
                        if (Math.random() > CHANCE_OF_GETTING_SHOT) {
                            System.out.println("     BANG!!!!!   YOU'RE DEAD!");
                            System.out.println("CONDOLENCES WILL BE SENT TO YOUR RELATIVES.");
                            gameState = GAME_STATE.NEXT_VICTIM;
                        } else if (bulletsShot > BULLETS_IN_CHAMBER) {
                            System.out.println("YOU WIN!!!!!");
                            System.out.println("LET SOMEONE ELSE BLOW HIS BRAINS OUT.");
                            gameState = GAME_STATE.GAME_START;
                        } else {
                            // Phew player survived this round
                            System.out.println("- CLICK -");
                        }
                    } else {
                        // Player gave up
                        System.out.println("     CHICKEN!!!!!");
                        gameState = GAME_STATE.NEXT_VICTIM;

                    }
                    break;

                case NEXT_VICTIM:
                    System.out.println("...NEXT VICTIM...");
                    gameState = GAME_STATE.GAME_START;
            }
            // Infinite loop - based on original basic version
        } while (true);
    }

    private void intro() {
        System.out.println(addSpaces(28) + "RUSSIAN ROULETTE");
        System.out.println(addSpaces(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("THIS IS A GAME OF >>>>>>>>>>RUSSIAN ROULETTE.");
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

        RussianRoulette russianRoulette = new RussianRoulette();
        russianRoulette.play();
    }
}