import java.lang.Math;
import java.util.Scanner;

/**
 * Game of Combat
 * <p>
 * Based on the BASIC game of Combat here
 * https://github.com/coding-horror/basic-computer-games/blob/main/28%20Combat/combat.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 *
 * Converted from BASIC to Java by Darren Cardenas.
 */

public class Combat {

  private static final int MAX_UNITS = 72000;  // Maximum number of total units per player

  private final Scanner scan;  // For user input

  private boolean planeCrashWin = false;

  private int usrArmy = 0;      // Number of user Army units
  private int usrNavy = 0;      // Number of user Navy units
  private int usrAir = 0;       // Number of user Air Force units
  private int cpuArmy = 30000;  // Number of cpu Army units
  private int cpuNavy = 20000;  // Number of cpu Navy units
  private int cpuAir = 22000;   // Number of cpu Air Force units

  public Combat() {

    scan = new Scanner(System.in);

  }  // End of constructor Combat

  public void play() {

    showIntro();
    getForces();
    attackFirst();
    attackSecond();

  }  // End of method play

  private static void showIntro() {

    System.out.println(" ".repeat(32) + "COMBAT");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");
    System.out.println("I AM AT WAR WITH YOU.");
    System.out.println("WE HAVE " + MAX_UNITS + " SOLDIERS APIECE.\n");

  }  // End of method showIntro

  private void getForces() {

    do {
      System.out.println("DISTRIBUTE YOUR FORCES.");
      System.out.println("              ME              YOU");
      System.out.print("ARMY           " + cpuArmy + "        ? ");
      usrArmy = scan.nextInt();
      System.out.print("NAVY           " + cpuNavy + "        ? ");
      usrNavy = scan.nextInt();
      System.out.print("A. F.          " + cpuAir + "        ? ");
      usrAir = scan.nextInt();

    } while ((usrArmy + usrNavy + usrAir) > MAX_UNITS);  // Avoid exceeding the maximum number of total units

  }  // End of method getForces

  private void attackFirst() {

    int numUnits = 0;
    int unitType = 0;

    do {
      System.out.println("YOU ATTACK FIRST. TYPE (1) FOR ARMY; (2) FOR NAVY;");
      System.out.println("AND (3) FOR AIR FORCE.");
      System.out.print("? ");
      unitType = scan.nextInt();
    } while ((unitType < 1) || (unitType > 3));  // Avoid out-of-range values

    do {
      System.out.println("HOW MANY MEN");
      System.out.print("? ");
      numUnits = scan.nextInt();
    } while ((numUnits < 0) ||                // Avoid negative values
             ((unitType == 1) && (numUnits > usrArmy)) ||  // Avoid exceeding the number of available Army units
             ((unitType == 2) && (numUnits > usrNavy)) ||  // Avoid exceeding the number of available Navy units
             ((unitType == 3) && (numUnits > usrAir)));    // Avoid exceeding the number of available Air Force units

    // Begin handling deployment type
    switch (unitType) {
      case 1:  // Army deployed

        if (numUnits < (usrArmy / 3.0)) {  // User deployed less than one-third of their Army units
          System.out.println("YOU LOST " + numUnits + " MEN FROM YOUR ARMY.");
          usrArmy = usrArmy - numUnits;
        }
        else if (numUnits < (2.0 * usrArmy / 3.0)) {  // User deployed less than two-thirds of their Army units
          System.out.println("YOU LOST " + (int) Math.floor(numUnits / 3.0) + " MEN, BUT I LOST " + (int) Math.floor(2.0 * cpuArmy / 3.0));
          usrArmy = (int) Math.floor(usrArmy - numUnits / 3.0);
          cpuArmy = 0;
        }
        else {  // User deployed two-thirds or more of their Army units
          System.out.println("YOU SUNK ONE OF MY PATROL BOATS, BUT I WIPED OUT TWO");
          System.out.println("OF YOUR AIR FORCE BASES AND 3 ARMY BASES.");
          usrArmy = (int) Math.floor(usrArmy / 3.0);
          usrAir = (int) Math.floor(usrAir / 3.0);
          cpuNavy = (int) Math.floor(2.0 * cpuNavy / 3.0);
        }
        break;

      case 2:  // Navy deployed

        if (numUnits < (cpuNavy / 3.0)) {  // User deployed less than one-third relative to cpu Navy units
          System.out.println("YOUR ATTACK WAS STOPPED!");
          usrNavy = usrNavy - numUnits;
        }
        else if (numUnits < (2.0 * cpuNavy / 3.0)) {  // User deployed less than two-thirds relative to cpu Navy units
          System.out.println("YOU DESTROYED " + (int) Math.floor(2.0 * cpuNavy / 3.0) + " OF MY ARMY.");
          cpuNavy = (int) Math.floor(cpuNavy / 3.0);
        }
        else {  // User deployed two-thirds or more relative to cpu Navy units
          System.out.println("YOU SUNK ONE OF MY PATROL BOATS, BUT I WIPED OUT TWO");
          System.out.println("OF YOUR AIR FORCE BASES AND 3 ARMY BASES.");
          usrArmy = (int) Math.floor(usrArmy / 3.0);
          usrAir = (int) Math.floor(usrAir / 3.0);
          cpuNavy = (int) Math.floor(2.0 * cpuNavy / 3.0);
        }
        break;

      case 3:  // Air Force deployed

        if (numUnits < (usrAir / 3.0)) {  // User deployed less than one-third of their Air Force units
          System.out.println("YOUR ATTACK WAS WIPED OUT.");
          usrAir = usrAir - numUnits;
        }
        else if (numUnits < (2.0 * usrAir / 3.0)) {  // User deployed less than two-thirds of their Air Force units
          System.out.println("WE HAD A DOGFIGHT. YOU WON - AND FINISHED YOUR MISSION.");
          cpuArmy = (int) Math.floor(2.0 * cpuArmy / 3.0);
          cpuNavy = (int) Math.floor(cpuNavy / 3.0);
          cpuAir = (int) Math.floor(cpuAir / 3.0);
        }
        else {  // User deployed two-thirds or more of their Air Force units
          System.out.println("YOU WIPED OUT ONE OF MY ARMY PATROLS, BUT I DESTROYED");
          System.out.println("TWO NAVY BASES AND BOMBED THREE ARMY BASES.");
          usrArmy = (int) Math.floor(usrArmy / 4.0);
          usrNavy = (int) Math.floor(usrNavy / 3.0);
          cpuArmy = (int) Math.floor(2.0 * cpuArmy / 3.0);
        }
        break;

    }  // End handling deployment type

  }  // End of method attackFirst

