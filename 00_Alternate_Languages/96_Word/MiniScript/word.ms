words = ["dinky", "smoke", "water", "grass", "train", "might", 
         "first", "candy", "champ", "would", "clump", "dopey"]

playGame = function
	secret = words[rnd * words.len]
	guesses = 0
	exact = ["-"]*5
	
	print "You are starting a new game..."
	while true
		guess = ""
		while guess == ""
			print
			guess = input("Guess a five letter word. ").lower
			if guess == "?" then break
			if guess.len != 5 then
				guess = ""
				print "You must guess a five letter word. Try again."
			end if
		end while
		guesses += 1
		
		if guess == "?" then
			print "The secret word is " + secret
			break
		else
			common = ""
			for i in range(0, 4)
				if secret.indexOf(guess[i]) != null then
					common += guess[i]
					if secret[i] == guess[i] then
						exact[i] = guess[i]
					end if
				end if
			end for
			print "There were " + common.len + " matches and the common letters were..." + common
			print "From the exact letter matches, you know"+"."*16 + exact.join("")
			
			if secret == guess or secret == exact.join("") then
				print "You have guessed the word. It took " + guesses + " guesses!"
				break
			else if common.len < 2 then
				print
				print "If you give up, type '?' for your next guess."
			end if
		end if
	end while
end function

print " " * 33 + "WORD"
print " " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print
print "I am thinking of a word -- you guess it. I will give you"
print "clues to help you get it. Good luck!"
print

playing = "y"
while playing == "y"
	playGame
	print
	playing = input("Want to play again? ") + " "
	playing = playing[0].lower
end while
