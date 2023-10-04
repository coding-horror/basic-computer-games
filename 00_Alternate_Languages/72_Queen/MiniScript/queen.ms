print " "*33 + "QUEEN"
print " "*15 + "Creative Computing  Morristown  New Jersey"
print; print; print

getYesNo = function(prompt)
	while true
		inp = input(prompt + "? ").lower + " "
		if inp[0] == "y" then return "yes"
		if inp[0] == "n" then return "no"
		print "Please answer 'yes' or 'no'."
	end while
end function

printDirections = function
	print "We are going to play a game based on one of the chess"
	print "moves.  Our queen will be able to move only to the left "
	print "down  or diagonally down and to the left."
	print
	print "The object of the game is to place the queen in the lower"
	print "left hand square by alternating moves between you and the"
	print "computer.  The first one to place the queen there wins."
	print
	print "You go first and place the queen in any one of the squares"
	print "on the top row or right hand column."
	print "That will be your first move."
	print "We alternate moves."
	print "You may forfeit by typing '0' as your move."
	print "Be sure to press the return key after each response."
	print
	input "(Press Return to continue.)"
	print
end function

printCoordinates = function
	// Porting note: I cannot imagine what possessed the original author to
	// use such a crazy numbering scheme, which is not easy for the human
	// player OR the code.  But assumptions about it are scattered throughout
	// the whole program, so we're stuck with it.
	print
	print "  81   71   61   51   41   31   21   11"
	print "  92   82   72   62   52   42   32   22"
	print " 103   93   83   73   63   53   43   33"
	print " 114  104   94   84   74   64   54   44"
	print " 125  115  105   95   85   75   65   55"
	print " 136  126  116  106   96   86   76   66"
	print " 147  137  127  117  107   97   87   77"
	print " 158  148  138  128  118  108   98   88"
	print
end function

getStartPos = function
	while true
		m1 = input("Where would you like to start? ").val
		tens = floor(m1/10)
		ones = m1 % 10
		if ones == 1 or tens == ones or m1 == 0 then return m1
		print "Please read the directions again."
		print "You have begun illegally."
		print
	end while
end function

isGoodMove = function(ones, tens)
	pos = 10 * tens + ones
	return [158, 127, 126, 75, 73].indexOf(pos) != null
end function

getRandomMove = function(queenPos)
	tens = floor(queenPos/10)
	ones = queenPos % 10
	z = rnd
	if z > 0.6 then
		return 10 * (tens+1) + ones
	else if z > 0.3 then
		return 10 * (tens+2) + (ones+1)
	else
		return 10 * (tens+1) + ones
	end if
end function

getComputerMove = function(queenPos)
	tens = floor(queenPos/10)
	ones = queenPos % 10
	if [41, 44, 73, 75, 126, 127].indexOf(queenPos) != null then return getRandomMove(queenPos)
	for k in range(7, 1)
		if isGoodMove(ones, tens+k) then return 10 * (tens+k) + ones		// left
		if isGoodMove(ones+k, tens+k) then return 10 * (tens+k) + (ones+k)	// down
		if isGoodMove(ones+k, tens+k*2) then return 10 * (tens+k*2) + (ones+k) // down-left
	end for
	return getRandomMove(queenPos)		
end function

getHumanMove = function(queenPos)
	tens = floor(queenPos/10)
	ones = queenPos % 10
	while true
		pos = input("What is your move? ").val
		if pos == 0 then return 0
		dTens = floor(pos/10) - tens
		dOnes = pos % 10 - ones
		ok = false
		if dOnes == 0 and dTens > 0 then ok = true	// moving left
		if dOnes == dTens and dOnes > 0 then ok = true	// moving down
		if dTens == dOnes*2 and dOnes > 0 then ok = true	// moving down-left
		if ok then return pos
		print
		print "Y O U   C H E A T . . .  Try again";
	end while
end function

playGame = function
	queenPos = getStartPos
	while true
		if queenPos == 0 then
			print "It looks like I have won by forfeit."
			return
		end if
		
		// computer move
		queenPos = getComputerMove(queenPos)
		print "Computer moves to square " + queenPos
		if queenPos == 158 then
			print
			print "Nice try, but it looks like I have won."
			return
		end if

		// human move
		queenPos = getHumanMove(queenPos)
		if queenPos == 158 then
			print
			print "C O N G R A T U L A T I O N S . . ."
			print
			print "You have won--very well played."
			print "It looks like I have met my match."
			print "Thanks for playing---I can't win all the time."
			return
		end if
	end while		
end function

// Main program
if getYesNo("Do you want instructions") == "yes" then printDirections
while true
	printCoordinates
	playGame
	print
	if getYesNo("Anyone else care to try") == "no" then break
end while
print
print "OK --- Thanks again."
