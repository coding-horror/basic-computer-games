print " "*34 + "Hi Lo"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "This is the game of hi lo."; print
print "You will have 6 tries to guess the amount of money in the"
print "hi lo jackpot, which is between 1 and 100 dollars.  If you"
print "guess the amount, you win all the money in the jackpot!"
print "Then you get another chance to win more money.  However,"
print "if you do not guess the amount, the game ends."; print
total = 0
while true
	guesses=0
	print
	number=floor(100*rnd)
	while true
		guess = input("Your guess? ").val
		guesses += 1
		if guess < number then
			print "Your guess is too low."
		else if guess > number then
			print "Your guess is too high."
		else
			print "Got it!!!!!!!!!!   You win " + number + " dollars."
			total += number
			print "Your total winnings are now " + total + " dollars."
			break
		end if
		if guesses >= 6 then
			print "You blew it...too bad...the number was " + number
			total = 0
			break
		end if
	end while

	print
	yn = input("Play again (yes or no)?").lower
	if not yn or yn[0] != "y" then break
end while

print
print "So long.  Hope you enjoyed yourself!!!"
