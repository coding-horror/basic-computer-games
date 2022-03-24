import java.util.stream.IntStream;

/**
 * Map of the galaxy divided in Quadrants and Sectors,
 * populated with stars, starbases, klingons, and the Enterprise.
 */
public class GalaxyMap {

    // markers
    static final String MARKER_EMPTY = "   ";
    static final String MARKER_ENTERPRISE = "<*>";
    static final String MARKER_KLINGON = "+K+";
    static final String MARKER_STARBASE = ">!<";
    static final String MARKER_STAR = " * ";

    static final int AVG_KLINGON_SHIELD_ENERGY = 200;

    // galaxy map
    public static final String QUADRANT_ROW = "                         ";
    String quadrantMap = QUADRANT_ROW + QUADRANT_ROW + QUADRANT_ROW + QUADRANT_ROW + QUADRANT_ROW + QUADRANT_ROW + QUADRANT_ROW + Util.leftStr(QUADRANT_ROW, 17);       // current quadrant map
    final int[][] galaxy = new int[9][9];    // 8x8 galaxy map G
    final int[][] klingonQuadrants = new int[4][4];    // 3x3 position of klingons K
    final int[][] chartedGalaxy = new int[9][9];    // 8x8 charted galaxy map Z

    // galaxy state
    int basesInGalaxy = 0;
    int remainingKlingons;
    int klingonsInGalaxy = 0;
    final Enterprise enterprise = new Enterprise();

    // quadrant state
    int klingons = 0;
    int starbases = 0;
    int stars = 0;
    int starbaseX = 0; // X coordinate of starbase
    int starbaseY = 0; // Y coord of starbase

    public Enterprise getEnterprise() {
        return enterprise;
    }

    public int getBasesInGalaxy() {
        return basesInGalaxy;
    }

    public int getRemainingKlingons() {
        return remainingKlingons;
    }

    public int getKlingonsInGalaxy() {
        return klingonsInGalaxy;
    }

    double fnd(int i) {
        return Math.sqrt((klingonQuadrants[i][1] - enterprise.getSector()[Enterprise.COORD_X]) ^ 2 + (klingonQuadrants[i][2] - enterprise.getSector()[Enterprise.COORD_Y]) ^ 2);
    }

    public GalaxyMap() {
        int quadrantX = enterprise.getQuadrant()[Enterprise.COORD_X];
        int quadrantY = enterprise.getQuadrant()[Enterprise.COORD_Y];
        // populate Klingons, Starbases, Stars
        IntStream.range(1, 8).forEach(x -> {
            IntStream.range(1, 8).forEach(y -> {
                klingons = 0;
                chartedGalaxy[x][y] = 0;
                float random = Util.random();
                if (random > .98) {
                    klingons = 3;
                    klingonsInGalaxy += 3;
                } else if (random > .95) {
                    klingons = 2;
                    klingonsInGalaxy += 2;
                } else if (random > .80) {
                    klingons = 1;
                    klingonsInGalaxy += 1;
                }
                starbases = 0;
                if (Util.random() > .96) {
                    starbases = 1;
                    basesInGalaxy = +1;
                }
                galaxy[x][y] = klingons * 100 + starbases * 10 + Util.fnr();
            });
        });
        if (basesInGalaxy == 0) {
            if (galaxy[quadrantX][quadrantY] < 200) {
                galaxy[quadrantX][quadrantY] = galaxy[quadrantX][quadrantY] + 120;
                klingonsInGalaxy = +1;
            }
            basesInGalaxy = 1;
            galaxy[quadrantX][quadrantY] = +10;
            enterprise.setQuadrant(new int[]{ Util.fnr(), Util.fnr() });
        }
        remainingKlingons = klingonsInGalaxy;
    }

