print " "*31 + "Digits"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print

y1 = 0
m1 = 0
print "     This is a two person game called 'Even Wins.'"
print "To play the game, the players need 27 marbles or"
print "other objects on a table."
print
print
print "     The 2 players alternate turns, with each player"
print "removing from 1 to 4 marbles on each move.  The game"
print "ends when there are no marbles left, and the winner"
print "is the one with an even number of marbles."
print
print
print "     The only rules are that (1) you must alternate turns,"
print "(2) you must take between 1 and 4 marbles each turn,"
print "and (3) you cannot skip a turn."
print
print
print
while true
	print "     Type a '1' if you want to go first, and type"
	print "a '0' if you want me to go first."
	c = input.val
	print
	if c != 0 then
		t = 27
		print
		print
		print
		print "Total=" + t 
		print
		print
		print "What is your first move?"
		m = 0
	else
		t = 27
		m = 2
		print
		print "Total= " + t
		print
		m1 += m
		t -= m
	end if
	while true
		if m then
			print "I pick up " + m + " marbles."
			if t == 0 then break
			print
			print "Total=" + t
			print
			print "     And what is your next move, my total is " + m1
		end if
		while true
			y = input.val
			print
			if y < 1 or y > 4 then
				print
				print "The number of marbles you must take be a positive"
				print "integer between 1 and 4."
				print
				print "     What is your next move?"
				print
			else if y > t then
				print "     You have tried to take more marbles than there are"
				print "left.  Try again."
			else
				break
			end if
		end while

		y1 += y
		t -= y
		if t == 0 then break
		print "Total=" + t
		print
		print "Your total is " + y1
		if t < 0.5 then break
		r = t % 6
		if y1 % 2 != 0 then
			if t >= 4.2 then
				if r <= 3.4 then
					m = r + 1
					m1 += m
					t -= m
				else if r < 4.7 or r > 3.5 then
					m = 4
					m1 += m
					t -= m
				else
					m = 1
					m1 += m
					t -= m
				end if
			else
				m = t
				t -= m
				print "I pick up " + m + " marbles."
				print
				print "Total = 0"
				m1 += m
				break
			end if
		else
			if r < 1.5 or r > 5.3 then
				m = 1
				m1 += m
				t -= m
			else
				m = r - 1
				m1 += m
				t -= m
				if t < 0.2 then
					print "I pick up " + m + " marbles."
					print
					break
				end if
			end if
		end if
	end while
	print "That is all of the marbles."
	print
	print " My total is " + m1 + ", your total is " + y1
	print
	if m1 % 2 then
		print "     You won.  Do you want to play"
	else
		print "     I won.  Do you want to play"
	end if
	print "again?  Type 1 for yes and 0 for no."
	a1 = input.val
	if a1 == 0 then break
	m1 = 0
	y1 = 0
end while
print
print "OK.  See you later"
