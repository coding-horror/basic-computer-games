import java.util.ArrayList;
import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Love
 * <p>
 * Based on the Basic game of Love here
 * https://github.com/coding-horror/basic-computer-games/blob/main/58%20Love/love.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */

public class Love {

    // This is actually defined in the data, but made it a const for readability
    public static final int ROW_LENGTH = 60;

    // Contains the data to draw the picture
    private final ArrayList<Integer> data;

    // Used for keyboard input
    private final Scanner kbScanner;

    public Love() {
        data = storeData();
        kbScanner = new Scanner(System.in);
    }

    /**
     * Show an intro, accept a message, then draw the picture.
     */
    public void process() {
        intro();

        int rowLength = data.get(0);

        String message = displayTextAndGetInput("YOUR MESSAGE, PLEASE ");

        // ensure the string is at least 60 characters
        while (message.length() < rowLength) {
            message += message;
        }

        // chop of any extra characters so its exactly ROW_LENGTH in length
        if (message.length() > ROW_LENGTH) {
            message = message.substring(0, ROW_LENGTH);
        }

        // Print header
        System.out.println(message);

        int pos = 1;  // don't read row length which is value in first element position

        int runningLineTotal = 0;
        StringBuilder lineText = new StringBuilder();
        boolean outputChars = true;
        while (true) {
            int charsOrSpacesLength = data.get(pos);
            if (charsOrSpacesLength == ROW_LENGTH) {
                // EOF, so exit
                break;
            }
            if (outputChars) {
                // add characters from message string for charsOrSpacesLength characters
                for (int i = 0; i < charsOrSpacesLength; i++) {
                    lineText.append(message.charAt(i + runningLineTotal));
                    // switch to spaces which will be in the next element of the arraylist
                    outputChars = false;
                }
            } else {
                // add charsOrSpacesLength spaces to the string
                lineText.append(addSpaces(charsOrSpacesLength));
                // Switch to chars to output on next loop
                outputChars = true;
            }

            // We need to know when to print the string out
            runningLineTotal += charsOrSpacesLength;

            // Are we at end of line?  If so print and reset for next line
            if (runningLineTotal >= ROW_LENGTH) {
                System.out.println(lineText);
                lineText = new StringBuilder();
                runningLineTotal = 0;
                outputChars = true;
            }

            // Move to next arraylist element
            pos++;
        }

        // Print footer
        System.out.println(message);

    }

    private void intro() {
        System.out.println(addSpaces(33) + "LOVE");
        System.out.println(addSpaces(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("A TRIBUTE TO THE GREAT AMERICAN ARTIST, ROBERT INDIANA.");
        System.out.println("HIS GREATEST WORK WILL BE REPRODUCED WITH A MESSAGE OF");
        System.out.println("YOUR CHOICE UP TO 60 CHARACTERS.  IF YOU CAN'T THINK OF");
        System.out.println("A MESSAGE, SIMPLE TYPE THE WORD 'LOVE'");
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

    /**
     * Original Basic program had the data in DATA format.  We're importing all the data into an array for ease of
     * processing.
     * Format of data is
     * FIRST int of data is 60, which is the number of characters per line.
     * LAST int of data is same as FIRST above.
     * Then the data alternates between how many characters to print and how many spaces to print
     * You need to keep a running total of the count of ints read and once this hits 60, its time to
     * print and then reset count to zero.
     *
     * @return ArrayList of type Integer containing the data
     */
    private ArrayList<Integer> storeData() {

        ArrayList<Integer> theData = new ArrayList<>();

        theData.addAll(Arrays.asList(60, 1, 12, 26, 9, 12, 3, 8, 24, 17, 8, 4, 6, 23, 21, 6, 4, 6, 22, 12, 5, 6, 5));
        theData.addAll(Arrays.asList(4, 6, 21, 11, 8, 6, 4, 4, 6, 21, 10, 10, 5, 4, 4, 6, 21, 9, 11, 5, 4));
        theData.addAll(Arrays.asList(4, 6, 21, 8, 11, 6, 4, 4, 6, 21, 7, 11, 7, 4, 4, 6, 21, 6, 11, 8, 4));
        theData.addAll(Arrays.asList(4, 6, 19, 1, 1, 5, 11, 9, 4, 4, 6, 19, 1, 1, 5, 10, 10, 4, 4, 6, 18, 2, 1, 6, 8, 11, 4));
        theData.addAll(Arrays.asList(4, 6, 17, 3, 1, 7, 5, 13, 4, 4, 6, 15, 5, 2, 23, 5, 1, 29, 5, 17, 8));
        theData.addAll(Arrays.asList(1, 29, 9, 9, 12, 1, 13, 5, 40, 1, 1, 13, 5, 40, 1, 4, 6, 13, 3, 10, 6, 12, 5, 1));
        theData.addAll(Arrays.asList(5, 6, 11, 3, 11, 6, 14, 3, 1, 5, 6, 11, 3, 11, 6, 15, 2, 1));
        theData.addAll(Arrays.asList(6, 6, 9, 3, 12, 6, 16, 1, 1, 6, 6, 9, 3, 12, 6, 7, 1, 10));
        theData.addAll(Arrays.asList(7, 6, 7, 3, 13, 6, 6, 2, 10, 7, 6, 7, 3, 13, 14, 10, 8, 6, 5, 3, 14, 6, 6, 2, 10));
        theData.addAll(Arrays.asList(8, 6, 5, 3, 14, 6, 7, 1, 10, 9, 6, 3, 3, 15, 6, 16, 1, 1));
        theData.addAll(Arrays.asList(9, 6, 3, 3, 15, 6, 15, 2, 1, 10, 6, 1, 3, 16, 6, 14, 3, 1, 10, 10, 16, 6, 12, 5, 1));
        theData.addAll(Arrays.asList(11, 8, 13, 27, 1, 11, 8, 13, 27, 1, 60));

        return theData;
    }

    public static void main(String[] args) {

        Love love = new Love();
        love.process();
    }
}
