from typing import List, Tuple, Union


class TicTacToe:
    def __init__(self, pick, sz=3) -> None:
        self.pick = pick
        self.dim_sz = sz
        self.board = self.clear_board()

    def clear_board(self) -> List[List[str]]:
        board = [["blur" for i in range(self.dim_sz)] for j in range(self.dim_sz)]
        # made a 3x3 by-default board
        return board

    def move_record(self, r, c) -> Union[str, bool]:
        if r > self.dim_sz or c > self.dim_sz:
            return "Out of Bounds"
        if self.board[r][c] != "blur":
            return "Spot Pre-Occupied"
        self.board[r][c] = self.pick
        return True

    def check_win(self) -> int:  # 1 you won, 0 computer won, -1 tie
        # Flag syntax -> first player no. ,
        # User is Player#1 ;
        # Check set 1 -> row and '\' diagonal & Check set 2 -> col and '/' diagonal

        for i in range(0, self.dim_sz):  # Rows
            flag11 = True
            flag21 = True

            flag12 = True
            flag22 = True
            for j in range(0, self.dim_sz):

                ch2 = self.board[i][j]
                ch1 = self.board[j][i]
                # Row
                if ch1 == self.pick:  # if it's mine, computer didn't make it
                    flag21 = False
                elif ch1 == "blur":  # if it's blank no one made it
                    flag11 = False
                    flag21 = False
                else:
                    flag11 = False  # else i didn't make it

                if ch2 == self.pick:  # Same but for Col
                    flag22 = False
                elif ch2 == "blur":
                    flag12 = False
                    flag22 = False
                else:
                    flag12 = False

            if flag11 is True or flag12 is True:  # I won
                return 1
            if flag21 is True or flag22 is True:  # Computer Won
                return 0

        # Diagonals#
        flag11 = True
        flag21 = True

        flag12 = True
        flag22 = True
        for i in range(0, self.dim_sz):

            ch2 = self.board[i][i]
            ch1 = self.board[i][self.dim_sz - 1 - i]

            if ch1 == self.pick:
                flag21 = False
            elif ch1 == "blur":
                flag11 = False
                flag21 = False
            else:
                flag11 = False

            if ch2 == self.pick:
                flag22 = False
            elif ch2 == "blur":
                flag12 = False
                flag22 = False
            else:
                flag12 = False

        if flag11 or flag12:
            return 1
        if flag21 or flag22:
            return 0

        return -1

    def next_move(self) -> Union[Tuple[int, int], Tuple[List[int], List[int]]]:
        available_moves = []  # will carry all available moves
        player_win_spot = []  # if player (user Wins)
        comp_pick = "O"
        if self.pick == "O":
            comp_pick = "X"
        for i in range(0, self.dim_sz):
            for j in range(0, self.dim_sz):

                if self.board[i][j] == "blur":  # BLANK
                    t = (i, j)
                    available_moves.append(t)  # add it to available moves
                    self.board[i][j] = comp_pick  # Check if I (Computer can win)
                    if self.check_win() == 0:  # Best Case I(Computer) win!
                        return i, j
                    self.board[i][j] = self.pick
                    if (
                        self.check_win() == 1
                    ):  # Second Best Case, he (player) didn't won
                        player_win_spot.append(t)
                    self.board[i][j] = "blur"

        if len(player_win_spot) != 0:
            self.board[player_win_spot[0][0]][player_win_spot[0][1]] = comp_pick
            return player_win_spot[0][0], player_win_spot[0][1]
        if len(available_moves) == 1:
            self.board[available_moves[0][0]][available_moves[0][1]] = comp_pick
            return [available_moves[0][0]], [available_moves[0][1]]
        if len(available_moves) == 0:
            return -1, -1

        c1, c2 = self.dim_sz // 2, self.dim_sz // 2
        if (c1, c2) in available_moves:  # CENTER
            self.board[c1][c2] = comp_pick
            return c1, c2
        for i in range(c1 - 1, -1, -1):  # IN TO OUT
            gap = c1 - i
            # checking  - 4 possibilities at max
            # EDGES
            if (c1 - gap, c2 - gap) in available_moves:
                self.board[c1 - gap][c2 - gap] = comp_pick
                return c1 - gap, c2 - gap
            if (c1 - gap, c2 + gap) in available_moves:
                self.board[c1 - gap][c2 + gap] = comp_pick
                return c1 - gap, c2 + gap
            if (c1 + gap, c2 - gap) in available_moves:
                self.board[c1 + gap][c2 - gap] = comp_pick
                return c1 + gap, c2 - gap
            if (c1 + gap, c2 + gap) in available_moves:
                self.board[c1 + gap][c2 + gap] = comp_pick
                return c1 + gap, c2 + gap

            # Four Lines

            for i in range(0, gap):
                if (c1 - gap, c2 - gap + i) in available_moves:  # TOP LEFT TO TOP RIGHT
                    self.board[c1 - gap][c2 - gap + i] = comp_pick
                    return c1 - gap, c2 - gap + i
                if (
                    c1 + gap,
                    c2 - gap + i,
                ) in available_moves:  # BOTTOM LEFT TO BOTTOM RIGHT
                    self.board[c1 + gap][c2 - gap + i] = comp_pick
                    return c1 + gap, c2 - gap + i
                if (c1 - gap, c2 - gap) in available_moves:  # LEFT TOP TO LEFT BOTTOM
                    self.board[c1 - gap + i][c2 - gap] = comp_pick
                    return c1 - gap + i, c2 - gap
                if (
                    c1 - gap + i,
                    c2 + gap,
                ) in available_moves:  # RIGHT TOP TO RIGHT BOTTOM
                    self.board[c1 - gap + i][c2 + gap] = comp_pick
                    return c1 - gap + i, c2 + gap
        raise RuntimeError("No moves available")


def display(game: TicTacToe) -> None:
    line1 = ""
    for i in range(0, game.dim_sz):
        for j in range(0, game.dim_sz - 1):
            if game.board[i][j] == "blur":
                line1 = line1 + "    |"
            else:
                line1 = line1 + "  " + game.board[i][j] + " |"
        if game.board[i][game.dim_sz - 1] == "blur":
            line1 = line1 + "    \n"
        else:
            line1 = line1 + "  " + game.board[i][game.dim_sz - 1] + " \n"
    print(line1, "\n\n")


def main() -> None:
    pick = input("Pick 'X' or 'O' ").strip().upper()
    if pick == "O":
        game = TicTacToe("O")
    else:
        game = TicTacToe("X")
    display(game=game)
    while True:
        temp: Union[bool, str] = False
        while not temp:
            move = list(
                map(
                    int,
                    input("Make A Move in Grid System from (0,0) to (2,2) ").split(),
                )
            )
            temp = game.move_record(move[0], move[1])
            if not temp:
                print(temp)

        if game.check_win() == 1:
            print("You Won!")
            break
        print("Your Move:- ")
        display(game)
        C1, C2 = game.next_move()
        if C1 == -1 and C2 == -1:
            print("Game Tie!")
            break
        if game.check_win() == 0:
            print("You lost!")
            break
        print("Computer's Move :-")
        display(game)


if __name__ == "__main__":
    main()
