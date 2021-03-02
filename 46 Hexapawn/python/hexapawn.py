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

import collections
import random

PAGE_WIDTH = 64

HUMAN_PIECE = 1
EMPTY_SPACE = 0
COMPUTER_PIECE = -1

ComputerMove = collections.namedtuple('ComputerMove', ['x', 'y', 'm1', 'm2'])

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
    print("""
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

""")


def prompt_yes_no(msg):
    while True:
        print(msg)
        response = input().upper()
        if response[0] == "Y":
            return True
        elif response[0] == "N":
            return False

def reverse_board_position(x):
    assert(x >= 1 and x < 10)
    
    score = {1: 3,
             2: 2,
             3: 1,
             4: 6,
             5: 5,
             6: 4,
             7: 9,
             8: 8,
             9: 7}
    return score[x]

def get_b(x, y):
    data = [[-1, -1, -1,  1,  0,  0,  0,  1,  1],
            [-1, -1, -1,  0,  1,  0,  1,  0,  1],
            [-1,  0, -1, -1,  1,  0,  0,  0,  1],
            [ 0, -1, -1,  1, -1,  0,  0,  0,  1],
            [-1,  0, -1,  1,  1,  0,  0,  1,  0],
            [-1, -1,  0,  1,  0,  1,  0,  0,  1],
            [ 0, -1, -1,  0, -1,  1,  1,  0,  0],
            [ 0, -1, -1, -1,  1,  1,  1,  0,  0],
            [-1,  0, -1, -1,  0,  1,  0,  1,  0],
            [ 0, -1, -1,  0,  1,  0,  0,  0,  1],
            [ 0, -1, -1,  0,  1,  0,  1,  0,  0],
            [-1,  0, -1,  1,  0,  0,  0,  0,  1],
            [ 0,  0, -1, -1, -1,  1,  0,  0,  0],
            [-1,  0,  0,  1,  1,  1,  0,  0,  0],
            [ 0, -1,  0, -1,  1,  1,  0,  0,  0],
            [-1,  0,  0, -1, -1,  1,  0,  0,  0],
            [ 0,  0, -1, -1,  1,  0,  0,  0,  0],
            [ 0, -1,  0,  1, -1,  0,  0,  0,  0],
            [-1,  0,  0, -1,  1,  0,  0,  0,  0]]

    assert(x >= 1 and x < 20)
    assert(y >= 1 and y < 10)

    return data[x-1][y-1]

m_data = [[24, 25, 36,  0],
          [14, 15, 36,  0],
          [15, 35, 36, 47],
          [36, 58, 59,  0],
          [15, 35, 36,  0],
          [24, 25, 26,  0],
          [26, 57, 58,  0],
          [26, 35,  0,  0],
          [47, 48,  0,  0],
          [35, 36,  0,  0],
          [35, 36,  0,  0],
          [36,  0,  0,  0],
          [47, 58,  0,  0],
          [15,  0,  0,  0],
          [26, 47,  0,  0],
          [47, 58,  0,  0],
          [35, 36, 47,  0],
          [28, 58,  0,  0],
          [15, 47,  0,  0]]

def get_m(x, y):
    assert(x >= 1 and x < 20)
    assert(y >= 1 and y < 5)

    return m_data[x-1][y-1]

def set_m(x, y, value):
    m_data[x-1][y-1] = value

def init_board():
    return ([COMPUTER_PIECE] * 3 +
            [EMPTY_SPACE] * 3 +
            [HUMAN_PIECE] * 3)

def print_board(board):
    piece_dict = {COMPUTER_PIECE: 'X',
                  EMPTY_SPACE: '.',
                  HUMAN_PIECE: 'O'}

    space = " "*10
    print()
    for i in range(3):
        line = ""
        for j in range(3):
            line += space
            space_number = i * 3 + j
            space_contents = board[space_number]
            line += piece_dict[space_contents]
        print(line)
    print()

def get_coordinates():
    while True:
        try:
            print ("YOUR MOVE?")
            response = input()
            m1, m2 = [int(c) for c in response.split(',')]
            return m1, m2
        except ValueError as ve:
            print_illegal()

def print_illegal():
    print("ILLEGAL MOVE.")

def board_contents(board, space_number):
    return board[space_number - 1]

def set_board(board, space_number, new_value):
    board[space_number - 1] = new_value

def is_legal_move(board, m1, m2):
    if board_contents(board, m1) != HUMAN_PIECE:
        # Start space doesn't contain player's piece
        return False
    if board_contents(board, m2) == HUMAN_PIECE:
        # Destination space contains player's piece (can't capture your own piece)
        return False
    # line 160
    is_capture = (m2-m1 != -3)
    if is_capture and board_contents(board, m2) != COMPUTER_PIECE:
        # Destination does not contain computer piece
        return False
    # line 170
    if m2 > m1:
        # can't move backwards
        return False
    # line 180
    if (not is_capture) and board_contents(board, m2) != EMPTY_SPACE:
        # Destination is not open
        return False
    # line 185
    if m2-m1 < -4:
        # too far
        return False
    # line 186
    if m1 == 7 and m2 == 3:
        # can't jump corner to corner (wrapping around the board)
        return False
    return True

