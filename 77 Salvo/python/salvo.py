import re

# declare static variables
BOARD_WIDTH = 10
BOARD_HEIGHT = 10

SHIPS = [("BATTLESHIP", 5),
         ("CRUISER", 3),
         ("DESTROYER<A>", 2),
         ("DESTROYER<B>", 2)]

VALID_MOVES = [[-1, 0],
               [-1, 1],
               [0, 1],
               [1, 1],
               [1, 0],
               [1, -1],
               [0, -1],
               [1, -1]]

COORD_REGEX = '[ \t]{0,}(-?[0-9]{1,3})[ \t]{0,},[ \t]{0,}(-?[0-9]{1,2})'


# input_coord
#
# ask user for single (x,y) coordinate
# validate the coordinates are within the bounds
# of the board width and height. mimic the behavior
# of the original program which exited with error
# messages if coordinates where outside of array bounds.
# if input is not numeric, print error out to user and
# let them try again.
def input_coord():
    match = None
    while not match:
        coords = input("? ")
        match = re.match(COORD_REGEX, coords)
        if not match:
            print("!NUMBER EXPECTED - RETRY INPUT LINE")
    x = int(match.group(1))
    y = int(match.group(2))

    if x > BOARD_HEIGHT or y > BOARD_WIDTH:
        print("!OUT OF ARRAY BOUNDS IN LINE 1540")
        exit()

    if x <= 0 or y <= 0:
        print("!NEGATIVE ARRAY DIM IN LINE 1540")
        exit()

    return x, y


# input_ship_coords
#
# ask the user for coordinates for each
# ship on their board. uses input_coord()
# to read each coord.
# returns an array of arrays, one array for
# each ship's coordinates, which is an array 
# of (x,y) sets.
def input_ship_coords():
    print("ENTER COORDINATES FOR...")

    coords = []
    for ship in SHIPS:
        print(ship[0])
        list = []
        for i in range(ship[1]):
            x, y = input_coord()
            list.append((x, y))
        coords.append(list)
    return coords


# print out the title 'screen'
print('{0:>38}'.format("SALVO"))
print('{0:>57s}'.format("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"))
print('\n\n')

# ask the user for ship coordinates
coords = input_ship_coords()
