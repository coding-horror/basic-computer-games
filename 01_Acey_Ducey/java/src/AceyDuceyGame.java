/**
 * This class is used to invoke the game.
 *
 */
public class AceyDuceyGame {

    public static void main(String[] args) {

        boolean keepPlaying;
        AceyDucey game = new AceyDucey();

        // Keep playing game until infinity or the player loses
        do {
            game.play();
            System.out.println();
            System.out.println();
            System.out.println();
            keepPlaying = game.playAgain();
        } while (keepPlaying);
    }
}