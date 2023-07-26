print " "*32 + "Checkers"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

print "This is the game of Checkers.  The computer is X,"
print "and you are O.  The computer will move first."
print "Squares are referred to by a coordinate system."
print "(0,0) is the lower left corner"
print "(0,7) is the upper left corner"
print "(7,0) is the lower right corner"
print "(7,7) is the upper right corner"
print "The computer will type '+TO' when you have another"
print "jump.  Type two negative numbers if you cannot jump."
print; print; print

input "(Press Return.)"; print		// (give player a chance to read)

// The board.  Pieces are represented by numeric values:
//    - 0      =  empty square
//    - -1,-2  =  computer (X) (-1 for regular piece, -2 for king)
//    - 1,2    =  human (O)    (1 for regular piece, 2 for king)
// Board is indexed by [x][y], so we have to initialize it sideways:
board = [
	[ 1,  0,  1,  0,  0,  0, -1,  0],
	[ 0,  1,  0,  0,  0, -1,  0, -1],
	[ 1,  0,  1,  0,  0,  0, -1,  0],
	[ 0,  1,  0,  0,  0, -1,  0, -1],
	[ 1,  0,  1,  0,  0,  0, -1,  0],
	[ 0,  1,  0,  0,  0, -1,  0, -1],
	[ 1,  0,  1,  0,  0,  0, -1,  0],
	[ 0,  1,  0,  0,  0, -1,  0, -1]]

// Function to print the board
printBoard = function	
	for y in range(7, 0)
		for x in range(0, 7)
			// We print by indexing with the board entry (-2 to 2) into a list
			// of possible representations.  Remember that in MiniScript, a
			// negative index counts from the end.
			print [". ", "O ", "O*", "X*", "X "][board[x][y]], "   "
		end for
		print; print
	end for
end function

// Function to get x,y coordinates from the player.
// This is written to allow the two numbers to be 
// separated by any combination of ',' and ' '.
// Returns input as [x,y].
inputPosition = function(prompt, requiredPieceSign, allowCancel=false)
	while true
		ans = input(prompt + "? ").replace(",", " ").split
		if ans.len < 2 then
			print "Enter two coordinates, for example: 4 0"
			continue
		end if
		x = val(ans[0])
		y = val(ans[-1])
		if x < 0 and y < 0 and allowCancel then
			return [x, y]
		else if x < 0 or x > 7 or y < 0 or y > 7 then
			print "Coordinates must be in the range 0-7"
		else if x%2 != y%2 then
			print "Invalid coordinates (both must be odd, or both even)"
		else if sign(board[x][y]) != requiredPieceSign then
			print "Invalid coordinates"
		else
			return [x, y]
		end if
	end while
end function

// Evaluate a potential (computer) move.
evalMove = function(fromX, fromY, toX, toY)
	score = 0
	
	// +2 if it promotes this piece
	if toY == 0 and board[fromX][fromY] == -1 then score += 2

	// +5 if it jumps an opponent's piece
	if abs(fromY-toY) == 2 then score += 5

	// -2 if the piece is moving away from the top boundary
	if fromY == 7 then score -= 2

	// +1 for putting the piece against a vertical boundary
	if toX == 0 or toX == 7 then score += 1

	// check neighboring pieces of the target position
	for c in [-1, 1]
		if toX+c < 0 or toX+c > 7 or toY-1 < 0 then continue
		// +1 for each adjacent friendly piece
		if board[toX+c][toY-1] < 0 then score += 1

		// -1 for each opponent piece that could now jump this one
		if toX-c >= 0 and toX-c <= 7 and toY+1 <= 7 and board[toX+c][toY-1] > 0 and (
			board[toX-c][toY+1] == 0 or (toX-c == fromX and toY+1 == fromY)) then score -= 2
	end for
	return score
end function

