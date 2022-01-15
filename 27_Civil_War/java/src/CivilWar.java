import java.io.PrintStream;
import java.util.List;
import java.util.Scanner;
import java.util.function.Predicate;

import static java.util.stream.Collectors.joining;
import static java.util.stream.IntStream.range;

@SuppressWarnings("SpellCheckingInspection")
public class CivilWar {

    private final PrintStream out;
    private final List<HistoricalDatum> data;

    private BattleState currentBattle;
    private int R1;
    private int R2;
    private final int[] strategies;
    private int numGenerals;
    private final int[] B;
    private final int[] F;
    private final int[] H;
    private final int[] D;
    private final double[] O;
    private int battleNumber;
    private int Y;
    private int Y2;
    private final ArmyPair<Integer> totalExpectedCasualties;
    private final ArmyPair<Integer> totalCasualties;
    private int excessiveConfederateLosses;
    private int excessiveUnionLosses;
    private int L;
    private int W;
    private int M3;
    private int M4;
    private int Q1;
    private int Q2;
    private double C6;
    private double E2;
    private int W0;
    private int confedTroops;  // M5
    private int unionTroops;  // M6
    private boolean confedSurrender;
    private boolean unionSurrender;
    private int I1;
    private int I2;
    private double R;
    private boolean wantBattleDescriptions;

    private final static String YES_NO_REMINDER = "(ANSWER YES OR NO)";
    private final static Predicate<String> YES_NO_CHECKER = i -> isYes(i) || isNo(i);

    /**
     * ORIGINAL GAME DESIGN: CRAM, GOODIE, HIBBARD LEXINGTON H.S.
     * MODIFICATIONS: G. PAUL, R. HESS (TIES), 1973
     */
    public static void main(String[] args) {
        var x = new CivilWar(System.out);
        x.showCredits();

        // LET D=RND(-1) ???

        System.out.print("DO YOU WANT INSTRUCTIONS? ");

        if (isYes(inputString(YES_NO_CHECKER, YES_NO_REMINDER))) {
            x.showHelp();
        }

        x.gameLoop();
    }

    private void gameLoop() {
        out.println();
        out.println();
        out.println();
        out.print("ARE THERE TWO GENERALS PRESENT (ANSWER YES OR NO)? ");

        if (isYes(inputString(YES_NO_CHECKER, YES_NO_REMINDER))) {
            this.numGenerals = 2;
        } else {
            this.numGenerals = 1;
            out.println();
            out.println("YOU ARE THE CONFEDERACY.   GOOD LUCK!");
            out.println();
        }

        out.println("SELECT A BATTLE BY TYPING A NUMBER FROM 1 TO 14 ON");
        out.println("REQUEST.  TYPE ANY OTHER NUMBER TO END THE SIMULATION.");
        out.println("BUT '0' BRINGS BACK EXACT PREVIOUS BATTLE SITUATION");
        out.println("ALLOWING YOU TO REPLAY IT");
        out.println();
        out.println("NOTE: A NEGATIVE FOOD$ ENTRY CAUSES THE PROGRAM TO ");
        out.println("USE THE ENTRIES FROM THE PREVIOUS BATTLE");
        out.println();

        out.print("AFTER REQUESTING A BATTLE, DO YOU WISH BATTLE DESCRIPTIONS (ANSWER YES OR NO)? ");

        this.wantBattleDescriptions = isYes(inputString(YES_NO_CHECKER, YES_NO_REMINDER));

        while (true) {
            var battle = startBattle();
            if (battle == null) {
                break;
            }

            this.currentBattle = battle;

            offensiveLogic(battle.data);

            unionStrategy();

            simulatedLosses(battle.data);
            calcLosses(battle);

            reset();

            if (this.confedSurrender) {
                out.println("THE CONFEDERACY HAS SURRENDERED");
            } else if (unionSurrender) {  // FIXME Is this actually possible? 2850
                out.println("THE UNION HAS SURRENDERED.");
            }
        }

        complete();
    }

