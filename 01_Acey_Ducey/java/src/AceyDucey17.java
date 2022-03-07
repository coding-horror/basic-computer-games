import java.util.Random;
import java.util.Scanner;

/**
 * A modern version (JDK17) of ACEY DUCEY using post Java 8 features. Notes
 * regarding new java features or differences in the original basic
 * implementation are numbered and at the bottom of this code.
 * The goal is to recreate the exact look and feel of the original program
 * minus a large glaring bug in the original code that lets you cheat.
 */
public class AceyDucey17 {

  public static void main(String[] args) {
    // notes [1]
    System.out.println("""
                                        ACEY DUCEY CARD GAME
                             CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY


              ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER
              THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP
              YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING
              ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE
              A VALUE BETWEEN THE FIRST TWO.
              IF YOU DO NOT WANT TO BET, INPUT A 0""");

    do {
      playGame();
    } while (stillInterested());
    System.out.println("O.K., HOPE YOU HAD FUN!");
  }

  public static void playGame() {
    int cashOnHand = 100; // our only mutable variable  note [11]
    System.out.println("YOU NOW HAVE  "+ cashOnHand +"  DOLLARS.");// note [6]
    while (cashOnHand > 0) {
      System.out.println();
      System.out.println("HERE ARE YOUR NEXT TWO CARDS:");

      final Card lowCard = Card.getRandomCard(2, Card.KING); //note [3]
      System.out.println(lowCard);
      final Card highCard = Card.getRandomCard(lowCard.rank() + 1, Card.ACE);
      System.out.println(highCard);

      final int bet = getBet(cashOnHand);
      final int winnings = determineWinnings(lowCard,highCard,bet);
      cashOnHand += winnings;
      if(winnings != 0 || cashOnHand != 0){  //note [2]
        System.out.println("YOU NOW HAVE  "+ cashOnHand +"  DOLLARS.");//note [6]
      }
    }
  }

  public static int determineWinnings(Card lowCard, Card highCard, int bet){
    if (bet <= 0) {    // note [5]
      System.out.println("CHICKEN!!");
      return 0;
    }
    Card nextCard = Card.getRandomCard(2, Card.ACE);
    System.out.println(nextCard);
    if(nextCard.between(lowCard,highCard)){
      System.out.println("YOU WIN!!!");
      return bet;
    }
    System.out.println("SORRY, YOU LOSE");
    return -bet;
  }

  public static boolean stillInterested(){
    System.out.println();
    System.out.println();
    System.out.println("SORRY, FRIEND, BUT YOU BLEW YOUR WAD.");
    System.out.println();
    System.out.println();
    System.out.print("TRY AGAIN (YES OR NO)? ");
    Scanner input = new Scanner(System.in);
    boolean playAgain = input.nextLine()
                             .toUpperCase()
                             .startsWith("Y"); // note [9]
    System.out.println();
    System.out.println();
    return playAgain;
  }

  public static int getBet(int cashOnHand){
    int bet;
    do{
      System.out.println();
      System.out.print("WHAT IS YOUR BET? ");
      bet = inputNumber();
      if (bet > cashOnHand) {
        System.out.println("SORRY, MY FRIEND, BUT YOU BET TOO MUCH.");
        System.out.println("YOU HAVE ONLY  "+cashOnHand+"  DOLLARS TO BET.");
      }
    }while(bet > cashOnHand);
    return bet;
  }

  public static int inputNumber() {
    final Scanner input = new Scanner(System.in);
    // set to negative to mark as not entered yet in case of input error.
    int number = -1;
    while (number < 0) {
      try {
        number = input.nextInt();
      } catch(Exception ex) {   // note [7]
        System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE");
        System.out.print("? ");
        try{
          input.nextLine();
        }
        catch(Exception ns_ex){ // received EOF (ctrl-d or ctrl-z if windows)
          System.out.println("END OF INPUT, STOPPING PROGRAM.");
          System.exit(1);
        }
      }
    }
    return number;
  }

  record Card(int rank){
    // Some constants to describe face cards.
    public static final int JACK = 11, QUEEN = 12, KING = 13, ACE = 14;
    private static final Random random = new Random();

    public static Card getRandomCard(int from, int to){
      return new Card(random.nextInt(from, to+1));  // note [4]
    }

    public boolean between(Card lower, Card higher){
      return lower.rank() < this.rank() && this.rank() < higher.rank();
    }

    @Override
    public String toString() { // note [13]
      return switch (rank) {
        case JACK -> "JACK";
        case QUEEN -> "QUEEN";
        case KING -> "KING";
        case ACE -> "ACE\n"; // note [10]
        default -> " "+rank+" "; // note [6]
      };
    }
  }

  /*
    Notes:
    1. Multiline strings, a.k.a. text blocks, were added in JDK15.
    2. The original game only displays the players balance if it changed,
       which it does not when the player chickens out and bets zero.
       It also doesn't display the balance when it becomes zero because it has
       a more appropriate message: Sorry, You Lose.
    3. To pick two cards to show, the original BASIC implementation has a
       bug that could cause a race condition if the RND function never chose
       a lower number first and higher number second. It loops infinitely
       re-choosing random numbers until the condition is met of the first
       one being lower. The logic is changed a bit here so that the first
       card picked is anything but an ACE, the highest possible card,
       and then the second card is between the just picked first card upto
       and including the ACE.
    4. Random.nextInt(origin, bound) was added in JDK17, and allows to
       directly pick a range for a random integer to be generated. The second
       parameter is exclusive of the range and thus why they are stated with
       +1's to the face card.
    5. The original BASIC implementation has a bug that allows negative value
       bets. Since you can't bet MORE cash than you have you can always bet
       less including a very, very large negative value. You would do this when
       the chances of winning are slim or zero since losing a hand SUBTRACTS
       your bet from your cash; subtracting a negative number actually ADDS
       to your cash, potentially making you an instant billionaire.
       This loophole is now closed.
    6. The subtle behavior of the BASIC PRINT command causes a space to be
       printed before all positive numbers as well as a trailing space. Any
       place a non-face card or the players balance is printed has extra space
       to mimic this behavior.
    7. Errors on input were probably specific to the interpreter. This program
       tries to match the Vintage Basic interpreter's error messages. The final
       input.nextLine() command exists to clear the blockage of whatever
       non-number input was entered.  But even that could fail if the user
       types Ctrl-D (windows Ctrl-Z), signifying an EOF (end of file) and thus
       the closing of STDIN channel. The original program on an EOF signal prints
       "END OF INPUT IN LINE 660" and thus we cover it roughly the same way.
       All of this is necessary to avoid a messy stack trace from being
       printed as the program crashes.
    9. The original game only accepted a full upper case "YES" to continue
       playing if bankrupted. This program is more lenient and will accept
       any input that starts with the letter 'y', uppercase or not.
   10. The original game prints an extra blank line if the card is an ACE. There
       is seemingly no rationale for this.
   11. Modern java best practices are edging toward a more functional paradigm
       and as such, mutating state is discouraged. All other variables besides
       the cashOnHand are final and initialized only once.
   12. Refactoring of the concept of a card is done with a record. Records were
       introduced in JDK14. Card functionality is encapsulated in this example
       of a record.  An enum could be a better alternative since there are
       technically only 13 cards possible.
   13. Switch expressions were introduced as far back as JDK12 but continue to
       be refined for clarity, exhaustiveness. As of JDK17 pattern matching
       for switch expressions can be accessed by enabling preview features.
   */
}
