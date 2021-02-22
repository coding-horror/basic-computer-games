import java.util.HashSet;
import java.util.Scanner;

/**
 * Game of Bombardment
 * <p>
 * Based on the Basic game of Bombardment here
 * https://github.com/coding-horror/basic-computer-games/blob/main/11%20Bombardment/bombardment.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */
public class Bombardment {

    public static final int MAX_GRID_SIZE = 25;
    public static final int PLATOONS = 4;

    private enum GAME_STATE {
        STARTING,
        DRAW_BATTLEFIELD,
        GET_PLAYER_CHOICES,
        PLAYERS_TURN,
        COMPUTER_TURN,
        PLAYER_WON,
        PLAYER_LOST,
        GAME_OVER
    }

    private GAME_STATE gameState;

    public static final String[] PLAYER_HIT_MESSAGES = {"ONE DOWN, THREE TO GO.",
            "TWO DOWN, TWO TO GO.", "THREE DOWN, ONE TO GO."};

    public static final String[] COMPUTER_HIT_MESSAGES = {"YOU HAVE ONLY THREE OUTPOSTS LEFT.",
            "YOU HAVE ONLY TWO OUTPOSTS LEFT.", "YOU HAVE ONLY ONE OUTPOST LEFT."};

    private HashSet<Integer> computersPlatoons;
    private HashSet<Integer> playersPlatoons;

    private HashSet<Integer> computersGuesses;

    // Used for keyboard input
    private final Scanner kbScanner;

    public Bombardment() {

        gameState = GAME_STATE.STARTING;

        // Initialise kb scanner
        kbScanner = new Scanner(System.in);
    }

    /**
     * Main game loop
     */
    public void play() {

        do {
            switch (gameState) {

                // Show an introduction the first time the game is played.
                case STARTING:
                    init();
                    intro();
                    gameState = GAME_STATE.DRAW_BATTLEFIELD;
                    break;

                // Enter the players name
                case DRAW_BATTLEFIELD:
                    drawBattlefield();
                    gameState = GAME_STATE.GET_PLAYER_CHOICES;
                    break;

                // Get the players 4 locations for their platoons
                case GET_PLAYER_CHOICES:
                    String playerChoices = displayTextAndGetInput("WHAT ARE YOUR FOUR POSITIONS? ");

                    // Store the 4 player choices that were entered separated with commas
                    for (int i = 0; i < PLATOONS; i++) {
                        playersPlatoons.add(getDelimitedValue(playerChoices, i));
                    }

                    gameState = GAME_STATE.PLAYERS_TURN;
                    break;

                // Players turn to pick a location
                case PLAYERS_TURN:

                    int firePosition = getDelimitedValue(
                            displayTextAndGetInput("WHERE DO YOU WISH TO FIRE YOUR MISSILE? "), 0);

                    if (didPlayerHitComputerPlatoon(firePosition)) {
                        // Player hit a player platoon
                        int hits = updatePlayerHits(firePosition);
                        // How many hits has the player made?
                        if (hits != PLATOONS) {
                            showPlayerProgress(hits);
                            gameState = GAME_STATE.COMPUTER_TURN;
                        } else {
                            // Player has obtained 4 hits, they win
                            gameState = GAME_STATE.PLAYER_WON;
                        }
                    } else {
                        // Player missed
                        System.out.println("HA, HA YOU MISSED. MY TURN NOW:");
                        System.out.println();
                        gameState = GAME_STATE.COMPUTER_TURN;
                    }

                    break;

                // Computers time to guess a location
                case COMPUTER_TURN:

                    // Computer takes a guess of a location
                    int computerFirePosition = uniqueComputerGuess();
                    if (didComputerHitPlayerPlatoon(computerFirePosition)) {
                        // Computer hit a player platoon
                        int hits = updateComputerHits(computerFirePosition);
                        // How many hits has the computer made?
                        if (hits != PLATOONS) {
                            showComputerProgress(hits, computerFirePosition);
                            gameState = GAME_STATE.PLAYERS_TURN;
                        } else {
                            // Computer has obtained 4 hits, they win
                            System.out.println("YOU'RE DEAD. YOUR LAST OUTPOST WAS AT " + computerFirePosition
                                    + ". HA, HA, HA.");
                            gameState = GAME_STATE.PLAYER_LOST;
                        }
                    } else {
                        // Computer missed
                        System.out.println("I MISSED YOU, YOU DIRTY RAT. I PICKED " + computerFirePosition
                                + ". YOUR TURN:");
                        System.out.println();
                        gameState = GAME_STATE.PLAYERS_TURN;
                    }

                    break;

                // The player won
                case PLAYER_WON:
                    System.out.println("YOU GOT ME, I'M GOING FAST. BUT I'LL GET YOU WHEN");
                    System.out.println("MY TRANSISTO&S RECUP%RA*E!");
                    gameState = GAME_STATE.GAME_OVER;
                    break;

                case PLAYER_LOST:
                    System.out.println("BETTER LUCK NEXT TIME.");
                    gameState = GAME_STATE.GAME_OVER;
                    break;

                // GAME_OVER State does not specifically have a case
            }
        } while (gameState != GAME_STATE.GAME_OVER);
    }

    /**
     * Calculate computer guess.  Make that the computer does not guess the same
     * location twice
     *
     * @return location of the computers guess that has not been guessed previously
     */
    private int uniqueComputerGuess() {

        boolean validGuess = false;
        int computerGuess;
        do {
            computerGuess = randomNumber();

            if (!computersGuesses.contains(computerGuess)) {
                validGuess = true;
            }
        } while (!validGuess);

        computersGuesses.add(computerGuess);

        return computerGuess;
    }

