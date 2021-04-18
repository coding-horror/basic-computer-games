"""
AWARI

An ancient African game (see also Kalah, Mancala).

Ported by Dave LeCompte
"""

"""

PORTING NOTES

This game started out as 70 lines of BASIC, and I have ported it
before. I find it somewhat amazing how efficient (densely packed) the
original code is. Of course, the original code has fairly cryptic
variable names (as was forced by BASIC's limitation on long (2+
character) variable names). I have done my best here to interpret what
each variable is doing in context, and rename them appropriately.

I have endeavored to leave the logic of the code in place, as it's
interesting to see a 2-ply game tree evaluation written in BASIC,
along with what a reader in 2021 would call "machine learning".

As each game is played, the move history is stored as base-6
digits stored losing_book[game_number]. If the human player wins or
draws, the computer increments game_number, effectively "recording"
that loss to be referred to later. As the computer evaluates moves, it
checks the potential game state against these losing game records, and
if the potential move matches with the losing game (up to the current
number of moves), that move is evaluated at a two point penalty.  

Compare this, for example with MENACE, a mechanical device for
"learning" tic-tac-toe:
https://en.wikipedia.org/wiki/Matchbox_Educable_Noughts_and_Crosses_Engine

The base-6 representation allows game history to be VERY efficiently
represented. I considered whether to rewrite this representation to be
easier to read, but I elected to TRY to document it, instead.

Another place where I have made a difficult decision between accuracy
and correctness is inside the "wrapping" code where it considers
"while human_move_end > 13". The original BASIC code reads:

830 IF L>13 THEN L=L-14:R=1:GOTO 830

I suspect that the intention is not to assign 1 to R, but to increment
R. I discuss this more in a porting note comment next to the
translated code. If you wish to play a more accurate version of the
game as written in the book, you can convert the increment back to an
assignment.


I continue to be impressed with this jewel of a game; as soon as I had
the AI playing against me, it was beating me. I've been able to score
a few wins against the computer, but even at its 2-ply lookahead, it
beats me nearly always. I would like to become better at this game to
explore the effectiveness of the "losing book" machine learning.


EXERCISES FOR THE READER
One could go many directions with this game:

- change the initial number of stones in each pit

- change the number of pits

- only allow capturing if you end on your side of the board

- don't allow capturing at all

- don't drop a stone into the enemy "home"

- go clockwise, instead

- allow the player to choose to go clockwise or counterclockwise

- instead of a maximum of two moves, allow each move that ends on the
  "home" to be followed by a free move.

- increase the AI lookahead

- make the scoring heuristic a little more nuanced

- store history to a file on disk (or in the cloud!) to allow the AI
  to learn over more than a single session

"""


game_number = 0
move_count = 0
losing_book = []
n = 0

MAX_HISTORY = 9
LOSING_BOOK_SIZE = 50


def print_with_tab(space_count, msg):
    if space_count > 0:
        spaces = " " * space_count
    else:
        spaces = ""
    print(spaces + msg)


def draw_pit(line, board, pit_index):
    val = board[pit_index]
    line = line + " "
    if val < 10:
        line = line + " "
    line = line + str(val) + " "
    return line


def draw_board(board):
    print()

    # Draw the top (computer) pits
    line = "   "
    for i in range(12, 6, -1):
        line = draw_pit(line, board, i)
    print(line)

    # Draw the side (home) pits
    line = draw_pit("", board, 13)
    line += " " * 24
    line = draw_pit(line, board, 6)
    print(line)

    # Draw the bottom (player) pits
    line = "   "
    for i in range(0, 6):
        line = draw_pit(line, board, i)
    print(line)
    print()
    print()


def play_game(board):
    # Place the beginning stones
    for i in range(0, 13):
        board[i] = 3

    # Empty the home pits
    board[6] = 0
    board[13] = 0

    global move_count
    move_count = 0

    # clear the history record for this game
    losing_book[game_number] = 0

    while True:
        draw_board(board)

        print("YOUR MOVE")
        landing_spot, is_still_going, home = player_move(board)
        if not is_still_going:
            break
        if landing_spot == home:
            landing_spot, is_still_going, home = player_move_again(board)
        if not is_still_going:
            break

        print("MY MOVE")
        landing_spot, is_still_going, home, msg = computer_move("", board)

        if not is_still_going:
            print(msg)
            break
        if landing_spot == home:
            landing_spot, is_still_going, home, msg = computer_move(msg + " , ", board)
        if not is_still_going:
            print(msg)
            break
        print(msg)

    game_over(board)


