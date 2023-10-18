print " "*33 + "QUBIC"
print " "*15 + "Creative Computing  Morristown  New Jersey"
print; print; print

getYesNo = function(prompt)
	while true
		yn = input(prompt + "? ").lower + " "
		if yn[0] == "y" then return "yes"
		if yn[0] == "n" then return "no"
		print "Incorrect answer.  Please type 'yes' or 'no'"
	end while
end function

// Data defining "lines" as sets of board indexes which form 4-in-a-row:
ma = [null,
          [null, 1,2,3,4],    // 1
          [null, 5,6,7,8],    // 2
          [null, 9,10,11,12], // 3
          [null, 13,14,15,16],    // 4
          [null, 17,18,19,20],    // 5
          [null, 21,22,23,24],    // 6
          [null, 25,26,27,28],    // 7
          [null, 29,30,31,32],    // 8
          [null, 33,34,35,36],    // 9
          [null, 37,38,39,40],    // 10
          [null, 41,42,43,44],    // 11
          [null, 45,46,47,48],    // 12
          [null, 49,50,51,52],    // 13
          [null, 53,54,55,56],    // 14
          [null, 57,58,59,60],    // 15
          [null, 61,62,63,64],    // 16
          [null, 1,17,33,49], // 17
          [null, 5,21,37,53],    // 18
          [null, 9,25,41,57],   // 19
          [null, 13,29,45,61], // 20
          [null, 2,18,34,50], // 21
          [null, 6,22,38,54],    // 22
          [null, 10,26,42,58],  // 23
          [null, 14,30,46,62],   // 24
          [null, 3,19,35,51], // 25
          [null, 7,23,39,55],    // 26
          [null, 11,27,43,59],  // 27
          [null, 15,31,47,63], // 28
          [null, 4,20,36,52], // 29
          [null, 8,24,40,56], // 30
          [null, 12,28,44,60],    // 31
          [null, 16,32,48,64],    // 32
          [null, 1,5,9,13],   // 33
          [null, 17,21,25,29],    // 34
          [null, 33,37,41,45],    // 35
          [null, 49,53,57,61],    // 36
          [null, 2,6,10,14],  // 37
          [null, 18,22,26,30],    // 38
          [null, 34,38,42,46],    // 39
          [null, 50,54,58,62],    // 40
          [null, 3,7,11,15],  // 41
          [null, 19,23,27,31],    // 42
          [null, 35,39,43,47],    // 43
          [null, 51,55,59,63],    // 44
          [null, 4,8,12,16],  // 45
          [null, 20,24,28,32],    // 46
          [null, 36,40,44,48],    // 47
          [null, 52,56,60,64],    // 48
          [null, 1,6,11,16],  // 49
          [null, 17,22,27,32],    // 50
          [null, 33,38,43,48],    // 51
          [null, 49,54,59,64],    // 52
          [null, 13,10,7,4],  // 53
          [null, 29,26,23,20],    // 54
          [null, 45,42,39,36],    // 55
          [null, 61,58,55,52],    // 56
          [null, 1,21,41,61], // 57
          [null, 2,22,42,62], // 58
          [null, 3,23,43,63], // 59
          [null, 4,24,44,64], // 60
          [null, 49,37,25,13],    // 61
          [null, 50,38,26,14],    // 62
          [null, 51,39,27,15],    // 63
          [null, 52,40,28,16],    // 64
          [null, 1,18,35,52], // 65
          [null, 5,22,39,56], // 66
          [null, 9,26,43,60], // 67
          [null, 13,30,47,64],    // 68
          [null, 49,34,19,4], // 69
          [null, 53,38,23,8], // 70
          [null, 57,42,27,12],    // 71
          [null, 61,46,31,16],    // 72
          [null, 1,22,43,64], // 73
          [null, 16,27,38,49],    // 74
          [null, 4,23,42,61], // 75
          [null, 13,26,39,52]] // 76
          
// "opening book", i.e. spots that are generally good to hold if available
ya = [null, 1,49,52,4,13,61,64,16,22,39,23,38,26,42,27,43]

showBoard = function
	for i in range(1,9); print; end for
	for i in range(1, 4)
		print
		for j in range(1, 4)
			str = "   " * j
			for k in range(1, 4)
				q = 16 * i + 4 * j + k - 20
				if xa[q] == 5 then
					str += "(M)"
				else if xa[q] == 1 then
					str += "(Y)"
				else
					str += "( )"
				end if
				str += "      "
			end for
			print str
			// print	(makes display too tall for the screen)
		end for
		print
	end for
end function

clearFractions = function
	for i in range(1, 64)
		if xa[i] == 1/8 then xa[i] = 0
	end for
end function

checkForLines = function
	for s in range(1, 76)
		j1 = ma[s][1];
		j2 = ma[s][2];
		j3 = ma[s][3];
		j4 = ma[s][4];
		la[s] = xa[j1] + xa[j2] + xa[j3] + xa[j4]
	end for
end function

doPlayerMove = function
	clearFractions
	while true
		print
		inp = input("Your move? ")
		if inp == "0" then
			showBoard
			continue
		end if
		if inp == "1" then exit
		ok = true
		if inp.len != 3 then
			ok = false
		else
			i = inp[0].val
			j = inp[1].val
			k = inp[2].val
			ok = (0 < i < 5) and (0 < j < 5) and (0 < k < 5)
		end if
		if not ok then
			print "Incorrect move, retype it--"
		else
			m = 16 * i + 4 * j + k - 20
			if xa[m] != 0 then
				print "That square is used, try again."
			else
				xa[m] = 1
				break
			end if
		end if
	end while
