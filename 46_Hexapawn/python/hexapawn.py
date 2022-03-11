"""
HEXAPAWN

A machine learning game, an interpretation of HEXAPAWN game as
presented in Martin Gardner's "The Unexpected Hanging and Other
Mathematical Diversions", Chapter Eight: A Matchbox Game-Learning
Machine.

Original version for H-P timeshare system by R.A. Kaapke 5/5/76
Instructions by Jeff Dalton
Conversion to MITS BASIC by Steve North


Port to Python by Dave LeCompte
"""

# PORTING NOTES:
#
# I printed out the BASIC code and hand-annotated what each little block
# of code did, which feels amazingly retro.
#
# I encourage other porters that have a complex knot of GOTOs and
# semi-nested subroutines to do hard-copy hacking, it might be a
# different perspective that helps.
#
# A spoiler - the objective of the game is not documented, ostensibly to
# give the human player a challenge. If a player (human or computer)
# advances a pawn across the board to the far row, that player wins. If
# a player has no legal moves (either by being blocked, or all their
# pieces having been captured), that player loses.
#
# The original BASIC had 2 2-dimensional tables stored in DATA at the
# end of the program. This encoded all 19 different board configurations
# (Hexapawn is a small game), with reflections in one table, and then in
# a parallel table, for each of the 19 rows, a list of legal moves was
# encoded by turning them into 2-digit decimal numbers. As gameplay
# continued, the AI would overwrite losing moves with 0 in the second
# array.
#
# My port takes this "parallel array" structure and turns that
# information into a small Python class, BoardLayout. BoardLayout stores
# the board description and legal moves, but stores the moves as (row,
# column) 2-tuples, which is easier to read. The logic for checking if a
# BoardLayout matches the current board, as well as removing losing move
# have been moved into methods of this class.

import collections
import random

PAGE_WIDTH = 64

HUMAN_PIECE = 1
EMPTY_SPACE = 0
COMPUTER_PIECE = -1

ComputerMove = collections.namedtuple(
    "ComputerMove", ["board_index", "move_index", "m1", "m2"]
)

wins = 0
losses = 0


def print_centered(msg):
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title):
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def print_instructions():
    print(
        """
THIS PROGRAM PLAYS THE GAME OF HEXAPAWN.
HEXAPAWN IS PLAYED WITH CHESS PAWNS ON A 3 BY 3 BOARD.
THE PAWNS ARE MOVED AS IN CHESS - ONE SPACE FORWARD TO
AN EMPTY SPACE OR ONE SPACE FORWARD AND DIAGONALLY TO
CAPTURE AN OPPOSING MAN.  ON THE BOARD, YOUR PAWNS
ARE 'O', THE COMPUTER'S PAWNS ARE 'X', AND EMPTY
SQUARES ARE '.'.  TO ENTER A MOVE, TYPE THE NUMBER OF
THE SQUARE YOU ARE MOVING FROM, FOLLOWED BY THE NUMBER
OF THE SQUARE YOU WILL MOVE TO.  THE NUMBERS MUST BE
SEPERATED BY A COMMA.

THE COMPUTER STARTS A SERIES OF GAMES KNOWING ONLY WHEN
THE GAME IS WON (A DRAW IS IMPOSSIBLE) AND HOW TO MOVE.
IT HAS NO STRATEGY AT FIRST AND JUST MOVES RANDOMLY.
HOWEVER, IT LEARNS FROM EACH GAME.  THUS, WINNING BECOMES
MORE AND MORE DIFFICULT.  ALSO, TO HELP OFFSET YOUR
INITIAL ADVANTAGE, YOU WILL NOT BE TOLD HOW TO WIN THE
GAME BUT MUST LEARN THIS BY PLAYING.

THE NUMBERING OF THE BOARD IS AS FOLLOWS:
          123
          456
          789

FOR EXAMPLE, TO MOVE YOUR RIGHTMOST PAWN FORWARD,
YOU WOULD TYPE 9,6 IN RESPONSE TO THE QUESTION
'YOUR MOVE ?'.  SINCE I'M A GOOD SPORT, YOU'LL ALWAYS
GO FIRST.

"""
    )


