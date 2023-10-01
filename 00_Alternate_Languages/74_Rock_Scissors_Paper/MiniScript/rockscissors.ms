print " "*21 + "Game of Rock, Scissors, Paper"
print " "*15 + "Creative Computing  Morristown  New Jersey"
print; print; print

while true
	numGames = input("How many games? ").val
	if 0 < numGames < 11 then break
	print "Sorry, but we aren't allowed to play that many."
end while

computerWins = 0
playerWins = 0
for game in range(1, numGames)
	print; print "Game number " + game
	myChoice = floor(rnd*3 + 1)
	while true
		print "3=Rock...2=Scissors...1=Paper"
		playerChoice = input("1...2...3...What's your choice? ").val
		if [1,2,3].indexOf(playerChoice) != null then break
		print "Invalid."
	end while
	print "This is my choice..."
	print ["...Paper", "...Scissors", "...Rock"][myChoice-1]
	diff = myChoice - playerChoice
	if diff == 0 then
		print "Tie game.  No winner."
	else if diff == 1 or diff == -2 then
		print "Wow!  I win!!!"
		computerWins += 1
	else
		print "You win!!!"
		playerWins += 1
	end if
end for

print; print "Here is the final game score:"
print "I have won " + computerWins + " game(s)."
print "You have won " + playerWins + " game(s)."
print "And " + (numGames - computerWins - playerWins) + " game(s) ended in a tie."
print; print "Thanks for playing!!"