// Consider a possible (computer) move, including whether it's even valid.
// Return it or the previous best, whichever is better.
consider = function(fromX, fromY, toX, toY, previousBest)

	// make sure it's within the bounds of the board
	if toX < 0 or toX > 7 or toY < 0 or toY > 7 then return previousBest
	
	// if it's an opponent's piece, consider jumping it instead
	dx = toX - fromX
	if board[toX][toY] > 0 and abs(dx) == 1 then
		dy = toY - fromY
		return consider(fromX, fromY, fromX + dx*2, fromY + dy*2, previousBest)
	end if
	
	// if it's a jump, make sure it's over an opponent piece
	if abs(dx) == 2 then
		midX = (fromX + toX)/2; midY = (fromY + toY)/2
		if board[midX][midY] < 1 then return previousBest
	end if

	// make sure the destination is empty
	if board[toX][toY] then return previousBest
		
	// all checks passed; score it, and return whichever is better
	rating = evalMove(fromX, fromY, toX, toY)
	if not previousBest or rating > previousBest.rating then
		newBest = {}
		newBest.fromX = fromX; newBest.fromY = fromY
		newBest.toX = toX; newBest.toY = toY
		newBest.rating = rating
		return newBest
	else
		return previousBest
	end if
end function

// Do the computer's turn
doComputerTurn = function
	// For each square on the board containing one of my pieces, consider
	// possible moves and keep the best one so far.  Start with a step
	// size of 1 (ordinary move), but if we jump, then keep jumping.
	stepSize = 1
	while true
		bestMove = null
		for x in range(0, 7)
			for y in range(0, 7)
				if board[x][y] >= 0 then continue	// not my piece
				move = {}
				move.fromPos = [x,y]
			
				// Consider forward moves; if it's a king, also consider backward moves
				for dx in [-stepSize, stepSize]
					move.toPos = [dx, -stepSize]
					bestMove = consider(x, y, x+dx, y-stepSize, bestMove)
					if board[x][y] == -2 then bestMove = consider(x, y, x+dx, y+stepSize, bestMove)
				end for
			end for
		end for
	
		if not bestMove then
			// No valid move -- if step size is still 1, this means we 
			// couldn't find ANY move on our turn, and we have lost.
			// Otherwise, we're done.
			if stepSize == 1 then
				globals.gameOver = true
				globals.winner = 1
			end if
			break
		end if
		
		// Do the move, and stop if we did not jump.
		if stepSize == 1 then
			print "From " + bestMove.fromX + " " + bestMove.fromY, ""
		end if
		print " to " + bestMove.toX + " " + bestMove.toY, ""

		movePiece bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY
		if abs(bestMove.toX - bestMove.fromX) == 1 then break
		stepSize = 2
	end while
	print
end function

// Move one piece (including captures and crowning
movePiece = function(fromX, fromY, toX, toY)
	piece = board[fromX][fromY]
	board[toX][toY] = piece
	board[fromX][fromY] = 0
	// capture piece jumped over
	if abs(toX - fromX) == 2 then
		board[(fromX+toX)/2][(fromY+toY)/2] = 0
	end if
	// crown (make into a king) a piece that reaches the back row
	if (toY == 7 and piece > 0) or (toY == 0 and piece < 0) then
		board[toX][toY] = 2 * sign(piece)
	end if
end function

// Handle the player's move.
doPlayerTurn = function
	fromPos = inputPosition("From", 1, true)
	fromX = fromPos[0]; fromY = fromPos[1]
	if fromX < 0 then
		// Player concedes the game.
		globals.gameOver = true
		globals.winner = -1
		return
	end if
	while true
		toPos = inputPosition("To", 0)
		toX = toPos[0]; toY = toPos[1]
		dist = abs(toX - fromX)
		if dist <= 2 and abs(toY - fromY) == dist then break
	end while
	while true
		// Make the move, and continue as long as we have a jump
		movePiece fromX, fromY, toX, toY
		if dist != 2 then break
		
		// Prompt for another move, allowing a cancel (negative input).
		fromX = toX; fromY = toX
		toPos = inputPosition("+To", 0, true)
		if toPos[0] < 0 or toPos[1] < 0 then break
		toX = toPos[0]; toY = toPos[1]
	end while
	// If piece has reached the end of the board, crown this piece
	if toY == 7 then board[toX][toY] = 2
end function

// Main loop.
gameOver = false
while not gameOver
	doComputerTurn
	if gameOver then break
	printBoard
	doPlayerTurn
	if gameOver then break
	printBoard
end while

print
if winner > 0 then print "You win." else print "I win."
