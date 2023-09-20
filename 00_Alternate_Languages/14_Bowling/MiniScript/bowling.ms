import "listUtil"

print " "*34 + "Bowl"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

pinDown = [0]*10		// state of each pin: 1=down, 0=standing
player = 1
frame = 1
ball = 1
scores = list.init3d(10, 4, 3, 0)	// index by [frame][player][ball], all 0-based

printInstructions = function
	print "The game of bowling takes mind and skill. During the game"
	print "the computer will keep score. You may compete with"
	print "other players [up to four]. You will be playing ten frames."
	print "On the pin diagram 'O' means the pin is down...'+' means the"
	print "pin is standing. After the game the computer will show your"
	print "scores."
end function

printPinDiagram = function
	print "Player: " + (player+1) + "  Frame: " + (frame+1) + "  Ball: " + (ball+1)
	print
	k = 0
	for row in range (0, 3)
		line = " " * row
		for j in range(1, 4-row)
			line += "+O"[pinDown[k]] + " "
			k += 1
		end for
		print line
	end for
end function

printAnalysis = function(previousDown=0)
	pinsLeft = 10 - pinDown.sum
	if pinDown.sum == previousDown then print "Gutter!!"
	if ball == 0 and pinsLeft == 0 then
		print "Strike!!!!!" + char(7)*4
		globals.status = 3
	else if ball == 1 and pinsLeft == 0 then
		print "Spare!!!!"
		globals.status = 2
	else if ball == 1 and pinsLeft > 0 then
		print "Error!!!"	// (i.e., didn't clear all the pins in 2 balls)
		globals.status = 1
	end if
end function

rollOneBall = function
	print "Type roll to get the ball going."
	input		// (response ignored)
	for i in range(1, 20)
		// Generate a random number from 0-99, then take this mod 15.
		// This gives us a slightly higher chance of hitting a non-existent
		// pin than one of the actual 10.
		x = floor(rnd*100)
		if x % 15 < 10 then pinDown[x % 15] = 1
	end for
	printPinDiagram
end function

doOneFrame = function
	globals.pinDown = [0]*10
	globals.ball = 0
	rollOneBall
	printAnalysis
	hitOnBall0 = pinDown.sum
	scores[frame][player][ball] = hitOnBall0
	
	globals.ball = 1
	if hitOnBall0 < 10 then
		print "Roll your 2nd ball"
		print
		rollOneBall
		printAnalysis hitOnBall0
	end if
	// Note: scoring in this program is not like real bowling.
	// It just stores the number of pins down at the end of each ball,
	// and a status code (1, 2, or 3).
	scores[frame][player][ball] = pinDown.sum
	scores[frame][player][2] = status
end function

pad = function(n, width=3)
	return (" "*width + n)[-width:]
end function

printFinalScores = function
	print "FRAMES"
	for i in range(1,10)
		print pad(i), ""
	end for
	print
	for player in range(0, numPlayers-1)
		for i in range(0, 2)
			for frame in range(0, 9)
				print pad(scores[frame][player][i]), ""
			end for
			print
		end for
		print
	end for
end function

playOneGame = function
	for f in range(0, 9)
		globals.frame = f
		for p in range(0, numPlayers-1)
			globals.player = p
			doOneFrame
		end for
	end for
	print
	printFinalScores
end function

// Main program
print "Welcome to the alley"
print "Bring your friends"
print "Okay let's first get acquainted"
print
ans = input("The instructions (Y/N)? ").upper
if not ans or ans[0] != "N" then printInstructions
while true
	numPlayers = input("First of all...How many are playing? ").val
	if 0 < numPlayers < 5 then break
	print "Please enter a number from 1 to 4."
end while
print
print "Very good..."
while true
	playOneGame
	print
	ans = input("Do you want another game? ").upper
	if not ans or ans[0] != "Y" then break
end while
