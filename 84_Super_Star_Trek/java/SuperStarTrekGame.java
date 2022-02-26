import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.Random;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

/**
 * SUPER STARTREK - MAY 16,1978
 * ****        **** STAR TREK ****        ****
 * **** SIMULATION OF A MISSION OF THE STARSHIP ENTERPRISE,
 * **** AS SEEN ON THE STAR TREK TV SHOW.
 * **** ORIGINAL PROGRAM BY MIKE MAYFIELD, MODIFIED VERSION
 * **** PUBLISHED IN DEC'S "101 BASIC GAMES", BY DAVE AHL.
 * **** MODIFICATIONS TO THE LATTER (PLUS DEBUGGING) BY BOB
 * *** LEEDOM - APRIL & DECEMBER 1974,
 * *** WITH A LITTLE HELP FROM HIS FRIENDS . . .
 *
 * Ported to Java in Jan-Feb 2022 by
 * Taciano Dreckmann Perez (taciano.perez@gmail.com)
 */
public class SuperStarTrekGame {

    // markers
    static final String MARKER_EMPTY = "   ";
    static final String MARKER_ENTERPRISE = "<*>";
    static final String MARKER_KLINGON = "+K+";
    static final String MARKER_STARBASE = ">!<";
    static final String MARKER_STAR = " * ";

    // commands
    static final int COMMAND_NAV = 1;
    static final int COMMAND_SRS = 2;
    static final int COMMAND_LRS = 3;
    static final int COMMAND_PHA = 4;
    static final int COMMAND_TOR = 5;
    static final int COMMAND_SHE = 6;
    static final int COMMAND_DAM = 7;
    static final int COMMAND_COM = 8;
    static final int COMMAND_XXX = 9;

    // computer commands
    static final int COMPUTER_COMMAND_CUMULATIVE_GALACTIC_RECORD = 1;
    static final int COMPUTER_COMMAND_STATUS_REPORT = 2;
    static final int COMPUTER_COMMAND_PHOTON_TORPEDO_DATA = 3;
    static final int COMPUTER_COMMAND_STARBASE_NAV_DATA = 4;
    static final int COMPUTER_COMMAND_DIR_DIST_CALC = 5;
    static final int COMPUTER_COMMAND_GALAXY_MAP = 6;

    // devices
    static final int DEVICE_WARP_ENGINES = 1;
    static final int DEVICE_SHORT_RANGE_SENSORS = 2;
    static final int DEVICE_LONG_RANGE_SENSORS = 3;
    static final int DEVICE_PHASER_CONTROL = 4;
    static final int DEVICE_PHOTON_TUBES = 5;
    static final int DEVICE_DAMAGE_CONTROL = 6;
    static final int DEVICE_SHIELD_CONTROL = 7;
    static final int DEVICE_LIBRARY_COMPUTER = 8;

    // other constants
    static final String QUADRANT_ROW = "                         ";
    static final String COMMANDS = "NAVSRSLRSPHATORSHEDAMCOMXXX";
    static final Random random = new Random();

    // game state
    final int galaxy[][] = new int[9][9];    // 8x8 galaxy map G
    final int cardinalDirections[][] = new int[10][3];   // 9x2 vectors in cardinal directions C
    final int klingonQuadrants[][] = new int[4][4];    // 3x3 position of klingons K
    final int chartedGalaxy[][] = new int[9][9];    // 8x8 charted galaxy map Z
    final double deviceStatus[] = new double[9];   // 8  device damage stats D
    String quadrantMap = QUADRANT_ROW + QUADRANT_ROW + QUADRANT_ROW + QUADRANT_ROW + QUADRANT_ROW + QUADRANT_ROW + QUADRANT_ROW + leftStr(QUADRANT_ROW, 17);       // current quadrant map
    double stardate = toInt(random() * 20 + 20); // T
    int energy = 3000;   // E
    boolean shipDocked = false;  // D0
    int torpedoes = 10;     // P
    int shields = 0;      // S
    int missionDuration = 25 + toInt(random() * 10);    // T9 (mission duration in stardates)
    int basesInGalaxy = 0;   // B9
    int remainingKlingons;     // K7
    int klingonsInGalaxy = 0;   // K9
    int quadrantX;         // Q1
    int quadrantY;         // Q2
    int sectorX;         // S1
    int sectorY;         // S2
    int klingons = 0; //K3
    int starbases = 0;   // B3
    int stars = 0;   // S3
    int starbaseX = 0;   // X coordinate of starbase
    int starbaseY = 0; // Y coord of starbase
    double repairCost;  // damage repair cost D4
    boolean restart = false;

    // initial values
    final double initialStardate = stardate;   // T0
    final int initialEnergy = energy;      // E0
    final int initialTorpedoes = torpedoes;        // P0
    final int avgKlingonShieldEnergy = 200; // S9

    public static void main(String[] args) {
        final SuperStarTrekGame game = new SuperStarTrekGame();
        printBanner();
        while (true) {
            game.setup();
            game.enterNewQuadrant();
            game.restart = false;
            game.commandLoop();
        }
    }

    static void printBanner() {     // 220
        IntStream.range(1, 10).forEach(i -> {
            println("");
        });
        println(
                """
                                                            ,------*------,
                                            ,-------------   '---  ------'
                                             '-------- --'      / /
                                                 ,---' '-------/ /--,
                                                  '----------------'
                                                  
                                            THE USS ENTERPRISE --- NCC-1701"
                                
                        """
        );
    }

    double fnd(int i) { // 470
        return Math.sqrt((klingonQuadrants[i][1] - sectorX) ^ 2 + (klingonQuadrants[i][2] - sectorY) ^ 2);
    }

    static int fnr() {    // 475
        // Generate a random integer from 1 to 8 inclusive.
        return toInt(random() * 7 + 1);
    }

    void setup() {
        this.initEnterprisePosition();
        this.setupWhatExistsInTheGalaxy();
    }

    void initEnterprisePosition() {     // 480
        quadrantX = fnr();
        quadrantY = fnr();
        sectorX = fnr();
        sectorY = fnr();
        IntStream.range(1, 9).forEach(i -> {
            cardinalDirections[i][1] = 0;
            cardinalDirections[i][2] = 0;
        });
        cardinalDirections[3][1] = -1;
        cardinalDirections[2][1] = -1;
        cardinalDirections[4][1] = -1;
        cardinalDirections[4][2] = -1;
        cardinalDirections[5][2] = -1;
        cardinalDirections[6][2] = -1;
        cardinalDirections[1][2] = 1;
        cardinalDirections[2][2] = 1;
        cardinalDirections[6][1] = 1;
        cardinalDirections[7][1] = 1;
        cardinalDirections[8][1] = 1;
        cardinalDirections[8][2] = 1;
        cardinalDirections[9][2] = 1;
        IntStream.range(1, 8).forEach(i -> deviceStatus[i] = 0);
    }

