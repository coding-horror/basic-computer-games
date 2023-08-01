import "listUtil"

print " "*33 + "Digits"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

printInColumns = function(a, b, c, d)
	print (a+" "*16)[:16] + (b+" "*16)[:16] + (c+" "*16)[:16] + (d+" "*16)[:16]
end function

print "This is a game of guessing."
print "For instructions, type '1', else type '0'"
if input != "0" then
	print
	print "Please take a piece of paper and write down"
	print "the digits '0', '1', or '2' thirty times at random."
	print "Arrange them in three lines of ten digits each."
	print "I will ask for then ten at a time."
	print "I will always guess them first and then look at your"
	print "next number to see if i was right. By pure luck,"
	print "I ought to be right ten times. But i hope to do better"
	print "than that *****"
	print; print
end if

a = 0; b = 1; c = 3

while true
	m = list.init2d(27, 3, 1)
	k = list.init2d(3, 3, 9)
	l = list.init2d(9, 3, 3)
	l[0][0] = 2; l[4][1] = 2; l[8][2] = 2
	z=26; z1=8; z2=2
	qtyRight = 0
	guess = 0
	for t in range(1,3)
		while true
			print
			print "Ten numbers, please";
			n = input.replace(",", " ").replace("  ", "").split
			if n.len != 10 then continue
			valid = true
			for i in n.indexes
				n[i] = n[i].val
				if n[i] < 0 or n[i] > 2 then
					print "Only use the digits '0', '1', or '2'."
					print "Let's try again."
					valid = false
					break
				end if
			end for
			if valid then break
		end while
	
		print; printInColumns "My guess","Your no.","Result","No. right"; print
		for u in range(0, 9)
			yourNum = n[u]; s=0
			for j in range(0,2)
				s1 = a*k[z2][j] + b*l[z1][j] + c*m[z][j]
				if s > s1 then continue
				if s < s1 or rnd >= 0.5 then
					s = s1; guess = j
				end if
			end for
			if guess == yourNum then
				outcome = " right"
				qtyRight += 1
			else
				outcome = " wrong"
			end if
			printInColumns "  "+guess, "   " + yourNum, outcome, qtyRight

			m[z][yourNum] += 1
			l[z1][yourNum] += 1
			k[z2][yourNum] += 1
			z -= floor(z/9)*9
			z = 3*z + yourNum
			z1 = z - floor(z/9)*9
			z2 = yourNum
		end for
	end for

	print
	if qtyRight > 10 then
		print "I guessed more than 1/3 of your numbers."
		print "I win."
		print char(7) * 10
	else if qtyRight < 10 then
		print "I guessed less than 1/3 of your numbers."
		print "You beat me.  Congratulations *****"
	else
		print "I guessed exactly 1/3 of your numbers."
		print "It's a tie game."
	end if
	print "Do you want to try again (1 for yes, 0 for no)";
	if input != "1" then break
end while

print; print "Thanks for the game."
