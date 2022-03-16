import java.util.Arrays;
import java.util.InputMismatchException;
import java.util.Random;
import java.util.Scanner;

/**
 * Slalom
 * <p>
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 *
 * There is a bug in the original version where the data pointer doesn't reset after a race is completed. This causes subsequent races to error at
 * some future point on line "540    READ Q"
 */
public class Slalom {

    private static final int MAX_NUM_GATES = 25;
    private static final int[] MAX_SPEED = {
            14, 18, 26, 29, 18,
            25, 28, 32, 29, 20,
            29, 29, 25, 21, 26,
            29, 20, 21, 20, 18,
            26, 25, 33, 31, 22
    };

    public static void main(String[] args) {
        var random = new Random();

        printIntro();
        Scanner scanner = new Scanner(System.in);

        int numGates = readNumberOfGatesChoice(scanner);

        printMenu();
        MenuChoice menuChoice;
        do {
            menuChoice = readMenuOption(scanner);
            switch (menuChoice) {
                case INS:
                    printInstructions();
                    break;
                case MAX:
                    printApproxMaxSpeeds(numGates);
                    break;
                case RUN:
                    run(numGates, scanner, random);
                    break;
            }
        } while (menuChoice != MenuChoice.RUN);
    }

    private static void run(int numGates, Scanner scan, Random random) {
        int rating = readSkierRating(scan);
        boolean gameInProgress = true;
        var medals = new Medals(0, 0, 0);

        while (gameInProgress) {
            System.out.println("THE STARTER COUNTS DOWN...5...4...3...2...1...GO!");
            System.out.println("YOU'RE OFF!");

            int speed = random.nextInt(18 - 9) + 9;

            float totalTimeTaken = 0;
            try {
                totalTimeTaken = runThroughGates(numGates, scan, random, speed);
                System.out.printf("%nYOU TOOK %.2f SECONDS.%n", totalTimeTaken + random.nextFloat());

                medals = evaluateAndUpdateMedals(totalTimeTaken, numGates, rating, medals);
            } catch (WipedOutOrSnaggedAFlag | DisqualifiedException e) {
                //end of this race! Print time taken and stop
                System.out.printf("%nYOU TOOK %.2f SECONDS.%n", totalTimeTaken + random.nextFloat());
            }

            gameInProgress = readRaceAgainChoice(scan);
        }

        System.out.println("THANKS FOR THE RACE");
        if (medals.getGold() >= 1) System.out.printf("GOLD MEDALS: %d%n", medals.getGold());
        if (medals.getSilver() >= 1) System.out.printf("SILVER MEDALS: %d%n", medals.getSilver());
        if (medals.getBronze() >= 1) System.out.printf("BRONZE MEDALS: %d%n", medals.getBronze());
    }

    private static Medals evaluateAndUpdateMedals(float totalTimeTaken, int numGates, int rating,
                                                  Medals medals) {
        var m = totalTimeTaken;
        m = m / numGates;
        int goldMedals = medals.getGold();
        int silverMedals = medals.getSilver();
        int bronzeMedals = medals.getBronze();
        if (m < 1.5 - (rating * 0.1)) {
            System.out.println("YOU WON A GOLD MEDAL!");
            goldMedals++;
        } else if (m < 2.9 - rating * 0.1) {
            System.out.println("YOU WON A SILVER MEDAL");
            silverMedals++;
        } else if (m < 4.4 - rating * 0.01) {
            System.out.println("YOU WON A BRONZE MEDAL");
            bronzeMedals++;
        }
        return new Medals(goldMedals, silverMedals, bronzeMedals);
    }

