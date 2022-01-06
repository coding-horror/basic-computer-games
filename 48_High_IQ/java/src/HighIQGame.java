import java.util.Scanner;

public class HighIQGame {
    public static void main(String[] args) {

        printInstructions();

        Scanner scanner = new Scanner(System.in);
        do {
            new HighIQ(scanner).play();
            System.out.println("PLAY AGAIN (YES OR NO)");
        } while(scanner.nextLine().equalsIgnoreCase("yes"));
    }

    public static void printInstructions() {
        System.out.println("HERE IS THE BOARD:");
        System.out.println("          !    !    !");
        System.out.println("         13   14   15\n");
        System.out.println("          !    !    !");
        System.out.println("         22   23   24\n");
        System.out.println("!    !    !    !    !    !    !");
        System.out.println("29   30   31   32   33   34   35\n");
        System.out.println("!    !    !    !    !    !    !");
        System.out.println("38   39   40   41   42   43   44\n");
        System.out.println("!    !    !    !    !    !    !");
        System.out.println("47   48   49   50   51   52   53\n");
        System.out.println("          !    !    !");
        System.out.println("         58   59   60\n");
        System.out.println("          !    !    !");
        System.out.println("         67   68   69");
        System.out.println("TO SAVE TYPING TIME, A COMPRESSED VERSION OF THE GAME BOARD");
        System.out.println("WILL BE USED DURING PLAY.  REFER TO THE ABOVE ONE FOR PEG");
        System.out.println("NUMBERS.  OK, LET'S BEGIN.");
    }
}
