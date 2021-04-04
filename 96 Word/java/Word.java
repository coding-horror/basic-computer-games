import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Word
 * <p>
 * Based on the BASIC game of Word here
 * https://github.com/coding-horror/basic-computer-games/blob/main/96%20Word/word.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 * 
 * Converted from BASIC to Java by Darren Cardenas.
 */
 
public class Word {
  
  private final static String[] WORDS = { 

  "DINKY", "SMOKE", "WATER", "GRASS", "TRAIN", "MIGHT",
  "FIRST", "CANDY", "CHAMP", "WOULD", "CLUMP", "DOPEY"

  };
   
  private final Scanner scan;  // For user input
  
  private enum Step {
    INITIALIZE, MAKE_GUESS, USER_WINS
  }   
    
  public Word() {
    
    scan = new Scanner(System.in);    

  }  // End of constructor Word 
  
  public void play() {

    showIntro();
    startGame();

  }  // End of method play  
    
  private void showIntro() {    
    
    System.out.println(" ".repeat(32) + "WORD");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");
    
    System.out.println("I AM THINKING OF A WORD -- YOU GUESS IT.  I WILL GIVE YOU");
    System.out.println("CLUES TO HELP YOU GET IT.  GOOD LUCK!!");
    System.out.println("\n");
    
  }  // End of method showIntro      

  private void startGame() {        
  
    char[] commonLetters = new char[8];
    char[] exactLetters = new char[8];
      
    int commonIndex = 0;
    int ii = 0;  // Loop iterator
    int jj = 0;  // Loop iterator       
    int numGuesses = 0;
    int numMatches = 0;
    int wordIndex = 0;
  
    Step nextStep = Step.INITIALIZE;    

    String commonString = "";    
    String exactString = "";
    String guessWord = "";
    String secretWord = "";    
    String userResponse = "";

    // Begin outer while loop       
    while (true) {
     
      switch (nextStep) {

        case INITIALIZE:
        
          System.out.println("\n");
          System.out.println("YOU ARE STARTING A NEW GAME...");

          // Select a secret word from the list
          wordIndex = (int) (Math.random() * WORDS.length);
          secretWord = WORDS[wordIndex];          
          
          numGuesses = 0;
          
          Arrays.fill(exactLetters, 1, 6, '-');
          Arrays.fill(commonLetters, 1, 6, '\0');

          nextStep = Step.MAKE_GUESS;
          break;                    
          
        case MAKE_GUESS:
        
          System.out.print("GUESS A FIVE LETTER WORD? ");
          guessWord = scan.nextLine().toUpperCase();

          numGuesses++;
          
          // Win condition
          if (guessWord.equals(secretWord)) {            
            nextStep = Step.USER_WINS;
            continue;
          }
          
          Arrays.fill(commonLetters, 1, 8, '\0');          

          // Surrender condition
          if (guessWord.equals("?")) {            
            System.out.println("THE SECRET WORD IS " + secretWord);
            System.out.println("");
            nextStep = Step.INITIALIZE;  // Play again
            continue;        
          }
          
          // Check for valid input
          if (guessWord.length() != 5) {            
            System.out.println("YOU MUST GUESS A 5 LETTER WORD.  START AGAIN.");            
            numGuesses--;
            nextStep = Step.MAKE_GUESS;  // Guess again
            continue;            
          }
          
          numMatches = 0;
          commonIndex = 1;          
          
          for (ii = 1; ii <= 5; ii++) {  
          
            for (jj = 1; jj <= 5; jj++) {                
            
              if (secretWord.charAt(ii - 1) != guessWord.charAt(jj - 1)) {                
                continue;                
              } 

              // Avoid out of bounds errors
              if (commonIndex <= 5) {
                commonLetters[commonIndex] = guessWord.charAt(jj - 1);
                commonIndex++;
              }              

              if (ii == jj) {                
                exactLetters[jj] = guessWord.charAt(jj - 1);                
              }
              
              // Avoid out of bounds errors
              if (numMatches < 5) {
                numMatches++;
              }
            }            
          }
          
          exactString = "";
          commonString = "";
          
          // Build the exact letters string
          for (ii = 1; ii <= 5; ii++) {          
            exactString += exactLetters[ii];            
          }
          
          // Build the common letters string
          for (ii = 1; ii <= numMatches; ii++) {            
            commonString += commonLetters[ii];            
          }
          
          System.out.println("THERE WERE " + numMatches + " MATCHES AND THE COMMON LETTERS WERE..." 
                             + commonString);
          
          System.out.println("FROM THE EXACT LETTER MATCHES, YOU KNOW................" + exactString);
          
          // Win condition
          if (exactString.equals(secretWord)) {            
            nextStep = Step.USER_WINS;
            continue;            
          }
          
          // No matches
          if (numMatches <= 1) {
            System.out.println("");
            System.out.println("IF YOU GIVE UP, TYPE '?' FOR YOUR NEXT GUESS.");
          }
          
          System.out.println("");
          nextStep = Step.MAKE_GUESS;
          break;        
        
        case USER_WINS:
        
          System.out.println("YOU HAVE GUESSED THE WORD.  IT TOOK " + numGuesses + " GUESSES!");
          System.out.println("");
          
          System.out.print("WANT TO PLAY AGAIN? ");
          userResponse = scan.nextLine();

          if (userResponse.toUpperCase().equals("YES")) {                    
            nextStep = Step.INITIALIZE;  // Play again                                     
          } else {                      
            return;  // Quit game                        
          }                                    
          break;
       
        default:
          System.out.println("INVALID STEP");
          break;       

      }   
    
    }  // End outer while loop 
    
  }  // End of method startGame
  
  public static void main(String[] args) {
    
    Word word = new Word();
    word.play();
    
  }  // End of method main

}  // End of class Word
