CR = char(13)

print " "*32 + "WEEKDAY"
print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"+CR+CR+CR

print "This is a game called '23 Matches'."
print
print "When it is your turn, you may take one, two, or three"
print "matches. The object of the game is not to have to take"
print "the last match."
print
print "Let's flip a coin to see who goes first."
print "If it comes up heads, I will win the toss."
print
matches = 23
humanTurn = floor(rnd * 2)

if humanTurn then
	print "Tails! You go first."
	prompt = "How many do you wish to remove? "
else
	print "Heads! I win! Ha! Ha!"
	print "Prepare to lose, meatball-nose!!"
end if

choice = 2
while matches > 0
	if humanTurn then
		choice = 0
		if matches == 1 then choice = 1
		while choice == 0
			choice = input(prompt).val
			if choice <1 or choice > 3 or choice > matches then
				choice = 0
				print "Very funny! Dummy!"
				print "Do you want to play or goof around?"
				prompt = "Now, how many matches do you want? "
			end if
		end while
		matches = matches - choice
		if matches == 0 then
			print "You poor boob! You took the last match! I gotcha!!"
			print "Ha! Ha! I beat you !!"
			print "Good bye loser!"
		else
			print "There are now " + matches + " matches remaining." + CR
		end if
	else
		choice_comp = 4 - choice
		if matches == 1 then
			choice_comp = 1
		else if 1 < matches and matches < 4 then
			choice_comp = matches - 1
		end if
		matches = matches - choice_comp
		if matches == 0 then
			print "You won, floppy ears!"
			print "Think you're pretty smart!"
			print "Let's play again and I'll blow your shoes off!!"
		else
			print "My turn! I remove " + choice_comp + " matches"
			print "The number of matches is now " + matches + CR
		end if
	end if
	humanTurn = not humanTurn
	prompt = "Your turn -- you may take 1, 2 or 3 matches." + CR + "How many do you wish to remove? "
end while
