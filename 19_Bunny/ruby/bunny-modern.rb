#!/bin/env ruby

# Bunny - Print a large ASCII-pixel bunny icon made up of the letters
# of the word BUNNY.

# This is a recreation of bunny.bas in Ruby that takes advantage of
# the language's features to make the code easier to understand and
# modify.
#
# Instead of storing the image as a set of ranges, we just store it as
# ASCII art, then replace the pixes with the appropriate letters when
# printing.  In addition to being simpler, this also lets you modify
# the image much more easily.


# We assume a screen width of 80.  (With modern consoles, doing this
# accurately gets complex and it's really not worth the bother for
# this little program so 80 columns it is.)
ScreenWidth = 80


# The bunny image.  Totally not the logo of a magazine that was edgy
# in the 1970s.
Bunny = <<EOF
 **
***                                          ******
******                                     **********
********                                 ************
 *********                           **************
  **********                        ***************
   ***********                    ****************
    ***********                 *****************
     ***********               *****************
      ***********             ****************
       ***********           ****************
        ************        ****************
         ************      ***************
          ************    ***************
           ************  **************
            *********** *************
             **********************
              ********************
               *****************
                 *************
                  **********
                   ********
                *************
             ******************
           *********************
          ***********************
        **************************
       ****************************
      ********  *******************
     ********   ********************
    *********   ********************
   **********  *********************
  **********************************
 ***********************************
  *********************************
   ********************************
    ******************************
      ****************************
          *********************** *
              **** *******  ****   *
               *****   ********     *
              *****  *  *******      *
             ******    *******   ******
            ****************** ***
           ***   * *  * ********
          **     **   * *    *
                      **  ****
                           ***
                            **
EOF


# Do the thing.  (We put it in a function to keep from spewing global
# variables all over the place.  It's not really necessary here but
# it's good practice.)
def main

  puts_centered "Bunny"
  puts_centered "Creative Computing  Morristown, New Jersey"

  # Print some blank lines
  print "\n\n\n"

  print_bunny_text(Bunny)

  puts
end

def puts_centered(str)
  print ' ' * ((ScreenWidth - str.size)/2)
  puts str
end

# Print an ASCII-pixel image such that each pixel is a letter of the
# word BUNNY chosen by the column in which it appears.
def print_bunny_text(text)
  bunny = "BUNNY"

  # Take the initial string and split it on the newlines (i.e. turn it
  # into a list of strings, each string making up one line), then loop
  # over the lines one at a time, converting and then printing each of
  # them.
  for line in text.split(/\n/)

    # Replace the '*' with the correct letter.
    for n in (0..line.size - 1)
      line[n] = bunny[n % 5]  if line[n] != ' '
    end

    # Bonus hackery: we could replace the above with this one-liner,
    # but it's kind of hard to understand so it may or may not be wise
    # to use it.
    # 
    #   line.gsub!(/(\S)/) {|m| bunny[$~.begin(0) % bunny.size] }

    puts line
  end
end

main()
