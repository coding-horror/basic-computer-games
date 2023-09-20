print " "*33 + "Gomoku"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "Welcome to the Oriental game of Gomoko."
print; print "The game is played on an N by N grid of a size"
print "that you specify  During your play, you may cover one grid"
print "intersection with a marker. The object of the game is to get"
print "5 adjacent markers in a row -- horizontally, vertically, or"
print "diagonally.  On the board diagram, your moves are marked"
print "with a '1' and the computer moves with a '2'."
print; print "The computer does not keep track of who has won."
print "To end the game, type -1,-1 for your move."; print

inBounds = function(x,y)
    return 0 < x <= n and 0 < y <= n
end function

empty = function(x,y)
    return A[x][y] == 0
end function

printBoard = function
    for y in range(1,n)
        print A[y][1:n+1].join
    end for
    print
end function

doPlayerMove = function
    while true
        inp = input("Your play (i,j)? ").replace(" ", "").split(",")
        print
        if inp.len != 2 then continue
        x = inp[0].val; y = inp[1].val
        if x == -1 then return false
        if not inBounds(x,y) then
            print "Illegal move.  Try again..."
        else if not empty(x,y) then
            print "Square occupied.  Try again..."
        else
            break
        end if
    end while
    A[x][y] = 1
    globals.lastPlayerMove = [x,y]
    return true
end function

doComputerMove = function
	// Computer tries a move near the player's last move
	for e in range(-1,1)
		for f in range(-1,1)
			if e==0 and f==0 then continue
			x = lastPlayerMove[0] + e; y = lastPlayerMove[1] + f
			if inBounds(x,y) and empty(x,y) then
				A[x][y] = 2
				return
			end if
		end for
	end for
	
	// Computer tries a random move
	while true
		x = floor(n * rnd + 1); y = floor(n * rnd + 1)
		if empty(x,y) then break
	end while
	A[x][y] = 2
end function

playGame = function
    while true
        globals.n = input("What is your board size (min 7/ max 19)? ").val
        if 7 <= n <= 19 then break
        print "I said, the minimum is 7, the maximum is 19."
    end while
    globals.A = []
    for i in range(0,19)
        A.push [0]*20
    end for
    print; print "We alternate moves.  You go first..."; print
    while true
        if not doPlayerMove then return
        doComputerMove
        printBoard
    end while
end function

// Main loop
while true
    playGame
    print; print "Thanks for the game!!"
    q = input("Play again (1 for Yes, 0 for No)? ").val
    if q != 1 then break
end while