end function

selectMove = function(lineIndex, valueToReplace)
    if lineIndex % 4 <= 1 then a = 1 else a = 2
    for j in range(a, 5 - a, 5 - 2 * a)
        if xa[ma[lineIndex][j]] == valueToReplace then
			xa[ma[lineIndex][j]] = 5
			m = ma[lineIndex][j]
			print "Machine takes " + squareName(m)
			return true
        end if
    end for
    return false
end function

doComputerMove = function
	// look for lines with two M's and two blanks; add 1/8 to the blank spots
	for i in range(1, 76)
		la[i] = xa[ma[i][1]] + xa[ma[i][2]] + xa[ma[i][3]] + xa[ma[i][4]]
		l = la[i]
		if l == 10 then
			for j in range(1, 4)
				if (xa[ma[i][j]] == 0) then xa[ma[i][j]] = 1/8
			end for
		end if
	end for
	// ...and if we now find lines containing 4 such spots, or 1 M and 3 such spots,
	// then pick one of those spots for our move
	checkForLines
	for i in range(1, 76)
		if la[i] == 4/8 or la[i] == 5 + 3/8 then
			selectMove i, 1/8
			return true
		end if
	end for

	// now look for lines containing 2 Y's, and mark (with 1/8) the blank spots
	clearFractions
	for i in range(1, 76)
		la[i] = xa[ma[i][1]] + xa[ma[i][2]] + xa[ma[i][3]] + xa[ma[i][4]]
		l = la[i]
		if l == 2 then
			for j in range(1, 4)
				if (xa[ma[i][j]] == 0) then xa[ma[i][j]] = 1 / 8
			end for
		end if
	end for
	// ...and again, if we find 4 such spots in a row, or 1 player move plus
	// 3 marked spots, then pick one of those
	checkForLines
	for i in range(1, 76)
		if la[i] == 4/8  or la[i] == 1 + 3/8 then
			selectMove 1/8
			return true
		end if
	end for

	// do mysterious stuff that depends on the order of lines in ma
	// in ways I don't understand
	for k in range(1, 18)
		p = 0
		for i in range(4 * k - 3, 4 * k)
			for j in range(1, 4)
				p += xa[ma[i][j]]
			end for
		end for
		if p == 4 or p == 9 then
			for i in range(4 * k - 3, 4 * k)
				if selectMove(1/8) then return true
			end for
			s = 0
		end if
	end for

	// look for certain "good" spots in our ya array that are still available
	clearFractions
	found = false
	for z in range(1, 17)
		if xa[ya[z]] == 0 then
			found = true
			break
		end if
	end for
	if not found then
		// getting desperate, look for any open spot
		for i in range(1, 64)
			if xa[i] == 0 then
				xa[i] = 5
				print "Machine likes " + squareName(i)
				return true
			end if
		end for
		print "The game is a draw."
		return false
	end if
	m = ya[z]
	xa[m] = 5
	print "Machine moves to " + squareName(m)
	return true
end function

squareName = function(m)
	k1 = floor((m - 1) / 16) + 1
	j2 = m - 16 * (k1 - 1)
	k2 = floor((j2 - 1) / 4) + 1
	k3 = m - (k1 - 1) * 16 - (k2 - 1) * 4
	return str(k1) + k2 + k3
end function

doOneGame = function
	globals.xa = [null] + [0] * 64	// board state
	globals.la = [null] + [0] * 76	// line data
	skipFirst = getYesNo("Do you want to move first") == "no"
	while true
		if skipFirst then
			skipFirst = false
		else
			doPlayerMove
		end if
		checkForLines
		machineDone = false
		// take three passes over the straight lines in the board
		for j in range(1, 3)
			for i in range(1, 76)
				// first pass: check for player win
				if j == 1 then
					if la[i] != 4 then continue;
					print "You win as follows" + " (line " + i + ")"
					for j in range(1, 4)
						print squareName(ma[i][j])
					end for
					return
				end if
				// second pass: check for machine able to win
				if j == 2 then
					if la[i] != 15 then continue;
					for j in range(1, 4)
						m = ma[i][j]
						if (xa[m] != 0) then continue;
						xa[m] = 5
						break
					end for
					print "Machine moves to " + squareName(m) + ", and wins as follows"
					for j in range(1, 4)
						print squareName(ma[i][j])
					end for
					return
				end if
				// third pass: check for player about to win
				if j == 3 then
					if la[i] != 3 then continue
					for j in range(1, 4)
						m = ma[i][j]
						if xa[m] != 0 then continue
						xa[m] = 5
						print "Nice try, machine moves to " + squareName(m)
						machineDone = true
					end for
					break
				end if
			end for
		end for
		if (machineDone) then continue

		if not doComputerMove then break
	end while
end function


// Main program
if getYesNo("Do you want instructions") == "yes" then
	print
	print "The game is tic-tac-toe in a 4 x 4 x 4 cube."
	print "Each move is indicated by a 3 digit number, with each"
	print "digit between 1 and 4 inclusive.  The digits indicate the"
	print "level, row, and column, respectively, of the occupied"
	print "place.  "
	print
	print "To print the playing board, type 0 (zero) as your move."
	print "The program will print the board with your moves indi-"
	print "cated with a (Y), the machine's moves with an (M), and"
	print "unused squares with a ( )." // "output is on paper."
	print
	print "To stop the program run, type 1 as your move."
	print
	print
end if
while true
	doOneGame
	print
	if getYesNo("Do you want to try another game") == "no" then break
end while



