"""
SUPER STARTREK INSTRUCTIONS
MAR 5, 1978

Just the instructions for SUPERSTARTREK

Ported by Dave LeCompte
"""


def get_yes_no(prompt: str) -> bool:
    response = input(prompt).upper()
    return response[0] != "N"


def print_header() -> None:
    for _ in range(12):
        print()
    t10 = " " * 10
    print(t10 + "*************************************")
    print(t10 + "*                                   *")
    print(t10 + "*                                   *")
    print(t10 + "*      * * SUPER STAR TREK * *      *")
    print(t10 + "*                                   *")
    print(t10 + "*                                   *")
    print(t10 + "*************************************")
    for _ in range(8):
        print()


def print_instructions() -> None:
    # Back in the 70s, at this point, the user would be prompted to
    # turn on their (printing) TTY to capture the output to hard copy.

    print("      INSTRUCTIONS FOR 'SUPER STAR TREK'")
    print()
    print("1. WHEN YOU SEE \\COMMAND ?\\ PRINTED, ENTER ONE OF THE LEGAL")
    print("     COMMANDS (NAV,SRS,LRS,PHA,TOR,SHE,DAM,COM, OR XXX).")
    print("2. IF YOU SHOULD TYPE IN AN ILLEGAL COMMAND, YOU'LL GET A SHORT")
    print("     LIST OF THE LEGAL COMMANDS PRINTED OUT.")
    print("3. SOME COMMANDS REQUIRE YOU TO ENTER DATA (FOR EXAMPLE, THE")
    print("     'NAV' COMMAND COMES BACK WITH 'COURSE (1-9) ?'.)  IF YOU")
    print("     TYPE IN ILLEGAL DATA (LIKE NEGATIVE NUMBERS), THAN COMMAND")
    print("     WILL BE ABORTED")
    print()
    print("     THE GALAXY IS DIVIDED INTO AN 8 X 8 QUADRANT GRID,")
    print("AND EACH QUADRANT IS FURTHER DIVIDED INTO AN 8 X 8 SECTOR GRID.")
    print()
    print("     YOU WILL BE ASSIGNED A STARTING POINT SOMEWHERE IN THE")
    print("GALAXY TO BEGIN A TOUR OF DUTY AS COMANDER OF THE STARSHIP")
    print("\\ENTERPRISE\\; YOUR MISSION: TO SEEK AND DESTROY THE FLEET OF")
    print("KLINGON WARWHIPS WHICH ARE MENACING THE UNITED FEDERATION OF")
    print("PLANETS.")
    print()
    print("     YOU HAVE THE FOLLOWING COMMANDS AVAILABLE TO YOU AS CAPTAIN")
    print("OF THE STARSHIP ENTERPRISE:")
    print()
    print("\\NAV\\ COMMAND = WARP ENGINE CONTROL --")
    print("     COURSE IS IN A CIRCULAR NUMERICAL      4  3  2")
    print("     VECTOR ARRANGEMENT AS SHOWN             . . .")
    print("     INTEGER AND REAL VALUES MAY BE           ...")
    print("     USED.  (THUS COURSE 1.5 IS HALF-     5 ---*--- 1")
    print("     WAY BETWEEN 1 AND 2                      ...")
    print("                                             . . .")
    print("     VALUES MAY APPROACH 9.0, WHICH         6  7  8")
    print("     ITSELF IS EQUIVALENT TO 1.0")
    print("                                            COURSE")
    print("     ONE WARP FACTOR IS THE SIZE OF ")
    print("     ONE QUADTANT.  THEREFORE, TO GET")
    print("     FROM QUADRANT 6,5 TO 5,5, YOU WOULD")
    print("     USE COURSE 3, WARP FACTOR 1.")
    print()
    print("\\SRS\\ COMMAND = SHORT RANGE SENSOR SCAN")
    print("     SHOWS YOU A SCAN OF YOUR PRESENT QUADRANT.")
    print()
    print("     SYMBOLOGY ON YOUR SENSOR SCREEN IS AS FOLLOWS:")
    print("        <*> = YOUR STARSHIP'S POSITION")
    print("        +K+ = KLINGON BATTLE CRUISER")
    print("        >!< = FEDERATION STARBASE (REFUEL/REPAIR/RE-ARM HERE!)")
    print("         *  = STAR")
    print()
    print("     A CONDENSED 'STATUS REPORT' WILL ALSO BE PRESENTED.")
    print()
    print("\\LRS\\ COMMAND = LONG RANGE SENSOR SCAN")
    print("     SHOWS CONDITIONS IN SPACE FOR ONE QUADRANT ON EACH SIDE")
    print("     OF THE ENTERPRISE (WHICH IS IN THE MIDDLE OF THE SCAN)")
    print("     THE SCAN IS CODED IN THE FORM \\###\\, WHERE TH UNITS DIGIT")
    print("     IS THE NUMBER OF STARS, THE TENS DIGIT IS THE NUMBER OF")
    print("     STARBASES, AND THE HUNDRESDS DIGIT IS THE NUMBER OF")
    print("     KLINGONS.")
    print()
    print("     EXAMPLE - 207 = 2 KLINGONS, NO STARBASES, & 7 STARS.")
    print()
    print("\\PHA\\ COMMAND = PHASER CONTROL.")
    print("     ALLOWS YOU TO DESTROY THE KLINGON BATTLE CRUISERS BY ")
    print("     ZAPPING THEM WITH SUITABLY LARGE UNITS OF ENERGY TO")
    print("     DEPLETE THEIR SHIELD POWER.  (REMEMBER, KLINGONS HAVE")
    print("     PHASERS TOO!)")
    print()
    print("\\TOR\\ COMMAND = PHOTON TORPEDO CONTROL")
    print("     TORPEDO COURSE IS THE SAME AS USED IN WARP ENGINE CONTROL")
    print("     IF YOU HIT THE KLINGON VESSEL, HE IS DESTROYED AND")
    print("     CANNOT FIRE BACK AT YOU.  IF YOU MISS, YOU ARE SUBJECT TO")
    print("     HIS PHASER FIRE.  IN EITHER CASE, YOU ARE ALSO SUBJECT TO ")
    print("     THE PHASER FIRE OF ALL OTHER KLINGONS IN THE QUADRANT.")
    print()
    print("     THE LIBRARY-COMPUTER (\\COM\\ COMMAND) HAS AN OPTION TO ")
    print("     COMPUTE TORPEDO TRAJECTORY FOR YOU (OPTION 2)")
    print()
    print("\\SHE\\ COMMAND = SHIELD CONTROL")
    print("     DEFINES THE NUMBER OF ENERGY UNITS TO BE ASSIGNED TO THE")
    print("     SHIELDS.  ENERGY IS TAKEN FROM TOTAL SHIP'S ENERGY.  NOTE")
    print("     THAN THE STATUS DISPLAY TOTAL ENERGY INCLUDES SHIELD ENERGY")
    print()
    print("\\DAM\\ COMMAND = DAMMAGE CONTROL REPORT")
    print("     GIVES THE STATE OF REPAIR OF ALL DEVICES.  WHERE A NEGATIVE")
    print("     'STATE OF REPAIR' SHOWS THAT THE DEVICE IS TEMPORARILY")
    print("     DAMAGED.")
    print()
    print("\\COM\\ COMMAND = LIBRARY-COMPUTER")
    print("     THE LIBRARY-COMPUTER CONTAINS SIX OPTIONS:")
    print("     OPTION 0 = CUMULATIVE GALACTIC RECORD")
    print("        THIS OPTION SHOWES COMPUTER MEMORY OF THE RESULTS OF ALL")
    print("        PREVIOUS SHORT AND LONG RANGE SENSOR SCANS")
    print("     OPTION 1 = STATUS REPORT")
    print("        THIS OPTION SHOWS THE NUMBER OF KLINGONS, STARDATES,")
    print("        AND STARBASES REMAINING IN THE GAME.")
    print("     OPTION 2 = PHOTON TORPEDO DATA")
    print("        WHICH GIVES DIRECTIONS AND DISTANCE FROM THE ENTERPRISE")
    print("        TO ALL KLINGONS IN YOUR QUADRANT")
    print("     OPTION 3 = STARBASE NAV DATA")
    print("        THIS OPTION GIVES DIRECTION AND DISTANCE TO ANY ")
    print("        STARBASE WITHIN YOUR QUADRANT")
    print("     OPTION 4 = DIRECTION/DISTANCE CALCULATOR")
    print("        THIS OPTION ALLOWS YOU TO ENTER COORDINATES FOR")
    print("        DIRECTION/DISTANCE CALCULATIONS")
    print("     OPTION 5 = GALACTIC /REGION NAME/ MAP")
    print("        THIS OPTION PRINTS THE NAMES OF THE SIXTEEN MAJOR ")
    print("        GALACTIC REGIONS REFERRED TO IN THE GAME.")


def main() -> None:
    print_header()
    if not get_yes_no("DO YOU NEED INSTRUCTIONS (Y/N)? "):
        return
    print_instructions()


if __name__ == "__main__":
    main()
