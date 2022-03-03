import java.util.Collections;
import java.util.LinkedList;
import java.util.List;

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
        splitHand = null;
    }

    public int getPlayerNumber() {
        return this.playerNumber;
    }
    
    public double getCurrentBet() {
        return this.currentBet;
    }

    public void setCurrentBet(double currentBet) {
        this.currentBet = currentBet;
    }

    public double getSplitBet() {
        return splitBet;
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
        dealCard(card, 1);
    }
    
    public void dealCard(Card card, int handNumber) {
        if(handNumber == 1) {
            hand.add(card);
        } else if (handNumber == 2) {
            splitHand.add(card);
        } else {
            throw new IllegalArgumentException("Invalid hand number " + handNumber);
        }
    }

    public boolean canSplit() {
        if(isSplit()) {
            // Can't split twice
            return false;
        } else {
            boolean isPair = this.hand.get(0).getValue() == this.hand.get(1).getValue();
            return isPair;
        }
    }

    public boolean isSplit() {
        return this.splitHand != null;
    }

    /**
     * Removes first card from hand to add it to new split hand
     */
    public void split() {
        this.splitBet = this.currentBet;
        this.splitHand = new LinkedList<>();
        splitHand.add(hand.pop());
    }

    public boolean canDoubleDown(int handNumber) {
        if(handNumber == 1){
            return this.hand.size() == 2;
        } else if(handNumber == 2){
            return this.splitHand.size() == 2;
        } else {
            throw new IllegalArgumentException("Invalid hand number " + handNumber);
        }
    }

    public void doubleDown(Card card, int handNumber) {
        if(handNumber == 1){
            this.currentBet = this.currentBet * 2;
        } else if(handNumber == 2){
            this.splitBet = this.splitBet * 2;
        } else {
            throw new IllegalArgumentException("Invalid hand number " + handNumber);
        }
        this.dealCard(card, handNumber);
    }

    //   resetHand resets 'hand' & 'splitHand' to empty lists
    public void resetHand() {
        this.hand = new LinkedList<>();
        this.splitHand = null;
    }

    public List<Card> getHand() {
        return getHand(1);
    }

    public List<Card> getHand(int handNumber) {
        if(handNumber == 1){
            return Collections.unmodifiableList(this.hand);
        } else if(handNumber == 2){
            return Collections.unmodifiableList(this.splitHand);
        } else {
            throw new IllegalArgumentException("Invalid hand number " + handNumber);
        }
    }

}