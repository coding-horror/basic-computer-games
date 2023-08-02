print " "*28 + "Game Of Even Wins"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print
print
yesNo = input("Do you want instructions (yes or no)? ").lower
if not yesNo or yesNo[0] != "n" then
	print "The game is played as follows:"
	print
	print "At the beginning of the game, a random number of chips are"
	print "placed on the board.  The number of chips always starts"
	print "as an odd number.  On each turn, a player must take one,"
	print "two, three, or four chips.  The winner is the player who"
	print "finishes with a total number of chips that is even."
	print "The computer starts out knowing only the rules of the"
	print "game.  It gradually learns to play well.  It should be"
	print "difficult to beat the computer after twenty games in a row."
	print "Try it!!!!"
	print
	print "To quit at any time, type a '0' as your move."
	print
end if

l = 0
b = 0
r = [[4]*6, [4]*6]

while true
	a = 0
	b = 0
	e = 0
	l = 0
	p = floor((13 * rnd + 9) / 2) * 2 + 1;
	while true
		if p == 1 then
			print "There is 1 chip on the board."
		else
			print "There are " + p + " chips on the board."
		end if
		e1 = e
		l1 = l
		e = a % 2
		l = p % 6
		if r[e][l] < p then
			m = r[e][l]
			if m <= 0 then
				m = 1
				b = 1
				break
			end if
			p -= m
			if m == 1 then
				prompt = "Computer takes 1 chip leaving " + p + "... Your move? "
			else
				prompt = "Computer takes " + m + " chips leaving " + p + "... Your move? "
			end if
			b += m
			while true
				m = input(prompt).val
				if m == 0 then break
				if 1 <= m <= p and m <= 4 then break
				prompt = m + " is an illegal move ... Your move? "
			end while
			if m == 0 then break
			if m == p then break		// <--  Really?  Before we've done the math?
			p -= m
			a += m
		else
			if p == 1 then
				print "Computer takes 1 chip."
			else
				print "Computer takes " + p + " chips."
			end if
			r[e][l] = p
			b += p
			break
		end if
	end while
	if m == 0 then break
	if b % 2 != 0 then
		print "Game over ... you win!!!"
		if r[e][l] != 1 then
			r[e][l] -= 1
		else if (r[e1][l1] != 1) then
			r[e1][l1] -= 1
		end if
	else
		print "Game over ... I win!!!"
	end if
	print
end while