    private BattleState startBattle() {
        out.println();
        out.println();
        out.println();
        out.print("WHICH BATTLE DO YOU WISH TO SIMULATE ? ");

        var terminalInput = new Scanner(System.in);
        var battleNumber = terminalInput.nextInt();

        if (battleNumber == 0 && this.currentBattle != null) {
            out.println(this.currentBattle.data.name + " INSTANT REPLAY");
            return this.currentBattle;
        }

        if (battleNumber <= 0 || battleNumber > this.data.size()) {
            return null;
        }

        this.battleNumber = battleNumber;

        var battle = this.data.get(this.battleNumber - 1);
        var battleState = new BattleState(battle);


        excessiveConfederateLosses = 0;

        // INFLATION CALC
        // REM - ONLY IN PRINTOUT IS CONFED INFLATION = I1+15%
        I1 = 10 + (L - W) * 2;
        I2 = 10 + (W - L) * 2;

        // MONEY AVAILABLE
        this.D[0] = 100 * (int) Math.floor((battle.troops.confederate * (100.0 - I1) / 2000) * (1 + (R1 - Q1) / (R1 + 1.0)) + .5);

        // MEN AVAILABLE
        this.confedTroops = (int) Math.floor(battle.troops.confederate * (1 + (totalExpectedCasualties.confederate - totalCasualties.confederate) / (M3 + 1.0)));
        this.unionTroops = (int) Math.floor(battle.troops.union * (1 + (totalExpectedCasualties.union - totalCasualties.union) / (M4 + 1.0)));
        battleState.F1 = 5 * battle.troops.confederate / 6.0;

        if (this.numGenerals == 2) {
            this.D[1] = 100 * (int) Math.floor((battle.troops.union * (100.0 - I2) / 2000) * (1 + (R2 - Q2) / (R2 + 1.0)) + .5);
        } else {
            this.D[1] = 100 * (int) Math.floor(battle.troops.union * (100.0 - I2) / 2000 + .5);
        }

        out.println();
        out.println();
        out.println();
        out.println();
        out.println();
        out.println("THIS IS THE BATTLE OF " + battle.name);

        if (this.wantBattleDescriptions) {
            for (var eachLine : battle.blurb) {
                out.println(eachLine);
            }
        }

        out.println();
        out.println("          CONFEDERACY     UNION");
        out.println("MEN         " + confedTroops + "          " + unionTroops);
        out.println("MONEY     $ " + this.D[0] + "       $ " + this.D[1]);
        out.println("INFLATION   " + (I1 + 15) + "%          " + I2 + "%");

        // ONLY IN PRINTOUT IS CONFED INFLATION = I1+15%
        // IF TWO GENERALS, INPUT CONFED. FIRST

        for (int i = 0; i < numGenerals; i++) {
            out.println();

            if (this.numGenerals == 1 || i == 0) {
                out.print("CONFEDERATE GENERAL --- ");
            } else {
                out.print("UNION GENERAL --- ");
            }

            out.println("HOW MUCH DO YOU WISH TO SPEND FOR");
            out.print("- FOOD...... ? ");
            var F = terminalInput.nextInt();
            if (F == 0) {
                if (this.R1 != 0) {
                    out.println("ASSUME YOU WANT TO KEEP SAME ALLOCATIONS");
                    out.println();
                }
            }

            this.F[i] = F;

            out.print("- SALARIES.. ? ");
            this.H[i] = terminalInput.nextInt();

            out.print("- AMMUNITION ? ");
            this.B[i] = terminalInput.nextInt();  // FIXME Retry if -ve

            if (this.F[i] + this.H[i] + this.B[i] > this.D[i]) {
                out.println("THINK AGAIN! YOU HAVE ONLY $" + this.D[i]);
                // FIXME Redo inputs from Food
            }
        }

        out.println();

        // Record Morale
        out.println(range(0, numGenerals).mapToObj(i -> moraleForArmy(battleState, i)).collect(joining(", ")));

        out.println();

        return battleState;
    }

