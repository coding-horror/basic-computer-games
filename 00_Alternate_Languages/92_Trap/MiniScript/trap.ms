// TRAP
// STEVE ULLMAN, 8-1-72
// Ported to MiniScript by Ryushinaka and Joe Strout, 2023

print "TRAP"
print "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print ""

// constants:
G = 6    // number of guesses
N = 100  // range of numbers

// Get a yes/no (or at least y/n) response from the user.
askYesNo = function(prompt)
	while true
		answer = input(prompt).lower[:1]
		if answer == "y" or answer == "n" then return answer
	end while
end function

if askYesNo("Instructions? ") == "y" then
	print "I am thinking of a number between 1 and " + N
	print "Try to guess my number. On each guess, "
	print "you are to enter 2 numbers, trying to trap"
	print "my number between the two numbers. I will"
	print "tell you if you have trapped my number, if my"
	print "number is larger than your two numbers, or if"
	print "my number is smaller than your two numbers."
	print "If you want to guess one single number, type"
	print "your guess for both your trap numbers."
	print "You get " + G + " guesses to get my number."
	print
end if

doOneGame = function
	computers_number = ceil(N*rnd)
	
	for Q in range(1,G)	
		print ""
		while true
			guess = input("Guess #" + Q + ": ").replace(" ","")
			guess = guess.split(",")
			if guess.len == 2 then break
			print "Enter your guess like: 30,40"
		end while
		A = guess[0].val
		B = guess[1].val
		
		if A == computers_number and B == computers_number then
			print "You got it!!!"
			return
		else if A <= computers_number and B >= computers_number then
			print "You have trapped my number."
		else if A > computers_number and B > computers_number then
			print "My number is smaller than your trap numbers."				
		else if A < computers_number and B < computers_number then
			print "My number is larger than your trap numbers."
		end if
	end for
	print "Sorry, that's " + G + " guesses. The number was " + computers_number
end function

// main loop
while true
	print
	doOneGame
	print
	if askYesNo("Try Again? ") == "n" then break
end while
