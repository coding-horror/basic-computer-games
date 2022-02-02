import java.util.LinkedList;

public class Player {

    private int playerNumber;     // e.g. playerNumber = 1 means "this is Player 1"
    private int currentBet;
    private int total;
    private LinkedList<Card> hand;
    // TODO we'll need to decide how to deal with a split hand or doubled down bet.

    /**
    * Represents a player in the game with cards, bets, total and a playerNumber. 
    */
    public Player(int playerNumber) {
        this.playerNumber = playerNumber;
        currentBet = 0;
        total = 0;
        hand = new LinkedList<>();
    }

    public void setPlayerNumber(int playerNumber) { //TODO: Is this needed if set in constructor?
        this.playerNumber = playerNumber;
    }

    public int getPlayerNumber() {
        return this.playerNumber;
    }
    
    public void setCurrentBet(int currentBet) {
        this.currentBet = currentBet;
    }

    public int getCurrentBet() {
        return this.currentBet;
    }

    /**
    * RecordWin adds 'currentBet' to 'total' and then sets 'currentBet' to zero
    */
    public void recordWin() {
        this.total = this.total + this.currentBet;
        this.currentBet = 0;
    }
    /**
    * RecordLoss subtracts 'currentBet' to 'total' and then sets 'currentBet' to zero
    */
    public void recordLoss() {
        total = total - currentBet;
        currentBet = 0;
    }
    /**
     * Returns the total of all bets won.
     * @return Total value
     */
    public int getTotal() {
        return this.total;
    }

    //   dealCard adds the given card to the player's hand
    public void dealCard(Card card) {
        hand.add(card);
    }

    //   resetHand resets 'hand' to an empty list
    public void resetHand() {
        this.hand = new LinkedList<>();
    }

    public LinkedList<Card> getHand() {
        return this.hand;
    }
}