    void setupWhatExistsInTheGalaxy() {     // 810
        // KLINGONS, STARBASES, STARS
        IntStream.range(1, 8).forEach(x -> {
            IntStream.range(1, 8).forEach(y -> {
                klingons = 0;
                chartedGalaxy[x][y] = 0;
                float random = random();
                if (random > .98) {
                    klingons = 3;
                    klingonsInGalaxy = +3;
                } else if (random > .95) {
                    klingons = 2;
                    klingonsInGalaxy = +2;
                } else if (random > .80) {
                    klingons = 1;
                    klingonsInGalaxy = +1;
                }
                starbases = 0;
                if (random() > .96) {
                    starbases = 1;
                    basesInGalaxy = +1;
                }
                galaxy[x][y] = klingons * 100 + starbases * 10 + fnr();
            });
        });
        if (klingonsInGalaxy > missionDuration) missionDuration = klingonsInGalaxy + 1;
        if (basesInGalaxy == 0) {
            if (galaxy[quadrantX][quadrantY] < 200) {
                galaxy[quadrantX][quadrantY] = galaxy[quadrantX][quadrantY] + 120;
                klingonsInGalaxy = +1;
            }
            basesInGalaxy = 1;
            galaxy[quadrantX][quadrantY] = +10;
            quadrantX = fnr();
            quadrantY = fnr();
        }
        remainingKlingons = klingonsInGalaxy;
        println("YOUR ORDERS ARE AS FOLLOWS:\n" +
                "     DESTROY THE " + klingonsInGalaxy + " KLINGON WARSHIP" + ((klingonsInGalaxy == 1) ? "" : "S") + " WHICH HAVE INVADED\n" +
                "   THE GALAXY BEFORE THEY CAN ATTACK FEDERATION HEADQUARTERS\n" +
                "   ON STARDATE " + initialStardate + missionDuration + "  THIS GIVES YOU " + missionDuration + " DAYS.  THERE " + ((basesInGalaxy == 1) ? "IS" : "ARE") + "\n" +
                "  " + basesInGalaxy + " STARBASE" + ((basesInGalaxy == 1) ? "" : "S") + " IN THE GALAXY FOR RESUPPLYING YOUR SHIP");
    }

    void enterNewQuadrant() {   // 1320
        // ANY TIME NEW QUADRANT ENTERED
        klingons = 0;
        starbases = 0;
        stars = 0;
        repairCost = .5 * random();
        chartedGalaxy[quadrantX][quadrantY] = galaxy[quadrantX][quadrantY];
        if (!(quadrantX < 1 || quadrantX > 8 || quadrantY < 1 || quadrantY > 8)) {
            final String quadrantName = getQuadrantName(false, quadrantX, quadrantY);
            if (initialStardate == stardate) {
                println("YOUR MISSION BEGINS WITH YOUR STARSHIP LOCATED\n" +
                        "IN THE GALACTIC QUADRANT, '" + quadrantName + "'.");
            } else {
                println("NOW ENTERING " + quadrantName + " QUADRANT . . .");
            }
            println("");
            klingons = (int) Math.round(galaxy[quadrantX][quadrantY] * .01);
            starbases = (int) Math.round(galaxy[quadrantX][quadrantY] * .1) - 10 * klingons;
            stars = galaxy[quadrantX][quadrantY] - 100 * klingons - 10 * starbases;
            if (klingons != 0) {
                println("COMBAT AREA      CONDITION RED");
                if (shields <= 200) {
                    println("   SHIELDS DANGEROUSLY LOW");
                }
            }
            IntStream.range(1, 3).forEach(i -> {
                klingonQuadrants[i][1] = 0;
                klingonQuadrants[i][2] = 0;
            });
        }
        IntStream.range(1, 3).forEach(i -> {
            klingonQuadrants[i][3] = 0;
        });
        // POSITION ENTERPRISE IN QUADRANT
        insertMarker(MARKER_ENTERPRISE, sectorX, sectorY);
        // position Klingons
        if (klingons >= 1) {
            for (int i = 1; i <= klingons; i++) {
                final int[] emptyCoordinate = findEmptyPlaceInQuadrant(quadrantMap);
                insertMarker(MARKER_KLINGON, emptyCoordinate[0], emptyCoordinate[1]);
                klingonQuadrants[i][1] = emptyCoordinate[0];
                klingonQuadrants[i][2] = emptyCoordinate[1];
                klingonQuadrants[i][3] = (int) Math.round(avgKlingonShieldEnergy * (0.5 + random()));
            }
        }
        // position Bases
        if (starbases >= 1) {
            final int[] emptyCoordinate = findEmptyPlaceInQuadrant(quadrantMap);
            starbaseX = emptyCoordinate[0];
            starbaseY = emptyCoordinate[1];
            insertMarker(MARKER_STARBASE, emptyCoordinate[0], emptyCoordinate[1]);
        }
        // position stars
        for (int i = 1; i <= stars; i++) {
            final int[] emptyCoordinate = findEmptyPlaceInQuadrant(quadrantMap);
            insertMarker(MARKER_STAR, emptyCoordinate[0], emptyCoordinate[1]);
        }
        shortRangeSensorScan(); // 1980
    }

    void commandLoop() {
        while (!this.restart) {
            checkShipEnergy();    // 1990
            String cmdStr = "";
            while ("".equals(cmdStr)) cmdStr = inputStr("COMMAND");
            boolean foundCommand = false;
            for (int i = 1; i <= 9; i++) {
                if (leftStr(cmdStr, 3).equals(midStr(COMMANDS, 3 * i - 2, 3))) {
                    switch (i) {
                        case COMMAND_NAV:
                            navigation();
                            foundCommand = true;
                            break;
                        case COMMAND_SRS:
                            shortRangeSensorScan();
                            foundCommand = true;
                            break;
                        case COMMAND_LRS:
                            longRangeSensorScan();
                            foundCommand = true;
                            break;
                        case COMMAND_PHA:
                            firePhasers();
                            foundCommand = true;
                            break;
                        case COMMAND_TOR:
                            firePhotonTorpedo();
                            foundCommand = true;
                            break;
                        case COMMAND_SHE:
                            shieldControl();
                            foundCommand = true;
                            break;
                        case COMMAND_DAM:
                            damageControl();
                            foundCommand = true;
                            break;
                        case COMMAND_COM:
                            libraryComputer();
                            foundCommand = true;
                            break;
                        case COMMAND_XXX:
                            endGameFail(false);
                            foundCommand = true;
                            break;
                        default:
                            printCommandOptions();
                            foundCommand = true;
                    }
                }
            }
            if (!foundCommand) printCommandOptions();
        }
    }

