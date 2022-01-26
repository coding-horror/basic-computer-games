import java.util.LinkedList;

public class Player {

    // TODO add 'playerNumber' property. e.g. playerNumber = 1 means "this is Player 1"
    private int currentBet;
    private int total;
    private LinkedList<Card> hand;
    // TODO we'll need to decide how to deal with a split hand or doubled down bet.

    public Player() {
        // TODO initilize 'total' to zero and 'hand' to an empty List
    }

    public void setCurrentBet(int currentBet) {
        this.currentBet = currentBet;
    }

    public int getCurrentBet() {
        return this.currentBet;
    }

    // TODO replace Player.setTotal with recordWin and recordLoss
    //   recordWin adds 'currentBet' to 'total' and then sets 'currentBet' to zero
    //   recordLoss subtracts 'currentBet' to 'total' and then sets 'currentBet' to zero
    public void setTotal(int total) {
        this.total = total;
    }

    public int getTotal() {
        return this.total;
    }

    // TODO replace Player.setHand with 'dealCard(Card card)' and resetHand()
    //   dealCard adds the given card to the player's hand
    //   resetHand resets 'hand' to an empty list
    public void setHand(LinkedList<Card> hand) {
        this.hand = hand;
    }

    public LinkedList<Card> getHand() {
        return this.hand;
    }
}