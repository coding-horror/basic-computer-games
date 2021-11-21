import java.util.Scanner;
import java.util.HashMap;
import java.util.Map;
import java.lang.Math;

/**
 * Game of Banner
 * <p>
 * Based on the BASIC game of Banner here
 * https://github.com/coding-horror/basic-computer-games/blob/main/06%20Banner/banner.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 * 
 * Converted from BASIC to Java by Darren Cardenas.
 */
 
public class Banner {  

  private final Scanner scan;  // For user input
  
  public Banner() {
    
    scan = new Scanner(System.in);
    
  }  // End of constructor Banner   

  public void play() {    

    int bitIndex = 0;
    int centerFlag = 0;
    int dataIndex = 0;
    int hIndex = 0;
    int horizontal = 0;
    int index = 0;
    int letterIndex = 0;
    int tempVal = 0;
    int vertical = 0;
    int vIndex = 0;
    int writeIndex = 0;
    
    int[] writerMap = new int[10];
    int[] writeLimit = new int[10];   
    
    String centerResponse = "";
    String characters = "";
    String letter = "";
    String lineContent = ""; 
    String setPage = "";
    String statement = "";
    String token = "";  // Print token
    
    Map<String, int[]> symbolData = new HashMap<String, int[]>();  
    symbolData.put(" ", new int[]{0,0,0,0,0,0,0,0              });
    symbolData.put("A", new int[]{0,505,37,35,34,35,37,505     });
    symbolData.put("G", new int[]{0,125,131,258,258,290,163,101});
    symbolData.put("E", new int[]{0,512,274,274,274,274,258,258});
    symbolData.put("T", new int[]{0,2,2,2,512,2,2,2            });
    symbolData.put("W", new int[]{0,256,257,129,65,129,257,256 });
    symbolData.put("L", new int[]{0,512,257,257,257,257,257,257});
    symbolData.put("S", new int[]{0,69,139,274,274,274,163,69  });
    symbolData.put("O", new int[]{0,125,131,258,258,258,131,125});
    symbolData.put("N", new int[]{0,512,7,9,17,33,193,512      });
    symbolData.put("F", new int[]{0,512,18,18,18,18,2,2        });
    symbolData.put("K", new int[]{0,512,17,17,41,69,131,258    });
    symbolData.put("B", new int[]{0,512,274,274,274,274,274,239});
    symbolData.put("D", new int[]{0,512,258,258,258,258,131,125});
    symbolData.put("H", new int[]{0,512,17,17,17,17,17,512     });
    symbolData.put("M", new int[]{0,512,7,13,25,13,7,512       });
    symbolData.put("?", new int[]{0,5,3,2,354,18,11,5          });
    symbolData.put("U", new int[]{0,128,129,257,257,257,129,128});
    symbolData.put("R", new int[]{0,512,18,18,50,82,146,271    });
    symbolData.put("P", new int[]{0,512,18,18,18,18,18,15      });
    symbolData.put("Q", new int[]{0,125,131,258,258,322,131,381});
    symbolData.put("Y", new int[]{0,8,9,17,481,17,9,8          });
    symbolData.put("V", new int[]{0,64,65,129,257,129,65,64    });
    symbolData.put("X", new int[]{0,388,69,41,17,41,69,388     });
    symbolData.put("Z", new int[]{0,386,322,290,274,266,262,260});
    symbolData.put("I", new int[]{0,258,258,258,512,258,258,258});
    symbolData.put("C", new int[]{0,125,131,258,258,258,131,69 });
    symbolData.put("J", new int[]{0,65,129,257,257,257,129,128 });
    symbolData.put("1", new int[]{0,0,0,261,259,512,257,257    });
    symbolData.put("2", new int[]{0,261,387,322,290,274,267,261});
    symbolData.put("*", new int[]{0,69,41,17,512,17,41,69      });
    symbolData.put("3", new int[]{0,66,130,258,274,266,150,100 });
    symbolData.put("4", new int[]{0,33,49,41,37,35,512,33      });
    symbolData.put("5", new int[]{0,160,274,274,274,274,274,226});
    symbolData.put("6", new int[]{0,194,291,293,297,305,289,193});
    symbolData.put("7", new int[]{0,258,130,66,34,18,10,8      });
    symbolData.put("8", new int[]{0,69,171,274,274,274,171,69  });
    symbolData.put("9", new int[]{0,263,138,74,42,26,10,7      });
    symbolData.put("=", new int[]{0,41,41,41,41,41,41,41       });
    symbolData.put("!", new int[]{0,1,1,1,384,1,1,1            });
    symbolData.put("0", new int[]{0,57,69,131,258,131,69,57    });
    symbolData.put(".", new int[]{0,1,1,129,449,129,1,1        });    

    System.out.print("HORIZONTAL? "); 
    horizontal = Integer.parseInt(scan.nextLine());

    System.out.print("VERTICAL? "); 
    vertical = Integer.parseInt(scan.nextLine());   
    
    System.out.print("CENTERED? "); 
    centerResponse = scan.nextLine().toUpperCase();
    
    centerFlag = 0;
    
    // Lexicographical comparison
    if (centerResponse.compareTo("P") > 0) {
      centerFlag = 1;      
    }
    
    System.out.print("CHARACTER (TYPE 'ALL' IF YOU WANT CHARACTER BEING PRINTED)? "); 
    characters = scan.nextLine().toUpperCase();
    
    System.out.print("STATEMENT? ");
    statement = scan.nextLine().toUpperCase();
    
    // Initiates the print
    System.out.print("SET PAGE? ");
    setPage = scan.nextLine();  
    
    // Begin loop through statement letters
    for (letterIndex = 1; letterIndex <= statement.length(); letterIndex++) {        
      
      // Extract a letter
      letter = String.valueOf(statement.charAt(letterIndex - 1));
      
      // Begin loop through all symbol data 
      for (String symbolString: symbolData.keySet()) {   
       
        // Begin letter handling
        if (letter.equals(" ")) {       
          for (index = 1; index <= (7 * horizontal); index++) {                 
            System.out.println("");
          }                  
          break;
          
        } else if (letter.equals(symbolString)) {          
          token = characters;

          if (characters.equals("ALL")) {                      
            token = symbolString;
          }   
         
          for (dataIndex = 1; dataIndex <= 7; dataIndex++) {
            
            // Avoid overwriting symbol data
            tempVal = symbolData.get(symbolString)[dataIndex];
            
            for (bitIndex = 8; bitIndex >= 0; bitIndex--) {

              if (Math.pow(2, bitIndex) < tempVal) {
                writerMap[9 - bitIndex] = 1;
                tempVal -= Math.pow(2, bitIndex);

                if (tempVal == 1) {                   
                  writeLimit[dataIndex] = 9 - bitIndex;                    
                  break;
                }
                
              } else {
                
                writerMap[9 - bitIndex] = 0;               
              }
            }  // End of bitIndex loop
            
            for (hIndex = 1; hIndex <= horizontal; hIndex++) {
            
              // Add whitespace for centering 
              lineContent = " ".repeat((int)((63 - 4.5 * vertical) * centerFlag / token.length()));
              
              for (writeIndex = 1; writeIndex <= writeLimit[dataIndex]; writeIndex++) {
                
                if (writerMap[writeIndex] == 0) {                                 
                
                  for (vIndex = 1; vIndex <= vertical; vIndex++) {                    
                  
                    for (index = 1; index <= token.length(); index++) {                              
                      lineContent += " ";                              
                    }                            
                  }                       
                  
                } else {
                  
                  for (vIndex = 1; vIndex <= vertical; vIndex++) {                    
                    lineContent += token;
                  }                                                                             
                }

              }  // End of writeIndex loop

              System.out.println(lineContent);
              
            }  // End of hIndex loop            

          }  // End of dataIndex loop
          
          // Add padding between letters
          for (index = 1; index <= 2 * horizontal; index++) {            
            System.out.println("");
          }
          
        }  // End letter handling

      }  // End loop through all symbol data
      
    }  // End loop through statement letters

    // Add extra length to the banner
    for (index = 1; index <= 75; index++) {    
      System.out.println("");
    }  

  }  // End of method play     
  
  public static void main(String[] args) {
    
    Banner game = new Banner();
    game.play();
    
  }  // End of method main
    
}  // End of class Banner
