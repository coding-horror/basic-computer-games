public class Pelt {

    private String name;
    private int number;

    public Pelt(String name, int number) {
        this.name = name;
        this.number = number;
    }

    public int getNumber() {
        return this.number;
    }

    public String getName() {
        return this.name;
    }

    public Object lostPelts() {
        this.number = 0;
        return null;
    }
}
