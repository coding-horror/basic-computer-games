import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Random;
import java.util.Scanner;

/**
 * HORSERACE
 * <p>
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 */
public class Horserace {

    private static final String[] horseNames = {
            "JOE MAW",
            "L.B.J.",
            "MR.WASHBURN",
            "MISS KAREN",
            "JOLLY",
            "HORSE",
            "JELLY DO NOT",
            "MIDNIGHT"
    };
    public static final int MAX_DISTANCE = 28;
    public static final int NUM_HORSES = 8;

    public static void main(String[] args) {
        printHeader();

        Scanner scanner = new Scanner(System.in);
        Random random = new Random();

        printHelp(scanner);

        List<String> betNames = readBetNames(scanner);

        boolean donePlaying = false;
        while (!donePlaying) {

            int[] odds = generateOdds(random);
            int sumOdds = Arrays.stream(odds).sum();
            printOdds(sumOdds, odds);

            Map<String, Bet> bets = takeBets(scanner, betNames);

            var horsePositions = runRace(horseNames.length, sumOdds, odds, random);

            printRaceResults(horsePositions, bets, sumOdds, odds);

            donePlaying = readDonePlaying(scanner);
        }
    }

    private static int[] generateOdds(Random random) {
        int[] odds = new int[NUM_HORSES];
        for (int i = 0; i < NUM_HORSES; i++) {
            odds[i] = (int) (10 * random.nextFloat() + 1);
        }
        return odds;
    }

    private static void printOdds(int R, int[] D) {
        System.out.printf("%n%-28s%-14s%-14s%n%n", "HORSE", "NUMBER", "ODDS");
        for (int n = 0; n < horseNames.length; n++) {
            System.out.printf("%-28s% -14d%.6f :1%n", horseNames[n], n + 1, ((float) R / D[n]));
        }
    }

    private static boolean readDonePlaying(Scanner scan) {
        System.out.println("DO YOU WANT TO BET ON THE NEXT RACE ?");
        System.out.print("YES OR NO? ");
        String choice = scan.nextLine();
        return !choice.equalsIgnoreCase("YES");
    }

    /**
     * Simulate the race run, returning the final positions of the horses.
     */
    private static int[] runRace(int numberOfHorses, int sumOdds, int[] odds, Random random) {
        int[] positionChange = new int[numberOfHorses];

        System.out.println();
        System.out.println("1 2 3 4 5 6 7 8");

        int totalDistance = 0;
        int[] currentPositions = new int[NUM_HORSES];
        int[] horsePositions = new int[NUM_HORSES];

        while (totalDistance < MAX_DISTANCE) {
            System.out.println("XXXXSTARTXXXX");

            for (int i = 0; i < numberOfHorses; i++) {
                horsePositions[i] = i + 1;
                positionChange[i] = calculatePositionChanges(sumOdds, odds[i], random);
                currentPositions[i] += positionChange[i];
            }

            sortHorsePositionsBasedOnCurrent(currentPositions, horsePositions);

            totalDistance = currentPositions[horsePositions[7] - 1];

            boolean raceFinished = false;
            int i = 0;
            while (i < NUM_HORSES && !raceFinished) {
                int distanceToNextHorse = currentPositions[(horsePositions[i] - 1)] - (i < 1 ? 0 : currentPositions[(horsePositions[i - 1] - 1)]);
                if (distanceToNextHorse != 0) {
                    int a = 0;
                    while (a < distanceToNextHorse && !raceFinished) {
                        System.out.println();
                        if (currentPositions[horsePositions[i] - 1] >= MAX_DISTANCE) {
                            raceFinished = true;
                        }
                        a++;
                    }
                }

                if (!raceFinished) {
                    System.out.print(" " + horsePositions[i] + " "); // Print horse number
                }
                i++;
            }

            if (!raceFinished) {
                //Print additional empty lines
                for (int a = 0; a < MAX_DISTANCE - totalDistance; a++) {
                    System.out.println();
                }
            }

            System.out.println("XXXXFINISHXXXX");
            System.out.println("\n");
            System.out.println("---------------------------------------------");
            System.out.println("\n");
        }

        return horsePositions;
    }

    /**
     * Sorts the horsePositions array in place, based on the currentPositions of the horses.
     * (bubble sort)
     */
    private static void sortHorsePositionsBasedOnCurrent(int[] currentPositions, int[] horsePositions) {
        for (int l = 0; l < NUM_HORSES; l++) {
            int i = 0;
            /*
            uses a do-while instead of a for loop here, because in BASIC
            a FOR I=1 TO 0 causes at least one execution of the loop
            */
            do {
                if (currentPositions[horsePositions[i] - 1] >= currentPositions[horsePositions[i + 1] - 1]) {
                    int h = horsePositions[i];
                    horsePositions[i] = horsePositions[i + 1];
                    horsePositions[i + 1] = h;
                }
                i++;
            } while (i < (7 - l));
        }
    }

