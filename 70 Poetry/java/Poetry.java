/**
 * Game of Poetry
 * <p>
 * Based on the BASIC game of Poetry here
 * https://github.com/coding-horror/basic-computer-games/blob/main/70%20Poetry/poetry.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 * 
 * Converted from BASIC to Java by Darren Cardenas.
 */
 
public class Poetry {
  
  private final static double COMMA_RATE = 0.19;
  private final static double SPACE_RATE = 0.65;  
  private final static int PARAGRAPH_RATE = 20;
  
  private enum Step {
    WORD_GROUP1, WORD_GROUP2, WORD_GROUP3, WORD_GROUP4, RANDOMIZE_COMMA, 
    RANDOMIZE_WHITESPACE, RANDOMIZE_COUNTERS
  }   

  public void play() {

    showIntro();
    startGame();

  }  // End of method play  
    
  private void showIntro() {    
    
    System.out.println(" ".repeat(29) + "POETRY");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");
    
  }  // End of method showIntro      

  private void startGame() {        
      
    int groupIndex = 0;
    int paragraphIndex = 0;
    int punctuationIndex = 0;
    int wordIndex = 1;
    
    Step nextStep = Step.WORD_GROUP1;

    // Begin outer while loop       
    while (true) {
     
      switch (nextStep) {

        case WORD_GROUP1:
          
          if (wordIndex == 1) {
            
            System.out.print("MIDNIGHT DREARY");
            nextStep = Step.RANDOMIZE_COMMA;
            
          } else if (wordIndex == 2) {
            
            System.out.print("FIERY EYES");
            nextStep = Step.RANDOMIZE_COMMA;
            
          } else if (wordIndex == 3) {
            
            System.out.print("BIRD OR FIEND");
            nextStep = Step.RANDOMIZE_COMMA;           
            
          } else if (wordIndex == 4) {
            
            System.out.print("THING OF EVIL");
            nextStep = Step.RANDOMIZE_COMMA;           
            
          } else if (wordIndex == 5) {
            
            System.out.print("PROPHET");
            nextStep = Step.RANDOMIZE_COMMA;                      
          }          
          break;
          
        case WORD_GROUP2:
          
          if (wordIndex == 1) {
            
            System.out.print("BEGUILING ME");
            nextStep = Step.RANDOMIZE_COMMA;
            
          } else if (wordIndex == 2) {
            
            System.out.print("THRILLED ME");
            nextStep = Step.RANDOMIZE_COMMA;
            
          } else if (wordIndex == 3) {
            
            System.out.print("STILL SITTING....");
            nextStep = Step.RANDOMIZE_WHITESPACE;           
            
          } else if (wordIndex == 4) {
            
            System.out.print("NEVER FLITTING");
            nextStep = Step.RANDOMIZE_COMMA;           
            
          } else if (wordIndex == 5) {
            
            System.out.print("BURNED");
            nextStep = Step.RANDOMIZE_COMMA;                      
          }          
          break;

        case WORD_GROUP3:
          
          if (wordIndex == 1) {
            
            System.out.print("AND MY SOUL");
            nextStep = Step.RANDOMIZE_COMMA;
            
          } else if (wordIndex == 2) {
            
            System.out.print("DARKNESS THERE");
            nextStep = Step.RANDOMIZE_COMMA;
            
          } else if (wordIndex == 3) {
            
            System.out.print("SHALL BE LIFTED");
            nextStep = Step.RANDOMIZE_COMMA;           
            
          } else if (wordIndex == 4) {
            
            System.out.print("QUOTH THE RAVEN");
            nextStep = Step.RANDOMIZE_COMMA;           
            
          } else if (wordIndex == 5) {
            
            if (punctuationIndex != 0) {
              
              System.out.print("SIGN OF PARTING");               
            }            
            
            nextStep = Step.RANDOMIZE_COMMA;                      
          }          
          break;   
          
        case WORD_GROUP4:
          
          if (wordIndex == 1) {
            
            System.out.print("NOTHING MORE");
            nextStep = Step.RANDOMIZE_COMMA;
            
          } else if (wordIndex == 2) {
            
            System.out.print("YET AGAIN");
            nextStep = Step.RANDOMIZE_COMMA;
            
          } else if (wordIndex == 3) {
            
            System.out.print("SLOWLY CREEPING");
            nextStep = Step.RANDOMIZE_WHITESPACE;           
            
          } else if (wordIndex == 4) {
            
            System.out.print("...EVERMORE");
            nextStep = Step.RANDOMIZE_COMMA;           
            
          } else if (wordIndex == 5) {
            
            System.out.print("NEVERMORE");
            nextStep = Step.RANDOMIZE_COMMA;                      
          }          
          break;          
          
        case RANDOMIZE_COMMA:
        
          // Insert commas
          if ((punctuationIndex != 0) && (Math.random() <= COMMA_RATE)) {
          
            System.out.print(",");
            punctuationIndex = 2;                       
          }           
          nextStep = Step.RANDOMIZE_WHITESPACE;
          break;
          
          
        case RANDOMIZE_WHITESPACE:

          // Insert spaces
          if (Math.random() <= SPACE_RATE) {
            
            System.out.print(" ");            
            punctuationIndex++;                           
            
          } 
          // Insert newlines
          else {  
            
            System.out.println("");
            punctuationIndex = 0;            
          }          
          nextStep = Step.RANDOMIZE_COUNTERS;
          break;
          
        case RANDOMIZE_COUNTERS:
          
          wordIndex = (int)((int)(10 * Math.random()) / 2) + 1;
          
          groupIndex++;
          paragraphIndex++;
          
          if ((punctuationIndex == 0) && (groupIndex % 2 == 0)) {
            
            System.out.print("     ");            
          }
        
          if (groupIndex == 1) {
            
            nextStep = Step.WORD_GROUP1;          
              
          } else if (groupIndex == 2) {
            
            nextStep = Step.WORD_GROUP2;
            
          } else if (groupIndex == 3) {
            
            nextStep = Step.WORD_GROUP3;
            
          } else if (groupIndex == 4) {
            
            nextStep = Step.WORD_GROUP4;
            
          } else if (groupIndex == 5) {
            
            groupIndex = 0;
            System.out.println("");
            
            if (paragraphIndex > PARAGRAPH_RATE) {
              
              System.out.println("");
              punctuationIndex = 0;
              paragraphIndex = 0;
              nextStep = Step.WORD_GROUP2;                     
              
            } else {
              
              nextStep = Step.RANDOMIZE_COUNTERS;              
            }            
          }
          break;                    
          
        default:
          System.out.println("INVALID STEP");
          break;       
      }   
    
    }  // End outer while loop 
    
  }  // End of method startGame
  
  public static void main(String[] args) {
    
    Poetry poetry = new Poetry();
    poetry.play();
    
  }  // End of method main

}  // End of class Poetry
