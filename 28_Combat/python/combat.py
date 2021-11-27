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