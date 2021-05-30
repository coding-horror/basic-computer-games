import java.util.Scanner;

/**
 * TARGET
 * <p>
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 */
public class Target {

	private static final double RADIAN = 180 / Math.PI;

	public static void main(String[] args) {
		Scanner scan = new Scanner(System.in);

		printIntro();

		//continue till the user aborts
		while (true) {
			int numberShots = 0;

			final double xAxisInRadians = Math.random() * 2 * Math.PI;
			final double yAxisInRadians = Math.random() * 2 * Math.PI;
			System.out.printf("RADIANS FROM X AXIS = %.7f     FROM Z AXIS = %.7f\n", xAxisInRadians, yAxisInRadians);

			final double p1 = 100000 * Math.random() + Math.random();
			final double x = Math.sin(yAxisInRadians) * Math.cos(xAxisInRadians) * p1;
			final double y = Math.sin(yAxisInRadians) * Math.sin(xAxisInRadians) * p1;
			final double z = Math.cos(yAxisInRadians) * p1;
			System.out.printf("TARGET SIGHTED: APPROXIMATE COORDINATES:  X=%.3f  Y=%.3f  Z=%.3f\n", x, y, z);
			boolean targetOrSelfDestroyed = false;
			while (!targetOrSelfDestroyed) {
				numberShots++;
				int estimatedDistance = 0;
				switch (numberShots) {
					case 1:
						estimatedDistance = (int) (p1 * .05) * 20;
						break;
					case 2:
						estimatedDistance = (int) (p1 * .1) * 10;
						break;
					case 3:
						estimatedDistance = (int) (p1 * .5) * 2;
						break;
					case 4:
					case 5:
						estimatedDistance = (int) (p1);
						break;
				}

				System.out.printf("     ESTIMATED DISTANCE: %s\n\n", estimatedDistance);

				final TargetAttempt targetAttempt = readInput(scan);
				if (targetAttempt.distance < 20) {
					System.out.println("YOU BLEW YOURSELF UP!!");
					targetOrSelfDestroyed = true;
				} else {
					final double a1 = targetAttempt.xDeviation / RADIAN;
					final double b1 = targetAttempt.zDeviation / RADIAN;
					System.out.printf("RADIANS FROM X AXIS = %.7f  FROM Z AXIS = %.7f\n", a1, b1);

					final double x1 = targetAttempt.distance * Math.sin(b1) * Math.cos(a1);
					final double y1 = targetAttempt.distance * Math.sin(b1) * Math.sin(a1);
					final double z1 = targetAttempt.distance * Math.cos(b1);

					double distance = Math.sqrt((x1 - x) * (x1 - x) + (y1 - y) * (y1 - y) + (z1 - z) * (z1 - z));
					if (distance > 20) {
						double X2 = x1 - x;
						double Y2 = y1 - y;
						double Z2 = z1 - z;
						if (X2 < 0) {
							System.out.printf("SHOT BEHIND TARGET %.7f KILOMETERS.\n", -X2);
						} else {
							System.out.printf("SHOT IN FRONT OF TARGET %.7f KILOMETERS.\n", X2);
						}
						if (Y2 < 0) {
							System.out.printf("SHOT TO RIGHT OF TARGET %.7f KILOMETERS.\n", -Y2);
						} else {
							System.out.printf("SHOT TO LEFT OF TARGET %.7f KILOMETERS.\n", Y2);
						}
						if (Z2 < 0) {
							System.out.printf("SHOT BELOW TARGET %.7f KILOMETERS.\n", -Z2);
						} else {
							System.out.printf("SHOT ABOVE TARGET %.7f KILOMETERS.\n", Z2);
						}
						System.out.printf("APPROX POSITION OF EXPLOSION:  X=%.7f   Y=%.7f   Z=%.7f\n", x1, y1, z1);
						System.out.printf("     DISTANCE FROM TARGET =%.7f\n\n\n\n", distance);
					} else {
						System.out.println(" * * * HIT * * *   TARGET IS NON-FUNCTIONAL");
						System.out.printf("DISTANCE OF EXPLOSION FROM TARGET WAS %.5f KILOMETERS.\n", distance);
						System.out.printf("MISSION ACCOMPLISHED IN %s SHOTS.\n", numberShots);
						targetOrSelfDestroyed = true;
					}
				}
			}
			System.out.println("\n\n\n\n\nNEXT TARGET...\n");
		}
	}

	private static TargetAttempt readInput(Scanner scan) {
		System.out.println("INPUT ANGLE DEVIATION FROM X, DEVIATION FROM Z, DISTANCE ");
		boolean validInput = false;
		TargetAttempt targetAttempt = new TargetAttempt();
		while (!validInput) {
			String input = scan.nextLine();
			final String[] split = input.split(",");
			try {
				targetAttempt.xDeviation = Float.parseFloat(split[0]);
				targetAttempt.zDeviation = Float.parseFloat(split[1]);
				targetAttempt.distance = Float.parseFloat(split[2]);
				validInput = true;
			} catch (NumberFormatException nfe) {
				System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE\n? ");
			}

		}
		return targetAttempt;
	}

	private static void printIntro() {
		System.out.println("                                TARGET");
		System.out.println("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
		System.out.println("\n\n");
		System.out.println("YOU ARE THE WEAPONS OFFICER ON THE STARSHIP ENTERPRISE");
		System.out.println("AND THIS IS A TEST TO SEE HOW ACCURATE A SHOT YOU");
		System.out.println("ARE IN A THREE-DIMENSIONAL RANGE.  YOU WILL BE TOLD");
		System.out.println("THE RADIAN OFFSET FOR THE X AND Z AXES, THE LOCATION");
		System.out.println("OF THE TARGET IN THREE DIMENSIONAL RECTANGULAR COORDINATES,");
		System.out.println("THE APPROXIMATE NUMBER OF DEGREES FROM THE X AND Z");
		System.out.println("AXES, AND THE APPROXIMATE DISTANCE TO THE TARGET.");
		System.out.println("YOU WILL THEN PROCEED TO SHOOT AT THE TARGET UNTIL IT IS");
		System.out.println("DESTROYED!");
		System.out.println("\nGOOD LUCK!!\n\n");
	}

	/**
	 * Represents the user input
	 */
	private static class TargetAttempt {

		double xDeviation;
		double zDeviation;
		double distance;
	}
}


