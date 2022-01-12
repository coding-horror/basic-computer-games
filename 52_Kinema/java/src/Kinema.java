import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Kinema
 * <p>
 * Based on the Basic game of Kinema here
 * https://github.com/coding-horror/basic-computer-games/blob/main/52%20Kinema/kinema.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Kinema {

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        STARTUP,
        INIT,
        HOW_HIGH,
        SECONDS_TILL_IT_RETURNS,
        ITS_VELOCITY,
        RESULTS,
        GAME_OVER
    }

    // Current game state
    private GAME_STATE gameState;

    private int numberAnswersCorrect;

    // How many meters per second a ball is thrown
    private int velocity;

    public Kinema() {
        kbScanner = new Scanner(System.in);

        gameState = GAME_STATE.STARTUP;
    }

    /**
     * Main game loop
     */
    public void play() {

        double playerAnswer;
        double correctAnswer;
        do {
            switch (gameState) {

                case STARTUP:
                    intro();
                    gameState = GAME_STATE.INIT;
                    break;

                case INIT:
                    numberAnswersCorrect = 0;

                    // calculate a random velocity for the player to use in the calculations
                    velocity = 5 + (int) (35 * Math.random());
                    System.out.println("A BALL IS THROWN UPWARDS AT " + velocity + " METERS PER SECOND.");
                    gameState = GAME_STATE.HOW_HIGH;
                    break;

                case HOW_HIGH:

                    playerAnswer = displayTextAndGetNumber("HOW HIGH WILL IT GO (IN METERS)? ");

                    // Calculate the correct answer to how high it will go
                    correctAnswer = 0.05 * Math.pow(velocity, 2);
                    if (calculate(playerAnswer, correctAnswer)) {
                        numberAnswersCorrect++;
                    }
                    gameState = GAME_STATE.ITS_VELOCITY;
                    break;

                case ITS_VELOCITY:

                    playerAnswer = displayTextAndGetNumber("HOW LONG UNTIL IT RETURNS (IN SECONDS)? ");

                    // Calculate current Answer for how long until it returns to the ground in seconds
                    correctAnswer = (double) velocity / 5;
                    if (calculate(playerAnswer, correctAnswer)) {
                        numberAnswersCorrect++;
                    }
                    gameState = GAME_STATE.SECONDS_TILL_IT_RETURNS;
                    break;

                case SECONDS_TILL_IT_RETURNS:

                    // Calculate random number of seconds for 3rd question
                    double seconds = 1 + (Math.random() * (2 * velocity)) / 10;

                    // Round to one decimal place.
                    double scale = Math.pow(10, 1);
                    seconds = Math.round(seconds * scale) / scale;

                    playerAnswer = displayTextAndGetNumber("WHAT WILL ITS VELOCITY BE AFTER " + seconds + " SECONDS? ");

                    // Calculate the velocity after the given number of seconds
                    correctAnswer = velocity - (10 * seconds);
                    if (calculate(playerAnswer, correctAnswer)) {
                        numberAnswersCorrect++;
                    }
                    gameState = GAME_STATE.RESULTS;
                    break;

                case RESULTS:
                    System.out.println(numberAnswersCorrect + " RIGHT OUT OF 3");
                    if (numberAnswersCorrect > 1) {
                        System.out.println(" NOT BAD.");
                    }
                    gameState = GAME_STATE.STARTUP;
                    break;
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    private void intro() {
        System.out.println(simulateTabs(33) + "KINEMA");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
    }

    private boolean calculate(double playerAnswer, double correctAnswer) {

        boolean gotItRight = false;

        if (Math.abs((playerAnswer - correctAnswer) / correctAnswer) < 0.15) {
            System.out.println("CLOSE ENOUGH");
            gotItRight = true;
        } else {
            System.out.println("NOT EVEN CLOSE");
        }
        System.out.println("CORRECT ANSWER IS " + correctAnswer);
        System.out.println();

        return gotItRight;
    }

    /*
     * Print a message on the screen, then accept input from Keyboard.
     * Converts input to a Double
     *
     * @param text message to be displayed on screen.
     * @return what was typed by the player.
     */
    private double displayTextAndGetNumber(String text) {
        return Double.parseDouble(displayTextAndGetInput(text));
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
