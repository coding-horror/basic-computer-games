import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.Random;
import java.util.function.Predicate;

class Ship {
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

