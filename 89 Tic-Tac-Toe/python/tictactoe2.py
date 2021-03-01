#!/usr/bin/env python3
from enum import Enum


class OccupiedBy(Enum):
    COMPUTER=-1
    EMPTY=0
    PLAYER=1


class Winner(Enum):
    NONE=0
    COMPUTER=1
    PLAYER=2
    DRAW=3


class Space(Enum):
    TOP_LEFT = 0
    TOP_CENTER = 1
    TOP_RIGHT = 2
    MID_LEFT = 3
    MID_CENTER = 4
    MID_RIGHT = 5
    BOT_LEFT = 6
    BOT_CENTER = 7
    BOT_RIGHT = 8


def line_170(board, g, h, j, k):
    if g == OccupiedBy.Player:
        if board[Space.MID_CENTER] == g:
            if board[Space.TOP_RIGHT] == g and board[Space.BOTTOM_LEFT] is OccupiedBy.EMPTY: # Line 171
                return Space.BOTTOM_LEFT # Line 187
            elif board[Space.BOTTOM_RIGHT] == g and board[Space.TOP_LEFT] is OccupiedBy.EMPTY: # Line 172
                return Space.TOP_LEFT # Line 181
            elif board[Space.BOTTOM_LEFT] == g and board[Space.TOP_RIGHT] is OccupiedBy.EMPTY: # Line 173
                return Space.TOP_RIGHT # Line 183
            elif board[Space.BOTTOM_RIGHT] is OccupiedBy.PLAYER and board[Space.TOP_RIGHT] is OccupiedBy.EMPTY: # Line 174
                return Space.TOP_RIGHT # Line 189
            elif g is OccupiedBy.COMPUTER:
                g = OccupiedBy.PLAYER
                h = OccupiedBy.COMPUTER
                return line_118(board, g, h, j, k)


def line_150(board, g, h, j, k):
    if board[k] != g: # line 150
        if (board[k] == h       # line 160
            or board[k + 6] != g   # line 161
            or board[k + 3] != g): # line 162
            return -1 # Goto 170
        else:
            return k + 3  # Line 163
    elif board[k + 6] != g: # line 152
        if board[k + 6] != 0 or board[k + 3] != g: # line 165
            return -1 # Goto 170
    elif board[k + 3]: # line 156
        return - 1

    return k + 6


def line_120(board, g, h, j, k):
    if board[j] != g:
        if board[j] == h or board[j+2] != g or board[j+1] != g:
            if board[k] != g:
                if board[k + 6] != g and (board[k + 6] != 0 or board[k+3] != g):
                    # 450 IF G=1 THEN 465
                    pass
            elif board[j + 2] is not g: # Line 122
                pass
            elif board[j + 1] is not OccupiedBy.EMPTY:
                pass


def line_118(board, g, h):
    for j in range(7):
        for k in range (3):
            return line_120(board, g, h, j, k)


def think(board, g, h, moves):

    if board[Space.MID_CENTER] is OccupiedBy.EMPTY:
        return Space.MID_CENTER

    if board[Space.MID_CENTER] is OccupiedBy.PLAYER:
        if board[Space.TOP_CENTER] is OccupiedBy.PLAYER and board[Space.TOP_LEFT] is OccupiedBy.EMPTY \
           or board[Space.MID_LEFT] is OccupiedBy.PLAYER and board[Space.TOP_LEFT] is OccupiedBy.EMPTY:
             return Space.BOT_LEFT
        elif board[Space.MID_RIGHT] is OccupiedBy.PLAYER and board[Space.BOT_RIGHT] is OccupiedBy.EMPTY \
           or board[Space.BOT_CENTER] is OccupiedBy.PLAYER and board[Space.BOT_RIGHT] is OccupiedBy.EMPTY:
             return Space.BOT_RIGHT

    if g == OccupiedBy.PLAYER:
        j = 3 * int((moves-1) / 3)
        if move == j + 1:
            k = 1
        if move == j + 2:
            k = 2
        if move == j + 3:
            k = 3
        return subthink(g, h, j, k)


