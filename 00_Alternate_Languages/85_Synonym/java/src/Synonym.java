import java.util.ArrayList;
import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Synonym
 * <p>
 * Based on the Basic game of Synonym here
 * https://github.com/coding-horror/basic-computer-games/blob/main/85%20Synonym/synonym.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Synonym {

    public static final String[] RANDOM_ANSWERS = {"RIGHT", "CORRECT", "FINE", "GOOD!", "CHECK"};

    // Used for keyboard input
    private final Scanner kbScanner;

    // List of words and synonyms
    private final ArrayList<SynonymList> synonyms;

    private enum GAME_STATE {
        INIT,
        PLAY,
        GAME_OVER
    }

    // Current game state
    private GAME_STATE gameState;

    private int currentQuestion;

    public Synonym() {

        kbScanner = new Scanner(System.in);
        synonyms = new ArrayList<>();

        gameState = GAME_STATE.INIT;
    }

    /**
     * Main game loop
     */
    public void play() {

        do {
            switch (gameState) {

                case INIT:
                    intro();
                    currentQuestion = 0;

                    // Load data
                    synonyms.add(new SynonymList("FIRST", new String[]{"START", "BEGINNING", "ONSET", "INITIAL"}));
                    synonyms.add(new SynonymList("SIMILAR", new String[]{"SAME", "LIKE", "RESEMBLING"}));
                    synonyms.add(new SynonymList("MODEL", new String[]{"PATTERN", "PROTOTYPE", "STANDARD", "CRITERION"}));
                    synonyms.add(new SynonymList("SMALL", new String[]{"INSIGNIFICANT", "LITTLE", "TINY", "MINUTE"}));
                    synonyms.add(new SynonymList("STOP", new String[]{"HALT", "STAY", "ARREST", "CHECK", "STANDSTILL"}));
                    synonyms.add(new SynonymList("HOUSE", new String[]{"DWELLING", "RESIDENCE", "DOMICILE", "LODGING", "HABITATION"}));
                    synonyms.add(new SynonymList("PIT", new String[]{"HOLE", "HOLLOW", "WELL", "GULF", "CHASM", "ABYSS"}));
                    synonyms.add(new SynonymList("PUSH", new String[]{"SHOVE", "THRUST", "PROD", "POKE", "BUTT", "PRESS"}));
                    synonyms.add(new SynonymList("RED", new String[]{"ROUGE", "SCARLET", "CRIMSON", "FLAME", "RUBY"}));
                    synonyms.add(new SynonymList("PAIN", new String[]{"SUFFERING", "HURT", "MISERY", "DISTRESS", "ACHE", "DISCOMFORT"}));

                    gameState = GAME_STATE.PLAY;
                    break;

                case PLAY:

                    // Get the word and synonyms to ask a question about
                    SynonymList synonym = synonyms.get(currentQuestion);
                    String getAnswer = displayTextAndGetInput("     WHAT IS A SYNONYM OF " + synonym.getWord() + " ? ");

                    // HELP is used to give a random synonym for the current word
                    if (getAnswer.equals("HELP")) {
                        int randomSynonym = (int) (Math.random() * synonym.size());
                        System.out.println("**** A SYNONYM OF " + synonym.getWord() + " IS " + synonym.getSynonyms()[randomSynonym] + ".");
                    } else {
                        // Check if the entered word is in the synonym list
                        if (synonym.exists(getAnswer)) {
                            // If it is, give a random "correct" response
                            System.out.println(RANDOM_ANSWERS[(int) (Math.random() * RANDOM_ANSWERS.length)]);
                            currentQuestion++;
                            // Have we reached the final word/synonyms on file?
                            if (currentQuestion == synonyms.size()) {
                                // We have so end game.
                                System.out.println("SYNONYM DRILL COMPLETED.");
                                gameState = GAME_STATE.GAME_OVER;
                            }
                        } else {
                            // Word does not exist in the synonym list
                            System.out.println("TRY AGAIN.");
                        }
                    }
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    private void intro() {
        System.out.println(simulateTabs(33) + "SYNONYM");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("A SYNONYM OF A WORD MEANS ANOTHER WORD IN THE ENGLISH");
        System.out.println("LANGUAGE WHICH HAS THE SAME OR VERY NEARLY THE SAME");
        System.out.println(" MEANING.");
        System.out.println("I CHOOSE A WORD -- YOU TYPE A SYNONYM.");
        System.out.println("IF YOU CAN'T THINK OF A SYNONYM, TYPE THE WORD 'HELP'");
        System.out.println("AND I WILL TELL YOU A SYNONYM.");
        System.out.println();
    }

    /*
     * Print a message on the screen, then accept input from Keyboard.
     * Converts input to uppercase.
     *
     * @param text message to be displayed on screen.
     * @return what was typed by the player.
     */
    private String displayTextAndGetInput(String text) {
        System.out.print(text);
        return kbScanner.next().toUpperCase();
    }

    /**
     * Simulate the old basic tab(xx) command which indented text by xx spaces.
     *
     * @param spaces number of spaces required
     * @return String with number of spaces
     */
    private String simulateTabs(int spaces) {
        char[] spacesTemp = new char[spaces];
        Arrays.fill(spacesTemp, ' ');
        return new String(spacesTemp);
    }
}
