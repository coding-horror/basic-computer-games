import java.util.stream.IntStream;

/**
 * The starship Enterprise.
 */
public class Enterprise {

    public static final int COORD_X = 0;
    public static final int COORD_Y = 1;

    // devices
    static final int DEVICE_WARP_ENGINES = 1;
    static final int DEVICE_SHORT_RANGE_SENSORS = 2;
    static final int DEVICE_LONG_RANGE_SENSORS = 3;
    static final int DEVICE_PHASER_CONTROL = 4;
    static final int DEVICE_PHOTON_TUBES = 5;
    static final int DEVICE_DAMAGE_CONTROL = 6;
    static final int DEVICE_SHIELD_CONTROL = 7;
    static final int DEVICE_LIBRARY_COMPUTER = 8;
    final double[] deviceStatus = new double[9];   // 8  device damage stats

    // position
    final int[][] cardinalDirections = new int[10][3];   // 9x2 vectors in cardinal directions
    int quadrantX;
    int quadrantY;
    int sectorX;
    int sectorY;

    // ship status
    boolean docked = false;
    int energy = 3000;
    int torpedoes = 10;
    int shields = 0;
    double repairCost;

    final int initialEnergy = energy;
    final int initialTorpedoes = torpedoes;

    public Enterprise() {
        // random initial position
        this.setQuadrant(new int[]{ Util.fnr(), Util.fnr() });
        this.setSector(new int[]{ Util.fnr(), Util.fnr() });
        // init cardinal directions
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
        // init devices
        IntStream.range(1, 8).forEach(i -> deviceStatus[i] = 0);
    }

    public int getShields() {
        return shields;
    }

    /**
     * Enterprise is hit by enemies.
     * @param hits the number of hit points
     */
    public void sufferHitPoints(int hits) {
        this.shields = shields - hits;
    }

    public int getEnergy() {
        return energy;
    }

    public void replenishSupplies() {
        this.energy = this.initialEnergy;
        this.torpedoes = this.initialTorpedoes;
    }

    public void decreaseEnergy(final double amount) {
        this.energy -= amount;
    }

    public void decreaseTorpedoes(final int amount) {
        torpedoes -= amount;
    }

    public void dropShields() {
        this.shields = 0;
    }

    public int getTotalEnergy() {
        return (shields + energy);
    }

    public int getInitialEnergy() {
        return initialEnergy;
    }

    public int getTorpedoes() {
        return torpedoes;
    }

    public double[] getDeviceStatus() {
        return deviceStatus;
    }

    public int[][] getCardinalDirections() {
        return cardinalDirections;
    }

    public void setDeviceStatus(final int device, final double status) {
        this.deviceStatus[device] = status;
    }

    public boolean isDocked() {
        return docked;
    }

    public void setDocked(boolean docked) {
        this.docked = docked;
    }

    public int[] getQuadrant() {
        return new int[] {quadrantX, quadrantY};
    }

    public void setQuadrant(final int[] quadrant) {
        this.quadrantX = quadrant[COORD_X];
        this.quadrantY = quadrant[COORD_Y];
    }

    public int[] getSector() {
        return new int[] {sectorX, sectorY};
    }

    public void setSector(final int[] sector) {
        this.sectorX = sector[COORD_X];
        this.sectorY = sector[COORD_Y];
    }