    void checkShipEnergy() {
        int totalEnergy = (shields + energy);
        if (totalEnergy < 10 && (energy <= 10 || deviceStatus[DEVICE_SHIELD_CONTROL] != 0)) {
            println("\n** FATAL ERROR **   YOU'VE JUST STRANDED YOUR SHIP IN ");
            println("SPACE");
            println("YOU HAVE INSUFFICIENT MANEUVERING ENERGY,");
            println(" AND SHIELD CONTROL");
            println("IS PRESENTLY INCAPABLE OF CROSS");
            println("-CIRCUITING TO ENGINE ROOM!!");
            endGameFail(false);
        }
    }

    void printCommandOptions() {
        println("ENTER ONE OF THE FOLLOWING:");
        println("  NAV  (TO SET COURSE)");
        println("  SRS  (FOR SHORT RANGE SENSOR SCAN)");
        println("  LRS  (FOR LONG RANGE SENSOR SCAN)");
        println("  PHA  (TO FIRE PHASERS)");
        println("  TOR  (TO FIRE PHOTON TORPEDOES)");
        println("  SHE  (TO RAISE OR LOWER SHIELDS)");
        println("  DAM  (FOR DAMAGE CONTROL REPORTS)");
        println("  COM  (TO CALL ON LIBRARY-COMPUTER)");
        println("  XXX  (TO RESIGN YOUR COMMAND)\n");
    }

    void navigation() {  // 2290
        float course = toInt(inputFloat("COURSE (0-9)"));
        if (course == 9) course = 1;
        if (course < 1 || course >= 9) {
            println("   LT. SULU REPORTS, 'INCORRECT COURSE DATA, SIR!'");
            return;
        }
        println("WARP FACTOR (0-" + ((deviceStatus[DEVICE_WARP_ENGINES] < 0) ? "0.2" : "8") + ")");
        float warp = inputFloat("");
        if (deviceStatus[DEVICE_WARP_ENGINES] < 0 && warp > .2) {
            // 2470
            println("WARP ENGINES ARE DAMAGED.  MAXIMUM SPEED = WARP 0.2");
            return;
        }
        if (warp == 0) return;
        if (warp > 0 && warp <= 8) {
            // 2490
            int n = toInt(warp * 8);
            if (energy - n >= 0) {
                klingonsMoveAndFire();
                repairDamagedDevices(course, warp, n);
                moveStarship(course, warp, n);
            } else {
                println("ENGINEERING REPORTS   'INSUFFICIENT ENERGY AVAILABLE");
                println("                       FOR MANEUVERING AT WARP" + warp + "!'");
                if (shields < n - energy || deviceStatus[DEVICE_SHIELD_CONTROL] < 0) return;
                println("DEFLECTOR CONTROL ROOM ACKNOWLEDGES" + shields + "UNITS OF ENERGY");
                println("                         PRESENTLY DEPLOYED TO SHIELDS.");
                return;
            }
        } else {
            println("   CHIEF ENGINEER SCOTT REPORTS 'THE ENGINES WON'T TAKE");
            println(" WARP " + warp + "!'");
            return;
        }
    }

    void klingonsMoveAndFire() { // 2590
        // KLINGONS MOVE/FIRE ON MOVING STARSHIP . . .
        for (int i = 1; i <= klingons; i++) {
            if (klingonQuadrants[i][3] == 0) continue;
            insertMarker(MARKER_EMPTY, klingonQuadrants[i][1], klingonQuadrants[i][2]);
            final int[] newCoords = findEmptyPlaceInQuadrant(quadrantMap);
            klingonQuadrants[i][1] = newCoords[0];
            klingonQuadrants[i][2] = newCoords[1];
            insertMarker(MARKER_KLINGON, klingonQuadrants[i][1], klingonQuadrants[i][2]);
        }
        klingonsShoot();
    }

    void repairDamagedDevices(final float course, final float warp, final int N) {
        // repair damaged devices and print damage report
        for (int i = 1; i <= 8; i++) {
            if (deviceStatus[i] < 0) {
                deviceStatus[i] += Math.min(warp, 1);
                if ((deviceStatus[i] > -.1) && (deviceStatus[i] < 0)) {
                    deviceStatus[i] = -.1;
                    break;
                } else if (deviceStatus[i] >= 0) {
                    println("DAMAGE CONTROL REPORT:  ");
                    println(tab(8) + printDeviceName(i) + " REPAIR COMPLETED.");
                }
            }
        }
        if (random() > .2) moveStarship(course, warp, N);  // 80% chance no damage nor repair
        int randomDevice = fnr();    // random device
        if (random() >= .6) {   // 40% chance of repair of random device
            deviceStatus[randomDevice] = deviceStatus[randomDevice] + random() * 3 + 1;
            println("DAMAGE CONTROL REPORT:  " + printDeviceName(randomDevice) + " STATE OF REPAIR IMPROVED\n");
        } else {            // 60% chance of damage of random device
            deviceStatus[randomDevice] = deviceStatus[randomDevice] - (random() * 5 + 1);   //
            println("DAMAGE CONTROL REPORT:  " + printDeviceName(randomDevice) + " DAMAGED");
        }
    }

