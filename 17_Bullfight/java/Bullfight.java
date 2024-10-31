import java.util.Random;
import java.util.Scanner;

/**
 * BULLFIGHT
 * <p>
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 */
public class Bullfight {

    private static final int MAX_PASSES_BEFORE_CHARGE = 3;

    public static void main(String[] args) {
        printHeader();

        Scanner scanner = new Scanner(System.in);
        System.out.print("\n\n");

        var instructionsChoice = readInstructionsChoice(scanner);
        if (instructionsChoice) {
            printInstructions();
        }

        Random random = new Random();

        //initialize the game with default values
        GameState gameState = new GameState();

        //Randomly select a bull grade
        int randomGrade = (int) (random.nextFloat() * BullGrade.values().length + 1);
        var bullGrade = BullGrade.fromValue(randomGrade);

        printBullGradeInfo(bullGrade);

        System.out.println();

        //D[1] in the original
        gameState.picadoresDamage = firstStage("PICADO", "RES", bullGrade, random);

        //D[2] in the original
        gameState.toreadoresDamage = firstStage("TOREAD", "ORES", bullGrade, random);

        boolean done = false; //controls the main game loop

        while (!done) {

            gameState.passNumber++;
            System.out.printf("\n\nPASS NUMBER %d \n", gameState.passNumber);

            if (gameState.passNumber < MAX_PASSES_BEFORE_CHARGE) {
                System.out.println("THE BULL IS CHARGING AT YOU!  YOU ARE THE MATADOR--");
                System.out.print("DO YOU WANT TO KILL THE BULL? ");
            } else {
                System.out.print("HERE COMES THE BULL.  TRY FOR A KILL? ");
            }

            BooleanAnswer yesOrNo = readYesOrNo(scanner);
            if (yesOrNo.equals(BooleanAnswer.YES)) {
                gameState = attemptKillBull(bullGrade, random, gameState, scanner);
                done = true;
            } else {
                int capeMove;
                if (gameState.passNumber < MAX_PASSES_BEFORE_CHARGE) {
                    capeMove = readCapeMove("WHAT MOVE DO YOU MAKE WITH THE CAPE", scanner);
                } else {
                    capeMove = readCapeMove("CAPE MOVE", scanner);
                }

                //handle cape move
                gameState = handleCapeMove(capeMove, random, scanner, bullGrade, gameState);

                if (gameState.matadorStatus.equals(MatadorStatus.DEFEATED) || gameState.matadorStatus.equals(MatadorStatus.DEAD)) {
                    done = true;
                }
            }

        }

        crowdReaction(gameState, bullGrade, random);

        System.out.println("\nADIOS\n\n");
    }

    private static void printBullGradeInfo(BullGrade bullGrade) {
        System.out.println("\n\nYOU HAVE DRAWN A " + bullGrade.name() + " BULL.");
        if (bullGrade.equals(BullGrade.AWFUL)) {
            System.out.println("YOU'RE LUCKY.");
        } else if (bullGrade.equals(BullGrade.SUPERB)) {
            System.out.println("GOOD LUCK.  YOU'LL NEED IT.");
        }
    }

    private static void crowdReaction(GameState gameState, BullGrade bullGrade, Random random) {
        System.out.println("\n\n");
        if (!gameState.matadorStatus.equals(MatadorStatus.DEFEATED)) {
            if (!gameState.matadorStatus.equals(MatadorStatus.INJURED)) {
                if (gameState.bullStatus.equals(BullStatus.DEAD)) {
                    System.out.println("THE CROWD CHEERS!");
                }
            } else {
                System.out.println("THE CROWD CHEERS WILDLY!");
            }

            System.out.println("\nTHE CROWD AWARDS YOU");
            var crowdReactionScore = calculateCrowdReactionScore(gameState, bullGrade, random);
            if (crowdReactionScore < 2.4) {
                System.out.println("NOTHING AT ALL.");
            } else if (crowdReactionScore < 4.9) {
                System.out.println("ONE EAR OF THE BULL.");
            } else if (crowdReactionScore < 7.4) {
                System.out.println("BOTH EARS OF THE BULL!");
                System.out.println("OLE!");
            } else {
                System.out.println("OLE!  YOU ARE 'MUY HOMBRE'!! OLE!  OLE!");
            }

        } else {
            System.out.println("THE CROWD BOOS FOR TEN MINUTES.  IF YOU EVER DARE TO SHOW");
            System.out.println("YOUR FACE IN A RING AGAIN, THEY SWEAR THEY WILL KILL YOU--");
            System.out.println("UNLESS THE BULL DOES FIRST.");
        }
    }

