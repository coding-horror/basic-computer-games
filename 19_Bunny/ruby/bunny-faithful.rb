#!/bin/env ruby

# Bunny - Print a large ASCII-pixel bunny icon made up of the letters
# of the word BUNNY.

# This is a recreation of bunny.bas in Ruby that attempts to remain
# relatively faithful to the design of the original program.
#
# The BASIC version works by storing the image as a series of pairs of
# numbers containing ranges of text columns that will need to be
# filled in with non-blank characters.
#
# For example, the first few entries in the block of DATA statements are:
#
#       1,2,-1,0,2,45,50,-1 ...
#
#           * 1,2 means "write letters from columns 1 to 2
#           * -1 starts a new line
#           * 0,2 draws letters between columns 0 and 2
#           * 45,50 draws letters between columns 45 and 50
#           * -1 starts a new line
# 
# ...and so on.
#
# We keep the data statements as they are and redraw the image using
# them.  (Well, we drop the last one because it's the end-of-data flag
# and Ruby is perfectly effective at finding the end of the list.)
#
# One tricky bit: BASIC has a function called 'tab()' which sets the
# output column to the given position and which the BASIC version uses
# to pick the columns to write to.  Ruby doesn't have an equivalent
# feature (well, not without a *lot* more complexity).  Fortunately,
# the data always draws from left to right so we just keep track of
# the last column written to and then add some spaces to advance to
# where we need to be.
# 


# Do the thing.  (We put it in a function to keep from spewing global
# variables all over the place.  It's not really necessary here but
# it's good practice.)
def main

  # Print the heading.  Note the highly advanced lower-case letters.
  puts ' '*33 + "Bunny"
  puts ' '*15 + "Creative Computing  Morristown, New Jersey"

  # Print blank lines.
  print "\n\n\n"

  # The positions to write; this is ripped from the BASIC program's
  # DATA statements.
  positions = [
    1,2,-1,0,2,45,50,-1,0,5,43,52,-1,0,7,41,52,-1,    
    1,9,37,50,-1,2,11,36,50,-1,3,13,34,49,-1,4,14,32,48,-1,
    5,15,31,47,-1,6,16,30,45,-1,7,17,29,44,-1,8,19,28,43,-1,
    9,20,27,41,-1,10,21,26,40,-1,11,22,25,38,-1,12,22,24,36,-1,
    13,34,-1,14,33,-1,15,31,-1,17,29,-1,18,27,-1,
    19,26,-1,16,28,-1,13,30,-1,11,31,-1,10,32,-1,
    8,33,-1,7,34,-1,6,13,16,34,-1,5,12,16,35,-1,
    4,12,16,35,-1,3,12,15,35,-1,2,35,-1,1,35,-1,
    2,34,-1,3,34,-1,4,33,-1,6,33,-1,10,32,34,34,-1,
    14,17,19,25,28,31,35,35,-1,15,19,23,30,36,36,-1,
    14,18,21,21,24,30,37,37,-1,13,18,23,29,33,38,-1,
    12,29,31,33,-1,11,13,17,17,19,19,22,22,24,31,-1,
    10,11,17,18,22,22,24,24,29,29,-1,
    22,23,26,29,-1,27,29,-1,28,29,-1,
  ]

  # The text we're writing.
  text = "BUNNY"

  # Draw the bunny.
  last_pos = 0
  while positions.size > 0
    first = positions.shift

    # If we've found -1, start a new line
    if first == -1
      puts
      last_pos = 0
      next
    end      

    # Advance to start of the range
    print ' '*(first - last_pos)
    last_pos = first

    # Now, draw pixels:
    second = positions.shift
    for i in first .. second
      print text[i % text.size] # choose the letter according to the column
      last_pos += 1
    end
  end

  # Print the final blank line
  puts
end

main