    void moveStarship(final float course, final float warp, final int n) {    // 3070
        insertMarker(MARKER_EMPTY, toInt(sectorX), toInt(sectorY));
        int ic1 = toInt(course);
        float x1 = cardinalDirections[ic1][1] + (cardinalDirections[ic1 + 1][1] - cardinalDirections[ic1][1]) * (course - ic1);
        float x = sectorX;
        float y = sectorY;
        float x2 = cardinalDirections[ic1][2] + (cardinalDirections[ic1 + 1][2] - cardinalDirections[ic1][2]) * (course - ic1);
        final int initialQuadrantX = quadrantX;
        final int initialQuadrantY = quadrantY;
        for (int i = 1; i <= n; i++) {
            sectorX += x1;
            sectorY += x2;
            if (sectorX < 1 || sectorX >= 9 || sectorY < 1 || sectorY >= 9) {
                // exceeded quadrant limits
                x = 8 * quadrantX + x + n * x1;
                y = 8 * quadrantY + y + n * x2;
                quadrantX = toInt(x / 8);
                quadrantY = toInt(y / 8);
                sectorX = toInt(x - quadrantX * 8);
                sectorY = toInt(y - quadrantY * 8);
                if (sectorX == 0) {
                    quadrantX = quadrantX - 1;
                    sectorX = 8;
                }
                if (sectorY == 0) {
                    quadrantY = quadrantY - 1;
                    sectorY = 8;
                }
                boolean hitEdge = false;
                if (quadrantX < 1) {
                    hitEdge = true;
                    quadrantX = 1;
                    sectorX = 1;
                }
                if (quadrantX > 8) {
                    hitEdge = true;
                    quadrantX = 8;
                    sectorX = 8;
                }
                if (quadrantY < 1) {
                    hitEdge = true;
                    quadrantY = 8;
                    sectorY = 8;
                }
                if (quadrantY > 8) {
                    hitEdge = true;
                    quadrantY = 8;
                    sectorY = 8;
                }
                if (hitEdge) {
                    println("LT. UHURA REPORTS MESSAGE FROM STARFLEET COMMAND:");
                    println("  'PERMISSION TO ATTEMPT CROSSING OF GALACTIC PERIMETER");
                    println("  IS HEREBY *DENIED*.  SHUT DOWN YOUR ENGINES.'");
                    println("CHIEF ENGINEER SCOTT REPORTS  'WARP ENGINES SHUT DOWN");
                    println("  AT SECTOR " + sectorX + "," + sectorY + " OF QUADRANT " + quadrantX + "," + quadrantY + ".'");
                    if (stardate > initialStardate + missionDuration) endGameFail(false);
                }
                if (8 * quadrantX + quadrantY == 8 * initialQuadrantX + initialQuadrantY) {
                    break;
                }
                stardate += 1;
                maneuverEnergySR(n);
                enterNewQuadrant();
                return;
            } else {
                int S8 = toInt(sectorX) * 24 + toInt(sectorY) * 3 - 26; // S8 = pos
                if (!("  ".equals(midStr(quadrantMap, S8, 2)))) {
                    sectorX = toInt(sectorX - x1);
                    sectorY = toInt(sectorY - x2);
                    println("WARP ENGINES SHUT DOWN AT ");
                    println("SECTOR " + sectorX + "," + sectorY + " DUE TO BAD NAVIGATION");
                    break;
                }
            }
        }
        sectorX = toInt(sectorX);
        sectorY = toInt(sectorY);
        insertMarker(MARKER_ENTERPRISE, toInt(sectorX), toInt(sectorY));
        maneuverEnergySR(n);
        double stardateDelta = 1;
        if (warp < 1) stardateDelta = .1 * toInt(10 * warp);
        stardate = stardate + stardateDelta;
        if (stardate > initialStardate + missionDuration) endGameFail(false);
        shortRangeSensorScan();
    }

    void maneuverEnergySR(final int N) {  // 3910
        energy = energy - N - 10;
        if (energy >= 0) return;
        println("SHIELD CONTROL SUPPLIES ENERGY TO COMPLETE THE MANEUVER.");
        shields = shields + energy;
        energy = 0;
        if (shields <= 0) shields = 0;
    }

    void longRangeSensorScan() {    // 3390
        // LONG RANGE SENSOR SCAN CODE
        if (deviceStatus[DEVICE_LONG_RANGE_SENSORS] < 0) {
            println("LONG RANGE SENSORS ARE INOPERABLE");
            return;
        }
        println("LONG RANGE SCAN FOR QUADRANT " + quadrantX + "," + quadrantY);
        final String rowStr = "-------------------";
        println(rowStr);
        final int n[] = new int[4];
        for (int i = quadrantX - 1; i <= quadrantX + 1; i++) {
            n[1] = -1;
            n[2] = -2;
            n[3] = -3;
            for (int j = quadrantY - 1; j <= quadrantY + 1; j++) {
                if (i > 0 && i < 9 && j > 0 && j < 9) {
                    n[j - quadrantY + 2] = galaxy[i][j];
                    chartedGalaxy[i][j] = galaxy[i][j];
                }
            }
            for (int l = 1; l <= 3; l++) {
                print(": ");
                if (n[l] < 0) {
                    print("*** ");
                    continue;
                }
                print(": " + rightStr(Integer.toString(n[l] + 1000), 3) + " ");
            }
            println(": \n" + rowStr);
        }
    }

    void firePhasers() {    // 4260
        if (deviceStatus[DEVICE_PHASER_CONTROL] < 0) {
            println("PHASERS INOPERATIVE");
            return;
        }
        if (klingons <= 0) {
            printNoEnemyShipsMessage();
            return;
        }
        if (deviceStatus[DEVICE_LIBRARY_COMPUTER] < 0) println("COMPUTER FAILURE HAMPERS ACCURACY");
        println("PHASERS LOCKED ON TARGET;  ");
        int nrUnitsToFire;
        while (true) {
            println("ENERGY AVAILABLE = " + energy + " UNITS");
            nrUnitsToFire = toInt(inputFloat("NUMBER OF UNITS TO FIRE"));
            if (nrUnitsToFire <= 0) return;
            if (energy - nrUnitsToFire >= 0) break;
        }
        energy = energy - nrUnitsToFire;
        if (deviceStatus[DEVICE_SHIELD_CONTROL] < 0) nrUnitsToFire = toInt(nrUnitsToFire * random());
        int h1 = toInt(nrUnitsToFire / klingons);
        for (int i = 1; i <= 3; i++) {
            if (klingonQuadrants[i][3] <= 0) break;
            int hitPoints = toInt((h1 / fnd(0)) * (random() + 2));
            if (hitPoints <= .15 * klingonQuadrants[i][3]) {
                println("SENSORS SHOW NO DAMAGE TO ENEMY AT " + klingonQuadrants[i][1] + "," + klingonQuadrants[i][2]);
                continue;
            }
            klingonQuadrants[i][3] = klingonQuadrants[i][3] - hitPoints;
            println(hitPoints + " UNIT HIT ON KLINGON AT SECTOR " + klingonQuadrants[i][1] + "," + klingonQuadrants[i][2]);
            if (klingonQuadrants[i][3] <= 0) {
                println("*** KLINGON DESTROYED ***");
                klingons -= 1;
                klingonsInGalaxy -= 1;
                insertMarker(MARKER_EMPTY, klingonQuadrants[i][1], klingonQuadrants[i][2]);
                klingonQuadrants[i][3] = 0;
                galaxy[quadrantX][quadrantY] -= 100;
                chartedGalaxy[quadrantX][quadrantY] = galaxy[quadrantX][quadrantY];
                if (klingonsInGalaxy <= 0) endGameSuccess();
            } else {
                println("   (SENSORS SHOW" + klingonQuadrants[i][3] + "UNITS REMAINING)");
            }
        }
        klingonsShoot();
    }