    /**
     * Create four unique platoons locations for the computer
     * We are using a hashset which guarantees uniqueness so
     * all we need to do is keep trying to add a random number
     * until all four are in the hashset
     *
     * @return 4 locations of computers platoons
     */
    private HashSet<Integer> computersChosenPlatoons() {

        // Initialise contents
        HashSet<Integer> tempPlatoons = new HashSet<>();

        boolean allPlatoonsAdded = false;

        do {
            tempPlatoons.add(randomNumber());

            // All four created?
            if (tempPlatoons.size() == PLATOONS) {
                // Exit when we have created four
                allPlatoonsAdded = true;
            }

        } while (!allPlatoonsAdded);

        return tempPlatoons;
    }

    /**
     * Shows a different message for each number of hits
     *
     * @param hits total number of hits by player on computer
     */
    private void showPlayerProgress(int hits) {

        System.out.println("YOU GOT ONE OF MY OUTPOSTS!");
        showProgress(hits, PLAYER_HIT_MESSAGES);
    }

    /**
     * Shows a different message for each number of hits
     *
     * @param hits total number of hits by computer on player
     */
    private void showComputerProgress(int hits, int lastGuess) {

        System.out.println("I GOT YOU. IT WON'T BE LONG NOW. POST " + lastGuess + " WAS HIT.");
        showProgress(hits, COMPUTER_HIT_MESSAGES);
    }

    /**
     * Prints a message from the passed array based on the value of hits
     *
     * @param hits     - number of hits the player or computer has made
     * @param messages - an array of string with messages
     */
    private void showProgress(int hits, String[] messages) {
        System.out.println(messages[hits - 1]);
    }

    /**
     * Update a player hit - adds a hit the player made on the computers platoon.
     *
     * @param fireLocation - computer location that got hit
     * @return number of hits the player has inflicted on the computer in total
     */
    private int updatePlayerHits(int fireLocation) {

        // N.B. only removes if present, so its redundant to check if it exists first
        computersPlatoons.remove(fireLocation);

        // return number of hits in total
        return PLATOONS - computersPlatoons.size();
    }

    /**
     * Update a computer hit - adds a hit the computer made on the players platoon.
     *
     * @param fireLocation - player location that got hit
     * @return number of hits the player has inflicted on the computer in total
     */
    private int updateComputerHits(int fireLocation) {

        // N.B. only removes if present, so its redundant to check if it exists first
        playersPlatoons.remove(fireLocation);

        // return number of hits in total
        return PLATOONS - playersPlatoons.size();
    }

    /**
     * Determine if the player hit one of the computers platoons
     *
     * @param fireLocation the players choice of location to fire on
     * @return true if a computer platoon was at that position
     */
    private boolean didPlayerHitComputerPlatoon(int fireLocation) {
        return computersPlatoons.contains(fireLocation);
    }

    /**
     * Determine if the computer hit one of the players platoons
     *
     * @param fireLocation the computers choice of location to fire on
     * @return true if a players platoon was at that position
     */
    private boolean didComputerHitPlayerPlatoon(int fireLocation) {
        return playersPlatoons.contains(fireLocation);
    }

    /**
     * Draw the battlefield grid
     */
    private void drawBattlefield() {
        for (int i = 1; i < MAX_GRID_SIZE + 1; i += 5) {
            System.out.printf("%-2s %-2s %-2s %-2s %-2s %n", i, i + 1, i + 2, i + 3, i + 4);
        }
    }

    /**
     * Basic information about the game
     */
    private void intro() {
        System.out.println("BOMBARDMENT");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println();
        System.out.println("YOU ARE ON A BATTLEFIELD WITH 4 PLATOONS AND YOU");
        System.out.println("HAVE 25 OUTPOSTS AVAILABLE WHERE THEY MAY BE PLACED.");
        System.out.println("YOU CAN ONLY PLACE ONE PLATOON AT ANY ONE OUTPOST.");
        System.out.println("THE COMPUTER DOES THE SAME WITH ITS FOUR PLATOONS.");
        System.out.println();
        System.out.println("THE OBJECT OF THE GAME IS TO FIRE MISSILES AT THE");
        System.out.println("OUTPOSTS OF THE COMPUTER.  IT WILL DO THE SAME TO YOU.");
        System.out.println("THE ONE WHO DESTROYS ALL FOUR OF THE ENEMY'S PLATOONS");
        System.out.println("FIRST IS THE WINNER.");
        System.out.println();
        System.out.println("GOOD LUCK... AND TELL US WHERE YOU WANT THE BODIES SENT!");
        System.out.println();
        System.out.println("TEAR OFF MATRIX AND USE IT TO CHECK OFF THE NUMBERS.");
        System.out.println();
        System.out.println();
    }

    private void init() {

        // Create four locations for the computers platoons.
        computersPlatoons = computersChosenPlatoons();

        // Players platoons.
        playersPlatoons = new HashSet<>();

        computersGuesses = new HashSet<>();
    }

    /**
     * Accepts a string delimited by comma's and returns the nth delimited
     * value (starting at count 0).
     *
     * @param text - text with values separated by comma's
     * @param pos  - which position to return a value for
     * @return the int representation of the value
     */
    private int getDelimitedValue(String text, int pos) {
        String[] tokens = text.split(",");
        return Integer.parseInt(tokens[pos]);
    }


    /*
     * Print a message on the screen, then accept input from Keyboard.
     *
     * @param text message to be displayed on screen.
     * @return what was typed by the player.
     */
    private String displayTextAndGetInput(String text) {
        System.out.print(text);
        return kbScanner.next();
    }

    /**
     * Generate random number
     *
     * @return random number
     */
    private int randomNumber() {
        return (int) (Math.random()
                * (MAX_GRID_SIZE) + 1);
    }
}