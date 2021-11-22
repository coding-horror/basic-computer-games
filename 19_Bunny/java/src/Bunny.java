import java.util.ArrayList;
import java.util.Arrays;

/**
 * Bunny
 * <p>
 * Based on the Basic program Bunny
 * https://github.com/coding-horror/basic-computer-games/blob/main/19%20Bunny/bunny.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */

public class Bunny {

    // First 4 elements are the text BUNNY, so skip those
    public static final int REAL_DATA_START_POS = 5;

    // Data for characters is not representative of three ASCII character, so we have
    // to add 64 to it as per original program design.
    public static final int CONVERT_TO_ASCII = 64;

    public static final int EOF = 4096; //End of file
    public static final int EOL = -1;  // End of line

    // Contains the data to draw the picture
    private final ArrayList<Integer> data;

    public Bunny() {
        data = loadData();
    }

    /**
     * Show an intro, then draw the picture.
     */
    public void process() {

        intro();

        // First 5 characters of data spells out BUNNY, so add this to a string
        StringBuilder bunnyBuilder = new StringBuilder();
        for (int i = 0; i < REAL_DATA_START_POS; i++) {
            // Convert the data to the character representation for output
            // Ascii A=65, B=66 - see loadData method
            bunnyBuilder.append(Character.toChars(data.get(i) + CONVERT_TO_ASCII));
        }

        // We now have the string to be used in the output
        String bunny = bunnyBuilder.toString();

        int pos = REAL_DATA_START_POS;  // Point to the start of the actual data
        int previousPos = 0;

        // Loop until we reach a number indicating EOF
        while (true) {
            // This is where we want to start drawing
            int first = data.get(pos);
            if (first == EOF) {
                break;
            }
            if (first == EOL) {
                System.out.println();
                previousPos = 0;
                // Move to the next element in the ArrayList
                pos++;
                continue;
            }

            // Because we are not using screen positioning, we just add an appropriate
            // numbers of spaces from where we want to be, and where we last outputted something
            System.out.print(addSpaces(first - previousPos));

            // We use this next time around the loop
            previousPos = first;

            // Move to next element
            pos++;
            // This is where we want to stop drawing/
            int second = data.get(pos);

            // Now we loop through the number of characters to draw using
            // the starting and ending point.
            for (int i = first; i <= second; i++) {
                // Cycle through the actual number of characters but use the
                // remainder operator to ensure we only use characters from the
                // bunny string
                System.out.print(bunny.charAt(i % bunny.length()));
                // Advance where we were at.
                previousPos += 1;
            }
            // Point to next data element
            pos++;
        }

        System.out.println();

    }

    private void intro() {
        System.out.println(addSpaces(33) + "BUNNY");
        System.out.println(addSpaces(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
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
     * Original Basic program had the data in DATA format.
     * We're importing all the data into an array for ease of processing.
     * Format of data is
     * characters 0-4 is the letters that will be used in the output. 64 + the value represents the ASCII character
     * ASCII code 65 = A, 66 = B, etc.  so 2+64=66 (B), 21+64=85 (U) and so on.
     * Then we next have pairs of numbers.
     * Looking at the this data
     * 1,2,-1,0,2,45,50,-1
     * That reads as
     * 1,2 = draw characters - in this case BU
     * -1 = go to a new line
     * 0,2 = DRAW BUN
     * 45,50 = DRAW BUNNYB starting at position 45
     * and so on.
     * 4096 is EOF
     *
     * @return ArrayList of type Integer containing the data
     */
    private ArrayList<Integer> loadData() {

        ArrayList<Integer> theData = new ArrayList<>();

        // This is the data faithfully added from the original basic program.
        // Notes:
        // The first 5 ints are ASCII character (well 64 is added to make them ASCII chars we can output).
        theData.addAll(Arrays.asList(2, 21, 14, 14, 25));
        theData.addAll(Arrays.asList(1, 2, -1, 0, 2, 45, 50, -1, 0, 5, 43, 52, -1, 0, 7, 41, 52, -1));
        theData.addAll(Arrays.asList(1, 9, 37, 50, -1, 2, 11, 36, 50, -1, 3, 13, 34, 49, -1, 4, 14, 32, 48, -1));
        theData.addAll(Arrays.asList(5, 15, 31, 47, -1, 6, 16, 30, 45, -1, 7, 17, 29, 44, -1, 8, 19, 28, 43, -1));
        theData.addAll(Arrays.asList(9, 20, 27, 41, -1, 10, 21, 26, 40, -1, 11, 22, 25, 38, -1, 12, 22, 24, 36, -1));
        theData.addAll(Arrays.asList(13, 34, -1, 14, 33, -1, 15, 31, -1, 17, 29, -1, 18, 27, -1));
        theData.addAll(Arrays.asList(19, 26, -1, 16, 28, -1, 13, 30, -1, 11, 31, -1, 10, 32, -1));
        theData.addAll(Arrays.asList(8, 33, -1, 7, 34, -1, 6, 13, 16, 34, -1, 5, 12, 16, 35, -1));
        theData.addAll(Arrays.asList(4, 12, 16, 35, -1, 3, 12, 15, 35, -1, 2, 35, -1, 1, 35, -1));
        theData.addAll(Arrays.asList(2, 34, -1, 3, 34, -1, 4, 33, -1, 6, 33, -1, 10, 32, 34, 34, -1));
        theData.addAll(Arrays.asList(14, 17, 19, 25, 28, 31, 35, 35, -1, 15, 19, 23, 30, 36, 36, -1));
        theData.addAll(Arrays.asList(14, 18, 21, 21, 24, 30, 37, 37, -1, 13, 18, 23, 29, 33, 38, -1));
        theData.addAll(Arrays.asList(12, 29, 31, 33, -1, 11, 13, 17, 17, 19, 19, 22, 22, 24, 31, -1));
        theData.addAll(Arrays.asList(10, 11, 17, 18, 22, 22, 24, 24, 29, 29, -1));
        theData.addAll(Arrays.asList(22, 23, 26, 29, -1, 27, 29, -1, 28, 29, -1, 4096));

        return theData;
    }

    public static void main(String[] args) {

        Bunny bunny = new Bunny();
        bunny.process();
    }
}