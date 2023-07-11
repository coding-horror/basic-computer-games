print " "*33 + "BAGELS"
print " "*15 + "Creative Computing  Morristown, New Jersey"; print; print
// *** BAGELS Number Guessing Game
// *** Original source unknown but suspected to be
// *** Lawrence Hall of Science, U.C. Berkely

print; print; print
inp = input("Would you like the rules (yes or no)? ")
if not inp or inp[0].lower != "n" then
	print; print "I am thinking of a three-digit number.  Try to guess"
	print "my number and I will give you clues as follows:"
	print "   PICO   - one digit correct but in the wrong position"
	print "   FERMI  - one digit correct and in the right position"
	print "   BAGELS - no digits correct"
end if

pickNumber = function
	// pick three unique random digits
	while true
		actual = [floor(10*rnd), floor(10*rnd), floor(10*rnd)]
		if actual[0] != actual[1] and actual[0] != actual[2] and actual[1] != actual[2] then break
	end while
	//print "DEBUG: actual=" + actual
	print; print "O.K.  I have a number in mind."
	return actual
end function

getGuess = function(guessNum)
	isNotDigit = function(c); return c < "0" or c > "9"; end function
	while true
		inp = input("Guess #" + guessNum + "? ")
		if inp.len != 3 then
			print "Try guessing a three-digit number."
		else if inp[0] == inp[1] or inp[0] == inp[2] or inp[1] == inp[2] then
			print "Oh, I forgot to tell you that the number I have in mind"
			print "has no two digits the same."
		else if isNotDigit(inp[0]) or isNotDigit(inp[1]) or isNotDigit(inp[2]) then
			print "What?"
		else
			return [inp[0].val, inp[1].val, inp[2].val]
		end if
	end while
end function

doOneGame = function
	actual = pickNumber
	for guessNum in range(1, 20)
		guess = getGuess(guessNum)
		picos = 0; fermis = 0
		for i in [0,1,2]
			if guess[i] == actual[i] then
				fermis += 1
			else if actual.indexOf(guess[i]) != null then
				picos += 1
			end if
		end for
		if fermis == 3 then
			print "YOU GOT IT!!!"
			globals.score += 1
			return
		else if picos or fermis then
			print "PICO " * picos + "FERMI " * fermis
		else
			print "BAGELS"
		end if
	end for
	print "Oh well."
	print "That's twenty guesses.  My number was " + actual.join("")
end function

// main loop
score = 0
while true
	doOneGame
	print
	inp = input("Play again (yes or no)? ")
	if not inp or inp[0].upper != "Y" then break
end while
if score then
	print; print "A " + score + " point BAGELS buff!!"
end if
print "Hope you had fun.  Bye."