    private String moraleForArmy(BattleState battleState, int armyIdx) {
        var builder = new StringBuilder();

        if (this.numGenerals == 1 || armyIdx == 0) {
            builder.append("CONFEDERATE ");
        } else {
            builder.append("UNION ");
        }

        // FIND MORALE
        this.O[armyIdx] = (2 * Math.pow(F[armyIdx], 2) + Math.pow(H[armyIdx], 2)) / Math.pow(battleState.F1, 2) + 1;
        if (this.O[armyIdx] >= 10) {
            builder.append("MORALE IS HIGH");
        } else if (this.O[armyIdx] >= 5) {
            builder.append("MORALE IS FAIR");
        } else {
            builder.append("MORALE IS POOR");
        }

        return builder.toString();
    }

    private enum OffensiveStatus {
        DEFENSIVE("YOU ARE ON THE DEFENSIVE"), OFFENSIVE("YOU ARE ON THE OFFENSIVE"), BOTH_OFFENSIVE("BOTH SIDES ARE ON THE OFFENSIVE");

        private final String label;

        OffensiveStatus(String label) {
            this.label = label;
        }
    }

    private void offensiveLogic(HistoricalDatum battle) {
        out.print("CONFEDERATE GENERAL---");
        // ACTUAL OFF/DEF BATTLE SITUATION
        out.println(battle.offensiveStatus.label);

        // CHOOSE STRATEGIES

        if (numGenerals == 2) {
            out.print("CONFEDERATE STRATEGY ? ");
        } else {
            out.print("YOUR STRATEGY ? ");
        }

        var terminalInput = new Scanner(System.in);
        Y = terminalInput.nextInt();
        if (Math.abs(Y - 3) >= 3) {
            out.println("STRATEGY " + Y + " NOT ALLOWED.");
            // FIXME Proper numeric check!! Not abs
            // FIXME Retry Y input
        }

        if (Y == 5) {  // 1970
            confedSurrender = true;
        }
    }

    // 2070  REM : SIMULATED LOSSES-NORTH
    private void simulatedLosses(HistoricalDatum battle) {
        C6 = (2.0 * battle.expectedCasualties.union / 5) * (1 + 1.0 / (2 * (Math.abs(Y2 - Y) + 1)));
        C6 = C6 * (1.28 + (5.0 * battle.troops.union / 6) / (B[1] + 1));
        C6 = Math.floor(C6 * (1 + 1 / O[1]) + 0.5);
        // IF LOSS > MEN PRESENT, RESCALE LOSSES
        E2 = 100 / O[1];
        if (Math.floor(C6 + E2) >= unionTroops) {
            C6 = Math.floor(13.0 * unionTroops / 20);
            E2 = 7 * C6 / 13;
            excessiveUnionLosses = 1;
        }
    }

