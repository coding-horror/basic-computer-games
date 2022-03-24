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
 * Ported to Java in Jan-Mar 2022 by
 * Taciano Dreckmann Perez (taciano.perez@gmail.com)
 */
public class SuperStarTrekGame implements GameCallback {

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

    // other constants
    static final String COMMANDS = "NAVSRSLRSPHATORSHEDAMCOMXXX";

    // game state
    final GalaxyMap galaxyMap = new GalaxyMap();
    double stardate = Util.toInt(Util.random() * 20 + 20);
    int missionDuration = Math.max((25 + Util.toInt(Util.random() * 10)), galaxyMap.getKlingonsInGalaxy()+1);    // T9 (mission duration in stardates)
    boolean restart = false;

    // initial values
    final double initialStardate = stardate;

    public static void main(String[] args) {
        final SuperStarTrekGame game = new SuperStarTrekGame();
        printBanner();
        while (true) {
            game.orders();
            game.enterNewQuadrant();
            game.restart = false;
            game.commandLoop();
        }
    }

    static void printBanner() {
        IntStream.range(1, 10).forEach(i -> {
            Util.println("");
        });
        Util.println(
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

    void orders() {
        Util.println("YOUR ORDERS ARE AS FOLLOWS:\n" +
                "     DESTROY THE " + galaxyMap.getKlingonsInGalaxy() + " KLINGON WARSHIP" + ((galaxyMap.getKlingonsInGalaxy() == 1) ? "" : "S") + " WHICH HAVE INVADED\n" +
                "   THE GALAXY BEFORE THEY CAN ATTACK FEDERATION HEADQUARTERS\n" +
                "   ON STARDATE " + initialStardate + missionDuration + "  THIS GIVES YOU " + missionDuration + " DAYS.  THERE " + ((galaxyMap.getBasesInGalaxy() == 1) ? "IS" : "ARE") + "\n" +
                "  " + galaxyMap.getBasesInGalaxy() + " STARBASE" + ((galaxyMap.getBasesInGalaxy() == 1) ? "" : "S") + " IN THE GALAXY FOR RESUPPLYING YOUR SHIP");
    }

    public void enterNewQuadrant() {
        galaxyMap.newQuadrant(stardate, initialStardate);
        shortRangeSensorScan();
    }

    void commandLoop() {
        while (!this.restart) {
            checkShipEnergy();
            String cmdStr = "";
            while ("".equals(cmdStr)) cmdStr = Util.inputStr("COMMAND");
            boolean foundCommand = false;
            for (int i = 1; i <= 9; i++) {
                if (Util.leftStr(cmdStr, 3).equals(Util.midStr(COMMANDS, 3 * i - 2, 3))) {
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
                            galaxyMap.getEnterprise().damageControl(this);
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
        final Enterprise enterprise = galaxyMap.getEnterprise();
        if (enterprise.getTotalEnergy() < 10 && (enterprise.getEnergy() <= 10 || enterprise.getDeviceStatus()[Enterprise.DEVICE_SHIELD_CONTROL] != 0)) {
            Util.println("\n** FATAL ERROR **   YOU'VE JUST STRANDED YOUR SHIP IN ");
            Util.println("SPACE");
            Util.println("YOU HAVE INSUFFICIENT MANEUVERING ENERGY,");
            Util.println(" AND SHIELD CONTROL");
            Util.println("IS PRESENTLY INCAPABLE OF CROSS");
            Util.println("-CIRCUITING TO ENGINE ROOM!!");
            endGameFail(false);
        }
    }

    void printCommandOptions() {
        Util.println("ENTER ONE OF THE FOLLOWING:");
        Util.println("  NAV  (TO SET COURSE)");
        Util.println("  SRS  (FOR SHORT RANGE SENSOR SCAN)");
        Util.println("  LRS  (FOR LONG RANGE SENSOR SCAN)");
        Util.println("  PHA  (TO FIRE PHASERS)");
        Util.println("  TOR  (TO FIRE PHOTON TORPEDOES)");
        Util.println("  SHE  (TO RAISE OR LOWER SHIELDS)");
        Util.println("  DAM  (FOR DAMAGE CONTROL REPORTS)");
        Util.println("  COM  (TO CALL ON LIBRARY-COMPUTER)");
        Util.println("  XXX  (TO RESIGN YOUR COMMAND)\n");
    }

    void navigation() {
        float course = Util.toInt(Util.inputFloat("COURSE (0-9)"));
        if (course == 9) course = 1;
        if (course < 1 || course >= 9) {
            Util.println("   LT. SULU REPORTS, 'INCORRECT COURSE DATA, SIR!'");
            return;
        }
        final Enterprise enterprise = galaxyMap.getEnterprise();
        final double[] deviceStatus = enterprise.getDeviceStatus();
        Util.println("WARP FACTOR (0-" + ((deviceStatus[Enterprise.DEVICE_WARP_ENGINES] < 0) ? "0.2" : "8") + ")");
        float warp = Util.inputFloat("");
        if (deviceStatus[Enterprise.DEVICE_WARP_ENGINES] < 0 && warp > .2) {
            Util.println("WARP ENGINES ARE DAMAGED.  MAXIMUM SPEED = WARP 0.2");
            return;
        }
        if (warp == 0) return;
        if (warp > 0 && warp <= 8) {
            int n = Util.toInt(warp * 8);
            if (enterprise.getEnergy() - n >= 0) {
                galaxyMap.klingonsMoveAndFire(this);
                repairDamagedDevices(course, warp, n);
                galaxyMap.moveEnterprise(course, warp, n, stardate, initialStardate, missionDuration, this);
            } else {
                Util.println("ENGINEERING REPORTS   'INSUFFICIENT ENERGY AVAILABLE");
                Util.println("                       FOR MANEUVERING AT WARP " + warp + "!'");
                if (enterprise.getShields() < n - enterprise.getEnergy() || deviceStatus[Enterprise.DEVICE_SHIELD_CONTROL] < 0) return;
                Util.println("DEFLECTOR CONTROL ROOM ACKNOWLEDGES " + enterprise.getShields() + " UNITS OF ENERGY");
                Util.println("                         PRESENTLY DEPLOYED TO SHIELDS.");
            }
        } else {
            Util.println("   CHIEF ENGINEER SCOTT REPORTS 'THE ENGINES WON'T TAKE");
            Util.println(" WARP " + warp + "!'");
        }
    }

    void repairDamagedDevices(final float course, final float warp, final int N) {
        final Enterprise enterprise = galaxyMap.getEnterprise();
        // repair damaged devices and print damage report
        enterprise.repairDamagedDevices(warp);
        if (Util.random() > .2) return;  // 80% chance no damage nor repair
        int randomDevice = Util.fnr();    // random device
        final double[] deviceStatus = enterprise.getDeviceStatus();
        if (Util.random() >= .6) {   // 40% chance of repair of random device
            enterprise.setDeviceStatus(randomDevice, deviceStatus[randomDevice] + Util.random() * 3 + 1);
            Util.println("DAMAGE CONTROL REPORT:  " + Enterprise.printDeviceName(randomDevice) + " STATE OF REPAIR IMPROVED\n");
        } else {            // 60% chance of damage of random device
            enterprise.setDeviceStatus(randomDevice, deviceStatus[randomDevice] - (Util.random() * 5 + 1));
            Util.println("DAMAGE CONTROL REPORT:  " + Enterprise.printDeviceName(randomDevice) + " DAMAGED");
        }
    }

    void longRangeSensorScan() {
        // LONG RANGE SENSOR SCAN CODE
        galaxyMap.longRangeSensorScan();
    }

    void firePhasers() {
        galaxyMap.firePhasers(this);
    }

    void firePhotonTorpedo() {
        galaxyMap.firePhotonTorpedo(stardate, initialStardate, missionDuration, this);
    }

    void shieldControl() {
        galaxyMap.getEnterprise().shieldControl();
    }

    void shortRangeSensorScan() {
        // SHORT RANGE SENSOR SCAN & STARTUP SUBROUTINE
        galaxyMap.shortRangeSensorScan(stardate);
    }

    void libraryComputer() {
        // REM LIBRARY COMPUTER CODE
        if (galaxyMap.getEnterprise().getDeviceStatus()[Enterprise.DEVICE_LIBRARY_COMPUTER] < 0) {
            Util.println("COMPUTER DISABLED");
            return;
        }
        while (true) {
            final float commandInput = Util.inputFloat("COMPUTER ACTIVE AND AWAITING COMMAND");
            if (commandInput < 0) return;
            Util.println("");
            int command = Util.toInt(commandInput) + 1;
            if (command >= COMPUTER_COMMAND_CUMULATIVE_GALACTIC_RECORD && command <= COMPUTER_COMMAND_GALAXY_MAP) {
                switch (command) {
                    case COMPUTER_COMMAND_CUMULATIVE_GALACTIC_RECORD:
                        galaxyMap.cumulativeGalacticRecord(true);
                        return;
                    case COMPUTER_COMMAND_STATUS_REPORT:
                        statusReport();
                        return;
                    case COMPUTER_COMMAND_PHOTON_TORPEDO_DATA:
                        galaxyMap.photonTorpedoData();
                        return;
                    case COMPUTER_COMMAND_STARBASE_NAV_DATA:
                        galaxyMap.starbaseNavData();
                        return;
                    case COMPUTER_COMMAND_DIR_DIST_CALC:
                        galaxyMap.directionDistanceCalculator();
                        return;
                    case COMPUTER_COMMAND_GALAXY_MAP:
                        galaxyMap.cumulativeGalacticRecord(false);
                        return;
                }
            } else {
                // invalid command
                Util.println("FUNCTIONS AVAILABLE FROM LIBRARY-COMPUTER:");
                Util.println("   0 = CUMULATIVE GALACTIC RECORD");
                Util.println("   1 = STATUS REPORT");
                Util.println("   2 = PHOTON TORPEDO DATA");
                Util.println("   3 = STARBASE NAV DATA");
                Util.println("   4 = DIRECTION/DISTANCE CALCULATOR");
                Util.println("   5 = GALAXY 'REGION NAME' MAP");
                Util.println("");
            }
        }
    }

    void statusReport() {
        Util.println("   STATUS REPORT:");
        Util.println("KLINGON" + ((galaxyMap.getKlingonsInGalaxy() > 1)? "S" : "")  + " LEFT: " + galaxyMap.getKlingonsInGalaxy());
        Util.println("MISSION MUST BE COMPLETED IN " + .1 * Util.toInt((initialStardate + missionDuration - stardate) * 10) + " STARDATES");
        if (galaxyMap.getBasesInGalaxy() >= 1) {
            Util.println("THE FEDERATION IS MAINTAINING " + galaxyMap.getBasesInGalaxy() + " STARBASE" + ((galaxyMap.getBasesInGalaxy() > 1)? "S" : "") + " IN THE GALAXY");
        } else {
            Util.println("YOUR STUPIDITY HAS LEFT YOU ON YOUR OWN IN");
            Util.println("  THE GALAXY -- YOU HAVE NO STARBASES LEFT!");
        }
        galaxyMap.getEnterprise().damageControl(this);
    }

    public void incrementStardate(double increment) {
        this.stardate += increment;
    }

    public void endGameFail(final boolean enterpriseDestroyed) {    // 6220
        if (enterpriseDestroyed) {
            Util.println("\nTHE ENTERPRISE HAS BEEN DESTROYED.  THEN FEDERATION ");
            Util.println("WILL BE CONQUERED");
        }
        Util.println("\nIT IS STARDATE " + stardate);
        Util.println("THERE WERE " + galaxyMap.getKlingonsInGalaxy() + " KLINGON BATTLE CRUISERS LEFT AT");
        Util.println("THE END OF YOUR MISSION.");
        repeatGame();
    }

    public void endGameSuccess() {
        Util.println("CONGRATULATION, CAPTAIN!  THE LAST KLINGON BATTLE CRUISER");
        Util.println("MENACING THE FEDERATION HAS BEEN DESTROYED.\n");
        Util.println("YOUR EFFICIENCY RATING IS " + (Math.sqrt(1000 * (galaxyMap.getRemainingKlingons() / (stardate - initialStardate)))));
        repeatGame();
    }

    void repeatGame() {
        Util.println("\n");
        if (galaxyMap.getBasesInGalaxy() != 0) {
            Util.println("THE FEDERATION IS IN NEED OF A NEW STARSHIP COMMANDER");
            Util.println("FOR A SIMILAR MISSION -- IF THERE IS A VOLUNTEER,");
            final String reply = Util.inputStr("LET HIM STEP FORWARD AND ENTER 'AYE'");
            if ("AYE".equals(reply)) {
                this.restart = true;
            } else {
                System.exit(0);
            }
        }
    }

}
