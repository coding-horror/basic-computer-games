import java.util.LinkedList;

public class Player {

    private int playerNumber;     // e.g. playerNumber = 1 means "this is Player 1"
    private double currentBet;
    private double insuranceBet;
    private double splitBet;
    private double total;
    private LinkedList<Card> hand;
    private LinkedList<Card> splitHand;

    /**
    * Represents a player in the game with cards, bets, total and a playerNumber. 
    */
    public Player(int playerNumber) {
        this.playerNumber = playerNumber;
        currentBet = 0;
        insuranceBet = 0;
        splitBet = 0;
        total = 0;
        hand = new LinkedList<>();
        splitHand = new LinkedList<>();
    }

    public void setPlayerNumber(int playerNumber) {
        this.playerNumber = playerNumber;
    }

    public int getPlayerNumber() {
        return this.playerNumber;
    }
    
    public void setCurrentBet(double currentBet) {
        this.currentBet = currentBet;
    }

    public double getCurrentBet() {
        return this.currentBet;
    }

    public double getSplitBet() {
        return splitBet;
    }

    public void setSplitBet(double splitBet) {
        this.splitBet = splitBet;
    }

    public double getInsuranceBet() {
        return insuranceBet;
    }

    public void setInsuranceBet(double insuranceBet) {
        this.insuranceBet = insuranceBet;
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
    public double getTotal() {
        return this.total;
    }

    //   dealCard adds the given card to the player's hand
    public void dealCard(Card card) {
        hand.add(card);
    }
    
    public void dealSplitHandCard(Card card) {
        splitHand.add(card);
    }
    /**
     * Removes first card from hand to adds it to split hand
     */
    public void split() {
        splitHand.add(hand.pop());
    }

    //   resetHand resets 'hand' & 'splitHand' to empty lists
    public void resetHand() {
        this.hand = new LinkedList<>();
        this.splitHand = new LinkedList<>();
    }

    public LinkedList<Card> getHand() {
        return this.hand;
    }

    public LinkedList<Card> getSplitHand() {
        return this.splitHand;
    }
}