import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.IOException;
import java.text.NumberFormat;

// This class handles reading input from the player
// Each input is an x and y coordinate
// e.g. 5,3
public class Input {
    private BufferedReader reader;
    private NumberFormat parser;
    private int scale;             // size of the sea, needed to validate input
    private boolean isQuit;        // whether the input has ended
    private int[] coords;          // the last coordinates read

    public Input(int seaSize) {
        scale = seaSize;
        reader = new BufferedReader(new InputStreamReader(System.in));
        parser = NumberFormat.getIntegerInstance();
    }

    public boolean readCoordinates() throws IOException {
        while (true) {
            // Write a prompt
            System.out.print("\nTarget x,y\n> ");
            String inputLine = reader.readLine();
            if (inputLine == null) {
                // If the input stream is ended, there is no way to continue the game
                System.out.println("\nGame quit\n");
                isQuit = true;
                return false;
            }

            // split the input into two fields
            String[] fields = inputLine.split(",");
            if (fields.length != 2) {
                // has to be exactly two
                System.out.println("Need two coordinates separated by ','");
                continue;
            }
            
            coords = new int[2];
            boolean error = false;
            // each field should contain an integer from 1 to the size of the sea 
            try {
                for (int c = 0 ; c < 2; ++c ) {
                    int val = Integer.parseInt(fields[c].strip());
                    if ((val < 1) || (val > scale)) {
                        System.out.println("Coordinates must be from 1 to " + scale);
                        error = true;
                    } else {
                        coords[c] = val;
                    }
                }
            }
            catch (NumberFormatException ne) {
                // this happens if the field is not a valid number
                System.out.println("Coordinates must be numbers");
                error = true;
            }
            if (!error) return true;
        }
    }

    public int x() { return coords[0]; }
    public int y() { return coords[1]; }
}
