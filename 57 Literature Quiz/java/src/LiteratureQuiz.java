import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Literature Quiz
 * <p>
 * Based on the Basic game of Literature Quiz here
 * https://github.com/coding-horror/basic-computer-games/blob/main/57%20Literature%20Quiz/litquiz.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class LiteratureQuiz {

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        STARTUP,
        QUESTIONS,
        RESULTS,
        GAME_OVER
    }

    // Current game state
    private GAME_STATE gameState;
    // Players correct answers
    private int correctAnswers;

    public LiteratureQuiz() {

        gameState = GAME_STATE.STARTUP;

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
                case STARTUP:
                    intro();
                    correctAnswers = 0;
                    gameState = GAME_STATE.QUESTIONS;
                    break;

                // Ask the player four questions
                case QUESTIONS:

                    // Question 1
                    System.out.println("IN PINOCCHIO, WHAT WAS THE NAME OF THE CAT");
                    int question1Answer = displayTextAndGetNumber("1)TIGGER, 2)CICERO, 3)FIGARO, 4)GUIPETTO ? ");
                    if (question1Answer == 3) {
                        System.out.println("VERY GOOD!  HERE'S ANOTHER.");
                        correctAnswers++;
                    } else {
                        System.out.println("SORRY...FIGARO WAS HIS NAME.");
                    }

                    System.out.println();

                    // Question 2
                    System.out.println("FROM WHOSE GARDEN DID BUGS BUNNY STEAL THE CARROTS?");
                    int question2Answer = displayTextAndGetNumber("1)MR. NIXON'S, 2)ELMER FUDD'S, 3)CLEM JUDD'S, 4)STROMBOLI'S ? ");
                    if (question2Answer == 2) {
                        System.out.println("PRETTY GOOD!");
                        correctAnswers++;
                    } else {
                        System.out.println("TOO BAD...IT WAS ELMER FUDD'S GARDEN.");
                    }

                    System.out.println();

                    // Question 3
                    System.out.println("IN THE WIZARD OF OS, DOROTHY'S DOG WAS NAMED");
                    int question3Answer = displayTextAndGetNumber("1)CICERO, 2)TRIXIA, 3)KING, 4)TOTO ? ");
                    if (question3Answer == 4) {
                        System.out.println("YEA!  YOU'RE A REAL LITERATURE GIANT.");
                        correctAnswers++;
                    } else {
                        System.out.println("BACK TO THE BOOKS,...TOTO WAS HIS NAME.");
                    }

                    System.out.println();

                    // Question 4
                    System.out.println("WHO WAS THE FAIR MAIDEN WHO ATE THE POISON APPLE");
                    int question4Answer = displayTextAndGetNumber("1)SLEEPING BEAUTY, 2)CINDERELLA, 3)SNOW WHITE, 4)WENDY ? ");
                    if (question4Answer == 3) {
                        System.out.println("GOOD MEMORY!");
                        correctAnswers++;
                    } else {
                        System.out.println("OH, COME ON NOW...IT WAS SNOW WHITE.");
                    }

                    System.out.println();
                    gameState = GAME_STATE.RESULTS;
                    break;

                // How did the player do?
                case RESULTS:
                    if (correctAnswers == 4) {
                        // All correct
                        System.out.println("WOW!  THAT'S SUPER!  YOU REALLY KNOW YOUR NURSERY");
                        System.out.println("YOUR NEXT QUIZ WILL BE ON 2ND CENTURY CHINESE");
                        System.out.println("LITERATURE (HA, HA, HA)");
                        // one or none correct
                    } else if (correctAnswers < 2) {
                        System.out.println("UGH.  THAT WAS DEFINITELY NOT TOO SWIFT.  BACK TO");
                        System.out.println("NURSERY SCHOOL FOR YOU, MY FRIEND.");
                        // two or three correct
                    } else {
                        System.out.println("NOT BAD, BUT YOU MIGHT SPEND A LITTLE MORE TIME");
                        System.out.println("READING THE NURSERY GREATS.");
                    }
                    gameState = GAME_STATE.GAME_OVER;
                    break;
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    public void intro() {
        System.out.println(simulateTabs(25) + "LITERATURE QUIZ");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("LITERATURE QUIZ");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("TEST YOUR KNOWLEDGE OF CHILDREN'S LITERATURE.");
        System.out.println("THIS IS A MULTIPLE-CHOICE QUIZ.");
        System.out.println("TYPE A 1, 2, 3, OR 4 AFTER THE QUESTION MARK.");
        System.out.println();
        System.out.println("GOOD LUCK!");
        System.out.println();
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
}