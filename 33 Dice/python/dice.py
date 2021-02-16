########################################################
#
# Dice
#
# From: BASIC Computer Games (1978)
#       Edited by David H. Ahl
#
# "Not exactly a game, this program simulates rolling
#  a pair of dice a large number of times and prints out
#  the frequency distribution.  You simply input the
#  number of rolls.  It is interesting to see how many
#  rolls are necessary to approach the theoretical
#  distribution:
#
#  2  1/36  2.7777...%
#  3  2/36  5.5555...%
#  4  3/36  8.3333...%
#    etc.
#
# "Daniel Freidus wrote this program while in the
#  seventh grade at Harrison Jr-Sr High School,
#  Harrison, New York."
#
# Python port by Jeff Jetton, 2019
#
########################################################


import random


# We'll track counts of roll outcomes in a 13-element list.
# The first two indices (0 & 1) are ignored, leaving just
# the indices that match the roll values (2 through 12).
freq = [0] * 13


# Display intro text
print("\n                   Dice")
print("Creative Computing  Morristown, New Jersey")
print("\n\n")
# "Danny Freidus"
print("This program simulates the rolling of a")
print("pair of dice.")
print("You enter the number of times you want the computer to")
print("'roll' the dice.   Watch out, very large numbers take")
print("a long time.  In particular, numbers over 5000.")

still_playing = True
while still_playing:
    print("")
    n = int(input("How many rolls? "))

    # Roll the dice n times
    for i in range(n):
        die1 = random.randint(1, 6)
        die2 = random.randint(1, 6)
        roll_total = die1 + die2
        freq[roll_total] += 1

    # Display final results
    print("\nTotal Spots   Number of Times")
    for i in range(2, 13):
        print(" %-14d%d" % (i, freq[i]))

    # Keep playing?
    print("")
    response = input("Try again? ")
    if len(response) > 0 and response.upper()[0] == 'Y':
        # Clear out the frequency list
        freq = [0]*13
    else:
        # Exit the game loop
        still_playing = False
    



########################################################
#
# Porting Notes
#
#   A fairly straightforward port.  The only change is
#   in the handling of the user's "try again" response.
#   The original program only continued if the user
#   entered "YES", whereas this version will continue
#   if any word starting with "Y" or "y" is given.
#
#   The instruction text--which, like all these ports,
#   was taken verbatim from the original listing--is
#   charmingly quaint in its dire warning against
#   setting the number of rolls too high.  At the time
#   of this writing, on a fairly slow computer, a
#   5000-roll run typically clocks in at well under
#   1/10 of a second!
#
#
# Ideas for Modifications
#
#   Have the results include a third column showing
#   the percent of rolls each count represents.  Or
#   (better yet) print a low-fi bar graph using
#   rows of asterisks to represent relative values,
#   with each asterisk representing one percent,
#   for example.
#
#   Add a column showing the theoretically expected
#   percentage, for comparison.
#
#   Keep track of how much time the series of rolls
#   takes and add that info to the final report.
#
#   What if three (or four, or five...) dice were
#   rolled each time?
#
########################################################


            
        
    

    
