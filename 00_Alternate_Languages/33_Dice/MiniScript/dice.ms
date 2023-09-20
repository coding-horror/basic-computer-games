// Dice
//
// Danny Freidus
// Ported from BASIC to MiniScript by Joe Strout

print " "*34 + "DICE"
print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print; print; print
print "This program simulates the rolling of a"
print "pair of dice."
print "You enter the number of times you want the computer to"
print "'roll' the dice.  Watch out, very large numbers take"
print "a long time.  In particular, numbers over 5000."

// Function to do one run of the simulation.
runOnce = function
	// Clear the array we'll use to hold the counts
	counts = [0] * 13
	// Loop the desired number of times
	x = input("How many rolls? ").val
	for s in range(1, x)
		// roll two dice and find the sum
		die1 = ceil(6 * rnd)
		die2 = ceil(6 * rnd)
		sum = die1 + die2
		// update the count for that sum
		counts[sum] += 1
	end for
	print
	
	// print a table showing how many times each sum was rolled
	print "Total Spots   Number of Times"
	for v in range(2, 12)
		// (the [-6:] trick below right-aligns the number)
		print ("     " + v)[-6:] + " "*10 + counts[v]
	end for
	print; print
end function

// Get a yes/no (or at least y/n) response from the user.
askYesNo = function(prompt)
	while true
		answer = input(prompt).lower[:1]
		if answer == "y" or answer == "n" then return answer
	end while
end function

// main loop
while true
	print
	runOnce	
	if askYesNo("Try again? ") == "n" then break
end while
