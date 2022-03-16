import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintStream;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.Random;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.time.format.FormatStyle;

public class Roulette {
    public static void main(String args[]) throws Exception {
        Roulette r = new Roulette();
        r.play();
    }

    private BufferedReader reader;
    private PrintStream writer;

    private int house;      // how much money does the house have
    private int player;     // how much money does the player have
    private Wheel wheel = new Wheel();

    public Roulette() {
        reader = new BufferedReader(new InputStreamReader(System.in));
        writer = System.out;
        house = 100000;
        player = 1000;
    }

    // for a test / cheat mode -- set the random number generator to a known value
    private void setSeed(long l) {
        wheel.setSeed(l);
    }

    public void play() {
        try {
            intro();
            writer.println("WELCOME TO THE ROULETTE TABLE\n" +
                           "DO YOU WANT INSTRUCTIONS");
            String instr = reader.readLine();
            if (!instr.toUpperCase().startsWith("N"))
                instructions();

            while (betAndSpin()) { // returns true if the game is to continue
            }

            if (player <= 0) {
                // player ran out of money
                writer.println("THANKS FOR YOUR MONEY.\nI'LL USE IT TO BUY A SOLID GOLD ROULETTE WHEEL");
            } else {
                // player has money -- print them a check
                writer.println("TO WHOM SHALL I MAKE THE CHECK");

                String payee = reader.readLine();

                writer.println("-".repeat(72));
                tab(50); writer.println("CHECK NO. " + (new Random().nextInt(100) + 1));
                writer.println();
                tab(40); writer.println(LocalDate.now().format(DateTimeFormatter.ofLocalizedDate(FormatStyle.LONG)));
                writer.println("\n\nPAY TO THE ORDER OF-----" + payee + "-----$ " + player);
                writer.print("\n\n");
                tab(10); writer.println("THE MEMORY BANK OF NEW YORK\n");
                tab(40); writer.println("THE COMPUTER");
                tab(40); writer.println("----------X-----\n");
                writer.println("-".repeat(72));
                writer.println("COME BACK SOON!\n");
            }
        }
        catch (IOException e) {
            // this should not happen
            System.err.println("System error:\n" + e);
        }
    }

    /* Write the starting introduction */
    private void intro() throws IOException {
        tab(32); writer.println("ROULETTE");
        tab(15); writer.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n");
    }

    /* Display the game instructions */
    private void instructions() {
        String[] instLines = new String[] {
            "THIS IS THE BETTING LAYOUT",
            "  (*=RED)",
            "" ,
            " 1*    2     3*",
            " 4     5*    6 ",
            " 7*    8     9*",
            "10    11    12*",
            "---------------",
            "13    14*   15 ",
            "16*   17    18*",
            "19*   20    21*",
            "22    23*   24 ",
            "---------------",
            "25*   26    27*",
            "28    29    30*",
            "31    32*   33 ",
            "34*   35    36*",
            "---------------",
            "    00    0    ",
            "" ,
            "TYPES OF BETS",
            ""  ,
            "THE NUMBERS 1 TO 36 SIGNIFY A STRAIGHT BET",
            "ON THAT NUMBER.",
            "THESE PAY OFF 35:1",
            ""  ,
            "THE 2:1 BETS ARE:",
            " 37) 1-12     40) FIRST COLUMN",
            " 38) 13-24    41) SECOND COLUMN",
            " 39) 25-36    42) THIRD COLUMN",
            ""  ,
            "THE EVEN MONEY BETS ARE:",
            " 43) 1-18     46) ODD",
            " 44) 19-36    47) RED",
            " 45) EVEN     48) BLACK",
            "",
            " 49)0 AND 50)00 PAY OFF 35:1",
            " NOTE: 0 AND 00 DO NOT COUNT UNDER ANY",
            "       BETS EXCEPT THEIR OWN.",
            "",
            "WHEN I ASK FOR EACH BET, TYPE THE NUMBER",
            "AND THE AMOUNT, SEPARATED BY A COMMA.",
            "FOR EXAMPLE: TO BET $500 ON BLACK, TYPE 48,500",
            "WHEN I ASK FOR A BET.",
            "",
            "THE MINIMUM BET IS $5, THE MAXIMUM IS $500.",
            "" };
        writer.println(String.join("\n", instLines));
    }

    /* Take a set of bets from the player, then spin the wheel and work out the winnings *
     * This returns true if the game is to continue afterwards
     */
    private boolean betAndSpin() throws IOException {
        int betCount = 0;

        while (betCount == 0) {   // keep asking how many bets until we get a good answer
            try {
                writer.println("HOW MANY BETS");
                String howMany = reader.readLine();
                betCount = Integer.parseInt(howMany.strip());

                if ((betCount < 1) || (betCount > 100)) betCount = 0; // bad -- set zero and ask again
            }
            catch (NumberFormatException e) {
                // this happens if the input is not a number
                writer.println("INPUT ERROR");
            }
        }

        HashSet<Integer> betsMade = new HashSet<>(); // Bet targets already made, so we can spot repeats
        ArrayList<Bet> bets = new ArrayList<>();     // All the bets for this round

        while (bets.size() < betCount) {
            Bet bet = new Bet(0, 0);                 // an invalid bet to hold the place
            while (!bet.isValid()) {                 // keep asking until it is valid
                try {
                    writer.println("NUMBER " + (bets.size() + 1));
                    String fields[] = reader.readLine().split(",");
                    if (fields.length == 2) {
                        bet = new Bet(Integer.parseInt(fields[0].strip()),
                                      Integer.parseInt(fields[1].strip()));
                    }
                }
                catch (NumberFormatException e) {
                    writer.println("INPUT ERROR");
                }
            }

            // Check if there is already a bet on the same target
            if (betsMade.contains(bet.target)) {
                writer.println("YOU MADE THAT BET ONCE ALREADY,DUM-DUM");
            } else {
                betsMade.add(bet.target); // note this target has now been bet on
                bets.add(bet);
            }
        }

        writer.println("SPINNING\n\n");

        wheel.spin(); // this deliberately takes some random amount of time

        writer.println(wheel.value());

        // go through the bets, and evaluate each one
        int betNumber = 1;
        for (Bet b : bets) {
            int multiplier = b.winsOn(wheel);
            if (multiplier == 0) {
                // lost the amount of the bet
                writer.println("YOU LOSE " + b.amount + " DOLLARS ON BET " + betNumber);
                house += b.amount;
                player -= b.amount;
            } else {
                // won the amount of the bet, multiplied by the odds
                int winnings = b.amount * multiplier;
                writer.println("YOU WIN " + winnings + " DOLLARS ON BET " + betNumber);
                house -= winnings;
                player += winnings;
            }
            ++betNumber;
        }

        writer.println("\nTOTALS:\tME\tYOU\n\t" + house + "\t" + player);

        if (player <= 0) {
            writer.println("OOPS! YOU JUST SPENT YOUR LAST DOLLAR");
            return false;     // do not repeat since the player has no more money
        }
        if (house <= 0) {
            writer.println("YOU BROKE THE HOUSE!");
            player = 101000;  // can't win more than the house started with
            return false;     // do not repeat since the house has no more money
        }

        // player still has money, and the house still has money, so ask the player
        // if they want to continue
        writer.println("AGAIN");
        String doContinue = reader.readLine();

        // repeat if the answer was not "n" or "no"
        return (!doContinue.toUpperCase().startsWith("N"));
    }

    // utility to print n spaces for formatting
    private void tab(int n) {
        writer.print(" ".repeat(n));
    }
}
