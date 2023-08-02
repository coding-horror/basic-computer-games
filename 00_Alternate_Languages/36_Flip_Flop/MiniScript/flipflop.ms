print " "*32 + "FlipFlop"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print
// Created by Michael Cass (1978)
// Ported to MiniScript by Joe Strout (2023)

print "The object of this puzzle is to change this:"
print
print "X X X X X X X X X X"
print
print "to this:"
print
print "O O O O O O O O O O"
print
print "By typing the number corresponding to the position of the"
print "letter on some numbers, one position will change, on"
print "others, two will change.  To reset line to all X's, type 0"
print "(zero) and to start over in the middle of a game, type "
print "11 (eleven)."
print

startNewGame = function
	globals.q = rnd
	globals.guesses = 0
	print "Here is the starting line of X's."
	print
	print "1 2 3 4 5 6 7 8 9 10"
	print "X X X X X X X X X X"
	print
end function

getInput = function
	while true
		n = input("Input the number: ").val
		if n == floor(n) and 0 <= n <= 11 then break
		print "Illegal entry--try again."		
	end while
	return n
end function

startNewGame
while true
	A = [""] + ["X"] * 10	// (include empty 0th element so we can index 1-based
	m = 0		// (previous input)
	while true
		n = getInput
		if n == 11 then
			startNewGame
			continue
		else if n == 0 then
			A = ["X"] * 10
			continue
		end if
	
		if n != m then
			// when user enters a different number from previous time
			m = n
			if A[n] == "O" then A[n] = "X" else A[n] = "O"
			while m == n
				r = tan(q + n/q - n) - sin(q/n) + 336*sin(8*n)
				n = floor(10 * (r - floor(r)))
				if A[n] != "O" then
					A[n] = "O"
					break
				end if
				A[n] = "X"
			end while		
		else
			// when n == m, i.e., user entered the same number twice in a row
			while m == n
				if A[n] == "O" then A[n] = "X" else A[n] = "O"
				r = 0.592 * (1 / tan(q/n + q)) / sin(n*2 + q) - cos(n)
				n = floor(10 * (r - floor(r)))
				if A[n] != "O" then
					A[n] = "O"
					break
				end if
				A[n] = "X"
			end while
		end if

		print "1 2 3 4 5 6 7 8 9 10"
		print A[1:].join
		guesses += 1
		if A[1:] == ["O"]*10 then break
	end while

	if guesses <= 12 then
		print "Very good.  You guessed it in only " + guesses + " guesses."
	else
		print "Try harder next time.  It took you " + guesses + " guesses."
	end if

	yesNo = input("Do you want to try another puzzle? ").lower
	if not yesNo or yesNo[0] == "n" then break
	print
end while
