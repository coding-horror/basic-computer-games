print " "*32 + "Hexapawn"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

//  Hexapawn:  interpretation of hexapawn game as presented in
//  Martin Gardner's "The Unexpected Hanging and Other Mathematic-
//  al Diversions", chapter eight:  A Matchbox Game-Learning Machine
//  Original version for H-P timeshare system by reversed.A. Kaapke 5/5/76
//  Instructions by jeff dalton
//  Conversion to MITS BASIC by Steve North
//	Conversion to MiniScript by Joe Strout

// All 19 possible board positions:
ba = [null,
          [null,-1,-1,-1,1,0,0,0,1,1],
          [null,-1,-1,-1,0,1,0,1,0,1],
          [null,-1,0,-1,-1,1,0,0,0,1],
          [null,0,-1,-1,1,-1,0,0,0,1],
          [null,-1,0,-1,1,1,0,0,1,0],
          [null,-1,-1,0,1,0,1,0,0,1],
          [null,0,-1,-1,0,-1,1,1,0,0],
          [null,0,-1,-1,-1,1,1,1,0,0],
          [null,-1,0,-1,-1,0,1,0,1,0],
          [null,0,-1,-1,0,1,0,0,0,1],
          [null,0,-1,-1,0,1,0,1,0,0],
          [null,-1,0,-1,1,0,0,0,0,1],
          [null,0,0,-1,-1,-1,1,0,0,0],
          [null,-1,0,0,1,1,1,0,0,0],
          [null,0,-1,0,-1,1,1,0,0,0],
          [null,-1,0,0,-1,-1,1,0,0,0],
          [null,0,0,-1,-1,1,0,0,0,0],
          [null,0,-1,0,1,-1,0,0,0,0],
          [null,-1,0,0,-1,1,0,0,0,0]]
          
// Possible responses for each board position (move from x to y,
// represented as x*10 + y):
ma = [null,
          [null,24,25,36,0],
          [null,14,15,36,0],
          [null,15,35,36,47],
          [null,36,58,59,0],
          [null,15,35,36,0],
          [null,24,25,26,0],
          [null,26,57,58,0],
          [null,26,35,0,0],
          [null,47,48,0,0],
          [null,35,36,0,0],
          [null,35,36,0,0],
          [null,36,0,0,0],
          [null,47,58,0,0],
          [null,15,0,0,0],
          [null,26,47,0,0],
          [null,47,58,0,0],
          [null,35,36,47,0],
          [null,28,58,0,0],
          [null,15,47,0,0]]
s = [0]*10
t = [0]*10

showBoard = function
    print
    for i in [1,2,3]
        print " "*10, ""
        for j in [1,2,3]
            print "X.O"[s[(i - 1) * 3 + j] + 1], ""
        end for
        print
    end for
end function

mirror = function(x)
	return [null, 3,2,1, 6,5,4, 9,8,7][x]
end function

mirrorBoard = function(b)
	return [null, b[3],b[2],b[1], b[6],b[5],b[4], b[9],b[8],b[7]]
end function

intro = function
	while true
		s = input("Instructions (Y-N)? ").lower
		if s then s = s[0]
		if s == "n" then return
		if s == "y" then break
	end while
	print
	print "This program plays the game of Hexapawn."
	print "Hexapawn is played with Chess pawns on a 3 by 3 board."
	print "The pawns are moved as in Chess - one space forward to"
	print "an empty space or one space forward and diagonally to"
	print "capture an opposing man.  On the board, your pawns"
	print "are 'O', the computer's pawns are 'X', and empty "
	print "squares are '.'.  To enter a move, type the number of"
	print "the square you are moving from, followed by the number"
	print "of the square you will move to.  The numbers must be"
	print "seperated by a comma."
	print
	input "(Press Return.)"
	print
	print "The computer starts a series of games knowing only when"
	print "the game is won (a draw is impossible) and how to move."
	print "It has no strategy at first and just moves randomly."
	print "However, it learns from each game.  Thus, winning becomes"
	print "more and more difficult.  Also, to help offset your"
	print "initial advantage, you will not be told how to win the"
	print "game but must learn this by playing."
	print
	print "The numbering of the board is as follows:"
	print " "*10 + "123"
	print " "*10 + "456"
	print " "*10 + "789"
	print
	print "For example, to move your rightmost pawn forward,"
	print "you would type 9,6 in response to the question"
	print "'Your move?'.  Since I'm a good sport, you'll always"
	print "go first."
	print
