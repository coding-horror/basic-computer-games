/**
 * Pelt object - tracks the name and number of pelts the player has for this pelt type
 */
public class Pelt {

    private final String name;
    private int number;

    public Pelt(String name, int number) {
        this.name = name;
        this.number = number;
    }

    public void setPeltCount(int pelts) {
        this.number = pelts;
    }

    public int getNumber() {
        return this.number;
    }

    public String getName() {
        return this.name;
    }

    public void lostPelts() {
        this.number = 0;
    }
}