def player_piece_on_back_row(board):
    for space in range(1,4):
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
    set_m(last_computer_move.x, last_computer_move.y, 0)
    global l
    l += 1

def computer_win(has_moves):
    if has_moves:
        msg = "YOU CAN'T MOVE, SO "
    else:
        msg = ""
    msg += "I WIN"
    print(msg)
    global w
    w += 1

def show_scores():
    print(f"I HAVE WON {w} AND YOU {l} OUT OF {w+l} GAMES.")
    print()

def human_has_move(board):
    # line 690
    for i in get_human_spaces(board):
        if board_contents(board, i-3) == EMPTY_SPACE:
            # can move piece forward
            return True
        elif reverse_board_position(i) == i:
            # line 780
            # can capture from center
            if ((board_contents(board, i-2) == COMPUTER_PIECE) or
                (board_contents(board, i-4) == COMPUTER_PIECE)):
                return True
            else:
                continue 
        elif i < 7:
            # Line 760            
            assert((i == 4) or (i == 6))
            # can capture computer piece at 2
            if board_contents(board, 2) == COMPUTER_PIECE:
                return True
            else:
                continue
        elif board_contents(board, 5) == COMPUTER_PIECE:
            assert((i == 7) or (i == 9))
            # can capture computer piece at 5
            return True
        else:
            continue
    return False


def get_board_spaces():
    yield from range(1, 10)

def get_board_spaces_with(board, val):
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
        found_move = False
        if board_contents(board, i+3) == EMPTY_SPACE:
            # can move forward (down)
            return True

        # line 260
        if reverse_board_position(i) == i:
            # i is in the middle column
            if ((board_contents(board, i + 2) == HUMAN_PIECE) or
                (board_contents(board, i + 4) == HUMAN_PIECE)):
                return True
        else:
            # line 270
            if i > 3:
                # beyond the first row
                if board_contents(board, 8) == HUMAN_PIECE:
                    # can capture on 8
                    return True
                else:
                    continue
            else:
                # line 280
                if board_contents(board, 5) == HUMAN_PIECE:
                    # can capture on 5
                    return True
                else:
                    continue
    return False

def get_flipped_table(b_line): # TODO remove table altogether
    t = {}
    # line 360
    for row in range(1, 4):
        for column in range(1, 4):
            # line 380
            flipped_column = 4 - column
            
            # fill out t to represent the data from b flipped left to right
            space = (row-1) * 3 + column
            flipped_space = (row - 1) * 3 + flipped_column
            
            t[space] = get_b(b_line, flipped_space)
    return t

def board_matches_b(b_line, board):
    for s in get_board_spaces():
        if get_b(b_line, s) != board_contents(board, s):
            return False
    return True

def board_matches_flipped_b(b_line, board):
    flipped_table = get_flipped_table(b_line)
            
    for s in get_board_spaces():
        if flipped_table[s] != board_contents(board, s):
            return False
    return True

def does_b_line_match(b_line, board):
    if board_matches_b(b_line, board):
        return True, False
    elif board_matches_flipped_b(b_line, board):
        return True, True
    else:
        return False, None

def has_any_m_table(x):
    for i in range(1,5):
        if get_m(x, i) != 0:
            return True
    return False

def pick_from_m_table(x):
    valid_y_list = [y for y in range(1,5) if get_m(x, y) != 0]
    assert(len(valid_y_list) > 0)
    return random.choice(valid_y_list)
        

def get_move_for_b_line(b_line, reverse_board):
    # line 540
    x = b_line

    if not has_any_m_table(x):
        return None

    # line 600
    y = pick_from_m_table(x)
    
    # line 610
    mxy = get_m(x, y)
    m1 = mxy // 10
    m2 = mxy % 10
    if reverse_board:
        m1 = reverse_board_position(m1)
        m2 = reverse_board_position(m2)

    return ComputerMove(x, y, m1, m2)


def find_b_line_that_matches_board(board):
    for b_line in range(1,20):
         matches, reverse_board = does_b_line_match(b_line, board)
         if matches:
             return b_line, reverse_board
                    
    # THE TERMINATION OF THIS LOOP IS IMPOSSIBLE
    print("ILLEGAL BOARD PATTERN.")
    assert(False)
    
    
def pick_computer_move(board):
    if not has_computer_move(board):
        # Line 340
        return None

    # line 350
    b_line, reverse_board = find_b_line_that_matches_board(board)

    m = get_move_for_b_line(b_line, reverse_board)

    if m == None:
        print("I RESIGN")
        return None

    return m
    
    


