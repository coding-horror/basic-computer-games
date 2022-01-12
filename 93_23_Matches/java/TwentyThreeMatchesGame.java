/**
 * Game of 23 Matches
 * <p>
 * Based on the BASIC game of 23 Matches here
 * https://github.com/coding-horror/basic-computer-games/blob/main/93%2023%20Matches/23matches.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 * <p>
 * Converted from BASIC to Java by Darren Cardenas.
 */
public class TwentyThreeMatchesGame {

    public static void main(String[] args) {
        showIntro();
        TwentyThreeMatches game = new TwentyThreeMatches();
        game.startGame();
    }

    private static void showIntro() {
        System.out.println(Messages.INTRO);
    }

}
