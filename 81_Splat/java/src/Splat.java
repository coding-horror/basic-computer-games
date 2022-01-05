import java.util.Arrays;
import java.util.Random;
import java.util.Scanner;

public class Splat {
    private static final Random random = new Random();
    private final Scanner scanner = new Scanner(System.in);

    public static void main(String[] args) {
        new Splat().run();
    }

    public void run() {
        showIntroduction();

        float[] Arr = new float[42];
        Arrays.fill(Arr, 0.0f);
        int K = 0;
        int K1 = 0;
        while (true) {

            InitialJumpConditions jump = buildInitialConditions();
            final float V = jump.getTerminalVelocity();
            final float A = jump.getGravitationalAcceleration();

            System.out.println();
            System.out.printf("    ALTITUDE         = %d FT\n", jump.getAltitude());
            System.out.printf("    TERM. VELOCITY   = %.2f FT/SEC +/-5%%\n", jump.getOriginalTerminalVelocity());
            System.out.printf("    ACCELERATION     = %.2f FT/SEC/SEC +/-5%%\n", jump.getOriginalGravitationalAcceleration());

            System.out.println("SET THE TIMER FOR YOUR FREEFALL.");
            System.out.print("HOW MANY SECONDS ");
            float T = scanner.nextFloat();
            System.out.println("HERE WE GO.\n");
            System.out.println("TIME (SEC)  DIST TO FALL (FT)");
            System.out.println("==========  =================");
            boolean splat = false;
            boolean terminalReached = false;
            float D = 0.0f;
            for (float i = 0.0f; !splat && (i < T); i += T / 8) {
                if (i > jump.getTimeOfTerminalAccelerationReached()) {
                    terminalReached = true;
                    System.out.printf("TERMINAL VELOCITY REACHED AT T PLUS %f SECONDS.\n", jump.getTimeOfTerminalAccelerationReached());
                    for (i = i; i < T; i += T / 8) {
                        D = jump.getAltitude() - ((V * V / (2 * A)) + (V * (i - (V / A))));
                        if (D <= 0) {
                            splat = true;
                            break;
                        }
                        System.out.printf("%10.2f  %f\n", i, D);
                    }
                    break;
                }
                D = jump.getAltitude() - ((A / 2) * i * i);
                if (D <= 0) {
                    splat = true;
                    break;
                }
                System.out.printf("%10.2f  %f\n", i, D);
            }

            if (splat) {
                if (terminalReached) {
                    System.out.printf("%.2f SPLAT\n", (V / A) + ((jump.getAltitude() - (V * V / (2 * A))) / V));
                } else {
                    System.out.printf("%.2f SPLAT\n", Math.sqrt(2 * jump.getAltitude() / A));

                }
                showRandomSplatMessage();
            } else {

                System.out.println("CHUTE OPEN");
                int J = 0;
                for (J = 0; J < 42; J++) {
                    if (Arr[J] == 0) {
                        Arr[J] = D;
                        break;
                    }
                    K = K + 1;
                    if (D > Arr[J]) {
                        continue;
                    }
                    K1 = K1 + 1;
                }

                if (J > 2) {
                    if (K - K1 <= 0.1 * K) {
                        System.out.printf("WOW!  THAT'S SOME JUMPING.  OF THE %d SUCCESSFUL JUMPS\n", K);
                        System.out.printf("BEFORE YOURS, ONLY %d OPENED THEIR CHUTES LOWER THAN\n", K - K1);
                        System.out.println("YOU DID.");
                    } else if (K - K1 <= 0.25 * K) {
                        System.out.printf("PRETTY GOOD!  %d SUCCESSFUL JUMPS PRECEDED YOURS AND ONLY\n", K);
                        System.out.printf("%d OF THEM GOT LOWER THAN YOU DID BEFORE THEIR CHUTES\n", K - K1);
                        System.out.println("OPENED.");
                    } else if (K - K1 <= 0.5 * K) {
                        System.out.printf("NOT BAD.  THERE HAVE BEEN %d SUCCESSFUL JUMPS BEFORE YOURS.\n", K);
                        System.out.printf("YOU WERE BEATEN OUT BY %d OF THEM.\n", K - K1);
                    } else if (K - K1 <= 0.75 * K) {
                        System.out.printf("CONSERVATIVE, AREN'T YOU?  YOU RANKED ONLY %d IN THE\n", K - K1);
                        System.out.printf("%d SUCCESSFUL JUMPS BEFORE YOURS.", K);
                    } else if (K - K1 <= -0.9 * K) {
                        System.out.println("HUMPH!  DON'T YOU HAVE ANY SPORTING BLOOD?  THERE WERE");
                        System.out.printf("%d SUCCESSFUL JUMPS BEFORE YOURS AND YOU CAME IN %d JUMPS\n", K, K1);
                        System.out.println("BETTER THAN THE WORST.  SHAPE UP!!!\n");
                    } else {
                        System.out.printf("HEY!  YOU PULLED THE RIP CORD MUCH TOO SOON.  %f SUCCESSFUL\n", K);
                        System.out.printf("JUMPS BEFORE YOURS AND YOU CAME IN NUMBER %d!  GET WITH IT!\n", K - K1);
                    }
                } else {
                    System.out.println("AMAZING!!! NOT BAD FOR YOUR ");
                    switch (J) {
                        case 0:
                            System.out.print("1ST ");
                            break;
                        case 1:
                            System.out.print("2ND ");
                            break;
                        case 2:
                            System.out.print("3RD ");
                            break;
                    }
                    System.out.println("SUCCESSFUL JUMP!!!");
                }


            }
            boolean chosen = false;
            while (!chosen) {
                System.out.print("DO YOU WANT TO PLAY AGAIN ");
                String Z = scanner.next();
                if (Z.equals("YES")) {
                    break;
                }
                if (Z.equals("NO")) {
                    System.out.print("PLEASE ");
                    while (true) {
                        Z = scanner.next();
                        if (Z.equals("YES")) {
                            chosen = true;
                            break;
                        }
                        if (Z.equals("NO")) {
                            System.out.println("SSSSSSSSSS.");
                            return;
                        }
                        System.out.print("YES OR NO ");
                    }
                    continue;
                } else {
                    System.out.print("YES OR NO ");
                }
            }

        }

    }

