MAX_UNITS = 72000
planeCrashWin = False
usrArmy = 0
usrNavy = 0
usrAir = 0
cpuArmy = 30000
cpuNavy = 20000
cpuAir = 22000


def showIntro():
    global MAX_UNITS

    print(" " * 32 + "COMBAT")
    print(" " * 14 + "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY")
    print("\n\n")
    print("I AM AT WAR WITH YOU.")
    print("WE HAVE " + str(MAX_UNITS) + " SOLDIERS APIECE.")


def getForces():
    global usrArmy, usrNavy, usrAir

    while True:
        print("DISTRIBUTE YOUR FORCES.")
        print("              ME              YOU")
        print("ARMY           " + str(cpuArmy) + "        ? ", end="")
        usrArmy = int(input())
        print("NAVY           " + str(cpuNavy) + "        ? ", end="")
        usrNavy = int(input())
        print("A. F.          " + str(cpuAir) + "        ? ", end="")
        usrAir = int(input())
        if not ((usrArmy + usrNavy + usrAir) > MAX_UNITS):
            break


def attackFirst():
    global usrArmy, usrNavy, usrAir
    global cpuArmy, cpuNavy, cpuAir

    numUnits = 0
    unitType = 0

    while True:
        print("YOU ATTACK FIRST. TYPE (1) FOR ARMY; (2) FOR NAVY;")
        print("AND (3) FOR AIR FORCE.")
        print("?", end=' ')
        unitType = int(input())
        if not (unitType < 1 or unitType > 3):
            break

    while True:
        print("HOW MANY MEN")
        print("?", end=' ')
        numUnits = int(input())
        if not ((numUnits < 0) or ((unitType == 1) and (numUnits > usrArmy)) or (
                (unitType == 2) and (numUnits > usrNavy)) or ((unitType == 3) and (numUnits > usrAir))):
            break

    if unitType == 1:
        if numUnits < (usrArmy / 3):
            print("YOU LOST " + str(numUnits) + " MEN FROM YOUR ARMY.")
            usrArmy = usrArmy - numUnits
        elif numUnits < (2 * usrArmy / 3):
            print("YOU LOST " + str(int(numUnits / 3)) + " MEN, BUT I LOST " + str(int(2 * cpuArmy / 3)))
            usrArmy = int(usrArmy - (numUnits / 3))
            cpuArmy = 0
        else:
            print("YOU SUNK ONE OF MY PATROL BOATS, BUT I WIPED OUT TWO")
            print("OF YOUR AIR FORCE BASES AND 3 ARMY BASES.")
            usrArmy = int(usrArmy / 3)
            usrAir = int(usrAir / 3)
            cpuNavy = int(2 * cpuNavy / 3)
    elif unitType == 2:
        if numUnits < cpuNavy / 3:
            print("YOUR ATTACK WAS STOPPED!")
            usrNavy = usrNavy - numUnits
        elif numUnits < 2 * cpuNavy / 3:
            print("YOU DESTROYED " + str(int(2 * cpuNavy / 3)) + " OF MY ARMY.")
            cpuNavy = int(cpuNavy / 3)
        else:
            print("YOU SUNK ONE OF MY PATROL BOATS, BUT I WIPED OUT TWO")
            print("OF YOUR AIR FORCE BASES AND 3 ARMY BASES.")
            usrArmy = int(usrArmy / 3)
            usrAir = int(usrAir / 3)
            cpuNavy = int(2 * cpuNavy / 3)
    elif unitType == 3:
        if numUnits < usrAir / 3:
            print("YOUR ATTACK WAS WIPED OUT.")
            usrAir = usrAir - numUnits
        elif numUnits < 2 * usrAir / 3:
            print("WE HAD A DOGFIGHT. YOU WON - AND FINISHED YOUR MISSION.")
            cpuArmy = int(2 * cpuArmy / 3)
            cpuNavy = int(cpuNavy / 3)
            cpuAir = int(cpuAir / 3)
        else:
            print("YOU WIPED OUT ONE OF MY ARMY PATROLS, BUT I DESTROYED")
            print("TWO NAVY BASES AND BOMBED THREE ARMY BASES.")
            usrArmy = int(usrArmy / 4)
            usrNavy = int(usrNavy / 3)
            cpuArmy = int(2 * cpuArmy / 3)