def prompt_yes_no(msg):
    while True:
        print(msg)
        response = input().upper()
        if response[0] == "Y":
            return True
        elif response[0] == "N":
            return False


def reverse_space_name(space_name):
    # reverse a space name in the range 1-9 left to right
    assert 1 <= space_name <= 9

    reflections = {1: 3, 2: 2, 3: 1, 4: 6, 5: 5, 6: 4, 7: 9, 8: 8, 9: 7}
    return reflections[space_name]


def is_space_in_center_column(space_name):
    return reverse_space_name(space_name) == space_name


class BoardLayout:
    def __init__(self, cells, move_list):
        self.cells = cells
        self.moves = move_list

    def _check_match_no_mirror(self, cell_list):
        for space_index, board_contents in enumerate(self.cells):
            if board_contents != cell_list[space_index]:
                return False
        return True

    def _check_match_with_mirror(self, cell_list):
        for space_index, board_contents in enumerate(self.cells):
            reversed_space_index = reverse_space_name(space_index + 1) - 1
            if board_contents != cell_list[reversed_space_index]:
                return False
        return True

    def check_match(self, cell_list):
        if self._check_match_with_mirror(cell_list):
            return True, True
        elif self._check_match_no_mirror(cell_list):
            return True, False
        return False, None

    def get_random_move(self, reverse_board):
        if not self.moves:
            return None
        move_index = random.randrange(len(self.moves))

        m1, m2 = self.moves[move_index]
        if reverse_board:
            m1 = reverse_space_name(m1)
            m2 = reverse_space_name(m2)

        return move_index, m1, m2


boards = [
    BoardLayout([-1, -1, -1, 1, 0, 0, 0, 1, 1], [(2, 4), (2, 5), (3, 6)]),
    BoardLayout([-1, -1, -1, 0, 1, 0, 1, 0, 1], [(1, 4), (1, 5), (3, 6)]),
    BoardLayout([-1, 0, -1, -1, 1, 0, 0, 0, 1], [(1, 5), (3, 5), (3, 6), (4, 7)]),
    BoardLayout([0, -1, -1, 1, -1, 0, 0, 0, 1], [(3, 6), (5, 8), (5, 9)]),
    BoardLayout([-1, 0, -1, 1, 1, 0, 0, 1, 0], [(1, 5), (3, 5), (3, 6)]),
    BoardLayout([-1, -1, 0, 1, 0, 1, 0, 0, 1], [(2, 4), (2, 5), (2, 6)]),
    BoardLayout([0, -1, -1, 0, -1, 1, 1, 0, 0], [(2, 6), (5, 7), (5, 8)]),
    BoardLayout([0, -1, -1, -1, 1, 1, 1, 0, 0], [(2, 6), (3, 5)]),
    BoardLayout([-1, 0, -1, -1, 0, 1, 0, 1, 0], [(4, 7), (4, 8)]),
    BoardLayout([0, -1, -1, 0, 1, 0, 0, 0, 1], [(3, 5), (3, 6)]),
    BoardLayout([0, -1, -1, 0, 1, 0, 1, 0, 0], [(3, 5), (3, 6)]),
    BoardLayout([-1, 0, -1, 1, 0, 0, 0, 0, 1], [(3, 6)]),
    BoardLayout([0, 0, -1, -1, -1, 1, 0, 0, 0], [(4, 7), (5, 8)]),
    BoardLayout([-1, 0, 0, 1, 1, 1, 0, 0, 0], [(1, 5)]),
    BoardLayout([0, -1, 0, -1, 1, 1, 0, 0, 0], [(2, 6), (4, 7)]),
    BoardLayout([-1, 0, 0, -1, -1, 1, 0, 0, 0], [(4, 7), (5, 8)]),
    BoardLayout([0, 0, -1, -1, 1, 0, 0, 0, 0], [(3, 5), (3, 6), (4, 7)]),
    BoardLayout([0, -1, 0, 1, -1, 0, 0, 0, 0], [(2, 8), (5, 8)]),
    BoardLayout([-1, 0, 0, -1, 1, 0, 0, 0, 0], [(1, 5), (4, 7)]),
]


