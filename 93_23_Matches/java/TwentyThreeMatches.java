import java.util.Scanner;
import java.lang.Math;

/**
 * Game of 23 Matches
 * <p>
 * Based on the BASIC game of 23 Matches here
 * https://github.com/coding-horror/basic-computer-games/blob/main/93%2023%20Matches/23matches.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 * 
 * Converted from BASIC to Java by Darren Cardenas.
 */
 
public class TwentyThreeMatches {  

  private static final int MATCH_COUNT_START = 23;

  private static final int HEADS = 1;
  
  private final Scanner scan;  // For user input
  
  public TwentyThreeMatches() {
    
    scan = new Scanner(System.in);
    
  }  // End of constructor TwentyThreeMatches 

  public void play() {
    
    showIntro();
    startGame();
    
  }  // End of method play  
    
  private static void showIntro() {
    
    System.out.println(" ".repeat(30) + "23 MATCHES");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");

    System.out.println(" THIS IS A GAME CALLED '23 MATCHES'.");
    System.out.println("");
    System.out.println("WHEN IT IS YOUR TURN, YOU MAY TAKE ONE, TWO, OR THREE");
    System.out.println("MATCHES. THE OBJECT OF THE GAME IS NOT TO HAVE TO TAKE");
    System.out.println("THE LAST MATCH.");
    System.out.println("");
    System.out.println("LET'S FLIP A COIN TO SEE WHO GOES FIRST.");
    System.out.println("IF IT COMES UP HEADS, I WILL WIN THE TOSS.");   
    System.out.println("");
    
  }  // End of method showIntro        
  
  private void startGame() {

    int coinSide = (int) (3 * Math.random());
    int cpuRemoves = 0;   
    int matchesLeft = MATCH_COUNT_START;    
    int playerRemoves = 0;
       
    if (coinSide == HEADS) {
      
      System.out.println("HEADS! I WIN! HA! HA!");
      System.out.println("PREPARE TO LOSE, MEATBALL-NOSE!!");
      System.out.println("");
      System.out.println("I TAKE 2 MATCHES");
      
      matchesLeft -= 2;
      
    } else {
      
      System.out.println("TAILS! YOU GO FIRST. ");
      System.out.println("");
      
    }

    // Begin outer while loop    
    while (true) {
      
      if (coinSide == HEADS) {
        
        System.out.println("THE NUMBER OF MATCHES IS NOW " + matchesLeft);
        System.out.println("");   
        System.out.println("YOUR TURN -- YOU MAY TAKE 1, 2 OR 3 MATCHES.");
        
      } 
      
      coinSide = HEADS;

      System.out.print("HOW MANY DO YOU WISH TO REMOVE? ");
      
      // Begin match removal while loop
      while (true) {       
        
        playerRemoves = scan.nextInt();
      
        // Handle invalid entries
        if ((playerRemoves > 3) || (playerRemoves <= 0)) {
          
          System.out.println("VERY FUNNY! DUMMY!");
          System.out.println("DO YOU WANT TO PLAY OR GOOF AROUND?");
          System.out.print("NOW, HOW MANY MATCHES DO YOU WANT? ");
          continue;
          
        }
        
        break;
        
      }  // End match removal while loop
        
      matchesLeft -= playerRemoves;
      
      System.out.println("THERE ARE NOW " + matchesLeft + " MATCHES REMAINING.");
      
      // Win condition
      if (matchesLeft <= 1) {
        
        // Win condition
        System.out.println("YOU WON, FLOPPY EARS !");
        System.out.println("THINK YOU'RE PRETTY SMART !");
        System.out.println("LETS PLAY AGAIN AND I'LL BLOW YOUR SHOES OFF !!");
        System.out.println("");    
        return;
        
      } else if ((matchesLeft >= 2) && (matchesLeft <= 4)) {
        
        cpuRemoves = matchesLeft - 1;          
        
      } else {
        
        cpuRemoves = 4 - playerRemoves;          
        
      }
      
      System.out.println("MY TURN ! I REMOVE " + cpuRemoves + " MATCHES");
      
      matchesLeft -= cpuRemoves;

      // Lose condition        
      if (matchesLeft <= 1) {

        System.out.println("");
        System.out.println("YOU POOR BOOB! YOU TOOK THE LAST MATCH! I GOTCHA!!");
        System.out.println("HA ! HA ! I BEAT YOU !!!");
        System.out.println("");
        System.out.println("GOOD BYE LOSER!");
        return;          
        
      }

    }  // End outer while loop      
    
  }  // End of method startGame  

  public static void main(String[] args) {
    
    TwentyThreeMatches game = new TwentyThreeMatches();
    game.play();
    
  }  // End of method main
    
}  // End of class TwentyThreeMatches
