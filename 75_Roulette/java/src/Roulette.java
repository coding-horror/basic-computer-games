import java.io.PrintStream;

public class Roulette {

    private PrintStream out;

    public static void main(String[] args) {
        new Roulette(System.out).play();
    }

    public Roulette(PrintStream out) {
        this.out = out;
    }

    public void play() {
        printInstructions();
    }


    public void printInstructions() {
        System.out.println();
        System.out.println( "THIS IS THE BETTING LAYOUT");
        System.out.println( "  (*=RED)");
        System.out.println();
        System.out.println( " 1*    2     3*");
        System.out.println( " 4     5*    6 ");
        System.out.println( " 7*    8     9*");
        System.out.println( "10    11    12*");
        System.out.println( "---------------");
        System.out.println( "13    14*   15 ");
        System.out.println( "16*   17    18*");
        System.out.println( "19*   20    21*");
        System.out.println( "22    23*   24 ");
        System.out.println( "---------------");
        System.out.println( "25*   26    27*");
        System.out.println( "28    29    30*");
        System.out.println( "31    32*   33 ");
        System.out.println( "34*   35    36*");
        System.out.println( "---------------");
        System.out.println( "    00    0    ");
        System.out.println();
        System.out.println( "TYPES OF BETS");
        System.out.println();
        System.out.println( "THE NUMBERS 1 TO 36 SIGNIFY A STRAIGHT BET");
        System.out.println( "ON THAT NUMBER.");
        System.out.println( "THESE PAY OFF 35:1");
        System.out.println();
        System.out.println( "THE 2:1 BETS ARE:");
        System.out.println( " 37) 1-12     40) FIRST COLUMN");
        System.out.println( " 38) 13-24    41) SECOND COLUMN");
        System.out.println( " 39) 25-36    42) THIRD COLUMN");
        System.out.println();
        System.out.println( "THE EVEN MONEY BETS ARE:");
        System.out.println( " 43) 1-18     46) ODD");
        System.out.println( " 44) 19-36    47) RED");
        System.out.println( " 45) EVEN     48) BLACK");
        System.out.println();
        System.out.println( " 49)0 AND 50)00 PAY OFF 35:1");
        System.out.println( " NOTE: 0 AND 00 DO NOT COUNT UNDER ANY");
        System.out.println( "       BETS EXCEPT THEIR OWN.");
        System.out.println();
        System.out.println( "WHEN I ASK FOR EACH BET, TYPE THE NUMBER");
        System.out.println( "AND THE AMOUNT, SEPARATED BY A COMMA.");
        System.out.println( "FOR EXAMPLE: TO BET $500 ON BLACK, TYPE 48,500");
        System.out.println( "WHEN I ASK FOR A BET.");
        System.out.println();
        System.out.println( "THE MINIMUM BET IS $5, THE MAXIMUM IS $500.");
    }
}
