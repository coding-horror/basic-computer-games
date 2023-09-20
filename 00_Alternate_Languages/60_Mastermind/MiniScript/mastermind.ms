//
//  MASTERMIND II
//  STEVE NORTH
//  CREATIVE COMPUTING
//  PO BOX 789-M MORRISTOWN NEW JERSEY 07960
//
//	Converted to MiniScript by Joe Strout (Sep 2023)

// Advance the given combination to the next possible combination.
// Note that (unlike the BASIC program) our values are 0-based, not 1-based.
incrementCombo = function(combo)
	idx = 0
	while true
		combo[idx] += 1
		if combo[idx] < colorCount then return
		combo[idx] = 0
		idx += 1
	end while
end function

// Convert a numeric combination like [2, 0, 1] into a color string like "RBW".
comboToColorString = function(combo)
	result = ""
	for q in combo
		result += colorCodes[q]
	end for
	return result
end function

// Get a random color code like "RBW".
getRandomCode = function
	// We do this by starting with a numeric combo of all 0's (first color),
	// and then stepping through subsequent combos a random number of times.
	combo = [0] * posCount
	steps = floor(possibilities * rnd)
	while steps
		incrementCombo combo
		steps -= 1
	end while
	return comboToColorString(combo)
end function

// Return [blackPins, whitePins] for the given guess and actual code.
// blackPins is how many guess entries are the correct color AND position;
// whitePins is how many guess entries have the right color, but wrong position.
// This works with either color strings or numeric combos.
calcResult = function(guess, actual)
	if guess isa string then guess = guess.split("") else guess = guess[:]
	if actual isa string then actual = actual.split("") else actual = actual[:]
	black = 0; white = 0
	for i in guess.indexes
		if guess[i] == actual[i] then
			black += 1
			actual[i] = null
		else
			for j in actual.indexes
				if guess[i] == actual[j] and guess[j] != actual[j] then
					white += 1
					actual[j] = null
					break
				end if
			end for
		end if
		guess[i] = null
	end for
	return [black, white]
end function

// Pad a string with spaces, to the given width.
pad = function(s, width=12)
	return (s + " "*width)[:width]
end function

// Print the history of guesses and results.
printBoard = function
	print
	print "BOARD"
	print "Move     Guess          Black     White"
	for i in guessHistory.indexes
		print pad(i+1, 9) + pad(guessHistory[i], 15) + 
		  pad(guessResult[i][0], 10) + guessResult[i][1]
	end for
	print	
end function

// Quit the game (after reporting the computer's secret code).
quit = function(computerCode)
	print "Quitter!  My combination was: " + computerCode
	print
	print "Good bye"
	exit
end function

// Prompt the user for a guess (e.g. "RBW").  
// Also handle "BOARD" and "QUIT" commands.
inputGuess = function(guessNum, secretCode)
	while true
		guess = input("Move #" + guessNum + " Guess? ").upper
		if guess == "BOARD" then
			printBoard
		else if guess == "QUIT" then
			quit secretCode
		else if guess.len != posCount then
			print "Bad number of positions."
		else
			ok = true
			for c in guess
				if colorCodes.indexOf(c) == null then
					print "'" + c + "' is unrecognized."
					ok = false
					break
				end if
			end for
			if ok then return guess
		end if
	end while
end function

// Play one half-round where the computer picks a code,
// and the human tries to guess it.
doHumanGuesses = function
	print "Guess my combination."
	print
	secretCode = getRandomCode
	//print "My secret combo is: " + secretCode	// (for debugging purposes)
	
	globals.guessHistory = []	// list of guesses, e.g. "RBW"
	globals.guessResult = []	// result of each guess, as [BlackPins, WhitePins]
	
	for guessNum in range(1, 10)
		guess = inputGuess(guessNum, secretCode)
		result = calcResult(guess, secretCode)
		if result[0] == posCount then
			print "You guessed it in " + guessNum + " moves!"
			break
		end if
		// Tell human results
		print "You have " + result[0] + " blacks and " + result[1] + " whites."
		// Save all this stuff for board printout later
		guessHistory.push guess
		guessResult.push result
	end for
	if guess != secretCode then
		print "You ran out of moves!  That's all you get!"
		print "The actual combination was: " + secretCode
	end if
	globals.humanScore += guessNum
end function

// Play one half-round where the human picks a code,
// and the computer tries to guess it.
// Return true if this goes OK, and false if we need a do-over.
doComputerGuesses = function
	print "Now I guess.  Think of a combination."
	input "Hit Return when ready:"
	
	// Make a list of possible combination *numbers*, from 0 to possibilities-1.
	// We'll remove entries from this list as we eliminate them with our guesses.
	possible = range(0, possibilities-1)
	
	gotIt = false
	for guessNum in range(1, 10)
		if not possible then
			print "You have given me inconsistent information."
			print "Try again, and this time please be more careful."
			return false
		end if
		guessIdx = possible[rnd * possible.len]
		guessCombo = [0] * posCount
		if guessIdx > 0 then
			for x in range(0, guessIdx-1)
				incrementCombo guessCombo
			end for
		end if
		
		print "My guess is: " + comboToColorString(guessCombo)

		while true
			s = input("  Blacks, Whites? ").replace(",", " ").replace("  ", " ").split
			if s.len == 2 then break
		end while
		actualResult = [s[0].val, s[1].val]

		if actualResult[0] == posCount then
			print "I got it in " + guessNum + " moves!"
			gotIt = true
			break
		end if

		// Now zip through all possibilities, and if it's still in our 
		// possible list but doesn't match the given result, remove it.
		combo = [0] * posCount
		for x in range(0, possibilities-1)
			if x > 0 then incrementCombo combo
			idx = possible.indexOf(x)
			if idx == null then continue	// (already eliminated)
			result = calcResult(combo, guessCombo)
			if result != actualResult then
				//print "Eliminating #" + x + ", " + comboToColorString(combo)
				possible.remove idx
			end if
		end for
	end for
	
	if not gotIt then
		print "I used up all my moves!"
		print "I guess my CPU is just having an off day."
	end if
	globals.computerScore += guessNum
	return true
end function

// Show the score (with the given header).
showScore = function(header="Score")
	print header + ":"
	print "     COMPUTER " + computerScore
	print "     HUMAN    " + humanScore
	print
end function

// Initialization of global variables
colorCodes = "BWRGOYPT"
colorNames = "Black,White,Red,Green,Orange,Yellow,Purple,Tan".split(",")
computerScore = 0
humanScore = 0

// Main program
print " "*30 + "Mastermind"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
while true
	colorCount = input("Number of colors? ").val
	if 0 < colorCount <= 8 then break
	print "No more than 8, please!"
end while
posCount = input("Number of positions? ").val
roundCount = input("Number of rounds? ").val
possibilities = colorCount ^ posCount
print "Total possibilities = " + possibilities

print; print
print "Color    Letter"
print "=====    ======"
for x in range(0, colorCount-1)
	print pad(colorNames[x], 9) + colorCodes[x]
end for
print

for round in range(1, roundCount)
	print
	print "Round number " + round + " ----"
	print
	doHumanGuesses
	showScore
	while not doComputerGuesses; end while
	showScore
end for

print "GAME OVER"
showScore "Final score"