    public int[] moveShip(final float course, final int n, final String quadrantMap, final double stardate, final double initialStardate, final int missionDuration, final GameCallback callback) {
        int ic1 = Util.toInt(course);
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
                quadrantX = Util.toInt(x / 8);
                quadrantY = Util.toInt(y / 8);
                sectorX = Util.toInt(x - quadrantX * 8);
                sectorY = Util.toInt(y - quadrantY * 8);
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
                    Util.println("LT. UHURA REPORTS MESSAGE FROM STARFLEET COMMAND:");
                    Util.println("  'PERMISSION TO ATTEMPT CROSSING OF GALACTIC PERIMETER");
                    Util.println("  IS HEREBY *DENIED*.  SHUT DOWN YOUR ENGINES.'");
                    Util.println("CHIEF ENGINEER SCOTT REPORTS  'WARP ENGINES SHUT DOWN");
                    Util.println("  AT SECTOR " + sectorX + "," + sectorY + " OF QUADRANT " + quadrantX + "," + quadrantY + ".'");
                    if (stardate > initialStardate + missionDuration) callback.endGameFail(false);
                }
                if (8 * quadrantX + quadrantY == 8 * initialQuadrantX + initialQuadrantY) {
                    break;
                }
                callback.incrementStardate(1);
                maneuverEnergySR(n);
                callback.enterNewQuadrant();
                return this.getSector();
            } else {
                int S8 = Util.toInt(sectorX) * 24 + Util.toInt(sectorY) * 3 - 26; // S8 = pos
                if (!("  ".equals(Util.midStr(quadrantMap, S8, 2)))) {
                    sectorX = Util.toInt(sectorX - x1);
                    sectorY = Util.toInt(sectorY - x2);
                    Util.println("WARP ENGINES SHUT DOWN AT ");
                    Util.println("SECTOR " + sectorX + "," + sectorY + " DUE TO BAD NAVIGATION");
                    break;
                }
            }
        }
        sectorX = Util.toInt(sectorX);
        sectorY = Util.toInt(sectorY);
        return this.getSector();
    }

    void randomRepairCost() {
        repairCost = .5 * Util.random();
    }

    public void repairDamagedDevices(final float warp) {
        // repair damaged devices and print damage report
        for (int i = 1; i <= 8; i++) {
            if (deviceStatus[i] < 0) {
                deviceStatus[i] += Math.min(warp, 1);
                if ((deviceStatus[i] > -.1) && (deviceStatus[i] < 0)) {
                    deviceStatus[i] = -.1;
                    break;
                } else if (deviceStatus[i] >= 0) {
                    Util.println("DAMAGE CONTROL REPORT:  ");
                    Util.println(Util.tab(8) + printDeviceName(i) + " REPAIR COMPLETED.");
                }
            }
        }
    }

    public void maneuverEnergySR(final int N) {
        energy = energy - N - 10;
        if (energy >= 0) return;
        Util.println("SHIELD CONTROL SUPPLIES ENERGY TO COMPLETE THE MANEUVER.");
        shields = shields + energy;
        energy = 0;
        if (shields <= 0) shields = 0;
    }

    void shieldControl() {
        if (deviceStatus[DEVICE_SHIELD_CONTROL] < 0) {
            Util.println("SHIELD CONTROL INOPERABLE");
            return;
        }
        Util.println("ENERGY AVAILABLE = " + (energy + shields));
        int energyToShields = Util.toInt(Util.inputFloat("NUMBER OF UNITS TO SHIELDS"));
        if (energyToShields < 0 || shields == energyToShields) {
            Util.println("<SHIELDS UNCHANGED>");
            return;
        }
        if (energyToShields > energy + energyToShields) {
            Util.println("SHIELD CONTROL REPORTS  'THIS IS NOT THE FEDERATION TREASURY.'");
            Util.println("<SHIELDS UNCHANGED>");
            return;
        }
        energy = energy + shields - energyToShields;
        shields = energyToShields;
        Util.println("DEFLECTOR CONTROL ROOM REPORT:");
        Util.println("  'SHIELDS NOW AT " + Util.toInt(shields) + " UNITS PER YOUR COMMAND.'");
    }

    void damageControl(GameCallback callback) {
        if (deviceStatus[DEVICE_DAMAGE_CONTROL] < 0) {
            Util.println("DAMAGE CONTROL REPORT NOT AVAILABLE");
        } else {
            Util.println("\nDEVICE             STATE OF REPAIR");
            for (int deviceNr = 1; deviceNr <= 8; deviceNr++) {
                Util.print(printDeviceName(deviceNr) + Util.leftStr(GalaxyMap.QUADRANT_ROW, 25 - Util.strlen(printDeviceName(deviceNr))) + " " + Util.toInt(deviceStatus[deviceNr] * 100) * .01 + "\n");
            }
        }
        if (!docked) return;

        double deltaToRepair = 0;
        for (int i = 1; i <= 8; i++) {
            if (deviceStatus[i] < 0) deltaToRepair += .1;
        }
        if (deltaToRepair > 0) {
            deltaToRepair += repairCost;
            if (deltaToRepair >= 1) deltaToRepair = .9;
            Util.println("TECHNICIANS STANDING BY TO EFFECT REPAIRS TO YOUR SHIP;");
            Util.println("ESTIMATED TIME TO REPAIR:'" + .01 * Util.toInt(100 * deltaToRepair) + " STARDATES");
            final String reply = Util.inputStr("WILL YOU AUTHORIZE THE REPAIR ORDER (Y/N)");
            if ("Y".equals(reply)) {
                for (int deviceNr = 1; deviceNr <= 8; deviceNr++) {
                    if (deviceStatus[deviceNr] < 0) deviceStatus[deviceNr] = 0;
                }
                callback.incrementStardate(deltaToRepair + .1);
            }
        }
    }

    public static String printDeviceName(final int deviceNumber) {  // 8790
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

}