    void printNoEnemyShipsMessage() {   // 4270
        println("SCIENCE OFFICER SPOCK REPORTS  'SENSORS SHOW NO ENEMY SHIPS");
        println("                                IN THIS QUADRANT'");
    }

    void firePhotonTorpedo() {  // 4700
        // PHOTON TORPEDO CODE BEGINS HERE
        if (torpedoes <= 0) {
            println("ALL PHOTON TORPEDOES EXPENDED");
            return;
        }
        if (deviceStatus[DEVICE_PHOTON_TUBES] < 0) {
            println("PHOTON TUBES ARE NOT OPERATIONAL");
        }
        float c1 = inputFloat("PHOTON TORPEDO COURSE (1-9)");
        if (c1 == 9) c1 = 1;
        if (c1 < 1 && c1 >= 9) {
            println("ENSIGN CHEKOV REPORTS,  'INCORRECT COURSE DATA, SIR!'");
            return;
        }
        int ic1 = toInt(c1);
        float x1 = cardinalDirections[ic1][1] + (cardinalDirections[ic1 + 1][1] - cardinalDirections[ic1][1]) * (c1 - ic1);
        energy = energy - 2;
        torpedoes = torpedoes - 1;
        float x2 = cardinalDirections[ic1][2] + (cardinalDirections[ic1 + 1][2] - cardinalDirections[ic1][2]) * (c1 - ic1);
        float x = sectorX;
        float y = sectorY;
        println("TORPEDO TRACK:");
        while (true) {
            x = x + x1;
            y = y + x2;
            int x3 = Math.round(x);
            int y3 = Math.round(y);
            if (x3 < 1 || x3 > 8 || y3 < 1 || y3 > 8) {
                println("TORPEDO MISSED"); // 5490
                klingonsShoot();
                return;
            }
            println("               " + x3 + "," + y3);
            if (compareMarker(quadrantMap, MARKER_EMPTY, toInt(x), toInt(y)))  {
                continue;
            } else if (compareMarker(quadrantMap, MARKER_KLINGON, toInt(x), toInt(y))) {
                println("*** KLINGON DESTROYED ***");
                klingons = klingons - 1;
                klingonsInGalaxy = klingonsInGalaxy - 1;
                if (klingonsInGalaxy <= 0) endGameSuccess();
                for (int i = 1; i <= 3; i++) {
                    if (x3 == klingonQuadrants[i][1] && y3 == klingonQuadrants[i][2]) break;
                }
                int i = 3;
                klingonQuadrants[i][3] = 0;
            } else if (compareMarker(quadrantMap, MARKER_STAR, toInt(x), toInt(y))) {
                println("STAR AT " + x3 + "," + y3 + " ABSORBED TORPEDO ENERGY.");
                klingonsShoot();
                return;
            } else if (compareMarker(quadrantMap, MARKER_STARBASE, toInt(x), toInt(y))) {
                println("*** STARBASE DESTROYED ***");
                starbases = starbases - 1;
                basesInGalaxy = basesInGalaxy - 1;
                if (basesInGalaxy == 0 && klingonsInGalaxy <= stardate - initialStardate - missionDuration) {
                    println("THAT DOES IT, CAPTAIN!!  YOU ARE HEREBY RELIEVED OF COMMAND");
                    println("AND SENTENCED TO 99 STARDATES AT HARD LABOR ON CYGNUS 12!!");
                    endGameFail(false);
                } else {
                    println("STARFLEET COMMAND REVIEWING YOUR RECORD TO CONSIDER");
                    println("COURT MARTIAL!");
                    shipDocked = false;
                }
            }
            insertMarker(MARKER_EMPTY, toInt(x), toInt(y));
            galaxy[quadrantX][quadrantY] = klingons * 100 + starbases * 10 + stars;
            chartedGalaxy[quadrantX][quadrantY] = galaxy[quadrantX][quadrantY];
            klingonsShoot();
        }
    }

    void shieldControl() {
        if (deviceStatus[DEVICE_SHIELD_CONTROL] < 0) {
            println("SHIELD CONTROL INOPERABLE");
            return;
        }
        println("ENERGY AVAILABLE = " + (energy + shields));
        int energyToShields = toInt(inputFloat("NUMBER OF UNITS TO SHIELDS"));
        if (energyToShields < 0 || shields == energyToShields) {
            println("<SHIELDS UNCHANGED>");
            return;
        }
        if (energyToShields > energy + energyToShields) {
            println("SHIELD CONTROL REPORTS  'THIS IS NOT THE FEDERATION TREASURY.'");
            println("<SHIELDS UNCHANGED>");
            return;
        }
        energy = energy + shields - energyToShields;
        shields = energyToShields;
        println("DEFLECTOR CONTROL ROOM REPORT:");
        println("  'SHIELDS NOW AT " + toInt(shields) + " UNITS PER YOUR COMMAND.'");
    }

