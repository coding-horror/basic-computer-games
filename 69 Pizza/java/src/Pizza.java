import java.util.Scanner;

/**
 * Game of Pizza
 * <p>
 * Based on the Basic game of Hurkle here
 * https://github.com/coding-horror/basic-computer-games/blob/main/69%20Pizza/pizza.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Pizza {

    private final int MAX_DELIVERIES = 5;

    private enum GAME_STATE {
        STARTING,
        ENTER_NAME,
        DRAW_MAP,
        MORE_DIRECTIONS,
        START_DELIVER,
        DELIVER_PIZZA,
        TOO_DIFFICULT,
        END_GAME,
        GAME_OVER
    }

    // houses that can order pizza
    private final char[] houses = new char[]{'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
            'J', 'K', 'L', 'M', 'N', 'O', 'P'};

    // size of grid
    private final int[] gridPos = new int[]{1, 2, 3, 4};

    private GAME_STATE gameState;

    private String playerName;

    // How many pizzas have been successfully delivered
    private int pizzaDeliveryCount;

    // current house that ordered a pizza
    private int currentHouseDelivery;

    // Used for keyboard input
    private final Scanner kbScanner;

    public Pizza() {

        gameState = GAME_STATE.STARTING;

        // Initialise kb scanner
        kbScanner = new Scanner(System.in);
    }

    /**
     * Main game loop
     */
    public void play() {

        do {
            switch (gameState) {

                // Show an introduction the first time the game is played.
                case STARTING:
                    init();
                    intro();
                    gameState = GAME_STATE.ENTER_NAME;
                    break;

                // Enter the players name
                case ENTER_NAME:
                    playerName = displayTextAndGetInput("WHAT IS YOUR FIRST NAME? ");
                    System.out.println("HI " + playerName + ". IN GAME YOU ARE TO TAKE ORDERS");
                    System.out.println("FOR PIZZAS.  THEN YOU ARE TO TELL A DELIVERY BOY");
                    System.out.println("WHERE TO DELIVER THE ORDERED PIZZAS.");
                    System.out.println();
                    gameState = GAME_STATE.DRAW_MAP;
                    break;

                // Draw the map
                case DRAW_MAP:
                    drawMap();
                    gameState = GAME_STATE.MORE_DIRECTIONS;
                    break;

                // need more directions (how to play) ?
                case MORE_DIRECTIONS:
                    extendedIntro();
                    String moreInfo = displayTextAndGetInput("DO YOU NEED MORE DIRECTIONS? ");
                    if (!yesOrNoEntered(moreInfo)) {
                        System.out.println("'YES' OR 'NO' PLEASE, NOW THEN,");
                    } else {
                        // More instructions selected
                        if (yesEntered(moreInfo)) {
                            displayMoreDirections();
                            // Player understand now?
                            if (yesEntered(displayTextAndGetInput("UNDERSTAND? "))) {
                                System.out.println("GOOD.  YOU ARE NOW READY TO START TAKING ORDERS.");
                                System.out.println();
                                System.out.println("GOOD LUCK!!");
                                System.out.println();
                                gameState = GAME_STATE.START_DELIVER;
                            } else {
                                // Not understood, essentially game over
                                gameState = GAME_STATE.TOO_DIFFICULT;
                            }
                        } else {
                            // no more directions were needed, start delivering pizza
                            gameState = GAME_STATE.START_DELIVER;
                        }
                    }

                    break;

                // Too difficult to understand, game over!
                case TOO_DIFFICULT:
                    System.out.println("JOB IS DEFINITELY TOO DIFFICULT FOR YOU. THANKS ANYWAY");
                    gameState = GAME_STATE.GAME_OVER;
                    break;

                // Delivering pizza
                case START_DELIVER:
                    // select a random house and "order" a pizza for them.
                    currentHouseDelivery = (int) (Math.random()
                            * (houses.length) + 1) - 1; // Deduct 1 for 0-based array

                    System.out.println("HELLO " + playerName + "'S PIZZA.  IS "
                            + houses[currentHouseDelivery] + ".");
                    System.out.println("  PLEASE SEND A PIZZA.");
                    gameState = GAME_STATE.DELIVER_PIZZA;
                    break;

                // Try and deliver the pizza
                case DELIVER_PIZZA:

                    String question = "  DRIVER TO " + playerName + ":  WHERE DOES "
                            + houses[currentHouseDelivery] + " LIVE ? ";
                    String answer = displayTextAndGetInput(question);

                    // Convert x,y entered by player to grid position of a house
                    int x = getDelimitedValue(answer, 0);
                    int y = getDelimitedValue(answer, 1);
                    int calculatedPos = (x + (y - 1) * 4) - 1;

                    // Did the player select the right house to deliver?
                    if (calculatedPos == currentHouseDelivery) {
                        System.out.println("HELLO " + playerName + ".  IS " + houses[currentHouseDelivery]
                                + ", THANKS FOR THE PIZZA.");
                        pizzaDeliveryCount++;

                        // Delivered enough pizza?

                        if (pizzaDeliveryCount > MAX_DELIVERIES) {
                            gameState = GAME_STATE.END_GAME;
                        } else {
                            gameState = GAME_STATE.START_DELIVER;
                        }
                    } else {
                        System.out.println("IS " + houses[calculatedPos] + ".  I DID NOT ORDER A PIZZA.");
                        System.out.println("I LIVE AT " + x + "," + y);
                        // Don't change gameState so state is executed again
                    }

                    break;

                // Sign off message for cases where the Chief is not upset
                case END_GAME:
                    if (yesEntered(displayTextAndGetInput("DO YOU WANT TO DELIVER MORE PIZZAS? "))) {
                        init();
                        gameState = GAME_STATE.START_DELIVER;
                    } else {
                        System.out.println();
                        System.out.println("O.K. " + playerName + ", SEE YOU LATER!");
                        System.out.println();
                        gameState = GAME_STATE.GAME_OVER;
                    }
                    break;

                // GAME_OVER State does not specifically have a case
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    private void drawMap() {

        System.out.println("MAP OF THE CITY OF HYATTSVILLE");
        System.out.println();
        System.out.println(" -----1-----2-----3-----4-----");
        int k = 3;
        for (int i = 1; i < 5; i++) {
            System.out.println("-");
            System.out.println("-");
            System.out.println("-");
            System.out.println("-");

            System.out.print(gridPos[k]);
            int pos = 16 - 4 * i;
            System.out.print("     " + houses[pos]);
            System.out.print("     " + houses[pos + 1]);
            System.out.print("     " + houses[pos + 2]);
            System.out.print("     " + houses[pos + 3]);
            System.out.println("     " + gridPos[k]);
            k = k - 1;
        }
        System.out.println("-");
        System.out.println("-");
        System.out.println("-");
        System.out.println("-");
        System.out.println(" -----1-----2-----3-----4-----");
    }

    /**
     * Basic information about the game
     */
    private void intro() {
        System.out.println("PIZZA");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println();
        System.out.println("PIZZA DELIVERY GAME");
        System.out.println();
    }

    private void extendedIntro() {
        System.out.println("THE OUTPUT IS A MAP OF THE HOMES WHERE");
        System.out.println("YOU ARE TO SEND PIZZAS.");
        System.out.println();
        System.out.println("YOUR JOB IS TO GIVE A TRUCK DRIVER");
        System.out.println("THE LOCATION OR COORDINATES OF THE");
        System.out.println("HOME ORDERING THE PIZZA.");
        System.out.println();
    }

    private void displayMoreDirections() {
        System.out.println();
        System.out.println("SOMEBODY WILL ASK FOR A PIZZA TO BE");
        System.out.println("DELIVERED.  THEN A DELIVERY BOY WILL");
        System.out.println("ASK YOU FOR THE LOCATION.");
        System.out.println("     EXAMPLE:");
        System.out.println("IS J.  PLEASE SEND A PIZZA.");
        System.out.println("DRIVER TO " + playerName + ".  WHERE DOES J LIVE?");
        System.out.println("YOUR ANSWER WOULD BE 2,3");
        System.out.println();
    }

    private void init() {
        pizzaDeliveryCount = 1;
    }

    /**
     * Accepts a string delimited by comma's and returns the nth delimited
     * value (starting at count 0).
     *
     * @param text - text with values separated by comma's
     * @param pos  - which position to return a value for
     * @return the int representation of the value
     */
    private int getDelimitedValue(String text, int pos) {
        String[] tokens = text.split(",");
        return Integer.parseInt(tokens[pos]);
    }

    /**
     * Returns true if a given string is equal to at least one of the values specified in the call
     * to the stringIsAnyValue method
     *
     * @param text string to search
     * @return true if string is equal to one of the varargs
     */
    private boolean yesEntered(String text) {
        return stringIsAnyValue(text, "Y", "YES");
    }

    /**
     * returns true if Y, YES, N, or NO was the compared value in text
     * case-insensitive
     *
     * @param text search string
     * @return true if one of the varargs was found in text
     */
    private boolean yesOrNoEntered(String text) {
        return stringIsAnyValue(text, "Y", "YES", "N", "NO");
    }

    /**
     * Returns true if a given string contains at least one of the varargs (2nd parameter).
     * Note: Case insensitive comparison.
     *
     * @param text   string to search
     * @param values varargs of type string containing values to compare
     * @return true if one of the varargs arguments was found in text
     */
    private boolean stringIsAnyValue(String text, String... values) {

        // Cycle through the variable number of values and test each
        for (String val : values) {
            if (text.equalsIgnoreCase(val)) {
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

}