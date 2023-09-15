print "Solitaire Checker Puzzle by david ahl"
print
print "48 checkers are placed on the 2 outside spaces of a"
print "standard 64-square checkerboard.  The object is to"
print "remove as many checkers as possible by diagonal jumps"
print "(as in standard checkers).  Use the numbered board to"
print "indicate the square you wish to jump from and to.  On"
print "the board printed out on each turn '1' indicates a"
print "checker and '0' an empty square.  When you have no"
print "possible jumps remaining, input a '0' in response to"
print "question 'Jump from?'"
print

pad4 = function(n)
	return ("    " + n)[-4:]
end function

printBoard = function
	// Idea: This program could be greatly improved by printing the board
	// as the index numbers (1-64), indicating which of those positions
	// contain checkers via color or punctuation, e.g. "(42)" vs " 42 ".
	// This would make it much easier for the user to figure out what
	// numbers correspond to the positions they have in mind.
	print
	for j in range(1, 57, 8)
		print " " + board[j:j+8].join("  ")
	end for
end function

initBoard = function
	globals.board = [null] + [1] * 64	// treat this as 1-based array
	for j in range(19, 43, 8)
		for i in range(j, j+3)
			board[i] = 0
		end for
	end for
end function

isLegal = function(from, to)
	if board[from] == 0 then return false
	if board[to] == 1 then return false
	if board[(to+from)/2] == 0 then return false
	fromRow = floor((from-1) / 8)	// row in range 0-7
	fromCol = from - fromRow*8		// column in range 1-8
	toRow = floor((to-1) / 8)
	toCol = to - toRow*8
	if fromRow > 7 or toRow > 7 or fromCol > 8 or toCol > 8 then return false
	if abs(fromRow-toRow) != 2 or abs(fromCol-toCol) != 2 then return false
	return true
end function

askYesNo = function(prompt)
	while true
		answer = input(prompt + "? ").lower
		if answer and answer[0] == "y" then return "yes"
		if answer and answer[0] == "n" then return "no"
		print "Please answer 'yes' or 'no'."
	end while
end function

print "Here is the numerical board:"
print
for j in range(1, 57, 8)
	print pad4(j) + pad4(j+1) + pad4(j+2) + pad4(j+3) +
	  pad4(j+4) + pad4(j+5) + pad4(j+6) + pad4(j+7)
end for

while true
	initBoard
	print
	print "And here is the opening position of the checkers."
	printBoard
	jumps = 0
	while true
		fromPos = input("Jump from? ").val
		if fromPos == 0 then break
		toPos = input("To? ").val
		print
		if not isLegal(fromPos, toPos) then
			print "Illegal move.  Try again..."
			continue
		end if
		board[fromPos] = 0
		board[toPos] = 1
		board[(toPos+fromPos)/2] = 0
		jumps += 1
		printBoard
	end while
	// End game summary
	sum = board[1:].sum
	print "You made " + jumps + " jumps and had " + sum + " pieces"
	print "remaining on the board."
	print
	if askYesNo("Try again") == "no" then break
end while
print
print "O.K.  Hope you had fun!!"

	