    void shortRangeSensorScan() { // 6430
        // SHORT RANGE SENSOR SCAN & STARTUP SUBROUTINE
        boolean docked = false;
        String shipCondition; // ship condition (docked, red, yellow, green)
        for (int i = sectorX - 1; i <= sectorX + 1; i++) {
            for (int j = sectorY - 1; j <= sectorY + 1; j++) {
                if ((toInt(i) >= 1) && (toInt(i) <= 8) && (toInt(j) >= 1) && (toInt(j) <= 8)) {
                    if (compareMarker(quadrantMap, MARKER_STARBASE, i, j)) {
                        docked = true;
                    }
                }
            }
        }
        if (!docked) {
            shipDocked = false;
            if (klingons > 0) {
                shipCondition = "*RED*";
            } else {
                shipCondition = "GREEN";
                if (energy < initialEnergy * .1) {
                    shipCondition = "YELLOW";
                }
            }
        } else {
            shipDocked = true;
            shipCondition = "DOCKED";
            energy = initialEnergy;
            torpedoes = initialTorpedoes;
            println("SHIELDS DROPPED FOR DOCKING PURPOSES");
            shields = 0;
        }
        if (deviceStatus[DEVICE_SHORT_RANGE_SENSORS] < 0) { // are short range sensors out?
            println("\n*** SHORT RANGE SENSORS ARE OUT ***\n");
            return;
        }
        final String row = "---------------------------------";
        println(row);
        for (int i = 1; i <= 8; i++) {
            String sectorMapRow = "";
            for (int j = (i - 1) * 24 + 1; j <= (i - 1) * 24 + 22; j += 3) {
                sectorMapRow += " " + midStr(quadrantMap, j, 3);
            }
            switch (i) {
                case 1:
                    println(sectorMapRow + "        STARDATE           " + toInt(stardate * 10) * .1);
                    break;
                case 2:
                    println(sectorMapRow + "        CONDITION          " + shipCondition);
                    break;
                case 3:
                    println(sectorMapRow + "        QUADRANT           " + quadrantX + "," + quadrantY);
                    break;
                case 4:
                    println(sectorMapRow + "        SECTOR             " + sectorX + "," + sectorY);
                    break;
                case 5:
                    println(sectorMapRow + "        PHOTON TORPEDOES   " + toInt(torpedoes));
                    break;
                case 6:
                    println(sectorMapRow + "        TOTAL ENERGY       " + toInt((energy + shields)));
                    break;
                case 7:
                    println(sectorMapRow + "        SHIELDS            " + toInt(shields));
                    break;
                case 8:
                    println(sectorMapRow + "        KLINGONS REMAINING " + toInt(klingonsInGalaxy));
            }
            ;
        }
        println(row); // 7260
    }

    void libraryComputer() {    // 7290
        // REM LIBRARY COMPUTER CODE
        if (deviceStatus[DEVICE_LIBRARY_COMPUTER] < 0) {
            println("COMPUTER DISABLED");
            return;
        }
        while (true) {
            final float commandInput = inputFloat("COMPUTER ACTIVE AND AWAITING COMMAND");
            if (commandInput < 0) return;
            println("");
            int command = toInt(commandInput) + 1;
            if (command >= COMPUTER_COMMAND_CUMULATIVE_GALACTIC_RECORD && command <= COMPUTER_COMMAND_GALAXY_MAP) {
                switch (command) {
                    case COMPUTER_COMMAND_CUMULATIVE_GALACTIC_RECORD:
                        //GOTO 7540
                        cumulativeGalacticRecord(true);
                        return;
                    case COMPUTER_COMMAND_STATUS_REPORT:
                        //GOTO 7900
                        statusReport();
                        return;
                    case COMPUTER_COMMAND_PHOTON_TORPEDO_DATA:
                        //GOTO 8070
                        photonTorpedoData();
                        return;
                    case COMPUTER_COMMAND_STARBASE_NAV_DATA:
                        //GOTO 8500
                        starbaseNavData();
                        return;
                    case COMPUTER_COMMAND_DIR_DIST_CALC:
                        //GOTO 8150
                        directionDistanceCalculator();
                        return;
                    case COMPUTER_COMMAND_GALAXY_MAP:
                        //GOTO 7400
                        cumulativeGalacticRecord(false);
                        return;
                }
            } else {
                // invalid command
                println("FUNCTIONS AVAILABLE FROM LIBRARY-COMPUTER:");
                println("   0 = CUMULATIVE GALACTIC RECORD");
                println("   1 = STATUS REPORT");
                println("   2 = PHOTON TORPEDO DATA");
                println("   3 = STARBASE NAV DATA");
                println("   4 = DIRECTION/DISTANCE CALCULATOR");
                println("   5 = GALAXY 'REGION NAME' MAP");
                println("");
            }
        }
    }

    void cumulativeGalacticRecord(final boolean cumulativeReport) {   // 7540
        if (cumulativeReport) {
            println("");
            println("        ");
            println("COMPUTER RECORD OF GALAXY FOR QUADRANT " + quadrantX + "," + quadrantY);
            println("");
        } else {
            println("                        THE GALAXY");
        }
        println("       1     2     3     4     5     6     7     8");
        final String rowDivider = "     ----- ----- ----- ----- ----- ----- ----- -----";
        println(rowDivider);
        for (int i = 1; i <= 8; i++) {
            print(i + "  ");
            if (cumulativeReport) {
                int y = 1;
                String quadrantName = getQuadrantName(false, i, y);
                int tabLen = toInt(15 - .5 * strlen(quadrantName));
                println(tab(tabLen) + quadrantName);
                y = 5;
                quadrantName = getQuadrantName(false, i, y);
                tabLen = toInt(39 - .5 * strlen(quadrantName));
                println(tab(tabLen) + quadrantName);
            } else {
                for (int j = 1; j <= 8; j++) {
                    print("   ");
                    if (chartedGalaxy[i][j] == 0) {
                        print("***");
                    } else {
                        print(rightStr(Integer.toString(chartedGalaxy[i][j] + 1000), 3));
                    }
                }
            }
            println("");
            println(rowDivider);
        }
        println("");
    }

    void statusReport() {   // 7900
        println("   STATUS REPORT:");
        println("KLINGON" + ((klingonsInGalaxy > 1)? "S" : "")  + " LEFT: " + klingonsInGalaxy);
        println("MISSION MUST BE COMPLETED IN " + .1 * toInt((initialStardate + missionDuration - stardate) * 10) + "STARDATES");
        if (basesInGalaxy >= 1) {
            println("THE FEDERATION IS MAINTAINING " + basesInGalaxy + " STARBASE" + ((basesInGalaxy > 1)? "S" : "") + " IN THE GALAXY");
        } else {
            println("YOUR STUPIDITY HAS LEFT YOU ON YOUR OWN IN");
            println("  THE GALAXY -- YOU HAVE NO STARBASES LEFT!");
        }
        damageControl();
    }

