import java.io.PrintStream;
import java.util.HashSet;
import java.util.Random;
import java.util.Scanner;
import java.util.Set;

/**
 * Game of Cube
 * <p>
 * Based on game of Cube at:
 * https://github.com/coding-horror/basic-computer-games/blob/main/30_Cube/cube.bas
 *
 *
 */
public class Cube {

    //Current player location
    private Location playerLocation;

    //Current list of mines
    private Set<Location> mines;

    //System input / output objects
    private PrintStream out;
    private Scanner scanner;

    //Player's current money
    private int money;

    /**
     * Entry point, creates a new Cube object and calls the play method
     * @param args Java execution arguments, not used in application
     */
    public static void main(String[] args) {
        new Cube().play();
    }

    public Cube() {
        out = System.out;
        scanner = new Scanner(System.in);
        money = 500;
        mines = new HashSet<>(5);
    }

    /**
     * Clears mines and places 5 new mines on the board
     */
    private void placeMines() {
        mines.clear();
        Random random = new Random();
        for(int i = 0; i < 5; i++) {
            int x = random.nextInt(1,4);
            int y = random.nextInt(1,4);
            int z = random.nextInt(1,4);
            mines.add(new Location(x,y,z));
        }
    }

    /**
     * Runs the entire game until the player runs out of money or chooses to stop
     */
    public void play() {
        out.println("DO YOU WANT TO SEE INSTRUCTIONS? (YES--1,NO--0)");
        if(readParsedBoolean()) {
            printInstructions();
        }
        do {
            placeMines();
            out.println("WANT TO MAKE A WAGER?");
            int wager = 0 ;

            if(readParsedBoolean()) {
                out.println("HOW MUCH?");
                do {
                    wager = Integer.parseInt(scanner.nextLine());
                    if(wager > money) {
                        out.println("TRIED TO FOOL ME; BET AGAIN");
                    }
                } while(wager > money);
            }

            playerLocation = new Location(1,1,1);
            while(playerLocation.x + playerLocation.y + playerLocation.z != 9) {
                out.println("\nNEXT MOVE");
                String input = scanner.nextLine();

                String[] stringValues = input.split(",");

                if(stringValues.length < 3) {
                    out.println("ILLEGAL MOVE, YOU LOSE.");
                    return;
                }

                int x = Integer.parseInt(stringValues[0]);
                int y = Integer.parseInt(stringValues[1]);
                int z = Integer.parseInt(stringValues[2]);

                Location location = new Location(x,y,z);

                if(x < 1 || x > 3 || y < 1 || y > 3 || z < 1 || z > 3 || !isMoveValid(playerLocation,location)) {
                    out.println("ILLEGAL MOVE, YOU LOSE.");
                    return;
                }

                playerLocation = location;

                if(mines.contains(location)) {
                    out.println("******BANG******");
                    out.println("YOU LOSE!\n\n");
                    money -= wager;
                    break;
                }
            }

            if(wager > 0) {
                out.printf("YOU NOW HAVE %d DOLLARS\n",money);
            }

        } while(money > 0 && doAnotherRound());

        out.println("TOUGH LUCK!");
        out.println("\nGOODBYE.");
    }

    /**
     * Queries the user whether they want to play another round
     * @return True if the player decides to play another round,
     * False if the player would not like to play again
     */
    private boolean doAnotherRound() {
        if(money > 0) {
            out.println("DO YOU WANT TO TRY AGAIN?");
            return readParsedBoolean();
        } else {
            return false;
        }
    }

    /**
     * Prints the instructions to the game, copied from the original code.
     */
    public void printInstructions() {
        out.println("THIS IS A GAME IN WHICH YOU WILL BE PLAYING AGAINST THE");
        out.println("RANDOM DECISION OF THE COMPUTER. THE FIELD OF PLAY IS A");
        out.println("CUBE OF SIDE 3. ANY OF THE 27 LOCATIONS CAN BE DESIGNATED");
        out.println("BY INPUTTING THREE NUMBERS SUCH AS 2,3,1. AT THE START");
        out.println("YOU ARE AUTOMATICALLY AT LOCATION 1,1,1. THE OBJECT OF");
        out.println("THE GAME IS TO GET TO LOCATION 3,3,3. ONE MINOR DETAIL:");
        out.println("THE COMPUTER WILL PICK, AT RANDOM, 5 LOCATIONS AT WHICH");
        out.println("IT WILL PLANT LAND MINES. IF YOU HIT ONE OF THESE LOCATIONS");
        out.println("YOU LOSE. ONE OTHER DETAIL: YOU MAY MOVE ONLY ONE SPACE");
        out.println("IN ONE DIRECTION EACH MOVE. FOR  EXAMPLE: FROM 1,1,2 YOU");
        out.println("MAY MOVE TO 2,1,2 OR 1,1,3. YOU MAY NOT CHANGE");
        out.println("TWO OF THE NUMBERS ON THE SAME MOVE. IF YOU MAKE AN ILLEGAL");
        out.println("MOVE, YOU LOSE AND THE COMPUTER TAKES THE MONEY YOU MAY");
        out.println("\n");
        out.println("ALL YES OR NO QUESTIONS WILL BE ANSWERED BY A 1 FOR YES");
        out.println("OR A 0 (ZERO) FOR NO.");
        out.println();
        out.println("WHEN STATING THE AMOUNT OF A WAGER, PRINT ONLY THE NUMBER");
        out.println("OF DOLLARS (EXAMPLE: 250)  YOU ARE AUTOMATICALLY STARTED WITH");
        out.println("500 DOLLARS IN YOUR ACCOUNT.");
        out.println();
        out.println("GOOD LUCK!");
    }

    /**
     * Waits for the user to input a boolean value. This could either be (true,false), (1,0), (y,n), (yes,no), etc.
     * By default, it will return false
     * @return Parsed boolean value of the user input
     */
    private boolean readParsedBoolean() {
        String in = scanner.nextLine();
        try {
            return in.toLowerCase().charAt(0) == 'y' || Boolean.parseBoolean(in) || Integer.parseInt(in) == 1;
        } catch(NumberFormatException exception) {
            return false;
        }
    }

    /**
     * Checks if a move is valid
     * @param from The point that the player is at
     * @param to The point that the player wishes to move to
     * @return True if the player is only moving, at most, 1 location in any direction, False if the move is invalid
     */
    private boolean isMoveValid(Location from, Location to) {
        return Math.abs(from.x - to.x) + Math.abs(from.y - to.y) + Math.abs(from.z - to.z) <= 1;
    }

    public class Location {
        int x,y,z;

        public Location(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /*
        For use in HashSet and checking if two Locations are the same
         */
        @Override
        public boolean equals(Object o) {
            if (this == o) return true;
            if (o == null || getClass() != o.getClass()) return false;

            Location location = (Location) o;

            if (x != location.x) return false;
            if (y != location.y) return false;
            return z == location.z;
        }

        /*
        For use in the HashSet to accordingly index the set
         */
        @Override
        public int hashCode() {
            int result = x;
            result = 31 * result + y;
            result = 31 * result + z;
            return result;
        }
    }
}
