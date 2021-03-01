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

PAGE_WIDTH = 64

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

def fnr(x):
    score = {1: -3,
             2: -2,
             3: -1,
             4: -6,
             5: -5,
             6: -4,
             7: -9,
             8: -8,
             9: -7}
    return score[x]

def fnm(y):
    return y % 10

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

    return data[x+1][y+1]

def get_m(x, y):
    data = [[24, 25,36,0],
            [14,15,36,0],
            [15,35,36,47],
            [36,58,59,0],
            [15,35,36,0],
            [24,25,26,0],
            [26,57,58,0],
            [26,35,0,0],
            [47,48,0,0],
            [35,36,0,0],
            [35,36,0,0],
            [36,0,0,0],
            [47,58,0,0],
            [15,0,0,0],
            [26,47,0,0],
            [47,58,0,0],
            [35,36,47,0],
            [28,58,0,0],
            [15,47,0,0]]

    return data[x+1][y+1]

def init_board():
    return [-1] * 3 + [0] * 3 + [1] * 3

def print_board(board):
    pieces = "X.O"

    space = " "*10
    print()
    for i in range(3):
        line = ""
        for j in range(3):
            line += space
            space_number = i * 3 + j
            space_contents = board[space_number]
            line += pieces[space_contents + 1]
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
            print("ILLEGAL MOVE.")

def print_illegal():
    print("ILLEGAL MOVE.")

def board_contents(board, space_number):
    return board[space_number - 1]

def set_board(board, space_number, new_value):
    board[space_number - 1] = new_value

def main():
    print_header("HEXAPAWN")
    if prompt_yes_no("INSTRUCTIONS (Y-N)?"):
        print_instructions()

    w = 0
    l = 0

    x = 0
    y = 0

    board = init_board()

    print_board(board)

    while True:
        m1, m2 = get_coordinates()

        if board_contents(board, m1) != 1:
            # Start space doesn't contain player's piece
            print_illegal()
            continue
        if board_contents(board, m2) == 1:
            # Destination space contains player's piece (can't capture your own piece)
            print_illegal()
            continue
        # line 160
        is_capture = (m2-m1 != -3)
        if is_capture and board_contents(board, m2) != -1:
            # Destination does not contain computer piece
            print_illegal()
            continue
        # line 170
        if m2 > m1:
            # can't move backwards
            print_illegal()
            continue
        # line 180
        if (not is_capture) and board_contents(board, m2) != 0:
            # Destination is not open
            print_illegal()
            continue
        # line 185
        if m2-m1 < -4:
            # too far
            print_illegal()
            continue
        # line 186
        if m1 == 7 and m2 == 3:
            # can't jump corner to corner ?!
            print_illegal()
            continue

        # otherwise, acceptable move
        break
        
    set_board(board, m1, 0)
    set_board(board, m2, 1)

    # line 205
    print_board(board)
    

if __name__ == "__main__":
    main()

"""
210 IF S(1)=1 OR S(2)=1 OR S(3)=1 THEN 820
220 FOR I=1 TO 9
221 IF S(I)=-1 THEN 230
222 NEXT I
223 GOTO 820
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
