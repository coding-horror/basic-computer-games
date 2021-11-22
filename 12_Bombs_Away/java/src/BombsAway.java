import java.util.Scanner;

/**
 * Game of Bombs Away
 *
 * Based on the Basic game of Bombs Away here
 * https://github.com/coding-horror/basic-computer-games/blob/main/12%20Bombs%20Away/bombsaway.bas
 *
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 *        new features - no additional text, error checking, etc has been added.
 */
public class BombsAway {

    public static final int MAX_PILOT_MISSIONS = 160;
    public static final int MAX_CASUALTIES = 100;
    public static final int MISSED_TARGET_CONST_1 = 2;
    public static final int MISSED_TARGET_CONST_2 = 30;
    public static final int CHANCE_OF_BEING_SHOT_DOWN_BASE = 100;
    public static final double SIXTY_FIVE_PERCENT = .65;

    private enum GAME_STATE {
        START,
        CHOOSE_SIDE,
        CHOOSE_PLANE,
        CHOOSE_TARGET,
        CHOOSE_MISSIONS,
        CHOOSE_ENEMY_DEFENCES,
        FLY_MISSION,
        DIRECT_HIT,
        MISSED_TARGET,
        PROCESS_FLAK,
        SHOT_DOWN,
        MADE_IT_THROUGH_FLAK,
        PLAY_AGAIN,
        GAME_OVER
    }

    public enum SIDE {
        ITALY(1),
        ALLIES(2),
        JAPAN(3),
        GERMANY(4);

        private final int value;

        SIDE(int value) {
            this.value = value;
        }

        public int getValue() {
            return value;
        }


    }

    public enum TARGET {
        ALBANIA(1),
        GREECE(2),
        NORTH_AFRICA(3),
        RUSSIA(4),
        ENGLAND(5),
        FRANCE(6);

        private final int value;

        TARGET(int value) {
            this.value = value;
        }

        public int getValue() {
            return value;
        }
    }

    public enum ENEMY_DEFENCES {
        GUNS(1),
        MISSILES(2),
        BOTH(3);

        private final int value;

        ENEMY_DEFENCES(int value) {
            this.value = value;
        }

        public int getValue() {
            return value;
        }
    }

    public enum AIRCRAFT {
        LIBERATOR(1),
        B29(2),
        B17(3),
        LANCASTER(4);

        private final int value;

        AIRCRAFT(int value) {
            this.value = value;
        }

        public int getValue() {
            return value;
        }
    }

    // Used for keyboard input
    private final Scanner kbScanner;

    // Current game state
    private GAME_STATE gameState;

    private SIDE side;

    private int missions;

    private int chanceToHit;
    private int percentageHitRateOfGunners;

    public BombsAway() {

        gameState = GAME_STATE.START;

        // Initialise kb scanner
        kbScanner = new Scanner(System.in);
    }

