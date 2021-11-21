import java.util.Arrays;

/**
 * Sine Wave
 *
 * Based on the Sine Wave program here
 * https://github.com/coding-horror/basic-computer-games/blob/main/78%20Sine%20Wave/sinewave.bas
 *
 * Note:  The idea was to create a version of the 1970's Basic program in Java, without introducing
 *        new features - no additional text, error checking, etc has been added.
 */
public class SineWave {

    public static void main(String[] args) {

        System.out.println("SINE WAVE");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();

        int toggle = 0;
        for(double t = 0; t<40; t += .25) {
            int a = 26 + (int) (25 * Math.sin(t));
            char[] repeat = new char[a];
            Arrays.fill(repeat,' ');
            System.out.print(new String(repeat));
            if (toggle == 1) {
                System.out.println("COMPUTING");
                toggle = 0;
            } else {
                System.out.println("CREATIVE");
                toggle = 1;
            }
        }
    }
}
