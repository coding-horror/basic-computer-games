print " "*33 + "WAR"
print " "*15 + "CREATIVE COMPUTER  MORRISTOWN, NEW JERSEY"
print; print; print

print "This is the card game of War.  Each card is given by SUIT-#"
print "as S-7 for Spade 7."

// Get a yes/no (or at least y/n) response from the user.
askYesNo = function(prompt)
	while true
		answer = input(prompt + "? ").lower[:1]
		if answer == "y" or answer == "n" then return answer
		print "Answer yes or no, please."
	end while
end function

if askYesNo("Do you want directions") == "y" then
	print "The computer gives you and it a 'card'.  The higher card"
	print "(numerically) wins.  The game ends when you choose not to"
	print "continue or when you have finished the pack."
end if
print
print

cardValues = "2 3 4 5 6 7 8 9 10 J Q K A".split
deck = []
for suits in "SHCD"
	for value in cardValues
		deck.push suits + "-" + value
	end for
end for
deck.shuffle

playerScore = 0
computerScore = 0

while true
	m1 = deck.pop
	m2 = deck.pop
	print "You: " + m1 + "; Computer: " + m2
	n1 = cardValues.indexOf(m1[2:])
	n2 = cardValues.indexOf(m2[2:])
	if n1 > n2 then
		playerScore += 1
		print "You win. You have " + playerScore + " and the computer has " + computerScore
	else if n2 > n1 then
		computerScore += 1
		print "The computer wins!!! You have " + playerScore + " and the computer has " + computerScore
	else
		print "Tie.  No score change."
	end if
	if not deck then break
	if askYesNo("Do you want to continue") == "n" then break
end while

if not deck then
	print
	print
	print "We have run out of cards.  Final score: You: " + playerScore + 
	  "  The computer: " + computerScore
	print
end if
print "Thanks for playing.  It was fun."
print
		