  private void attackSecond() {

    int numUnits = 0;
    int unitType = 0;

    System.out.println("");
    System.out.println("              YOU           ME");
    System.out.print("ARMY           ");
    System.out.format("%-14s%s\n", usrArmy, cpuArmy);
    System.out.print("NAVY           ");
    System.out.format("%-14s%s\n", usrNavy, cpuNavy);
    System.out.print("A. F.          ");
    System.out.format("%-14s%s\n", usrAir, cpuAir);

    do {
      System.out.println("WHAT IS YOUR NEXT MOVE?");
      System.out.println("ARMY=1  NAVY=2  AIR FORCE=3");
      System.out.print("? ");
      unitType = scan.nextInt();
    } while ((unitType < 1) || (unitType > 3));  // Avoid out-of-range values

    do {
      System.out.println("HOW MANY MEN");
      System.out.print("? ");
      numUnits = scan.nextInt();
    } while ((numUnits < 0) ||                // Avoid negative values
             ((unitType == 1) && (numUnits > usrArmy)) ||  // Avoid exceeding the number of available Army units
             ((unitType == 2) && (numUnits > usrNavy)) ||  // Avoid exceeding the number of available Navy units
             ((unitType == 3) && (numUnits > usrAir)));    // Avoid exceeding the number of available Air Force units

    // Begin handling deployment type
    switch (unitType) {
      case 1:  // Army deployed

        if (numUnits < (cpuArmy / 2.0)) {  // User deployed less than half relative to cpu Army units
          System.out.println("I WIPED OUT YOUR ATTACK!");
          usrArmy = usrArmy - numUnits;
        }
        else {  // User deployed half or more relative to cpu Army units
          System.out.println("YOU DESTROYED MY ARMY!");
          cpuArmy = 0;
        }
        break;

      case 2:  // Navy deployed

        if (numUnits < (cpuNavy / 2.0)) {  // User deployed less than half relative to cpu Navy units
          System.out.println("I SUNK TWO OF YOUR BATTLESHIPS, AND MY AIR FORCE");
          System.out.println("WIPED OUT YOUR UNGUARDED CAPITOL.");
          usrArmy = (int) Math.floor(usrArmy / 4.0);
          usrNavy = (int) Math.floor(usrNavy / 2.0);
        }
        else { // User deployed half or more relative to cpu Navy units
          System.out.println("YOUR NAVY SHOT DOWN THREE OF MY XIII PLANES,");
          System.out.println("AND SUNK THREE BATTLESHIPS.");
          cpuAir = (int) Math.floor(2.0 * cpuAir / 3.0);
          cpuNavy = (int) Math.floor(cpuNavy / 2.0);
        }
        break;

      case 3:  // Air Force deployed

        if (numUnits > (cpuAir / 2.0)) {  // User deployed more than half relative to cpu Air Force units
          System.out.println("MY NAVY AND AIR FORCE IN A COMBINED ATTACK LEFT");
          System.out.println("YOUR COUNTRY IN SHAMBLES.");
          usrArmy = (int) Math.floor(usrArmy / 3.0);
          usrNavy = (int) Math.floor(usrNavy / 3.0);
          usrAir = (int) Math.floor(usrAir / 3.0);
        }
        else {  // User deployed half or less relative to cpu Air Force units
          System.out.println("ONE OF YOUR PLANES CRASHED INTO MY HOUSE. I AM DEAD.");
          System.out.println("MY COUNTRY FELL APART.");
          planeCrashWin = true;
        }
        break;

    }  // End handling deployment type

    // Suppress message for plane crashes
    if (planeCrashWin == false) {
      System.out.println("");
      System.out.println("FROM THE RESULTS OF BOTH OF YOUR ATTACKS,");
    }

    // User wins
    if ((planeCrashWin == true) ||
        ((usrArmy + usrNavy + usrAir) > ((int) Math.floor((3.0 / 2.0 * (cpuArmy + cpuNavy + cpuAir)))))) {
      System.out.println("YOU WON, OH! SHUCKS!!!!");
    }
    // User loses
    else if ((usrArmy + usrNavy + usrAir) < ((int) Math.floor((2.0 / 3.0 * (cpuArmy + cpuNavy + cpuAir))))) {  // User loss
      System.out.println("YOU LOST-I CONQUERED YOUR COUNTRY.  IT SERVES YOU");
      System.out.println("RIGHT FOR PLAYING THIS STUPID GAME!!!");
    }
    // Peaceful outcome
    else {
      System.out.println("THE TREATY OF PARIS CONCLUDED THAT WE TAKE OUR");
      System.out.println("RESPECTIVE COUNTRIES AND LIVE IN PEACE.");
    }

  }  // End of method attackSecond

  public static void main(String[] args) {

    Combat combat = new Combat();
    combat.play();

  }  // End of method main

}  // End of class Combat