end function	

getMove = function
	while true
		inp = input("Your move? ").replace(",", " ").split
		if inp.len > 1 then
			m1 = inp[0].val
			m2 = inp[-1].val
			if 0 < m1 < 10 and 0 < m2 < 10 then
				if s[m1] != 1 or s[m2] == 1 or 
				  (m2 - m1 != -3 and s[m2] != -1) or 
				  (m2 > m1) or (m2 - m1 == -3 and s[m2] != 0) or 
				  (m2 - m1 < -4) or (m1 == 7 and m2 == 3) then
					print "Illegal move."
					continue
				end if
				return [m1, m2]
			end if
		end if
		print "Illegal co-ordinates."
	end while
end function

// Find the current board number (1-19) and whether it is mirrored.
findBoardNum = function
	idx = ba.indexOf(s)
	if idx != null then return [idx, false]
	idx = ba.indexOf(mirrorBoard(s))
	if idx != null then return [idx, true]
	return null
end function

// Main program
intro
wins = 0
losses = 0
while true
	s = [null, -1,-1,-1, 0,0,0, 1,1,1]
	computerWins = false
	showBoard
	while true
		// Input player's move
		userMove = getMove
		m1 = userMove[0]; m2 = userMove[1]

		// Move player's pawn
		s[m1] = 0
		s[m2] = 1
		showBoard

		// If no computer pawns, or player reached top, then computer loses
		if s.indexOf(-1) == null or s[1] == 1 or s[2] == 1 or s[3] == 1 then
			break
		end if
		// Ensure at least one computer pawn with valid move.
		// (Note: original BASIC code for this had several bugs; the code
		// below should be more correct.)
		anyValidMove = false
		for i in range(1, 6)	// (no sense checking position 7-9)
			if s[i] != -1 then continue
			// check for a straight-ahead move
			if s[i + 3] == 0 then anyValidMove = true
			// check for a capture
			if i == 2 or i == 5 then
				if s[i+2] == 1 or s[i+4] == 1 then anyValidMove = true
			else if i == 1 or i == 4 then
				if s[i+4] == 1 then anyValidMove = true
			else
				if s[i+2] == 1 then anyValidMove = true
			end if
		end for
		if not anyValidMove then break
		
		boardAndReversed = findBoardNum
		if boardAndReversed == null then
			print "Illegal board pattern"	// (should never happen in normal play)
			break
		end if
		x = boardAndReversed[0]; reversed = boardAndReversed[1]

		// Select a random move for board X, as permitted by our memory
		possibilities = []
		for i in range(1, 4)
			if ma[x][i] != 0 then possibilities.push i
		end for

		// For more insight into how the computer learns, uncomment this line:
		//print "Considering for board " + x + ": " + possibilities + " (reversed=" + reversed + ")"
		if not possibilities then
			print "I resign."
			break
		end if
		possibilities.shuffle
		y = possibilities[0]
		
		m1 = floor(ma[x][y] / 10)
		m2 = ma[x][y] % 10
		if reversed then
			m1 = mirror(m1)
			m2 = mirror(m2)
		end if
		
		// Announce move
		print "I move from " + m1 + " to " + m2
		s[m1] = 0
		s[m2] = -1
		showBoard
		
		// Finish if computer reaches bottom, or no player pawns are left
		if s[7] == -1 or s[8] == -1 or s[9] == -1 or s.indexOf(1) == null then
			computerWins = true
			break
		end if
		
		// Finish if player cannot move
		playerCanMove = false
		for i in range(1, 9)
			if s[i] != 1 then continue
			if i > 3 and s[i - 3] == 0 then playerCanMove = true
			if mirror(i) != i then
				if i >= 7 then
					if s[5] == -1 then playerCanMove = true
				else
					if s[2] == -1 then playerCanMove = true
				end if
			else
				if s[i - 2] == -1 or s[i - 4] == -1 then playerCanMove = true
			end if
		end for
		if not playerCanMove then
			print "You can't move, so ", ""
			computerWins = true
			break
		end if
	end while
	if computerWins then
		print "I win."
		wins += 1
	else 
		print "You win"
		// Because we lost, clear out the last response used, so that we don't
		// make the same mistake again.  This is how the computer learns!
		ma[x][y] = 0
		losses += 1
	end if
	print "I have won " + wins + " and you " + losses + " out of " + (losses + wins) + " games."
	print
	wait 2
end while