    /**
     * Main game loop
     *
     */
    public void play() {

        do {
            switch (gameState) {

                // Show an introduction the first time the game is played.
                case START:
                    intro();
                    chanceToHit = 0;
                    percentageHitRateOfGunners = 0;

                    gameState = GAME_STATE.CHOOSE_SIDE;
                    break;

                case CHOOSE_SIDE:
                    side = getSide("WHAT SIDE -- ITALY(1), ALLIES(2), JAPAN(3), GERMANY(4) ? ");
                    if (side == null) {
                        System.out.println("TRY AGAIN...");
                    } else {
                        // Different game paths depending on which side was chosen
                        switch (side) {
                            case ITALY:
                            case GERMANY:
                                gameState = GAME_STATE.CHOOSE_TARGET;
                                break;
                            case ALLIES:
                            case JAPAN:
                                gameState = GAME_STATE.CHOOSE_PLANE;
                                break;
                        }
                    }
                    break;

                case CHOOSE_TARGET:
                    String prompt;
                    if (side == SIDE.ITALY) {
                        prompt = "YOUR TARGET -- ALBANIA(1), GREECE(2), NORTH AFRICA(3) ? ";
                    } else {
                        // Germany
                        System.out.println("A NAZI, EH?  OH WELL.  ARE YOU GOING FOR RUSSIA(1),");
                        prompt = "ENGLAND(2), OR FRANCE(3) ? ";
                    }
                    TARGET target = getTarget(prompt);
                    if (target == null) {
                        System.out.println("TRY AGAIN...");
                    } else {
                        displayTargetMessage(target);
                        gameState = GAME_STATE.CHOOSE_MISSIONS;
                    }

                case CHOOSE_MISSIONS:
                    missions = getNumberFromKeyboard("HOW MANY MISSIONS HAVE YOU FLOWN? ");

                    if(missions <25) {
                        System.out.println("FRESH OUT OF TRAINING, EH?");
                        gameState = GAME_STATE.FLY_MISSION;
                    } else if(missions < 100) {
                        System.out.println("THAT'S PUSHING THE ODDS!");
                        gameState = GAME_STATE.FLY_MISSION;
                    } else if(missions >=160) {
                        System.out.println("MISSIONS, NOT MILES...");
                        System.out.println("150 MISSIONS IS HIGH EVEN FOR OLD-TIMERS.");
                        System.out.println("NOW THEN, ");
                    } else {
                        // No specific message if missions is 100-159, but still valid
                        gameState = GAME_STATE.FLY_MISSION;
                    }
                    break;

                case CHOOSE_PLANE:
                    switch(side) {
                        case ALLIES:
                            AIRCRAFT plane = getPlane("AIRCRAFT -- LIBERATOR(1), B-29(2), B-17(3), LANCASTER(4)? ");
                            if(plane == null) {
                                System.out.println("TRY AGAIN...");
                            } else {
                                switch(plane) {

                                    case LIBERATOR:
                                        System.out.println("YOU'VE GOT 2 TONS OF BOMBS FLYING FOR PLOESTI.");
                                        break;
                                    case B29:
                                        System.out.println("YOU'RE DUMPING THE A-BOMB ON HIROSHIMA.");
                                        break;
                                    case B17:
                                        System.out.println("YOU'RE CHASING THE BISMARK IN THE NORTH SEA.");
                                        break;
                                    case LANCASTER:
                                        System.out.println("YOU'RE BUSTING A GERMAN HEAVY WATER PLANT IN THE RUHR.");
                                        break;
                                }

                                gameState = GAME_STATE.CHOOSE_MISSIONS;
                            }
                            break;

                        case JAPAN:
                            System.out.println("YOU'RE FLYING A KAMIKAZE MISSION OVER THE USS LEXINGTON.");
                            if(yesEntered(displayTextAndGetInput("YOUR FIRST KAMIKAZE MISSION(Y OR N) ? "))) {
                                if(randomNumber(1) > SIXTY_FIVE_PERCENT) {
                                    gameState = GAME_STATE.DIRECT_HIT;
                                } else {
                                    // It's a miss
                                    gameState = GAME_STATE.MISSED_TARGET;
                                }
                            } else {
                                gameState = GAME_STATE.PROCESS_FLAK;
                            }
                            break;
                    }
                    break;

                case FLY_MISSION:
                    double missionResult = (MAX_PILOT_MISSIONS * randomNumber(1));
                    if(missions > missionResult) {
                        gameState = GAME_STATE.DIRECT_HIT;
                    } else {
                        gameState = GAME_STATE.MISSED_TARGET;
                    }

                    break;

                case DIRECT_HIT:
                    System.out.println("DIRECT HIT!!!! " + (int) Math.round(randomNumber(MAX_CASUALTIES)) + " KILLED.");
                    System.out.println("MISSION SUCCESSFUL.");
                    gameState = GAME_STATE.PLAY_AGAIN;
                    break;

                case MISSED_TARGET:
                    System.out.println("MISSED TARGET BY " + (int) Math.round(MISSED_TARGET_CONST_1 + MISSED_TARGET_CONST_2 * (randomNumber(1))) + " MILES!");
                    System.out.println("NOW YOU'RE REALLY IN FOR IT !!");
                    System.out.println();
                    gameState = GAME_STATE.CHOOSE_ENEMY_DEFENCES;
                    break;

                case CHOOSE_ENEMY_DEFENCES:
                    boolean bothWeapons = true;

                    ENEMY_DEFENCES enemyDefences = getEnemyDefences("DOES THE ENEMY HAVE GUNS(1), MISSILES(2), OR BOTH(3) ? ");
                    if(enemyDefences == null) {
                        System.out.println("TRY AGAIN...");
                    } else {
                        switch(enemyDefences) {
                            case MISSILES:
                            case GUNS:
                                bothWeapons = false;

                                // fall through on purpose to BOTH since its pretty much identical code other than the chance to hit
                                // increasing if both weapons are part of the defence.

                            case BOTH:
                                percentageHitRateOfGunners = getNumberFromKeyboard("WHAT'S THE PERCENT HIT RATE OF ENEMY GUNNERS (10 TO 50)? ");
                                if(percentageHitRateOfGunners < 10) {
                                    System.out.println("YOU LIE, BUT YOU'LL PAY...");
                                }
                                if(bothWeapons) {
                                    chanceToHit = 35;

                                }
                                break;
                        }
                    }
                    gameState = GAME_STATE.PROCESS_FLAK;

                // Determine if the players airplan makes it through the Flak.
                case PROCESS_FLAK:
                    double calc = (CHANCE_OF_BEING_SHOT_DOWN_BASE * randomNumber(1));

                    if ((chanceToHit + percentageHitRateOfGunners) > calc) {
                        gameState = GAME_STATE.SHOT_DOWN;
                    } else {
                        gameState = GAME_STATE.MADE_IT_THROUGH_FLAK;
                    }
                    break;

                case SHOT_DOWN:
                    System.out.println("* * * * BOOM * * * *");
                    System.out.println("YOU HAVE BEEN SHOT DOWN.....");
                    System.out.println("DEARLY BELOVED, WE ARE GATHERED HERE TODAY TO PAY OUR");
                    System.out.println("LAST TRIBUTE...");
                    gameState = GAME_STATE.PLAY_AGAIN;
                    break;

                case MADE_IT_THROUGH_FLAK:
                    System.out.println("YOU MADE IT THROUGH TREMENDOUS FLAK!!");
                    gameState = GAME_STATE.PLAY_AGAIN;
                    break;

                case PLAY_AGAIN:
                    if(yesEntered(displayTextAndGetInput("ANOTHER MISSION (Y OR N) ? "))) {
                        gameState = GAME_STATE.START;
                    } else {
                        System.out.println("CHICKEN !!!");
                        gameState = GAME_STATE.GAME_OVER;
                    }
                    break;
            }
        } while (gameState != GAME_STATE.GAME_OVER) ;
    }

