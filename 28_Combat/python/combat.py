MAX_UNITS = 72000
plane_crash_win = False
usr_army = 0
usr_navy = 0
usr_air = 0
cpu_army = 30000
cpu_navy = 20000
cpu_air = 22000


def show_intro() -> None:
    global MAX_UNITS

    print(" " * 32 + "COMBAT")
    print(" " * 14 + "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY")
    print("\n\n")
    print("I AM AT WAR WITH YOU.")
    print("WE HAVE " + str(MAX_UNITS) + " SOLDIERS APIECE.")


def get_forces() -> None:
    global usr_army, usr_navy, usr_air

    while True:
        print("DISTRIBUTE YOUR FORCES.")
        print("              ME              YOU")
        print("ARMY           " + str(cpu_army) + "        ? ", end="")
        usr_army = int(input())
        print("NAVY           " + str(cpu_navy) + "        ? ", end="")
        usr_navy = int(input())
        print("A. F.          " + str(cpu_air) + "        ? ", end="")
        usr_air = int(input())
        if (usr_army + usr_navy + usr_air) <= MAX_UNITS:
            break


def attack_first() -> None:
    global usr_army, usr_navy, usr_air
    global cpu_army, cpu_navy, cpu_air

    num_units = 0
    unit_type = 0

    while True:
        print("YOU ATTACK FIRST. TYPE (1) FOR ARMY; (2) FOR NAVY;")
        print("AND (3) FOR AIR FORCE.")
        print("?", end=" ")
        unit_type = int(input())
        if not (unit_type < 1 or unit_type > 3):
            break

    while True:
        print("HOW MANY MEN")
        print("?", end=" ")
        num_units = int(input())
        if not (
            (num_units < 0)
            or ((unit_type == 1) and (num_units > usr_army))
            or ((unit_type == 2) and (num_units > usr_navy))
            or ((unit_type == 3) and (num_units > usr_air))
        ):
            break

    if unit_type == 1:
        if num_units < (usr_army / 3):
            print("YOU LOST " + str(num_units) + " MEN FROM YOUR ARMY.")
            usr_army = usr_army - num_units
        elif num_units < (2 * usr_army / 3):
            print(
                "YOU LOST "
                + str(int(num_units / 3))
                + " MEN, BUT I LOST "
                + str(int(2 * cpu_army / 3))
            )
            usr_army = int(usr_army - (num_units / 3))
            cpu_army = 0
        else:
            print("YOU SUNK ONE OF MY PATROL BOATS, BUT I WIPED OUT TWO")
            print("OF YOUR AIR FORCE BASES AND 3 ARMY BASES.")
            usr_army = int(usr_army / 3)
            usr_air = int(usr_air / 3)
            cpu_navy = int(2 * cpu_navy / 3)
    elif unit_type == 2:
        if num_units < cpu_navy / 3:
            print("YOUR ATTACK WAS STOPPED!")
            usr_navy = usr_navy - num_units
        elif num_units < 2 * cpu_navy / 3:
            print("YOU DESTROYED " + str(int(2 * cpu_navy / 3)) + " OF MY ARMY.")
            cpu_navy = int(cpu_navy / 3)
        else:
            print("YOU SUNK ONE OF MY PATROL BOATS, BUT I WIPED OUT TWO")
            print("OF YOUR AIR FORCE BASES AND 3 ARMY BASES.")
            usr_army = int(usr_army / 3)
            usr_air = int(usr_air / 3)
            cpu_navy = int(2 * cpu_navy / 3)
    elif unit_type == 3:
        if num_units < usr_air / 3:
            print("YOUR ATTACK WAS WIPED OUT.")
            usr_air = usr_air - num_units
        elif num_units < 2 * usr_air / 3:
            print("WE HAD A DOGFIGHT. YOU WON - AND FINISHED YOUR MISSION.")
            cpu_army = int(2 * cpu_army / 3)
            cpu_navy = int(cpu_navy / 3)
            cpu_air = int(cpu_air / 3)
        else:
            print("YOU WIPED OUT ONE OF MY ARMY PATROLS, BUT I DESTROYED")
            print("TWO NAVY BASES AND BOMBED THREE ARMY BASES.")
            usr_army = int(usr_army / 4)
            usr_navy = int(usr_navy / 3)
            cpu_army = int(2 * cpu_army / 3)


