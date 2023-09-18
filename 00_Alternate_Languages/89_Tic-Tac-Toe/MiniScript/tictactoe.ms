// This program plays Tic Tac Toe
// The machine goes first and the way this is set up
// there's no way the human player can win. At best
// it will be a draw.
computerNext = function(x)
	return x - 8 * floor((x - 1) / 8)
end function

print " " * 30 + "TIC TAC TOE"
print " " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print; print; print

print "The game board is numbered:"
print
print "1 2 3"
print "8 9 4"
print "7 6 5"
print

while true
	computer = 9
	gameOver = false
	// MOVE ONE line 240 in original
	// Computer always moves first and takes the center
	print "Computer moves " + computer
	player = input("Your move? ").val
	playerFirstMove = player
	
	// MOVE TWO line 280
	// Computer's 2nd move - always the next space clockwise
	// from the player's
	computer = computerNext(player + 1)
	canWinAt = computerNext(computer+4)
	print "Computer moves " + computer
	player = input("Your move? ").val
	
	// MOVE THREE line 300
	// Computer has two consecutive cells. This includes the
	// middle so, to complete this 3-in-a-row, get the opposite
	// value of comp's last move - which is four cells clockwise away.
	
	if player != canWinAt then
		computer = canWinAt
		print "Computer moves " + computer
		print "... and wins ********"
		gameOver = true
	else
		// Blocked - so two cells away from comp's last move
		// line 360
		computer = computerNext(computer + 2)
		print "Computer moves " + computer
	end if
	
	if gameOver == false then
		canWinAt = computerNext(computer+4)
		player = input("Your move? ").val
		
		// MOVE FOUR - line 400
		if player != canWinAt then
			computer = canWinAt
			print "Computer moves " + computer
			print "... and wins ********"
			gameOver = true
		else
			// Foiled again! - line 450
			if playerFirstMove % 2 == 0 then
				computer = computerNext(computer + 7)
				print "Computer moves " + computer
				print "... and wins ********"
				gameOver = true
			else // line 500
				computer = computerNext(computer + 3)
				print "Computer moves " + computer
			end if
		end if
	end if
	
	if gameOver == false then
		// line 520
		player = input("Your move? ").val
		if player != computerNext(computer + 4) then
			computer = computerNext(computer + 4)
		else
			computer = computerNext(computer + 6)
		end if
		print "Computer moves " + computer
		print "The game is a draw."
	end if
	print
end while