    private static int calculatePositionChanges(int r, int d, Random random) {
        int positionChange = (int) (100 * random.nextFloat() + 1);

        if (positionChange < 10) {
            positionChange = 1;
        } else {
            int s = (int) ((float) r / d + 0.5);
            if (positionChange < (s + 17)) {
                positionChange = 2;
            } else if (positionChange < s + 37) {
                positionChange = 3;
            } else if (positionChange < s + 57) {
                positionChange = 4;
            } else if (positionChange < s + 77) {
                positionChange = 5;
            } else if (positionChange < s + 92) {
                positionChange = 6;
            } else {
                positionChange = 7;
            }
        }

        return positionChange;
    }

    private static void printRaceResults(int[] m, Map<String, Bet> bets, int r, int[] d) {
        System.out.println("THE RACE RESULTS ARE:");
        int z9 = 1;
        for (int i = 7; i >= 0; i--) {
            int f = m[i];
            System.out.println();
            System.out.println(z9 + " PLACE HORSE NO. " + f + " AT " + (r / d[f - 1]) + ":1");
            z9++;
        }
        bets.forEach((betName, bet) -> {
            if (bet.horseNumber == m[7]) {
                int n = bet.horseNumber;
                System.out.println();
                System.out.printf("%s WINS $ %.2f %n", bet.betName, ((float) r / d[n]) * bet.amount);
            }
        });
    }

    private static Map<String, Bet> takeBets(Scanner scanner, List<String> betNames) {
        Map<String, Bet> bets = new HashMap<>();
        System.out.println("--------------------------------------------------");
        System.out.println("PLACE YOUR BETS...HORSE # THEN AMOUNT");
        for (String betName : betNames) {
            boolean validInput = false;
            while (!validInput) {
                int horseNumber = readInt(betName, scanner);//Q in the original
                double betAmount = readDouble("?", scanner); //P in the original
                if (betAmount < 1 || betAmount > 100000) {
                    System.out.println("  YOU CAN'T DO THAT!");
                } else {
                    bets.put(betName, new Bet(betName, horseNumber, betAmount));
                    validInput = true;
                }
            }
        }

        return bets;
    }

    private static void printHelp(Scanner scanner) {
        System.out.print("DO YOU WANT DIRECTIONS");

        String directions = readChoice(scanner);

        if (!directions.equalsIgnoreCase("NO")) {
            System.out.println("UP TO 10 MAY PLAY.  A TABLE OF ODDS WILL BE PRINTED.  YOU");
            System.out.println("MAY BET ANY + AMOUNT UNDER 100000 ON ONE HORSE.");
            System.out.println("DURING THE RACE, A HORSE WILL BE SHOWN BY ITS");
            System.out.println("NUMBER.  THE HORSES RACE DOWN THE PAPER!");
            System.out.println();
        }
    }

    private static String readChoice(Scanner scanner) {
        System.out.print("? ");
        return scanner.nextLine();
    }

    private static int readInt(String prompt, Scanner scanner) {
        System.out.print(prompt);
        while (true) {
            System.out.print("? ");
            String input = scanner.nextLine();
            try {
                return Integer.parseInt(input);
            } catch (NumberFormatException e) {
                System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE");
            }
        }
    }

    private static double readDouble(String prompt, Scanner scanner) {
        System.out.print(prompt);
        while (true) {
            System.out.print("? ");
            String input = scanner.nextLine();
            try {
                return Double.parseDouble(input);
            } catch (NumberFormatException e) {
                System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE");
            }
        }
    }

    private static List<String> readBetNames(Scanner scanner) {
        int c = readInt("HOW MANY WANT TO BET ", scanner);
        System.out.println("WHEN ? APPEARS,TYPE NAME");
        List<String> names = new ArrayList<>();
        for (int i = 1; i <= c; i++) {
            System.out.print("? ");
            names.add(scanner.nextLine());
        }

        return names;
    }

    private static void printHeader() {
        System.out.println("                                               HORSERACE");
        System.out.println("                            CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
        System.out.println("\n\n");
        System.out.println("WELCOME TO SOUTH PORTLAND HIGH RACETRACK");
        System.out.println("                      ...OWNED BY LAURIE CHEVALIER");
    }

    private static class Bet {
        String betName;
        int horseNumber;
        double amount;

        public Bet(String betName, int horseNumber, double amount) {
            this.betName = betName;
            this.horseNumber = horseNumber;
            this.amount = amount;
        }

        @Override
        public String toString() {
            return "Bet{" +
                    "betName='" + betName + '\'' +
                    ", horseNumber=" + horseNumber +
                    ", amount=" + amount +
                    '}';
        }
    }
}

