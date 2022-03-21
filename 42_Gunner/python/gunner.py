#!/usr/bin/env python3
#
# Ported to Python by @iamtraction

from math import sin
from random import random


def gunner() -> None:
    gun_range = int(40000 * random() + 20000)

    print("\nMAXIMUM RANGE OF YOUR GUN IS", gun_range, "YARDS.")

    killed_enemies = 0
    S1 = 0

    while True:
        target_distance = int(gun_range * (0.1 + 0.8 * random()))
        shots = 0

        print("\nDISTANCE TO THE TARGET IS", target_distance, "YARDS.")

        while True:
            elevation = float(input("\n\nELEVATION? "))

            if elevation > 89:
                print("MAXIMUM ELEVATION IS 89 DEGREES.")
                continue

            if elevation < 1:
                print("MINIMUM ELEVATION IS ONE DEGREE.")
                continue

            shots += 1

            if shots < 6:
                B2 = 2 * elevation / 57.3
                shot_impact = gun_range * sin(B2)
                shot_proximity = target_distance - shot_impact
                shot_proximity_int = int(shot_proximity)

                if abs(shot_proximity_int) < 100:
                    print(
                        "*** TARGET DESTROYED *** ",
                        shots,
                        "ROUNDS OF AMMUNITION EXPENDED.",
                    )
                    S1 += shots
                    if killed_enemies == 4:
                        print("\n\nTOTAL ROUNDS EXPENDED WERE: ", S1)
                        if S1 > 18:
                            print("BETTER GO BACK TO FORT SILL FOR REFRESHER TRAINING!")
                            return
                        else:
                            print("NICE SHOOTING !!")
                            return
                    else:
                        killed_enemies += 1
                        print(
                            "\nTHE FORWARD OBSERVER HAS SIGHTED MORE ENEMY ACTIVITY..."
                        )
                        break
                else:
                    if shot_proximity_int > 100:
                        print("SHORT OF TARGET BY", abs(shot_proximity_int), "YARDS.")
                    else:
                        print("OVER TARGET BY", abs(shot_proximity_int), "YARDS.")
            else:
                print("\nBOOM !!!!   YOU HAVE JUST BEEN DESTROYED BY THE ENEMY.\n\n\n")
                print("BETTER GO BACK TO FORT SILL FOR REFRESHER TRAINING!")
                return


if __name__ == "__main__":
    print(" " * 33 + "GUNNER")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")

    print("\n\n\n")

    print("YOU ARE THE OFFICER-IN-CHARGE, GIVING ORDERS TO A GUN")
    print("CREW, TELLING THEM THE DEGREES OF ELEVATION YOU ESTIMATE")
    print("WILL PLACE A PROJECTILE ON TARGET.  A HIT WITHIN 100 YARDS")
    print("OF THE TARGET WILL DESTROY IT.")

    while True:
        gunner()

        Y = input("TRY AGAIN (Y OR N)? ")
        if Y != "Y":
            print("\nOK.  RETURN TO BASE CAMP.")
            break