def render_board(board, space_mapping):
    vertical_divider = '!'
    horizontal_divider = '---+---+---'
    lines = []
    lines.append(vertical_divider.join(space_mapping[space] for space in board[0:3]))
    lines.append(horizontal_divider)
    lines.append(vertical_divider.join(space_mapping[space] for space in board[3:6]))
    lines.append(horizontal_divider)
    lines.append(vertical_divider.join(space_mapping[space] for space in board[6:9]))
    return '\n'.join(lines)


def determine_winner(board, g):
    # Check for matching horizontal lines
    for i in range(Space.TOP_LEFT.value, Space.BOT_LEFT.value + 1, 3):      # Line 1095
        if board[i] != board[i+1] or board[i] != board[i+2]:  # Lines 1100 and 1105
            continue # First third of Line 1115 
        elif board[i] == OccupiedBy.COMPUTER: # 
            return Winner.COMPUTER
        elif board[i] == OccupiedBy.PLAYER:
            return Winner.PLAYER

    # Check for matching vertical lines
    for i in range(Space.TOP_LEFT.value, Space.TOP_LEFT.value + 1, 1):  # Second third of Line 1115
        if board[i] != board[i+3] or board[i] != board[i+6]: # Last third of Line 1115
            continue # First third of 1150
        elif board[i] == OccupiedBy.COMPUTER: # Line 1135
            return Winner.COMPUTER
        elif board[i] == OccupiedBy.PLAYER:   # Line 1137
            return Winner.PLAYER

    # Check diagonals
    if any(space is OccupiedBy.EMPTY for space in board):
        if board[Space.MID_CENTER.value] != g:
            return Winner.NONE
        elif (board[Space.TOP_LEFT.value] == g and board[Space.BOT_RIGHT.value] == g) or \
            (board[Space.BOT_LEFT.value] == g and board[Space.TOP_RIGHT.value] == g):
            return Winner.COMPUTER if g is OccupiedBy.COMPUTER else Winner.PLAYER
        else:
            return Winner.NONE

    return Winner.DRAW


def computer_think(board):
        empty_indices = [index for index, space in enumerate(board)
                         if space is OccupiedBy.EMPTY]

        return empty_indices[0]


def prompt_player(board):
    while True:
        move = int(input("\nWHERE DO YOU MOVE? "))

        if move == 0:
            return 0

        if move > 9 or board[move - 1] is not OccupiedBy.EMPTY:
            print("THAT SQUARE IS OCCUPIED.\n\n")
            continue

        return move


def play():
    print(' '*30 + 'TIC-TAC-TOE')
    print(' '*15 + 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY')
    print('\n\n')

    print('THE BOARD IS NUMBERED:')
    print(' 1  2  3')
    print(' 4  5  6')
    print(' 7  8  9')
    print('\n\n')

    # Default state
    board = [OccupiedBy.EMPTY] * 9
    current_player = OccupiedBy.PLAYER
    space_mapping = {
        OccupiedBy.EMPTY: '   ',
        OccupiedBy.PLAYER: ' X ',
        OccupiedBy.COMPUTER: ' O '
    }

    symbol = input("DO YOU WANT 'X' OR 'O'? ").upper()

    # If the player doesn't choose X, then assume you want O
    # and the computer goes first.
    if symbol != "X":
        space_mapping[OccupiedBy.PLAYER] = ' O '
        space_mapping[OccupiedBy.COMPUTER] = ' X '
        current_player = OccupiedBy.COMPUTER

    while True:
        if current_player is OccupiedBy.PLAYER:
            move = prompt_player(board)
            if move == 0:
                print("THANKS FOR THE GAME.")
                break
            board[move - 1] = current_player
            print(render_board(board, space_mapping))

        elif current_player is OccupiedBy.COMPUTER:
            print("\nTHE COMPUTER MOVES TO...")
            board[computer_think(board)] = current_player

        print(render_board(board, space_mapping))

        winner = determine_winner(board, current_player)

        if winner is not Winner.NONE:
           print(winner)
           break

        if current_player is OccupiedBy.COMPUTER:
            current_player = OccupiedBy.PLAYER
        elif current_player is OccupiedBy.PLAYER:
            current_player = OccupiedBy.COMPUTER


if __name__ == '__main__':
    play()
