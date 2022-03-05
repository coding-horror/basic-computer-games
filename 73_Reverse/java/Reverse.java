import java.util.Scanner;
import java.lang.Math;

/**
 * Game of Reverse
 * <p>
 * Based on the BASIC game of Reverse here
 * https://github.com/coding-horror/basic-computer-games/blob/main/73%20Reverse/reverse.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 *
 * Converted from BASIC to Java by Darren Cardenas.
 */

public class Reverse {

  private final int NUMBER_COUNT = 9;

  private final Scanner scan;  // For user input

  private enum Step {
    INITIALIZE, PERFORM_REVERSE, TRY_AGAIN, END_GAME
  }

  public Reverse() {

    scan = new Scanner(System.in);

  }  // End of constructor Reverse

  public void play() {

    showIntro();
    startGame();

  }  // End of method play

  private static void showIntro() {

    System.out.println(" ".repeat(31) + "REVERSE");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");
    System.out.println("REVERSE -- A GAME OF SKILL");
    System.out.println("");

  }  // End of method showIntro

  private void startGame() {

    int index = 0;
    int numMoves = 0;
    int numReverse = 0;
    int tempVal = 0;
    int[] numList = new int[NUMBER_COUNT + 1];

    Step nextStep = Step.INITIALIZE;

    String userResponse = "";

    System.out.print("DO YOU WANT THE RULES? ");
    userResponse = scan.nextLine();

    if (!userResponse.toUpperCase().equals("NO")) {

      this.printRules();
    }

    // Begin outer while loop
    while (true) {

    // Begin outer switch
    switch (nextStep) {

      case INITIALIZE:

        // Make a random list of numbers
       numList[1] = (int)((NUMBER_COUNT - 1) * Math.random() + 2);

         for (index = 2; index <= NUMBER_COUNT; index++) {

          // Keep generating lists if there are duplicates
          while (true) {

            numList[index] = (int)(NUMBER_COUNT * Math.random() + 1);

            // Search for duplicates
            if (!this.findDuplicates(numList, index)) {
              break;
            }
          }
        }

        System.out.println("");
        System.out.println("HERE WE GO ... THE LIST IS:");

        numMoves = 0;

        this.printBoard(numList);

        nextStep = Step.PERFORM_REVERSE;
        break;

      case PERFORM_REVERSE:

        System.out.print("HOW MANY SHALL I REVERSE? ");
        numReverse = Integer.parseInt(scan.nextLine());

        if (numReverse == 0) {

          nextStep = Step.TRY_AGAIN;

        } else if (numReverse > NUMBER_COUNT) {

          System.out.println("OOPS! TOO MANY! I CAN REVERSE AT MOST " + NUMBER_COUNT);
          nextStep = Step.PERFORM_REVERSE;

        } else {

          numMoves++;

          for (index = 1; index <= (int)(numReverse / 2.0); index++) {

            tempVal = numList[index];
            numList[index] = numList[numReverse - index + 1];
            numList[numReverse - index + 1] = tempVal;
          }

          this.printBoard(numList);

          nextStep = Step.TRY_AGAIN;

          // Check for a win
          for (index = 1; index <= NUMBER_COUNT; index++) {

            if (numList[index] != index) {
              nextStep = Step.PERFORM_REVERSE;
            }
          }

          if (nextStep == Step.TRY_AGAIN) {
            System.out.println("YOU WON IT IN " + numMoves + " MOVES!!!");
            System.out.println("");
          }
        }
        break;

      case TRY_AGAIN:

        System.out.println("");
        System.out.print("TRY AGAIN (YES OR NO)? ");
        userResponse = scan.nextLine();

        if (userResponse.toUpperCase().equals("YES")) {
          nextStep = Step.INITIALIZE;
        } else {
          nextStep = Step.END_GAME;
        }
        break;

      case END_GAME:

        System.out.println("");
        System.out.println("O.K. HOPE YOU HAD FUN!!");
        return;

      default:

        System.out.println("INVALID STEP");
        break;

      }  // End outer switch

    }  // End outer while loop

  }  // End of method startGame

  public boolean findDuplicates(int[] board, int length) {

    int index = 0;

    for (index = 1; index <= length - 1; index++) {

      // Identify duplicates
      if (board[length] == board[index]) {

        return true;  // Found a duplicate
      }
    }

    return false;  // No duplicates found

  }  // End of method findDuplicates

  public void printBoard(int[] board) {

    int index = 0;

    System.out.println("");

    for (index = 1; index <= NUMBER_COUNT; index++) {

      System.out.format("%2d", board[index]);
    }

    System.out.println("\n");

  }  // End of method printBoard

  public void printRules() {

    System.out.println("");
    System.out.println("THIS IS THE GAME OF 'REVERSE'.  TO WIN, ALL YOU HAVE");
    System.out.println("TO DO IS ARRANGE A LIST OF NUMBERS (1 THROUGH " + NUMBER_COUNT + ")");
    System.out.println("IN NUMERICAL ORDER FROM LEFT TO RIGHT.  TO MOVE, YOU");
    System.out.println("TELL ME HOW MANY NUMBERS (COUNTING FROM THE LEFT) TO");
    System.out.println("REVERSE.  FOR EXAMPLE, IF THE CURRENT LIST IS:");
    System.out.println("");
    System.out.println("2 3 4 5 1 6 7 8 9");
    System.out.println("");
    System.out.println("AND YOU REVERSE 4, THE RESULT WILL BE:");
    System.out.println("");
    System.out.println("5 4 3 2 1 6 7 8 9");
    System.out.println("");
    System.out.println("NOW IF YOU REVERSE 5, YOU WIN!");
    System.out.println("");
    System.out.println("1 2 3 4 5 6 7 8 9");
    System.out.println("");
    System.out.println("NO DOUBT YOU WILL LIKE THIS GAME, BUT");
    System.out.println("IF YOU WANT TO QUIT, REVERSE 0 (ZERO).");
    System.out.println("");

  }  // End of method printRules

  public static void main(String[] args) {

    Reverse game = new Reverse();
    game.play();

  }  // End of method main

}  // End of class Reverse