    private void showRandomSplatMessage() {
        switch (random.nextInt(10)) {
            case 0:
                System.out.println("REQUIESCAT IN PACE.");
                break;
            case 1:
                System.out.println("MAY THE ANGEL OF HEAVEN LEAD YOU INTO PARADISE.");
                break;
            case 2:
                System.out.println("REST IN PEACE.");
                break;
            case 3:
                System.out.println("SON-OF-A-GUN.");
                break;
            case 4:
                System.out.println("#$%&&%!$");
                break;
            case 5:
                System.out.println("A KICK IN THE PANTS IS A BOOST IF YOU'RE HEADED RIGHT.");
                break;
            case 6:
                System.out.println("HMMM. SHOULD HAVE PICKED A SHORTER TIME.");
                break;
            case 7:
                System.out.println("MUTTER. MUTTER. MUTTER.");
                break;
            case 8:
                System.out.println("PUSHING UP DAISIES.");
                break;
            default:
                System.out.println("EASY COME, EASY GO.");

        }
    }

    private InitialJumpConditions buildInitialConditions() {
        System.out.print("\n\n");
        float terminalVelocity = promptTerminalVelocity();
        float gravitationalAcceleration = promptGravitationalAcceleration();
        return InitialJumpConditions.create(terminalVelocity, gravitationalAcceleration);
    }

