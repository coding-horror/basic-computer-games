setup = function
	print " "*33 + "GUESS"
	print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
	print; print; print
	print "This is a number guessing game. I'll think"
	print "of a number between 1 and any limit you want."
	print "Then you have to guess what it is."
	print
	while true
		globals.limit = input("What limit do you want? ").val
		if limit > 1 then break
		print "Please enter a number greater than 1."
	end while
	globals.par = floor(log(limit, 2)) + 1
end function

printGap = function
	for i in range(1, 5)
		print
	end for
end function

doOneGame = function
	rightAnswer = ceil(rnd * limit)
	print "I'm thinking of a number between 1 and " + limit
	print "Now you try to guess what it is."
	guess = 0
	while true
		guess = guess + 1
		num = input("Your guess: ").val
		if num <= 0 then
			printGap
			setup
			return
		end if
		if num == rightAnswer then
			print "That's it! You got it in " + guess + " tries."
			if guess < par then
				print "Very good."
			else if guess == par then
				print "Good."
			else
				print "You should have been able to get it in only " + par + "."
			end if
			printGap
			return
		end if
		if num > rightAnswer then
			print "Too high. Try a smaller answer."
		else
			print "Too low. Try a bigger answer."
		end if
	end while
end function

setup
while true
	doOneGame
end while