def get_move(board_index, move_index):
    assert board_index >= 0 and board_index < len(boards)
    board = boards[board_index]

    assert move_index >= 0 and move_index < len(board.moves)

    return board.moves[move_index]


def remove_move(board_index, move_index):
    assert board_index >= 0 and board_index < len(boards)
    board = boards[board_index]

    assert move_index >= 0 and move_index < len(board.moves)

    del board.moves[move_index]


def init_board():
    return [COMPUTER_PIECE] * 3 + [EMPTY_SPACE] * 3 + [HUMAN_PIECE] * 3


def print_board(board):
    piece_dict = {COMPUTER_PIECE: "X", EMPTY_SPACE: ".", HUMAN_PIECE: "O"}

    space = " " * 10
    print()
    for row in range(3):
        line = ""
        for column in range(3):
            line += space
            space_number = row * 3 + column
            space_contents = board[space_number]
            line += piece_dict[space_contents]
        print(line)
    print()


def get_coordinates():
    while True:
        try:
            print("YOUR MOVE?")
            response = input()
            m1, m2 = (int(c) for c in response.split(","))
            return m1, m2
        except ValueError:
            print_illegal()


def print_illegal():
    print("ILLEGAL MOVE.")


def board_contents(board, space_number):
    return board[space_number - 1]


def set_board(board, space_number, new_value):
    board[space_number - 1] = new_value


def is_legal_human_move(board, m1, m2):
    if board_contents(board, m1) != HUMAN_PIECE:
        # Start space doesn't contain player's piece
        return False
    if board_contents(board, m2) == HUMAN_PIECE:
        # Destination space contains player's piece (can't capture your own piece)
        return False

    is_capture = m2 - m1 != -3
    if is_capture and board_contents(board, m2) != COMPUTER_PIECE:
        # Destination does not contain computer piece
        return False

    if m2 > m1:
        # can't move backwards
        return False

    if (not is_capture) and board_contents(board, m2) != EMPTY_SPACE:
        # Destination is not open
        return False

    if m2 - m1 < -4:
        # too far
        return False

    if m1 == 7 and m2 == 3:
        # can't jump corner to corner (wrapping around the board)
        return False
    return True


def player_piece_on_back_row(board):
    for space in range(1, 4):
        if board_contents(board, space) == HUMAN_PIECE:
            return True
    return False


def computer_piece_on_front_row(board):
    for space in range(7, 10):
        if board_contents(board, space) == COMPUTER_PIECE:
            return True
    return False


def all_human_pieces_captured(board):
    return len(list(get_human_spaces(board))) == 0


def all_computer_pieces_captured(board):
    return len(list(get_computer_spaces(board))) == 0


def human_win(last_computer_move):
    print("YOU WIN")
    remove_move(last_computer_move.board_index, last_computer_move.move_index)
    global losses
    losses += 1


def computer_win(has_moves):
    if not has_moves:
        msg = "YOU CAN'T MOVE, SO "
    else:
        msg = ""
    msg += "I WIN"
    print(msg)
    global wins
    wins += 1


def show_scores():
    print(f"I HAVE WON {wins} AND YOU {losses} OUT OF {wins + losses} GAMES.")
    print()


