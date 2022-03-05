import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.Random;
import java.util.function.Predicate;

/** A single ship, with its position and where it has been hit */
class Ship {
    // These are the four directions that ships can be in
    public static final int ORIENT_E=0;   // goes East from starting position
    public static final int ORIENT_SE=1;  // goes SouthEast from starting position
    public static final int ORIENT_S=2;   // goes South from starting position
    public static final int ORIENT_SW=3;  // goes SouthWest from starting position

    private int id;                   // ship number
    private int size;                 // how many tiles it occupies
    private boolean placed;           // whether this ship is in the sea yet
    private boolean sunk;             // whether this ship has been sunk
    private ArrayList<Boolean> hits;  // which tiles of the ship have been hit

    private int startX;               // starting position coordinates
    private int startY;
    private int orientX;              // x and y deltas from each tile occupied to the next
    private int orientY;

    public Ship(int i, int sz) {
        id = i; size = sz;
        sunk = false; placed = false;
        hits = new ArrayList<>(Collections.nCopies(size, false));
    }

    /** @returns the ship number */
    public int id() { return id; }
    /** @returns the ship size */
    public int size() { return size; }

    /* record the ship as having been hit at the given coordinates */
    public void hit(int x, int y) {
        // need to work out how many tiles from the ship's starting position the hit is at
        // that can be worked out from the difference between the starting X coord and this one
        // unless the ship runs N-S, in which case use the Y coord instead
        int offset;
        if (orientX != 0) {
            offset = (x - startX) / orientX;
        } else {
            offset = (y - startY) / orientY;
        }
        hits.set(offset, true);

        // if every tile of the ship has been hit, the ship is sunk
        sunk = hits.stream().allMatch(Predicate.isEqual(true));
    }

    public boolean isSunk() { return sunk; }

    // whether the ship has already been hit at the given coordinates
    public boolean wasHit(int x, int y) {
        int offset;
        if (orientX != 0) {
            offset = (x - startX) / orientX;
        } else {
            offset = (y - startY) / orientY;
        }
        return hits.get(offset);
    };

    // Place the ship in the sea.
    // choose a random starting position, and a random direction
    // if that doesn't fit, keep picking different positions and directions
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

    // Attempt to fit the ship into the sea, starting from a given position and
    // in a given direction
    // This is by far the most complicated part of the program.
    // It will start at the position provided, and attempt to occupy tiles in the
    // requested direction. If it does not fit, either because of the edge of the
    // sea, or because of ships already in place, it will try to extend the ship
    // in the opposite direction instead. If that is not possible, it fails.
    public boolean place(Sea s, int x, int y, int orient) {
        if (placed) {
            throw new RuntimeException("Program error - placed ship " + id + " twice");
        }
        switch(orient) {
        case ORIENT_E:                 // east is increasing X coordinate
            orientX = 1; orientY = 0;
            break;
        case ORIENT_SE:                // southeast is increasing X and Y
            orientX = 1; orientY = 1;
            break;
        case ORIENT_S:                 // south is increasing Y
            orientX = 0; orientY = 1;
            break;
        case ORIENT_SW:                // southwest is increasing Y but decreasing X
            orientX = -1; orientY = 1;
            break;
        default:
            throw new RuntimeException("Invalid orientation " + orient);
        }

        if (!s.isEmpty(x, y)) return false; // starting position is occupied - placing fails

        startX = x; startY = y;
        int tilesPlaced = 1;
        int nextX = startX;
        int nextY = startY;
        while (tilesPlaced < size) {
            if (extendShip(s, nextX, nextY, nextX + orientX, nextY + orientY)) {
                // It is clear to extend the ship forwards
                tilesPlaced += 1;
                nextX = nextX + orientX;
                nextY = nextY + orientY;
            } else {
                int backX = startX - orientX;
                int backY = startY - orientY;

                if (extendShip(s, startX, startY, backX, backY)) {
                    // We can move the ship backwards, so it can be one tile longer
                    tilesPlaced +=1;
                    startX = backX;
                    startY = backY;
                } else {
                    // Could not make it longer or move it backwards
                    return false;
                }
            }
        }

        // Mark in the sea which tiles this ship occupies
        for (int i = 0; i < size; ++i) {
            int sx = startX + i * orientX;
            int sy = startY + i * orientY;
            s.set(sx, sy, id);
        }

        placed = true;
        return true;
    }

    // Check whether a ship which already occupies the "from" coordinates,
    // can also occupy the "to" coordinates.
    // They must be within the sea area, empty, and not cause the ship to cross
    // over another ship
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
}
