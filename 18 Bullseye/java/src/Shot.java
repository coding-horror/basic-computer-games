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
        for(int i=0; i<shots.length; i++) {
            chances[i] = shots[i];
        }
    }

    public double getShot(int index) {
        return chances[index];
    }
}
