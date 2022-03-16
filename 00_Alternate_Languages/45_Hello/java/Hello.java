import java.util.Scanner;

/**
 * Game of Hello
 * <p>
 * Based on the BASIC game of Hello here
 * https://github.com/coding-horror/basic-computer-games/blob/main/45%20Hello/hello.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 *
 * Converted from BASIC to Java by Darren Cardenas.
 */

public class Hello {

  private static final int MONEY_WAIT_MS = 3000;

  private final boolean goodEnding = false;

  private final Scanner scan;  // For user input

  public Hello() {

    scan = new Scanner(System.in);

  }  // End of constructor Hello

  public void play() {

    showIntro();
    startGame();

  }  // End of method play

  private static void showIntro() {

    System.out.println(" ".repeat(32) + "HELLO");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");

  }  // End of method showIntro

  private void startGame() {

    boolean moreProblems = true;

    String userCategory = "";
    String userName = "";
    String userResponse = "";

    // Name question
    System.out.println("HELLO.  MY NAME IS CREATIVE COMPUTER.\n\n");
    System.out.print("WHAT'S YOUR NAME? ");
    userName = scan.nextLine();
    System.out.println("");

    // Enjoyment question
    System.out.print("HI THERE, " + userName + ", ARE YOU ENJOYING YOURSELF HERE? ");

    while (true) {
      userResponse = scan.nextLine();
      System.out.println("");

      if (userResponse.toUpperCase().equals("YES")) {
        System.out.println("I'M GLAD TO HEAR THAT, " + userName + ".\n");
        break;
      }
      else if (userResponse.toUpperCase().equals("NO")) {
        System.out.println("OH, I'M SORRY TO HEAR THAT, " + userName + ". MAYBE WE CAN");
        System.out.println("BRIGHTEN UP YOUR VISIT A BIT.");
        break;
      }
      else {
        System.out.println(userName + ", I DON'T UNDERSTAND YOUR ANSWER OF '" + userResponse + "'.");
        System.out.print("PLEASE ANSWER 'YES' OR 'NO'.  DO YOU LIKE IT HERE? ");
      }
    }

    // Category question
    System.out.println("");
    System.out.println("SAY, " + userName + ", I CAN SOLVE ALL KINDS OF PROBLEMS EXCEPT");
    System.out.println("THOSE DEALING WITH GREECE.  WHAT KIND OF PROBLEMS DO");
    System.out.print("YOU HAVE (ANSWER SEX, HEALTH, MONEY, OR JOB)? ");

    while (moreProblems) {
      userCategory = scan.nextLine();
      System.out.println("");

      // Sex advice
      if (userCategory.toUpperCase().equals("SEX")) {
        System.out.print("IS YOUR PROBLEM TOO MUCH OR TOO LITTLE? ");
        userResponse = scan.nextLine();
        System.out.println("");

        while (true) {
          if (userResponse.toUpperCase().equals("TOO MUCH")) {
            System.out.println("YOU CALL THAT A PROBLEM?!!  I SHOULD HAVE SUCH PROBLEMS!");
            System.out.println("IF IT BOTHERS YOU, " + userName + ", TAKE A COLD SHOWER.");
            break;
          }
          else if (userResponse.toUpperCase().equals("TOO LITTLE")) {
            System.out.println("WHY ARE YOU HERE IN SUFFERN, " + userName + "?  YOU SHOULD BE");
            System.out.println("IN TOKYO OR NEW YORK OR AMSTERDAM OR SOMEPLACE WITH SOME");
            System.out.println("REAL ACTION.");
            break;
          }
          else {
            System.out.println("DON'T GET ALL SHOOK, " + userName + ", JUST ANSWER THE QUESTION");
            System.out.print("WITH 'TOO MUCH' OR 'TOO LITTLE'.  WHICH IS IT? ");
            userResponse = scan.nextLine();
          }
        }
      }
      // Health advice
      else if (userCategory.toUpperCase().equals("HEALTH")) {
        System.out.println("MY ADVICE TO YOU " + userName + " IS:");
        System.out.println("     1.  TAKE TWO ASPRIN");
        System.out.println("     2.  DRINK PLENTY OF FLUIDS (ORANGE JUICE, NOT BEER!)");
        System.out.println("     3.  GO TO BED (ALONE)");
      }
      // Money advice
      else if (userCategory.toUpperCase().equals("MONEY")) {
        System.out.println("SORRY, " + userName + ", I'M BROKE TOO.  WHY DON'T YOU SELL");
        System.out.println("ENCYCLOPEADIAS OR MARRY SOMEONE RICH OR STOP EATING");
        System.out.println("SO YOU WON'T NEED SO MUCH MONEY?");
      }
      // Job advice
      else if (userCategory.toUpperCase().equals("JOB")) {
        System.out.println("I CAN SYMPATHIZE WITH YOU " + userName + ".  I HAVE TO WORK");
        System.out.println("VERY LONG HOURS FOR NO PAY -- AND SOME OF MY BOSSES");
        System.out.println("REALLY BEAT ON MY KEYBOARD.  MY ADVICE TO YOU, " + userName + ",");
        System.out.println("IS TO OPEN A RETAIL COMPUTER STORE.  IT'S GREAT FUN.");
      }
      else {
        System.out.println("OH, " + userName + ", YOUR ANSWER OF " + userCategory + " IS GREEK TO ME.");
      }

      // More problems question
      while (true) {
        System.out.println("");
        System.out.print("ANY MORE PROBLEMS YOU WANT SOLVED, " + userName + "? ");
        userResponse = scan.nextLine();
        System.out.println("");

        if (userResponse.toUpperCase().equals("YES")) {
          System.out.print("WHAT KIND (SEX, MONEY, HEALTH, JOB)? ");
          break;
        }
        else if (userResponse.toUpperCase().equals("NO")) {
          moreProblems = false;
          break;
        }
        else {
          System.out.println("JUST A SIMPLE 'YES' OR 'NO' PLEASE, " + userName + ".");
        }
      }
    }

    // Payment question
    System.out.println("");
    System.out.println("THAT WILL BE $5.00 FOR THE ADVICE, " + userName + ".");
    System.out.println("PLEASE LEAVE THE MONEY ON THE TERMINAL.");

    // Pause
    try {
      Thread.sleep(MONEY_WAIT_MS);
    } catch (Exception e) {
      System.out.println("Caught Exception: " + e.getMessage());
    }

    System.out.println("\n\n");

    while (true) {
      System.out.print("DID YOU LEAVE THE MONEY? ");
      userResponse = scan.nextLine();
      System.out.println("");

      if (userResponse.toUpperCase().equals("YES")) {
        System.out.println("HEY, " + userName + "??? YOU LEFT NO MONEY AT ALL!");
        System.out.println("YOU ARE CHEATING ME OUT OF MY HARD-EARNED LIVING.");
        System.out.println("");
        System.out.println("WHAT A RIP OFF, " + userName + "!!!\n");
        break;
      }
      else if (userResponse.toUpperCase().equals("NO")) {
        System.out.println("THAT'S HONEST, " + userName + ", BUT HOW DO YOU EXPECT");
        System.out.println("ME TO GO ON WITH MY PSYCHOLOGY STUDIES IF MY PATIENTS");
        System.out.println("DON'T PAY THEIR BILLS?");
        break;
      }
      else {
        System.out.println("YOUR ANSWER OF '" + userResponse + "' CONFUSES ME, " + userName + ".");
        System.out.println("PLEASE RESPOND WITH 'YES' OR 'NO'.");
      }
    }

    // Legacy included unreachable code
    if (goodEnding) {
      System.out.println("NICE MEETING YOU, " + userName + ", HAVE A NICE DAY.");
    }
    else {
      System.out.println("");
      System.out.println("TAKE A WALK, " + userName + ".\n");
    }

  }  // End of method startGame

  public static void main(String[] args) {

    Hello hello = new Hello();
    hello.play();

  }  // End of method main

}  // End of class Hello
