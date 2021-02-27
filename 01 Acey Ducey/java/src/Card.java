/**
 * A card from a deck - the value is between 2-14 to cover
 * cards with a face value of 2-9 and then a Jack, Queen, King, and Ace
 */
public class Card {
    private int value;
    private String name;

    Card(int value) {
        init(value);
    }

    private void init(int value) {
        this.value = value;
        if (value < 11) {
            this.name = String.valueOf(value);
        } else {
            switch (value) {
                case 11:
                    this.name = "Jack";
                    break;
                case 12:
                    this.name = "Queen";
                    break;
                case 13:
                    this.name = "King";
                    break;
                case 14:
                    this.name = "Ace";
                    break;

                default:
                    this.name = "Unknown";
            }
        }
    }

    public int getValue() {
        return value;
    }

    public String getName() {
        return name;
    }
}
