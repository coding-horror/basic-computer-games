print " "*34 + "Bullseye"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

if input("Do you want to see the instructions? (yes--1,no--0) ").val then
	print "This is a game in which you will be playing against the"
	print "random decision of the computer. The field of play is a"
	print "cube of size 3. Any of the locations can be designated"
	print "by inputing three numbers such as 2,3,1. At the start,"
	print "you are automatically at location 1,1,1. The object of"
	print "the game is to get to location 3,3,3. One minor detail:"
	print "the computer will pick, at random, locations at which"
	print "it will plant land mines. If you hit one of these locations"
	print "you lose. One other detail: you may move only one space "
	print "in one direction each move. For  example: from 1,1,2 you"
	print "may move to 2,1,2 or 1,1,3. You may not change"
	print "two of the numbers on the same move. If you make an illegal"
	print "move, you lose and the computer takes the money you may"
	print "have bet on that round."
	print
	print
	print "All yes or no questions will be answered by a 1 for yes"
	print "or a 0 (zero) for no."
	print
	print "When stating the amount of a wager, print only the number"
	print "of dollars (example: 250). You are automatically started with"
	print "500 dollars in your account."
	print
	print "Good luck!"
end if

money = 500

while money >= 0
	mineLocs = []
	// Note: unlike the original BASIC program, which doesn't actually pick random
	// numbers unless you manually set X to a positive value before running the 
	// program, we do pick five random mine locations here.
	// But like that program, we make no attempt to avoid 1,1,1 or 3,3,3, or to
	// ensure that we have picked five DIFFERENT locations.
	for i in range(1,5)
		mineLocs.push [floor(3 * rnd + 1), floor(3 * rnd + 1), floor(3 * rnd + 1)]
	end for
	wager = 0
	if input("Want to make a wager? ").val then
		wager = input("How much? ").val
		while money < wager
			wager = input("Tried to fool me; bet again? ").val
		end while
	end if
	position = [1,1,1]
	print
	inp = input("It's your move:  ")
	won = 0
	while true
		inp = inp.replace(",", " ").replace("  ", " ").split
		newPos = []
		if inp.len == 3 then newPos = [inp[0].val, inp[1].val, inp[2].val]
		legal = newPos.len == 3
		totalChange = 0
		for i in newPos.indexes
			// The original game allowed you to walk outside the 1-3 range,
			// thus safely avoiding all the mines.  To disallow this exploit,
			// uncomment the following line.
			//if newPos[i] < 1 or newPos[i] > 3 then legal = false
			totalChange += abs(newPos[i] - position[i])
		end for
		if totalChange > 1 then legal = false
		if not legal then
			print; print "Illegal move. You lose."
			won = -wager
			break
		end if
		if newPos == [3,3,3] then
			print "Congratulations!"
			won = wager
			break
		else if mineLocs.indexOf(newPos) != null then
			print "******BANG******"
			print "You lose!"
			print
			print
			won = -wager
			break
		end if
		position = newPos
		inp = input("Next move: ")
	end while
	if won != 0 then
		globals.money += won
		if money <= 0 then print "You bust."
	end if
	if wager then
		print " You now have " + money + " dollars."
	end if
	if not input("Do you want to try again? ").val then break
end while
print "Tough luck!"
print
print "Goodbye."
