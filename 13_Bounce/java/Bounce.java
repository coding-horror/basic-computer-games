import java.util.Scanner;
import java.lang.Math;

/**
 * Game of Bounce
 * <p>
 * Based on the BASIC game of Bounce here
 * https://github.com/coding-horror/basic-computer-games/blob/main/13%20Bounce/bounce.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 * 
 * Converted from BASIC to Java by Darren Cardenas.
 */
 
public class Bounce {  

  private final Scanner scan;  // For user input
  
  public Bounce() {
    
    scan = new Scanner(System.in);
    
  }  // End of constructor Bounce   

  public void play() {   

    showIntro();
    startGame();
    
  }  // End of method play  

  private void showIntro() {    
    
    System.out.println(" ".repeat(32) + "BOUNCE");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");
    
  }  // End of method showIntro

  private void startGame() { 

    double coefficient = 0;
    double height = 0;          
    double timeIncrement = 0;
    double timeIndex = 0;
    double timeTotal = 0;
    double velocity = 0;    
 
    double[] timeData = new double[21];
    
    int heightInt = 0;
    int index = 0;
    int maxData = 0;
    
    String lineContent = "";
    
    System.out.println("THIS SIMULATION LETS YOU SPECIFY THE INITIAL VELOCITY");
    System.out.println("OF A BALL THROWN STRAIGHT UP, AND THE COEFFICIENT OF");
    System.out.println("ELASTICITY OF THE BALL.  PLEASE USE A DECIMAL FRACTION");
    System.out.println("COEFFICIENCY (LESS THAN 1).");
    System.out.println("");
    System.out.println("YOU ALSO SPECIFY THE TIME INCREMENT TO BE USED IN");
    System.out.println("'STROBING' THE BALL'S FLIGHT (TRY .1 INITIALLY).");
    System.out.println("");
    
    // Begin outer while loop
    while (true) {
      
      System.out.print("TIME INCREMENT (SEC)? ");
      timeIncrement = Double.parseDouble(scan.nextLine());
      System.out.println("");
      
      System.out.print("VELOCITY (FPS)? ");
      velocity = Double.parseDouble(scan.nextLine());    
      System.out.println("");
      
      System.out.print("COEFFICIENT? ");
      coefficient = Double.parseDouble(scan.nextLine());    
      System.out.println("");

      System.out.println("FEET");
      System.out.println("");
      
      maxData = (int)(70 / (velocity / (16 * timeIncrement)));
      
      for (index = 1; index <= maxData; index++) {                
        timeData[index] = velocity * Math.pow(coefficient, index - 1) / 16;
      }
   
      // Begin loop through all rows of y-axis data
      for (heightInt = (int)(-16 * Math.pow(velocity / 32, 2) + Math.pow(velocity, 2) / 32 + 0.5) * 10; 
           heightInt >= 0; heightInt -= 5) {

        height = heightInt / 10.0;

        lineContent = "";
        
        if ((int)(Math.floor(height)) == height) {

          lineContent += " " + (int)(height) + " ";
        } 
        
        timeTotal = 0;  
        
        for (index = 1; index <= maxData; index++) {                  
          
          for (timeIndex = 0; timeIndex <= timeData[index]; timeIndex += timeIncrement) {
            
            timeTotal += timeIncrement;

            if (Math.abs(height - (0.5 * (-32) * Math.pow(timeIndex, 2) + velocity 
                * Math.pow(coefficient, index - 1) * timeIndex)) <= 0.25) {
             
              while (lineContent.length() < (timeTotal / timeIncrement) - 1) {            
                lineContent += " ";            
              } 
              lineContent += "0";                          
            }                       
          }
          
          timeIndex = timeData[index + 1] / 2;
          
          if (-16 * Math.pow(timeIndex, 2) + velocity * Math.pow(coefficient, index - 1) * timeIndex < height) {
            
            break;                        
          }                                   
        } 
        
        System.out.println(lineContent);
        
      }  // End loop through all rows of y-axis data

      lineContent = "";
      
      // Show the x-axis
      for (index = 1; index <= (int)(timeTotal + 1) / timeIncrement + 1; index++) {
        
        lineContent += ".";        
      }
      
      System.out.println(lineContent);
      
      lineContent = " 0";
      
      for (index = 1; index <= (int)(timeTotal + 0.9995); index++) {
        
        while (lineContent.length() < (int)(index / timeIncrement)) {            
          lineContent += " ";            
        }     
        lineContent += index;        
      }
      
      System.out.println(lineContent);

      System.out.println(" ".repeat((int)((timeTotal + 1) / (2 * timeIncrement) - 3)) + "SECONDS");      
      
    }  // End outer while loop 

  }  // End of method startGame     
  
  public static void main(String[] args) {
    
    Bounce game = new Bounce();
    game.play();
    
  }  // End of method main
    
}  // End of class Bounce
