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
        System.out.println("""
           SINE WAVE
           CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY
           """);
        var isCreative = true;
        for(var t = 0d; t<40; t += .25) {
            //Indent output
            var indentations = 26 + (int) (25 * Math.sin(t));
            System.out.print(" ".repeat(indentations));
            //Change output every iteration
            var word = isCreative ? "CREATIVE" : "COMPUTING";
            System.out.println(word);
            isCreative  = !isCreative ;
        }
    }
}
