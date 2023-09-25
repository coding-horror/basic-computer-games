print " "*33 + "LIFE2"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print " "*10 + "U.B. LIFE GAME"

// N: counts neighbors and game state, as follows:
//    1's digit: player 1 neighbors
//   10's digit: player 2 neighbors
//  100's digit: player 1 current live cell
// 1000's digit: player 2 current live cell
N = []
for i in range(0,6); N.push [0]*7; end for

// K: encodes the rule for what cells come to life, based on
// the value in N.  The first 9 entries mean new life for Player 1;
// the second 9 entries mean new life for Player 2.
K = [3,102,103,120,130,121,112,111,12,
	 21,30,1020,1030,1011,1021,1003,1002,1012]

// population: how many live cells each player (1-2) has
population = [null, 0, 0]

// Function to get input coordinates from the player, for any empty space.
// Where possible, hide the input so the other player can't see it.
getCoords = function
	while true
		print "X,Y"
		inp = input.replace(",", " ").replace("  "," ")
		if version.hostName == "Mini Micro" then
			text.row = text.row + 1; print " "*60
		end if
		parts = inp.split
		if parts.len == 2 then
			x = parts[0].val
			y = parts[1].val
			if 0 < x <= 5 and 0 < y <= 5 and N[x][y] == 0 then break
		end if
		print "Illegal coords. Retype"
	end while
	return [x, y]
end function

// Function to print the board.  At the same time, it replaces
// any player 1 value (as judged by list K) with 100, and any
// player 2 value with 1000.  Also update population[] with the
// number of pieces of each player.
printBoard = function
	population[1] = 0
	population[2] = 0
	for y in range(0, 6)
		if y == 0 or y == 6 then
			print " 0  1  2  3  4  5  0"
		else
			print " " + y, "  "
			for x in range(1, 5)
				kIndex = K.indexOf(N[x][y])
				if kIndex == null then
					print " ", "  "
					N[x][y] = 0
				else if kIndex < 9 then
					print "*", "  "
					N[x][y] = 100
					population[1] += 1
				else
					print "#", "  "
					N[x][y] = 1000
					population[2] += 1
				end if
			end for
			print y
		end if
	end for
	print
end function

// Function to update the board (N).
updateBoard = function
	for j in range(1,5)
		for k in range(1,5)
			if N[j][k] < 100 then continue  // not a live cell
			if N[j][k] > 999 then value = 10 else value = 1
			for x in range(j-1, j+1)
				for y in range(k-1, k+1)
					if x == j and y == k then continue
					N[x][y] += value
					//if [x,y] == [2,1] then print "adding " + value + " from " + j+","+k + " to " + x+","+y + ", --> " + N[x][y]
				end for
			end for
		end for
	end for
end function


// Get initial player positions.
for player in [1,2]
	print; print "Player " + player + " - 3 live pieces."
	if player == 2 then value = 30 else value = 3
	for k in range(1,3)
		pos = getCoords
		N[pos[0]][pos[1]] = value
	end for
end for

printBoard
while true
	updateBoard
	printBoard
	if population[1] == 0 and population[2] == 0 then
		print "A DRAW"
		break
	else if population[1] == 0 then
		print "PLAYER 2 IS THE WINNER"
		break
	else if population[2] == 0 then
		print "PLAYER 1 IS THE WINNER"
		break
	end if
	
	print; print "Player 1 (*)"
	p1pos = getCoords
	print; print "Player 2 (#)"
	p2pos = getCoords
	if p1pos == p2pos then
		print "Same coord.  Set to 0"
	else
		N[p1pos[0]][p1pos[1]] = 100
		N[p2pos[0]][p2pos[1]] = 1000
	end if
end while