class TicTacToe:
    def __init__(self,pick,sz=3):
        self.pick = pick
        self.dim_sz = sz
        self.board = self.ClearBoard()

    def ClearBoard(self):
        board = [['blur' for i in range(self.dim_sz)] for j in range(self.dim_sz)]
        # made a 3x3 by-default board
        return board
    
    def MoveRecord(self,r,c):
        if r > self.dim_sz or c > self.dim_sz:
            return "Out of Bounds"
        if self.board[r][c] != 'blur':
            return "Spot Pre-Occupied"
        self.board[r][c] = self.pick
        return True 
    
    def CheckWin(self):# 1 you won, 0 computer won, -1 tie

        #Flag syntax -> first player no. , User is Player#1 ; Check set 1 -> row and '\' diagonal & Check set 2 -> col and '/' diagonal


        for i in range(0,self.dim_sz):#Rows
            flag11 = True
            flag21 = True

            flag12 = True
            flag22 = True 
            for j in range(0,self.dim_sz):

                ch2 = self.board[i][j]
                ch1 = self.board[j][i]
                                       #Row
                if ch1 == self.pick:# if it's mine, computer didn't make it
                    flag21 = False
                elif ch1 == 'blur':#if it's blank no one made it
                    flag11 = False
                    flag21 = False
                else: flag11 = False# else i didn't make it 

                if ch2 == self.pick:#Same but for Col
                    flag22 = False
                elif ch2 == 'blur':
                    flag12 = False
                    flag22 = False
                else: flag12 = False

            if flag11 is True or flag12 is True:# I won
                return 1
            if flag21 is True or flag22 is True:#Computer Won
                return 0

        #Diagonals#
        flag11 = True
        flag21 = True

        flag12 = True
        flag22 = True 
        for i in range(0,self.dim_sz):

            ch2 = self.board[i][i]
            ch1 = self.board[i][self.dim_sz-1-i]

            if ch1 == self.pick:
                flag21 = False
            elif ch1 == 'blur':
                flag11 = False
                flag21 = False
            else:flag11 = False

            if ch2 == self.pick:
                flag22 = False
            elif ch2 == 'blur':
                flag12 = False
                flag22 = False
            else:flag12 = False

        if flag11 or flag12:
            return 1
        if flag21 or flag22:
            return 0

        return -1
    

    def NextMove(self):
        AvailableMoves = []# will carry all available moves
        PlayerWinSpot = []#if player (user Wins)
        CompPick = 'O'
        if self.pick == 'O':
            CompPick = 'X'
        for i in range(0,self.dim_sz):
            for j in range(0,self.dim_sz):

                if self.board[i][j] == 'blur':#BLANK
                    t = (i,j)
                    AvailableMoves.append(t)#add it to available moves
                    self.board[i][j] = CompPick#Check if I (Computer can win)
                    if self.CheckWin() ==0:#Best Case I(Computer) win!
                        return i,j;
                    self.board[i][j] = self.pick
                    if self.CheckWin() == 1: #Second Best Case, he (player) didn't won
                        PlayerWinSpot.append(t)
                    self.board[i][j] = 'blur'

        if len(PlayerWinSpot) != 0:
            self.board[PlayerWinSpot[0][0]] [PlayerWinSpot[0][1]] = CompPick
            return PlayerWinSpot[0][0],PlayerWinSpot[0][1]
        #print(AvailableMoves)
        if len(AvailableMoves) == 1:
            self.board[ AvailableMoves[0][0] ][ AvailableMoves[0][1] ] = CompPick
            return [ AvailableMoves[0][0] ],[ AvailableMoves[0][1] ]
        if len(AvailableMoves) == 0:
            return -1,-1

        c1 , c2  = self.dim_sz//2,self.dim_sz//2
        #print(c1,c2,self.dim_sz)
        if (c1,c2) in AvailableMoves:#CENTER 
            self.board[c1][c2] = CompPick
            return c1,c2
        for i in range(c1-1,-1,-1):#IN TO OUT
            gap = c1 - i
            #checking  - 4 possibilities at max
                                                     #EDGES 
            if  (c1-gap,c2-gap) in AvailableMoves:
                self.board[c1-gap][c2-gap] = CompPick
                return c1-gap,c2-gap
            if  (c1-gap,c2+gap) in AvailableMoves:
                self.board[c1-gap][c2+gap] = CompPick
                return c1-gap,c2+gap
            if  (c1+gap,c2-gap) in AvailableMoves:
                self.board[c1+gap][c2-gap] = CompPick
                return c1+gap,c2-gap
            if  (c1+gap,c2+gap) in AvailableMoves:
                self.board[c1+gap][c2+gap] = CompPick
                return c1+gap,c2+gap

            #Four Lines

            for i in range(0,gap):
                if  (c1-gap,c2-gap+i) in AvailableMoves:#TOP LEFT TO TOP RIGHT
                    self.board[c1-gap][c2-gap+i] = CompPick
                    return c1-gap,c2-gap+i
                if  (c1+gap,c2-gap+i) in AvailableMoves:#BOTTOM LEFT TO BOTTOM RIGHT
                    self.board[c1+gap][c2-gap+i] = CompPick
                    return c1+gap,c2-gap+i
                if  (c1-gap,c2-gap) in AvailableMoves:#LEFT TOP TO LEFT BOTTOM
                    self.board[c1-gap+i][c2-gap] = CompPick
                    return c1-gap+i,c2-gap
                if  (c1-gap+i,c2+gap) in AvailableMoves:#RIGHT TOP TO RIGHT BOTTOM
                    self.board[c1-gap+i][c2+gap] = CompPick
                    return c1-gap+i,c2+gap

def Display(Game : TicTacToe):
    line1 = ""
    for i in range(0,Game.dim_sz):
        for j in range(0,Game.dim_sz-1):
            if Game.board[i][j] == 'blur':line1 = line1 + "    |"
            else:line1 = line1 + "  " + Game.board[i][j] + " |"
        if Game.board[i][Game.dim_sz-1] == 'blur':line1 = line1 + "    \n"
        else:line1 = line1 + "  " + Game.board[i][Game.dim_sz-1] + " \n"   
        #line1 = line1 + " " + Game.board[i][Game.dim_sz-1] + "\n"
    print(line1,"\n\n")


def play():
    Pick = input("Pick 'X' or 'O' ").strip().upper()
    if Pick == 'O':
        Game = TicTacToe("O")
    else: Game = TicTacToe("X")
    Display(Game=Game)
    while True:
        #Display(Game)
        temp = False
        while temp != True:
            move = list(map(int,input("Make A Move in Grid System from (0,0) to (2,2) ").split()) )
            temp = Game.MoveRecord(move[0],move[1])
            if temp != True:
                print(temp)

        if Game.CheckWin() == 1:
            print("You Won!")
            break;
        print("Your Move:- ")
        Display(Game)
        C1,C2 = Game.NextMove()
        if C1 == -1 and C2 == -1:
            print("Game Tie!")
            break;
        if Game.CheckWin() == 0:
            print("You lost!")
            break;
        print("Computer's Move :-")
        Display(Game)


if __name__ == "__main__":
    play()  