    private void showIntroduction() {
        System.out.printf("%33s%s\n", " ", "SPLAT");
        System.out.printf("%15s%s\n", " ", "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.print("\n\n\n");
        System.out.println("WELCOME TO 'SPLAT' -- THE GAME THAT SIMULATES A PARACHUTE");
        System.out.println("JUMP.  TRY TO OPEN YOUR CHUTE AT THE LAST POSSIBLE");
        System.out.println("MOMENT WITHOUT GOING SPLAT.");
    }

    private float promptTerminalVelocity() {
        if(askYesNo("SELECT YOUR OWN TERMINAL VELOCITY")){
            System.out.print("WHAT TERMINAL VELOCITY (MI/HR) ");
            return mphToFeetPerSec(scanner.nextFloat());
        }
        float terminalVelocity = (int) (1000 * random.nextFloat());
        System.out.printf("OK.  TERMINAL VELOCITY = %.2f MI/HR\n", terminalVelocity);
        return mphToFeetPerSec(terminalVelocity);
    }

    private float promptGravitationalAcceleration() {
        if(askYesNo("WANT TO SELECT ACCELERATION DUE TO GRAVITY")){
            System.out.print("WHAT ACCELERATION (FT/SEC/SEC) ");
            return scanner.nextFloat();
        }
        return chooseRandomAcceleration();
    }

    private float mphToFeetPerSec(float speed){
        return speed * (5280.0f / 3600.0f);
    }

    private boolean askYesNo(String prompt){
        System.out.printf("%s (YES OR NO) ", prompt);
        while (true) {
            String answer = scanner.next();
            switch(answer){
                case "YES": return true;
                case "NO": return false;
                default: System.out.print("YES OR NO ");
            }
        }
    }

    private float chooseRandomAcceleration() {
        Planet planet = Planet.pickRandom();
        System.out.printf("%s. ACCELERATION=%.2f FT/SEC/SEC.\n", planet.getMessage(), planet.getAcceleration());
        return planet.getAcceleration();
    }


    // Immutable
    static class InitialJumpConditions {
        private final float originalTerminalVelocity;
        private final float originalGravitationalAcceleration;
        private final float terminalVelocity;
        private final float gravitationalAcceleration;
        private final int altitude;

        private InitialJumpConditions(float originalTerminalVelocity, float originalGravitationalAcceleration,
                                      float terminalVelocity, float gravitationalAcceleration, int altitude) {
            this.originalTerminalVelocity = originalTerminalVelocity;
            this.originalGravitationalAcceleration = originalGravitationalAcceleration;
            this.terminalVelocity = terminalVelocity;
            this.gravitationalAcceleration = gravitationalAcceleration;
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

        public float getOriginalTerminalVelocity() {
            return originalTerminalVelocity;
        }

        public float getOriginalGravitationalAcceleration() {
            return originalGravitationalAcceleration;
        }

        public float getTerminalVelocity() {
            return terminalVelocity;
        }

        public float getGravitationalAcceleration() {
            return gravitationalAcceleration;
        }

        public int getAltitude() {
            return altitude;
        }

        public float getTimeOfTerminalAccelerationReached(){
            return terminalVelocity / gravitationalAcceleration;
        }
    }

    enum Planet {
        MERCURY("FINE. YOU'RE ON MERCURY", 12.2f),
        VENUS("ALL RIGHT. YOU'RE ON VENUS", 28.3f),
        EARTH("THEN YOU'RE ON EARTH", 32.16f),
        MOON("FINE. YOU'RE ON THE MOON", 5.15f),
        MARS("ALL RIGHT. YOU'RE ON MARS", 12.5f),
        JUPITER("THEN YOU'RE ON JUPITER", 85.2f),
        SATURN("FINE. YOU'RE ON SATURN", 37.6f),
        URANUS("ALL RIGHT. YOU'RE ON URANUS", 33.8f),
        NEPTUNE("THEN YOU'RE ON NEPTUNE", 39.6f),
        SUN("FINE. YOU'RE ON THE SUN", 896.0f);

        static final Random random = new Random();
        private final String message;
        private final float acceleration;

        Planet(String message, float acceleration) {
            this.message = message;
            this.acceleration = acceleration;
        }

        static Planet pickRandom(){
            return values()[random.nextInt(Planet.values().length)];
        }

        public String getMessage() {
            return message;
        }

        public float getAcceleration() {
            return acceleration;
        }
    }
}