def attackSecond():
    global usrArmy, usrNavy, usrAir, cpuArmy, cpuNavy, cpuAir
    global planeCrashWin
    numUnits = 0
    unitType = 0

    print("")
    print("              YOU           ME")
    print("ARMY           ", end="")
    print("%-14s%s\n" % (usrArmy, cpuArmy), end="")
    print("NAVY           ", end="")
    print("%-14s%s\n" % (usrNavy, cpuNavy), end="")
    print("A. F.          ", end="")
    print("%-14s%s\n" % (usrAir, cpuAir), end="")

    while True:
        print("WHAT IS YOUR NEXT MOVE?")
        print("ARMY=1  NAVY=2  AIR FORCE=3")
        print("? ", end="")
        unitType = int(input())
        if not ((unitType < 1) or (unitType > 3)):
            break
        
    while True:
        print("HOW MANY MEN")
        print("? ", end="")
        numUnits = int(input())
        if not((numUnits < 0) or ((unitType == 1) and (numUnits > usrArmy)) or ((unitType == 2) and (numUnits > usrNavy)) or ((unitType == 3) and (numUnits > usrAir))):
            break

    if unitType == 1:
        if numUnits < (cpuArmy/2):
            print("I WIPED OUT YOUR ATTACK!")
            usrArmy = usrArmy - numUnits
        else:
            print("YOU DESTROYED MY ARMY!")
            cpuArmy = 0
    elif unitType == 2:
        if numUnits < (cpuNavy/2):
            print("I SUNK TWO OF YOUR BATTLESHIPS, AND MY AIR FORCE")
            print("WIPED OUT YOUR UNGUARDED CAPITOL.")
            usrArmy = int(usrArmy/4)
            usrNavy = int(usrNavy/2)
        else:
            print("YOUR NAVY SHOT DOWN THREE OF MY XIII PLANES,")
            print("AND SUNK THREE BATTLESHIPS.")
            cpuAir = int(2*cpuAir/3)
            cpuNavy = int(cpuNavy/2)
    elif unitType == 3:
        if numUnits > (cpuAir/2):
            print("MY NAVY AND AIR FORCE IN A COMBINED ATTACK LEFT")
            print("YOUR COUNTRY IN SHAMBLES.")
            usrArmy = int(usrArmy/3)
            usrNavy = int(usrNavy/3)
            usrAir = int(usrAir/3)
        else:
            print("ONE OF YOUR PLANES CRASHED INTO MY HOUSE. I AM DEAD.")
            print("MY COUNTRY FELL APART.")
            planeCrashWin = True

    if planeCrashWin == False:
        print("")
        print("FROM THE RESULTS OF BOTH OF YOUR ATTACKS,")

    if (planeCrashWin == True) or ((usrArmy + usrNavy + usrAir) > (int(3/2*(cpuArmy + cpuNavy + cpuAir)))):
        print("YOU WON, OH! SHUCKS!!!!")
    elif (usrArmy + usrNavy + usrAir) < int(2/3*(cpuArmy + cpuNavy + cpuAir)):
        print("YOU LOST-I CONQUERED YOUR COUNTRY.  IT SERVES YOU")
        print("RIGHT FOR PLAYING THIS STUPID GAME!!!")
    else:
        print("THE TREATY OF PARIS CONCLUDED THAT WE TAKE OUR")
        print("RESPECTIVE COUNTRIES AND LIVE IN PEACE.")


def main():
    showIntro()
    getForces()
    attackFirst()
    attackSecond()
    
if __name__ == '__main__':
    main()