def computer_move(msg, board):
    # This function does a two-ply lookahead evaluation; one computer
    # move plus one human move.
    #
    # To do this, it makes a copy (temp_board) of the board, plays
    # each possible computer move and then uses math to work out what
    # the scoring heuristic is for each possible human move.
    #
    # Additionally, if it detects that a potential move puts it on a
    # series of moves that it has recorded in its "losing book", it
    # penalizes that move by two stones.

    best_quality = -99

    # Make a copy of the board, so that we can experiment. We'll put
    # everything back, later.
    temp_board = board[:]

    # For each legal computer move 7-12
    for computer_move in range(7, 13):
        if board[computer_move] == 0:
            continue
        do_move(computer_move, 13, board)  # try the move (1 move lookahead)

        best_player_move_quality = 0
        # for all legal human moves 0-5 (responses to computer move computer_move)
        for human_move_start in range(0, 6):
            if board[human_move_start] == 0:
                continue

            human_move_end = board[human_move_start] + human_move_start
            this_player_move_quality = 0

            # If this move goes around the board, wrap backwards.
            #
            # PORTING NOTE: The careful reader will note that I am
            # incrementing this_player_move_quality for each wrap,
            # while the original code only set it equal to 1.
            #
            # I expect this was a typo or oversight, but I also
            # recognize that you'd have to go around the board more
            # than once for this to be a difference, and even so, it
            # would be a very small difference; there are only 36
            # stones in the game, and going around the board twice
            # requires 24 stones.

            while human_move_end > 13:
                human_move_end = human_move_end - 14
                this_player_move_quality += 1

            if (
                (board[human_move_end] == 0)
                and (human_move_end != 6)
                and (human_move_end != 13)
            ):
                # score the capture
                this_player_move_quality += board[12 - human_move_end]

            if this_player_move_quality > best_player_move_quality:
                best_player_move_quality = this_player_move_quality

        # This is a zero sum game, so the better the human player's
        # move is, the worse it is for the computer player.
        computer_move_quality = board[13] - board[6] - best_player_move_quality

        if move_count < MAX_HISTORY:
            move_digit = computer_move
            if move_digit > 6:
                move_digit = move_digit - 7

            # Calculate the base-6 history representation of the game
            # with this move. If that history is in our "losing book",
            # penalize that move.
            for prev_game_number in range(game_number):
                if losing_book[game_number] * 6 + move_digit == int(
                    losing_book[prev_game_number] / 6 ^ (7 - move_count) + 0.1
                ):
                    computer_move_quality -= 2

        # Copy back from temporary board
        for i in range(14):
            board[i] = temp_board[i]

        if computer_move_quality >= best_quality:
            best_move = computer_move
            best_quality = computer_move_quality

    selected_move = best_move

    move_str = chr(42 + selected_move)
    if msg:
        msg += ", " + move_str
    else:
        msg = move_str

    move_number, is_still_going, home = execute_move(selected_move, 13, board)

    return move_number, is_still_going, home, msg


def game_over(board):
    print()
    print("GAME OVER")

    pit_difference = board[6] - board[13]
    if pit_difference < 0:
        print(f"I WIN BY {-pit_difference} POINTS")

    else:
        global n
        n = n + 1

        if pit_difference == 0:
            print("DRAWN GAME")
        else:
            print(f"YOU WIN BY {pit_difference} POINTS")


def do_capture(m, home, board):
    board[home] += board[12 - m] + 1
    board[m] = 0
    board[12 - m] = 0


def do_move(m, home, board):
    move_stones = board[m]
    board[m] = 0

    for stones in range(move_stones, 0, -1):
        m = m + 1
        if m > 13:
            m = m - 14
        board[m] += 1
    if board[m] == 1:
        # capture
        if (m != 6) and (m != 13) and (board[12 - m] != 0):
            do_capture(m, home, board)
    return m


def player_has_stones(board):
    for i in range(6):
        if board[i] > 0:
            return True
    return False


def computer_has_stones(board):
    for i in range(7, 13):
        if board[i] > 0:
            return True
    return False


def execute_move(move, home, board):
    move_digit = move
    last_location = do_move(move, home, board)

    if move_digit > 6:
        move_digit = move_digit - 7

    global move_count
    move_count += 1
    if move_count < MAX_HISTORY:
        # The computer keeps a chain of moves in losing_book by
        # storing a sequence of moves as digits in a base-6 number.
        #
        # game_number represents the current game,
        # losing_book[game_number] records the history of the ongoing
        # game.  When the computer evaluates moves, it tries to avoid
        # moves that will lead it into paths that have led to previous
        # losses.
        losing_book[game_number] = losing_book[game_number] * 6 + move_digit

    if player_has_stones(board) and computer_has_stones(board):
        is_still_going = True
    else:
        is_still_going = False
    return last_location, is_still_going, home


def player_move_again(board):
    print("AGAIN")
    return player_move(board)


def player_move(board):
    while True:
        print("SELECT MOVE 1-6")
        m = int(input()) - 1

        if m > 5 or m < 0 or board[m] == 0:
            print("ILLEGAL MOVE")
            continue

        break

    ending_spot, is_still_going, home = execute_move(m, 6, board)

    draw_board(board)

    return ending_spot, is_still_going, home


def main():
    print_with_tab(34, "AWARI")
    print_with_tab(15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()

    board = [0] * 14  # clear the board representation
    global losing_book
    losing_book = [0] * LOSING_BOOK_SIZE  # clear the "machine learning" state

    while True:
        play_game(board)


if __name__ == "__main__":
    main()
