/******************************************************************************
*
* Encapsulates all the state and game logic for one single game of Bagels
*
* Used by Bagels.java
*
* Jeff Jetton, 2020
*
******************************************************************************/

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashSet;
import java.util.List;
import java.util.Random;
import java.util.Set;

public class BagelGame {
  
  public static final String CORRECT = "FERMI FERMI FERMI";
  public static final int MAX_GUESSES = 20;
  
  enum GameState {
      RUNNING,
      WON,
      LOST
    }
    
  private GameState      state = GameState.RUNNING;
  private List<Integer>  secretNum;
  private int            guessNum = 1;
  
  public BagelGame() {
    // No-arg constructor for when you don't need to set the seed
    this(new Random());
  }
  
  public BagelGame(long seed) {
    // Setting the seed as a long value
    this(new Random(seed));
  }
  
  public BagelGame(Random rand) {
    // This is the "real" constructor, which expects an instance of
    // Random to use for shuffling the digits of the secret number.
    
    // Since the digits cannot repeat in our "number", we can't just
    // pick three random 0-9 integers. Instead, we'll treat it like
    // a deck of ten cards, numbered 0-9.
    List<Integer> digits = new ArrayList<Integer>(10);
    // The 10 specified initial allocation, not actual size,
    // which is why we add rather than set each element...
    for (int i = 0; i < 10; i++) {
      digits.add(i);
    }
    // Collections offers a handy-dandy shuffle method. Normally it
    // uses a fresh Random class PRNG, but we're supplying our own
    // to give us controll over whether or not we set the seed
    Collections.shuffle(digits, rand);
    
    // Just take the first three digits
    secretNum = digits.subList(0, 3);
  }
  
  public boolean isOver() {
    return state != GameState.RUNNING;
  }
  
  public boolean isWon() {
    return state == GameState.WON;
  }
    
  public int getGuessNum() {
    return guessNum;
  }
  
  public String getSecretAsString() {
    // Convert the secret number to a three-character string
    String secretString = "";
    for (int n : secretNum) {
      secretString += n;
    }
    return secretString;
  }
  
  @Override
  public String toString() {
    // Quick report of game state for debugging purposes
    String s = "Game is " + state + "\n";
    s += "Current Guess Number: " + guessNum + "\n";
    s += "Secret Number: " + secretNum;
    return s;
  }
  
  public String validateGuess(String guess) {
    // Checks the passed string and returns null if it's a valid guess
    // (i.e., exactly three numeric characters)
    // If not valid, returns an "error" string to display to user.
    String error = "";
    
    if (guess.length() == 3) {
      // Correct length. Are all the characters numbers?
      try {
        Integer.parseInt(guess);
      } catch (NumberFormatException ex) {
        error = "What?";
      }
      if (error == "") {
        // Check for unique digits by placing each character in a set
        Set<Character> uniqueDigits = new HashSet<Character>();
        for (int i = 0; i < guess.length(); i++){
          uniqueDigits.add(guess.charAt(i));
        }
        if (uniqueDigits.size() != guess.length()) {
          error = "Oh, I forgot to tell you that the number I have in mind\n";
          error += "has no two digits the same.";
        }
      }
    } else {
      error = "Try guessing a three-digit number.";
    }

    return error;
  }
  
  public String makeGuess(String s) throws IllegalArgumentException {
    // Processes the passed guess string (which, ideally, should be
    // validated by previously calling validateGuess)
    // Return a response string (PICO, FERMI, etc.) if valid
    // Also sets game state accordingly (sets win state or increments
    // number of guesses)
    
    // Convert string to integer list, just to keep things civil
    List<Integer> guess = new ArrayList<Integer>(3);
    for (int i = 0; i < 3; i++) {
      guess.add((int)s.charAt(i) - 48);
    }
    
    // Build response string...
    String response = "";
    // Correct digit, but in wrong place?
    for (int i = 0; i < 2; i++) {
      if (secretNum.get(i) == guess.get(i+1)) {
        response += "PICO ";
      }
      if (secretNum.get(i+1) == guess.get(i)) {
        response += "PICO ";
      }
    }
    if (secretNum.get(0) == guess.get(2)) {
      response += "PICO ";
    }
    if (secretNum.get(2) == guess.get(0)) {
      response += "PICO ";
    }
    // Correct digits in right place?
    for (int i = 0; i < 3; i++) {
      if (secretNum.get(i) == guess.get(i)) {
        response += "FERMI ";
      }
    }
    // Nothin' right?
    if (response == "") {
      response = "BAGELS";
    }
    // Get rid of any space that might now be at the end
    response = response.trim();
    // If correct, change state
    if (response.equals(CORRECT)) {
      state = GameState.WON;
    } else {
      // If not, increment guess counter and check for game over
      guessNum++;
      if (guessNum > MAX_GUESSES) {
        state = GameState.LOST;
      }
    }
    return response;
  }
    
}