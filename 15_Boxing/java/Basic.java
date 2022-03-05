import java.util.Scanner;

/**
 * It provide some kind of BASIC language behaviour simulations.
 */
final class Basic {

    public static int randomOf(int base) {
        return (int)Math.round(Math.floor(base* Math.random() + 1));
    }

    /**
     * The Console "simulate" the message error when input does not match with the expected type.
     * Specifically for this game if you enter an String when and int was expected.
     */
    public static class Console {
        private final Scanner input = new Scanner(System.in);

        public String readLine() {
            return input.nextLine();
        }

        public int readInt() {
            int ret = -1;
            boolean failedInput = true;
            do {
                boolean b = input.hasNextInt();
                if (b) {
                    ret = input.nextInt();
                    failedInput = false;
                } else {
                    input.next(); // discard read
                    System.out.print("!NUMBER EXPECTED - RETRY INPUT LINE\n? ");
                }

            } while (failedInput);

            return ret;
        }

        public void print(String message, Object... args) {
            System.out.printf(message, args);
        }
    }
}
