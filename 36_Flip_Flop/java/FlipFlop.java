import java.util.Scanner;
import java.lang.Math;

/**
 * Game of FlipFlop
 * <p>
 * Based on the BASIC game of FlipFlop here
 * https://github.com/coding-horror/basic-computer-games/blob/main/36%20Flip%20Flop/flipflop.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 *
 * Converted from BASIC to Java by Darren Cardenas.
 */

public class FlipFlop {

  private final Scanner scan;  // For user input

  private enum Step {
    RANDOMIZE, INIT_BOARD, GET_NUMBER, ILLEGAL_ENTRY, FLIP_POSITION, SET_X_FIRST, SET_X_SECOND,
    GENERATE_R_FIRST, GENERATE_R_SECOND, PRINT_BOARD, QUERY_RETRY
  }

  public FlipFlop() {

    scan = new Scanner(System.in);

  }  // End of constructor FlipFlop

  public void play() {

    showIntro();
    startGame();

  }  // End of method play

  private static void showIntro() {

    System.out.println(" ".repeat(31) + "FLIPFLOP");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("");

  }  // End of method showIntro

  private void startGame() {

    double mathVal = 0;
    double randVal = 0;
    double tmpVal = 0;

    int index = 0;
    int match = 0;
    int numFlip = 0;
    int numGuesses = 0;

    Step nextStep = Step.RANDOMIZE;

    String userResponse = "";

    String[] board = new String[21];

    System.out.println("THE OBJECT OF THIS PUZZLE IS TO CHANGE THIS:");
    System.out.println("");
    System.out.println("X X X X X X X X X X");
    System.out.println("");
    System.out.println("TO THIS:");
    System.out.println("");
    System.out.println("O O O O O O O O O O");
    System.out.println("");
    System.out.println("BY TYPING THE NUMBER CORRESPONDING TO THE POSITION OF THE");
    System.out.println("LETTER ON SOME NUMBERS, ONE POSITION WILL CHANGE, ON");
    System.out.println("OTHERS, TWO WILL CHANGE.  TO RESET LINE TO ALL X'S, TYPE 0");
    System.out.println("(ZERO) AND TO START OVER IN THE MIDDLE OF A GAME, TYPE ");
    System.out.println("11 (ELEVEN).");
    System.out.println("");

    // Begin outer while loop
    while (true) {

      // Begin switch
      switch (nextStep) {

        case RANDOMIZE:

          randVal = Math.random();

          System.out.println("HERE IS THE STARTING LINE OF X'S.");
          System.out.println("");

          numGuesses = 0;
          nextStep = Step.INIT_BOARD;
          break;

        case INIT_BOARD:

          System.out.println("1 2 3 4 5 6 7 8 9 10");
          System.out.println("X X X X X X X X X X");
          System.out.println("");

          // Avoid out of bounds error by starting at zero
          for (index = 0; index <= 10; index++) {
            board[index] = "X";
          }

          nextStep = Step.GET_NUMBER;
          break;

        case GET_NUMBER:

          System.out.print("INPUT THE NUMBER? ");
          userResponse = scan.nextLine();

          try {
            numFlip = Integer.parseInt(userResponse);
          }
          catch (NumberFormatException ex) {
            nextStep = Step.ILLEGAL_ENTRY;
            break;
          }

          // Command to start a new game
          if (numFlip == 11) {
            nextStep = Step.RANDOMIZE;
            break;
          }

          if (numFlip > 11) {
            nextStep = Step.ILLEGAL_ENTRY;
            break;
          }

          // Command to reset the board
          if (numFlip == 0) {
            nextStep = Step.INIT_BOARD;
            break;
          }

          if (match == numFlip) {
            nextStep = Step.FLIP_POSITION;
            break;
          }

          match = numFlip;

          if (board[numFlip].equals("O")) {
            nextStep = Step.SET_X_FIRST;
            break;
          }

          board[numFlip] = "O";
          nextStep = Step.GENERATE_R_FIRST;
          break;

        case ILLEGAL_ENTRY:
          System.out.println("ILLEGAL ENTRY--TRY AGAIN.");
          nextStep = Step.GET_NUMBER;
          break;

        case GENERATE_R_FIRST:

          mathVal = Math.tan(randVal + numFlip / randVal - numFlip) - Math.sin(randVal / numFlip) + 336
                    * Math.sin(8 * numFlip);

          tmpVal = mathVal - (int)Math.floor(mathVal);

          numFlip = (int)(10 * tmpVal);

          if (board[numFlip].equals("O")) {
            nextStep = Step.SET_X_FIRST;
            break;
          }

          board[numFlip] = "O";
          nextStep = Step.PRINT_BOARD;
          break;

        case SET_X_FIRST:
          board[numFlip] = "X";

          if (match == numFlip) {
            nextStep = Step.GENERATE_R_FIRST;
          } else {
            nextStep = Step.PRINT_BOARD;
          }
          break;

        case FLIP_POSITION:

          if (board[numFlip].equals("O")) {
            nextStep = Step.SET_X_SECOND;
            break;
          }

          board[numFlip] = "O";
          nextStep = Step.GENERATE_R_SECOND;
          break;

        case GENERATE_R_SECOND:

          mathVal = 0.592 * (1 / Math.tan(randVal / numFlip + randVal)) / Math.sin(numFlip * 2 + randVal)
                    - Math.cos(numFlip);

          tmpVal = mathVal - (int)mathVal;
          numFlip = (int)(10 * tmpVal);

          if (board[numFlip].equals("O")) {
            nextStep = Step.SET_X_SECOND;
            break;
          }

          board[numFlip] = "O";
          nextStep = Step.PRINT_BOARD;
          break;

        case SET_X_SECOND:

          board[numFlip] = "X";
          if (match == numFlip) {
            nextStep = Step.GENERATE_R_SECOND;
            break;
          }

          nextStep = Step.PRINT_BOARD;
          break;

        case PRINT_BOARD:
          System.out.println("1 2 3 4 5 6 7 8 9 10");

          for (index = 1; index <= 10; index++) {
            System.out.print(board[index] + " ");
          }

          numGuesses++;

          System.out.println("");

          for (index = 1; index <= 10; index++) {
            if (!board[index].equals("O")) {
              nextStep = Step.GET_NUMBER;
              break;
            }
          }

          if (nextStep == Step.GET_NUMBER) {
            break;
          }

          if (numGuesses > 12) {
            System.out.println("TRY HARDER NEXT TIME.  IT TOOK YOU " + numGuesses + " GUESSES.");
          } else {
            System.out.println("VERY GOOD.  YOU GUESSED IT IN ONLY " + numGuesses + " GUESSES.");
          }
          nextStep = Step.QUERY_RETRY;
          break;

        case QUERY_RETRY:

          System.out.print("DO YOU WANT TO TRY ANOTHER PUZZLE? ");
          userResponse = scan.nextLine();

          if (userResponse.toUpperCase().charAt(0) == 'N') {
            return;
          }
          System.out.println("");
          nextStep = Step.RANDOMIZE;
          break;

        default:
          System.out.println("INVALID STEP");
          nextStep = Step.QUERY_RETRY;
          break;

      }  // End of switch

    }  // End outer while loop

  }  // End of method startGame

  public static void main(String[] args) {

    FlipFlop game = new FlipFlop();
    game.play();

  }  // End of method main

}  // End of class FlipFlop
