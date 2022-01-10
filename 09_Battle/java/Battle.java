import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Comparator;
import java.util.Random;
import java.util.function.Predicate;
import java.text.NumberFormat;

public class Battle {
    private int seaSize;
    private int[] sizes;
    private int[] counts;
    
    private ArrayList<Ship> ships;
    private Sea sea;

    private int[] losses;
    private int hits;
    private int misses;

    private static String NAMES_BY_SIZE[] = {
        "error",
        "size1",
        "destroyer",
        "cruiser",
        "aircraft carrier",
        "size5" };

    public static void main(String args[]) {
        Battle game = new Battle(6,
                                 new int[] { 2, 3, 4 },
                                 new int[] { 2, 2, 2 });
        game.play();
    }

    public Battle(int scale, int[] shipSizes, int[] shipCounts) {
        seaSize = scale;
        sizes = shipSizes;
        counts = shipCounts;

        /* validate parameters */
        if (seaSize < 4) throw new RuntimeException("Sea Size " + seaSize + " invalid, must be at least 4");

        for (int sz : sizes) {
            if ((sz < 1) || (sz > seaSize))
                throw new RuntimeException("Ship has invalid size " + sz);
        }

        if (counts.length != sizes.length) {
            throw new RuntimeException("Ship counts must match");
        }

        sea = new Sea(seaSize);
        ships = new ArrayList<Ship>();
        losses = new int[counts.length];

        int shipNumber = 1;
        for (int type = 0; type < counts.length; ++type) {
            for (int i = 0; i < counts[i]; ++i) {
                ships.add(new Ship(shipNumber++, "Ship", sizes[type]));
            }
        }

        ArrayList<Ship> largestFirst = new ArrayList<>(ships);
        Collections.sort(largestFirst, Comparator.comparingInt((Ship ship) -> ship.size()).reversed());

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
        try {
            BufferedReader input = new BufferedReader(new InputStreamReader(System.in));
            NumberFormat parser = NumberFormat.getIntegerInstance();

            while (lost < ships.size()) {
                System.out.print("\nTarget x,y\n> ");
                String inputLine = input.readLine();
                if (inputLine == null) {
                    System.out.println("Game quit\n");
                    return;
                }
                String[] coords = inputLine.split(",");
                if (coords.length != 2) {
                    System.out.println("Need two coordinates separated by ','");
                    continue;
                }
                int[] xy = new int[2];
                boolean error = false;
                try {
                    for (int c = 0 ; c < 2; ++c ) {
                        int val = Integer.parseInt(coords[c].strip());
                        if ((val < 1) || (val > seaSize)) {
                            System.out.println("Coordinates must be from 1 to " + seaSize);
                            error = true;
                        } else {
                            xy[c] = val;
                        }
                    }
                }
                catch (NumberFormatException ne) {
                    System.out.println("Coordinates must be numbers");
                    error = true;
                }
                if (error) continue;

                int row = seaSize - xy[1];
                int col = xy[0] - 1;

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
        }
    }

    private static class Ship {
        public static final int ORIENT_E=0;
        public static final int ORIENT_SE=1;
        public static final int ORIENT_S=2;
        public static final int ORIENT_SW=3;

        private int id;
        private int size;
        private String type;
        private boolean placed;
        private boolean sunk;
        private ArrayList<Boolean> hits;
        
        private int startX;
        private int startY;
        private int orientX;
        private int orientY;

        public Ship(int i, String name, int sz) {
            id = i; type = name; size = sz;
            sunk = false; placed = false;
            hits = new ArrayList<>(Collections.nCopies(size, false));
        }

        public int id() { return id; }
        public int size() { return size; }

        public void hit(int x, int y) {
            int offset;
            if (orientX != 0) {
                offset = (x - startX) / orientX;
            } else {
                offset = (y - startY) / orientY;
            }
            hits.set(offset, true);

            sunk = hits.stream().allMatch(Predicate.isEqual(true));
        }

        public boolean isSunk() { return sunk; }

        public boolean wasHit(int x, int y) {
            int offset;
            if (orientX != 0) {
                offset = (x - startX) / orientX;
            } else {
                offset = (y - startY) / orientY;
            }
            return hits.get(offset);
        };

        public void placeRandom(Sea s) {
            Random random = new Random();
            for (int tries = 0 ; tries < 1000 ; ++tries) {
                int x = random.nextInt(s.size());
                int y = random.nextInt(s.size());
                int orient = random.nextInt(4);

                if (place(s, x, y, orient)) return;
            }

            throw new RuntimeException("Could not place any more ships");
        }

        private boolean extendShip(Sea s, int fromX, int fromY, int toX, int toY) {
            if (!s.isEmpty(toX, toY)) return false;                  // no space
            if ((fromX == toX)||(fromY == toY)) return true;         // horizontal or vertical

            // we can extend the ship without colliding, but we are going diagonally
            // and it should not be possible for two ships to cross each other on
            // opposite diagonals.

            // check the two tiles that would cross us here - if either is empty, we are OK
            // if they both contain different ships, we are OK
            // but if they both contain the same ship, we are crossing!
            int corner1 = s.get(fromX, toY);
            int corner2 = s.get(toX, fromY);
            if ((corner1 == 0) || (corner1 != corner2)) return true;
            return false;
        }

        public boolean place(Sea s, int x, int y, int orient) {
            if (placed) {
                throw new RuntimeException("Program error - placed ship " + id + " twice");
            }
            switch(orient) {
            case ORIENT_E:
                orientX = 1; orientY = 0;
                break;
            case ORIENT_SE:
                orientX = 1; orientY = 1;
                break;
            case ORIENT_S:
                orientX = 0; orientY = 1;
                break;
            case ORIENT_SW:
                orientX = -1; orientY = 1;
                break;
            default:
                throw new RuntimeException("Invalid orientation " + orient);
            }

            if (!s.isEmpty(x, y)) return false;
            startX = x; startY = y;
            int tilesPlaced = 1;
            int nextX = startX;
            int nextY = startY;
            while (tilesPlaced < size) {
                if (extendShip(s, nextX, nextY, nextX + orientX, nextY + orientY)) {
                    tilesPlaced += 1;
                    nextX = nextX + orientX;
                    nextY = nextY + orientY;
                } else {
                    int backX = startX - orientX;
                    int backY = startY - orientY;

                    if (extendShip(s, startX, startY, backX, backY)) {
                        tilesPlaced +=1;
                        startX = backX;
                        startY = backY;
                    } else {
                        return false;
                    }
                }
            }

            for (int i = 0; i < size; ++i) {
                int sx = startX + i * orientX;
                int sy = startY + i * orientY;
                s.set(sx, sy, id);
            }
            placed = true;
            return true;
        }

    }

    private static class Sea {
        private int tiles[];
        private boolean hits[];

        private int size;
        public Sea(int make_size) {
            size = make_size;
            tiles = new int[size*size];
        }

        public int size() { return size; }

        public String encodedDump() {
            StringBuilder out = new StringBuilder();
            for (int x = 0; x < size; ++x) {
                for (int y = 0; y < size; ++y) 
                    out.append(Integer.toString(get(x, y)));
                out.append('\n');
            }
            return out.toString();   
        }

        /* return true if x,y is in the sea and empty
         * return false if x,y is occupied or is out of range
         */
        public boolean isEmpty(int x, int y) {
            if ((x<0)||(x>=size)||(y<0)||(y>=size)) return false;
            return (get(x,y) == 0);
        }

        /* return the ship number, or zero if no ship */
        public int get(int x, int y) {
            return tiles[index(x,y)];
        }

        public void set(int x, int y, int value) {
            tiles[index(x, y)] = value;
        }

        public int shipHit(int x, int y) {
            if (hits[index(x,y)]) return get(x, y);
            else return 0;
        }

        public void recordHit(int x, int y) {
            hits[index(x, y)] = true;
        }

        private int index(int x, int y) {
            if ((x < 0) || (x >= size))
                throw new ArrayIndexOutOfBoundsException("Program error: x cannot be " + x);
            if ((y < 0) || (y >= size))
                throw new ArrayIndexOutOfBoundsException("Program error: y cannot be " + y);

            return y*size + x;
        }
    }
}