def attack_second() -> None:
    global usr_army, usr_navy, usr_air, cpu_army, cpu_navy, cpu_air
    global plane_crash_win
    num_units = 0
    unit_type = 0

    print("")
    print("              YOU           ME")
    print("ARMY           ", end="")
    print("%-14s%s\n" % (usr_army, cpu_army), end="")
    print("NAVY           ", end="")
    print("%-14s%s\n" % (usr_navy, cpu_navy), end="")
    print("A. F.          ", end="")
    print("%-14s%s\n" % (usr_air, cpu_air), end="")

    while True:
        print("WHAT IS YOUR NEXT MOVE?")
        print("ARMY=1  NAVY=2  AIR FORCE=3")
        print("? ", end="")
        unit_type = int(input())
        if not ((unit_type < 1) or (unit_type > 3)):
            break

    while True:
        print("HOW MANY MEN")
        print("? ", end="")
        num_units = int(input())
        if not (
            (num_units < 0)
            or ((unit_type == 1) and (num_units > usr_army))
            or ((unit_type == 2) and (num_units > usr_navy))
            or ((unit_type == 3) and (num_units > usr_air))
        ):
            break

    if unit_type == 1:
        if num_units < (cpu_army / 2):
            print("I WIPED OUT YOUR ATTACK!")
            usr_army = usr_army - num_units
        else:
            print("YOU DESTROYED MY ARMY!")
            cpu_army = 0
    elif unit_type == 2:
        if num_units < (cpu_navy / 2):
            print("I SUNK TWO OF YOUR BATTLESHIPS, AND MY AIR FORCE")
            print("WIPED OUT YOUR UNGUARDED CAPITOL.")
            usr_army = int(usr_army / 4)
            usr_navy = int(usr_navy / 2)
        else:
            print("YOUR NAVY SHOT DOWN THREE OF MY XIII PLANES,")
            print("AND SUNK THREE BATTLESHIPS.")
            cpu_air = int(2 * cpu_air / 3)
            cpu_navy = int(cpu_navy / 2)
    elif unit_type == 3:
        if num_units > (cpu_air / 2):
            print("MY NAVY AND AIR FORCE IN A COMBINED ATTACK LEFT")
            print("YOUR COUNTRY IN SHAMBLES.")
            usr_army = int(usr_army / 3)
            usr_navy = int(usr_navy / 3)
            usr_air = int(usr_air / 3)
        else:
            print("ONE OF YOUR PLANES CRASHED INTO MY HOUSE. I AM DEAD.")
            print("MY COUNTRY FELL APART.")
            plane_crash_win = True

    if not plane_crash_win:
        print("")
        print("FROM THE RESULTS OF BOTH OF YOUR ATTACKS,")

    if plane_crash_win or (
        (usr_army + usr_navy + usr_air) > (int(3 / 2 * (cpu_army + cpu_navy + cpu_air)))
    ):
        print("YOU WON, OH! SHUCKS!!!!")
    elif (usr_army + usr_navy + usr_air) < int(2 / 3 * (cpu_army + cpu_navy + cpu_air)):
        print("YOU LOST-I CONQUERED YOUR COUNTRY.  IT SERVES YOU")
        print("RIGHT FOR PLAYING THIS STUPID GAME!!!")
    else:
        print("THE TREATY OF PARIS CONCLUDED THAT WE TAKE OUR")
        print("RESPECTIVE COUNTRIES AND LIVE IN PEACE.")


def main() -> None:
    show_intro()
    get_forces()
    attack_first()
    attack_second()


if __name__ == "__main__":
    main()
