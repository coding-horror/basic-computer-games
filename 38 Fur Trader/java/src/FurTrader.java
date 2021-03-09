import java.util.ArrayList;
import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Fur Trader
 * <p>
 * Based on the Basic game of Fur Trader here
 * https://github.com/coding-horror/basic-computer-games/blob/main/38%20Fur%20Trader/furtrader.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class FurTrader {

    public static final double START_SAVINGS_AMOUNT = 600.0;
    public static final int STARTING_FURS = 190;

    public static final int FORT_HOCHELAGA_MONTREAL = 1;
    public static final int FORT_STADACONA_QUEBEC = 2;
    public static final int FORT_NEW_YORK = 3;

    public static final String MINK = "MINK";
    public static final int MINK_ENTRY = 0;
    public static final String BEAVER = "BEAVER";
    public static final int BEAVER_ENTRY = 1;
    public static final String ERMINE = "ERMINE";
    public static final int ERMINE_ENTRY = 2;
    public static final String FOX = "FOX";
    public static final int FOX_ENTRY = 3;


    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        STARTUP,
        INIT,
        TRADE_AT_FORT,
        TRADE_SUMMARY,
        TRADE_AGAIN,
        GAME_OVER
    }

    // Current game state
    private GAME_STATE gameState;

    private double savings;
    private double minkPrice;
    private double beaverPrice;
    private double erminePrice;
    private double foxPrice;

    private ArrayList<Pelt> pelts;

    public FurTrader() {
        kbScanner = new Scanner(System.in);
        gameState = GAME_STATE.INIT;
    }

    /**
     * Main game loop
     */
    public void play() {

        do {
            switch (gameState) {

                case INIT:
                    savings = START_SAVINGS_AMOUNT;

                    gameStartupMessage();

                    if (yesEntered(displayTextAndGetInput("DO YOU WISH TO TRADE FURS? "))) {
                        gameState = GAME_STATE.STARTUP;
                        intro();
                    } else {
                        gameState = GAME_STATE.GAME_OVER;
                    }

                    System.out.println("YOU HAVE $" + savings + " SAVINGS.");
                    System.out.println("AND " + STARTING_FURS + " FURS TO BEGIN THE EXPEDITION.");

                    break;

                case STARTUP:

                    pelts = new ArrayList<>();

                    pelts.add(getPelt(MINK));
                    pelts.add(getPelt(BEAVER));
                    pelts.add(getPelt(ERMINE));
                    pelts.add(getPelt(FOX));

                    // Determine if count of total pelts exceeds the maximum
                    // N.B. Original game was fine with entering less than the
                    // maximum so this functionality has been left in.
                    if (pelts.stream().mapToInt(Pelt::getNumber).sum() > STARTING_FURS) {
                        System.out.println("YOU MAY NOT HAVE THAT MANY FURS.");
                        System.out.println("DO NOT TRY TO CHEAT.  I CAN ADD.");
                        System.out.println("YOU MUST START AGAIN.");
                        gameState = GAME_STATE.STARTUP;
                    } else {
                        // No cheating, so start trading
                        erminePrice = ((.15 * Math.random() + .95) * (Math.pow(10, 2) + .5)) / Math.pow(10, 2);
                        beaverPrice = ((.25 * Math.random() + 1.00) * (Math.pow(10, 2) + .5)) / Math.pow(10, 2);
                        gameState = GAME_STATE.TRADE_AT_FORT;
                    }
                    break;

                case TRADE_AT_FORT:

                    extendedTradingInfo();
                    int answer = displayTextAndGetNumber("ANSWER 1, 2, OR 3. ");

                    // Now show the details of the fort they are about to trade
                    // and give the player the chance to NOT proceed.
                    // A "No" or false means they do not want to change to another fort
                    if (!confirmFort(answer)) {
                        switch (answer) {
                            case 1:
                                tradeAtFortHochelagaMontreal();
                                gameState = GAME_STATE.TRADE_SUMMARY;
                                break;

                            case 2:
                                tradeAtFortStadaconaQuebec();
                                gameState = GAME_STATE.TRADE_SUMMARY;
                                break;
                            case 3:
                                // Did the player and party all die?
                                if(!tradeAtFortNewYork()) {
                                    gameState = GAME_STATE.GAME_OVER;
                                } else {
                                    gameState = GAME_STATE.TRADE_SUMMARY;
                                }
                                break;
                        }

                        break;
                    }

                case TRADE_SUMMARY:

                    double beaverTotal = beaverPrice * pelts.get(BEAVER_ENTRY).getNumber();
                    System.out.println("YOUR BEAVER SOLD FOR $ " + beaverTotal);

                    double foxTotal = foxPrice * pelts.get(FOX_ENTRY).getNumber();
                    System.out.println("YOUR FOX SOLD FOR $ " + foxTotal);

                    double erMineTotal = erminePrice * pelts.get(ERMINE_ENTRY).getNumber();
                    System.out.println("YOUR ERMINE SOLD FOR $ " + erMineTotal);

                    double minkTotal = minkPrice * pelts.get(MINK_ENTRY).getNumber();
                    System.out.println("YOUR MINK SOLD FOR $ " + minkTotal);

                    savings += beaverTotal + foxTotal + erMineTotal + minkTotal;
                    System.out.println();
                    System.out.println("YOU NOW HAVE $" + savings + " INCLUDING YOUR PREVIOUS SAVINGS");

                    gameState = GAME_STATE.TRADE_AGAIN;
                    break;

                case TRADE_AGAIN:
                    if(yesEntered(displayTextAndGetInput("DO YOU WANT TO TRADE FURS NEXT YEAR? "))) {
                        gameState = GAME_STATE.STARTUP;
                    } else {
                        gameState = GAME_STATE.GAME_OVER;
                    }

            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    /**
     * Display a message about trading at each fort and confirm if the player wants to trade
     * at ANOTHER fort
     * @param fort the fort in question
     * @return true if YES was typed by player
     */
    private boolean confirmFort(int fort) {
        switch(fort) {
            case FORT_HOCHELAGA_MONTREAL:
                System.out.println("YOU HAVE CHOSEN THE EASIEST ROUTE.  HOWEVER, THE FORT");
                System.out.println("IS FAR FROM ANY SEAPORT.  THE VALUE");
                System.out.println("YOU RECEIVE FOR YOUR FURS WILL BE LOW AND THE COST");
                System.out.println("OF SUPPLIES HIGHER THAN AT FORTS STADACONA OR NEW YORK.");
                break;
            case FORT_STADACONA_QUEBEC:
                System.out.println("YOU HAVE CHOSEN A HARD ROUTE.  IT IS, IN COMPARSION,");
                System.out.println("HARDER THAN THE ROUTE TO HOCHELAGA BUT EASIER THAN");
                System.out.println("THE ROUTE TO NEW YORK.  YOU WILL RECEIVE AN AVERAGE VALUE");
                System.out.println("FOR YOUR FURS AND THE COST OF YOUR SUPPLIES WILL BE AVERAGE.");
                break;
            case FORT_NEW_YORK:
                System.out.println("YOU HAVE CHOSEN THE MOST DIFFICULT ROUTE.  AT");
                System.out.println("FORT NEW YORK YOU WILL RECEIVE THE HIGHEST VALUE");
                System.out.println("FOR YOUR FURS.  THE COST OF YOUR SUPPLIES");
                System.out.println("WILL BE LOWER THAN AT ALL THE OTHER FORTS.");
                break;
        }

        System.out.println("DO YOU WANT TO TRADE AT ANOTHER FORT?");
        return yesEntered(displayTextAndGetInput("ANSWER YES OR NO "));

    }

    /**
     * Trade at the safest fort - Fort Hochelaga
     * No chance of anything bad happening, so just calculate amount per pelt
     * and return
     */
    private void tradeAtFortHochelagaMontreal() {
        savings -= 160;
        System.out.println();
        System.out.println("SUPPLIES AT FORT HOCHELAGA COST $150.00.");
        System.out.println("YOUR TRAVEL EXPENSES TO HOCHELAGA WERE $10.00.");
        minkPrice = ((.2 * Math.random() + .7) * (Math.pow(10, 2) + .5)) / Math.pow(10, 2);
        erminePrice = ((.2 * Math.random() + .65) * (Math.pow(10, 2) + .5)) / Math.pow(10, 2);
        beaverPrice = ((.2 * Math.random() + .75) * (Math.pow(10, 2) + .5)) / Math.pow(10, 2);
        foxPrice = ((.2 * Math.random() + .8) * (Math.pow(10, 2) + .5)) / Math.pow(10, 2);
        return;
    }


    private void tradeAtFortStadaconaQuebec() {
        savings -= 140;
        System.out.println();
        minkPrice = ((.2 * Math.random() + .85) * (Math.pow(10, 2) + .5)) / Math.pow(10, 2);
        erminePrice = ((.2 * Math.random() + .8) * (Math.pow(10, 2) + .5)) / Math.pow(10, 2);
        beaverPrice = ((.2 * Math.random() + .9) * (Math.pow(10, 2) + .5)) / Math.pow(10, 2);

        // What happened during the trip to the fort?
        int tripResult = (int) (Math.random()*10) + 1;
        if(tripResult <= 2) {
            // Find the Beaver pelt in our Arraylist
            Pelt beaverPelt = pelts.get(BEAVER_ENTRY);
            // Pelts got stolen, so update to a count of zero
            beaverPelt.lostPelts();
            // Update it back in the ArrayList
            pelts.set(BEAVER_ENTRY, beaverPelt);

            System.out.println("YOUR BEAVER WERE TOO HEAVY TO CARRY ACROSS");
            System.out.println("THE PORTAGE.  YOU HAD TO LEAVE THE PELTS, BUT FOUND");
            System.out.println("THEM STOLEN WHEN YOU RETURNED.");
        } else if (tripResult <= 6) {
            System.out.println("YOU ARRIVED SAFELY AT FORT STADACONA.");
        } else if (tripResult <= 8) {
            System.out.println("YOUR CANOE UPSET IN THE LACHINE RAPIDS.  YOU");
            System.out.println("LOST ALL YOUR FURS.");
            // Clear out all pelts.
            for(int i=0; i<3; i++) {
                Pelt pelt = pelts.get(i);
                pelt.lostPelts();
                pelts.set(i, pelt);
            }
        } else if(tripResult <= 10) {
            // Fox pelts not cured
            // Bug because Fox furs were not calculated above in original basic program
            // Find the Beaver pelt in our Arraylist
            Pelt foxPelt = pelts.get(FOX_ENTRY);
            // Pelts got stolen, so update to a count of zero
            foxPelt.lostPelts();
            // Update it back in the ArrayList
            pelts.set(FOX_ENTRY, foxPelt);
        }

        System.out.println("SUPPLIES AT FORT STADACONA COST $125.00.");
        System.out.println("YOUR TRAVEL EXPENSES TO STADACONA WERE $15.00.");
    }

    private boolean tradeAtFortNewYork() {

        boolean playerAlive = true;
        savings -= 105;
        System.out.println();
        minkPrice = ((.2 * Math.random() + 1.05) * (Math.pow(10, 2) + .5)) / Math.pow(10, 2);
        foxPrice = ((.2 * Math.random() + 1.1) * (Math.pow(10, 2) + .5)) / Math.pow(10, 2);

        // What happened during the trip to the fort?
        int tripResult = (int) (Math.random()*10) + 1;
        if(tripResult <= 2) {
            System.out.println("YOU WERE ATTACKED BY A PARTY OF IROQUOIS.");
            System.out.println("ALL PEOPLE IN YOUR TRADING GROUP WERE");
            System.out.println("KILLED.  THIS ENDS THE GAME.");
            playerAlive = false;
        } else if (tripResult <= 6) {
            System.out.println("YOU WERE LUCKY.  YOU ARRIVED SAFELY");
            System.out.println("AT FORT NEW YORK.");
        } else if (tripResult <= 8) {
            System.out.println("YOU NARROWLY ESCAPED AN IROQUOIS RAIDING PARTY.");
            System.out.println("HOWEVER, YOU HAD TO LEAVE ALL YOUR FURS BEHIND.");
            // Clear out all pelts.
            for(int i=0; i<3; i++) {
                Pelt pelt = pelts.get(i);
                pelt.lostPelts();
                pelts.set(i, pelt);
            }
        } else if(tripResult <= 10) {
            beaverPrice /= 2;
            minkPrice /= 2;
            System.out.println("YOUR MINK AND BEAVER WERE DAMAGED ON YOUR TRIP.");
            System.out.println("YOU RECEIVE ONLY HALF THE CURRENT PRICE FOR THESE FURS.");
        }

        if (playerAlive) {
            System.out.println("SUPPLIES AT NEW YORK COST $80.00.");
            System.out.println("YOUR TRAVEL EXPENSES TO NEW YORK WERE $25.00.");
        }

        return playerAlive;
    }
    /**
     * Return a pelt object containing the user entered number of pelts.
     *
     * @param peltName  Type of pelt
     * @return Pelt object containing name and number of pelts.
     */
    private Pelt getPelt(String peltName) {
        int pelts = displayTextAndGetNumber("HOW MANY " + peltName + " PELTS DO YOU HAVE? ");
        return new Pelt(peltName, pelts);
    }

    private void extendedTradingInfo() {
        System.out.println("YOU MAY TRADE YOUR FURS AT FORT 1, FORT 2,");
        System.out.println("OR FORT 3.  FORT 1 IS FORT HOCHELAGA (MONTREAL)");
        System.out.println("AND IS UNDER THE PROTECTION OF THE FRENCH ARMY.");
        System.out.println("FORT 2 IS FORT STADACONA (QUEBEC) AND IS UNDER THE");
        System.out.println("PROTECTION OF THE FRENCH ARMY.  HOWEVER, YOU MUST");
        System.out.println("MAKE A PORTAGE AND CROSS THE LACHINE RAPIDS.");
        System.out.println("FORT 3 IS FORT NEW YORK AND IS UNDER DUTCH CONTROL.");
        System.out.println("YOU MUST CROSS THROUGH IROQUOIS LAND.");

    }
    private void gameStartupMessage() {
        System.out.println(simulateTabs(31) + "FUR TRADER");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
    }

    private void intro() {
        System.out.println("YOU ARE THE LEADER OF A FRENCH FUR TRADING EXPEDITION IN ");
        System.out.println("1776 LEAVING THE LAKE ONTARIO AREA TO SELL FURS AND GET");
        System.out.println("SUPPLIES FOR THE NEXT YEAR.  YOU HAVE A CHOICE OF THREE");
        System.out.println("FORTS AT WHICH YOU MAY TRADE.  THE COST OF SUPPLIES");
        System.out.println("AND THE AMOUNT YOU RECEIVE FOR YOUR FURS WILL DEPEND");
        System.out.println("ON THE FORT THAT YOU CHOOSE.");
    }

    /*
     * Print a message on the screen, then accept input from Keyboard.
     * Converts input to an Integer
     *
     * @param text message to be displayed on screen.
     * @return what was typed by the player.
     */
    private int displayTextAndGetNumber(String text) {
        return Integer.parseInt(displayTextAndGetInput(text));
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

    /**
     * Checks whether player entered Y or YES to a question.
     *
     * @param text player string from kb
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
     * @param text   source string
     * @param values a range of values to compare against the source string
     * @return true if a comparison was found in one of the variable number of strings passed
     */
    private boolean stringIsAnyValue(String text, String... values) {

        return Arrays.stream(values).anyMatch(str -> str.equalsIgnoreCase(text));
    }

}
