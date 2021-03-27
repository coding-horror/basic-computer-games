import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of One Check
 * <p>
 * Based on the BASIC game of One Check here
 * https://github.com/coding-horror/basic-computer-games/blob/main/67%20One%20Check/onecheck.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 * 
 * Converted from BASIC to Java by Darren Cardenas.
 */
 
public class OneCheck {  

  private final Scanner scan;  // For user input
    
  private enum Step {
    SHOW_INSTRUCTIONS, SHOW_BOARD, GET_MOVE, GET_SUMMARY, QUERY_RETRY
  }     
  
  public OneCheck() {
    
    scan = new Scanner(System.in);
    
  }  // End of constructor OneCheck 
        
  public void play() {
    
    showIntro();
    startGame();
    
  }  // End of method play  
    
  private static void showIntro() {
    
    System.out.println(" ".repeat(29) + "ONE CHECK");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");
    
  }  // End of method showIntro        
  
  private void startGame() {

    int fromSquare = 0;
    int numJumps = 0;
    int numPieces = 0;
    int square = 0;
    int startPosition = 0;
    int toSquare = 0;
    
    // Move legality test variables
    int fromTest1 = 0;
    int fromTest2 = 0;
    int toTest1 = 0;
    int toTest2 = 0;
    
    int[] positions = new int[65];
    
    Step nextStep = Step.SHOW_INSTRUCTIONS;
    
    String lineContent = "";
    String userResponse = "";       

    // Begin outer while loop
    while (true) {
      
      // Begin switch
      switch (nextStep) {
        
        case SHOW_INSTRUCTIONS:

          System.out.println("SOLITAIRE CHECKER PUZZLE BY DAVID AHL\n");
          System.out.println("48 CHECKERS ARE PLACED ON THE 2 OUTSIDE SPACES OF A");
          System.out.println("STANDARD 64-SQUARE CHECKERBOARD.  THE OBJECT IS TO");
          System.out.println("REMOVE AS MANY CHECKERS AS POSSIBLE BY DIAGONAL JUMPS");
          System.out.println("(AS IN STANDARD CHECKERS).  USE THE NUMBERED BOARD TO");
          System.out.println("INDICATE THE SQUARE YOU WISH TO JUMP FROM AND TO.  ON");
          System.out.println("THE BOARD PRINTED OUT ON EACH TURN '1' INDICATES A");
          System.out.println("CHECKER AND '0' AN EMPTY SQUARE.  WHEN YOU HAVE NO");
          System.out.println("POSSIBLE JUMPS REMAINING, INPUT A '0' IN RESPONSE TO");
          System.out.println("QUESTION 'JUMP FROM ?'\n");
          System.out.println("HERE IS THE NUMERICAL BOARD:\n");   
          
          nextStep = Step.SHOW_BOARD;
          break;
          
        case SHOW_BOARD:
        
          // Begin loop through all squares
          for (square = 1; square <= 57; square += 8) {
            
            lineContent = String.format("% -4d%-4d%-4d%-4d%-4d%-4d%-4d%-4d", square, square + 1, square + 2,
                                        square + 3, square + 4, square + 5, square + 6, square + 7);
            System.out.println(lineContent);
            
          }  // End loop through all squares
          
          System.out.println("");
          System.out.println("AND HERE IS THE OPENING POSITION OF THE CHECKERS.");
          System.out.println("");    
          
          Arrays.fill(positions, 1);
          
          // Begin generating start positions
          for (square = 19; square <= 43; square += 8) {
            
            for (startPosition = square; startPosition <= square + 3; startPosition++) {
              
              positions[startPosition] = 0;
              
            }        
          }  // End generating start positions
          
          numJumps = 0;
          
          printBoard(positions); 
      
          nextStep = Step.GET_MOVE;
          break;
          
        case GET_MOVE:
        
          System.out.print("JUMP FROM? ");
          fromSquare = scan.nextInt();
          scan.nextLine();  // Discard newline
          
          // User requested summary
          if (fromSquare == 0) {            
            nextStep = Step.GET_SUMMARY;
            break;            
          }
          
          System.out.print("TO? ");
          toSquare = scan.nextInt();
          scan.nextLine();  // Discard newline
          System.out.println("");      

          // Check legality of move
          fromTest1 = (int) Math.floor((fromSquare - 1.0) / 8.0);
          fromTest2 = fromSquare - 8 * fromTest1;
          toTest1 = (int) Math.floor((toSquare - 1.0) / 8.0);
          toTest2 = toSquare - 8 * toTest1;
          
          if ((fromTest1 > 7) ||
              (toTest1 > 7) ||
              (fromTest2 > 8) ||
              (toTest2 > 8) ||
              (Math.abs(fromTest1 - toTest1) != 2) ||
              (Math.abs(fromTest2 - toTest2) != 2) ||
              (positions[(toSquare + fromSquare) / 2] == 0) ||
              (positions[fromSquare] == 0) ||
              (positions[toSquare] == 1)) {
          
            System.out.println("ILLEGAL MOVE.  TRY AGAIN...");
            nextStep = Step.GET_MOVE;
            break;            
          }

          positions[toSquare] = 1;
          positions[fromSquare] = 0;
          positions[(toSquare + fromSquare) / 2] = 0;
          numJumps++;
          
          printBoard(positions); 
          
          nextStep = Step.GET_MOVE;
          break;
          
        case GET_SUMMARY:
        
          numPieces = 0;
      
          // Count remaining pieces
          for (square = 1; square <= 64; square++) {           
            numPieces += positions[square];            
          }
          
          System.out.println("");
          System.out.println("YOU MADE " + numJumps + " JUMPS AND HAD " + numPieces + " PIECES");
          System.out.println("REMAINING ON THE BOARD.\n");          
        
          nextStep = Step.QUERY_RETRY;      
          break;
          
        case QUERY_RETRY:

          while (true) {
            System.out.print("TRY AGAIN? ");
            userResponse = scan.nextLine();
            System.out.println("");
            
            if (userResponse.toUpperCase().equals("YES")) {
              nextStep = Step.SHOW_BOARD;
              break;
            }      
            else if (userResponse.toUpperCase().equals("NO")) {
              System.out.println("O.K.  HOPE YOU HAD FUN!!");
              return;
            }
            else {
              System.out.println("PLEASE ANSWER 'YES' OR 'NO'.");
            }
          }                               
          break;
          
        default:
          System.out.println("INVALID STEP");
          nextStep = Step.QUERY_RETRY;
          break;        

      }  // End of switch
      
    }  // End outer while loop      
    
  }  // End of method startGame  
    
  public void printBoard(int[] positions) {
    
    int column = 0;
    int row = 0;    
    String lineContent = "";
    
    // Begin loop through all rows
    for (row = 1; row <= 57; row += 8) {
      
      // Begin loop through all columns
      for (column = row; column <= row + 7; column++) {
        
        lineContent += " " + positions[column];               
        
      }  // End loop through all columns
      
      System.out.println(lineContent);
      lineContent = "";      
      
    }  // End loop through all rows

    System.out.println("");               
    
  }  // End of method printBoard
    
  public static void main(String[] args) {
    
    OneCheck game = new OneCheck();
    game.play();
    
  }  // End of method main
    
}  // End of class OneCheck
