print " "*32 + "Hamurabi"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "Try your hand at governing ancient Sumeria"
print "for a ten-year term of office."; print

eol = char(10)

game = {}
game.z = 0	// year
game.p = 95
game.s = 2800	// bushels in store
game.h = 3000
game.e = game.h - game.s	// bushels eaten by rats
game.food = 0	// bushels given to people to eat
game.y = 3		// value (in bushels) per acre
game.a = game.h / game.y	// acres owned
game.i = 5	// immigration/births
game.d = 0	// how many starved this year
game.d1 = 0	// total starved over the whole game
game.p1 = 0	// average % of population starved per year
game.q = 1	// if negative, then a plague strikes

startYear = function
	print; print; print "Hamurabi:  I beg to report to you,"
	game.z += 1
	print "In year " + game.z + ", " +
	  game.d + " people starved, " +
	  game.i + " came to the city,"
	game.p += game.i
	if game.q < 0 then
		game.p = floor(game.p / 2)
		print "A horrible plague struck!  Half the people died."
	end if
	print "Population is now " + game.p + "."
	print "The city now owns " + game.a + " acres."
	print "You harvested " + game.y + " bushels per acre."
	print "The rats ate " + game.e + " bushels."
	print "You now have " + game.s + " bushels in store."; print
end function

exitGame = function
	print; print char(7)*10
	print "So long for now."; print
	exit
end function

impeach = function
	print "Due to this extreme mismanagement you have not only"
	print "been impeached and thrown out of office but you have"
	print "also been declared national fink!!!!"
	exitGame
end function

getNumber = function(prompt, max, maxMsg)
	while true
		value = input(prompt + "? ").val
		if value < 0 then
			print; print "Hamurabi:  I cannot do what you wish."
			print "Get yourself another steward!"
			exitGame
		end if
		if value <= max then return value
		print "Hamurabi:  Think again.  " + maxMsg + "  Now then,"
	end while
end function

hint = function(msg)
	// This was not in the original program.  But if you want to make
	// the game easier, uncomment this line:
	print msg
end function

min = function(a, b, c)
	m = [a, b, c]
	m.sort
	return m[0]
end function

getDecisions = function
	// buy/sell land
	c = floor(10 * rnd); game.y = c + 17
	print "Land is trading at " + game.y + " bushels per acre."
	qty = getNumber("How many acres do you wish to buy",
		floor(game.s / game.y), "You have only" + eol + game.s + " bushels of grain.")
	if qty > 0 then
		game.a += qty
		game.s -= game.y * qty
	else
		qty = getNumber("How many acres do you wish to sell",
			game.a, "You own only" + eol + game.a + " acres.")
		game.a -= qty
		game.s += game.y * qty
	end if	
	
	// feed the people
	hint "Your people want " + (game.p * 20) + " bushels of food."
	game.food = getNumber("How many bushels do you wish to feed your people",
		game.s, "You have only" + eol + game.s + " bushels of grain.")
	game.s -= game.food
	
	// planting (a little more complicate because there are THREE limits)
	hint "You can plant up to " + 
	   min(game.a, floor(game.s * 2), floor(game.p*10-1)) + " acres."
	game.d = 0
	while game.a > 0 and game.s > 2
		game.d = getNumber("How many acres do you wish to plant with seed",
			game.a, "You own only " + game.a + " acres.")
		// enough grain for seed?  (each bushel can plant 2 acres)
		if floor(game.d / 2) > game.s then
			print "Hamurabi:  Think again.  You have only" + eol + game.s +
			  " bushels of grain.  Now then,"
			continue
		end if
		// enough people to tend the crops?  (each person can tend 10 acres)
		if game.d >= game.p * 10 then
			print "But you have only " + game.p + " people to tend the fields!  Now then,"
			continue
		end if
		break
	end while
	game.s -= floor(game.d / 2)
end function

simulateYear = function
	// A bountiful harvest!
	c = floor(rnd * 5) + 1
	game.y = c; game.h = game.d * game.y; game.e = 0	
	c = floor(rnd * 5) + 1
	if c % 2 == 0 then
		// rats are running wild!!
		game.e = floor(game.s / c)
	end if
	game.s += game.h - game.e
	
	// Let's have some babies
	c = floor(rnd * 5) + 1
	game.i = floor(c * (20 * game.a + game.s) / game.p / 100 + 1)
	// How many people had full tummies?
	c = floor(game.food / 20)
	// Horros, a 15% chance of plague
	game.q = floor(10 * (2 * rnd - 0.3))
	
	if game.p < c then
		game.d = 0
	else
		// starve enough for impeachment?
		game.d = game.p - c
		if game.d > 0.45 * game.p then
			print; print "You starved " + game.d + " people in one year!!!"
			impeach
		end if
		game.p1 = ((game.z - 1) * game.p1 + game.d * 100 / game.p) / game.z
		game.p = c
		game.d1 += game.d
	end if
end function

printFinalResult = function
	print "In your 10-year term of office, " + game.p1 + " percent of the"
	print "population starved per year on the average, i.e., a total of"
	print game.d1 + " people died!!"
	acresPer = game.a / game.p
	print "You started with 10 acres per person and ended with"
	print acresPer + " acres per person."; print
	if game.p1 > 33 or acresPer < 7 then impeach
	if game.p1 > 10 or acresPer < 9 then
		print "Your heavy-handed performance smacks of Nero and Ivan IV."
		print "The people (remaining) find you an unpleasant ruler, and,"
		print "frankly, hate your guts!!"
	else if game.p1 > 3 or acresPer < 10 then
		print "Your performance could have been somewhat better, but"
		print "really wasn't too bad at all.  " + floor(game.p * 0.8 * rnd) + " people"
		print "would dearly like to see you assassinated but we all have our"
		print "trivial problems."
	else
		print "A fantastic performance!!  Charlemange, Disraeli, and"
		print "Jefferson combined could not have done better!"
	end if
end function

// Main loop
while true
	startYear
	if game.z == 11 then break
	getDecisions
	simulateYear
end while
printFinalResult
exitGame
