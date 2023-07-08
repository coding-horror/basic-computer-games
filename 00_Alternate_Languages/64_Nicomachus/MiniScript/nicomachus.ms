// Nicomachus
// originally by David Ahl
// Ported from BASIC to MiniScript by Joe Strout, 2023

print " "*33 + "NICOMA"
print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print; print; print
print "Boomerang puzzle from Arithmetica of Nicomachus -- A.D. 90!"

// Get a yes/no (or at least y/n) response from the user.
askYesNo = function(prompt)
	while true
		answer = input(prompt)
		a1 = answer.lower[:1]
		if a1 == "y" or a1 == "n" then return a1
		print "Eh?  I don't understand '" + answer + "'  Try 'yes' or 'no'."
	end while
end function

doOne = function
	print
	print "Please think of a number between 1 and 100."
	A = input("Your number divided by 3 has a remainder of: ").val
	B = input("Your number divided by 5 has a remainder of: ").val
	C = input("Your number divided by 7 has a remainder of: ").val
	print
	print "Let me think a moment..."
	print
	wait 1.5
	D = 70*A + 21*B + 15*C
	D = D % 105   // gets the remainder after dividing by 105
	yesNo = askYesNo("Your number was " + D + ", right? ")
	if yesNo == "y" then
		print "How about that!"
	else
		print "I feel your arithmetic is in error."
	end if
end function

// Main loop -- press Control-C to break
while true
	doOne
	print
	print "Let's try another."
end while
