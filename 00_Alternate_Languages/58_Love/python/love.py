######################################################################
#
# LOVE
#
# From: BASIC Computer Games (1978)
#       Edited by David H. Ahl
#
# "This program is designed to reproduce Robert Indiana's great art
#  work 'Love' with a message of your choice up to 60 characters long.
#
# "The [DATA variable is] an alternating count of the number
#  of characters and blanks which form the design.  These data give
#  the correct proportions for a standard 10 character per inch
#  Teletype or line printer.
#
# "The LOVE program was created by David Ahl."
#
#
# Python port by Jeff Jetton, 2019
#
######################################################################


# Image data. Each top-level element is a row. Each row element
# contains alternating character and blank run lengths.
DATA = [
    [
        60,
    ],
    [1, 12, 26, 9, 12],
    [3, 8, 24, 17, 8],
    [4, 6, 23, 21, 6],
    [4, 6, 22, 12, 5, 6, 5],
    [4, 6, 21, 11, 8, 6, 4],
    [4, 6, 21, 10, 10, 5, 4],
    [4, 6, 21, 9, 11, 5, 4],
    [4, 6, 21, 8, 11, 6, 4],
    [4, 6, 21, 7, 11, 7, 4],
    [4, 6, 21, 6, 11, 8, 4],
    [4, 6, 19, 1, 1, 5, 11, 9, 4],
    [4, 6, 19, 1, 1, 5, 10, 10, 4],
    [4, 6, 18, 2, 1, 6, 8, 11, 4],
    [4, 6, 17, 3, 1, 7, 5, 13, 4],
    [4, 6, 15, 5, 2, 23, 5],
    [1, 29, 5, 17, 8],
    [1, 29, 9, 9, 12],
    [1, 13, 5, 40, 1],
    [1, 13, 5, 40, 1],
    [4, 6, 13, 3, 10, 6, 12, 5, 1],
    [5, 6, 11, 3, 11, 6, 14, 3, 1],
    [5, 6, 11, 3, 11, 6, 15, 2, 1],
    [6, 6, 9, 3, 12, 6, 16, 1, 1],
    [6, 6, 9, 3, 12, 6, 7, 1, 10],
    [7, 6, 7, 3, 13, 6, 6, 2, 10],
    [7, 6, 7, 3, 13, 14, 10],
    [8, 6, 5, 3, 14, 6, 6, 2, 10],
    [8, 6, 5, 3, 14, 6, 7, 1, 10],
    [9, 6, 3, 3, 15, 6, 16, 1, 1],
    [9, 6, 3, 3, 15, 6, 15, 2, 1],
    [10, 6, 1, 3, 16, 6, 14, 3, 1],
    [10, 10, 16, 6, 12, 5, 1],
    [11, 8, 13, 27, 1],
    [11, 8, 13, 27, 1],
    [
        60,
    ],
]


# Assume that the total length of the first element
# is the line length used by every row
ROW_LEN = sum(DATA[0])

# Display intro text
print("\n                  Love")
print("Creative Computing  Morristown, New Jersey")
print("\n\n")
print("A tribute to the great American artist, Robert Indiana.")
print("His great work will be reproduced with a message of")
print("your choice up to 60 characters.  If you can't think of")
print("a message, simple type the word 'love'\n")  # (sic)

# Get message from user
message = input("Your message, please? ")
if message == "":
    message = "LOVE"

# Repeat the message until we get at least one line's worth
while len(message) < ROW_LEN:
    message += message

# Display image
print("\n" * 9)
for row in DATA:
    print_message = True
    position = 0
    line_text = ""
    for length in row:
        if print_message:
            text = message[position : (position + length)]
            print_message = False
        else:
            text = " " * length
            print_message = True
        line_text += text
        position += length
    print(line_text)

print("")


######################################################################
#
# Porting Notes
#
#   Not too different from the original, logic-wise. The image was
#   originally encoded as a series of BASIC "DATA" lines. Here,
#   we've converted it to a more Pythonic nested list structure.
#   Other changes include reducing some of the vertical spacing
#   (since we'll probably be showing this on a screen rather than
#   the sort of tractor-feed printer the program was written for)
#   and having the message default to LOVE when no input is given.
#
#   This program uses a simple version of run-length encoding to
#   compress a 60 x 36 image (2,160 characters) into just 252 DATA
#   values.  That's about an 8.5-to-1 data compression ratio,
#   which is pretty good!
#
#
# Ideas for Modifications
#
#   Process the user's message input to remove spaces and change
#   to uppercase.
#
#   Encode other images in a similar fashion and let the user choose
#   which one they'd like to use to display their message.
#
#   To help with the above step, create a program that reads in a
#   text file of any sort of similar character/space art and produces
#   the Python code to initialize the correct nested list of values.
#
#   For example, if the input file were:
#
#     *****
#     *  **
#     **  *
#
#   Your program would output:
#
#    ((5, ), (1, 1, 2), (2, 1, 1))
#
######################################################################
