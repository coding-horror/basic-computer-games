import re
import random

###################
#
# static variables
#
###################

BOARD_WIDTH = 10
BOARD_HEIGHT = 10

SHIPS = [("BATTLESHIP", 5),
         ("CRUISER", 3),
         ("DESTROYER<A>", 2),
         ("DESTROYER<B>", 2)]

VALID_MOVES = [[-1, 0],   # North
               [-1, 1],   # North East
               [0, 1],    # East
               [1, 1],    # South East
               [1, 0],    # South
               [1, -1],   # South West
               [0, -1],   # West
               [-1, -1]]  # North West

COORD_REGEX = '[ \t]{0,}(-?[0-9]{1,3})[ \t]{0,},[ \t]{0,}(-?[0-9]{1,2})'

####################
#
# global variables
#
####################

# array of BOARD_HEIGHT arrays, BOARD_WIDTH in length,
# representing the human player and computer
player_board = []
computer_board = []

# array representing the coordinates
# for each ship for player and computer
# array is in the same order as SHIPS
player_ship_coords = []
computer_ship_coords = []

# keep track of the turn
current_turn = 0

# flag indicating if computer's shots are
# printed out during computer's turn
print_computer_shots = False

####################
#
# game functions
#
####################

# random number functions
#
# seed the random number generator
random.seed()


# random_x_y
#
# generate a valid x,y coordinate on the board
# returns: x,y
#   x: integer between 1 and BOARD_HEIGHT
#   y: integer between 1 and BOARD WIDTH
def random_x_y():
    x = random.randrange(1, BOARD_WIDTH+1)
    y = random.randrange(1, BOARD_HEIGHT+1)
    return (x, y)


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


# TODO: add an optional starting coordinate for testing
#       purposes
def generate_ship_coordinates(ship):
    # randomly generate starting x,y coordinates
    start_x, start_y = random_x_y()

    # using starting coordinates and the ship type,
    # generate a vector of possible directions the ship
    # could be placed. directions are numbered 0-7 along
    # points of the compass (N, NE, E, SE, S, SW, W, NW)
    # clockwise. a vector of valid directions where the
    # ship does not go off the board is determined
    ship_len = SHIPS[ship][1] - 1
    dirs = [False for x in range(8)]
    dirs[0] = (start_x - ship_len) >= 1
    dirs[2] = (start_y + ship_len) <= BOARD_WIDTH
    dirs[1] = dirs[0] and dirs[2]
    dirs[4] = (start_x + ship_len) <= BOARD_HEIGHT
    dirs[3] = dirs[2] and dirs[4]
    dirs[6] = (start_y - ship_len) >= 1
    dirs[5] = dirs[4] and dirs[6]
    dirs[7] = dirs[6] and dirs[0]
    directions = [p for p in range(len(dirs)) if dirs[p]]

    # using the vector of valid directions, pick a
    # random direction to place the ship
    dir_idx = random.randrange(len(directions))
    direction = directions[dir_idx]

    # using the starting x,y, direction and ship
    # type, return the coordinates of each point
    # of the ship. VALID_MOVES is a staic array
    # of coordinate offsets to walk from starting
    # coordinate to the end coordinate in the
    # chosen direction
    ship_len = SHIPS[ship][1] - 1
    d_x = VALID_MOVES[direction][0]
    d_y = VALID_MOVES[direction][1]

    coords = [(start_x, start_y)]
    x_coord = start_x
    y_coord = start_y
    for i in range(ship_len):
        x_coord = x_coord + d_x
        y_coord = y_coord + d_y
        coords.append((x_coord, y_coord))
    return coords


# create_blank_board
#
# helper function to create a game board
# that is blank
def create_blank_board():
    return [[None for y in range(BOARD_WIDTH)]
            for x in range(BOARD_HEIGHT)]


def print_board(board):

    # print board header (column numbers)
    print('  ', end='')
    for z in range(BOARD_WIDTH):
        print(f'{z+1:3}', end='')
    print('')

    for x in range(len(board)):
        print(f'{x+1:2}', end='')
        for y in range(len(board[x])):
            if(board[x][y] is None):
                print(f"{' ':3}", end='')
            else:
                print(f"{board[x][y]:3}", end='')
        print('')


# place_ship
#
# place a ship on a given board. updates
# the board's row,column value at the given
# coordinates to indicate where a ship is
# on the board.
#
# inputs: board - array of BOARD_HEIGHT by BOARD_WIDTH
#         coords - array of sets of (x,y) coordinates of each
#                  part of the given ship
#         ship - integer repreesnting the type of ship (given in SHIPS)
def place_ship(board, coords, ship):
    for coord in coords:
        board[coord[0]-1][coord[1]-1] = ship


# NOTE: A little quirk that exists here and in the orginal
#       game: Ships are allowed to cross each other!
#       For example: 2 destroyers, length 2, one at
#       [(1,1),(2,2)] and other at [(2,1),(1,2)]
def generate_board():
    board = create_blank_board()

    ship_coords = []
    for ship in range(len(SHIPS)):
        placed = False
        coords = []
        while not placed:
            coords = generate_ship_coordinates(ship)
            clear = True
            for coord in coords:
                if board[coord[0]-1][coord[1]-1] is not None:
                    clear = False
                    break
            if clear:
                placed = True
        place_ship(board, coords, ship)
        ship_coords.append(coords)
    return board, ship_coords


# initialize
#
# function to initialize global variables used
# during game play.
def initialize_game():

    # initialize the global player and computer
    # boards
    global player_board
    player_board = create_blank_board()

    # generate the ships for the computer's
    # board
    global computer_board
    global computer_ship_coords
    computer_board, computer_ship_coords = generate_board()

    # print out the title 'screen'
    print('{0:>38}'.format("SALVO"))
    print('{0:>57s}'.format("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"))
    print('\n\n')


######################
#
# main game flow
#
######################

# initialize the player and computer
# boards
initialize_game()

# ask the player for ship coordinates
print("ENTER COORDINATES FOR...")
ship_coords = []
for ship in SHIPS:
    print(ship[0])
    list = []
    for i in range(ship[1]):
        x, y = input_coord()
        list.append((x, y))
    ship_coords.append(list)

# add ships to the user's board
for ship in range(len(SHIPS)):
    place_ship(player_board, ship_coords[ship], ship)

# see if the player wants the computer's ship
# locations printed out and if the player wants to
# start
input_loop = True
player_start = "YES"
while input_loop:
    player_start = input("DO YOU WANT TO START? ")
    if player_start == "WHERE ARE YOUR SHIPS?":
        for ship in range(len(SHIPS)):
            print(SHIPS[ship][0])
            coords = computer_ship_coords[ship]
            for coord in coords:
                x = coord[0]
                y = coord[1]
                print('{0:2}'.format(x), '{0:2}'.format(y))
    else:
        input_loop = False

# ask the player if they want the computer's shots
# printed out each turn
see_computer_shots = input("DO YOU WANT TO SEE MY SHOTS? ")
if see_computer_shots.lower() == "yes":
    print_computer_shots = True

