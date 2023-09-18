kMaxNum = 100
kTries = 7

instructions = function
	print "I am thinking of a whole number from 1 to " + kMaxNum
	print "Try to guess my number. After you guess, I"
	print "will output one or more stars (*). The more"
	print "stars I type, the closer you are to my number."
	print "One star (*) means far away, seven stars (*******)"
	print "means really close! You get " + kTries + " guesses."
	print
end function

print " " * 34 + "Stars"
print " " * 15 + "Creative Computing  Morristown, New Jersey"
print; print; print

ans = input("Do you want instructions? ").lower + " "
if ans[0] == "y" then
	instructions
end if

while true
	print
	print "OK, I am thinking of a number, start guessing."
	starNum = floor(rnd * kMaxNum) + 1
	try = 0
	while try < kTries
		print
		guess = input("Your guess: ").val
		
		if guess == starNum then
			break
		else
			d = abs(guess - starNum)
			print "*" * (7 - floor(log(d,2)))
		end if
		try += 1
	end while
	
	if try < kTries then
		print "*" * 59
		print "You got it in " + (try + 1) + " guesses! Let's play again."
	else
		print "Sorry, that's " + try + " guesses. The number was " + starNum
	end if
	print
end while
