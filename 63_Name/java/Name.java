import java.util.Arrays;
import java.util.Scanner;

public class Name {

    public static void printempty() { System.out.println(" "); }

    public static void print(String toprint) { System.out.println(toprint); }

    public static void main(String[] args) {
        print("                                          NAME");
        print("                         CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        printempty();
        printempty();
        print("HELLO.");
        print("MY NAME iS CREATIVE COMPUTER.");
        print("WHATS YOUR NAME? (FIRST AND LAST)");

        Scanner namesc = new Scanner(System.in);
        String name = namesc.nextLine();

        String namereversed = new StringBuilder(name).reverse().toString();

        char namesorted[] = name.toCharArray();
        Arrays.sort(namesorted);

        printempty();
        print("THANK YOU, " + namereversed);
        printempty();
        print("OOPS!  I GUESS I GOT IT BACKWARDS.  A SMART");
        print("COMPUTER LIKE ME SHOULDN'T MAKE A MISTAKE LIKE THAT!");
        printempty();
        printempty();
        print("BUT I JUST NOTICED YOUR LETTERS ARE OUT OF ORDER.");

        print("LET'S PUT THEM IN ORDER LIKE THIS: " + new String(namesorted));
        printempty();
        printempty();

        print("DON'T YOU LIKE THAT BETTER?");
        printempty();

        Scanner agreementsc = new Scanner(System.in);
        String agreement = agreementsc.nextLine();

        if (agreement.equalsIgnoreCase("yes")) {
            print("I KNEW YOU'D AGREE!!");
        } else {
            print("I'M SORRY YOU DON'T LIKE IT THAT WAY.");
            printempty();
            print("I REALLY ENJOYED MEETING YOU, " + name);
            print("HAVE A NICE DAY!");
        }
    }
}