def human_has_move(board):
    for i in get_human_spaces(board):
        if board_contents(board, i - 3) == EMPTY_SPACE:
            # can move piece forward
            return True
        elif is_space_in_center_column(i):
            if (board_contents(board, i - 2) == COMPUTER_PIECE) or (
                board_contents(board, i - 4) == COMPUTER_PIECE
            ):
                # can capture from center
                return True
            else:
                continue
        elif i < 7:
            assert (i == 4) or (i == 6)
            if board_contents(board, 2) == COMPUTER_PIECE:
                # can capture computer piece at 2
                return True
            else:
                continue
        elif board_contents(board, 5) == COMPUTER_PIECE:
            assert (i == 7) or (i == 9)
            # can capture computer piece at 5
            return True
        else:
            continue
    return False


def get_board_spaces():
    """generates the space names (1-9)"""
    yield from range(1, 10)


def get_board_spaces_with(board, val):
    """generates spaces containing pieces of type val"""
    for i in get_board_spaces():
        if board_contents(board, i) == val:
            yield i


def get_human_spaces(board):
    yield from get_board_spaces_with(board, HUMAN_PIECE)


def get_empty_spaces(board):
    yield from get_board_spaces_with(board, EMPTY_SPACE)


def get_computer_spaces(board):
    yield from get_board_spaces_with(board, COMPUTER_PIECE)


def has_computer_move(board):
    for i in get_computer_spaces(board):
        if board_contents(board, i + 3) == EMPTY_SPACE:
            # can move forward (down)
            return True

        if is_space_in_center_column(i):
            # i is in the middle column
            if (board_contents(board, i + 2) == HUMAN_PIECE) or (
                board_contents(board, i + 4) == HUMAN_PIECE
            ):
                return True
        else:
            if i > 3:
                # beyond the first row
                if board_contents(board, 8) == HUMAN_PIECE:
                    # can capture on 8
                    return True
                else:
                    continue
            else:
                if board_contents(board, 5) == HUMAN_PIECE:
                    # can capture on 5
                    return True
                else:
                    continue
    return False


def find_board_index_that_matches_board(board):
    for board_index, board_layout in enumerate(boards):
        matches, is_reversed = board_layout.check_match(board)
        if matches:
            return board_index, is_reversed

    # THE TERMINATION OF THIS LOOP IS IMPOSSIBLE
    print("ILLEGAL BOARD PATTERN.")
    assert False


def pick_computer_move(board):
    if not has_computer_move(board):
        return None

    board_index, reverse_board = find_board_index_that_matches_board(board)

    m = boards[board_index].get_random_move(reverse_board)

    if m is None:
        print("I RESIGN")
        return None

    move_index, m1, m2 = m

    return ComputerMove(board_index, move_index, m1, m2)


def get_human_move(board):
    while True:
        m1, m2 = get_coordinates()

        if not is_legal_human_move(board, m1, m2):
            print_illegal()
        else:
            return m1, m2


def apply_move(board, m1, m2, piece_value):
    set_board(board, m1, EMPTY_SPACE)
    set_board(board, m2, piece_value)


def play_game():
    last_computer_move = None

    board = init_board()

    while True:
        print_board(board)

        m1, m2 = get_human_move(board)

        apply_move(board, m1, m2, HUMAN_PIECE)

        print_board(board)

        if player_piece_on_back_row(board) or all_computer_pieces_captured(board):
            human_win(last_computer_move)
            return

        computer_move = pick_computer_move(board)
        if computer_move is None:
            human_win(last_computer_move)
            return

        last_computer_move = computer_move

        m1, m2 = last_computer_move.m1, last_computer_move.m2

        print(f"I MOVE FROM {m1} TO {m2}")
        apply_move(board, m1, m2, COMPUTER_PIECE)

        print_board(board)

        if computer_piece_on_front_row(board):
            computer_win(True)
            return
        elif (not human_has_move(board)) or (all_human_pieces_captured(board)):
            computer_win(False)
            return


def main():
    print_header("HEXAPAWN")
    if prompt_yes_no("INSTRUCTIONS (Y-N)?"):
        print_instructions()

    global wins, losses
    wins = 0
    losses = 0

    while True:
        play_game()
        show_scores()


if __name__ == "__main__":
    main()
