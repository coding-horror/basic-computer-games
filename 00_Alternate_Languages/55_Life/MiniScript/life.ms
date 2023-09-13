print " "*34 + "LIFE"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
import "listUtil"		// (needed for list.init2d)

maxx = 66		// (size adjusted for Mini Micro display)
maxy = 23
A = list.init2d(maxx+1, maxy+1, 0)

// Stuff the given pattern into the center of the cell array.
// Return the number of live cells.
stuffIntoCenter = function(pattern)
	maxLen = 0
	for p in pattern
		if p.len > maxLen then maxLen = p.len
	end for

	population = 0
	y = floor(maxy/2 - pattern.len/2)
	for row in pattern
		x = floor(maxx/2 - maxLen/2)
		for c in row
			if c != " " then
				A[x][y] = 1
				population += 1
			end if
			x += 1
		end for
		y += 1
	end for
	return population
end function

// Get the initial pattern from the user
initToUserPattern = function
	print "Enter your pattern (enter DONE when done):"
	userPattern = []
	while true
		p = input("?")
		if p.upper == "DONE" then break
		if p and p[0] == "." then p = " " + p[1:]
		userPattern.push p
	end while
	return stuffIntoCenter(userPattern)
end function

// For testing purposes, skip asking the user and just use a hard-coded pattern.
initToStandardPattern = function
	pattern = [
		"   **",
		"  * *",
		"    *"]
	return stuffIntoCenter(pattern)
end function

// Or, just for fun, initialize to a random pattern of junk in the center.
initRandom = function
	for x in range(ceil(maxx*0.3), floor(maxx*0.7))
		for y in range(ceil(maxy*0.3), floor(maxy*0.7))
			A[x][y] = rnd > 0.5
		end for
	end for
end function

// Define a function to draw the current game state.
// This also changes 2 (dying) to 0 (dead), and 3 (being born) to 1 (alive).
drawGameState = function(generation=0, population=0, invalid=false)
	if version.hostName == "Mini Micro" then text.row = 26 else print
	print "Generation: " + generation + "    Population: " + population +
	  "    " + "INVALID!" * invalid
	for y in range(0, maxy)
		s = ""
		for x in range(0, maxx)
			if A[x][y] == 2 then
				A[x][y] = 0
			else if A[x][y] == 3 then
				A[x][y] = 1
			end if
			if A[x][y] then s += "*" else s += " "
		end for
		print s
	end for
end function

// Update the game state, setting cells that should be born to 3 and 
// cells that should die to 2.  Return the number of cells that will
// be alive after this update.  Also, set globals.invalid if any live
// cells are found on the edge of the map.
updateGameState = function
	population = 0
	for x in range(0, maxx)
		for y in range(0, maxy)
			c = A[x][y] == 1 or A[x][y] == 2	// center state
			n = -c	// number of neighbors
			for nx in range(x-1, x+1)
				if nx < 0 or nx > maxx then continue
				for ny in range(y-1, y+1)
					if ny < 0 or ny > maxy then continue
					n += A[nx][ny] == 1 or A[nx][ny] == 2
				end for
			end for
			if c and n != 2 and n != 3 then	// live cell with < 2 or > 3 neighbors...
				A[x][y] = 2					// dies
			else if not c and n == 3 then	// dead cell with 3 neighbors...
				A[x][y] = 3 				// comes to life
				if x == 0 or x == maxx or y == 0 or y == maxy then
					globals.invalid = true
				end if
			end if
			population += (A[x][y] == 1 or A[x][y] == 3)
		end for
	end for
	return population
end function
	

// Initialize the game state (uncomment one of the following three lines)
population = initToUserPattern
//population = initToStandardPattern
//population = initRandom

// Main loop
if version.hostName == "Mini Micro" then clear
invalid = false
generation = 0
while population > 0
	drawGameState generation, population, invalid
	population = updateGameState
	generation += 1
	//key.get		// <-- Uncomment this to single-step with each keypress!
end while
drawGameState generation, population, invalid
