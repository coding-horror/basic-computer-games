print " "*33 + "H-I-Q"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "Here is the board:"; print

print "          !    !    !"
print "         13   14   15"; print
print "          !    !    !"
print "         22   23   24"; print
print "!    !    !    !    !    !    !"
print "29   30   31   32   33   34   35"; print
print "!    !    !    !    !    !    !"
print "38   39   40   41   42   43   44"; print
print "!    !    !    !    !    !    !"
print "47   48   49   50   51   52   53"; print
print "          !    !    !"
print "         58   59   60"; print
print "          !    !    !"
print "         67   68   69"; print
print "To save typing time, a compressed version of the game board"
print "will be used during play.  Refer to the above one for peg"
input "numbers.  Press Return to begin."
print

// Prepare the board (t): a 9x9 2D array, with 5 for pins,
// -5 for invalid (no hole) positions, and 0 for empty holes.
// Also prepare the pinToPos map, which maps pin numbers to
// [row, column] positions.
setupBoard = function
	globals.t = [[-5]*10]
	globals.pinToPos = {}
	pinNums = [13,14,15,22,23,24,29,30,31,32,33,34,35,38,39,40,41,
	  42,43,44,47,48,49,50,51,52,53,58,59,60,67,68,69]
	for row in range(1,9)
		t.push [-5]*10
		for col in range(1,9)
			if row < 2 or row > 8 or col < 2 or col > 8 or
			  ((row < 4 or row > 6) and (col < 4 or col > 6)) then
				t[row][col] = -5
			else
				t[row][col] = 5
				pinToPos[pinNums.pull] = [row,col]
			end if
		end for
	end for
	t[5][5] = 0
end function

printBoard = function
	for x in range(1,9)
		s = ""
		for y in range(1,9)
			if t[x][y] < 0 then
				s += "  "
			else if t[x][y] == 0 then
				s += " o"
			else
				s += " !"
			end if
		end for
		print s
	end for
end function

isPin = function(pinNum)
	if not pinToPos.hasIndex(pinNum) then return false
	pos = pinToPos[pinNum]
	return t[pos[0]][pos[1]] == 5
end function

isHole = function(pinNum)
	if not pinToPos.hasIndex(pinNum) then return false
	pos = pinToPos[pinNum]
	return t[pos[0]][pos[1]] == 0
end function

isValidJump = function(pinFrom, pinTo)
	if not pinToPos.hasIndex(pinFrom) then return false
	posFrom = pinToPos[pinFrom]
	if not isHole(pinTo) then return false
	posTo = pinToPos[pinTo]
	// check that the Manhattan distance is exactly 2
	dist = abs(posFrom[0] - posTo[0]) + abs(posFrom[1] - posTo[1])
	if dist != 2 then return false
	// and check that the intervening position contains a pin
	if t[(posFrom[0]+posTo[0])/2][(posFrom[1]+posTo[1])/2] != 5 then return false
	return true
end function

// Check if the game is over (player has no legal moves).
// Return true if over, false if there are legal moves yet.
checkGameOver = function
	for row in range(2,8)
		for col in range(2,8)
			fromPin = pinToPos.indexOf([row,col])
			if fromPin == null or not isPin(fromPin) then continue
			for r2 in [row-2, row+2]
				toPin = pinToPos.indexOf([r2,col])
				if toPin == null then continue
				if isValidJump(fromPin, toPin) then return false
			end for
			for c2 in [col-2, col+2]
				toPin = pinToPos.indexOf([row,c2])
				if toPin == null then continue
				if isValidJump(fromPin, toPin) then return false
			end for
		end for
	end for
	return true // no legal moves found, so game over
end function

// Get the user's move, returning [[fromRow,fromCol], [toRow,toCol]].
// (Check legality and return only legal moves.)
getMove = function
	print
	while true
		fromNum = input("Move which piece? ").val
		if isPin(fromNum) then toNum = input("To where? ").val else toNum = 0
		if isHole(toNum) and isValidJump(fromNum, toNum) then break
		print "Illegal move, try again..."
	end while
	return [pinToPos[fromNum], pinToPos[toNum]]	
end function

// Get the user's move, and update the board accordingly.
doOneMove = function
	move = getMove
	fromRow = move[0][0]; fromCol = move[0][1]
	toRow = move[1][0];   toCol = move[1][1]
	t[fromRow][fromCol] = 0
	t[toRow][toCol] = 5
	t[(fromRow+toRow)/2][(fromCol+toCol)/2] = 0
end function

// Main program
while true
	setupBoard
	printBoard
	while true
		doOneMove
		print
		printBoard
		if checkGameOver then break
	end while
	print; print "The game is over."
	pinsLeft = 0
	for a in t
		for b in a
			if b == 5 then pinsLeft += 1
		end for
	end for
	print "You had " + pinsLeft + " pieces remaining."
	if pinsLeft == 1 then
		print "Bravo!  You made a perfect score!"
		print "Save this paper as a record of your accomplishment!"
	end if
	print
	yn = input("Play again (yes or no)? ").lower
	if yn and yn[0] == "n" then break
end while
print; print "So long for now."; print

	