    /**
     * Display a (brief) intro
     */
    public void intro() {
        System.out.println("YOU ARE A PILOT IN A WORLD WAR II BOMBER.");
    }

    /**
     * Determine the side the player is going to play on.
     * @param message displayed before the kb input
     * @return the SIDE enum selected by the player
     */
    private SIDE getSide(String message) {
        int valueEntered = getNumberFromKeyboard(message);
        for(SIDE side : SIDE.values()) {
            if(side.getValue() == valueEntered) {
                return side;
            }
        }

        // Input out of range
        return null;
    }

    /**
     * Determine the target the player is going for.
     * @param message displayed before the kb input
     * @return the TARGET enum selected by the player
     */
    private TARGET getTarget(String message) {
        int valueEntered = getNumberFromKeyboard(message);

        for(TARGET target : TARGET.values()) {
            if(target.getValue() == valueEntered) {
                return target;
            }
        }

        // Input out of range
        return null;
    }

    /**
     * Determine the airplane the player is going to fly.
     * @param message displayed before the kb input
     * @return the AIRCRAFT enum selected by the player
     */
    private AIRCRAFT getPlane(String message) {
        int valueEntered = getNumberFromKeyboard(message);

        for(AIRCRAFT plane : AIRCRAFT.values()) {
            if(plane.getValue() == valueEntered) {
                return plane;
            }
        }

        // Input out of range
        return null;

    }

    /**
     * Select the type of enemy defences.
     *
     * @param message displayed before kb input
     * @return the ENEMY_DEFENCES enum as selected by player
     */
    private ENEMY_DEFENCES getEnemyDefences(String message) {
        int valueEntered = getNumberFromKeyboard(message);
        for (ENEMY_DEFENCES enemyDefences : ENEMY_DEFENCES.values()) {
            if(enemyDefences.getValue() == valueEntered) {
                return enemyDefences;
            }
        }

        // Input out of range
        return null;
    }

    // output a specific message based on the target selected
    private void displayTargetMessage(TARGET target) {

        switch (target) {

            case ALBANIA:
                System.out.println("SHOULD BE EASY -- YOU'RE FLYING A NAZI-MADE PLANE.");
                break;
            case GREECE:
                System.out.println("BE CAREFUL!!!");
                break;
            case NORTH_AFRICA:
                System.out.println("YOU'RE GOING FOR THE OIL, EH?");
                break;
            case RUSSIA:
                System.out.println("YOU'RE NEARING STALINGRAD.");
                break;
            case ENGLAND:
                System.out.println("NEARING LONDON.  BE CAREFUL, THEY'VE GOT RADAR.");
                break;
            case FRANCE:
                System.out.println("NEARING VERSAILLES.  DUCK SOUP.  THEY'RE NEARLY DEFENSELESS.");
                break;
        }
    }

    /**
     * Accepts a string from the keyboard, and converts to an int
     *
     * @param message displayed text on screen before keyboard input
     *
     * @return the number entered by the player
     */
    private int getNumberFromKeyboard(String message) {

        String answer = displayTextAndGetInput(message);
        return Integer.parseInt(answer);
    }

    /**
     * Checks whether player entered Y or YES to a question.
     *
     * @param text  player string from kb
     * @return true of Y or YES was entered, otherwise false
     */
    private boolean yesEntered(String text) {
        return stringIsAnyValue(text, "Y", "YES");
    }

    /**
     * Check whether a string equals one of a variable number of values
     * Useful to check for Y or YES for example
     * Comparison is case insensitive.
     *
     * @param text source string
     * @param values a range of values to compare against the source string
     * @return true if a comparison was found in one of the variable number of strings passed
     */
    private boolean stringIsAnyValue(String text, String... values) {

        // Cycle through the variable number of values and test each
        for(String val:values) {
            if(text.equalsIgnoreCase(val)) {
                return true;
            }
        }

        // no matches
        return false;
    }

    /*
     * Print a message on the screen, then accept input from Keyboard.
     *
     * @param text message to be displayed on screen.
     * @return what was typed by the player.
     */
    private String displayTextAndGetInput(String text) {
        System.out.print(text);
        return kbScanner.next();
    }

    /**
     * Generate random number
     * Used as a single digit of the computer player
     *
     * @return random number
     */
    private double randomNumber(int range) {
        return (Math.random()
                * (range));
    }
}