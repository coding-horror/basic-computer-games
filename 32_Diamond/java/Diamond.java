import java.util.Scanner;

/**
 * Game of Diamond
 * <p>
 * Based on the BASIC game of Diamond here
 * https://github.com/coding-horror/basic-computer-games/blob/main/32%20Diamond/diamond.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 * 
 * Converted from BASIC to Java by Darren Cardenas.
 */
 
public class Diamond {    
  
  private static final int LINE_WIDTH = 60;
  
  private static final String PREFIX = "CC";
  
  private static final char SYMBOL = '!';
  
  private final Scanner scan;  // For user input  
  
  
  public Diamond() {
    
    scan = new Scanner(System.in);    
    
  }  // End of constructor Diamond 
  
  
  public void play() {

    showIntro();
    startGame();

  }  // End of method play  
  
  
  private void showIntro() {    
    
    System.out.println(" ".repeat(32) + "DIAMOND");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");
    
  }  // End of method showIntro      


  private void startGame() {        
      
    int body = 0;
    int column = 0;
    int end = 0;
    int fill = 0;
    int increment = 2;    
    int numPerSide = 0;    
    int prefixIndex = 0;
    int row = 0;       
    int start = 1;    
    int userNum = 0;        
    
    String lineContent = "";
    
    // Get user input
    System.out.println("FOR A PRETTY DIAMOND PATTERN,");
    System.out.print("TYPE IN AN ODD NUMBER BETWEEN 5 AND 21? ");
    userNum = scan.nextInt();
    System.out.println("");
    
    // Calcuate number of diamonds to be drawn on each side of screen
    numPerSide = (int) (LINE_WIDTH / userNum);    

    end = userNum;    
    
    // Begin loop through each row of diamonds
    for (row = 1; row <= numPerSide; row++) {
     
      // Begin loop through top and bottom halves of each diamond
      for (body = start; increment < 0 ? body >= end : body <= end; body += increment) {

        lineContent = "";
        
        // Add whitespace
        while (lineContent.length() < ((userNum - body) / 2)) {
          lineContent += " ";
        }
        
        // Begin loop through each column of diamonds
        for (column = 1; column <= numPerSide; column++) {
          
          prefixIndex = 1;
          
          // Begin loop that fills each diamond with characters
          for (fill = 1; fill <= body; fill++) {
            
            // Right side of diamond
            if (prefixIndex > PREFIX.length()) {
              
              lineContent += SYMBOL; 
              
            }
            // Left side of diamond
            else {
              
              lineContent += PREFIX.charAt(prefixIndex - 1);
              prefixIndex++;
              
            }           
            
          }  // End loop that fills each diamond with characters
          
          // Column finished
          if (column == numPerSide) {
            
            break;
            
          }
          // Column not finishd
          else {
            
            // Add whitespace
            while (lineContent.length() < (userNum * column + (userNum - body) / 2)) {
              lineContent += " ";
            }
            
          }          
          
        }  // End loop through each column of diamonds
        
        System.out.println(lineContent);

      }  // End loop through top and bottom half of each diamond

      if (start != 1) {

        start = 1;
        end = userNum;
        increment = 2;        
      
      }
      else {
          
        start = userNum - 2;
        end = 1;
        increment = -2;        
        row--;

      }        

    }  // End loop through each row of diamonds
    
  }  // End of method startGame
  
  
  public static void main(String[] args) {
    
    Diamond diamond = new Diamond();
    diamond.play();
    
  }  // End of method main

}  // End of class Diamond