    void photonTorpedoData() {  // 8070
        // TORPEDO, BASE NAV, D/D CALCULATOR
        if (klingons <= 0) {
            printNoEnemyShipsMessage();
            return;
        }
        println("FROM ENTERPRISE TO KLINGON BATTLE CRUISER" + ((klingons > 1)? "S" : ""));
        for (int i = 1; i <= 3; i++) {
            if (klingonQuadrants[i][3] > 0) {
                printDirection(sectorX, sectorY, klingonQuadrants[i][1], klingonQuadrants[i][2]);
            }
        }
    }

    void directionDistanceCalculator() {    // 8150
        println("DIRECTION/DISTANCE CALCULATOR:");
        println("YOU ARE AT QUADRANT " + quadrantX + "," + quadrantY + " SECTOR " + sectorX + "," + sectorY);
        print("PLEASE ENTER ");
        int[] initialCoords = inputCoords("  INITIAL COORDINATES (X,Y)");
        int[] finalCoords = inputCoords("  FINAL COORDINATES (X,Y)");
        printDirection(initialCoords[0], initialCoords[1], finalCoords[0], finalCoords[1]);
    }

    void printDirection(int from_x, int from_y, int to_x, int to_y) { // 8220
        to_y = to_y - from_y;  // delta 2
        from_y = from_x - to_x;    // delta 1
        if (to_y > 0) {
            if (from_y < 0) {
                from_x = 7;
            } else {
                from_x = 1;
                int tempA = from_y;
                from_y = to_y;
                to_y = tempA;
            }
        } else {
            if (from_y > 0) {
                from_x = 3;
            } else {
                from_x = 5;
                int tempA = from_y;
                from_y = to_y;
                to_y = tempA;
            }
        }

        from_y = Math.abs(from_y);
        to_y = Math.abs(to_y);

        if (from_y > 0 || to_y > 0) {
            if (from_y >= to_y) {
                println("DIRECTION = " + (from_x + to_y / from_y));
            } else {
                println("DIRECTION = " + (from_x + 2 - to_y / from_y));
            }
        }
        println("DISTANCE = " + round(Math.sqrt(to_y ^ 2 + from_y ^ 2), 6));
    }

    void starbaseNavData() {    // 8500
        if (starbases != 0) {
            println("FROM ENTERPRISE TO STARBASE:");
            printDirection(sectorX, sectorY, starbaseX, starbaseY);
        } else {
            println("MR. SPOCK REPORTS,  'SENSORS SHOW NO STARBASES IN THIS");
            println(" QUADRANT.'");
        }
    }

    /**
     * Finds random empty coordinates in a quadrant.
     *
     * @param quadrantString
     * @return an array with a pair of coordinates x, y
     */
    int[] findEmptyPlaceInQuadrant(String quadrantString) {   // 8590
        final int x = fnr();
        final int y = fnr();
        if (!compareMarker(quadrantString, MARKER_EMPTY, x, y)) {
            return findEmptyPlaceInQuadrant(quadrantString);
        }
        return new int[]{x, y};
    }


    void insertMarker(final String marker, final int x, final int y) {   // 8670
        final int pos = toInt(y) * 3 + toInt(x) * 24 + 1;
        if (marker.length() != 3) {
            System.err.println("ERROR");
            System.exit(-1);
        }
        if (pos == 1) {
            quadrantMap = marker + rightStr(quadrantMap, 189);
        }
        if (pos == 190) {
            quadrantMap = leftStr(quadrantMap, 189) + marker;
        }
        quadrantMap = leftStr(quadrantMap, (pos - 1)) + marker + rightStr(quadrantMap, (190 - pos));
    }

    String printDeviceName(final int deviceNumber) {  // 8790
        // PRINTS DEVICE NAME
        switch (deviceNumber) {
            case DEVICE_WARP_ENGINES:
                return "WARP ENGINES";
            case DEVICE_SHORT_RANGE_SENSORS:
                return "SHORT RANGE SENSORS";
            case DEVICE_LONG_RANGE_SENSORS:
                return "LONG RANGE SENSORS";
            case DEVICE_PHASER_CONTROL:
                return "PHASER CONTROL";
            case DEVICE_PHOTON_TUBES:
                return "PHOTON TUBES";
            case DEVICE_DAMAGE_CONTROL:
                return "DAMAGE CONTROL";
            case DEVICE_SHIELD_CONTROL:
                return "SHIELD CONTROL";
            case DEVICE_LIBRARY_COMPUTER:
                return "LIBRARY-COMPUTER";
        }
        return "";
    }

    boolean compareMarker(final String quadrantString, final String marker, int x, int y) { // 8830
        final int markerRegion = (y - 1) * 3 + (x - 1) * 24 + 1;
        if (midStr(quadrantString, markerRegion, 3).equals(marker)) {
            return true;
        }
        return false;
    }

    String getRegionName(final boolean regionNameOnly, final int y) {
        if (!regionNameOnly) {
            switch (y % 4) {
                case 1:
                    return " I";
                case 2:
                    return " II";
                case 3:
                    return " III";
                case 4:
                    return " IV";
            }
        }
        return "";
    }

    String getQuadrantName(final boolean regionNameOnly, final int x, final int y) { // 9030
        if (y <= 4) {
            switch (x) {
                case 1:
                    return "ANTARES" + getRegionName(regionNameOnly, y);
                case 2:
                    return "RIGEL" + getRegionName(regionNameOnly, y);
                case 3:
                    return "PROCYON" + getRegionName(regionNameOnly, y);
                case 4:
                    return "VEGA" + getRegionName(regionNameOnly, y);
                case 5:
                    return "CANOPUS" + getRegionName(regionNameOnly, y);
                case 6:
                    return "ALTAIR" + getRegionName(regionNameOnly, y);
                case 7:
                    return "SAGITTARIUS" + getRegionName(regionNameOnly, y);
                case 8:
                    return "POLLUX" + getRegionName(regionNameOnly, y);
            }
        } else {
            switch (x) {
                case 1:
                    return "SIRIUS" + getRegionName(regionNameOnly, y);
                case 2:
                    return "DENEB" + getRegionName(regionNameOnly, y);
                case 3:
                    return "CAPELLA" + getRegionName(regionNameOnly, y);
                case 4:
                    return "BETELGEUSE" + getRegionName(regionNameOnly, y);
                case 5:
                    return "ALDEBARAN" + getRegionName(regionNameOnly, y);
                case 6:
                    return "REGULUS" + getRegionName(regionNameOnly, y);
                case 7:
                    return "ARCTURUS" + getRegionName(regionNameOnly, y);
                case 8:
                    return "SPICA" + getRegionName(regionNameOnly, y);
            }
        }
        return "UNKNOWN - ERROR";
    }

