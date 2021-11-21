/**
 * Game Session
 * The session store the state of the game
 */
public class GameSession {
    private final Player player;
    private final Player opponent;
    private int opponentRoundWins = 0;
    private int playerRoundWins = 0;

    int playerPoints = 0;
    int opponentPoints = 0;
    boolean knocked = false;

    GameSession(Player player, Player opponent) {
        this.player = player;
        this.opponent = opponent;
    }

    public Player getPlayer() { return player;}
    public Player getOpponent() { return opponent;}

    public void setKnocked() {
        knocked = true;
    }

    public void resetPoints() {
        playerPoints = 0;
        opponentPoints = 0;
    }

    public void addPlayerPoints(int ptos) { playerPoints+=ptos;}
    public void addOpponentPoints(int ptos) { opponentPoints+=ptos;}

    public int getPoints(Player player) {
        if(player.isPlayer())
            return playerPoints;
        else
            return opponentPoints;
    }

    public void addRoundWind(Player player) {
        if(player.isPlayer()) playerRoundWins++; else opponentRoundWins++;
    }

    public boolean isOver() {
        return (opponentRoundWins >= 2 || playerRoundWins >= 2);
    }

    public boolean isRoundWinner(Player player) {
        if (player.isPlayer())
            return playerPoints > opponentPoints;
        else
            return opponentPoints > playerPoints;
    }

    public boolean isGameWinner(Player player) {
        if (player.isPlayer())
            return playerRoundWins > 2;
        else
            return opponentRoundWins > 2;
    }

    public boolean isPlayerKnocked() {
        return knocked;
    }
}