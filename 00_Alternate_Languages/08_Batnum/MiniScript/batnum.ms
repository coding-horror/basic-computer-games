print " "*33 + "Batnum"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "This program is a 'Battle of Numbers' game, where the"
print "computer is your opponent."
print
print "The game starts with an assumed pile of objects. You"
print "and your opponent alternately remove objects from the pile."
print "Winning is defined in advance as taking the last object or"
print "not. You can also specify some other beginning conditions."
print "Don't use zero, however, in playing the game."
print "Enter a negative number for new pile size to stop playing."
print

options = {}
getOptions = function
	while true
		options.pileSize = input("Enter pile size? ").val
		if options.pileSize != 0 and options.pileSize == floor(options.pileSize) then break
	end while
	if options.pileSize < 0 then return

	while true
		winOption = input("Enter win option - 1 to take last, 2 to avoid last: ")
		if winOption == "1" or winOption == "2" then break
	end while
	options.takeLast = (winOption == "1")

	while true
		minMax = input("Enter min and max? ").replace(",", " ").split
		if minMax.len < 2 then continue
		options.minTake = minMax[0].val
		options.maxTake = minMax[-1].val
		if options.minTake >= 1 and options.minTake < options.maxTake then break
	end while

	while true
		startOpt = input("Enter start option - 1 computer first, 2 you first: ")
		if startOpt == "1" or startOpt == "2" then break
	end while
	options.computerFirst = (startOpt == "1")
end function

computerTurn = function
	// Random computer play (not in original program):
	take = options.minTake + floor(rnd * (options.maxTake - options.minTake))
	
	// Proper (smart) computer play
	q = pile
	if not options.takeLast then q -= 1
	take = q % (options.minTake + options.maxTake)
	if take < options.minTake then take = options.minTake
	if take > options.maxTake then take = options.maxTake
	
	if take >= pile then
		if options.takeLast then
			print "Computer takes " + pile + " and wins."
		else
			print "Computer takes " + pile + " and loses."
		end if
		globals.gameOver = true
	else
		globals.pile -= take
		print "Computer takes " + take + " and leaves " + pile
	end if
end function

playerTurn = function
	while true
		print; take = input("Your move? ").val
		if take == 0 then
			print "I told you not to use zero! Computer wins by forfeit."
			globals.gameOver = true
			return
		end if
		if options.minTake <= take <= options.maxTake and take <= pile then break
		print "Illegal move, reenter it"
	end while
	if take >= pile then
		if options.takeLast then
			print "Congratulations, you win."
		else
			print "Tough luck, you lose."
		end if
		globals.gameOver = true
	else
		globals.pile -= take
		//print "You take " + take + ", leaving " + pile	// (not in original program)
	end if
end function

while true
	getOptions
	if options.pileSize < 0 then break
	pile = options.pileSize
	gameOver = false
	
	print; print

	if options.computerFirst then computerTurn
	while not gameOver
		playerTurn
		if gameOver then break
		computerTurn
	end while
	
	for i in range(1,10); print; end for
end while
print "OK, bye!"