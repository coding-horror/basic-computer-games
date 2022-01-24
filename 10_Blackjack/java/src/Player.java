import java.util.LinkedList;

public class Player {

    private int currentBet;
    private int total;
    private LinkedList<Card> hand; 

    public void setCurrentBet(int currentBet) {
        this.currentBet = currentBet;
    }

    public int getCurrentBet() {
        return this.currentBet;
    }

    public void setTotal(int total) {
        this.total = total;
    }

    public int getTotal() {
        return this.total;
    }

    public void setHand(LinkedList<Card> hand) {
        this.hand = hand;
    }

    public LinkedList<Card> getHand() {
        return this.hand;
    }
}