    // 2170: CALCULATE SIMULATED LOSSES
    private void calcLosses(BattleState battle) {
        // 2190
        out.println();
        out.println("            CONFEDERACY    UNION");

        var C5 = (2 * battle.data.expectedCasualties.confederate / 5) * (1 + 1.0 / (2 * (Math.abs(Y2 - Y) + 1)));
        C5 = (int) Math.floor(C5 * (1 + 1.0 / this.O[0]) * (1.28 + battle.F1 / (this.B[0] + 1.0)) + .5);
        var E = 100 / O[0];

        if (C5 + 100 / O[0] >= battle.data.troops.confederate * (1 + (totalExpectedCasualties.confederate - totalCasualties.confederate) / (M3 + 1.0))) {
            C5 = (int) Math.floor(13.0 * battle.data.troops.confederate / 20 * (1 + (totalExpectedCasualties.union - totalCasualties.confederate) / (M3 + 1.0)));
            E = 7 * C5 / 13.0;
            excessiveConfederateLosses = 1;
        }

        /////  2270

        if (this.numGenerals == 1) {
            C6 = (int) Math.floor(17.0 * battle.data.expectedCasualties.union * battle.data.expectedCasualties.confederate / (C5 * 20));
            E2 = 5 * O[0];
        }

        out.println("CASUALTIES:  " + rightAlignInt(C5) + "        " + rightAlignInt(C6));
        out.println("DESERTIONS:  " + rightAlignInt(E) + "        " + rightAlignInt(E2));
        out.println();

        if (numGenerals == 2) {
            out.println("COMPARED TO THE ACTUAL CASUALTIES AT " + battle.data.name);
            out.println("CONFEDERATE: " + (int) Math.floor(100 * (C5 / (double) battle.data.expectedCasualties.confederate) + 0.5) + " % OF THE ORIGINAL");
            out.println("UNION:       " + (int) Math.floor(100 * (C6 / (double) battle.data.expectedCasualties.union) + 0.5) + " % OF THE ORIGINAL");

            out.println();

            // REM - 1 WHO WON
            var winner = findWinner(C5 + E, C6 + E2);
            switch (winner) {
                case UNION -> {
                    out.println("THE UNION WINS " + battle.data.name);
                    L++;
                }
                case CONFED -> {
                    out.println("THE CONFEDERACY WINS " + battle.data.name);
                    W++;
                }
                case INDECISIVE -> {
                    out.println("BATTLE OUTCOME UNRESOLVED");
                    this.W0++;
                }
            }
        } else {
            out.println("YOUR CASUALTIES WERE " + Math.floor(100 * (C5 / (double) battle.data.expectedCasualties.confederate) + 0.5) + "% OF THE ACTUAL CASUALTIES AT " + battle.data.name);

            // FIND WHO WON

            if (excessiveConfederateLosses == 1) {
                out.println("YOU LOSE " + battle.data.name);

                if (this.battleNumber != 0) {
                    L++;
                }
            } else {
                out.println("YOU WIN " + battle.data.name);
                // CUMULATIVE BATTLE FACTORS WHICH ALTER HISTORICAL RESOURCES AVAILABLE.IF A REPLAY DON'T UPDATE.
                W++;
            }
        }

        if (this.battleNumber != 0) {
            totalCasualties.confederate += (int) (C5 + E);
            totalCasualties.union += (int) (C6 + E2);
            totalExpectedCasualties.confederate += battle.data.expectedCasualties.confederate;
            totalExpectedCasualties.union += battle.data.expectedCasualties.union;
            Q1 += F[0] + H[0] + B[0];
            Q2 += F[1] + H[1] + B[1];
            R1 += battle.data.troops.confederate * (100 - I1) / 20;
            R2 += battle.data.troops.union * (100 - I2) / 20;
            M3 += battle.data.troops.confederate;
            M4 += battle.data.troops.union;

            updateStrategies(this.Y);
        }
    }

    // 2790
    private void reset() {
        excessiveConfederateLosses = excessiveUnionLosses = 0;

        out.println("---------------");
    }

    // 2820  REM------FINISH OFF
    private void complete() {
        out.println();
        out.println();
        out.println();
        out.println();
        out.println();
        out.println();
        out.println("THE CONFEDERACY HAS WON " + this.W + " BATTLES AND LOST " + this.L);

        if (this.Y2 == 5) {
            out.println("THE CONFEDERACY HAS WON THE WAR");
        }

        if (this.Y == 5 || this.W <= this.L) {
            out.println("THE UNION HAS WON THE WAR");
        }

        out.println();

        // FIXME 2960  IF R1=0 THEN 3100

        out.println("FOR THE " + (W + L + W0) + " BATTLES FOUGHT (EXCLUDING RERUNS)");
//        out.println(" ", " ", " ");
        out.println("                       CONFEDERACY    UNION");
        out.println("HISTORICAL LOSSES      " + (int) Math.floor(totalExpectedCasualties.confederate + .5) + "          " + (int) Math.floor(totalExpectedCasualties.union + .5));
        out.println("SIMULATED LOSSES       " + (int) Math.floor(totalCasualties.confederate + .5) + "          " + (int) Math.floor(totalCasualties.union + .5));
        out.println();
        out.println("    % OF ORIGINAL      " + (int) Math.floor(100 * ((double) totalCasualties.confederate / totalExpectedCasualties.confederate) + .5) + "             " + (int) Math.floor(100 * ((double) totalCasualties.union / totalExpectedCasualties.union) + .5));

        if (this.numGenerals == 1) {
            out.println();
            out.println("UNION INTELLIGENCE SUGGESTS THAT THE SOUTH USED ");
            out.println("STRATEGIES 1, 2, 3, 4 IN THE FOLLOWING PERCENTAGES");
            out.println(this.strategies[0] + "," + this.strategies[1] + "," + this.strategies[2] + "," + this.strategies[3]);
        }
    }

