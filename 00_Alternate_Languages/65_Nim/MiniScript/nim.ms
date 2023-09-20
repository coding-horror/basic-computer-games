print " "*33 + "Nim"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

askYesNo = function(prompt)
	while true
		answer = input(prompt + "? ").lower
		if answer and answer[0] == "y" then return "yes"
		if answer and answer[0] == "n" then return "no"
		print "Please answer yes or no."
	end while
end function

print "This is the game of Nim."
if askYesNo("Do you want instructions") == "yes" then
	print "The game is played with a number of piles of objects."
	print "Any number of objects are removed from one pile by you and"
	print "the machine alternately.  On your turn, you may take"
	print "all the objects that remain in any pile, but you must"
	print "take at least one object, and you may take objects from"
	print "only one pile on a single turn.  You must specify whether"
	print "winning is defined as taking or not taking the last object,"
	print "the number of piles in the game, and how many objects are"
	print "originally in each pile.  Each pile may contain a"
	print "different number of objects."
	print "The machine will show its move by listing each pile and the"
	print "number of objects remaining in the piles after each of its"
	print "moves."
end if

allEmpty = function(piles)
	for i in range(1, piles.len-1)
		if piles[i] then return false
	end for
	return true
end function

// Do the computer's turn; return true if game over, false otherwise.
doComputerTurn = function(pile, winCondition)
	n = pile.len - 1
	d = [0,0,0]
	b = [null]
	for i in range(1, n)
		b.push [0]*11
	end for
	
	if winCondition == 2 then
		c = 0
		broke = false
		for i in range(1, n)
			if pile[i] == 0 then continue
			c += 1
			if c == 3 then; broke = true; break; end if
			d[c] = i
		end for
		if not broke then
			if c == 2 and (pile[d[1]] == 1 or pile[d[2]] == 1) then
				print "Machine wins"
				return true
			end if
			if pile[d[1]] > 1 then
				print "Machine wins"
				return true
			end if
			print "Machine loses"
			return true
		end if		
		c = 0
		broke = false
		for i in range(1, n)
			if pile[i] > 1 then; broke = true; break; end if
			if pile[i] == 0 then continue
			c += 1
		end for
		if not broke and c/2 != floor(c/2) then
			print "Machine loses"
			return true
		end if
	end if

	for i in range(1, n)
		e = pile[i]
		for j in range(0, 10)
			f = e/2
			b[i][j] = 2*(f-floor(f))
			e = floor(f)
		end for
	end for
	broke = false
	for j in range(10, 0)
		c = 0
		h = 0
		for i in range(1, n)
		if b[i][j] == 0 then continue
			c += 1
			if pile[i] <= h then continue
			h = pile[i]
			g = i
		end for
		if c/2 != floor(c/2) then; broke = true; break; end if
	end for
	if not broke then
		while true
			e = floor(n*rnd+1)
			if pile[e] != 0 then break
		end while
		f = floor(pile[e]*rnd+1)
		pile[e] -= f
	else
		pile[g] = 0
		for j in range(0, 10)
			b[g][j] = 0
			c=0
			for i in range(1,n)
				if b[i][j] == 0 then continue
				c += 1
			end for
			pile[g] += 2*(c/2-floor(c/2))*2^j
		end for
		if winCondition != 1 then
			c = 0
			broke = false
			for i in range(1, n)
				if pile[i]>1 then; broke = true; break; end if
				if pile[i] == 0 then continue
				c += 1
			end for
			if not broke and c/2 == floor(c/2) then pile[g]= 1 - pile[g]
		end if
	end if
	
	print "Pile  Size"
	for i in range(1, n)
		print i + "     " + pile[i]
	end for
	if winCondition == 1 and allEmpty(pile) then
		print "Machine wins"
		return true
	end if
	return false
end function

// Do the human player's turn; return true if game over, false otherwise.
doPlayerTurn = function(pile, winCondition)
	n = pile.len - 1
	while true
		inp = input("Your move - pile, number to be removed? ")
		inp = inp.replace(",", " ").replace("  ", " ").split
		if inp.len != 2 then continue
		x = inp[0].val; y = inp[1].val
		if x == floor(x) and y == floor(y) and 1 <= x <= n and 1 <= y <= pile[x] then break
	end while

	pile[x] -= y
	if allEmpty(pile) then
		if winCondition == 1 then print "Machine loses" else print "Machine wins"
		return true
	end if
	return false
end function

playOneGame = function
	print
	while true
		w = input("Enter win option - 1 to take last, 2 to avoid last? ").val
		if w == 1 or w == 2 then break
	end while
	while true
		n = input("Enter number of piles? ").val
		if n == floor(n) and 1 <= n <= 100 then break
	end while

	print "Enter pile sizes"
	pile = [null] + [0]*n	// (null at element 0 to make our array 1-based)
	for i in range(1, n)
		while true
			pile[i] = input(i + "? ").val
			if pile[i] == floor(pile[i]) and 1 <= pile[i] <= 2000 then break
		end while
	end for
	
	if askYesNo("Do you want to move first") == "yes" then
		if doPlayerTurn(pile, w) then return
	end if
	while true
		if doComputerTurn(pile, w) then return
		if doPlayerTurn(pile, w) then return
	end while
end function

while true
	playOneGame
	if askYesNo("Do you want to play another game") == "no" then break
end while
