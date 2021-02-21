/**
 * A Player in the game - consists of name and score
 *
 */
public class Player {

    private final String name;

    private int score;

    Player(String name) {
        this.name = name;
        this.score = 0;
    }

    public void addScore(int score) {
        this.score += score;
    }

    public String getName() {
        return name;
    }

    public int getScore() {
        return score;
    }
}
