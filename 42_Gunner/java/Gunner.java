import java.util.Random;
import java.util.Scanner;

public class Gunner {

    public static final int MAX_ROUNDS = 6;
    public static final int MAX_ENEMIES = 4;
    public static final int ERROR_DISTANCE = 100;

    private static Scanner scanner = new Scanner(System.in);
    private static Random random = new Random();

    public static void main(String[] args) {
        println("                              GUNNER");
        println("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        println();
        println();
        println();
        println("YOU ARE THE OFFICER-IN-CHARGE, GIVING ORDERS TO A GUN");
        println("CREW, TELLING THEM THE DEGREES OF ELEVATION YOU ESTIMATE");
        println("WILL PLACE A PROJECTILE ON TARGET.  A HIT WITHIN " + ERROR_DISTANCE + " YARDS");
        println("OF THE TARGET WILL DESTROY IT.");
        println();
        while (true) {
            int maxRange = random.nextInt(40000) + 20000;
            int enemyCount = 0;
            int totalRounds = 0;
            println("MAXIMUM RANGE OF YOUR GUN IS " + maxRange + " YARDS.\n");

            while (true) {
                int rounds = fightEnemy(maxRange);
                totalRounds += rounds;

                if (enemyCount == MAX_ENEMIES || rounds >= MAX_ROUNDS) {
                    if (rounds < MAX_ROUNDS) {
                        println("\n\n\nTOTAL ROUNDS EXPENDED WERE:" + totalRounds);
                    }
                    if (totalRounds > 18 || rounds >= MAX_ROUNDS) {
                        println("BETTER GO BACK TO FORT SILL FOR REFRESHER TRAINING!");
                    } else {
                        println("NICE SHOOTING !!");
                    }
                    println("\nTRY AGAIN (Y OR N)");
                    String tryAgainResponse = scanner.nextLine();
                    if ("Y".equals(tryAgainResponse) || "y".equals(tryAgainResponse)) {
                        break;
                    }
                    println("\nOK.  RETURN TO BASE CAMP.");
                    return;
                }
                enemyCount++;
                println("\nTHE FORWARD OBSERVER HAS SIGHTED MORE ENEMY ACTIVITY...");
            }
        }
    }

    private static int fightEnemy(int maxRange) {
        int rounds = 0;
        long target = Math.round(maxRange * (random.nextDouble() * 0.8 + 0.1));
        println("      DISTANCE TO THE TARGET IS " + target + " YARDS.");

        while (true) {
            println("\nELEVATION?");
            double elevation = Double.parseDouble(scanner.nextLine());
            if (elevation > 89.0) {
                println("MAXIMUM ELEVATION IS 89 DEGREES.");
                continue;
            }
            if (elevation < 1.0) {
                println("MINIMUM ELEVATION IS ONE DEGREE.");
                continue;
            }
            rounds++;
            if (rounds >= MAX_ROUNDS) {
                println("\nBOOM !!!!   YOU HAVE JUST BEEN DESTROYED ");
                println("BY THE ENEMY.\n\n\n");
                break;
            }

            long error = calculateError(maxRange, target, elevation);
            if (Math.abs(error) < ERROR_DISTANCE) {
                println("*** TARGET DESTROYED ***  " + rounds + " ROUNDS OF AMMUNITION EXPENDED.");
                break;
            } else if (error > ERROR_DISTANCE) {
                println("SHORT OF TARGET BY " + Math.abs(error) + " YARDS.");
            } else {
                println("OVER TARGET BY " + Math.abs(error) + " YARDS.");
            }

        }
        return rounds;
    }

    private static long calculateError(int maxRange, long target, double elevationInDegrees) {
        double elevationInRadians = Math.PI * elevationInDegrees / 90.0; //convert degrees to radians
        double impact = maxRange * Math.sin(elevationInRadians);
        double error = target - impact;
        return Math.round(error);
    }

    private static void println(String s) {
        System.out.println(s);
    }

    private static void println() {
        System.out.println();
    }
}