def play_game():
    last_computer_move = None

    board = init_board()

    while True:
        print_board(board)
    
        has_legal_move = False
        while not has_legal_move:
            m1, m2 = get_coordinates()
    
            if not is_legal_move(board, m1, m2):
                print_illegal()
            else:
                # otherwise, acceptable move
                has_legal_move = True
            
        set_board(board, m1, 0)
        set_board(board, m2, 1)
    
        # line 205
        print_board(board)
    
        if (player_piece_on_back_row(board) or
            all_computer_pieces_captured(board)):
            human_win(last_computer_move)
            return
    
        # line 230
        computer_move = pick_computer_move(board)
        if computer_move is None:
            human_win(last_computer_move)
            return

        last_computer_move = computer_move
    
        m1, m2 = last_computer_move.m1, last_computer_move.m2
        
        print(f"I MOVE FROM {m1} TO {m2}")
        set_board(board, m1, 0)
        set_board(board, m2, -1)
    
        # line 640
        print_board(board)
    
        if (computer_piece_on_front_row(board) or
            all_human_pieces_captured(board)):
            computer_win(True)
            return
        elif not human_has_move(board):
            computer_win(False)
            return
    
    
        
            

def main():
    print_header("HEXAPAWN")
    if prompt_yes_no("INSTRUCTIONS (Y-N)?"):
        print_instructions()

    global w, l
    w = 0
    l = 0

    while True:
        play_game()
        show_scores()
        
if __name__ == "__main__":
    main()

"""
230 FOR I=1 TO 9
240 IF S(I)<>-1 THEN 330
250 IF S(I+3)=0 THEN 350
260 IF FNR(I)=I THEN 320
270 IF I>3 THEN 300
280 IF S(5)=1 THEN 350
290 GOTO 330
300 IF S(8)=1 THEN 350
310 GOTO 330
320 IF S(I+2)=1 OR S(I+4)=1 THEN 350
330 NEXT I
340 GOTO 820
350 FOR I=1 TO 19
360 FOR J=1 TO 3
370 FOR K=3 TO 1 STEP -1
380 T((J-1)*3+K)=B(I,(J-1)*3+4-K)
390 NEXT K
400 NEXT J
410 FOR J=1 TO 9
420 IF S(J)<>B(I,J) THEN 460
430 NEXT J
440 R=0
450 GOTO 540
460 FOR J=1 TO 9
470 IF S(J)<>T(J) THEN 510
480 NEXT J
490 R=1
500 GOTO 540
510 NEXT I
511 REMEMBER THE TERMINATION OF THIS LOOP IS IMPOSSIBLE
512 PRINT "ILLEGAL BOARD PATTERN."
530 STOP
540 X=I
550 FOR I=1 TO 4
560 IF M(X,I)<>0 THEN 600
570 NEXT I
580 PRINT "I RESIGN."
590 GOTO 820
600 Y=INT(RND(1)*4+1)
601 IF M(X,Y)=0 THEN 600
610 IF R<>0 THEN 630
620 PRINT "I MOVE FROM ";STR$(INT(M(X,Y)/10));" TO ";STR$(FNM(M(X,Y)))
622 S(INT(M(X,Y)/10))=0
623 S(FNM(M(X,Y)))=-1
624 GOTO 640
630 PRINT "I MOVE FROM ";STR$(FNR(INT(M(X,Y)/10)));" TO ";
631 PRINT STR$(FNR(FNM(M(X,Y))))
632 S(FNR(INT(M(X,Y)/10)))=0
633 S(FNR(FNM(M(X,Y))))=-1
640 GOSUB 1000
641 IF S(7)=-1 OR S(8)=-1 OR S(9)=-1 THEN 870
650 FOR I=1 TO 9
660 IF S(I)=1 THEN 690
670 NEXT I
680 GOTO 870
690 FOR I=1 TO 9
700 IF S(I)<>1 THEN 790
710 IF S(I-3)=0 THEN 120
720 IF FNR(I)=I THEN 780
730 IF I<7 THEN 760
740 IF S(5)=-1 THEN 120
750 GOTO 790
760 IF S(2)=-1 THEN 120
770 GOTO 790
780 IF S(I-2)=-1 OR S(I-4)=-1 THEN 120
790 NEXT I
800 PRINT "YOU CAN'T MOVE, SO ";
810 GOTO 870
820 PRINT "YOU WIN."
830 M(X,Y)=0
840 L=L+1
850 PRINT "I HAVE WON";W;"AND YOU";L;"OUT OF";L+W;"GAMES."
851 PRINT
860 GOTO 100
870 PRINT "I WIN."
880 W=W+1
890 GOTO 850
"""