    /**
     * @return the total time taken through all the gates.
     */
    private static float runThroughGates(int numGates, Scanner scan, Random random, int speed) throws DisqualifiedException, WipedOutOrSnaggedAFlag {
        float totalTimeTaken = 0.0f;
        for (int i = 0; i < numGates; i++) {
            var gateNum = i + 1;
            boolean stillInRace = true;
            boolean gateCompleted = false;
            while (!gateCompleted) {
                System.out.printf("%nHERE COMES GATE # %d:%n", gateNum);
                printSpeed(speed);

                var tmpSpeed = speed;

                int chosenOption = readOption(scan);
                switch (chosenOption) {
                    case 0:
                        //how long
                        printHowLong(totalTimeTaken, random);
                        break;
                    case 1:
                        //speed up a lot
                        speed = speed + random.nextInt(10 - 5) + 5;
                        break;
                    case 2:
                        //speed up a little
                        speed = speed + random.nextInt(5 - 3) + 3;
                        break;
                    case 3:
                        //speed up a teensy
                        speed = speed + random.nextInt(4 - 1) + 1;
                        break;
                    case 4:
                        //keep going at the same speed
                        break;
                    case 5:
                        //check a teensy
                        speed = speed - random.nextInt(4 - 1) + 1;
                        break;
                    case 6:
                        //check a little
                        speed = speed - random.nextInt(5 - 3) + 3;
                        break;
                    case 7:
                        //check a lot
                        speed = speed - random.nextInt(10 - 5) + 5;
                        break;
                    case 8:
                        //cheat
                        System.out.println("***CHEAT");
                        if (random.nextFloat() < 0.7) {
                            System.out.println("AN OFFICIAL CAUGHT YOU!");
                            stillInRace = false;
                        } else {
                            System.out.println("YOU MADE IT!");
                            totalTimeTaken = totalTimeTaken + 1.5f;
                        }
                        break;
                }

                if (stillInRace) {
                    printSpeed(speed);
                    stillInRace = checkAndProcessIfOverMaxSpeed(random, speed, MAX_SPEED[i]);
                    if (!stillInRace) throw new WipedOutOrSnaggedAFlag();
                } else {
                    throw new DisqualifiedException();//we've been dis-qualified
                }

                if (speed < 7) {
                    System.out.println("LET'S BE REALISTIC, OK?  LET'S GO BACK AND TRY AGAIN...");
                    speed = tmpSpeed;
                    gateCompleted = false;
                } else {
                    totalTimeTaken = totalTimeTaken + (MAX_SPEED[i] - speed + 1);
                    if (speed > MAX_SPEED[i]) {
                        totalTimeTaken = totalTimeTaken + 0.5f;
                    }
                    gateCompleted = true;
                }
            }

        }
        return totalTimeTaken;
    }

    private static boolean checkAndProcessIfOverMaxSpeed(Random random, int speed, int maxSpeed) {
        boolean stillInRace = true;
        if (speed > maxSpeed) {
            if (random.nextFloat() >= (speed - maxSpeed) * 0.1 + 0.2) {
                System.out.println("YOU WENT OVER THE MAXIMUM SPEED AND MADE IT!");
            } else {
                System.out.print("YOU WENT OVER THE MAXIMUM SPEED AND ");
                if (random.nextBoolean()) {
                    System.out.println("WIPED OUT!");
                } else {
                    System.out.println("SNAGGED A FLAG!");
                }
                stillInRace = false;
            }
        } else if (speed > maxSpeed - 1) {
            System.out.println("CLOSE ONE!");
        }
        return stillInRace;
    }

    private static boolean readRaceAgainChoice(Scanner scan) {
        System.out.print("\nDO YOU WANT TO RACE AGAIN? ");
        String raceAgain = "";
        final String YES = "YES";
        final String NO = "NO";
        while (!YES.equals(raceAgain) && !NO.equals(raceAgain)) {
            raceAgain = scan.nextLine();
            if (!(YES.equals(raceAgain) || NO.equals(raceAgain))) {
                System.out.println("PLEASE TYPE 'YES' OR 'NO'");
            }
        }
        return raceAgain.equals(YES);
    }

    private static void printSpeed(int speed) {
        System.out.printf("%3d M.P.H.%n", speed);
    }

    private static void printHowLong(float t, Random random) {
        System.out.printf("YOU'VE TAKEN %.2f SECONDS.%n", t + random.nextFloat());
    }

    private static int readOption(Scanner scan) {
        Integer option = null;

        while (option == null) {
            System.out.print("OPTION? ");
            try {
                option = scan.nextInt();
            } catch (InputMismatchException ex) {
                System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE\n");
            }
            scan.nextLine();
            if (option != null && (option > 8 || option < 0)) {
                System.out.println("WHAT?");
                option = null;
            }
        }
        return option;
    }

