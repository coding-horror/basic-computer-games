/**
 * The Player class model the user and compuer player
 */
public class Player {
    private final String name;
    private final Punch bestPunch;
    private final Punch vulnerability;
    private boolean isPlayer = false;

    public Player(String name, Punch bestPunch, Punch vulnerability) {
        this.name = name;
        this.bestPunch = bestPunch;
        this.vulnerability = vulnerability;
        this.isPlayer = true;
    }

    /**
     * Player with random Best Punch and Vulnerability
     */
    public Player(String name) {
        this.name = name;

        int b1;
        int d1;

        do {
            b1 = Basic.randomOf(4);
            d1 = Basic.randomOf(4);
        } while (b1 == d1);

        this.bestPunch = Punch.fromCode(b1);
        this.vulnerability = Punch.fromCode(d1);
    }

    public boolean isPlayer() { return isPlayer; }
    public String getName() { return  name; }
    public Punch getBestPunch() { return bestPunch; }

    public boolean hitVulnerability(Punch punch) {
        return vulnerability == punch;
    }
}