print " "*34 + "Awari"
print " "*15 + "Creative Computing  Morristown, New Jersey"

// Keep a list of the opening moves (up to 8) of previously lost or drawn games.
// These will be used to penalize repeating the same moves again.
badGames = []

printQty = function(qty)
	print (" " + qty)[-2:], " "
end function

printBoard = function
	print; print "   ", ""
	for i in range(12, 7); printQty board[i]; end for
	print; printQty board[13]
	print "   " * 6, ""; printQty board[6]
	print; print "   ", ""
	for i in range(0, 5); printQty board[i]; end for
	print; print
end function

// Redistribute the stones starting at position.
// If the last one ends by itself, opposite some nonzero
// stones on the other side, then capture them into homePos.
// Return true if the last stone ended at homePos, false otherwise.
moveStones = function(board, position, homePos)
	p = board[position]; board[position] = 0
	while p
		position = (position + 1) % 14
		board[position] += 1
		p -= 1
	end while
	if board[position] == 1 and position != 6 and position != 13 and board[12-position] then
		board[homePos] += board[12-position] + 1
		board[position] = 0
		board[12-position] = 0
	end if
	globals.gameOver = board[0:6].sum == 0 or board[7:13].sum == 0
	return position == homePos
end function

// Get the player move.  Note that the player inputs their move as a number
// from 1-6, but we return it as an actual board position index 0-6.
getPlayerMove = function(prompt="Your move")
	while true
		pos = input(prompt + "? ").val
		if 0 < pos < 7 and board[pos-1] then return pos - 1
		print "Illegal move."
	end while
end function

getComputerMove = function
	// Copy the board for safekeeping
	boardCopy = board[:]
	bestScore = -99; bestMove = 0
	for j in range(7, 12)
		if board[j] == 0 then continue	// can't move from an empty spot
		// suppose we move at position j...
		moveStones board, j, 13
		// consider each possible response the player could make
		bestPlayerScore = 0
		for i in range(0, 5)
			if board[i] == 0 then continue
			landPos = board[i] + i	// figure the landing position
			score = floor(landPos / 14); landPos %= 14
			if board[landPos] == 0 and landPos != 6 and landPos != 13 then
				score += board[12 - landPos]	// points for capturing stones
			end if
			if score > bestPlayerScore then bestPlayerScore = score
		end for
		// figure our own score as our points, minus player points, minus best player score
		ourScore = board[13] - board[6] - bestPlayerScore
		if gameMoves.len < 8 then
			// subtract 2 points if current series of moves is in our bad-games list
			proposed = gameMoves + [j]
			for badGame in badGames
				if badGame[:proposed.len] == proposed then ourScore -= 2
			end for
		end if
		if ourScore > bestScore then
			bestScore = ourScore
			bestMove = j
		end if
		// restore the board
		globals.board = boardCopy[:]
	end for
	print char(42+bestMove)	// (labels computer spots as 1-6 from right to left)
	return bestMove
end function

// The game is over when either side has 0 stones left.
isGameOver = function
	return board[0:6].sum == 0 or board[7:13].sum == 0
end function

// Play one game to completion.
playOneGame = function
	// The board is represented as a list of 13 numbers.
	// Position 6 is the player's home; 13 is the computer's home.
	globals.board = [3]*14; board[13] = 0; board[6] = 0
	// Also keep a list of the moves in the current game
	globals.gameMoves = []
	print; print
	while true
		// Player's turn
		printBoard
		pos = getPlayerMove
		gameMoves.push pos
		if moveStones(board, pos, 6) then
			if gameOver then break
			printBoard
			pos = getPlayerMove("Again")
			gameMoves.push pos
			moveStones board, pos, 6
		end if
		if gameOver then break
		// Computer's turn
		printBoard; print "My move is ", ""
		pos = getComputerMove
		gameMoves.push pos
		if moveStones(board, pos, 13) then
			if gameOver then break
			printBoard; print "...followed by ", ""
			pos = getComputerMove
			gameMoves.push pos
			moveStones board, pos, 13
		end if
		if gameOver then break
	end while
	printBoard
	print; print "GAME OVER"
	delta = board[6] - board[13]
	if delta < 0 then
		print "I win by " + (-delta) + " points"
		return
	end if
	if delta == 0 then print "Drawn game" else print "You win by " + delta + " points"
	if gameMoves.len > 8 then gameMoves = gameMoves[:8]
	if badGames.indexOf(gameMoves) == null then badGames.push gameMoves	
end function

// Main loop
while true
	playOneGame
	print; print
end while
