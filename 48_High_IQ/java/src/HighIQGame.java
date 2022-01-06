import java.util.Scanner;

public class HighIQGame {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        do {
            new HighIQ(scanner).play();
            System.out.println("PLAY AGAIN (YES OR NO)");
        } while(scanner.nextLine().toLowerCase().equals("yes"));
    }
}
