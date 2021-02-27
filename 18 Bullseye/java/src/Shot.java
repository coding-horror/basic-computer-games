/**
 * This class records the percentage chance of a given type of shot
 * scoring specific points
 * see Bullseye class points calculation method where its used
 */
public class Shot {

    double[] chances;

    // Array of doubles are passed for a specific type of shot
    Shot(double[] shots) {
        chances = new double[shots.length];
        System.arraycopy(shots, 0, chances, 0, shots.length);
    }

    public double getShot(int index) {
        return chances[index];
    }
}
