import java.util.Random;
import java.util.Scanner;

public class TwentyThreeMatches {

    private static final int MATCH_COUNT_START = 23;
    private static final Random RAND = new Random();
    private final Scanner scan = new Scanner(System.in);

    public void startGame() {
        //Initialize values
        int cpuRemoves = 0;
        int matchesLeft = MATCH_COUNT_START;
        int playerRemoves = 0;

        //Flip coin and decide who goes first.
        CoinSide coinSide = flipCoin();
        if (coinSide == CoinSide.HEADS) {
            System.out.println(Messages.HEADS);
            matchesLeft -= 2;
        } else {
            System.out.println(Messages.TAILS);
        }

        // Game loop
        while (true) {
            //Show matches left if CPU went first or Player already removed matches
            if (coinSide == CoinSide.HEADS) {
                System.out.format(Messages.MATCHES_LEFT, matchesLeft);
            }
            coinSide = CoinSide.HEADS;

            // Player removes matches
            System.out.println(Messages.REMOVE_MATCHES_QUESTION);
            playerRemoves = turnOfPlayer();
            matchesLeft -= playerRemoves;
            System.out.format(Messages.REMAINING_MATCHES, matchesLeft);

            // If 1 match is left, the CPU has to take it. You win!
            if (matchesLeft <= 1) {
                System.out.println(Messages.WIN);
                return;
            }

            // CPU removes matches
            // At least two matches are left, because win condition above was not triggered.
            if (matchesLeft <= 4) {
                cpuRemoves = matchesLeft - 1;
            } else {
                cpuRemoves = 4 - playerRemoves;
            }
            System.out.format(Messages.CPU_TURN, cpuRemoves);
            matchesLeft -= cpuRemoves;

            // If 1 match is left, the Player has to take it. You lose!
            if (matchesLeft <= 1) {
                System.out.println(Messages.LOSE);
                return;
            }
        }
    }

    private CoinSide flipCoin() {
        return RAND.nextBoolean() ? CoinSide.HEADS : CoinSide.TAILS;
    }

    private int turnOfPlayer() {
        while (true) {
            int playerRemoves = scan.nextInt();
            // Handle invalid entries
            if ((playerRemoves > 3) || (playerRemoves <= 0)) {
                System.out.println(Messages.INVALID);
                continue;
            }
            return playerRemoves;
        }
    }

}
