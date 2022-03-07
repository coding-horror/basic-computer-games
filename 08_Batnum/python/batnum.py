from enum import Enum


class WinOptions(Enum):
    Undefined = 0
    TakeLast = 1
    AvoidLast = 2


class StartOptions(Enum):
    Undefined = 0
    ComputerFirst = 1
    PlayerFirst = 2


def PrintIntro():
    """Prints out the introduction and rules for the game."""
    print("BATNUM".rjust(33, " "))
    print("CREATIVE COMPUTING  MORRISSTOWN, NEW JERSEY".rjust(15, " "))
    print()
    print()
    print()
    print("THIS PROGRAM IS A 'BATTLE OF NUMBERS' GAME, WHERE THE")
    print("COMPUTER IS YOUR OPPONENT.")
    print()
    print("THE GAME STARTS WITH AN ASSUMED PILE OF OBJECTS. YOU")
    print("AND YOUR OPPONENT ALTERNATELY REMOVE OBJECTS FROM THE PILE.")
    print("WINNING IS DEFINED IN ADVANCE AS TAKING THE LAST OBJECT OR")
    print("NOT. YOU CAN ALSO SPECIFY SOME OTHER BEGINNING CONDITIONS.")
    print("DON'T USE ZERO, HOWEVER, IN PLAYING THE GAME.")
    print("ENTER A NEGATIVE NUMBER FOR NEW PILE SIZE TO STOP PLAYING.")
    print()
    return


def GetParams():
    """This requests the necessary parameters to play the game.

    Returns a set with the five game parameters:
        pileSize - the starting size of the object pile
        minSelect - minimum selection that can be made on each turn
        maxSelect - maximum selection that can be made on each turn
        startOption - 1 if the computer is first
                      or 2 if the player is first
        winOption - 1 if the goal is to take the last object
                    or 2 if the goal is to not take the last object
    """
    pileSize = 0
    winOption = WinOptions.Undefined
    minSelect = 0
    maxSelect = 0
    startOption = StartOptions.Undefined

    while pileSize < 1:
        pileSize = int(input("ENTER PILE SIZE "))
    while winOption == WinOptions.Undefined:
        winOption = int(input("ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST: "))
    while minSelect < 1 or maxSelect < 1 or minSelect > maxSelect:
        (minSelect, maxSelect) = (
            int(x) for x in input("ENTER MIN AND MAX ").split(" ")
        )
    while startOption == StartOptions.Undefined:
        startOption = int(input("ENTER START OPTION - 1 COMPUTER FIRST, 2 YOU FIRST "))
    return (pileSize, minSelect, maxSelect, startOption, winOption)


def PlayerMove(pileSize, minSelect, maxSelect, startOption, winOption):
    """This handles the player's turn - asking the player how many objects
    to take and doing some basic validation around that input.  Then it
    checks for any win conditions.

    Returns a boolean indicating whether the game is over and the new pileSize."""
    playerDone = False
    while not playerDone:
        playerMove = int(input("YOUR MOVE "))
        if playerMove == 0:
            print("I TOLD YOU NOT TO USE ZERO!  COMPUTER WINS BY FORFEIT.")
            return (True, pileSize)
        if playerMove > maxSelect or playerMove < minSelect:
            print("ILLEGAL MOVE, REENTER IT")
            continue
        pileSize = pileSize - playerMove
        playerDone = True
        if pileSize <= 0:
            if winOption == WinOptions.AvoidLast:
                print("TOUGH LUCK, YOU LOSE.")
            else:
                print("CONGRATULATIONS, YOU WIN.")
            return (True, pileSize)
    return (False, pileSize)


def ComputerPick(pileSize, minSelect, maxSelect, startOption, winOption):
    """This handles the logic to determine how many objects the computer
    will select on its turn.
    """
    q = pileSize - 1 if winOption == WinOptions.AvoidLast else pileSize
    c = minSelect + maxSelect
    computerPick = q - (c * int(q / c))
    if computerPick < minSelect:
        computerPick = minSelect
    if computerPick > maxSelect:
        computerPick = maxSelect
    return computerPick


def ComputerMove(pileSize, minSelect, maxSelect, startOption, winOption):
    """This handles the computer's turn - first checking for the various
    win/lose conditions and then calculating how many objects
    the computer will take.

    Returns a boolean indicating whether the game is over and the new pileSize."""
    # First, check for win conditions on this move
    # In this case, we win by taking the last object and
    # the remaining pile is less than max select
    # so the computer can grab them all and win
    if winOption == WinOptions.TakeLast and pileSize <= maxSelect:
        print(f"COMPUTER TAKES {pileSize} AND WINS.")
        return (True, pileSize)
    # In this case, we lose by taking the last object and
    # the remaining pile is less than minsize and the computer
    # has to take all of them.
    if winOption == WinOptions.AvoidLast and pileSize <= minSelect:
        print(f"COMPUTER TAKES {minSelect} AND LOSES.")
        return (True, pileSize)

    # Otherwise, we determine how many the computer selects
    currSel = ComputerPick(pileSize, minSelect, maxSelect, startOption, winOption)
    pileSize = pileSize - currSel
    print(f"COMPUTER TAKES {currSel} AND LEAVES {pileSize}")
    return (False, pileSize)


def PlayGame(pileSize, minSelect, maxSelect, startOption, winOption):
    """This is the main game loop - repeating each turn until one
    of the win/lose conditions is met.
    """
    gameOver = False
    # playersTurn is a boolean keeping track of whether it's the
    # player's or computer's turn
    playersTurn = startOption == StartOptions.PlayerFirst

    while not gameOver:
        if playersTurn:
            (gameOver, pileSize) = PlayerMove(
                pileSize, minSelect, maxSelect, startOption, winOption
            )
            playersTurn = False
            if gameOver:
                return
        if not playersTurn:
            (gameOver, pileSize) = ComputerMove(
                pileSize, minSelect, maxSelect, startOption, winOption
            )
            playersTurn = True

    return


if __name__ == "__main__":

    pileSize = 0
    minSelect = 0
    maxSelect = 0
    # 1 = to take last, 2 = to avoid last
    winOption = 0
    # 1 = computer first, 2 = user first
    startOption = 0

    while True:
        PrintIntro()
        (pileSize, minSelect, maxSelect, startOption, winOption) = GetParams()
        # Just keep playing the game until the user kills it with ctrl-C
        PlayGame(pileSize, minSelect, maxSelect, startOption, winOption)
