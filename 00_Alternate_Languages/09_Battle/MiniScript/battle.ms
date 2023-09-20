print " "*33 + "Battle"
print " "*15 + "Creative Computing  Morristown, New Jersey"
// -- BATTLE WRITTEN BY RAY WESTERGARD  10/70
// COPYRIGHT 1971 BY THE REGENTS OF THE UNIV. OF CALIF.
// PRODUCED AT THE LAWRENCE HALL OF SCIENCE, BERKELEY
// Converted to MiniScript by Joe Strout, July 2023.

import "listUtil"

setup = function
	// prepare the shield with the bad guys' ships
	globals.field = list.init2d(6,6, 0)	// matrix of enemy ships -- 0th row/column unused
	for shipType in [1,2,3]
		for ship in [1,2]
			while not addShip(shipType, ship)
				// keep trying until we successfully add it!
			end while
		end for
	end for
	
	// prepare another matrix to keep track of where we have hit
	globals.hits = list.init2d(6,6, 0)
	
	// and some some info per ship (ID) and ship type
	globals.shipHitsLeft = [null, 2, 2, 3, 3, 4, 4]	// hits left till each ship is sunk
	globals.sunkPerType = [0] * 3	// counts how many of each type were sunk
	
	// finally, keep track of hits and splashes
	globals.splashCount = 0
	globals.hitCount = 0
end function

// Try to add the given ship to field.
// Return true on success, false on failure.
addShip = function(shipType, shipNum)
	size = 5 - shipType
	x = floor(rnd * 6)
	y = floor(rnd * 6)
	direction = floor(rnd * 4)
	if direction == 0 then
		dx = 1; dy = 0
	else if direction == 1 then
		dx = 1; dy = 1
	else if direction == 2 then
		dx = 0; dy = 1
	else
		dx = -1; dy = 1
	end if
	// First make sure our placement doesn't go out of bounds
	if not (0 <= x + dx * size <= 5) then return false
	if not (0 <= y + dy * size <= 5) then return false
	// Then, make sure it's not blocked by another ship
	for i in range(0, size-1)
		px = x + dx * i	// (point under consideration)
		py = y + dy * i
		// make sure where we want to place the ship is still empty
		if field[px][py] then return false
		// if placing a ship diagonally, don't allow it to cross
		// another ship on the other diagonal
		if i > 0 and dx and dy then
			adjacent1 = field[px-dx][py]
			adjacent2 = field[px][py-dy]
			if adjacent1 and adjacent1 == adjacent2 then return false
		end if
	end for
	// Looks like it's all clear, so fill it in!
	id = 9 - 2 * shipType - shipNum;
	for i in range(0, size-1)
		field[x + dx * i][y + dy * i] = id
	end for
	return true
end function

// Print the "encoded" fleet disposition.  This is just the regular field,
// but rotated and flipped.
printField = function
	print
	print "The following code of the bad guys' fleet disposition"
	print "has been captured but not decoded:"
	print
	for i in range(0,5)
		for j in range(0,5)
			print field[5-j][i], " "
		end for
		print
	end for
end function

doOneTurn = function
	while true
		xy = input("??").replace(",", " ")
		if xy == "?" then; printField; continue; end if	// (not in original game)
		x = xy[0].val; y = xy[-1].val
		if xy.len < 2 or x != floor(x) or x < 1 or x > 6 or y != floor(y) or y < 1 or y > 6 then
			 print "Invalid input.  Try again."
			 continue
		end if
		break
	end while
	row = x - 1	// (minus one since our matrix is 0-based instead of 1-based)
	col = y - 1
	shipId = field[row][col]
	if shipId == 0 then
		// fall through to 'end if' below
	else if shipHitsLeft[shipId] == 0 then
		print "There used to be a ship at that point, but you sank it."
	else if hits[row][col] then
		print "You already put a hole in ship number " + shipId + " at that point."
	else
		hits[row][col] = shipId
		print "A direct hit on ship number " + shipId
		globals.hitCount += 1
		shipHitsLeft[shipId] -= 1
		if shipHitsLeft[shipId] == 0 then
			shipType = floor((shipId-1) / 2)  // get ship type, 0-2
			sunkPerType[shipType] += 1
			print "And you sunk it.  Hurray for the good guys."
			print "So far, the bad guys have lost"
			print sunkPerType[0] + " destroyer(s), " + sunkPerType[1] + 
			  " cruiser(s), and " + sunkPerType[2] + " aircraft carrier(s)."
			print "Your current splash/hit ratio is " + (splashCount / hitCount)
		end if
		return
	end if
	print "Splash!  Try again."
	globals.splashCount += 1
end function

playOneGame = function
	setup
	printField
	print
	print "De-code it and use it if you can"
	print "but keep the de-coding method a secret."
	print
	print "START GAME"
	
	while true
		doOneTurn
		if sunkPerType.sum == 6 then break
	end while
	
	print
	print "You have totally wiped out the bad guys' fleet"
	print "with a final splash/hit ratio of " + (splashCount / hitCount)
	if splashCount == 0 then
		print "Congratulations -- a direct hit every time."
	end if
end function

// Main loop
while true
	playOneGame
	print
	print "****************************"
	print
end while
