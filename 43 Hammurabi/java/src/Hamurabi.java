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

    private int year;
    private int population;
    private int acres;
    private int bushels;
    private int harvest;
    private int landTradingAt;
    private int cameToCity;
    private int died;
    private int q;
    private int ratsAte;
    private double peopleFed;
    private double totalPlanted;
    private double percentageStarved;
    private int acresToPlant;
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
                    population = 95;
                    bushels = 2800;
                    harvest = 3000;
                    landTradingAt = 3;
                    acres = harvest / landTradingAt;
                    cameToCity = 5;
                    died = 0;
                    q = 1;
                    ratsAte = harvest - bushels;
                    peopleFed = 0;
                    totalPlanted = 0;
                    percentageStarved = 0;
                    acresToPlant = 0;
                    bushelsToFeedPeople = 0;

                    gameState = GAME_STATE.YEAR_CYCLE;
                    break;

                case YEAR_CYCLE:
                    System.out.println();
                    year += 1;
                    System.out.println("HAMURABI:  I BEG TO REPORT TO YOU,");
                    System.out.println("IN YEAR " + year + "," + died + " PEOPLE STARVED," + cameToCity + " CAME TO THE CITY,");
                    population += cameToCity;
                    if (q == 0) {
                        population /= 2;
                        System.out.println("A HORRIBLE PLAGUE STRUCK!  HALF THE PEOPLE DIED.");
                    }
                    System.out.println("POPULATION IS NOW " + population);
                    System.out.println("THE CITY NOW OWNS " + acres + " ACRES.");
                    System.out.println("YOU HARVESTED " + landTradingAt + " BUSHELS PER ACRE.");
                    System.out.println("THE RATS ATE " + ratsAte + " BUSHELS.");
                    System.out.println("YOU NOW HAVE " + bushels + " BUSHELS IN STORE.");
                    System.out.println();

                    peopleFed = (int) (Math.random() * 10);
                    landTradingAt = (int) peopleFed + 17;
                    System.out.println("LAND IS TRADING AT " + landTradingAt + " BUSHELS PER ACRE.");

                    gameState = GAME_STATE.BUY_ACRES;
                    break;

                case BUY_ACRES:
                        int acresToBuy = displayTextAndGetNumber("HOW MANY ACRES DO YOU WISH TO BUY? ");
                        if (acresToBuy < 0) {
                            gameState = GAME_STATE.GAME_OVER;
                        }

                        if(acresToBuy >0) {
                            if ((landTradingAt * acresToBuy) > bushels) {
                                System.out.println("HAMURABI:  THINK AGAIN.  YOU HAVE ONLY");
                                System.out.println(bushels + " BUSHELS OF GRAIN.  NOW THEN,");
                            } else {
                                if (q == 0) {
                                    gameState = GAME_STATE.SELL_ACRES;
                                } else {
                                    acres += acresToBuy;
                                    bushels -= (landTradingAt * acresToBuy);
                                    peopleFed = 0;
                                    gameState = GAME_STATE.FEED_PEOPLE;
                                }
                            }
                        } else {
                            // 0 entered as buy so try to sell
                            gameState = GAME_STATE.SELL_ACRES;
                        }
                        break;

                case SELL_ACRES:
                        int acresToSell = displayTextAndGetNumber("HOW MANY ACRES DO YOU WISH TO SELL? ");
                        if (acresToSell < 0) {
                            gameState = GAME_STATE.GAME_OVER;
                        }
                        if (acresToSell < acres) {
                            acres -= acresToSell;
                            bushels += (landTradingAt * acresToSell);
                            gameState = GAME_STATE.FEED_PEOPLE;
                        } else {
                            System.out.println("HAMURABI:  THINK AGAIN.  YOU OWN ONLY " + acres + " ACRES.  NOW THEN,");
                        }
                        break;

                case FEED_PEOPLE:

                        bushelsToFeedPeople = displayTextAndGetNumber("HOW MANY BUSHELS DO YOU WISH TO FEED YOUR PEOPLE ? ");
                        if (bushelsToFeedPeople < 0) {
                            gameState = GAME_STATE.GAME_OVER;
                        }

                        if (bushelsToFeedPeople <= bushels) {
                            bushels -= bushelsToFeedPeople;
                            peopleFed = 1;
                            gameState = GAME_STATE.PLANT_SEED;
                        } else {
                            System.out.println("HAMURABI:  THINK AGAIN.  YOU HAVE ONLY");
                            System.out.println(bushels + " BUSHELS OF GRAIN.  NOW THEN,");
                        }
                        break;

                case PLANT_SEED:

                    acresToPlant = displayTextAndGetNumber("HOW MANY ACRES DO YOU WISH TO PLANT WITH SEED ? ");
                    if (acresToPlant < 0) {
                        gameState = GAME_STATE.GAME_OVER;
                    }

                    if (acresToPlant <= acres) {
                        if(acresToPlant/2 <= bushels) {
                            if(acresToPlant < 10*population) {
                                bushels -= acresToPlant/2;
                                peopleFed = (int) (Math.random()*5)+1;
                                landTradingAt = (int) peopleFed;
                                harvest = acresToPlant * landTradingAt;
                                ratsAte = 0;
                                gameState = GAME_STATE.CALCULATE_HARVEST;
                            } else {
                                System.out.println("BUT YOU HAVE ONLY " + population + " PEOPLE TO TEND THE FIELDS!  NOW THEN,");
                            }
                        } else {
                            System.out.println("HAMURABI:  THINK AGAIN.  YOU HAVE ONLY");
                            System.out.println("BUSHELS OF GRAIN.  NOW THEN,");
                        }
                    } else {
                        System.out.println("HAMURABI:  THINK AGAIN.  YOU OWN ONLY " + acres + " ACRES.  NOW THEN,");
                    }
                    break;

                case CALCULATE_HARVEST:

                    if( (int) (peopleFed/2) == peopleFed/2) {
                        // Rats are running wild
                            ratsAte = (int) (bushels / peopleFed);
                    }
                    bushels = bushels - ratsAte;
                    bushels += harvest;
                    gameState = GAME_STATE.CALCULATE_BABIES;
                    break;

                case CALCULATE_BABIES:

                    cameToCity = (int) (peopleFed*(20*acres+bushels)/population/100+1);
                    peopleFed = (int) (bushelsToFeedPeople/20);
                    q = (int) ((10*(Math.random()*2)-.3));
                    if(population <peopleFed) {
                        gameState = GAME_STATE.YEAR_CYCLE;
                    }

                    double starved = population - peopleFed;
                    if(starved>.45*population) {
                        System.out.println();
                        System.out.println("YOU STARVED " + (int) starved + " PEOPLE IN ONE YEAR!!!");
                        System.out.println("DUE TO THIS EXTREME MISMANAGEMENT YOU HAVE NOT ONLY");
                        System.out.println("BEEN IMPEACHED AND THROWN OUT OF OFFICE BUT YOU HAVE");
                        System.out.println("ALSO BEEN DECLARED NATIONAL FINK!!!!");
                        gameState = GAME_STATE.GAME_OVER;
                    } else {
                        percentageStarved = ((year-1)*percentageStarved+(double) (acresToPlant*100/population))/year;
                        population = (int) peopleFed;
                        totalPlanted  += acresToPlant;
                        gameState = GAME_STATE.YEAR_CYCLE;
                    }

                    break;







            }

        } while (gameState != GAME_STATE.GAME_OVER);
    }

    private void intro() {
        System.out.println(simulateTabs(32) + "HAMURABI");
        System.out.println(simulateTabs(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("TRY YOUR HAND AT GOVERNING ANCIENT SUMERIA");
        System.out.println("FOR A TEN-YEAR TERM OF OFFICE.");
        System.out.println();
    }

    private boolean calculate(double playerAnswer, double correctAnswer) {

        boolean gotItRight = false;

        if (Math.abs((playerAnswer - correctAnswer) / correctAnswer) < 0.15) {
            System.out.println("CLOSE ENOUGH");
            gotItRight = true;
        } else {
            System.out.println("NOT EVEN CLOSE");
        }
        System.out.println("CORRECT ANSWER IS " + correctAnswer);
        System.out.println();

        return gotItRight;
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
