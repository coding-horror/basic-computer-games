import java.lang.Math;
import java.util.Scanner;

/**
 * Game of Tower
 * <p>
 * Based on the BASIC game of Tower here
 * https://github.com/coding-horror/basic-computer-games/blob/main/90%20Tower/tower.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 * 
 * Converted from BASIC to Java by Darren Cardenas.
 */
 
public class Tower {
  
  private final static int MAX_DISK_SIZE = 15;
  
  private final static int MAX_NUM_COLUMNS = 3;
  
  private final static int MAX_NUM_MOVES = 128;
  
  private final static int MAX_NUM_ROWS = 7;    
  
  private final Scanner scan;  // For user input
  
  // Represent all possible disk positions
  private int[][] positions;
  
  private enum Step {
    INITIALIZE, SELECT_TOTAL_DISKS, SELECT_DISK_MOVE, SELECT_NEEDLE, CHECK_SOLUTION
  }   
  
  
  public Tower() {
    
    scan = new Scanner(System.in);    
    
    // Row 0 and column 0 are not used
    positions = new int[MAX_NUM_ROWS + 1][MAX_NUM_COLUMNS + 1]; 
    
  }  // End of constructor Tower 
  
  
  public class Position {
    
    public int row;
    public int column;
    
    public Position(int row, int column) {
      this.row = row;
      this.column = column;
      
    }  // End of constructor Position
    
  }  // End of inner class Position
  
  
  public void play() {

    showIntro();
    startGame();

  }  // End of method play  
  
  
  private void showIntro() {    
    
    System.out.println(" ".repeat(32) + "TOWERS");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");
    
  }  // End of method showIntro      


