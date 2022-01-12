#!/usr/bin/python3

# Ported from the BASIC source for 3D Tic Tac Toe
# in BASIC Computer Games, by David H. Ahl
# The code originated from Dartmouth College

from enum import Enum


class Move(Enum):
    """ Game status and types of machine move """
    HUMAN_WIN = 0
    MACHINE_WIN = 1
    DRAW = 2
    MOVES = 3
    LIKES = 4
    TAKES = 5
    GET_OUT = 6
    YOU_FOX = 7
    NICE_TRY = 8
    CONCEDES = 9

class Player(Enum):
    EMPTY = 0
    HUMAN = 1
    MACHINE = 2

class TicTacToe3D:
    """ The game logic for 3D Tic Tac Toe and the machine opponent """

    def __init__(self):
        
        # 4x4x4 board keeps track of which player occupies each place
        # and used by machine to work out its strategy
        self.board = [0] * 64

        # starting move
        self.corners = [0, 48, 51, 3, 12, 60, 63, 15,
                        21, 38, 22, 37, 25, 41, 26, 42]

        # lines to check for end game
        self.lines = [[0, 1, 2, 3],
                      [4, 5, 6, 7],
                      [8, 9, 10, 11],
                      [12, 13, 14, 15],
                      [16, 17, 18, 19],
                      [20, 21, 22, 23],
                      [24, 25, 26, 27],
                      [28, 29, 30, 31],
                      [32, 33, 34, 35],
                      [36, 37, 38, 39],
                      [40, 41, 42, 43],
                      [44, 45, 46, 47],
                      [48, 49, 50, 51],
                      [52, 53, 54, 55],
                      [56, 57, 58, 59],
                      [60, 61, 62, 63],
                      [0, 16, 32, 48],
                      [4, 20, 36, 52],
                      [8, 24, 40, 56],
                      [12, 28, 44, 60],
                      [1, 17, 33, 49],
                      [5, 21, 37, 53],
                      [9, 25, 41, 57],
                      [13, 29, 45, 61],
                      [2, 18, 34, 50],
                      [6, 22, 38, 54],
                      [10, 26, 42, 58],
                      [14, 30, 46, 62],
                      [3, 19, 35, 51],
                      [7, 23, 39, 55],
                      [11, 27, 43, 59],
                      [15, 31, 47, 63],
                      [0, 4, 8, 12],
                      [16, 20, 24, 28],
                      [32, 36, 40, 44],
                      [48, 52, 56, 60],
                      [1, 5, 9, 13],
                      [17, 21, 25, 29],
                      [33, 37, 41, 45],
                      [49, 53, 57, 61],
                      [2, 6, 10, 14],
                      [18, 22, 26, 30],
                      [34, 38, 42, 46],
                      [50, 54, 58, 62],
                      [3, 7, 11, 15],
                      [19, 23, 27, 31],
                      [35, 39, 43, 47],
                      [51, 55, 59, 63],
                      [0, 5, 10, 15],
                      [16, 21, 26, 31],
                      [32, 37, 42, 47],
                      [48, 53, 58, 63],
                      [12, 9, 6, 3],
                      [28, 25, 22, 19],
                      [44, 41, 38, 35],
                      [60, 57, 54, 51],
                      [0, 20, 40, 60],
                      [1, 21, 41, 61],
                      [2, 22, 42, 62],
                      [3, 23, 43, 63],
                      [48, 36, 24, 12],
                      [49, 37, 25, 13],
                      [50, 38, 26, 14],
                      [51, 39, 27, 15],
                      [0, 17, 34, 51],
                      [4, 21, 38, 55],
                      [8, 25, 42, 59],
                      [12, 29, 46, 63],
                      [48, 33, 18, 3],
                      [52, 37, 22, 7],
                      [56, 41, 26, 11],
                      [60, 45, 30, 15],
                      [0, 21, 42, 63],
                      [15, 26, 37, 48],
                      [3, 22, 41, 60],
                      [12, 25, 38, 51]]

    def get(self, x, y, z):
        m = self.board[4*(4*z + y) + x]
        if m == 40:
            return Player.MACHINE
        elif m == 8:
            return Player.HUMAN
        else:
            return Player.EMPTY

    def move3D(self, x, y, z, player):
        m = 4*(4*z + y) + x
        return self.move(m, player)

    def move(self, m, player):
        if self.board[m] > 1:
            return False
        
        if player == Player.MACHINE:
            self.board[m] = 40
        else:
            self.board[m] = 8
        return True

    def get3DPosition(self, m):
        x = m % 4
        y = (m // 4) % 4
        z = m // 16
        return x, y, z

    def evaluateLines(self):
        self.lineValues = [0] * 76
        for j in range(76):
            value = 0
            for k in range(4):
                value += self.board[self.lines[j][k]]
            self.lineValues[j] = value

    def strategyMarkLine(self, i):
        for j in range(4):
            m = self.lines[i][j]
            if self.board[m] == 0:
                self.board[m] = 1

    def clearStrategyMarks(self):
        for i in range(64):
            if self.board[i] == 1:
                self.board[i] = 0

    def markAndMove(self, vlow, vhigh, vmove):
        """ 
        mark lines that can potentially win the game for the human
        or the machine and choose best place to play
        """
        for i in range(76):
            value = 0
            for j in range(4):
                value += self.board[self.lines[i][j]]
            self.lineValues[i] = value
            if vlow <= value < vhigh:
                if value > vlow:
                    return self.moveTriple(i)
                self.strategyMarkLine(i)
        self.evaluateLines()

        for i in range(76):
            value = self.lineValues[i]
            if value == 4 or value == vmove:
                return self.moveDiagonals(i, 1)
        return None

    def machineMove(self):
        """ machine works out what move to play """
        self.clearStrategyMarks()

        self.evaluateLines()
        for value, event in [(32, self.humanWin),
                             (120, self.machineWin),
                             (24, self.blockHumanWin)]:
            for i in range(76):
                if self.lineValues[i] == value:
                    return event(i)

        m = self.markAndMove(80, 88, 43)
        if m != None:
            return m

        self.clearStrategyMarks()

        m = self.markAndMove(16, 24, 11)
        if m != None:
            return m

        for k in range(18):
            value = 0
            for i in range(4*k, 4*k+4):
                for j in range(4):
                    value += self.board[self.lines[i][j]]
            if (32 <= value < 40) or (72 <= value < 80):
                for s in [1, 0]:
                    for i in range(4*k, 4*k+4):
                        m = self.moveDiagonals(i, s)
                        if m != None:
                            return m

        self.clearStrategyMarks()

        for y in self.corners:
            if self.board[y] == 0:
                return (Move.MOVES, y)

        for i in range(64):
            if self.board[i] == 0:
                return (Move.LIKES, i)

        return (Move.DRAW, -1)
            
    def humanWin(self, i):
        return (Move.HUMAN_WIN, -1, i)

    def machineWin(self, i):
        for j in range(4):
            m = self.lines[i][j]
            if self.board[m] == 0:
                return (Move.MACHINE_WIN, m, i)
        return None

    def blockHumanWin(self, i):
        for j in range(4):
            m = self.lines[i][j]
            if self.board[m] == 0:
                return (Move.NICE_TRY, m)
        return None


    def moveTriple(self, i):
        """ make two lines-of-3 or prevent human from doing this """
        for j in range(4):
            m = self.lines[i][j]
            if self.board[m] == 1:
                if self.lineValues[i] < 40:
                    return (Move.YOU_FOX, m)
                else:
                    return (Move.GET_OUT, m)
        return (Move.CONCEDES, -1)

    # choose move in corners or center boxes of square 4x4
    def moveDiagonals(self, i, s):
        if 0 < (i % 4) < 3:
            jrange = [1, 2]
        else:
            jrange = [0, 3]
        for j in jrange:
            m = self.lines[i][j]
            if self.board[m] == s:
                return (Move.TAKES, m)
        return None

class Qubit:
    
    def moveCode(self, board, m):
        x, y, z = board.get3DPosition(m)
        return "{:d}{:d}{:d}".format(z+1, y+1, x+1)
        
    def showWin(self, board, i):
        for m in board.lines[i]:
            print(self.moveCode(board, m))

    def showBoard(self, board):
        c = " YM"
        for z in range(4):
            for y in range(4):
                print("   " * y, end="")
                for x in range(4):
                    p = board.get(x, y, z)
                    print("({})      ".format(c[p.value]), end="")
                print("\n")
            print("\n")
        
    def humanMove(self, board):
        print("")
        c = "1234"
        while True:
            h = input("Your move?\n")
            if h == "1":
                return False
            if h == "0":
                self.showBoard(board)
                continue
            if (len(h) == 3) and (h[0] in c) and (h[1] in c) and (h[2] in c):
                x = c.find(h[2])
                y = c.find(h[1])
                z = c.find(h[0])
                if board.move3D(x, y, z, Player.HUMAN):
                    break
                
                print("That square is used. Try again.")
            else:
                print("Incorrect move. Retype it--")

        return True
        
    def play(self):
        print("Qubic\n")
        print("Create Computing Morristown, New Jersey\n\n\n")
        while True:
            c = input("Do you want instructions?\n")
            if len(c) >= 1 and (c[0] in "ynYN"):
                break
            print("Incorrect answer. Please type 'yes' or 'no.")
            
        c = c.lower()
        if c[0] == 'y':
            print("The game is Tic-Tac-Toe in a 4 x 4 x 4 cube.")
            print("Each move is indicated by a 3 digit number, with each")
            print("digit between 1 and 4 inclusive.  The digits indicate the")
            print("level, row, and column, respectively, of the occupied")
            print("place.\n")

            print("To print the playing board, type 0 (zero) as your move.")
            print("The program will print the board with your moves indicated")
            print("with a (Y), the machine's moves with an (M), and")
            print("unused squares with a ( ).\n")

            print("To stop the program run, type 1 as your move.\n\n")

        
            
        play_again = True
        while play_again:
            board = TicTacToe3D()

            while True:
                s = input("Do you want to move first?\n")
                if len(s) >= 1 and (s[0] in "ynYN"):
                    break
                print("Incorrect answer. Please type 'yes' or 'no'.")

            skipHuman = s[0] in "nN"

            move_text = ["Machine moves to",
                         "Machine likes",
                         "Machine takes",
                         "Let's see you get out of this:  Machine moves to",
                         "You fox.  Just in the nick of time, machine moves to",
                         "Nice try. Machine moves to"]

            while True:
                if not skipHuman:
                    if not self.humanMove(board):
                        break
                skipHuman = False

                m = board.machineMove()
                if m[0] == Move.HUMAN_WIN:
                    print("You win as follows,")
                    self.showWin(board, m[2])
                    break
                elif m[0] == Move.MACHINE_WIN:
                    print("Machine moves to {}, and wins as follows".format(self.moveCode(board, m[1])))
                    self.showWin(board, m[2])
                    break
                elif m[0] == Move.DRAW:
                    print("The game is a draw.")
                    break
                elif m[0] == Move.CONCEDES:
                    print("Machine concedes this game.")
                    break
                else:
                    print(move_text[m[0].value - Move.MOVES.value])
                    print(self.moveCode(board, m[1]))
                    board.move(m[1], Player.MACHINE)

                self.showBoard(board)

            print(" ")
            while True:
                x = input("Do you want to try another game\n")
                if len(x) >= 1 and x[0] in "ynYN":
                    break
                print("Incorrect answer. Please Type 'yes' or 'no'.")

            play_again = x[0] in "yY"
        
        

if __name__ == "__main__":
    game = Qubit()
    game.play()
