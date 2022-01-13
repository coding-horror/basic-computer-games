
def new_board():
    # Using a dictionary in python to store the board, since we are not including all numbers within a given range.
    board = {}
    for i in [13, 14, 15, 22, 23, 24, 29, 30, 31, 32, 33, 34, 35, 38, 39, 40, 42, 43, 44, 47, 48, 49, 50, 51, 52, 53, 58, 59, 60, 67, 68, 69]:
        board[i] = '!'
    board[41] = 'O'
    return board


def print_instructions():
    print("""
HERE IS THE BOARD:

          !    !    !
         13   14   15

          !    !    !
         22   23   24

!    !    !    !    !    !    !
29   30   31   32   33   34   35

!    !    !    !    !    !    !
38   39   40   41   42   43   44

!    !    !    !    !    !    !
47   48   49   50   51   52   53

          !    !    !
         58   59   60

          !    !    !
         67   68   69

TO SAVE TYPING TIME, A COMPRESSED VERSION OF THE GAME BOARD
WILL BE USED DURING PLAY.  REFER TO THE ABOVE ONE FOR PEG
NUMBERS.  OK, LET'S BEGIN.
    """)

def print_board(board):
    """Prints the boards using indexes in the passed parameter"""
    print(" " * 2 + board[13] + board[14] + board[15])
    print(" " * 2 + board[22] + board[23] + board[24])
    print(board[29] + board[30] + board[31] + board[32] + board[33] + board[34] + board[35])
    print(board[38] + board[39] + board[40] + board[41] + board[42] + board[43] + board[44])
    print(board[47] + board[48] + board[49] + board[50] + board[51] + board[52] + board[53])
    print(" " * 2 + board[58] + board[59] + board[60])
    print(" " * 2 + board[67] + board[68] + board[69])

def play_game():
    # Create new board
    board = new_board()

    # Main game loop
    while not is_game_finished(board):
        print_board(board)
        while not move(board):
            print("ILLEGAL MOVE! TRY AGAIN")
    
    # Check peg count and print the user's score
    peg_count = 0
    for key in board.keys():
        if board[key] == '!':
            peg_count += 1
    
    print("YOU HAD " + str(peg_count) + " PEGS REMAINING")
    
    if peg_count == 1:
        print("BRAVO! YOU MADE A PERFECT SCORE!")
        print("SAVE THIS PAPER AS A RECORD OF YOUR ACCOMPLISHMENT!")
        
def move(board):
    """Queries the user to move. Returns false if the user puts in an invalid input or move, returns true if the move was successful"""
    try:
        # Ask for the "start" location
        start = int(input("MOVE WHICH PIECE? "))
        # Verify that the location has a peg
        if not (board[start] == '!'):
            return False
        
        # Ask for the "end" location
        end = int(input("TO WHERE? "))

        # difference and center
        difference = abs(end - start)
        center = (end + start) / 2
        
        # Execute the move if the difference is correct, there is a peg in the center and no peg at the end
        if (difference == 2 or difference == 18) and board[end] == 'O' and board[center] == '!':
            board[start] = 'O'
            board[center] = 'O'
            board[end] == '!'
            return True
        else:
            return False
    except:
        return False

def main():
    print(" " * 33 + "H-I-Q")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print_instructions()
    play_game()

def is_game_finished(board):
    # Checks all locations and whether or not a move is possible at that location.
    for pos in board.keys():
        if board[pos] == 'X':
            for space in [1,9]:
                # Checks if the next location has a peg
                nextToPeg = ((pos + space) in board) and board[pos + space] == '!'
                # Checks both going forward (+ location) or backwards (-location)
                hasMovableSpace = (not ((pos - space) in board and board[pos - space] == '!')) or (not ((pos + space * 2) in board and board[pos + space * 2] == '!'))
                if nextToPeg and hasMovableSpace:
                    return False
    return True


if __name__ == "__main__":
    main()
