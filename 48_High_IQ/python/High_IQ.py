
def new_board():
    board = {}
    for i in [13, 14, 15, 22, 23, 24, 29, 30, 31, 32, 33, 34, 35, 38, 39, 40, 42, 43, 44, 47, 48, 49, 50, 51, 52, 53, 58, 59, 60, 67, 68]:
        board[i] = "X"
    board[41] = "_"
    return board


def print_instructions():
    print("\n" * 3)
    print("HERE IS THE BOARD:\n")
    print("\n")
    print("          !    !    !\n")
    print("         13   14   15\n")
    print("\n")
    print("          !    !    !\n")
    print("         22   23   24\n")
    print("\n")
    print("!    !    !    !    !    !    !    !    !\n")
    print("29   30   31   32   33   34   35   36   37\n")
    print("\n")
    print("!    !    !    !    !    !    !\n")
    print("38   39   40   41   42   43   44\n")
    print("\n")
    print("!    !    !    !    !    !    !\n")
    print("47   48   49   50   51   52   53\n")
    print("\n")
    print("          !    !    !\n")
    print("         58   59   60\n")
    print("\n")
    print("          !    !    !\n")
    print("         67   68   69\n")
    print("\n")
    print("TO SAVE TYPING TIME, A COMPRESSED VERSION OF THE GAME BOARD\n")
    print("WILL BE USED DURING PLAY.  REFER TO THE ABOVE ONE FOR PEG\n")
    print("NUMBERS.  OK, LET'S BEGIN.\n")

def print_board(board):
    print(" " * 3 + board[13] + board[14] + board[15] + " " * 3)
    print(" " * 3 + board[22] + board[23] + board[24] + " " * 3)
    print(board[29] + board[30] + board[31] + board[32] + board[33] + board[34] + board[35])
    print(board[38] + board[39] + board[40] + board[41] + board[42] + board[43] + board[44])
    print(board[47] + board[48] + board[49] + board[50] + board[51] + board[52] + board[53])
    print(" " * 3 + board[58] + board[59] + board[60] + " " * 3)
    print(" " * 3 + board[67] + board[68] + board[69] + " " * 3)
    
def play_game():
    print("Lets play a game")
    board = new_board()
    print_board(board)

def main():
#     if input("Do you want instrunctions?\n").lower().startswith("y"):
    print("\t" * 33 + "H-I-Q")
    print("\t" * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print_instructions()
    play_game()

if __name__ == "__main__":
    main()
