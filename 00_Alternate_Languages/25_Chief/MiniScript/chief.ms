print " "*30 + "Chief"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "I am chief Numbers Freek, the great Indian math god."
yn = input("Are you ready to take the test you called me out for? ").lower
if not yn or yn[0] != "y" then
	print "Shut up, pale face with wise tongue."
end if
print " Take a number and add 3.  Divide this number by 5 and"
print "multiply by 8. Divide by 5 and add the same. Subtract 1."
b = input("  What do you have? ").val
c = (b+1-5)*5/8*5-3
yn = input("I bet your number was " + c + ". Am I right? ").lower
if yn and yn[0] == "y" then
	print "Bye!!!"
else
	k = input("What was your original number? ").val
	f=k+3
	g=f/5
	h=g*8
	i=h/5+5
	j=i-1
	print "So you think you're so smart, eh?"
	print "Now watch."
	print k + " plus 3 equals " + f +". This divided by 5 equals " + g + ";"
	print "this times 8 equals " + h + ". If we divide by 5 and add 5,"
	print "we get " + i + ", which, minus 1, equals " + j + "."
	yn = input("Now do you believe me? ").lower
	if yn and yn[0] == "y" then
		print "Bye!!!"
	else
		print "You have made me mad!!!"
		print "There must be a great lightning bolt!"
		print; print; wait 2
		for x in range(30, 22)
			print " "*x + "x x"; wait 0.1
		end for
		print " "*21 + "x xxx"; wait 0.1
		print " "*20 + "x   x"; wait 0.1
		print " "*19 + "xx x"; wait 0.1
		for y in range(20, 13)
			print " "*y + "x x"; wait 0.1
		end for
		print " "*12 + "xx"; wait 0.1
		print " "*11 + "x"; wait 0.1
		print " "*10 + "*"; wait 0.1
		print; print"#########################"; print
		print "I hope you believe me now, for your sake!!"
	end if
end if