    private Winner findWinner(double confLosses, double unionLosses) {
        if (this.excessiveConfederateLosses == 1 && this.excessiveUnionLosses == 1) {
            return Winner.INDECISIVE;
        }

        if (this.excessiveConfederateLosses == 1) {
            return Winner.UNION;
        }

        if (this.excessiveUnionLosses == 1 || confLosses < unionLosses) {
            return Winner.CONFED;
        }

        if (confLosses == unionLosses) {
            return Winner.INDECISIVE;
        }

        return Winner.UNION;  // FIXME Really? 2400-2420 ?
    }

    private enum Winner {
        CONFED, UNION, INDECISIVE
    }

    private void unionStrategy() {
        if (this.battleNumber != 0) {
            out.print("UNION STRATEGY ? ");
            var terminalInput = new Scanner(System.in);
            Y2 = terminalInput.nextInt();
            if (Y2 < 0) {
                out.println("ENTER 1, 2, 3, OR 4 (USUALLY PREVIOUS UNION STRATEGY)");
                // FIXME Retry Y2 input !!!
            }

            if (Y2 < 5) {  // 3155
                return;
            }
        }

        var S0 = 0;

        this.R = 100 * Math.random();

        for (Y2 = 0; Y2 < 4; Y2++) {
            S0 += this.strategies[Y2];
            // IF ACTUAL STRATEGY INFO IS IN PROGRAM DATA STATEMENTS THEN R-100 IS EXTRA WEIGHT GIVEN TO THAT STATEGY.
            if (R < S0) {
                break;
            }
        }
        // IF ACTUAL STRAT. IN,THEN HERE IS Y2= HIST. STRAT.
        out.println("UNION STRATEGY IS " + Y2);
    }

