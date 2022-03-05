import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Comparator;
import java.util.Random;
import java.util.function.Predicate;
import java.text.NumberFormat;


/* This class holds the game state and the game logic */
public class Battle {

    /* parameters of the game */
    private int seaSize;
    private int[] sizes;
    private int[] counts;

    /* The game setup - the ships and the sea */
    private ArrayList<Ship> ships;
    private Sea sea;

    /* game state counts */
    private int[] losses;    // how many of each type of ship have been sunk
    private int hits;        // how many hits the player has made
    private int misses;      // how many misses the player has made

    // Names of ships of each size. The game as written has ships of size 3, 4 and 5 but
    // can easily be modified. It makes no sense to have a ship of size zero though.
    private static String NAMES_BY_SIZE[] = {
        "error",
        "size1",
        "destroyer",
        "cruiser",
        "aircraft carrier",
        "size5" };

    // Entrypoint
    public static void main(String args[]) {
        Battle game = new Battle(6,                        // Sea is 6 x 6 tiles
                                 new int[] { 2, 3, 4 },    // Ships are of sizes 2, 3, and 4
                                 new int[] { 2, 2, 2 });   // there are two ships of each size
        game.play();
    }

    public Battle(int scale, int[] shipSizes, int[] shipCounts) {
        seaSize = scale;
        sizes = shipSizes;
        counts = shipCounts;

        // validate parameters
        if (seaSize < 4) throw new RuntimeException("Sea Size " + seaSize + " invalid, must be at least 4");

        for (int sz : sizes) {
            if ((sz < 1) || (sz > seaSize))
                throw new RuntimeException("Ship has invalid size " + sz);
        }

        if (counts.length != sizes.length) {
            throw new RuntimeException("Ship counts must match");
        }

        // Initialize game state
        sea = new Sea(seaSize);          // holds what ship if any occupies each tile
        ships = new ArrayList<Ship>();   // positions and states of all the ships
        losses = new int[counts.length]; // how many ships of each type have been sunk

        // Build up the list of all the ships
        int shipNumber = 1;
        for (int type = 0; type < counts.length; ++type) {
            for (int i = 0; i < counts[i]; ++i) {
                ships.add(new Ship(shipNumber++, sizes[type]));
            }
        }

        // When we put the ships in the sea, we put the biggest ones in first, or they might
        // not fit
        ArrayList<Ship> largestFirst = new ArrayList<>(ships);
        Collections.sort(largestFirst, Comparator.comparingInt((Ship ship) -> ship.size()).reversed());

        // place each ship into the sea
        for (Ship ship : largestFirst) {
            ship.placeRandom(sea);
        }
    }

    public void play() {
        System.out.println("The following code of the bad guys' fleet disposition\nhas been captured but not decoded:\n");
        System.out.println(sea.encodedDump());
        System.out.println("De-code it and use it if you can\nbut keep the de-coding method a secret.\n");

        int lost = 0;
        System.out.println("Start game");
        Input input = new Input(seaSize);
        try {
            while (lost < ships.size()) {          // the game continues while some ships remain unsunk
                if (! input.readCoordinates()) {   // ... unless there is no more input from the user
                    return;
                }

                // The computer thinks of the sea as a grid of rows, from top to bottom.
                // However, the user will use X and Y coordinates, with Y going bottom to top
                int row = seaSize - input.y();
                int col = input.x() - 1;

                if (sea.isEmpty(col, row)) {
                    ++misses;
                    System.out.println("Splash!  Try again.");
                } else {
                    Ship ship = ships.get(sea.get(col, row) - 1);
                    if (ship.isSunk()) {
                        ++misses;
                        System.out.println("There used to be a ship at that point, but you sunk it.");
                        System.out.println("Splash!  Try again.");
                    } else if (ship.wasHit(col, row)) {
                        ++misses;
                        System.out.println("You already put a hole in ship number " + ship.id());
                        System.out.println("Splash!  Try again.");
                    } else {
                        ship.hit(col, row);
                        ++hits;
                        System.out.println("A direct hit on ship number " + ship.id());

                        // If a ship was hit, we need to know whether it was sunk.
                        // If so, tell the player and update our counts
                        if (ship.isSunk()) {
                            ++lost;
                            System.out.println("And you sunk it.  Hurrah for the good guys.");
                            System.out.print("So far, the bad guys have lost ");
                            ArrayList<String> typeDescription = new ArrayList<>();
                            for (int i = 0 ; i < sizes.length; ++i) {
                                if (sizes[i] == ship.size()) {
                                    ++losses[i];
                                }
                                StringBuilder sb = new StringBuilder();
                                sb.append(losses[i]);
                                sb.append(" ");
                                sb.append(NAMES_BY_SIZE[sizes[i]]);
                                if (losses[i] != 1)
                                    sb.append("s");
                                typeDescription.add(sb.toString());
                            }
                            System.out.println(String.join(", ", typeDescription));
                            double ratioNum = ((double)misses)/hits;
                            String ratio = NumberFormat.getInstance().format(ratioNum);
                            System.out.println("Your current splash/hit ratio is " + ratio);

                            if (lost == ships.size()) {
                                System.out.println("You have totally wiped out the bad guys' fleet");
                                System.out.println("With a final splash/hit ratio of " + ratio);

                                if (misses == 0) {
                                    System.out.println("Congratulations - A direct hit every time.");
                                }

                                System.out.println("\n****************************\n");
                            }
                        }
                    }
                }
            }
        }
        catch (IOException e) {
            // This should not happen running from console, but java requires us to check for it
            System.err.println("System error.\n" + e);
        }
    }
}
