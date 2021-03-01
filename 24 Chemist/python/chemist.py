#!/usr/bin/env python3
# CHEMIST
#
# Converted from BASIC to Python by Jeff R. Allen

import random

def main():
    print(" " * 33 + "CHEMIST")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")
    
    print()
    print()
    print()
    
    print("The fictitious checmical kryptocyanic acid can only be")
    print("diluted by the ratio of 7 parts water to 3 parts acid.")
    print("If any other ratio is attempted, the acid becomes unstable")
    print("and soon explodes.  Given the amount of acid, you must")
    # The typo in the following line (who/how) was present in the oringal!
    # https://www.atariarchives.org/basicgames/showpage.php?page=42
    print("decide who much water to add for dilution.  If you miss")
    print("you face the consequences.")

    lives = 9
    while lives > 0:
        a = random.randint(0,50)
        w = 7 * a / 3

        r = input("\n%d litres of kryptocyanic acid. How much water? " % a)
        r = int(r)
        
        d = abs(w-r)
        if d <= w/20:
            print("Good job! You may breathe now, but don't inhale the fumes!")
        else:
            print("Sizzle! You have just been desalinated into a blob.")
            print("of quivering protoplasm!")
            lives -= 1
            if lives > 0:
                print("However, you may try again with another life.")
                
    print("Your 9 lives are used, but you will be long remembered for")
    print("your contributions to the field of comic book chemistry.")

if __name__ == "__main__":
    main()