  private void startGame() {        

    boolean diskMoved = false;
    
    int column = 0;
    int disk = 0;    
    int needle = 0;        
    int numDisks = 0;
    int numErrors = 0;
    int numMoves = 0;
    int row = 0;       
    
    Step nextStep = Step.INITIALIZE;    
    
    String userResponse = "";
    
    Position diskPosition = new Position(0, 0);
        
    // Begin outer while loop       
    while (true) {
     
      switch (nextStep) {
        
        
        case INITIALIZE:
        
          // Initialize error count
          numErrors = 0;
          
          // Initialize positions
          for (row = 1; row <= MAX_NUM_ROWS; row++) {
            for (column = 1; column <= MAX_NUM_COLUMNS; column++) {
              positions[row][column] = 0;
            }
          }

          // Display description
          System.out.println("");
          System.out.println("TOWERS OF HANOI PUZZLE.\n");
          System.out.println("YOU MUST TRANSFER THE DISKS FROM THE LEFT TO THE RIGHT");
          System.out.println("TOWER, ONE AT A TIME, NEVER PUTTING A LARGER DISK ON A");
          System.out.println("SMALLER DISK.\n");
          
          nextStep = Step.SELECT_TOTAL_DISKS;
          break;
          
          
        case SELECT_TOTAL_DISKS:  

          while (numErrors <= 2) {
            
            // Get user input
            System.out.print("HOW MANY DISKS DO YOU WANT TO MOVE (" + MAX_NUM_ROWS + " IS MAX)? ");
            numDisks = scan.nextInt();
            System.out.println("");
            
            numMoves = 0;
          
            // Ensure the number of disks is valid
            if ((numDisks < 1) || (numDisks > MAX_NUM_ROWS)) {
              
              numErrors++;
              
              // Handle user input errors
              if (numErrors < 3) {
                System.out.println("SORRY, BUT I CAN'T DO THAT JOB FOR YOU.");      
              }                          
              
            }
            else {
              break;  // Leave the while loop
            }
          } 
          
          // Too many user input errors
          if (numErrors > 2) {      
            System.out.println("ALL RIGHT, WISE GUY, IF YOU CAN'T PLAY THE GAME RIGHT, I'LL");
            System.out.println("JUST TAKE MY PUZZLE AND GO HOME.  SO LONG.");
            return;      
          }

          // Display detailed instructions    
          System.out.println("IN THIS PROGRAM, WE SHALL REFER TO DISKS BY NUMERICAL CODE.");
          System.out.println("3 WILL REPRESENT THE SMALLEST DISK, 5 THE NEXT SIZE,");
          System.out.println("7 THE NEXT, AND SO ON, UP TO 15.  IF YOU DO THE PUZZLE WITH");
          System.out.println("2 DISKS, THEIR CODE NAMES WOULD BE 13 AND 15.  WITH 3 DISKS");
          System.out.println("THE CODE NAMES WOULD BE 11, 13 AND 15, ETC.  THE NEEDLES");
          System.out.println("ARE NUMBERED FROM LEFT TO RIGHT, 1 TO 3.  WE WILL");
          System.out.println("START WITH THE DISKS ON NEEDLE 1, AND ATTEMPT TO MOVE THEM");
          System.out.println("TO NEEDLE 3.\n");
          System.out.println("GOOD LUCK!\n");    
        
          disk = MAX_DISK_SIZE;
          
          // Set disk starting positions
          for (row = MAX_NUM_ROWS; row > (MAX_NUM_ROWS - numDisks); row--) {       
            positions[row][1] = disk;            
            disk = disk - 2;            
          }            
          
          printPositions();
          
          nextStep = Step.SELECT_DISK_MOVE;
          break;
          

        case SELECT_DISK_MOVE:
       
          System.out.print("WHICH DISK WOULD YOU LIKE TO MOVE? ");
          
          numErrors = 0;
          
          while (numErrors < 2) {
            disk = scan.nextInt();

            // Validate disk numbers
            if ((disk - 3) * (disk - 5) * (disk - 7) * (disk - 9) * (disk - 11) * (disk - 13) * (disk - 15) == 0) {

              // Check if disk exists
              diskPosition = getDiskPosition(disk);
              
              // Disk found
              if ((diskPosition.row > 0) && (diskPosition.column > 0))
              {
                // Disk can be moved
                if (isDiskMovable(disk, diskPosition.row, diskPosition.column) == true) {
                  
                  break;
                  
                }
                // Disk cannot be moved
                else {
                  
                  System.out.println("THAT DISK IS BELOW ANOTHER ONE.  MAKE ANOTHER CHOICE.");                  
                  System.out.print("WHICH DISK WOULD YOU LIKE TO MOVE? ");               
                  
                }                
              }
              // Mimic legacy handling of valid disk number but disk not found
              else {
                
                System.out.println("THAT DISK IS BELOW ANOTHER ONE.  MAKE ANOTHER CHOICE.");
                System.out.print("WHICH DISK WOULD YOU LIKE TO MOVE? ");
                numErrors = 0;
                continue;
                
              }                                       
              
            }
            // Invalid disk number
            else {
              
              System.out.println("ILLEGAL ENTRY... YOU MAY ONLY TYPE 3,5,7,9,11,13, OR 15.");                
              numErrors++;
              
              if (numErrors > 1) {                
                break;
              }              
              
              System.out.print("? ");
          
            }
          }

          if (numErrors > 1) {      
        
            System.out.println("STOP WASTING MY TIME.  GO BOTHER SOMEONE ELSE.");
            return;
          }
    
          nextStep = Step.SELECT_NEEDLE;        
          break;
          

        case SELECT_NEEDLE:
        
          numErrors = 0;

          while (true) {
            
            System.out.print("PLACE DISK ON WHICH NEEDLE? ");
            needle = scan.nextInt();
            
            // Handle valid needle numbers
            if ((needle - 1) * (needle - 2) * (needle - 3) == 0) {                           
              
              // Ensure needle is safe for disk move        
              if (isNeedleSafe(needle, disk, row) == false) {
                
                System.out.println("YOU CAN'T PLACE A LARGER DISK ON TOP OF A SMALLER ONE,");
                System.out.println("IT MIGHT CRUSH IT!");
                System.out.print("NOW THEN, ");     

                nextStep = Step.SELECT_DISK_MOVE;
                break;                
              }
              
              diskPosition = getDiskPosition(disk);
              
              // Attempt to move the disk on a non-empty needle
              diskMoved = false;          
              for (row = 1; row <= MAX_NUM_ROWS; row++) {
                if (positions[row][needle] != 0) {
                  row--;
                  
                  positions[row][needle] = positions[diskPosition.row][diskPosition.column];
                  positions[diskPosition.row][diskPosition.column] = 0;
                  
                  diskMoved = true;
                  break;            
                }         
              }
              
              // Needle was empty, so move disk to the bottom
              if (diskMoved == false) {
                positions[MAX_NUM_ROWS][needle] = positions[diskPosition.row][diskPosition.column];
                positions[diskPosition.row][diskPosition.column] = 0;                        
              }                                       
              
              nextStep = Step.CHECK_SOLUTION;
              break;
              
            }
            // Handle invalid needle numbers
            else {
              
              numErrors++;
              
              if (numErrors > 1) {         
                System.out.println("I TRIED TO WARN YOU, BUT YOU WOULDN'T LISTEN.");
                System.out.println("BYE BYE, BIG SHOT.");
                return;
              }
              else {          
                System.out.println("I'LL ASSUME YOU HIT THE WRONG KEY THIS TIME.  BUT WATCH IT,");
                System.out.println("I ONLY ALLOW ONE MISTAKE.");         
              }                        
            }
            
          } 

          break;

          
        case CHECK_SOLUTION:
        
          printPositions();         
        
          numMoves++;
          
          // Puzzle is solved
          if (isPuzzleSolved() == true) {
            
            // Check for optimal solution
            if (numMoves == (Math.pow(2, numDisks) - 1)) {
              System.out.println("");
              System.out.println("CONGRATULATIONS!!\n");                
            }
            
            System.out.println("YOU HAVE PERFORMED THE TASK IN " + numMoves + " MOVES.\n");
            System.out.print("TRY AGAIN (YES OR NO)? ");  

            // Prompt for retries
            while (true) {
              userResponse = scan.next();
              
              if (userResponse.toUpperCase().equals("YES")) {                
                nextStep = Step.INITIALIZE;                
                break;
              }
              else if (userResponse.toUpperCase().equals("NO")) {
                System.out.println("");
                System.out.println("THANKS FOR THE GAME!\n");
                return;
              }
              else {
                System.out.print("'YES' OR 'NO' PLEASE? ");
              }
            }            
          }
          // Puzzle is not solved
          else {
            
            // Exceeded maximum number of moves
            if (numMoves > MAX_NUM_MOVES) {
              System.out.println("SORRY, BUT I HAVE ORDERS TO STOP IF YOU MAKE MORE THAN");
              System.out.println("128 MOVES."); 
              return;              
            }         

            nextStep = Step.SELECT_DISK_MOVE;
            break;            
          }      
          
          break;
          
        default:
          System.out.println("INVALID STEP");
          break;       

      }      
      
    }  // End outer while loop 
    
  }  // End of method startGame
  