    private static int readSkierRating(Scanner scan) {
        int rating = 0;

        while (rating < 1 || rating > 3) {
            System.out.print("RATE YOURSELF AS A SKIER, (1=WORST, 3=BEST)? ");
            try {
                rating = scan.nextInt();
                if (rating < 1 || rating > 3) {
                    System.out.println("THE BOUNDS ARE 1-3");
                }
            } catch (InputMismatchException ex) {
                System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE\n");
            }
            scan.nextLine();
        }
        return rating;
    }

    private static void printApproxMaxSpeeds(int numGates) {
        System.out.println("GATE MAX");
        System.out.println(" #  M.P.H.");
        System.out.println("---------");
        for (int i = 0; i < numGates; i++) {
            System.out.println((i+1) + "  " + MAX_SPEED[i]);
        }
    }

    private static void printInstructions() {
        System.out.println("\n*** SLALOM: THIS IS THE 1976 WINTER OLYMPIC GIANT SLALOM.  YOU ARE");
        System.out.println("            THE AMERICAN TEAM'S ONLY HOPE OF A GOLD MEDAL.");
        System.out.println();
        System.out.println("     0 -- TYPE THIS IS YOU WANT TO SEE HOW LONG YOU'VE TAKEN.");
        System.out.println("     1 -- TYPE THIS IF YOU WANT TO SPEED UP A LOT.");
        System.out.println("     2 -- TYPE THIS IF YOU WANT TO SPEED UP A LITTLE.");
        System.out.println("     3 -- TYPE THIS IF YOU WANT TO SPEED UP A TEENSY.");
        System.out.println("     4 -- TYPE THIS IF YOU WANT TO KEEP GOING THE SAME SPEED.");
        System.out.println("     5 -- TYPE THIS IF YOU WANT TO CHECK A TEENSY.");
        System.out.println("     6 -- TYPE THIS IF YOU WANT TO CHECK A LITTLE.");
        System.out.println("     7 -- TYPE THIS IF YOU WANT TO CHECK A LOT.");
        System.out.println("     8 -- TYPE THIS IF YOU WANT TO CHEAT AND TRY TO SKIP A GATE.");
        System.out.println();
        System.out.println(" THE PLACE TO USE THESE OPTIONS IS WHEN THE COMPUTER ASKS:");
        System.out.println();
        System.out.println("OPTION?");
        System.out.println();
        System.out.println("                GOOD LUCK!");
    }

    private static MenuChoice readMenuOption(Scanner scan) {
        System.out.print("COMMAND--? ");
        MenuChoice menuChoice = null;

        while (menuChoice == null) {
            String choice = scan.next();
            if (Arrays.stream(MenuChoice.values()).anyMatch(a -> a.name().equals(choice))) {
                menuChoice = MenuChoice.valueOf(choice);
            } else {
                System.out.print("\""+ choice + "\" IS AN ILLEGAL COMMAND--RETRY? ");
            }
            scan.nextLine();
        }
        return menuChoice;
    }

    private static void printMenu() {
        System.out.println("TYPE INS FOR INSTRUCTIONS");
        System.out.println("TYPE MAX FOR APPROXIMATE MAXIMUM SPEEDS");
        System.out.println("TYPE RUN FOR THE BEGINNING OF THE RACE");
    }

    private static int readNumberOfGatesChoice(Scanner scan) {
        int numGates = 0;
        while (numGates < 1) {
            System.out.print("HOW MANY GATES DOES THIS COURSE HAVE (1 TO 25)? ");
            numGates = scan.nextInt();
            if (numGates > MAX_NUM_GATES) {
                System.out.println(MAX_NUM_GATES + " IS THE LIMIT.");
                numGates = MAX_NUM_GATES;
            }
        }
        return numGates;
    }

    private static void printIntro() {
        System.out.println("                                SLALOM");
        System.out.println("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println("\n\n");
    }

    private enum MenuChoice {
        INS, MAX, RUN
    }

    private static class DisqualifiedException extends Exception {
    }

    private static class WipedOutOrSnaggedAFlag extends Exception {
    }

    private static class Medals {
        private int gold = 0;
        private int silver = 0;
        private int bronze = 0;

        public Medals(int gold, int silver, int bronze) {
            this.gold = gold;
            this.silver = silver;
            this.bronze = bronze;
        }

        public int getGold() {
            return gold;
        }

        public int getSilver() {
            return silver;
        }

        public int getBronze() {
            return bronze;
        }
    }


}