    public CivilWar(PrintStream out) {
        this.out = out;

        this.totalCasualties = new ArmyPair<>(0, 0);
        this.totalExpectedCasualties = new ArmyPair<>(0, 0);

        // UNION INFO ON LIKELY CONFEDERATE STRATEGY
        this.strategies = new int[]{25, 25, 25, 25};

        this.F = new int[]{0, 0};
        this.H = new int[]{0, 0};
        this.B = new int[]{0, 0};
        this.D = new int[]{0, 0};
        this.O = new double[]{0, 0};

        // READ HISTORICAL DATA.
        // HISTORICAL DATA...CAN ADD MORE (STRAT.,ETC) BY INSERTING DATA STATEMENTS AFTER APPRO. INFO, AND ADJUSTING READ
        this.data = List.of(new HistoricalDatum("BULL RUN", new ArmyPair<>(18000, 18500), new ArmyPair<>(1967, 2708), OffensiveStatus.DEFENSIVE, new String[]{"JULY 21, 1861.  GEN. BEAUREGARD, COMMANDING THE SOUTH, MET", "UNION FORCES WITH GEN. MCDOWELL IN A PREMATURE BATTLE AT", "BULL RUN. GEN. JACKSON HELPED PUSH BACK THE UNION ATTACK."}), new HistoricalDatum("SHILOH", new ArmyPair<>(40000, 44894), new ArmyPair<>(10699, 13047), OffensiveStatus.OFFENSIVE, new String[]{"APRIL 6-7, 1862.  THE CONFEDERATE SURPRISE ATTACK AT", "SHILOH FAILED DUE TO POOR ORGANIZATION."}), new HistoricalDatum("SEVEN DAYS", new ArmyPair<>(95000, 115000), new ArmyPair<>(20614, 15849), OffensiveStatus.OFFENSIVE, new String[]{"JUNE 25-JULY 1, 1862.  GENERAL LEE (CSA) UPHELD THE", "OFFENSIVE THROUGHOUT THE BATTLE AND FORCED GEN. MCCLELLAN", "AND THE UNION FORCES AWAY FROM RICHMOND."}), new HistoricalDatum("SECOND BULL RUN", new ArmyPair<>(54000, 63000), new ArmyPair<>(10000, 14000), OffensiveStatus.BOTH_OFFENSIVE, new String[]{"AUG 29-30, 1862.  THE COMBINED CONFEDERATE FORCES UNDER", " LEE", "AND JACKSON DROVE THE UNION FORCES BACK INTO WASHINGTON."}), new HistoricalDatum("ANTIETAM", new ArmyPair<>(40000, 50000), new ArmyPair<>(10000, 12000), OffensiveStatus.OFFENSIVE, new String[]{"SEPT 17, 1862.  THE SOUTH FAILED TO INCORPORATE MARYLAND", "INTO THE CONFEDERACY."}), new HistoricalDatum("FREDERICKSBURG", new ArmyPair<>(75000, 120000), new ArmyPair<>(5377, 12653), OffensiveStatus.DEFENSIVE, new String[]{"DEC 13, 1862.  THE CONFEDERACY UNDER LEE SUCCESSFULLY", "REPULSED AN ATTACK BY THE UNION UNDER GEN. BURNSIDE."}), new HistoricalDatum("MURFREESBORO", new ArmyPair<>(38000, 45000), new ArmyPair<>(11000, 12000), OffensiveStatus.DEFENSIVE, new String[]{"DEC 31, 1862.  THE SOUTH UNDER GEN. BRAGG WON A CLOSE BATTLE."}), new HistoricalDatum("CHANCELLORSVILLE", new ArmyPair<>(32000, 90000), new ArmyPair<>(13000, 17197), OffensiveStatus.BOTH_OFFENSIVE, new String[]{"MAY 1-6, 1863.  THE SOUTH HAD A COSTLY VICTORY AND LOST", "ONE OF THEIR OUTSTANDING GENERALS, 'STONEWALL' JACKSON."}), new HistoricalDatum("VICKSBURG", new ArmyPair<>(50000, 70000), new ArmyPair<>(12000, 19000), OffensiveStatus.DEFENSIVE, new String[]{"JULY 4, 1863.  VICKSBURG WAS A COSTLY DEFEAT FOR THE SOUTH", "BECAUSE IT GAVE THE UNION ACCESS TO THE MISSISSIPPI."}), new HistoricalDatum("GETTYSBURG", new ArmyPair<>(72500, 85000), new ArmyPair<>(20000, 23000), OffensiveStatus.OFFENSIVE, new String[]{"JULY 1-3, 1863.  A SOUTHERN MISTAKE BY GEN. LEE AT GETTYSBURG", "COST THEM ONE OF THE MOST CRUCIAL BATTLES OF THE WAR."}), new HistoricalDatum("CHICKAMAUGA", new ArmyPair<>(66000, 60000), new ArmyPair<>(18000, 16000), OffensiveStatus.BOTH_OFFENSIVE, new String[]{"SEPT. 15, 1863. CONFUSION IN A FOREST NEAR CHICKAMAUGA LED", "TO A COSTLY SOUTHERN VICTORY."}), new HistoricalDatum("CHATTANOOGA", new ArmyPair<>(37000, 60000), new ArmyPair<>(36700, 5800), OffensiveStatus.BOTH_OFFENSIVE, new String[]{"NOV. 25, 1863. AFTER THE SOUTH HAD SIEGED GEN. ROSENCRANS'", "ARMY FOR THREE MONTHS, GEN. GRANT BROKE THE SIEGE."}), new HistoricalDatum("SPOTSYLVANIA", new ArmyPair<>(62000, 110000), new ArmyPair<>(17723, 18000), OffensiveStatus.BOTH_OFFENSIVE, new String[]{"MAY 5, 1864.  GRANT'S PLAN TO KEEP LEE ISOLATED BEGAN TO", "FAIL HERE, AND CONTINUED AT COLD HARBOR AND PETERSBURG."}), new HistoricalDatum("ATLANTA", new ArmyPair<>(65000, 100000), new ArmyPair<>(8500, 3700), OffensiveStatus.DEFENSIVE, new String[]{"AUGUST, 1864.  SHERMAN AND THREE VETERAN ARMIES CONVERGED", "ON ATLANTA AND DEALT THE DEATH BLOW TO THE CONFEDERACY."}));
    }

