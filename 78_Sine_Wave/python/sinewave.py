########################################################
#
# Sine Wave
#
# From: BASIC Computer Games (1978)
#       Edited by David H. Ahl
#
# "Did you ever go to a computer show and see a bunch of
#  CRT terminals just sitting there waiting forlornly
#  for someone to give a demo on them.  It was one of
#  those moments when I was at DEC that I decided there
#  should be a little bit of background activity.  And
#  why not plot with words instead of the usual X's?
#  Thus SINE WAVE was born and lives on in dozens of
#  different versions.  At least those CRTs don't look
#  so lifeless anymore."
#
# Original BASIC version by David Ahl
#
# Python port by Jeff Jetton, 2019
#
########################################################

import math
import time

# Constants
STRINGS = ("Creative", "Computing")  # Text to display
MAX_LINES = 160
STEP_SIZE = 0.25  # Number of radians to increase at each
# line. Controls speed of horizontal
# printing movement.
CENTER = 26  # Controls left edge of "middle" string
DELAY = 0.05  # Amount of seconds to wait between lines


# Display "intro" text
print("\n                        Sine Wave")
print("         Creative Computing  Morristown, New Jersey")
print("\n\n\n\n")
# "REMarkable program by David Ahl"


string_index = 0
radians = 0
width = CENTER - 1

# "Start long loop"
for _line_num in range(MAX_LINES):

    # Get string to display on this line
    curr_string = STRINGS[string_index]

    # Calculate how far over to print the text
    sine = math.sin(radians)
    padding = int(CENTER + width * sine)
    print(curr_string.rjust(padding + len(curr_string)))

    # Increase radians and increment our tuple index
    radians += STEP_SIZE
    string_index += 1
    if string_index >= len(STRINGS):
        string_index = 0

    # Make sure the text doesn't fly by too fast...
    time.sleep(DELAY)


########################################################
#
# Porting Notes
#
#   The original BASIC version hardcoded two words in
#   the body of the code and then used a sentinel flag
#   (flipping between 0 and 1) with IF statements to
#   determine the word to display next.
#
#   Here, the words have been moved to a Python tuple,
#   which is iterated over without any assumptions about
#   how long it is.  The STRINGS tuple can therefore be
#   modified to have to program print out any sequence
#   of any number of lines of text.
#
#   Since a modern computer running Python will print
#   to the screen much more quickly than a '70s-era
#   computer running BASIC would, a delay component
#   has been introduced in this version to make the
#   output more historically accurate.
#
#
# Ideas for Modifications
#
#   Ask the user for desired number of lines (perhaps
#   with an "infinite" option) and/or step size.
#
#   Let the user input the text strings to display,
#   rather than having it pre-defined in a constant.
#   Calculate an appropriate CENTER based on length of
#   longest string.
#
#   Try changing STINGS so that it only includes a
#   single string, just like this:
#
#       STRINGS = ('Howdy!')
#
#   What happens? Why? How would you fix it?
#
########################################################
