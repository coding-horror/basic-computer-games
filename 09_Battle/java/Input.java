import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.IOException;
import java.text.NumberFormat;

public class Input {
    private BufferedReader reader;
    private NumberFormat parser;
    private int scale;
    private boolean isQuit;
    private int[] coords;

    public Input(int seaSize) {
        scale = seaSize;
        reader = new BufferedReader(new InputStreamReader(System.in));
        parser = NumberFormat.getIntegerInstance();
    }

    public boolean readCoordinates() throws IOException {
        while (true) {
            System.out.print("\nTarget x,y\n> ");
            String inputLine = reader.readLine();
            if (inputLine == null) {
                System.out.println("Game quit\n");
                isQuit = true;
                return false;
            }

            String[] fields = inputLine.split(",");
            if (fields.length != 2) {
                System.out.println("Need two coordinates separated by ','");
                continue;
            }
            
            coords = new int[2];
            boolean error = false;
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
                System.out.println("Coordinates must be numbers");
                error = true;
            }
            if (!error) return true;
        }
    }

    public int x() { return coords[0]; }
    public int y() { return coords[1]; }
}
