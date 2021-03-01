"""
ORBIT

Orbital mechanics simulation

Port by Dave LeCompte
"""

import math
import random

PAGE_WIDTH = 64


def print_centered(msg):
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_instructions():
    print(
        """SOMEWHERE ABOVE YOUR PLANET IS A ROMULAN SHIP.

THE SHIP IS IN A CONSTANT POLAR ORBIT.  ITS
DISTANCE FROM THE CENTER OF YOUR PLANET IS FROM
10,000 TO 30,000 MILES AND AT ITS PRESENT VELOCITY CAN
CIRCLE YOUR PLANET ONCE EVERY 12 TO 36 HOURS.

UNFORTUNATELY, THEY ARE USING A CLOAKING DEVICE SO
YOU ARE UNABLE TO SEE THEM, BUT WITH A SPECIAL
INSTRUMENT YOU CAN TELL HOW NEAR THEIR SHIP YOUR
PHOTON BOMB EXPLODED.  YOU HAVE SEVEN HOURS UNTIL THEY
HAVE BUILT UP SUFFICIENT POWER IN ORDER TO ESCAPE
YOUR PLANET'S GRAVITY.

YOUR PLANET HAS ENOUGH POWER TO FIRE ONE BOMB AN HOUR.

AT THE BEGINNING OF EACH HOUR YOU WILL BE ASKED TO GIVE AN
ANGLE (BETWEEN 0 AND 360) AND A DISTANCE IN UNITS OF
100 MILES (BETWEEN 100 AND 300), AFTER WHICH YOUR BOMB'S
DISTANCE FROM THE ENEMY SHIP WILL BE GIVEN.

AN EXPLOSION WITHIN 5,000 MILES OF THE ROMULAN SHIP
WILL DESTROY IT.

BELOW IS A DIAGRAM TO HELP YOU VISUALIZE YOUR PLIGHT.


                          90
                    0000000000000
                 0000000000000000000
               000000           000000
             00000                 00000
            00000    XXXXXXXXXXX    00000
           00000    XXXXXXXXXXXXX    00000
          0000     XXXXXXXXXXXXXXX     0000
         0000     XXXXXXXXXXXXXXXXX     0000
        0000     XXXXXXXXXXXXXXXXXXX     0000
180<== 00000     XXXXXXXXXXXXXXXXXXX     00000 ==>0
        0000     XXXXXXXXXXXXXXXXXXX     0000
         0000     XXXXXXXXXXXXXXXXX     0000
          0000     XXXXXXXXXXXXXXX     0000
           00000    XXXXXXXXXXXXX    00000
            00000    XXXXXXXXXXX    00000
             00000                 00000
               000000           000000
                 0000000000000000000
                    0000000000000
                         270

X - YOUR PLANET
O - THE ORBIT OF THE ROMULAN SHIP

ON THE ABOVE DIAGRAM, THE ROMULAN SHIP IS CIRCLING
COUNTERCLOCKWISE AROUND YOUR PLANET.  DON'T FORGET THAT
WITHOUT SUFFICIENT POWER THE ROMULAN SHIP'S ALTITUDE
AND ORBITAL RATE WILL REMAIN CONSTANT.

GOOD LUCK.  THE FEDERATION IS COUNTING ON YOU.
"""
    )


def get_yes_or_no():
    while True:
        response = input().upper()
        if response == "YES":
            return True
        elif response == "NO":
            return False
        else:
            print("PLEASE TYPE 'YES' OR 'NO'")


def game_over(is_success):
    if is_success:
        print("YOU HAVE SUCCESSFULLY COMPLETED YOUR MISSION.")
    else:
        print("YOU HAVE ALLOWED THE ROMULANS TO ESCAPE.")
    print("ANOTHER ROMULAN SHIP HAS GONE INTO ORBIT.")
    print("DO YOU WISH TO TRY TO DESTROY IT?")

    return get_yes_or_no()


def play_game():
    rom_angle = random.randint(0, 359)
    rom_distance = random.randint(100, 300)
    rom_angular_velocity = random.randint(10, 30)
    hour = 0
    while hour < 7:
        hour += 1
        print()
        print()
        print(f"THIS IS HOUR {hour}, AT WHAT ANGLE DO YOU WISH TO SEND")
        print("YOUR PHOTON BOMB?")

        bomb_angle = float(input())
        print("HOW FAR OUT DO YOU WISH TO DETONATE IT?")
        bomb_distance = float(input())
        print()
        print()

        rom_angle = (rom_angle + rom_angular_velocity) % 360
        angular_difference = rom_angle - bomb_angle
        c = math.sqrt(
            rom_distance ** 2
            + bomb_distance ** 2
            - 2
            * rom_distance
            * bomb_distance
            * math.cos(math.radians(angular_difference))
        )

        print(f"YOUR PHOTON BOMB EXPLODED {c:.4f}*10^2 MILES FROM THE")
        print("ROMULAN SHIP.")

        if c <= 50:
            # Destroyed the Romulan
            return True

    # Ran out of time
    return False


def main():
    print_centered("ORBIT")
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()

    print_instructions()

    while True:
        success = play_game()
        again = game_over(success)
        if not again:
            return


if __name__ == "__main__":
    main()
