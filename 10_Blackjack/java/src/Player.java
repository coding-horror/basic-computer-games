import java.util.Collections;
import java.util.LinkedList;
import java.util.List;

/**
 * Represents a player and data related to them (number, bets, cards).
 */
public class Player {

    private int playerNumber;     // e.g. playerNumber = 1 means "this is Player 1"
    private double currentBet;
    private double insuranceBet; // 0 when the player has not made an insurance bet (either it does not apply or they chose not to)
    private double splitBet; // 0 whenever the hand is not split
    private double total;
    private LinkedList<Card> hand;
    private LinkedList<Card> splitHand; // null whenever the hand is not split

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
     * Adds 2x the insurance bet to the players total and resets the insurance bet to zero.
     */
    public void recordInsuranceWin() {
        total = total + (insuranceBet * 2);
        insuranceBet = 0;
    }

    /**
     * Subtracts the insurance bet from the players total and resets the insurance bet to zero.
     */
    public void recordInsuranceLoss() {
        total = total - insuranceBet;
        insuranceBet = 0;
    }

    /**
     * Returns the total of all bets won/lost.
     * @return Total value
     */
    public double getTotal() {
        return this.total;
    }

    /**
     * Add the given card to the players main hand.
     * 
     * @param card The card to add.
     */
    public void dealCard(Card card) {
        dealCard(card, 1);
    }
    
    /**
     * Adds the given card to the players hand or split hand depending on the handNumber.
     * 
     * @param card The card to add
     * @param handNumber 1 for the "first" hand and 2 for the "second" hand in a split hand scenario.
     */
    public void dealCard(Card card, int handNumber) {
        if(handNumber == 1) {
            hand.add(card);
        } else if (handNumber == 2) {
            splitHand.add(card);
        } else {
            throw new IllegalArgumentException("Invalid hand number " + handNumber);
        }
    }

    /**
     * Determines whether the player is eligible to split.
     * @return True if the player has not already split, and their hand is a pair. False otherwise.
     */
    public boolean canSplit() {
        if(isSplit()) {
            // Can't split twice
            return false;
        } else {
            boolean isPair = this.hand.get(0).getValue() == this.hand.get(1).getValue();
            return isPair;
        }
    }

    /**
     * Determines whether the player has already split their hand.
     * @return false if splitHand is null, true otherwise.
     */
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

    /**
     * Determines whether the player can double down.
     * 
     * @param handNumber
     * @return
     */
    public boolean canDoubleDown(int handNumber) {
        if(handNumber == 1){
            return this.hand.size() == 2;
        } else if(handNumber == 2){
            return this.splitHand.size() == 2;
        } else {
            throw new IllegalArgumentException("Invalid hand number " + handNumber);
        }
    }

    /**
     * Doubles down on the given hand. Specifically, this method doubles the bet for the given hand and deals the given card.
     * 
     * @param card The card to deal
     * @param handNumber The hand to deal to and double the bet for
     */
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

    /**
     * Resets the hand to an empty list and the splitHand to null.
     */
    public void resetHand() {
        this.hand = new LinkedList<>();
        this.splitHand = null;
    }

    public List<Card> getHand() {
        return getHand(1);
    }

    /**
     * Returns the given hand
     * @param handNumber 1 for the "first" of a split hand (or the main hand when there is no split) or 2 for the "second" hand of a split hand.
     * @return The hand specified by handNumber
     */
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