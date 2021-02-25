import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Change
 * <p>
 * Based on the Basic game of Change here
 * https://github.com/coding-horror/basic-computer-games/blob/main/22%20Change/change.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Change {

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        START_GAME,
        INPUT,
        CALCULATE,
        END_GAME,
        GAME_OVER
    }

    // Current game state
    private GAME_STATE gameState;

    // Amount of change needed to be given
    private double change;

    public Change() {
        kbScanner = new Scanner(System.in);

        gameState = GAME_STATE.START_GAME;
    }

    /**
     * Main game loop
     */
    public void play() {

        do {
            switch (gameState) {
                case START_GAME:
                    intro();
                    gameState = GAME_STATE.INPUT;
                    break;

                case INPUT:

                    double costOfItem = displayTextAndGetNumber("COST OF ITEM ");
                    double amountPaid = displayTextAndGetNumber("AMOUNT OF PAYMENT ");
                    change = amountPaid - costOfItem;
                    if (change == 0) {
                        // No change needed
                        System.out.println("CORRECT AMOUNT, THANK YOU.");
                        gameState = GAME_STATE.END_GAME;
                    } else if (change < 0) {
                        System.out.println("YOU HAVE SHORT-CHANGES ME $" + (costOfItem - amountPaid));
                        // Don't change game state so it will loop back and try again
                    } else {
                        // Change needed.
                        gameState = GAME_STATE.CALCULATE;
                    }
                    break;

                case CALCULATE:
                    System.out.println("YOUR CHANGE, $" + change);
                    calculateChange();
                    gameState = GAME_STATE.END_GAME;
                    break;

                case END_GAME:
                    System.out.println("THANK YOU, COME AGAIN");
                    System.out.println();
                    gameState = GAME_STATE.INPUT;
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    /**
     * Calculate and output the change required for the purchase based on
     * what money was paid.
     */
    private void calculateChange() {

        double originalChange = change;

        int tenDollarBills = (int) change / 10;
        if (tenDollarBills > 0) {
            System.out.println(tenDollarBills + " TEN DOLLAR BILL(S)");
        }
        change = originalChange - (tenDollarBills * 10);

        int fiveDollarBills = (int) change / 5;
        if (fiveDollarBills > 0) {
            System.out.println(fiveDollarBills + " FIVE DOLLAR BILL(S)");
        }
        change = originalChange - (tenDollarBills * 10 + fiveDollarBills * 5);

        int oneDollarBills = (int) change;
        if (oneDollarBills > 0) {
            System.out.println(oneDollarBills + " ONE DOLLAR BILL(S)");
        }
        change = originalChange - (tenDollarBills * 10 + fiveDollarBills * 5 + oneDollarBills);

        change = change * 100;
        double cents = change;

        int halfDollars = (int) change / 50;
        if (halfDollars > 0) {
            System.out.println(halfDollars + " ONE HALF DOLLAR(S)");
        }
        change = cents - (halfDollars * 50);

        int quarters = (int) change / 25;
        if (quarters > 0) {
            System.out.println(quarters + " QUARTER(S)");
        }

        change = cents - (halfDollars * 50 + quarters * 25);

        int dimes = (int) change / 10;
        if (dimes > 0) {
            System.out.println(dimes + " DIME(S)");
        }

        change = cents - (halfDollars * 50 + quarters * 25 + dimes * 10);

        int nickels = (int) change / 5;
        if (nickels > 0) {
            System.out.println(nickels + " NICKEL(S)");
        }

        change = cents - (halfDollars * 50 + quarters * 25 + dimes * 10 + nickels * 5);

        int pennies = (int) (change + .5);
        if (pennies > 0) {
            System.out.println(pennies + " PENNY(S)");
        }

    }

    private void intro() {
        System.out.println(simulateTabs(33) + "CHANGE");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("I, YOUR FRIENDLY MICROCOMPUTER, WILL DETERMINE");
        System.out.println("THE CORRECT CHANGE FOR ITEMS COSTING UP TO $100.");
        System.out.println();
    }

    /*
     * Print a message on the screen, then accept input from Keyboard.
     * Converts input to a Double
     *
     * @param text message to be displayed on screen.
     * @return what was typed by the player.
     */
    private double displayTextAndGetNumber(String text) {
        return Double.parseDouble(displayTextAndGetInput(text));
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
