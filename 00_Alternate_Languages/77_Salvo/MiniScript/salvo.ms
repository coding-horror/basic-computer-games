print " "*33 + "Salvo"
print " "*15 + "Creative Computing  Morristown  New Jersey"
print; print; print

import "listUtil"

Ship = {}
Ship.name = "Ship"
Ship.positions = null		// list of [x,y] coordinates for this ship
Ship.shots = 1				// how many shots this ship provides
Ship.hitPoints = 1			// how many hits this ship can take before sinking
Ship.make = function(name, shots=1, size=2)
	result = new Ship
	result.name = name
	result.shots = shots
	result.positions = []
	result.hitPoints = size
	return result
end function

Board = {}
Board.ships = null			// list of Ship instances
Board.splash = null			// 2D array of: none, or turn on which it was hit
Board.shipAt = function(xy)
	for ship in self.ships
		if ship.positions.contains(xy) then return ship
	end for
end function
Board.isEmptySpot = function(xy)
	return 0 < xy[0] < 11 and 0 < xy[1] < 11 and self.shipAt(xy) == null
end function
Board.make = function
	result = new Board
	result.ships = []
	result.splash = list.init2d(11, 11)
	return result
end function
Board.totalShots = function
	sum = 0
	for ship in self.ships
		sum += ship.shots
	end for
	return sum
end function

computerBoard = Board.make
playerBoard = Board.make

directions = [[-1,-1], [-1,0], [-1,1], [0,-1], [0,1], [1,-1], [1,0], [1,1]]

randomPosition = function
	return [1 + floor(rnd*10), 1 + floor(rnd*10)]
end function

inputCoords = function(prompt="?")
	while true
		inp = input(prompt + "? ").replace(",", " ").replace("  ", " ").split
		if inp.len != 2 then
			print "Please enter coordinates such as: 5,3"
		else
			x = inp[0].val
			y = inp[1].val
			if 0 < x < 11 and 0 < y < 11 then return [x,y]
			print "X and Y coordinates must be in the range 1-10."
		end if
	end while
end function		

inBounds = function(pos)
	return 0 < pos[0] < 11 and 0 < pos[1] < 11
end function

placeComputerShips = function
	placeOne = function(ship)
		while true
			pos = randomPosition
			dir = directions.any
			ok = true
			p = pos[:]
			for i in range(0, ship.hitPoints - 1)
				if not computerBoard.isEmptySpot(p) then ok = false
				p.add dir
			end for
			if ok then break
		end while
		for i in range(0, ship.hitPoints - 1)
			ship.positions.push pos[:]
			pos.add dir
		end for
		computerBoard.ships.push ship
	end function
	placeOne Ship.make("Battleship", 3, 5)
	placeOne Ship.make("Cruiser", 2, 3)
	placeOne Ship.make("Destroyer<A>", 1, 2)
	placeOne Ship.make("Destroyer<B>", 1, 2)
end function

placePlayerShips = function
	placeOne = function(ship)
		print ship.name
		playerBoard.ships.push ship
		// Note: like the original BASIC program, we do no validation on
		// the input other than making sure it is in range.  So you can
		// have a ship scattered all over the map, have ships overlap, etc.		
		for i in range(1, ship.hitPoints)
			ship.positions.push inputCoords
		end for
	end function
	print "Enter coordinates for..."
	placeOne Ship.make("Battleship", 3, 5)
	placeOne Ship.make("Cruiser", 2, 3)
	placeOne Ship.make("Destroyer<A>", 1, 2)
	placeOne Ship.make("Destroyer<B>", 1, 2)
end function

printComputerShips = function
	for ship in computerBoard.ships
		print ship.name
		for pos in ship.positions
			print " " + pos.join("  ")
		end for
	end for
end function

doPlayerTurn = function(turnNum = 1)
	shots = playerBoard.totalShots
	print "You have " + shots + " shot" + "s"*(shots!=1) + "."
	if shots < 1 then
		print "I have won."
		exit
	end if
	hits = []
	for i in range(1, shots)
		while true
			pos = inputCoords
			prevHitOnTurn = computerBoard.splash[pos[0]][pos[1]]
			if prevHitOnTurn == null then break
			print "You shot there before on turn " + prevHitOnTurn
		end while
		computerBoard.splash[pos[0]][pos[1]] = turnNum
		hit = computerBoard.shipAt(pos)
		if hit then hits.push hit
	end for
	for hit in hits
		print "You hit my " + hit.name + "."
		hit.hitPoints -= 1
		if hit.hitPoints == 0 then
			print "...and sank it!"	// (not in original BASIC program)
			computerBoard.ships.removeVal hit
		end if
	end for
end function

pickShot = function
	// Pick a spot for the computer to shoot at.  We'll do this by
	// computing a "neighbor score" for each spot: the number of
	// neighboring spots that (1) we have previously hit, and (2)
	// contain an enemy ship.  Then we'll pick randomly from the
	// set of spots with the highest neighbor score.
	bestScore = 0
	spots = []
	for i in range(1,10)
		for j in range(1,10)
			pos = [i,j]
			if playerBoard.splash[pos[0]][pos[1]] then continue
			score = 0
			for dir in directions
				n = pos.plus(dir)
				if inBounds(n) and playerBoard.splash[n[0]][n[1]] and playerBoard.shipAt(n) then
					score += 1
				end if
			end for
			if score > bestScore then
				bestScore = score
				spots = [pos]
			else if score == bestScore then
				spots.push pos
			end if
		end for
	end for
	return spots.any
end function

doComputerTurn = function(turnNum = 1)
	shots = computerBoard.totalShots
	print "I have " + shots + " shot" + "s"*(shots!=1) + "."
	if shots < 1 then
		print "You have won."
		exit
	end if
	hits = []
	for i in range(1, shots)
		pos = pickShot
		playerBoard.splash[pos[0]][pos[1]] = turnNum
		hit = playerBoard.shipAt(pos)
		if hit then hits.push hit
		if seeComputerShots then print " " + pos.join("  ")
	end for
	for hit in hits
		print "I hit your " + hit.name + "."
		hit.hitPoints -= 1
		if hit.hitPoints == 0 then
			print "...and sank it!"	// (not in original BASIC program)
			playerBoard.ships.removeVal hit
		end if
	end for
end function

// Main Program

placeComputerShips
placePlayerShips
while true
	yn = input("Do you want to start? ").lower
	if yn == "where are your ships?" then
		printComputerShips
	else if yn and (yn[0] == "y" or yn[0] == "n") then
		break
	end if
end while
startWithPlayer = (yn[0] == "y")
while true
	yn = input("Do you want to see my shots? ").lower
	if yn and (yn[0] == "y" or yn[0] == "n") then break
end while
seeComputerShots = (yn[0] == "y")

turnNumber = 1
while true
	print
	print "Turn " + turnNumber
	if startWithPlayer then
		doPlayerTurn turnNumber
		doComputerTurn turnNumber
	else
		doComputerTurn turnNumber
		doPlayerTurn turnNumber
	end if
	turnNumber += 1
end while