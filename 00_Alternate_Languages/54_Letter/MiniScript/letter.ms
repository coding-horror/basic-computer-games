// Letter Guessing Game
// Ported to MiniScript by Joe Strout, 2023

print " "*33 + "LETTER"
print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print; print; print

print "Letter Guessing Game"; print
print "I'll think of a letter of the alphabet, A to Z."
print "Try to guess my letter and I'll give you clues"
print "as to how close you're getting to my letter."

// Function to play one round of the game
playOneGame = function
	letter = char(65 + floor(rnd * 26))
	guesses = 0
	print; print "OK, I have a letter.  Start guessing."
	while true
		print; guess = input("What is your guess? ")[:1].upper
		guesses = guesses + 1
		if guess == letter then
			print; print "You got it in " + guesses + " guesses!!"
			if guesses > 5 then
				print "But it shouldn't take more than 5 guesses!"
			else
				print "Good job !!!!!"
				print char(7) * 15
			end if
			return
		else if guess < letter then
			print "Too low.  Try a higher letter."
		else
			print "Too high.  Try a lower letter."
		end if
	end while
end function

// main loop -- press Control-C to exit
while true
	print
	playOneGame
	print
	print "Let's play again....."
end while
