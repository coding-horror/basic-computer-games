import java.util.Random;
import java.util.Scanner;

/**
 *  Port of Craps from BASIC to Java 17.
 */
public class Craps {
  public static final Random random = new Random();
  
  public static void main(String[] args) {
    System.out.println("""
                                                            CRAPS
                                          CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY
                                              
                                              
                           2,3,12 ARE LOSERS; 4,5,6,8,9,10 ARE POINTS; 7,11 ARE NATURAL WINNERS.
                           """);
    double winnings = 0.0;
    do {
      winnings = playCraps(winnings);
    } while (stillInterested(winnings));
    winningsReport(winnings);
  }

  public static double playCraps(double winnings) {
    double wager = getWager();
    System.out.println("I WILL NOW THROW THE DICE");
    int roll = rollDice();
    double payout = switch (roll) {
      case 7, 11 -> naturalWin(roll, wager);
      case 2, 3, 12 -> lose(roll, wager);
      default -> setPoint(roll, wager);
    };
    return winnings + payout;
  }

  public static int rollDice() {
    return random.nextInt(1, 7) + random.nextInt(1, 7);
  }

  private static double setPoint(int point, double wager) {
    System.out.printf("%1$ d IS THE POINT. I WILL ROLL AGAIN%n",point);
    return makePoint(point, wager);
  }

  private static double makePoint(int point, double wager) {
    int roll = rollDice();
    if (roll == 7)
      return lose(roll, wager);
    if (roll == point)
      return win(roll, wager);
    System.out.printf("%1$ d - NO POINT. I WILL ROLL AGAIN%n", roll);
    return makePoint(point, wager);  // recursive
  }

  private static double win(int roll, double wager) {
    double payout = 2 * wager;
    System.out.printf("%1$ d - A WINNER.........CONGRATS!!!!!!!!%n", roll);
    System.out.printf("%1$ d AT 2 TO 1 ODDS PAYS YOU...LET ME SEE...$%2$3.2f%n",
                      roll, payout);
    return payout;
  }

  private static double lose(int roll, double wager) {
    String msg = roll == 2 ? "SNAKE EYES.":"CRAPS";
    System.out.printf("%1$ d - %2$s...YOU LOSE.%n", roll, msg);
    System.out.printf("YOU LOSE $%3.2f%n", wager);
    return -wager;
  }

  public static double naturalWin(int roll, double wager) {
    System.out.printf("%1$ d - NATURAL....A WINNER!!!!%n", roll);
    System.out.printf("%1$ d PAYS EVEN MONEY, YOU WIN $%2$3.2f%n", roll, wager);
    return wager;
  }

  public static void winningsUpdate(double winnings) {
    System.out.println(switch ((int) Math.signum(winnings)) {
      case 1 -> "YOU ARE NOW AHEAD $%3.2f".formatted(winnings);
      case 0 -> "YOU ARE NOW EVEN AT 0";
      default -> "YOU ARE NOW UNDER $%3.2f".formatted(-winnings);
    });
  }

  public static void winningsReport(double winnings) {
    System.out.println(
        switch ((int) Math.signum(winnings)) {
          case 1 -> "CONGRATULATIONS---YOU CAME OUT A WINNER. COME AGAIN!";
          case 0 -> "CONGRATULATIONS---YOU CAME OUT EVEN, NOT BAD FOR AN AMATEUR";
          default -> "TOO BAD, YOU ARE IN THE HOLE. COME AGAIN.";
        }
    );
  }

  public static boolean stillInterested(double winnings) {
    System.out.print(" IF YOU WANT TO PLAY AGAIN PRINT 5 IF NOT PRINT 2 ");
    int fiveOrTwo = (int)getInput();
    winningsUpdate(winnings);
    return fiveOrTwo == 5;
  }

  public static double getWager() {
    System.out.print("INPUT THE AMOUNT OF YOUR WAGER. ");
    return getInput();
  }

  public static double getInput() {
    Scanner scanner = new Scanner(System.in);
    System.out.print("> ");
    while (true) {
      try {
        return scanner.nextDouble();
      } catch (Exception ex) {
        try {
          scanner.nextLine(); // flush whatever this non number stuff is.
        } catch (Exception ns_ex) { // received EOF (ctrl-d or ctrl-z if windows)
          System.out.println("END OF INPUT, STOPPING PROGRAM.");
          System.exit(1);
        }
      }
      System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE");
      System.out.print("> ");
    }
  }
}
