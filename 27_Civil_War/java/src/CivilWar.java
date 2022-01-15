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
    private final int[] strategies;
    private int numGenerals;
    private final ArmyPair<ArmyResources> resources;
    private int battleNumber;
    private int Y;
    private int Y2;
    private final ArmyPair<Integer> totalExpectedCasualties;
    private final ArmyPair<Integer> totalCasualties;
    private boolean excessiveConfederateLosses;
    private boolean excessiveUnionLosses;
    private final BattleResults results;
    private final ArmyPair<Integer> revenue;
    private final ArmyPair<Integer> inflation;
    private final ArmyPair<Integer> totalExpenditure;
    private final ArmyPair<Integer> totalTroops;
    private int unionTroops;  // M6
    private boolean confedSurrender;
    private boolean unionSurrender;
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


        excessiveConfederateLosses = false;

        // INFLATION CALC
        // REM - ONLY IN PRINTOUT IS CONFED INFLATION = I1+15%
        inflation.confederate = 10 + (results.union - results.confederate) * 2;
        inflation.union = 10 + (results.confederate - results.union) * 2;

        // MONEY AVAILABLE
        resources.confederate.budget = 100 * (int) Math.floor((battle.troops.confederate * (100.0 - inflation.confederate) / 2000) * (1 + (revenue.confederate - totalExpenditure.confederate) / (revenue.confederate + 1.0)) + .5);

        // MEN AVAILABLE
        var confedTroops = (int) Math.floor(battle.troops.confederate * (1 + (totalExpectedCasualties.confederate - totalCasualties.confederate) / (totalTroops.confederate + 1.0)));
        this.unionTroops = (int) Math.floor(battle.troops.union * (1 + (totalExpectedCasualties.union - totalCasualties.union) / (totalTroops.union + 1.0)));
        battleState.F1 = 5 * battle.troops.confederate / 6.0;

        if (this.numGenerals == 2) {
            resources.union.budget = 100 * (int) Math.floor((battle.troops.union * (100.0 - inflation.union) / 2000) * (1 + (revenue.union - totalExpenditure.union) / (revenue.union + 1.0)) + .5);
        } else {
            resources.union.budget = 100 * (int) Math.floor(battle.troops.union * (100.0 - inflation.union) / 2000 + .5);
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
        out.println("MONEY     $ " + resources.confederate.budget + "       $ " + resources.union.budget);
        out.println("INFLATION   " + (inflation.confederate + 15) + "%          " + inflation.union + "%");

        // ONLY IN PRINTOUT IS CONFED INFLATION = I1+15%
        // IF TWO GENERALS, INPUT CONFED. FIRST

        for (int i = 0; i < numGenerals; i++) {
            out.println();

            ArmyResources currentResources;

            if (this.numGenerals == 1 || i == 0) {
                out.print("CONFEDERATE GENERAL --- ");
                currentResources = resources.confederate;
            } else {
                out.print("UNION GENERAL --- ");
                currentResources = resources.union;
            }

            var validInputs = false;
            while (!validInputs) {
                out.println("HOW MUCH DO YOU WISH TO SPEND FOR");
                out.print("- FOOD...... ? ");
                var food = terminalInput.nextInt();
                if (food == 0) {
                    if (this.revenue.confederate != 0) {
                        out.println("ASSUME YOU WANT TO KEEP SAME ALLOCATIONS");
                        out.println();
                    }
                } else {
                    currentResources.food = food;
                }

                out.print("- SALARIES.. ? ");
                currentResources.salaries = terminalInput.nextInt();

                out.print("- AMMUNITION ? ");
                currentResources.ammunition = terminalInput.nextInt();  // FIXME Retry if -ve

                if (currentResources.getTotal() > currentResources.budget) {
                    out.println("THINK AGAIN! YOU HAVE ONLY $" + currentResources.budget);
                } else {
                    validInputs = true;
                }
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

        ArmyResources currentResources;

        if (this.numGenerals == 1 || armyIdx == 0) {
            builder.append("CONFEDERATE ");
            currentResources = resources.confederate;
        } else {
            builder.append("UNION ");
            currentResources = resources.union;
        }

        // FIND MORALE
        currentResources.morale = (2 * Math.pow(currentResources.food, 2) + Math.pow(currentResources.salaries, 2)) / Math.pow(battleState.F1, 2) + 1;
        if (currentResources.morale >= 10) {
            builder.append("MORALE IS HIGH");
        } else if (currentResources.morale >= 5) {
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
    private UnionLosses simulateUnionLosses(HistoricalDatum battle) {
        var losses = (2.0 * battle.expectedCasualties.union / 5) * (1 + 1.0 / (2 * (Math.abs(Y2 - Y) + 1)));
        losses = losses * (1.28 + (5.0 * battle.troops.union / 6) / (resources.union.ammunition + 1));
        losses = Math.floor(losses * (1 + 1 / resources.union.morale) + 0.5);
        // IF LOSS > MEN PRESENT, RESCALE LOSSES
        var moraleFactor = 100 / resources.union.morale;

        if (Math.floor(losses + moraleFactor) >= unionTroops) {
            losses = Math.floor(13.0 * unionTroops / 20);
            moraleFactor = 7 * losses / 13;
            excessiveUnionLosses = true;
        }

        return new UnionLosses((int) losses, (int) Math.floor(moraleFactor));
    }

    // 2170: CALCULATE SIMULATED LOSSES
    private void calcLosses(BattleState battle) {
        // 2190
        out.println();
        out.println("            CONFEDERACY    UNION");

        var C5 = (2 * battle.data.expectedCasualties.confederate / 5) * (1 + 1.0 / (2 * (Math.abs(Y2 - Y) + 1)));
        C5 = (int) Math.floor(C5 * (1 + 1.0 / resources.confederate.morale) * (1.28 + battle.F1 / (resources.confederate.ammunition + 1.0)) + .5);
        var E = 100 / resources.confederate.morale;

        if (C5 + 100 / resources.confederate.morale >= battle.data.troops.confederate * (1 + (totalExpectedCasualties.confederate - totalCasualties.confederate) / (totalTroops.confederate + 1.0))) {
            C5 = (int) Math.floor(13.0 * battle.data.troops.confederate / 20 * (1 + (totalExpectedCasualties.union - totalCasualties.confederate) / (totalTroops.confederate + 1.0)));
            E = 7 * C5 / 13.0;
            excessiveConfederateLosses = true;
        }

        /////  2270

        final UnionLosses unionLosses;

        if (this.numGenerals == 1) {
            unionLosses = new UnionLosses((int) Math.floor(17.0 * battle.data.expectedCasualties.union * battle.data.expectedCasualties.confederate / (C5 * 20)), (int) Math.floor(5 * resources.confederate.morale));
        } else {
            unionLosses = simulateUnionLosses(battle.data);
        }

        out.println("CASUALTIES:  " + rightAlignInt(C5) + "        " + rightAlignInt(unionLosses.losses));
        out.println("DESERTIONS:  " + rightAlignInt(E) + "        " + rightAlignInt(unionLosses.desertions));
        out.println();

        if (numGenerals == 2) {
            out.println("COMPARED TO THE ACTUAL CASUALTIES AT " + battle.data.name);
            out.println("CONFEDERATE: " + (int) Math.floor(100 * (C5 / (double) battle.data.expectedCasualties.confederate) + 0.5) + " % OF THE ORIGINAL");
            out.println("UNION:       " + (int) Math.floor(100 * (unionLosses.losses / (double) battle.data.expectedCasualties.union) + 0.5) + " % OF THE ORIGINAL");

            out.println();

            // REM - 1 WHO WON
            var winner = findWinner(C5 + E, unionLosses.losses + unionLosses.desertions);
            switch (winner) {
                case UNION -> {
                    out.println("THE UNION WINS " + battle.data.name);
                    results.union++;
                }
                case CONFED -> {
                    out.println("THE CONFEDERACY WINS " + battle.data.name);
                    results.confederate++;
                }
                case INDECISIVE -> {
                    out.println("BATTLE OUTCOME UNRESOLVED");
                    results.indeterminate++;
                }
            }
        } else {
            out.println("YOUR CASUALTIES WERE " + Math.floor(100 * (C5 / (double) battle.data.expectedCasualties.confederate) + 0.5) + "% OF THE ACTUAL CASUALTIES AT " + battle.data.name);

            // FIND WHO WON

            if (excessiveConfederateLosses) {
                out.println("YOU LOSE " + battle.data.name);

                if (this.battleNumber != 0) {
                    results.union++;
                }
            } else {
                out.println("YOU WIN " + battle.data.name);
                // CUMULATIVE BATTLE FACTORS WHICH ALTER HISTORICAL RESOURCES AVAILABLE.IF A REPLAY DON'T UPDATE.
                results.confederate++;
            }
        }

        if (this.battleNumber != 0) {
            totalCasualties.confederate += (int) (C5 + E);
            totalCasualties.union += unionLosses.losses + unionLosses.desertions;
            totalExpectedCasualties.confederate += battle.data.expectedCasualties.confederate;
            totalExpectedCasualties.union += battle.data.expectedCasualties.union;
            totalExpenditure.confederate += resources.confederate.getTotal();
            totalExpenditure.union += resources.union.getTotal();
            revenue.confederate += battle.data.troops.confederate * (100 - inflation.confederate) / 20;
            revenue.union += battle.data.troops.union * (100 - inflation.union) / 20;
            totalTroops.confederate += battle.data.troops.confederate;
            totalTroops.union += battle.data.troops.union;

            updateStrategies(this.Y);
        }
    }

    // 2790
    private void reset() {
        excessiveConfederateLosses = excessiveUnionLosses = false;

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
        out.println("THE CONFEDERACY HAS WON " + results.confederate + " BATTLES AND LOST " + results.union);

        if (this.Y2 == 5) {
            out.println("THE CONFEDERACY HAS WON THE WAR");
        }

        if (this.Y == 5 || results.confederate <= results.union) {
            out.println("THE UNION HAS WON THE WAR");
        }

        out.println();

        // FIXME 2960  IF R1=0 THEN 3100

        out.println("FOR THE " + results.getTotal() + " BATTLES FOUGHT (EXCLUDING RERUNS)");
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
        if (this.excessiveConfederateLosses && this.excessiveUnionLosses) {
            return Winner.INDECISIVE;
        }

        if (this.excessiveConfederateLosses) {
            return Winner.UNION;
        }

        if (this.excessiveUnionLosses || confLosses < unionLosses) {
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

        this.results = new BattleResults();

        this.totalCasualties = new ArmyPair<>(0, 0);
        this.totalExpectedCasualties = new ArmyPair<>(0, 0);
        this.totalExpenditure = new ArmyPair<>(0, 0);
        this.totalTroops = new ArmyPair<>(0, 0);

        this.revenue = new ArmyPair<>(0, 0);
        this.inflation = new ArmyPair<>(0, 0);

        this.resources = new ArmyPair<>(new ArmyResources(), new ArmyResources());

        // UNION INFO ON LIKELY CONFEDERATE STRATEGY
        this.strategies = new int[]{25, 25, 25, 25};

        // READ HISTORICAL DATA.
        // HISTORICAL DATA...CAN ADD MORE (STRAT.,ETC) BY INSERTING DATA STATEMENTS AFTER APPRO. INFO, AND ADJUSTING READ
        this.data = List.of(
                new HistoricalDatum("BULL RUN", new ArmyPair<>(18000, 18500), new ArmyPair<>(1967, 2708), OffensiveStatus.DEFENSIVE, new String[]{"JULY 21, 1861.  GEN. BEAUREGARD, COMMANDING THE SOUTH, MET", "UNION FORCES WITH GEN. MCDOWELL IN A PREMATURE BATTLE AT", "BULL RUN. GEN. JACKSON HELPED PUSH BACK THE UNION ATTACK."}),
                new HistoricalDatum("SHILOH", new ArmyPair<>(40000, 44894), new ArmyPair<>(10699, 13047), OffensiveStatus.OFFENSIVE, new String[]{"APRIL 6-7, 1862.  THE CONFEDERATE SURPRISE ATTACK AT", "SHILOH FAILED DUE TO POOR ORGANIZATION."}),
                new HistoricalDatum("SEVEN DAYS", new ArmyPair<>(95000, 115000), new ArmyPair<>(20614, 15849), OffensiveStatus.OFFENSIVE, new String[]{"JUNE 25-JULY 1, 1862.  GENERAL LEE (CSA) UPHELD THE", "OFFENSIVE THROUGHOUT THE BATTLE AND FORCED GEN. MCCLELLAN", "AND THE UNION FORCES AWAY FROM RICHMOND."}),
                new HistoricalDatum("SECOND BULL RUN", new ArmyPair<>(54000, 63000), new ArmyPair<>(10000, 14000), OffensiveStatus.BOTH_OFFENSIVE, new String[]{"AUG 29-30, 1862.  THE COMBINED CONFEDERATE FORCES UNDER", " LEE", "AND JACKSON DROVE THE UNION FORCES BACK INTO WASHINGTON."}),
                new HistoricalDatum("ANTIETAM", new ArmyPair<>(40000, 50000), new ArmyPair<>(10000, 12000), OffensiveStatus.OFFENSIVE, new String[]{"SEPT 17, 1862.  THE SOUTH FAILED TO INCORPORATE MARYLAND", "INTO THE CONFEDERACY."}),
                new HistoricalDatum("FREDERICKSBURG", new ArmyPair<>(75000, 120000), new ArmyPair<>(5377, 12653), OffensiveStatus.DEFENSIVE, new String[]{"DEC 13, 1862.  THE CONFEDERACY UNDER LEE SUCCESSFULLY", "REPULSED AN ATTACK BY THE UNION UNDER GEN. BURNSIDE."}),
                new HistoricalDatum("MURFREESBORO", new ArmyPair<>(38000, 45000), new ArmyPair<>(11000, 12000), OffensiveStatus.DEFENSIVE, new String[]{"DEC 31, 1862.  THE SOUTH UNDER GEN. BRAGG WON A CLOSE BATTLE."}),
                new HistoricalDatum("CHANCELLORSVILLE", new ArmyPair<>(32000, 90000), new ArmyPair<>(13000, 17197), OffensiveStatus.BOTH_OFFENSIVE, new String[]{"MAY 1-6, 1863.  THE SOUTH HAD A COSTLY VICTORY AND LOST", "ONE OF THEIR OUTSTANDING GENERALS, 'STONEWALL' JACKSON."}),
                new HistoricalDatum("VICKSBURG", new ArmyPair<>(50000, 70000), new ArmyPair<>(12000, 19000), OffensiveStatus.DEFENSIVE, new String[]{"JULY 4, 1863.  VICKSBURG WAS A COSTLY DEFEAT FOR THE SOUTH", "BECAUSE IT GAVE THE UNION ACCESS TO THE MISSISSIPPI."}),
                new HistoricalDatum("GETTYSBURG", new ArmyPair<>(72500, 85000), new ArmyPair<>(20000, 23000), OffensiveStatus.OFFENSIVE, new String[]{"JULY 1-3, 1863.  A SOUTHERN MISTAKE BY GEN. LEE AT GETTYSBURG", "COST THEM ONE OF THE MOST CRUCIAL BATTLES OF THE WAR."}),
                new HistoricalDatum("CHICKAMAUGA", new ArmyPair<>(66000, 60000), new ArmyPair<>(18000, 16000), OffensiveStatus.BOTH_OFFENSIVE, new String[]{"SEPT. 15, 1863. CONFUSION IN A FOREST NEAR CHICKAMAUGA LED", "TO A COSTLY SOUTHERN VICTORY."}),
                new HistoricalDatum("CHATTANOOGA", new ArmyPair<>(37000, 60000), new ArmyPair<>(36700, 5800), OffensiveStatus.BOTH_OFFENSIVE, new String[]{"NOV. 25, 1863. AFTER THE SOUTH HAD SIEGED GEN. ROSENCRANS'", "ARMY FOR THREE MONTHS, GEN. GRANT BROKE THE SIEGE."}),
                new HistoricalDatum("SPOTSYLVANIA", new ArmyPair<>(62000, 110000), new ArmyPair<>(17723, 18000), OffensiveStatus.BOTH_OFFENSIVE, new String[]{"MAY 5, 1864.  GRANT'S PLAN TO KEEP LEE ISOLATED BEGAN TO", "FAIL HERE, AND CONTINUED AT COLD HARBOR AND PETERSBURG."}),
                new HistoricalDatum("ATLANTA", new ArmyPair<>(65000, 100000), new ArmyPair<>(8500, 3700), OffensiveStatus.DEFENSIVE, new String[]{"AUGUST, 1864.  SHERMAN AND THREE VETERAN ARMIES CONVERGED", "ON ATLANTA AND DEALT THE DEATH BLOW TO THE CONFEDERACY."})
        );
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
        while (true) {
            try {
                var input = new Scanner(System.in).nextLine();
                if (validator.test(input)) {
                    return input;
                }
            } catch (InputMismatchException e) {
                // Ignore
            }
            System.out.println(reminder);
        }
    }

    private static int inputInt(Predicate<Integer> validator, Function<Integer, String> reminder) {
        while (true) {
            try {
                var input = new Scanner(System.in).nextInt();
                if (validator.test(input)) {
                    return input;
                }
                System.out.println(reminder.apply(input));
            } catch (InputMismatchException e) {
                System.out.println(reminder.apply(0));
            }
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

    private static class BattleResults {
        private int confederate;
        private int union;
        private int indeterminate;

        public int getTotal() {
            return confederate + union + indeterminate;
        }
    }

    private static class ArmyResources {
        private int food;
        private int salaries;
        private int ammunition;
        private int budget;

        private double morale;  // TODO really here?

        public int getTotal() {
            return this.food + this.salaries + this.ammunition;
        }
    }

    private record HistoricalDatum(String name, ArmyPair<Integer> troops,
                                   ArmyPair<Integer> expectedCasualties,
                                   OffensiveStatus offensiveStatus, String[] blurb) {
    }

    private record UnionLosses(int losses, int desertions) {
    }
}