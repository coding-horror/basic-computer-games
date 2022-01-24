public class Card {

    public enum Suit {
        HEARTS, DIAMONDS, SPADES, CLUBS;
    }

    private int value;
    private Suit suit;

    public void setValue(int value) {
        this.value = value;
    }

    public int getValue() {
        return this.value;
    } 
    
    public void setSuit(Suit suit) {
        this.suit = suit;
    }

    public Suit getSuit() {
        return this.suit;
    }

}