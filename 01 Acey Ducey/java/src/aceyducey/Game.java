package aceyducey;

/**
 * This class is used to invoke the game.
 *
 */
public class Game {

    private static AceyDucey game;

    public static void main(String[] args) {

        boolean keepPlaying = true;

        // Keep playing game until infinity or the player loses
        do {
            game = new AceyDucey();
            game.play();
            System.out.println();
            System.out.println();
            System.out.println();
            keepPlaying = game.playAgain();
        } while (keepPlaying);
    }
}