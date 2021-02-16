/******************************************************************************
*
* Bagels
*
* From: BASIC Computer Games (1978)
*       Edited by David H. Ahl
*
* "In this game, the computer picks a 3-digit secret number using
*  the digits 0 to 9 and you attempt to guess what it is.  You are
*  allowed up to twenty guesses.  No digit is repeated.  After
*  each guess the computer will give you clues about your guess
*  as follows:
*
*  PICO     One digit is correct, but in the wrong place
*  FERMI    One digit is in the correct place
*  BAGELS   No digit is correct
*
* "You will learn to draw inferences from the clues and, with
*  practice, you'll learn to improve your score.  There are several
*  good strategies for playing Bagels.  After you have found a good
*  strategy, see if you can improve it.  Or try a different strategy
*  altogether and see if it is any better.  While the program allows
*  up to twenty guesses, if you use a good strategy it should not
*  take more than eight guesses to get any number.
*
* "The original authors of this program are D. Resek and P. Rowe of
*  the Lawrence Hall of Science, Berkeley, California."
*
* Java port by Jeff Jetton, 2020, based on an earlier Python port
*
******************************************************************************/

import java.util.Scanner;

public class Bagels {

  public static void main(String[] args) {
    
    int gamesWon = 0;

    // Intro text
    System.out.println("\n\n                Bagels");
    System.out.println("Creative Computing  Morristown, New Jersey");
    System.out.println("\n\n");
    System.out.print("Would you like the rules (Yes or No)? ");
    
    // Need instructions?
    Scanner scan = new Scanner(System.in);
    String s = scan.nextLine();
    if (s.length() == 0 || s.toUpperCase().charAt(0) != 'N') {
      System.out.println();
      BagelInstructions.printInstructions();
    }
    
    // Loop for playing multiple games
    boolean stillPlaying = true;
    while(stillPlaying) {
      
      // Set up a new game
      BagelGame game = new BagelGame();
      System.out.println("\nO.K.  I have a number in mind.");
    
      // Loop guess and responsses until game is over
      while (!game.isOver()) {
        String guess = getValidGuess(game);
        String response = game.makeGuess(guess);
        // Don't print a response if the game is won
        if (!game.isWon()) {
          System.out.println(response);
        }
      }
    
      // Game is over. But did we win or lose?
      if (game.isWon()) {
        System.out.println("You got it!!!\n");
        gamesWon++;
      } else {
        System.out.println("Oh well");
        System.out.print("That's " + BagelGame.MAX_GUESSES + " guesses.  ");
        System.out.println("My number was " + game.getSecretAsString());
      }
      
      stillPlaying = getReplayResponse();
    }
    
    // Print goodbye message
    if (gamesWon > 0) {
      System.out.println("\nA " + gamesWon + " point Bagels buff!!");
    }
    System.out.println("Hope you had fun.  Bye.\n");
  }
  
  private static String getValidGuess(BagelGame game) {
    // Keep asking for a guess until valid
    Scanner scan = new Scanner(System.in);
    boolean valid = false;
    String guess = "";
    String error;
    
    while (!valid) {
      System.out.print("Guess # " + game.getGuessNum() + "     ? ");
      guess = scan.nextLine().trim();
      error = game.validateGuess(guess);
      if (error == "") {
        valid = true;
      } else {
        System.out.println(error);
      }
    }
    return guess;
  }
  
  private static boolean getReplayResponse() {
    // keep asking for response until valid
    Scanner scan = new Scanner(System.in);
    // Keep looping until a non-zero-length string is entered
    while (true) {
      System.out.print("Play again (Yes or No)? ");
      String response = scan.nextLine().trim();
      if (response.length() > 0) {
        return response.toUpperCase().charAt(0) == 'Y';
      }
    }
  }
  
}



