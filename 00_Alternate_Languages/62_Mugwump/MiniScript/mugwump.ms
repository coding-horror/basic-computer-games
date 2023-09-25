print " "*33 + "Mugwump"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
// Courtesy People's Computer Company
print "The object of this game is to find four mugwumps"
print "hidden on a 10 by 10 grid.  Homebase is position 0,0."
print "Any guess you make must be two numbers with each"
print "number between 0 and 9, inclusive.  First number"
print "is distance to right of homebase and second number"
print "is distance above homebase."
print
print "You get 10 tries.  After each try, I will tell"
print "you how far you are from each mugwump."
print

playOneGame = function
	mugwump = {}	// key: number 1-4; value: [x,y] position
	for i in range(1, 4)
		mugwump[i] = [floor(10*rnd), floor(10*rnd)]
	end for
	
	found = 0
	for turn in range(1, 10)
		print
		print
		while true
			inp = input("Turn no. " + turn + " -- what is your guess? ")
			inp = inp.replace(",", " ").replace("  ", " ").split
			if inp.len == 2 then break
		end while
		x = inp[0].val; y = inp[1].val
		for i in range(1, 4)
			pos = mugwump[i]
			if pos == null then continue	// (already found)
			if pos == [x,y] then
				print "You have found mugwump " + i
				mugwump[i] = null
				found += 1
			else
				d = sqrt( (pos[0] - x)^2 + (pos[1] - y)^2 )
				print "You are " + round(d, 1) + " units from mugwump " + i
			end if
		end for
		if found == 4 then
    		print
			print "You got them all in " + turn + " turns!"
			return
		end if
	end for
	print
	print "Sorry, that's 10 tries.  Here is where they're hiding:"
	for i in range(1, 4)
		pos = mugwump[i]
		if pos == null then continue
		print "Mugwump " + i + " is at (" + pos[0] + "," + pos[1] + ")"
	end for
end function

// Main loop
while true
	playOneGame
	print
	print "That was fun! Let's play again......."
	print "Four more mugwumps are now in hiding."
end while
