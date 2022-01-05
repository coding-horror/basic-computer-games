import java.util.*;

/**
 * SPLAT simulates a parachute jump in which you try to open your parachute at the last possible moment without going
 * splat! You may select your own terminal velocity or let the computer do it for you. You many also select the
 * acceleration due to gravity or, again, let the computer do it in which case you might wind up on any of eight
 * planets (out to Neptune), the moon, or the sun.
 * <p>
 * The computer then tells you the height you’re jumping from and asks for the seconds of free fall. It then divides
 * your free fall time into eight intervals and gives you progress reports on your way down. The computer also keeps
 * track of all prior jumps in the array A and lets you know how you compared with previous successful jumps. If you
 * want to recall information from previous runs, then you should store array A in a disk or take file and read it
 * before each run.
 * <p>
 * John Yegge created this program while at the Oak Ridge Associated Universities.
 * <p>
 * Ported from BASIC by jason plumb (@breedx2)
 * </p>
 */
public class Splat {
    private static final Random random = new Random();
    private final Scanner scanner = new Scanner(System.in);
    private final List<Float> pastSuccessfulJumpDistances = new ArrayList<>();

    public static void main(String[] args) {
        new Splat().run();
    }

    public void run() {
        showIntroduction();

        while (true) {

            InitialJumpConditions initial = buildInitialConditions();

            System.out.println();
            System.out.printf("    ALTITUDE         = %d FT\n", initial.getAltitude());
            System.out.printf("    TERM. VELOCITY   = %.2f FT/SEC +/-5%%\n", initial.getOriginalTerminalVelocity());
            System.out.printf("    ACCELERATION     = %.2f FT/SEC/SEC +/-5%%\n", initial.getOriginalAcceleration());

            System.out.println("SET THE TIMER FOR YOUR FREEFALL.");
            System.out.print("HOW MANY SECONDS ");
            float freefallTime = scanner.nextFloat();
            System.out.println("HERE WE GO.\n");
            System.out.println("TIME (SEC)  DIST TO FALL (FT)");
            System.out.println("==========  =================");

            JumpResult jump = executeJump(initial, freefallTime);
            showJumpResults(initial, jump);

            if (!playAgain()) {
                System.out.println("SSSSSSSSSS.");
                return;
            }
        }
    }

