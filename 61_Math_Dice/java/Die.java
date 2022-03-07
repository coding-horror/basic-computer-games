import java.util.Random;

public class Die {
    private static final int DEFAULT_SIDES = 6;
    private int faceValue;
    private int sides;
    private Random generator = new Random();

    /**
     * Construct a new Die with default sides
     */
    public Die() {
        this.sides = DEFAULT_SIDES;
        this.faceValue = 1 + generator.nextInt(sides);
    }

    /**
     * Generate a new random number between 1 and sides to be stored in faceValue
     */
    private void throwDie() {
        this.faceValue = 1 + generator.nextInt(sides);
    }


    /**
     * @return the faceValue
     */
    public int getFaceValue() {
        return faceValue;
    }


    public void printDie() {
        throwDie();
        int x = this.getFaceValue();

        System.out.println(" ----- ");

        if(x==4||x==5||x==6) {
            printTwo();
        } else if(x==2||x==3) {
            System.out.println("| *   |");
        } else {
            printZero();
        }

        if(x==1||x==3||x==5) {
            System.out.println("|  *  |");
        } else if(x==2||x==4) {
            printZero();
        } else {
            printTwo();
        }

        if(x==4||x==5||x==6) {
            printTwo();
        } else if(x==2||x==3) {
            System.out.println("|   * |");
        } else {
            printZero();
        }

        System.out.println(" ----- ");
    }

    private void printZero() {
        System.out.println("|     |");
    }

    private void printTwo() {
        System.out.println("| * * |");
    }
}