    private static GameState handleCapeMove(int capeMove, Random random, Scanner scanner, BullGrade bullGrade, GameState gameState) {
        double m;
        if (capeMove == 0) {
            m = 3;
        } else if (capeMove == 1) {
            m = 2;
        } else {
            //capeMove == 2
            m = 0.5;
        }
        gameState.capeMovesCumulative += m;

        double f = (6 - bullGrade.getValue() + m / 10) * random.nextFloat() / ((gameState.picadoresDamage + gameState.toreadoresDamage + gameState.passNumber / 10d) * 5);
        if (f >= 0.51) {
            System.out.println("THE BULL HAS GORED YOU!");
            gameState = stateAfterGoring(random, scanner, gameState, bullGrade);
        }
        return gameState;
    }

    private static GameState stateAfterGoring(Random random, Scanner scanner, GameState gameState, BullGrade bullGrade) {
        GameState newGameState = gameState.newCopy();
        if (random.nextBoolean()) {
            System.out.println("YOU ARE DEAD.");
            newGameState.matadorStatus = MatadorStatus.DEAD;
        } else {
            System.out.println("YOU ARE STILL ALIVE.");
            newGameState = readAndHandleForfeitDecision(random, scanner, newGameState, bullGrade);
        }
        return newGameState;
    }

    /**
     * Calculate the crowd's reaction score based on the game state plus some randomness.
     * (FNC in the original code on line 1390)
     */
    private static double calculateCrowdReactionScore(GameState gameState, BullGrade bullGrade, Random random) {
        return calculateGameScore(gameState, bullGrade) * random.nextFloat();
    }

