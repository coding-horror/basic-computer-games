import java.util.Arrays;
import java.util.HashSet;
import java.util.Random;

// The roulette wheel
public class Wheel {
    // List the numbers which are black
    private HashSet<Integer> black = new HashSet<>(Arrays.asList(new Integer[] { 1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36 }));

    private Random random = new Random();
    private int pocket = 38;

    public static final int ZERO=0;
    public static final int BLACK=1;
    public static final int RED=2;

    // Set up a wheel. You call "spin", and then can check the result.
    public Wheel() {
    }

    // Cheat / test mode
    void setSeed(long l) {
        random.setSeed(l);
    }

    // Spin the wheel onto a new random value.
    public void spin() {
        // keep spinning for a while
        do {
            try {
                // 1 second delay. Where it stops, nobody knows
                Thread.sleep(1000);
            }
            catch (InterruptedException e) {}

            pocket = random.nextInt(38) + 1;
        } while (random.nextInt(4) > 0); // keep spinning until it stops
    }

    // The string representation of the number; 1-36, 0, or 00
    public String value() {
        if (pocket == 37) return "0";
        else if (pocket == 38) return "00";
        else return String.valueOf(pocket);
    }

    // True if either 0 or 00 is hit
    public boolean zero() {
        return (pocket > 36);
    }

    // True if anything other than 0 or 00 is hit
    public boolean isNumber() {
        return (pocket < 37);
    }

    // The number rolled
    public int number() {
        if (zero()) return 0;
        else return pocket;
    }

    // Either ZERO, BLACK, or RED
    public int color() {
        if (zero()) return ZERO;
        else if (black.contains(pocket)) return BLACK;
        else return RED;
    }
}
