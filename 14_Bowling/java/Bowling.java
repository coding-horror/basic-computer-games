import java.util.Scanner;
import java.lang.Math;

/**
 * Game of Bowling
 * <p>
 * Based on the BASIC game of Bowling here
 * https://github.com/coding-horror/basic-computer-games/blob/main/14%20Bowling/bowling.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 *
 * Converted from BASIC to Java by Darren Cardenas.
 */

public class Bowling {

  private final Scanner scan;  // For user input

  public Bowling() {

    scan = new Scanner(System.in);

  }  // End of constructor Bowling

  public void play() {

    showIntro();
    startGame();

  }  // End of method play

  private static void showIntro() {

    System.out.println(" ".repeat(33) + "BOWL");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");

  }  // End of method showIntro

  private void startGame() {

    int ball = 0;
    int bell = 0;
    int frame = 0;
    int ii = 0;  // Loop iterator
    int jj = 0;  // Loop iterator
    int kk = 0;  // Loop iterator
    int numPlayers = 0;
    int pinsDownBefore = 0;
    int pinsDownNow = 0;
    int player = 0;
    int randVal = 0;
    int result = 0;

    int[] pins = new int[16];

    int[][] scores = new int[101][7];

    String userResponse = "";

    System.out.println("WELCOME TO THE ALLEY");
    System.out.println("BRING YOUR FRIENDS");
    System.out.println("OKAY LET'S FIRST GET ACQUAINTED");
    System.out.println("");
    System.out.println("THE INSTRUCTIONS (Y/N)");
    System.out.print("? ");

    userResponse = scan.nextLine();

    if (userResponse.toUpperCase().equals("Y")) {
      printRules();
    }

    System.out.print("FIRST OF ALL...HOW MANY ARE PLAYING? ");
    numPlayers = Integer.parseInt(scan.nextLine());

    System.out.println("");
    System.out.println("VERY GOOD...");

    // Begin outer while loop
    while (true) {

      for (ii = 1; ii <= 100; ii++) {
        for (jj = 1; jj <= 6; jj++) {
          scores[ii][jj] = 0;
        }
      }

      frame = 1;

      // Begin frame while loop
      while (frame < 11) {

        // Begin loop through all players
        for (player = 1; player <= numPlayers; player++) {

          pinsDownBefore = 0;
          ball = 1;
          result = 0;

          for (ii = 1; ii <= 15; ii++) {
            pins[ii] = 0;
          }

          while (true) {

            // Ball generator using mod '15' system

            System.out.println("TYPE ROLL TO GET THE BALL GOING.");
            System.out.print("? ");
            scan.nextLine();

            kk = 0;
            pinsDownNow = 0;

            for (ii = 1; ii <= 20; ii++) {

              randVal = (int)(Math.random() * 100) + 1;

              for (jj = 1; jj <= 10; jj++) {

                if (randVal < 15 * jj) {
                  break;
                }
              }
              pins[15 * jj - randVal] = 1;
            }

            // Pin diagram

            System.out.println("PLAYER: " + player + " FRAME: " + frame + " BALL: " + ball);

            for (ii = 0; ii <= 3; ii++) {

              System.out.println("");

              System.out.print(" ".repeat(ii));

              for (jj = 1; jj <= 4 - ii; jj++) {

                kk++;

                if (pins[kk] == 1) {

                  System.out.print("O ");

                } else {

                  System.out.print("+ ");
                }
              }
            }

            System.out.println("");

            // Roll analysis

            for (ii = 1; ii <= 10; ii++) {
              pinsDownNow += pins[ii];
            }

            if (pinsDownNow - pinsDownBefore == 0) {
              System.out.println("GUTTER!!");
            }

            if (ball == 1 && pinsDownNow == 10) {
              System.out.println("STRIKE!!!!!");

              // Ring bell
              for (bell = 1; bell <= 4; bell++) {
                System.out.print("\007");
                try {
                  Thread.sleep(500);
                } catch (InterruptedException e) {
                  Thread.currentThread().interrupt();
                }
              }
              result = 3;
            }

            if (ball == 2 && pinsDownNow == 10) {
              System.out.println("SPARE!!!!");
              result = 2;
            }

            if (ball == 2 && pinsDownNow < 10) {
              System.out.println("ERROR!!!");
              result = 1;
            }

            if (ball == 1 && pinsDownNow < 10) {
              System.out.println("ROLL YOUR 2ND BALL");
            }

            // Storage of the scores

            System.out.println("");

            scores[frame * player][ball] = pinsDownNow;

            if (ball != 2) {
              ball = 2;
              pinsDownBefore = pinsDownNow;

              if (result != 3) {
                scores[frame * player][ball] = pinsDownNow - pinsDownBefore;
                if (result == 0) {
                  continue;
                }
              } else {
                scores[frame * player][ball] = pinsDownNow;
              }

            }
            break;
          }

          scores[frame * player][3] = result;

        }  // End loop through all players

        frame++;

      }  // End frame while loop

      System.out.println("FRAMES");

      System.out.print(" ");
      for (ii = 1; ii <= 10; ii++) {
        System.out.print(ii + " ");
      }

      System.out.println("");

      for (player = 1; player <= numPlayers; player++) {
        for (ii = 1; ii <= 3; ii++) {
          System.out.print(" ");
          for (jj = 1; jj <= 10; jj++) {
            System.out.print (scores[jj * player][ii] + " ");
          }
          System.out.println("");
        }
        System.out.println("");
      }

      System.out.println("DO YOU WANT ANOTHER GAME");
      System.out.print("? ");

      userResponse = scan.nextLine();

      if (!String.valueOf(userResponse.toUpperCase().charAt(0)).equals("Y")) {
        break;
      }

    }  // End outer while loop

  }  // End of method startGame

  public static void printRules() {

    System.out.println("THE GAME OF BOWLING TAKES MIND AND SKILL.DURING THE GAME");
    System.out.println("THE COMPUTER WILL KEEP SCORE.YOU MAY COMPETE WITH");
    System.out.println("OTHER PLAYERS[UP TO FOUR].YOU WILL BE PLAYING TEN FRAMES");
    System.out.println("ON THE PIN DIAGRAM 'O' MEANS THE PIN IS DOWN...'+' MEANS THE");
    System.out.println("PIN IS STANDING.AFTER THE GAME THE COMPUTER WILL SHOW YOUR");
    System.out.println("SCORES .");

  }  // End of method printRules

  public static void main(String[] args) {

    Bowling game = new Bowling();
    game.play();

  }  // End of method main

}  // End of class Bowling