    private void showIntroduction() {
        System.out.printf("%33s%s\n", " ", "SPLAT");
        System.out.printf("%15s%s\n", " ", "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.print("\n\n\n");
        System.out.println("WELCOME TO 'SPLAT' -- THE GAME THAT SIMULATES A PARACHUTE");
        System.out.println("JUMP.  TRY TO OPEN YOUR CHUTE AT THE LAST POSSIBLE");
        System.out.println("MOMENT WITHOUT GOING SPLAT.");
    }

    private InitialJumpConditions buildInitialConditions() {
        System.out.print("\n\n");
        float terminalVelocity = promptTerminalVelocity();
        float acceleration = promptGravitationalAcceleration();
        return InitialJumpConditions.create(terminalVelocity, acceleration);
    }

    private float promptTerminalVelocity() {
        if (askYesNo("SELECT YOUR OWN TERMINAL VELOCITY")) {
            System.out.print("WHAT TERMINAL VELOCITY (MI/HR) ");
            return mphToFeetPerSec(scanner.nextFloat());
        }
        float terminalVelocity = (int) (1000 * random.nextFloat());
        System.out.printf("OK.  TERMINAL VELOCITY = %.2f MI/HR\n", terminalVelocity);
        return mphToFeetPerSec(terminalVelocity);
    }

    private float promptGravitationalAcceleration() {
        if (askYesNo("WANT TO SELECT ACCELERATION DUE TO GRAVITY")) {
            System.out.print("WHAT ACCELERATION (FT/SEC/SEC) ");
            return scanner.nextFloat();
        }
        return chooseRandomAcceleration();
    }

    private JumpResult executeJump(InitialJumpConditions initial, float chuteOpenTime) {
        JumpResult jump = new JumpResult(initial.getAltitude());
        for (float time = 0.0f; !jump.isSplat() && (time < chuteOpenTime); time += chuteOpenTime / 8) {
            if (!jump.hasReachedTerminalVelocity() && time > initial.getTimeOfTerminalAccelerationReached()) {
                jump.setReachedTerminalVelocity();
                System.out.printf("TERMINAL VELOCITY REACHED AT T PLUS %f SECONDS.\n", initial.getTimeOfTerminalAccelerationReached());
            }
            float newDistance = computeDistance(initial, time, jump.hasReachedTerminalVelocity());
            jump.setDistance(newDistance);

            if (jump.isSplat()) {
                return jump;
            }
            System.out.printf("%10.2f  %f\n", time, jump.getDistance());
        }
        return jump;
    }

    private float computeDistance(InitialJumpConditions initial, float i, boolean hasReachedTerminalVelocity) {
        final float V = initial.getTerminalVelocity();
        final float A = initial.getAcceleration();
        if (hasReachedTerminalVelocity) {
            return initial.getAltitude() - ((V * V / (2 * A)) + (V * (i - (V / A))));
        }
        return initial.getAltitude() - ((A / 2) * i * i);
    }

    private void showJumpResults(InitialJumpConditions initial, JumpResult jump) {
        if (jump.isSplat()) {
            showSplatMessage(initial, jump);
            showCleverSplatMessage();
            return;
        }
        System.out.println("CHUTE OPEN");
        int worseJumpCount = countWorseHistoricalJumps(jump);
        int successfulJumpCt = pastSuccessfulJumpDistances.size();
        pastSuccessfulJumpDistances.add(jump.getDistance());

        if (pastSuccessfulJumpDistances.size() <= 2) {
            List<String> ordinals = Arrays.asList("1ST", "2ND", "3RD");
            System.out.printf("AMAZING!!! NOT BAD FOR YOUR %s SUCCESSFUL JUMP!!!\n", ordinals.get(successfulJumpCt));
            return;
        }

        int betterThanCount = successfulJumpCt - worseJumpCount;
        if (betterThanCount <= 0.1 * successfulJumpCt) {
            System.out.printf("WOW!  THAT'S SOME JUMPING.  OF THE %d SUCCESSFUL JUMPS\n", successfulJumpCt);
            System.out.printf("BEFORE YOURS, ONLY %d OPENED THEIR CHUTES LOWER THAN\n", betterThanCount);
            System.out.println("YOU DID.");
        } else if (betterThanCount <= 0.25 * successfulJumpCt) {
            System.out.printf("PRETTY GOOD!  %d SUCCESSFUL JUMPS PRECEDED YOURS AND ONLY\n", successfulJumpCt);
            System.out.printf("%d OF THEM GOT LOWER THAN YOU DID BEFORE THEIR CHUTES\n", betterThanCount);
            System.out.println("OPENED.");
        } else if (betterThanCount <= 0.5 * successfulJumpCt) {
            System.out.printf("NOT BAD.  THERE HAVE BEEN %d SUCCESSFUL JUMPS BEFORE YOURS.\n", successfulJumpCt);
            System.out.printf("YOU WERE BEATEN OUT BY %d OF THEM.\n", betterThanCount);
        } else if (betterThanCount <= 0.75 * successfulJumpCt) {
            System.out.printf("CONSERVATIVE, AREN'T YOU?  YOU RANKED ONLY %d IN THE\n", betterThanCount);
            System.out.printf("%d SUCCESSFUL JUMPS BEFORE YOURS.\n", successfulJumpCt);
        } else if (betterThanCount <= -0.9 * successfulJumpCt) {
            System.out.println("HUMPH!  DON'T YOU HAVE ANY SPORTING BLOOD?  THERE WERE");
            System.out.printf("%d SUCCESSFUL JUMPS BEFORE YOURS AND YOU CAME IN %d JUMPS\n", successfulJumpCt, worseJumpCount);
            System.out.println("BETTER THAN THE WORST.  SHAPE UP!!!\n");
        } else {
            System.out.printf("HEY!  YOU PULLED THE RIP CORD MUCH TOO SOON.  %d SUCCESSFUL\n", successfulJumpCt);
            System.out.printf("JUMPS BEFORE YOURS AND YOU CAME IN NUMBER %d!  GET WITH IT!\n", betterThanCount);
        }
    }

    private void showSplatMessage(InitialJumpConditions initial, JumpResult jump) {
        double timeOfSplat = computeTimeOfSplat(initial, jump);
        System.out.printf("%10.2f  SPLAT\n", timeOfSplat);
    }

    /**
     * Returns the number of jumps for which this jump was better
     */
    private double computeTimeOfSplat(InitialJumpConditions initial, JumpResult jump) {
        final float V = initial.getTerminalVelocity();
        final float A = initial.getAcceleration();
        if (jump.hasReachedTerminalVelocity()) {
            return (V / A) + ((initial.getAltitude() - (V * V / (2 * A))) / V);
        }
        return Math.sqrt(2 * initial.getAltitude() / A);
    }

    private int countWorseHistoricalJumps(JumpResult jump) {
        return (int) pastSuccessfulJumpDistances.stream()
                .filter(distance -> jump.getDistance() < distance)
                .count();
    }

    private void showCleverSplatMessage() {
        List<String> messages = Arrays.asList(
                "REQUIESCAT IN PACE.",
                "MAY THE ANGEL OF HEAVEN LEAD YOU INTO PARADISE.",
                "REST IN PEACE.",
                "SON-OF-A-GUN.",
                "#$%&&%!$",
                "A KICK IN THE PANTS IS A BOOST IF YOU'RE HEADED RIGHT.",
                "HMMM. SHOULD HAVE PICKED A SHORTER TIME.",
                "MUTTER. MUTTER. MUTTER.",
                "PUSHING UP DAISIES.",
                "EASY COME, EASY GO."
        );
        System.out.println(messages.get(random.nextInt(10)));
    }

    private boolean playAgain() {
        if (askYesNo("DO YOU WANT TO PLAY AGAIN ")) {
            return true;
        }
        return askYesNo("PLEASE");
    }

    private float mphToFeetPerSec(float speed) {
        return speed * (5280.0f / 3600.0f);
    }

    private boolean askYesNo(String prompt) {
        System.out.printf("%s (YES OR NO) ", prompt);
        while (true) {
            String answer = scanner.next();
            switch (answer) {
                case "YES":
                    return true;
                case "NO":
                    return false;
                default:
                    System.out.print("YES OR NO ");
            }
        }
    }

    private float chooseRandomAcceleration() {
        Planet planet = Planet.pickRandom();
        System.out.printf("%s %s. ACCELERATION=%.2f FT/SEC/SEC.\n", planet.getMessage(), planet.name(), planet.getAcceleration());
        return planet.getAcceleration();
    }

    enum Planet {
        MERCURY("FINE. YOU'RE ON", 12.2f),
        VENUS("ALL RIGHT. YOU'RE ON", 28.3f),
        EARTH("THEN YOU'RE ON", 32.16f),
        MOON("FINE. YOU'RE ON THE", 5.15f),
        MARS("ALL RIGHT. YOU'RE ON", 12.5f),
        JUPITER("THEN YOU'RE ON", 85.2f),
        SATURN("FINE. YOU'RE ON", 37.6f),
        URANUS("ALL RIGHT. YOU'RE ON", 33.8f),
        NEPTUNE("THEN YOU'RE ON", 39.6f),
        SUN("FINE. YOU'RE ON THE", 896.0f);

        private static final Random random = new Random();
        private final String message;
        private final float acceleration;

        Planet(String message, float acceleration) {
            this.message = message;
            this.acceleration = acceleration;
        }

        static Planet pickRandom() {
            return values()[random.nextInt(Planet.values().length)];
        }

        String getMessage() {
            return message;
        }

        float getAcceleration() {
            return acceleration;
        }
    }

    // Mutable
    static class JumpResult {
        private boolean reachedTerminalVelocity = false;
        private float distance; // from the ground

        public JumpResult(float distance) {
            this.distance = distance;
        }

        boolean isSplat() {
            return distance <= 0;
        }

        boolean hasReachedTerminalVelocity() {
            return reachedTerminalVelocity;
        }

        float getDistance() {
            return distance;
        }

        void setDistance(float distance) {
            this.distance = distance;
        }

        void setReachedTerminalVelocity() {
            reachedTerminalVelocity = true;
        }
    }

    // Immutable
    static class InitialJumpConditions {
        private final float originalTerminalVelocity;
        private final float originalAcceleration;
        private final float terminalVelocity;
        private final float acceleration;
        private final int altitude;

        private InitialJumpConditions(float originalTerminalVelocity, float originalAcceleration,
                                      float terminalVelocity, float acceleration, int altitude) {
            this.originalTerminalVelocity = originalTerminalVelocity;
            this.originalAcceleration = originalAcceleration;
            this.terminalVelocity = terminalVelocity;
            this.acceleration = acceleration;
            this.altitude = altitude;
        }

        // Create initial jump conditions with adjusted velocity/acceleration and a random initial altitude
        private static InitialJumpConditions create(float terminalVelocity, float gravitationalAcceleration) {
            final int altitude = (int) (9001.0f * random.nextFloat() + 1000);
            return new InitialJumpConditions(terminalVelocity, gravitationalAcceleration,
                    plusMinus5Percent(terminalVelocity), plusMinus5Percent(gravitationalAcceleration), altitude);
        }

        private static float plusMinus5Percent(float value) {
            return value + ((value * random.nextFloat()) / 20.0f) - ((value * random.nextFloat()) / 20.0f);
        }

        float getOriginalTerminalVelocity() {
            return originalTerminalVelocity;
        }

        float getOriginalAcceleration() {
            return originalAcceleration;
        }

        float getTerminalVelocity() {
            return terminalVelocity;
        }

        float getAcceleration() {
            return acceleration;
        }

        int getAltitude() {
            return altitude;
        }

        float getTimeOfTerminalAccelerationReached() {
            return terminalVelocity / acceleration;
        }
    }
}