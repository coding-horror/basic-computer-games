import java.util.Arrays;
import java.util.Scanner;

/**
 * Train
 * <p>
 * Based on the Basic program Train here
 * https://github.com/coding-horror/basic-computer-games/blob/main/91%20Train/train.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic program in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Train {

    private final Scanner kbScanner;

    public Train() {
        kbScanner = new Scanner(System.in);
    }

    public void process() {

        intro();

        boolean gameOver = false;

        do {
            double carMph = (int) (25 * Math.random() + 40);
            double hours = (int) (15 * Math.random() + 5);
            double train = (int) (19 * Math.random() + 20);

            System.out.println(" A CAR TRAVELING " + (int) carMph + " MPH CAN MAKE A CERTAIN TRIP IN");
            System.out.println((int) hours + " HOURS LESS THAN A TRAIN TRAVELING AT " + (int) train + " MPH.");

            double howLong = Double.parseDouble(displayTextAndGetInput("HOW LONG DOES THE TRIP TAKE BY CAR? "));

            double hoursAnswer = hours * train / (carMph - train);
            int percentage = (int) (Math.abs((hoursAnswer - howLong) * 100 / howLong) + .5);
            if (percentage > 5) {
                System.out.println("SORRY.  YOU WERE OFF BY " + percentage + " PERCENT.");
            } else {
                System.out.println("GOOD! ANSWER WITHIN " + percentage + " PERCENT.");
            }
            System.out.println("CORRECT ANSWER IS " + hoursAnswer + " HOURS.");

            System.out.println();
            if (!yesEntered(displayTextAndGetInput("ANOTHER PROBLEM (YES OR NO)? "))) {
                gameOver = true;
            }

        } while (!gameOver);


    }

    private void intro() {
        System.out.println("TRAIN");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("TIME - SPEED DISTANCE EXERCISE");
        System.out.println();
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
     * Checks whether player entered Y or YES to a question.
     *
     * @param text player string from kb
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
     * @param text   source string
     * @param values a range of values to compare against the source string
     * @return true if a comparison was found in one of the variable number of strings passed
     */
    private boolean stringIsAnyValue(String text, String... values) {

        return Arrays.stream(values).anyMatch(str -> str.equalsIgnoreCase(text));

    }

    /**
     * Program startup.
     *
     * @param args not used (from command line).
     */
    public static void main(String[] args) {
        Train train = new Train();
        train.process();
    }
}