    /**
     * Calculates the ame score based on the current state and the selected bull grade.
     * (FND in the original code on line 1395)
     */
    private static double calculateGameScore(GameState gameState, BullGrade bullGrade) {
        return (4.5 + gameState.capeMovesCumulative / 6 - (gameState.picadoresDamage + gameState.toreadoresDamage) * 2.5 + 4 * gameState.matadorStatus.getValue() + 2 * gameState.bullStatus.getValue() - Math.pow(gameState.passNumber, 2) / 120f - bullGrade.getValue());
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

    private static int readCapeMove(String initialPrompt, Scanner scanner) {
        String prompt = initialPrompt;
        while (true) {
            int capeMove = readInt(prompt, scanner);
            if (capeMove <= 0 || capeMove > 3) {
                System.out.println("DON'T PANIC, YOU IDIOT!  PUT DOWN A CORRECT NUMBER");
                prompt = "";
            } else {
                return capeMove;
            }
        }
    }

    private static BooleanAnswer readYesOrNo(Scanner scanner) {
        while (true) {
            String answer = scanner.nextLine();
            if (answer.equalsIgnoreCase("YES")) {
                return BooleanAnswer.YES;
            } else if (answer.equalsIgnoreCase("NO")) {
                return BooleanAnswer.NO;
            } else {
                System.out.println("INCORRECT ANSWER - - PLEASE TYPE 'YES' OR 'NO'.");
            }
        }
    }

    private static void printInstructions() {
        System.out.print("HELLO, ALL YOU BLOODLOVERS AND AFICIONADOS.\n");
        System.out.print("HERE IS YOUR BIG CHANCE TO KILL A BULL.\n\n");
        System.out.print("ON EACH PASS OF THE BULL, YOU MAY TRY\n");
        System.out.print("0 - VERONICA (DANGEROUS INSIDE MOVE OF THE CAPE)\n");
        System.out.print("1 - LESS DANGEROUS OUTSIDE MOVE OF THE CAPE\n");
        System.out.print("2 - ORDINARY SWIRL OF THE CAPE.\n\n");
        System.out.print("INSTEAD OF THE ABOVE, YOU MAY TRY TO KILL THE BULL\n");
        System.out.print("ON ANY TURN: 4 (OVER THE HORNS), 5 (IN THE CHEST).\n");
        System.out.print("BUT IF I WERE YOU,\n");
        System.out.print("I WOULDN'T TRY IT BEFORE THE SEVENTH PASS.\n\n");
        System.out.print("THE CROWD WILL DETERMINE WHAT AWARD YOU DESERVE\n");
        System.out.print("(POSTHUMOUSLY IF NECESSARY).\n");
        System.out.print("THE BRAVER YOU ARE, THE BETTER THE AWARD YOU RECEIVE.\n\n");
        System.out.print("THE BETTER THE JOB THE PICADORES AND TOREADORES DO,\n");
        System.out.print("THE BETTER YOUR CHANCES ARE.\n\n");
    }

    private static void printHeader() {
        System.out.println("                                               BULL");
        System.out.println("                            CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    }

    /**
     * Random number 1 or 2
     * FNA in the original
     */
    private static int randomNumber1or2(Random random) {
        return (int) (random.nextFloat() * 2 + 1);
    }

    private static double firstStage(String horseType, String suffix, BullGrade bullGrade, Random random) {
        double b = ((3d / bullGrade.getValue()) * random.nextFloat());
        double c;

        if (b < 0.37) {
            c = 0.5;
        } else if (b < 0.5) {
            c = 0.4;
        } else if (b < 0.63) {
            c = 0.3;
        } else if (b < 0.87) {
            c = 0.2;
        } else {
            c = 0.1;
        }

        int t = (int) (10 * c + 0.2);

        System.out.println("THE " + horseType + suffix + " DID A " + getPerformanceRating(t) + " JOB.");

        if (t >= 4) {
            if (t == 5) {
                if (!horseType.equals("TOREAD")) {
                    System.out.println(" " + randomNumber1or2(random) + " OF THE HORSES OF THE " + horseType + suffix + " KILLED.");
                }
                System.out.println(" " + randomNumber1or2(random) + " OF THE " + horseType + suffix + " KILLED.");

            } else {
                if (random.nextBoolean()) {
                    System.out.println("ONE OF THE " + horseType + " " + suffix + " WAS KILLED.");
                } else {
                    System.out.println("NO " + horseType + " " + suffix + " WERE KILLED.");
                }
            }
        }

        System.out.println();
        return c;
    }

    public static String getPerformanceRating(int t) {
        switch (t) {
            case 1:
                return "SUPERB";
            case 2:
                return "GOOD";
            case 3:
                return "FAIR";
            case 4:
                return "POOR";
            default:
                return "AWFUL";
        }
    }

    private static GameState attemptKillBull(BullGrade bullGrade, Random random, GameState gameState, Scanner scanner) {
        GameState newGameState = gameState.newCopy();
        System.out.println("\nIT IS THE MOMENT OF TRUTH.\n");

        int h = readInt("HOW DO YOU TRY TO KILL THE BULL", scanner);
        if (h == 4 || h == 5) {
            var K = (6 - bullGrade.getValue()) * 10 * random.nextFloat() / ((gameState.picadoresDamage + gameState.toreadoresDamage) * 5 * gameState.passNumber);
            if (h == 4) {
                if (K > 0.8) {
                    System.out.println("THE BULL HAS GORED YOU!");
                    newGameState = stateAfterGoring(random, scanner, newGameState, bullGrade);
                } else {
                    System.out.println("YOU KILLED THE BULL!");
                    newGameState.bullStatus = BullStatus.DEAD;
                    return newGameState;//game over
                }
            } else {
                if (K > 0.2) {
                    System.out.println("THE BULL HAS GORED YOU!");
                    newGameState = stateAfterGoring(random, scanner, newGameState, bullGrade);
                } else {
                    System.out.println("YOU KILLED THE BULL!");
                    newGameState.bullStatus = BullStatus.DEAD;
                    return newGameState;//game over
                }
            }

        } else {
            System.out.println("YOU PANICKED.  THE BULL GORED YOU.");
            if (random.nextBoolean()) {
                System.out.println("YOU ARE DEAD.");
                newGameState.matadorStatus = MatadorStatus.DEAD;
                return newGameState;
            } else {
                return readAndHandleForfeitDecision(random, scanner, newGameState, bullGrade);
            }

        }

        return newGameState;
    }

    private static GameState readAndHandleForfeitDecision(Random random, Scanner scanner, GameState gameState, BullGrade bullGrade) {
        GameState newGameState = gameState.newCopy();

        System.out.print("\nDO YOU RUN FROM THE RING? ");
        BooleanAnswer yesOrNo = readYesOrNo(scanner);
        if (yesOrNo == BooleanAnswer.NO) {
            System.out.println("\n\nYOU ARE BRAVE.  STUPID, BUT BRAVE.");
            if (random.nextBoolean()) {
                newGameState.matadorStatus = MatadorStatus.INJURED;
                return newGameState;
            } else {
                System.out.println("YOU ARE GORED AGAIN!");
                return stateAfterGoring(random, scanner, newGameState, bullGrade);
            }
        } else {
            System.out.println("COWARD");
            newGameState.matadorStatus = MatadorStatus.DEFEATED;
            return newGameState;

        }
    }

    private static boolean readInstructionsChoice(Scanner scan) {
        System.out.print("DO YOU WANT INSTRUCTIONS? ");

        String choice = scan.nextLine();
        return !choice.equalsIgnoreCase("NO");
    }


    private enum BooleanAnswer {
        YES, NO
    }

    private enum BullGrade {
        SUPERB(1),
        GOOD(2),
        FAIR(3),
        POOR(4),
        AWFUL(5);

        private final int value;

        BullGrade(int value) {
            this.value = value;
        }

        public int getValue() {
            return value;
        }

        public static BullGrade fromValue(int value) {
            for (BullGrade grade : BullGrade.values()) {
                if (grade.getValue() == value) {
                    return grade;
                }
            }
            throw new IllegalArgumentException("Invalid value: " + value);
        }

    }

    /**
     * Represents the game state.
     */
    private static class GameState {
        private MatadorStatus matadorStatus; //D[4] in the original
        private BullStatus bullStatus; //D[5] in the original
        private double picadoresDamage; //D[1] in the original
        private double toreadoresDamage; //D[2] in the original
        private int passNumber; //D[3] in the original
        private double capeMovesCumulative; //L in the original

        public GameState() {
            picadoresDamage = 0;
            toreadoresDamage = 0;
            passNumber = 0;
            matadorStatus = MatadorStatus.ALIVE;
            bullStatus = BullStatus.ALIVE;
            capeMovesCumulative = 1;
        }

        public GameState newCopy() {
            GameState newState = new GameState();
            newState.matadorStatus = this.matadorStatus;
            newState.bullStatus = this.bullStatus;
            newState.picadoresDamage = this.picadoresDamage;
            newState.toreadoresDamage = this.toreadoresDamage;
            newState.passNumber = this.passNumber;
            newState.capeMovesCumulative = this.capeMovesCumulative;
            return newState;
        }
    }

    private enum MatadorStatus {
        ALIVE(1),
        INJURED(2),
        DEAD(1.5),
        DEFEATED(0);

        private final double value;

        MatadorStatus(double value) {
            this.value = value;
        }

        public double getValue() {
            return value;
        }
    }

    private enum BullStatus {
        ALIVE(1),
        DEAD(2);

        private final double value;

        BullStatus(double value) {
            this.value = value;
        }

        public double getValue() {
            return value;
        }
    }

}