    private void showCredits() {
        out.println(" ".repeat(26) + "CIVIL WAR");
        out.println(" ".repeat(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        out.println();
        out.println();
        out.println();
    }

    private void updateStrategies(int strategy) {
        // REM LEARN  PRESENT STRATEGY, START FORGETTING OLD ONES
        // REM - PRESENT STRATEGY OF SOUTH GAINS 3*S, OTHERS LOSE S
        // REM   PROBABILITY POINTS, UNLESS A STRATEGY FALLS BELOW 5%.

        var S = 3;
        var S0 = 0;
        for (int i = 0; i < 4; i++) {
            if (this.strategies[i] <= 5) {
                continue;
            }

            this.strategies[i] -= S;
            S0 += S;

        }
        this.strategies[strategy - 1] += S0;

    }

    private void showHelp() {
        out.println();
        out.println();
        out.println();
        out.println();
        out.println("THIS IS A CIVIL WAR SIMULATION.");
        out.println("TO PLAY TYPE A RESPONSE WHEN THE COMPUTER ASKS.");
        out.println("REMEMBER THAT ALL FACTORS ARE INTERRELATED AND THAT YOUR");
        out.println("RESPONSES COULD CHANGE HISTORY. FACTS AND FIGURES USED ARE");
        out.println("BASED ON THE ACTUAL OCCURRENCE. MOST BATTLES TEND TO RESULT");
        out.println("AS THEY DID IN THE CIVIL WAR, BUT IT ALL DEPENDS ON YOU!!");
        out.println();
        out.println("THE OBJECT OF THE GAME IS TO WIN AS MANY BATTLES AS ");
        out.println("POSSIBLE.");
        out.println();
        out.println("YOUR CHOICES FOR DEFENSIVE STRATEGY ARE:");
        out.println("        (1) ARTILLERY ATTACK");
        out.println("        (2) FORTIFICATION AGAINST FRONTAL ATTACK");
        out.println("        (3) FORTIFICATION AGAINST FLANKING MANEUVERS");
        out.println("        (4) FALLING BACK");
        out.println(" YOUR CHOICES FOR OFFENSIVE STRATEGY ARE:");
        out.println("        (1) ARTILLERY ATTACK");
        out.println("        (2) FRONTAL ATTACK");
        out.println("        (3) FLANKING MANEUVERS");
        out.println("        (4) ENCIRCLEMENT");
        out.println("YOU MAY SURRENDER BY TYPING A '5' FOR YOUR STRATEGY.");
    }

    private static final int MAX_NUM_LENGTH = 6;

    private String rightAlignInt(int number) {
        var s = String.valueOf(number);
        return " ".repeat(MAX_NUM_LENGTH - s.length()) + s;
    }

    private String rightAlignInt(double number) {
        return rightAlignInt((int) Math.floor(number));
    }

    private static String inputString(Predicate<String> validator, String reminder) {
        var terminalInput = new Scanner(System.in);

        while (true) {
            var input = terminalInput.nextLine();
            if (validator.test(input)) {
                return input;
            }
            System.out.println(reminder);
        }
    }

    private static boolean isYes(String s) {
        if (s == null) {
            return false;
        }
        var uppercase = s.toUpperCase();
        return uppercase.equals("Y") || uppercase.equals("YES");
    }

    private static boolean isNo(String s) {
        if (s == null) {
            return false;
        }
        var uppercase = s.toUpperCase();
        return uppercase.equals("N") || uppercase.equals("NO");
    }

    private static class BattleState {
        private final HistoricalDatum data;
        private double F1;

        public BattleState(HistoricalDatum data) {
            this.data = data;
        }
    }

    private static class ArmyPair<T> {
        private T confederate;
        private T union;

        public ArmyPair(T confederate, T union) {
            this.confederate = confederate;
            this.union = union;
        }
    }

    private record HistoricalDatum(String name, ArmyPair<Integer> troops,
                                   ArmyPair<Integer> expectedCasualties,
                                   OffensiveStatus offensiveStatus, String[] blurb) {
    }
}