  private boolean isPuzzleSolved() {
    
    int column = 0;
    int row = 0;

    // Puzzle is solved if first 2 needles are empty    
    for (row = 1; row <= MAX_NUM_ROWS; row++) {      
      for (column = 1; column <= 2; column++) {        
        if (positions[row][column] != 0) {
          return false;        
        }          
      }        
    }    

    return true;
    
  }  // End of method isPuzzleSolved

  
  private Position getDiskPosition(int disk) {
        
    int column = 0;
    int row = 0;
    
    Position pos = new Position(0, 0);

    // Begin loop through all rows  
    for (row = 1; row <= MAX_NUM_ROWS; row++) {
      
      // Begin loop through all columns
      for (column = 1; column <= MAX_NUM_COLUMNS; column++) {
        
        // Found the disk
        if (positions[row][column] == disk) {
          
          pos.row = row;
          pos.column = column;
          return pos;

        }
        
      }  // End loop through all columns

    }  // End loop through all rows  
    
    return pos;
    
  }  // End of method getDiskPosition    

  
  private boolean isDiskMovable(int disk, int row, int column) {      

    int ii = 0;  // Loop iterator

    // Begin loop through all rows above disk
    for (ii = row; ii >= 1; ii--) {

      // Disk can be moved
      if (positions[ii][column] == 0) {
        continue;                 
      }
      
      // Disk cannot be moved
      if (positions[ii][column] < disk) {
        return false;      
      }

    }  // End loop through all rows above disk
    
    return true;

  }  // End of method isDiskMovable    


  private boolean isNeedleSafe(int needle, int disk, int row) {
  
    for (row = 1; row <= MAX_NUM_ROWS; row++) {

      // Needle is not empty
      if (positions[row][needle] != 0) {
        
        // Disk crush condition
        if (disk >= positions[row][needle]) {     
          return false;
        }
      }
    }
    
    return true;
  
  }  // End of method isNeedleSafe
  
  
  private void printPositions() {
        
    int column = 1;
    int ii = 0;  // Loop iterator
    int numSpaces = 0;    
    int row = 1;  
    
    // Begin loop through all rows
    for (row = 1; row <= MAX_NUM_ROWS; row++) {
      
      numSpaces = 9;        
      
      // Begin loop through all columns
      for (column = 1; column <= MAX_NUM_COLUMNS; column++) {

        // No disk at the current position
        if (positions[row][column] == 0) {
          
          System.out.print(" ".repeat(numSpaces) + "*");
          numSpaces = 20;                   
        } 
        
        // Draw a disk at the current position
        else {
          
          System.out.print(" ".repeat(numSpaces - ((int) (positions[row][column] / 2))));

          for (ii = 1; ii <= positions[row][column]; ii++) {
            System.out.print("*");                      
          }     
          
          numSpaces = 20 - ((int) (positions[row][column] / 2));
        }                
        
      }  // End loop through all columns
      
      System.out.println("");        
      
    }  // End loop through all rows    
        
  }  // End of method printPositions      
  

  public static void main(String[] args) {
    
    Tower tower = new Tower();
    tower.play();
    
  }  // End of method main

}  // End of class Tower