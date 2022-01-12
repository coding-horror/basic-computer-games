#!/usr/bin/env python3
#
# Ported to Python by @iamtraction

from math import sin
from random import random


def gunner():
    R = int(40000 * random() + 20000)

    print("\nMAXIMUM RANGE OF YOUR GUN IS", R, "YARDS.")

    Z = 0
    S1 = 0

    while True:
        T = int(R * (.1 + .8 * random()))
        S = 0

        print("\nDISTANCE TO THE TARGET IS", T, "YARDS.")

        while True:
            B = float(input("\n\nELEVATION? "))

            if B > 89:
                print("MAXIMUM ELEVATION IS 89 DEGREES.")
                continue

            if B < 1:
                print("MINIMUM ELEVATION IS ONE DEGREE.")
                continue

            S += 1

            if S < 6:
                B2 = 2 * B / 57.3
                I = R * sin(B2)
                X = T - I
                E = int(X)

                if (abs(E) < 100):
                    print("*** TARGET DESTROYED *** ", S, "ROUNDS OF AMMUNITION EXPENDED.")
                    S1 += S
                    if Z == 4:
                        print("\n\nTOTAL ROUNDS EXPENDED WERE: ", S1)
                        if S1 > 18:
                            print("BETTER GO BACK TO FORT SILL FOR REFRESHER TRAINING!")
                            return
                        else:
                            print("NICE SHOOTING !!")
                            return
                    else:
                        Z += 1
                        print("\nTHE FORWARD OBSERVER HAS SIGHTED MORE ENEMY ACTIVITY...")
                        break
                else:
                    if E > 100:
                        print("SHORT OF TARGET BY", abs(E), "YARDS.")
                    else:
                        print("OVER TARGET BY", abs(E), "YARDS.")
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
        if (Y != "Y"):
            print("\nOK.  RETURN TO BASE CAMP.")
            break
