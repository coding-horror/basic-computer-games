print " "*32 + "Hangman"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

words = []
words += ["gum","sin","for","cry","lug","bye","fly"]
words += ["ugly","each","from","work","talk","with","self"]
words += ["pizza","thing","feign","fiend","elbow","fault","dirty"]
words += ["budget","spirit","quaint","maiden","escort","pickax"]
words += ["example","tension","quinine","kidney","replica","sleeper"]
words += ["triangle","kangaroo","mahogany","sergeant","sequence"]
words += ["moustache","dangerous","scientist","different","quiescent"]
words += ["magistrate","erroneously","loudspeaker","phytotoxic"]
words += ["matrimonial","parasympathomimetic","thigmotropism"]
// Note: on Mini Micro, you could instead do:
// words = file.readLines("/sys/data/englishWords.txt")

words.shuffle

addToPic = function(guessCount)
	if guessCount == 1 then
		print "First, we draw a head"
		ps[3][6]="-"; ps[3][7]="-"; ps[3][8]="-"; ps[4][5]="("; ps[4][6]="."
		ps[4][8]="."; ps[4][9]=")"; ps[5][6]="-"; ps[5][7]="-"; ps[5][8]="-"
	else if guessCount == 2 then
		print "Now we draw a body."
		for i in range(6, 9); ps[i][7]="x"; end for
	else if guessCount == 3 then
		print "Next we draw an arm."
		for i in range(4, 7); ps[i][i-1]="\"; end for
	else if guessCount == 4 then
		print "This time it's the other arm."
		ps[4][11]="/"; ps[5][10]="/"; ps[6][9]="/"; ps[7][8]="/"
	else if guessCount == 5 then
		print "Now, let's draw the right leg."
		ps[10][6]="/"; ps[11][5]="/"
	else if guessCount == 6 then
		print "This time we draw the left leg."
		ps[10][8]="\"; ps[11][9]="\"
	else if guessCount == 7 then
		print "Now we put up a hand."
		ps[3][11]="\"
	else if guessCount == 8 then
		print "Next the other hand."
		ps[3][3]="/"
	else if guessCount == 9 then
		print "Now we draw one foot"
		ps[12][10]="\"; ps[12][11]="-"
	else if guessCount == 10 then
		print "Here's the other foot -- you're hung!!"
		ps[12][3]="-"; ps[12][4]="/"
	end if
	for i in range(1, 12)
		print ps[i].join("")
	end for
	print
end function

doOneGame = function
	usedLetters = []
	globals.ps = []
	for i in range(0, 12); ps.push [" "]*12; end for
	for i in range(1,12); ps[i][1] = "X"; end for
	for i in range(1, 7); ps[1][i] = "X"; end for; ps[2][7] = "X"
	secretWord = words.pull.upper
	print "(Secret word: " + secretWord + ")"
	visibleWord = ["-"] * secretWord.len
	wrongGuesses = 0
	while true
		print "Here are the letters you used:"
		print usedLetters.join(",")
		print
		print visibleWord.join("")
		print
		guess = input("What is your guess? ").upper
		guess = (guess + " ")[0]
		if guess < "A" or guess > "Z" then continue
		if usedLetters.indexOf(guess) != null then
			print "You guessed that letter before!"
			continue
		end if
		usedLetters.push guess
		for i in visibleWord.indexes
			if secretWord[i] == guess then visibleWord[i] = guess
		end for
		if visibleWord.indexOf("-") == null then
			print "You found the word!"
			return true
		else if secretWord.indexOf(guess) != null then
			print
			print visibleWord.join("")
			print
			guess = input("What is your guess for the word? ").upper
			if guess == secretWord then
				print "Right!! It took you " + usedLetters.len + " guesses!"
				return true
			else
				print "Wrong.  Try another letter."
			end if
			print
		else
			print "Sorry, that letter isn't in the word."
			wrongGuesses += 1
			addToPic wrongGuesses
			if wrongGuesses > 9 then
				print "Sorry, you lose.  The word was " + secretWord
				return false
			end if
		end if
	end while
end function

while true
	if not words then
		print "You did all the words!!"
		break
	end if
	won = doOneGame
	if won then
		yn = input("Want another word? ").upper
	else
		yn = input("You missed that one.  Do you want another word? ").upper
	end if
	if not yn or yn[0] != "Y" then break
end while
print
print "It's been fun!  Bye for now."
