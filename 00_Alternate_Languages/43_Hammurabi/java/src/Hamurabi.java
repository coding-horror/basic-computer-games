import java.util.Arrays;
import java.util.Scanner;

/**
 * Game of Hamurabi
 * <p>
 * Based on the Basic game of Hamurabi here
 * https://github.com/coding-horror/basic-computer-games/blob/main/43%20Hammurabi/hammurabi.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Hamurabi {

    public static final int INITIAL_POPULATION = 95;
    public static final int INITIAL_BUSHELS = 2800;
    public static final int INITIAL_HARVEST = 3000;
    public static final int INITIAL_LAND_TRADING_AT = 3;
    public static final int INITIAL_CAME_TO_CITY = 5;
    public static final int MAX_GAME_YEARS = 10;
    public static final double MAX_STARVATION_IN_A_YEAR = .45d;

    private int year;
    private int population;
    private int acres;
    private int bushels;
    private int harvest;
    private int landTradingAt;
    private int cameToCity;
    private int starvedInAYear;
    private int starvedOverall;
    private boolean chanceOfPlague;
    private int ratsAte;
    private double peopleFed;
    private double percentageStarved;
    private int bushelsToFeedPeople;

    // Used for keyboard input
    private final Scanner kbScanner;

    private enum GAME_STATE {
        STARTUP,
        INIT,
        YEAR_CYCLE,
        BUY_ACRES,
        SELL_ACRES,
        FEED_PEOPLE,
        PLANT_SEED,
        CALCULATE_HARVEST,
        CALCULATE_BABIES,
        RESULTS,
        FINISH_GAME,
        GAME_OVER
    }

    // Current game state
    private GAME_STATE gameState;

    public Hamurabi() {
        kbScanner = new Scanner(System.in);
        gameState = GAME_STATE.STARTUP;
    }

    /**
     * Main game loop
     */
    public void play() {

        do {
            switch (gameState) {

                case STARTUP:
                    intro();
                    gameState = GAME_STATE.INIT;
                    break;

                case INIT:

                    // These are hard coded startup figures from the basic program
                    year = 0;
                    population = INITIAL_POPULATION;
                    bushels = INITIAL_BUSHELS;
                    harvest = INITIAL_HARVEST;
                    landTradingAt = INITIAL_LAND_TRADING_AT;
                    acres = INITIAL_HARVEST / INITIAL_LAND_TRADING_AT;
                    cameToCity = INITIAL_CAME_TO_CITY;
                    starvedInAYear = 0;
                    starvedOverall = 0;
                    chanceOfPlague = false;
                    ratsAte = INITIAL_HARVEST - INITIAL_BUSHELS;
                    peopleFed = 0;
                    percentageStarved = 0;
                    bushelsToFeedPeople = 0;

                    gameState = GAME_STATE.YEAR_CYCLE;
                    break;

                case YEAR_CYCLE:
                    System.out.println();
                    year += 1;
                    // End of game?
                    if (year > MAX_GAME_YEARS) {
                        gameState = GAME_STATE.RESULTS;
                        break;

                    }
                    System.out.println("HAMURABI:  I BEG TO REPORT TO YOU,");
                    System.out.println("IN YEAR " + year + "," + starvedInAYear + " PEOPLE STARVED," + cameToCity + " CAME TO THE CITY,");
                    population += cameToCity;
                    if (chanceOfPlague) {
                        population /= 2;
                        System.out.println("A HORRIBLE PLAGUE STRUCK!  HALF THE PEOPLE DIED.");
                    }
                    System.out.println("POPULATION IS NOW " + population);
                    System.out.println("THE CITY NOW OWNS " + acres + " ACRES.");
                    System.out.println("YOU HARVESTED " + landTradingAt + " BUSHELS PER ACRE.");
                    System.out.println("THE RATS ATE " + ratsAte + " BUSHELS.");
                    System.out.println("YOU NOW HAVE " + bushels + " BUSHELS IN STORE.");
                    System.out.println();

                    landTradingAt = (int) (Math.random() * 10) + 17;  // Original formula unchanged
                    System.out.println("LAND IS TRADING AT " + landTradingAt + " BUSHELS PER ACRE.");

                    gameState = GAME_STATE.BUY_ACRES;
                    break;

                case BUY_ACRES:
                    int acresToBuy = displayTextAndGetNumber("HOW MANY ACRES DO YOU WISH TO BUY? ");
                    if (acresToBuy < 0) {
                        gameState = GAME_STATE.FINISH_GAME;
                    }

                    if (acresToBuy > 0) {
                        if ((landTradingAt * acresToBuy) > bushels) {
                            notEnoughBushelsMessage();
                        } else {
                            acres += acresToBuy;
                            bushels -= (landTradingAt * acresToBuy);
                            peopleFed = 0;
                            gameState = GAME_STATE.FEED_PEOPLE;
                        }
                    } else {
                        // 0 entered as buy so try to sell
                        gameState = GAME_STATE.SELL_ACRES;
                    }
                    break;

                case SELL_ACRES:
                    int acresToSell = displayTextAndGetNumber("HOW MANY ACRES DO YOU WISH TO SELL? ");
                    if (acresToSell < 0) {
                        gameState = GAME_STATE.FINISH_GAME;
                    }
                    if (acresToSell < acres) {
                        acres -= acresToSell;
                        bushels += (landTradingAt * acresToSell);
                        gameState = GAME_STATE.FEED_PEOPLE;
                    } else {
                        notEnoughLandMessage();
                    }
                    break;

                case FEED_PEOPLE:

                    bushelsToFeedPeople = displayTextAndGetNumber("HOW MANY BUSHELS DO YOU WISH TO FEED YOUR PEOPLE ? ");
                    if (bushelsToFeedPeople < 0) {
                        gameState = GAME_STATE.FINISH_GAME;
                    }

                    if (bushelsToFeedPeople <= bushels) {
                        bushels -= bushelsToFeedPeople;
                        peopleFed = 1;
                        gameState = GAME_STATE.PLANT_SEED;
                    } else {
                        notEnoughBushelsMessage();
                    }
                    break;

                case PLANT_SEED:

                    int acresToPlant = displayTextAndGetNumber("HOW MANY ACRES DO YOU WISH TO PLANT WITH SEED ? ");
                    if (acresToPlant < 0) {
                        gameState = GAME_STATE.FINISH_GAME;
                    }

                    if (acresToPlant <= acres) {
                        if (acresToPlant / 2 <= bushels) {
                            if (acresToPlant < 10 * population) {
                                bushels -= acresToPlant / 2;
                                peopleFed = (int) (Math.random() * 5) + 1;
                                landTradingAt = (int) peopleFed;
                                harvest = acresToPlant * landTradingAt;
                                ratsAte = 0;
                                gameState = GAME_STATE.CALCULATE_HARVEST;
                            } else {
                                notEnoughPeopleMessage();
                            }
                        } else {
                            notEnoughBushelsMessage();
                        }
                    } else {
                        notEnoughLandMessage();
                    }
                    break;

                case CALCULATE_HARVEST:

                    if ((int) (peopleFed / 2) == peopleFed / 2) {
                        // Rats are running wild
                        ratsAte = (int) (bushels / peopleFed);
                    }
                    bushels = bushels - ratsAte;
                    bushels += harvest;
                    gameState = GAME_STATE.CALCULATE_BABIES;
                    break;

                case CALCULATE_BABIES:

                    cameToCity = (int) (peopleFed * (20 * acres + bushels) / population / 100 + 1);
                    peopleFed = (bushelsToFeedPeople / 20.0d);
                    // Simplify chance of plague to a true/false
                    chanceOfPlague = (int) ((10 * (Math.random() * 2) - .3)) == 0;
                    if (population < peopleFed) {
                        gameState = GAME_STATE.YEAR_CYCLE;
                    }

                    double starved = population - peopleFed;
                    if (starved < 0.0d) {
                        starvedInAYear = 0;
                        gameState = GAME_STATE.YEAR_CYCLE;
                    } else {
                        starvedInAYear = (int) starved;
                        starvedOverall += starvedInAYear;
                        if (starved > MAX_STARVATION_IN_A_YEAR * population) {
                            starvedTooManyPeopleMessage((int) starved);
                            gameState = GAME_STATE.FINISH_GAME;
                        } else {
                            percentageStarved = ((year - 1) * percentageStarved + starved * 100 / population) / year;
                            population = (int) peopleFed;
                            gameState = GAME_STATE.YEAR_CYCLE;
                        }

                    }

                    break;


                case RESULTS:

                    int acresPerPerson = acres / population;

                    System.out.println("IN YOUR 10-YEAR TERM OF OFFICE," + String.format("%.2f", percentageStarved) + "% PERCENT OF THE");
                    System.out.println("POPULATION STARVED PER YEAR ON THE AVERAGE, I.E. A TOTAL OF");
                    System.out.println(starvedOverall + " PEOPLE DIED!!");
                    System.out.println("YOU STARTED WITH 10 ACRES PER PERSON AND ENDED WITH");
                    System.out.println(acresPerPerson + " ACRES PER PERSON.");
                    System.out.println();

                    if (percentageStarved > 33.0d || acresPerPerson < 7) {
                        starvedTooManyPeopleMessage(starvedOverall);
                    } else if (percentageStarved > 10.0d || acresPerPerson < 9) {
                        heavyHandedMessage();
                    } else if (percentageStarved > 3.0d || acresPerPerson < 10) {
                        couldHaveBeenBetterMessage();
                    } else {
                        fantasticPerformanceMessage();
                    }


                    gameState = GAME_STATE.FINISH_GAME;

                case FINISH_GAME:
                    System.out.println("SO LONG FOR NOW.");
                    gameState = GAME_STATE.GAME_OVER;

            }

        } while (gameState != GAME_STATE.GAME_OVER);
    }

    private void starvedTooManyPeopleMessage(int starved) {
        System.out.println();
        System.out.println("YOU STARVED " + starved + " PEOPLE IN ONE YEAR!!!");
        System.out.println("DUE TO THIS EXTREME MISMANAGEMENT YOU HAVE NOT ONLY");
        System.out.println("BEEN IMPEACHED AND THROWN OUT OF OFFICE BUT YOU HAVE");
        System.out.println("ALSO BEEN DECLARED NATIONAL FINK!!!!");

    }

    private void heavyHandedMessage() {
        System.out.println("DUE TO THIS EXTREME MISMANAGEMENT YOU HAVE NOT ONLY");
        System.out.println("BEEN IMPEACHED AND THROWN OUT OF OFFICE BUT YOU HAVE");
        System.out.println("ALSO BEEN DECLARED NATIONAL FINK!!!!");
    }

    private void couldHaveBeenBetterMessage() {
        System.out.println("YOUR PERFORMANCE COULD HAVE BEEN SOMEWHAT BETTER, BUT");
        System.out.println("REALLY WASN'T TOO BAD AT ALL. " + (int) (Math.random() * (population * .8)) + " PEOPLE");
        System.out.println("WOULD DEARLY LIKE TO SEE YOU ASSASSINATED BUT WE ALL HAVE OUR");
        System.out.println("TRIVIAL PROBLEMS.");
    }

    private void fantasticPerformanceMessage() {
        System.out.println("A FANTASTIC PERFORMANCE!!!  CHARLEMANGE, DISRAELI, AND");
        System.out.println("JEFFERSON COMBINED COULD NOT HAVE DONE BETTER!");
    }

    private void notEnoughPeopleMessage() {
        System.out.println("BUT YOU HAVE ONLY " + population + " PEOPLE TO TEND THE FIELDS!  NOW THEN,");

    }

    private void notEnoughBushelsMessage() {
        System.out.println("HAMURABI:  THINK AGAIN.  YOU HAVE ONLY");
        System.out.println(bushels + " BUSHELS OF GRAIN.  NOW THEN,");
    }

    private void notEnoughLandMessage() {
        System.out.println("HAMURABI:  THINK AGAIN.  YOU OWN ONLY " + acres + " ACRES.  NOW THEN,");
    }


    private void intro() {
        System.out.println(simulateTabs(32) + "HAMURABI");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("TRY YOUR HAND AT GOVERNING ANCIENT SUMERIA");
        System.out.println("FOR A TEN-YEAR TERM OF OFFICE.");
        System.out.println();
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

}
