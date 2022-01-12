/* A bet has a target (the code entered, which is 1-36, or special values for
 * the various groups, zero and double-zero), and an amount in dollars
 */

public class Bet {
    public int target;
    public int amount;

    /* bet on a target, of an amount */
    public Bet(int on, int of) {
        target = on; amount = of;
    }

    /* check if this is a valid bet - on a real target and of a valid amount */
    public boolean isValid() {
        return ((target > 0) && (target <= 50) &&
                (amount >= 5) && (amount <= 500));
    }
        
    /* utility to return either the odds amount in the case of a win, or zero for a loss */
    private int m(boolean isWon, int odds) {
        return isWon? odds: 0;
    }

    /* look at the wheel to see if this bet won.
     * returns 0 if it didn't, or the odds if it did
     */
    public int winsOn(Wheel w) {
        if (target < 37) {
            // A number bet 1-36 wins at odds of 35 if it is the exact number
            return m(w.isNumber() && (w.number() == target), 35);
        } else
            switch (target) {
            case 37:   // 1-12, odds of 2
                return m(w.isNumber() && (w.number() <= 12), 2);
            case 38:   // 13-24, odds of 2
                return m(w.isNumber() && (w.number() > 12) && (w.number() <= 24), 2);
            case 39:   // 25-36, odds of 2
                return m(w.isNumber() && (w.number() > 24), 2);
            case 40:   // Column 1, odds of 2
                return m(w.isNumber() && ((w.number() % 3) == 1), 2);
            case 41:   // Column 2, odds of 2
                return m(w.isNumber() && ((w.number() % 3) == 2), 2);
            case 42:   // Column 3, odds of 2
                return m(w.isNumber() && ((w.number() % 3) == 0), 2);
            case 43:   // 1-18, odds of 1
                return m(w.isNumber() && (w.number() <= 18), 1);
            case 44:   // 19-36, odds of 1
                return m(w.isNumber() && (w.number() > 18), 1);
            case 45:   // even, odds of 1
                return m(w.isNumber() && ((w.number() %2) == 0), 1);
            case 46:   // odd, odds of 1
                return m(w.isNumber() && ((w.number() %2) == 1), 1);
            case 47:   // red, odds of 1
                return m(w.isNumber() && (w.color() == Wheel.BLACK), 1);
            case 48:   // black, odds of 1
                return m(w.isNumber() && (w.color() == Wheel.RED), 1);
            case 49: // single zero, odds of 35
                return m(w.value().equals("0"), 35);
            case 50: // double zero, odds of 35
                return m(w.value().equals("00"), 35);
            }
        throw new RuntimeException("Program Error - invalid bet");
    }
}
