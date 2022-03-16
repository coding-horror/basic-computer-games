import java.lang.Integer;
import java.lang.Math;
import java.util.Scanner;

/**
 * Game of Orbit
 * <p>
 * Based on the BASIC game of Orbit here
 * https://github.com/coding-horror/basic-computer-games/blob/main/68%20Orbit/orbit.bas
 * <p>
 * Note:  The idea was to create a version of the 1970's BASIC game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 *
 * Converted from BASIC to Java by Darren Cardenas.
 */

public class Orbit {

  private final Scanner scan;  // For user input

  public Orbit() {

    scan = new Scanner(System.in);

  }  // End of constructor Orbit

  public void play() {

    showIntro();
    startGame();

  }  // End of method play

  private static void showIntro() {

    System.out.println(" ".repeat(32) + "ORBIT");
    System.out.println(" ".repeat(14) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    System.out.println("\n\n");

    System.out.println("SOMEWHERE ABOVE YOUR PLANET IS A ROMULAN SHIP.");
    System.out.println("");
    System.out.println("THE SHIP IS IN A CONSTANT POLAR ORBIT.  ITS");
    System.out.println("DISTANCE FROM THE CENTER OF YOUR PLANET IS FROM");
    System.out.println("10,000 TO 30,000 MILES AND AT ITS PRESENT VELOCITY CAN");
    System.out.println("CIRCLE YOUR PLANET ONCE EVERY 12 TO 36 HOURS.");
    System.out.println("");
    System.out.println("UNFORTUNATELY, THEY ARE USING A CLOAKING DEVICE SO");
    System.out.println("YOU ARE UNABLE TO SEE THEM, BUT WITH A SPECIAL");
    System.out.println("INSTRUMENT YOU CAN TELL HOW NEAR THEIR SHIP YOUR");
    System.out.println("PHOTON BOMB EXPLODED.  YOU HAVE SEVEN HOURS UNTIL THEY");
    System.out.println("HAVE BUILT UP SUFFICIENT POWER IN ORDER TO ESCAPE");
    System.out.println("YOUR PLANET'S GRAVITY.");
    System.out.println("");
    System.out.println("YOUR PLANET HAS ENOUGH POWER TO FIRE ONE BOMB AN HOUR.");
    System.out.println("");
    System.out.println("AT THE BEGINNING OF EACH HOUR YOU WILL BE ASKED TO GIVE AN");
    System.out.println("ANGLE (BETWEEN 0 AND 360) AND A DISTANCE IN UNITS OF");
    System.out.println("100 MILES (BETWEEN 100 AND 300), AFTER WHICH YOUR BOMB'S");
    System.out.println("DISTANCE FROM THE ENEMY SHIP WILL BE GIVEN.");
    System.out.println("");
    System.out.println("AN EXPLOSION WITHIN 5,000 MILES OF THE ROMULAN SHIP");
    System.out.println("WILL DESTROY IT.");
    System.out.println("");
    System.out.println("BELOW IS A DIAGRAM TO HELP YOU VISUALIZE YOUR PLIGHT.");
    System.out.println("");
    System.out.println("");
    System.out.println("                          90");
    System.out.println("                    0000000000000");
    System.out.println("                 0000000000000000000");
    System.out.println("               000000           000000");
    System.out.println("             00000                 00000");
    System.out.println("            00000    XXXXXXXXXXX    00000");
    System.out.println("           00000    XXXXXXXXXXXXX    00000");
    System.out.println("          0000     XXXXXXXXXXXXXXX     0000");
    System.out.println("         0000     XXXXXXXXXXXXXXXXX     0000");
    System.out.println("        0000     XXXXXXXXXXXXXXXXXXX     0000");
    System.out.println("180<== 00000     XXXXXXXXXXXXXXXXXXX     00000 ==>0");
    System.out.println("        0000     XXXXXXXXXXXXXXXXXXX     0000");
    System.out.println("         0000     XXXXXXXXXXXXXXXXX     0000");
    System.out.println("          0000     XXXXXXXXXXXXXXX     0000");
    System.out.println("           00000    XXXXXXXXXXXXX    00000");
    System.out.println("            00000    XXXXXXXXXXX    00000");
    System.out.println("             00000                 00000");
    System.out.println("               000000           000000");
    System.out.println("                 0000000000000000000");
    System.out.println("                    0000000000000");
    System.out.println("                         270");
    System.out.println("");
    System.out.println("X - YOUR PLANET");
    System.out.println("O - THE ORBIT OF THE ROMULAN SHIP");
    System.out.println("");
    System.out.println("ON THE ABOVE DIAGRAM, THE ROMULAN SHIP IS CIRCLING");
    System.out.println("COUNTERCLOCKWISE AROUND YOUR PLANET.  DON'T FORGET THAT");
    System.out.println("WITHOUT SUFFICIENT POWER THE ROMULAN SHIP'S ALTITUDE");
    System.out.println("AND ORBITAL RATE WILL REMAIN CONSTANT.");
    System.out.println("");
    System.out.println("GOOD LUCK.  THE FEDERATION IS COUNTING ON YOU.");

  }  // End of method showIntro

  private void startGame() {

    double bombDistance = 0;
    int bombAltitude = 0;
    int bombAngle = 0;
    int deltaAngle = 0;
    int hour = 0;
    int shipAltitude = 0;
    int shipAngle = 0;
    int shipRate = 0;
    String userResponse = "";

    // Begin outer while loop
    while (true) {
      shipAngle = (int) (361 * Math.random());
      shipAltitude = (int) (201 * Math.random() + 200);
      shipRate = (int) (21 * Math.random() + 10);

      hour = 0;

      // Begin time limit loop
      while (hour < 7) {

        System.out.println("");
        System.out.println("");
        System.out.println("THIS IS HOUR " + (hour + 1) + ", AT WHAT ANGLE DO YOU WISH TO SEND");
        System.out.print("YOUR PHOTON BOMB? ");
        bombAngle = Integer.parseInt(scan.nextLine());

        System.out.print("HOW FAR OUT DO YOU WISH TO DETONATE IT? ");
        bombAltitude = Integer.parseInt(scan.nextLine());

        System.out.println("");
        System.out.println("");

        // Update ship position
        shipAngle += shipRate;

        // Handle full revolutions
        if (shipAngle >= 360) {
          shipAngle -= 360;
        }

        deltaAngle = Math.abs(shipAngle - bombAngle);

        // Keep angle in upper quadrants
        if (deltaAngle >= 180) {
          deltaAngle = 360 - deltaAngle;
        }

        bombDistance = Math.sqrt(shipAltitude * shipAltitude + bombAltitude * bombAltitude - 2 * shipAltitude
                       * bombAltitude * Math.cos(deltaAngle * Math.PI / 180));

        System.out.format("YOUR PHOTON BOMB EXPLODED " + "%.5f" + "*10^2 MILES FROM THE\n", bombDistance);
        System.out.println("ROMULAN SHIP.");

        // Win condition
        if (bombDistance <= 50) {
          System.out.println("YOU HAVE SUCCESSFULLY COMPLETED YOUR MISSION.");
          break;
        }

        hour++;

      }  // End time limit loop

      // Lose condition
      if (hour == 7) {
        System.out.println("YOU HAVE ALLOWED THE ROMULANS TO ESCAPE.");
      }

      System.out.println("ANOTHER ROMULAN SHIP HAS GONE INTO ORBIT.");
      System.out.print("DO YOU WISH TO TRY TO DESTROY IT? ");
      userResponse = scan.nextLine();

      if (!userResponse.toUpperCase().equals("YES")) {
        System.out.println("GOOD BYE.");
        break;
      }

    }  // End outer while loop

  }  // End of method startGame

  public static void main(String[] args) {

    Orbit game = new Orbit();
    game.play();

  }  // End of method main

}  // End of class Orbit
