import java.util.Scanner;
import java.lang.Math;

/**
 * Game of Depth Charge
 * <p>
 * Based on the BASIC game of Depth Charge here
 * https://github.com/coding-horror/basic-computer-games/blob/main/31%20Depth%20Charge/depthcharge.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 * 
 * Converted from BASIC to Java by Darren Cardenas.
 */
 
public class DepthCharge {  
  
  private final Scanner scan;  // For user input
  
  public DepthCharge() {
    
    scan = new Scanner(System.in);
    
  }  // End of constructor DepthCharge 

  public void play() {
    
    showIntro();
    startGame();
    
  }  // End of method play  
    
  private static void showIntro() {
    
    System.out.println(" ".repeat(29) + "DEPTH CHARGE");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");

  }  // End of method showIntro        
  
  private void startGame() {

    int searchArea = 0;
    int shotNum = 0;
    int shotTotal = 0;    
    int shotX = 0;
    int shotY = 0;
    int shotZ = 0;
    int targetX = 0;
    int targetY = 0;
    int targetZ = 0;
    int tries = 0;
    String[] userCoordinates;
    String userResponse = "";
    
    System.out.print("DIMENSION OF SEARCH AREA? ");
    searchArea = Integer.parseInt(scan.nextLine());
    System.out.println("");
    
    shotTotal = (int) (Math.log10(searchArea) / Math.log10(2)) + 1;      
    
    System.out.println("YOU ARE THE CAPTAIN OF THE DESTROYER USS COMPUTER");
    System.out.println("AN ENEMY SUB HAS BEEN CAUSING YOU TROUBLE.  YOUR");
    System.out.println("MISSION IS TO DESTROY IT.  YOU HAVE " + shotTotal + " SHOTS.");
    System.out.println("SPECIFY DEPTH CHARGE EXPLOSION POINT WITH A");
    System.out.println("TRIO OF NUMBERS -- THE FIRST TWO ARE THE");
    System.out.println("SURFACE COORDINATES; THE THIRD IS THE DEPTH.");
    
    // Begin outer while loop    
    while (true) {         
    
      System.out.println("");
      System.out.println("GOOD LUCK !");
      System.out.println("");
      
      targetX = (int) ((searchArea + 1) * Math.random());
      targetY = (int) ((searchArea + 1) * Math.random());
      targetZ = (int) ((searchArea + 1) * Math.random());
      
      // Begin loop through all shots
      for (shotNum = 1; shotNum <= shotTotal; shotNum++) {
      
        // Get user input
        System.out.println("");
        System.out.print("TRIAL # " + shotNum + "? ");        
        userResponse = scan.nextLine();
        
        // Split on commas
        userCoordinates = userResponse.split(",");
        
        // Assign to integer variables
        shotX = Integer.parseInt(userCoordinates[0].trim());
        shotY = Integer.parseInt(userCoordinates[1].trim());
        shotZ = Integer.parseInt(userCoordinates[2].trim());       
      
        // Win condition
        if (Math.abs(shotX - targetX) + Math.abs(shotY - targetY) 
            + Math.abs(shotZ - targetZ) == 0) {

          System.out.println("B O O M ! ! YOU FOUND IT IN" + shotNum + " TRIES!");
          break;
          
        }
        
        this.getReport(targetX, targetY, targetZ, shotX, shotY, shotZ);
        
        System.out.println("");                
      
      }  // End loop through all shots
      
      if (shotNum > shotTotal) {
        
        System.out.println("");
        System.out.println("YOU HAVE BEEN TORPEDOED!  ABANDON SHIP!");
        System.out.println("THE SUBMARINE WAS AT " + targetX + "," + targetY + "," + targetZ);
      }
      
      System.out.println("");
      System.out.println("");
      System.out.print("ANOTHER GAME (Y OR N)? ");               
      userResponse = scan.nextLine();      
      
      if (!userResponse.toUpperCase().equals("Y")) {
        System.out.print("OK.  HOPE YOU ENJOYED YOURSELF."); 
        return;
      }
    
    }  // End outer while loop
    
  }  // End of method startGame  
    
  public void getReport(int a, int b, int c, int x, int y, int z) {
    
    System.out.print("SONAR REPORTS SHOT WAS ");
    
    // Handle y coordinate
    if (y > b) {
      
      System.out.print("NORTH");      
      
    } else if (y < b) {
      
      System.out.print("SOUTH");
    }
    
    // Handle x coordinate
    if (x > a) {
      
      System.out.print("EAST");
      
    } else if (x < a) {
      
      System.out.print("WEST");      
    }
    
    if ((y != b) || (x != a)) {
      
      System.out.print(" AND");
    }
    
    // Handle depth
    if (z > c) {
      
      System.out.println(" TOO LOW.");
      
    } else  if (z < c) {
      
      System.out.println(" TOO HIGH.");
      
    } else {
      
      System.out.println(" DEPTH OK.");      
    }
    
    return;
    
  }  // End of method getReport
  
  public static void main(String[] args) {
    
    DepthCharge game = new DepthCharge();
    game.play();
    
  }  // End of method main
    
}  // End of class DepthCharge
