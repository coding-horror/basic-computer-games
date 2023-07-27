print " "*33 + "Chomp"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
// *** THE GAME OF CHOMP *** COPYRIGHT PCC 1973 ***

initBoard = function(rows, columns)
	globals.rows = rows
	globals.columns = columns
	globals.board = [[null]]		// indexed as [row][column], 1-based
	for i in range(1, rows)
		board.push ["."] + ["*"] * columns
	end for
	board[1][1] = "P"
end function

printBoard = function
	print
	print " "*7 + "1 2 3 4 5 6 7 8 9"
	for i in range(1, rows)
		print i + " "*6 + board[i][1:].join
	end for
	print
end function	

introduction = function
	print
	print "This is the game of chomp (Scientific American, Jan 1973)"
	r = input("Do you want the rules (1=yes, 0=no!)? ").val
	if r == 0 then return
	print "Chomp is for 1 or more players (humans only)."
	print
	print "Here's how a board looks (this one is 5 by 7):"
	initBoard 5, 7
	printBoard 
	print
	print "The board is a big cookie - R rows high and C columns"
	print "wide. You input R and C at the start. In the upper left"
	print "corner of the cookie is a poison square (P). The one who"
	print "chomps the poison square loses. To take a chomp, type the"
	print "row and column of one of the squares on the cookie."
	print "All of the squares below and to the right of that square"
	print "(including that square, too) disappear -- chomp!!"
	print "No fair chomping squares that have already been chomped,"
	print "or that are outside the original dimensions of the cookie."
	print
end function

setup = function	
	print
	globals.numPlayers = input("How many players? ").val
	rows = input("How many rows? ").val
	while rows > 9
		rows = input("Too many rows (9 is maximum). Now, how many rows? ").val
	end while
	columns = input("How many columns? ").val
	while rows > 9
		columns = input("Too many columns (9 is maximum). Now, how many columns? ").val
	end while
	print
	initBoard rows, columns
end function

doOneTurn = function(player)
	printBoard
	print "Player " + player
	while true
		inp = input("Coordinates of chomp (row,column)? ")
		inp = inp.replace(",", " ").split
		if inp.len < 2 then continue
		r = inp[0].val
		c = inp[-1].val
		if 1 <= r <= rows and 1 <= c <= columns and board[r][c] != " " then break
		print "No fair. You're trying to chomp on empty space!"
	end while
	if board[r][c] == "P" then
		print "You lose, player " + player
		globals.gameOver = true
	else
	
	end if
	for row in range(r, rows)
		for col in range(c, columns)
			board[row][col] = " "
		end for
	end for
end function

// Main program
introduction
while true
	setup
	gameOver = false
	player = 0
	while not gameOver
		player += 1
		if player > numPlayers then player = 1
		doOneTurn player
	end while
	print
	r = input("Again (1=yes, 0=no!)? ").val
	if r != 1 then break
end while