    void newQuadrant(final double stardate, final double initialStardate) {   // 1320
        final int quadrantX = enterprise.getQuadrant()[Enterprise.COORD_X];
        final int quadrantY = enterprise.getQuadrant()[Enterprise.COORD_Y];
        klingons = 0;
        starbases = 0;
        stars = 0;
        enterprise.randomRepairCost();
        chartedGalaxy[quadrantX][quadrantY] = galaxy[quadrantX][quadrantY];
        if (!(quadrantX < 1 || quadrantX > 8 || quadrantY < 1 || quadrantY > 8)) {
            final String quadrantName = getQuadrantName(false, quadrantX, quadrantY);
            if (initialStardate == stardate) {
                Util.println("YOUR MISSION BEGINS WITH YOUR STARSHIP LOCATED\n" +
                        "IN THE GALACTIC QUADRANT, '" + quadrantName + "'.");
            } else {
                Util.println("NOW ENTERING " + quadrantName + " QUADRANT . . .");
            }
            Util.println("");
            klingons = (int) Math.round(galaxy[quadrantX][quadrantY] * .01);
            starbases = (int) Math.round(galaxy[quadrantX][quadrantY] * .1) - 10 * klingons;
            stars = galaxy[quadrantX][quadrantY] - 100 * klingons - 10 * starbases;
            if (klingons != 0) {
                Util.println("COMBAT AREA      CONDITION RED");
                if (enterprise.getShields() <= 200) {
                    Util.println("   SHIELDS DANGEROUSLY LOW");
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
        // position enterprise in quadrant
        insertMarker(MARKER_ENTERPRISE, enterprise.getSector()[Enterprise.COORD_X], enterprise.getSector()[Enterprise.COORD_Y]);
        // position klingons
        if (klingons >= 1) {
            for (int i = 1; i <= klingons; i++) {
                final int[] emptyCoordinate = findEmptyPlaceInQuadrant(quadrantMap);
                insertMarker(MARKER_KLINGON, emptyCoordinate[0], emptyCoordinate[1]);
                klingonQuadrants[i][1] = emptyCoordinate[0];
                klingonQuadrants[i][2] = emptyCoordinate[1];
                klingonQuadrants[i][3] = (int) Math.round(AVG_KLINGON_SHIELD_ENERGY * (0.5 + Util.random()));
            }
        }
        // position bases
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
    }

    public void klingonsMoveAndFire(GameCallback callback) {
        for (int i = 1; i <= klingons; i++) {
            if (klingonQuadrants[i][3] == 0) continue;
            insertMarker(MARKER_EMPTY, klingonQuadrants[i][1], klingonQuadrants[i][2]);
            final int[] newCoords = findEmptyPlaceInQuadrant(quadrantMap);
            klingonQuadrants[i][1] = newCoords[0];
            klingonQuadrants[i][2] = newCoords[1];
            insertMarker(MARKER_KLINGON, klingonQuadrants[i][1], klingonQuadrants[i][2]);
        }
        klingonsShoot(callback);
    }

    void klingonsShoot(GameCallback callback) {
        if (klingons <= 0) return; // no klingons
        if (enterprise.isDocked()) {
            Util.println("STARBASE SHIELDS PROTECT THE ENTERPRISE");
            return;
        }
        for (int i = 1; i <= 3; i++) {
            if (klingonQuadrants[i][3] <= 0) continue;
            int hits = Util.toInt((klingonQuadrants[i][3] / fnd(1)) * (2 + Util.random()));
            enterprise.sufferHitPoints(hits);
            klingonQuadrants[i][3] = Util.toInt(klingonQuadrants[i][3] / (3 + Util.random()));
            Util.println(hits + " UNIT HIT ON ENTERPRISE FROM SECTOR " + klingonQuadrants[i][1] + "," + klingonQuadrants[i][2]);
            if (enterprise.getShields() <= 0) callback.endGameFail(true);
            Util.println("      <SHIELDS DOWN TO " + enterprise.getShields() + " UNITS>");
            if (hits < 20) continue;
            if ((Util.random() > .6) || (hits / enterprise.getShields() <= .02)) continue;
            int randomDevice = Util.fnr();
            enterprise.setDeviceStatus(randomDevice, enterprise.getDeviceStatus()[randomDevice]- hits / enterprise.getShields() - .5 * Util.random());
            Util.println("DAMAGE CONTROL REPORTS " + Enterprise.printDeviceName(randomDevice) + " DAMAGED BY THE HIT'");
        }
    }

    public void moveEnterprise(final float course, final float warp, final int n, final double stardate, final double initialStardate, final int missionDuration, final GameCallback callback) {
        insertMarker(MARKER_EMPTY, Util.toInt(enterprise.getSector()[Enterprise.COORD_X]), Util.toInt(enterprise.getSector()[Enterprise.COORD_Y]));
        final int[] sector = enterprise.moveShip(course, n, quadrantMap, stardate, initialStardate, missionDuration, callback);
        int sectorX = sector[Enterprise.COORD_X];
        int sectorY = sector[Enterprise.COORD_Y];
        insertMarker(MARKER_ENTERPRISE, Util.toInt(sectorX), Util.toInt(sectorY));
        enterprise.maneuverEnergySR(n);
        double stardateDelta = 1;
        if (warp < 1) stardateDelta = .1 * Util.toInt(10 * warp);
        callback.incrementStardate(stardateDelta);
        if (stardate > initialStardate + missionDuration) callback.endGameFail(false);
    }

    void shortRangeSensorScan(final double stardate) {
        final int sectorX = enterprise.getSector()[Enterprise.COORD_X];
        final int sectorY = enterprise.getSector()[Enterprise.COORD_Y];
        boolean docked = false;
        String shipCondition; // ship condition (docked, red, yellow, green)
        for (int i = sectorX - 1; i <= sectorX + 1; i++) {
            for (int j = sectorY - 1; j <= sectorY + 1; j++) {
                if ((Util.toInt(i) >= 1) && (Util.toInt(i) <= 8) && (Util.toInt(j) >= 1) && (Util.toInt(j) <= 8)) {
                    if (compareMarker(quadrantMap, MARKER_STARBASE, i, j)) {
                        docked = true;
                    }
                }
            }
        }
        if (!docked) {
            enterprise.setDocked(false);
            if (klingons > 0) {
                shipCondition = "*RED*";
            } else {
                shipCondition = "GREEN";
                if (enterprise.getEnergy() < enterprise.getInitialEnergy() * .1) {
                    shipCondition = "YELLOW";
                }
            }
        } else {
            enterprise.setDocked(true);
            shipCondition = "DOCKED";
            enterprise.replenishSupplies();
            Util.println("SHIELDS DROPPED FOR DOCKING PURPOSES");
            enterprise.dropShields();
        }
        if (enterprise.getDeviceStatus()[Enterprise.DEVICE_SHORT_RANGE_SENSORS] < 0) { // are short range sensors out?
            Util.println("\n*** SHORT RANGE SENSORS ARE OUT ***\n");
            return;
        }
        final String row = "---------------------------------";
        Util.println(row);
        for (int i = 1; i <= 8; i++) {
            String sectorMapRow = "";
            for (int j = (i - 1) * 24 + 1; j <= (i - 1) * 24 + 22; j += 3) {
                sectorMapRow += " " + Util.midStr(quadrantMap, j, 3);
            }
            switch (i) {
                case 1:
                    Util.println(sectorMapRow + "        STARDATE           " + Util.toInt(stardate * 10) * .1);
                    break;
                case 2:
                    Util.println(sectorMapRow + "        CONDITION          " + shipCondition);
                    break;
                case 3:
                    Util.println(sectorMapRow + "        QUADRANT           " + enterprise.getQuadrant()[Enterprise.COORD_X] + "," + enterprise.getQuadrant()[Enterprise.COORD_Y]);
                    break;
                case 4:
                    Util.println(sectorMapRow + "        SECTOR             " + sectorX + "," + sectorY);
                    break;
                case 5:
                    Util.println(sectorMapRow + "        PHOTON TORPEDOES   " + Util.toInt(enterprise.getTorpedoes()));
                    break;
                case 6:
                    Util.println(sectorMapRow + "        TOTAL ENERGY       " + Util.toInt(enterprise.getTotalEnergy()));
                    break;
                case 7:
                    Util.println(sectorMapRow + "        SHIELDS            " + Util.toInt(enterprise.getShields()));
                    break;
                case 8:
                    Util.println(sectorMapRow + "        KLINGONS REMAINING " + Util.toInt(klingonsInGalaxy));
            }
        }
        Util.println(row);
    }

    void longRangeSensorScan() {
        final int quadrantX = enterprise.getQuadrant()[Enterprise.COORD_X];
        final int quadrantY = enterprise.getQuadrant()[Enterprise.COORD_Y];
        if (enterprise.getDeviceStatus()[Enterprise.DEVICE_LONG_RANGE_SENSORS] < 0) {
            Util.println("LONG RANGE SENSORS ARE INOPERABLE");
            return;
        }
        Util.println("LONG RANGE SCAN FOR QUADRANT " + quadrantX + "," + quadrantY);
        final String rowStr = "-------------------";
        Util.println(rowStr);
        final int[] n = new int[4];
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
                Util.print(": ");
                if (n[l] < 0) {
                    Util.print("*** ");
                    continue;
                }
                Util.print(Util.rightStr(Integer.toString(n[l] + 1000), 3) + " ");
            }
            Util.println(": \n" + rowStr);
        }
    }

    void firePhasers(GameCallback callback) {
        final double[] deviceStatus = enterprise.getDeviceStatus();
        final int quadrantX = enterprise.getQuadrant()[Enterprise.COORD_X];
        final int quadrantY = enterprise.getQuadrant()[Enterprise.COORD_Y];
        if (deviceStatus[Enterprise.DEVICE_PHASER_CONTROL] < 0) {
            Util.println("PHASERS INOPERATIVE");
            return;
        }
        if (klingons <= 0) {
            printNoEnemyShipsMessage();
            return;
        }
        if (deviceStatus[Enterprise.DEVICE_LIBRARY_COMPUTER] < 0) Util.println("COMPUTER FAILURE HAMPERS ACCURACY");
        Util.println("PHASERS LOCKED ON TARGET;  ");
        int nrUnitsToFire;
        while (true) {
            Util.println("ENERGY AVAILABLE = " + enterprise.getEnergy() + " UNITS");
            nrUnitsToFire = Util.toInt(Util.inputFloat("NUMBER OF UNITS TO FIRE"));
            if (nrUnitsToFire <= 0) return;
            if (enterprise.getEnergy() - nrUnitsToFire >= 0) break;
        }
        enterprise.decreaseEnergy(nrUnitsToFire);
        if (deviceStatus[Enterprise.DEVICE_SHIELD_CONTROL] < 0) nrUnitsToFire = Util.toInt(nrUnitsToFire * Util.random());
        int h1 = Util.toInt(nrUnitsToFire / klingons);
        for (int i = 1; i <= 3; i++) {
            if (klingonQuadrants[i][3] <= 0) break;
            int hitPoints = Util.toInt((h1 / fnd(0)) * (Util.random() + 2));
            if (hitPoints <= .15 * klingonQuadrants[i][3]) {
                Util.println("SENSORS SHOW NO DAMAGE TO ENEMY AT " + klingonQuadrants[i][1] + "," + klingonQuadrants[i][2]);
                continue;
            }
            klingonQuadrants[i][3] = klingonQuadrants[i][3] - hitPoints;
            Util.println(hitPoints + " UNIT HIT ON KLINGON AT SECTOR " + klingonQuadrants[i][1] + "," + klingonQuadrants[i][2]);
            if (klingonQuadrants[i][3] <= 0) {
                Util.println("*** KLINGON DESTROYED ***");
                klingons -= 1;
                klingonsInGalaxy -= 1;
                insertMarker(MARKER_EMPTY, klingonQuadrants[i][1], klingonQuadrants[i][2]);
                klingonQuadrants[i][3] = 0;
                galaxy[quadrantX][quadrantY] -= 100;
                chartedGalaxy[quadrantX][quadrantY] = galaxy[quadrantX][quadrantY];
                if (klingonsInGalaxy <= 0) callback.endGameSuccess();
            } else {
                Util.println("   (SENSORS SHOW " + klingonQuadrants[i][3] + " UNITS REMAINING)");
            }
        }
        klingonsShoot(callback);
    }

    void firePhotonTorpedo(final double stardate, final double initialStardate, final double missionDuration, GameCallback callback) {
        if (enterprise.getTorpedoes() <= 0) {
            Util.println("ALL PHOTON TORPEDOES EXPENDED");
            return;
        }
        if (enterprise.getDeviceStatus()[Enterprise.DEVICE_PHOTON_TUBES] < 0) {
            Util.println("PHOTON TUBES ARE NOT OPERATIONAL");
        }
        float c1 = Util.inputFloat("PHOTON TORPEDO COURSE (1-9)");
        if (c1 == 9) c1 = 1;
        if (c1 < 1 && c1 >= 9) {
            Util.println("ENSIGN CHEKOV REPORTS,  'INCORRECT COURSE DATA, SIR!'");
            return;
        }
        int ic1 = Util.toInt(c1);
        final int[][] cardinalDirections = enterprise.getCardinalDirections();
        float x1 = cardinalDirections[ic1][1] + (cardinalDirections[ic1 + 1][1] - cardinalDirections[ic1][1]) * (c1 - ic1);
        enterprise.decreaseEnergy(2);
        enterprise.decreaseTorpedoes(1);
        float x2 = cardinalDirections[ic1][2] + (cardinalDirections[ic1 + 1][2] - cardinalDirections[ic1][2]) * (c1 - ic1);
        float x = enterprise.getSector()[Enterprise.COORD_X];
        float y = enterprise.getSector()[Enterprise.COORD_Y];
        Util.println("TORPEDO TRACK:");
        while (true) {
            x = x + x1;
            y = y + x2;
            int x3 = Math.round(x);
            int y3 = Math.round(y);
            if (x3 < 1 || x3 > 8 || y3 < 1 || y3 > 8) {
                Util.println("TORPEDO MISSED"); // 5490
                klingonsShoot(callback);
                return;
            }
            Util.println("               " + x3 + "," + y3);
            if (compareMarker(quadrantMap, MARKER_EMPTY, Util.toInt(x), Util.toInt(y)))  {
                continue;
            } else if (compareMarker(quadrantMap, MARKER_KLINGON, Util.toInt(x), Util.toInt(y))) {
                Util.println("*** KLINGON DESTROYED ***");
                klingons = klingons - 1;
                klingonsInGalaxy = klingonsInGalaxy - 1;
                if (klingonsInGalaxy <= 0) callback.endGameSuccess();
                for (int i = 1; i <= 3; i++) {
                    if (x3 == klingonQuadrants[i][1] && y3 == klingonQuadrants[i][2]) break;
                }
                int i = 3;
                klingonQuadrants[i][3] = 0;
            } else if (compareMarker(quadrantMap, MARKER_STAR, Util.toInt(x), Util.toInt(y))) {
                Util.println("STAR AT " + x3 + "," + y3 + " ABSORBED TORPEDO ENERGY.");
                klingonsShoot(callback);
                return;
            } else if (compareMarker(quadrantMap, MARKER_STARBASE, Util.toInt(x), Util.toInt(y))) {
                Util.println("*** STARBASE DESTROYED ***");
                starbases = starbases - 1;
                basesInGalaxy = basesInGalaxy - 1;
                if (basesInGalaxy == 0 && klingonsInGalaxy <= stardate - initialStardate - missionDuration) {
                    Util.println("THAT DOES IT, CAPTAIN!!  YOU ARE HEREBY RELIEVED OF COMMAND");
                    Util.println("AND SENTENCED TO 99 STARDATES AT HARD LABOR ON CYGNUS 12!!");
                    callback.endGameFail(false);
                } else {
                    Util.println("STARFLEET COMMAND REVIEWING YOUR RECORD TO CONSIDER");
                    Util.println("COURT MARTIAL!");
                    enterprise.setDocked(false);
                }
            }
            insertMarker(MARKER_EMPTY, Util.toInt(x), Util.toInt(y));
            final int quadrantX = enterprise.getQuadrant()[Enterprise.COORD_X];
            final int quadrantY = enterprise.getQuadrant()[Enterprise.COORD_Y];
            galaxy[quadrantX][quadrantY] = klingons * 100 + starbases * 10 + stars;
            chartedGalaxy[quadrantX][quadrantY] = galaxy[quadrantX][quadrantY];
            klingonsShoot(callback);
        }
    }

    public void cumulativeGalacticRecord(final boolean cumulativeReport) {
        final int quadrantX = enterprise.getQuadrant()[Enterprise.COORD_X];
        final int quadrantY = enterprise.getQuadrant()[Enterprise.COORD_Y];
        if (cumulativeReport) {
            Util.println("");
            Util.println("        ");
            Util.println("COMPUTER RECORD OF GALAXY FOR QUADRANT " + quadrantX + "," + quadrantY);
            Util.println("");
        } else {
            Util.println("                        THE GALAXY");
        }
        Util.println("       1     2     3     4     5     6     7     8");
        final String rowDivider = "     ----- ----- ----- ----- ----- ----- ----- -----";
        Util.println(rowDivider);
        for (int i = 1; i <= 8; i++) {
            Util.print(i + "  ");
            if (cumulativeReport) {
                int y = 1;
                String quadrantName = getQuadrantName(false, i, y);
                int tabLen = Util.toInt(15 - .5 * Util.strlen(quadrantName));
                Util.println(Util.tab(tabLen) + quadrantName);
                y = 5;
                quadrantName = getQuadrantName(false, i, y);
                tabLen = Util.toInt(39 - .5 * Util.strlen(quadrantName));
                Util.println(Util.tab(tabLen) + quadrantName);
            } else {
                for (int j = 1; j <= 8; j++) {
                    Util.print("   ");
                    if (chartedGalaxy[i][j] == 0) {
                        Util.print("***");
                    } else {
                        Util.print(Util.rightStr(Integer.toString(chartedGalaxy[i][j] + 1000), 3));
                    }
                }
            }
            Util.println("");
            Util.println(rowDivider);
        }
        Util.println("");
    }

    public void photonTorpedoData() {
        int sectorX = enterprise.getSector()[Enterprise.COORD_X];
        int sectorY = enterprise.getSector()[Enterprise.COORD_Y];
        if (klingons <= 0) {
            printNoEnemyShipsMessage();
            return;
        }
        Util.println("FROM ENTERPRISE TO KLINGON BATTLE CRUISER" + ((klingons > 1)? "S" : ""));
        for (int i = 1; i <= 3; i++) {
            if (klingonQuadrants[i][3] > 0) {
                printDirection(sectorX, sectorY, klingonQuadrants[i][1], klingonQuadrants[i][2]);
            }
        }
    }

    void directionDistanceCalculator() {
        int quadrantX = enterprise.getQuadrant()[Enterprise.COORD_X];
        int quadrantY = enterprise.getQuadrant()[Enterprise.COORD_Y];
        int sectorX = enterprise.getSector()[Enterprise.COORD_X];
        int sectorY = enterprise.getSector()[Enterprise.COORD_Y];
        Util.println("DIRECTION/DISTANCE CALCULATOR:");
        Util.println("YOU ARE AT QUADRANT " + quadrantX + "," + quadrantY + " SECTOR " + sectorX + "," + sectorY);
        Util.print("PLEASE ENTER ");
        int[] initialCoords = Util.inputCoords("  INITIAL COORDINATES (X,Y)");
        int[] finalCoords = Util.inputCoords("  FINAL COORDINATES (X,Y)");
        printDirection(initialCoords[0], initialCoords[1], finalCoords[0], finalCoords[1]);
    }

    void printDirection(int from_x, int from_y, int to_x, int to_y) {
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
                Util.println("DIRECTION = " + (from_x + to_y / from_y));
            } else {
                Util.println("DIRECTION = " + (from_x + 2 - to_y / from_y));
            }
        }
        Util.println("DISTANCE = " + Util.round(Math.sqrt(to_y ^ 2 + from_y ^ 2), 6));
    }

    void starbaseNavData() {
        int sectorX = enterprise.getSector()[Enterprise.COORD_X];
        int sectorY = enterprise.getSector()[Enterprise.COORD_Y];
        if (starbases != 0) {
            Util.println("FROM ENTERPRISE TO STARBASE:");
            printDirection(sectorX, sectorY, starbaseX, starbaseY);
        } else {
            Util.println("MR. SPOCK REPORTS,  'SENSORS SHOW NO STARBASES IN THIS");
            Util.println(" QUADRANT.'");
        }
    }

    void printNoEnemyShipsMessage() {
        Util.println("SCIENCE OFFICER SPOCK REPORTS  'SENSORS SHOW NO ENEMY SHIPS");
        Util.println("                                IN THIS QUADRANT'");
    }

    String getRegionName(final boolean regionNameOnly, final int y) {
        if (!regionNameOnly) {
            switch (y % 4) {
                case 0:
                    return " I";
                case 1:
                    return " II";
                case 2:
                    return " III";
                case 3:
                    return " IV";
            }
        }
        return "";
    }

    String getQuadrantName(final boolean regionNameOnly, final int x, final int y) {
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

    void insertMarker(final String marker, final int x, final int y) {
        final int pos = Util.toInt(y) * 3 + Util.toInt(x) * 24 + 1;
        if (marker.length() != 3) {
            System.err.println("ERROR");
            System.exit(-1);
        }
        if (pos == 1) {
            quadrantMap = marker + Util.rightStr(quadrantMap, 189);
        }
        if (pos == 190) {
            quadrantMap = Util.leftStr(quadrantMap, 189) + marker;
        }
        quadrantMap = Util.leftStr(quadrantMap, (pos - 1)) + marker + Util.rightStr(quadrantMap, (190 - pos));
    }

    /**
     * Finds random empty coordinates in a quadrant.
     *
     * @param quadrantString
     * @return an array with a pair of coordinates x, y
     */
    int[] findEmptyPlaceInQuadrant(final String quadrantString) {
        final int x = Util.fnr();
        final int y = Util.fnr();
        if (!compareMarker(quadrantString, MARKER_EMPTY, x, y)) {
            return findEmptyPlaceInQuadrant(quadrantString);
        }
        return new int[]{x, y};
    }

    boolean compareMarker(final String quadrantString, final String marker, final int x, final int y) {
        final int markerRegion = (y - 1) * 3 + (x - 1) * 24 + 1;
        if (Util.midStr(quadrantString, markerRegion, 3).equals(marker)) {
            return true;
        }
        return false;
    }

}