    void damageControl() {  // 5690
        if (deviceStatus[DEVICE_DAMAGE_CONTROL] < 0) {
            println("DAMAGE CONTROL REPORT NOT AVAILABLE");
        } else {
            println("\nDEVICE             STATE OF REPAIR");
            for (int deviceNr = 1; deviceNr <= 8; deviceNr++) {
                print(printDeviceName(deviceNr) + leftStr(QUADRANT_ROW, 25 - strlen(printDeviceName(deviceNr))) + " " + toInt(deviceStatus[deviceNr] * 100) * .01 + "\n");
            }
        }
        if (!shipDocked) return;

        double deltaToRepair = 0;
        for (int i = 1; i <= 8; i++) {
            if (deviceStatus[i] < 0) deltaToRepair += .1;
        }
        if (deltaToRepair > 0) {
            deltaToRepair += repairCost;
            if (deltaToRepair >= 1) deltaToRepair = .9;
            println("TECHNICIANS STANDING BY TO EFFECT REPAIRS TO YOUR SHIP;");
            println("ESTIMATED TIME TO REPAIR:'" + .01 * toInt(100 * deltaToRepair) + " STARDATES");
            final String reply = inputStr("WILL YOU AUTHORIZE THE REPAIR ORDER (Y/N)");
            if ("Y".equals(reply)) {
                for (int deviceNr = 1; deviceNr <= 8; deviceNr++) {
                    if (deviceStatus[deviceNr] < 0) deviceStatus[deviceNr] = 0;
                }
                stardate = stardate + deltaToRepair + .1;
            }
        }
    }

    void klingonsShoot() {   // 6000
        if (klingons <= 0) return; // no klingons
        if (shipDocked) {  // enterprise is docked
            println("STARBASE SHIELDS PROTECT THE ENTERPRISE");
            return;
        }
        for (int i = 1; i <= 3; i++) {
            if (klingonQuadrants[i][3] <= 0) continue;
            int hits = toInt((klingonQuadrants[i][3] / fnd(1)) * (2 + random())); // hit points
            shields = shields - hits;
            klingonQuadrants[i][3] = toInt(klingonQuadrants[i][3] / (3 + random()));      // FIXME: RND(0)
            println(hits + " UNIT HIT ON ENTERPRISE FROM SECTOR " + klingonQuadrants[i][1] + "," + klingonQuadrants[i][2]);
            if (shields <= 0) endGameFail(true);
            println("      <SHIELDS DOWN TO " + shields + " UNITS>");
            if (hits < 20) continue;
            if ((random() > .6) || (hits / shields <= .02)) continue;
            int randomDevice = fnr();
            deviceStatus[randomDevice] = deviceStatus[randomDevice] - hits / shields - .5 * random();
            println("DAMAGE CONTROL REPORTS " + printDeviceName(randomDevice) + " DAMAGED BY THE HIT'");
        }
    }

    void endGameFail(final boolean enterpriseDestroyed) {    // 6220
        if (enterpriseDestroyed) {
            println("\nTHE ENTERPRISE HAS BEEN DESTROYED.  THEN FEDERATION ");
            println("WILL BE CONQUERED");
        }
        println("\nIT IS STARDATE " + stardate);
        println("THERE WERE " + klingonsInGalaxy + " KLINGON BATTLE CRUISERS LEFT AT");
        println("THE END OF YOUR MISSION.");
        repeatGame();
    }

    void endGameSuccess() { // 6370
        println("CONGRATULATION, CAPTAIN!  THE LAST KLINGON BATTLE CRUISER");
        println("MENACING THE FEDERATION HAS BEEN DESTROYED.\n");
        println("YOUR EFFICIENCY RATING IS " + (Math.sqrt(1000 * (remainingKlingons / (stardate - initialStardate)))));
        repeatGame();
    }

    void repeatGame() {// 6290
        println("\n");
        if (basesInGalaxy != 0) {
            println("THE FEDERATION IS IN NEED OF A NEW STARSHIP COMMANDER");
            println("FOR A SIMILAR MISSION -- IF THERE IS A VOLUNTEER,");
            final String reply = inputStr("LET HIM STEP FORWARD AND ENTER 'AYE'");
            if ("AYE".equals(reply)) {
                this.restart = true;
            } else {
                System.exit(0);
            }
        }
    }

    static int toInt(final double num) {
        int x = (int) Math.floor(num);
        if (x < 0) x *= -1;
        return x;
    }

    static void println(final String s) {
        System.out.println(s);
    }

    static void print(final String s) {
        System.out.print(s);
    }

    static String tab(final int n) {
        return IntStream.range(1, n).mapToObj(num -> " ").collect(Collectors.joining());
    }

    static int strlen(final String s) {
        return s.length();
    }

    static String inputStr(final String message) {
        System.out.print(message + "? ");
        final BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
        try {
            return reader.readLine();
        } catch (IOException ioe) {
            ioe.printStackTrace();
            return "";
        }
    }

    static int[] inputCoords(final String message) {
        while (true) {
            final String input = inputStr(message);
            try {
                final String[] splitInput = input.split(",");
                if (splitInput.length == 2) {
                    int x = Integer.parseInt(splitInput[0]);
                    int y = Integer.parseInt(splitInput[0]);
                    return new int[]{x, y};
                }
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    static float inputFloat(final String message) {
        while (true) {
            System.out.print(message + "? ");
            final BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
            try {
                final String input = reader.readLine();
                if (input.length() > 0) {
                    return Float.parseFloat(input);
                }
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    static String leftStr(final String input, final int len) {
        if (input == null || input.length() < len) return input;
        return input.substring(0, len);
    }

    static String midStr(final String input, final int start, final int len) {
        if (input == null || input.length() < ((start - 1) + len)) return input;
        return input.substring(start - 1, (start - 1) + len);
    }

    static String rightStr(final String input, final int len) {
        if (input == null || input.length() < len) return "";
        return input.substring(input.length() - len);
    }

    static float random() {
        return random.nextFloat();
    }

    private static double round(double value, int places) {
        if (places < 0) throw new IllegalArgumentException();
        BigDecimal bd = new BigDecimal(Double.toString(value));
        bd = bd.setScale(places, RoundingMode.HALF_UP);
        return bd.doubleValue();
    }
}
