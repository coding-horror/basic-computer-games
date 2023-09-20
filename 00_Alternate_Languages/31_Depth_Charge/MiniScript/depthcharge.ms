// Depth Charge
// Originally by Dana Noftle (1978)
// Translated from BASIC to MiniScript by Ryushinaka and Joe Strout (2023)

askYesNo = function(prompt)
	while true
		answer = input(prompt).lower[:1]
		if answer == "y" or answer == "n" then return answer
	end while
end function

showWelcome = function
	print
	print " "*34 + "Depth Charge"
	print " "*15 + "Creative Computing Morristown, New Jersey"
	print
end function

setup = function
	while true
		globals.size = input("Dimension of search area? ").val
		if size > 0 then break
	end while

	globals.numCharges = ceil(log(size,2))
	if numCharges == 0 then globals.numCharges = 1  // ensure we have at least 1 shot

	print "You are the captain of the destroyer USS Computer."
	print "An enemy sub has been causing you trouble. Your"	
	print "mission is to destroy it. You have " + numCharges + " shots."
	print "Specify depth charge explosion point with a"
	print "trio of numbers -- the first two are the"
	print "surface coordinates; the third is the depth."
end function

showShotResult = function(shot,location)
	result = "Sonar reports shot was "
	if shot[1] > location[1] then
		result = result + "north"
	else if shot[1] < location[1] then
		result = result + "south"		
	end if
	
	if shot[0] > location[0] then
		result = result + "east"
	else if shot[0] < location[0] then
		result = result + "west"
	end if
	
	if shot[1] != location[1] or shot[0] != location[0] then
		result = result + " and "
	end if
	
	if shot[2] > location[2] then
		result = result + "too low."
	else if shot[2] < location[2] then
		result = result + "too high."
	else
		result = result + "depth OK."
	end if
	
	print result
end function

getShot = function
	shotPos = [0,0,0]
	while true
		rawGuess = input("Enter coordinates: ").split(" ")
		if rawGuess.len == 3 then
			shotPos[0] = rawGuess[0].val
			shotPos[1] = rawGuess[1].val
			shotPos[2] = rawGuess[2].val
			return shotPos
		else
			print "Please enter coordinates separated by spaces"
			print "Example: 3 2 1"
		end if
	end while
end function


playGame = function
	print "Good luck!"
	print 
	
	subPos = [floor(rnd*size), floor(rnd*size), floor(rnd*size)]
	
	// For debugging, you can give away the answer:
	//print "(Sub is hidden at: " + subPos.join(" ") + ")"
	
	for c in range(1, numCharges)
		print "Trial " + c
		
		shot = getShot
		
		if shot[0] == subPos[0] and shot[1] == subPos[1] and shot[2] == subPos[2] then
			print "B O O M ! ! You found it in " + c + " tries!"
			return
		else
			showShotResult(shot,subPos)
		end if
	end for
	
	print "You have been torpedoed! Abandon ship!"
	print "The submarine was at " + subPos.join(" ") + "."
end function

showWelcome
setup
while true
	playGame
	if askYesNo("Another game (Y or N): ") == "n" then
		print "OK. Hope you enjoyed yourself."
		break
	end if
end while
