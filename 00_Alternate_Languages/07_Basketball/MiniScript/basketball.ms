print " "*31 + "Basketball"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "This is Dartmouth College basketball.  You will be Dartmouth"
print " captain and playmaker.  Call shots as follows:  1. Long"
print " (30 ft.) jump shot; 2. Short (15 ft.) jump shot; 3. Lay"
print " up; 4. Set shot."
print "Both teams will use the same defense.  Call defense as"
print "follows:  6. Press; 6.5 Man-to-Man; 7. Zone; 7.5 None."
print "To change defense, just type 0 as your next shot."

inputDefense = function(prompt)
	while true
		globals.defense = input(prompt).val
		if defense >= 6 then break
	end while
end function

// Do the center jump; return US or THEM who gets the ball.
centerJump = function
	print "Center Jump"
	if rnd < 0.6 then
		print opponent + " controls the tap."
		return THEM
	else
		print "Dartmouth controls the tap."
		return US
	end if
end function

inputShot = function
	while true
		globals.shotType = input("Your shot (0-4): ")
		if shotType == "0" then
			inputDefense "Your new defensive alignment is? "
			continue
		end if
		globals.shotType = shotType.val
		if 1 <= shotType <= 4 then return
	end while
end function

endFirstHalf = function
	print "   ***** End of first half *****"; print
	print "Score: Dartmouth: " + score[US] + "   " + opponent + ": " + score[THEM]
	print; print
	globals.inControl = centerJump
end function

checkGameOver = function
	print
	if score[0] != score[1] then
		print "  ***** END OF GAME *****"
		print "Final score: Dartmouth: " + score[US] + "   " + opponent + ": " + score[THEM]
		print
		return true
	else
		print "  ***** End of Second Half *****"
		print "Score at end of regulation time:"
		print "        Dartmouth: " + score[US] + "   " + opponent + ": " + score[THEM]
		print
		print "Begin two minute overtime period"
		return false
	end if
end function

scoreBasket = function(who = null)
	if who == null then who = inControl
	score[who] += 2
	printScore
end function

printScore = function
	print "Score: " + score[1] + " to " + score[0]
	print "Time: " + timer
end function

// Logic for a Dartmouth jump shot.  Return true to continue as Dartmouth,
// false to pass the ball to the opponent.
dartmouthJumpShot = function
	if rnd <= 0.341 * defense / 8 then
		print "Shot is good."
		scoreBasket
		return false
	end if

	if rnd < 0.682 * defense / 8 then
		print "Shot is off target."
		if defense/6 * rnd > 0.45 then
			print "Rebound to " + opponent
			return false
		end if
		print "Dartmouth controls the rebound."
		if rnd <= 0.4 then
			globals.shotType = 3 + (rnd > 0.5)
			return true
		end if
		if defense == 6 and rnd < 0.6 then
			print "Pass stolen by " + opponent + ", easy layup."
			scoreBasket THEM
			return true
		end if
		print "Ball passed back to you."
		return true
	end if
	
	if rnd < 0.782 * defense/8 then
		print "Shot is blocked.  Ball controlled by ", ""
		if rnd > 0.5 then
			print opponent + "."
			return false
		else
			print "Dartmouth."
			return true
		end if
	end if
	
	if rnd > 0.843 * defense/8 then
		print "Charging foul.  Dartmouth loses ball."
	else
		print "Shooter is fouled.  Two shots."
		doFoulShots US
	end if
	return false
end function

// Logic for an opponent jump shot.  Return true to continue as opponent,
// false to pass the ball to Dartmouth.
opponentJumpShot = function
	if rnd <= 0.35 * defense / 8 then
		print "Shot is good."
		scoreBasket
		return false
	end if

	if 8 / defense * rnd <= 0.75 then
		print "Shot is off rim."
		return opponentMissed
	end if
	
	if 8 / defense * rnd <= 0.9 then
		print "Player fouled.  Two shots."
		doFoulShots THEM
		return false
	end if
	print "Offensive foul.  Dartmouth's ball."
	return false
end function

// Do a Dartmouth set shot or lay-up.  Return true to continue as Dartmouth,
// false to pass the ball to the opponent.
dartmouthSetOrLay = function
	if 7 / defense * rnd <= 0.4 then
		print "Shot is good.  Two points."
		scoreBasket
	else if 7 / defense * rnd <= 0.7 then
		print "Shot is off the rim."
		if rnd < 0.667 then
			print opponent + " controls the rebound."
			return false
		end if
		print "Dartmouth controls the rebound."
		if rnd < 0.4 then return true
		print "Ball passed back to you."
		return true
	else if 7 / defense * rnd < 0.875 then
		print "Shooter fouled.  Two shots."
		doFoulShots US
	else if 7 / defense * rnd < 0.925 then
		print "Shot blocked.  " + opponent + "'s ball."
	else
		print "Charging foul.  Dartmouth loses the ball."
	end if			
	return false
end function

// Do an opponent set shot or lay-up.  Return true to continue as opponent,
// false to pass the ball to Dartmouth.
opponentSetOrLay = function
	if 7 / defense * rnd <= 0.413 then
		print "Shot is missed."
		return opponentMissed
	else
		print "Shot is good."
		scoreBasket
		return false
	end if
end function

// Handle opponent missing a shot -- return true to continue as opponent,
// false to pass the ball to Dartmouth.
opponentMissed = function
	if defense / 6 * rnd <= 0.5 then
		print "Dartmouth controls the rebound."
		return false
	else
		print opponent + " controls the rebound."
		if defense == 6 and rnd <= 0.75 then
			print "Ball stolen.  Easy lay up for Dartmouth."
			scoreBasket US
			return true
		end if
		if rnd <= 0.5 then
			print "Pass back to " + opponent + " guard."
			return true
		end if
		globals.shotType = 4 - (rnd > 0.5)
		return true
	end if
end function

playOneSide = function
	print
	while true
		globals.timer += 1
		if timer == 50 then return endFirstHalf
		if time == 92 then
			print "  *** Two minutes left in the game ***"; print
		end if
		if shotType == 1 or shotType == 2 then
			print "Jump Shot"
			if inControl == US then
				if dartmouthJumpShot then continue else break
			else
				if opponentJumpShot then continue else break
			end if
		else		// (if shot type >= 3)
			if shotType > 3 then print "Set shot." else print "Lay up."
			if inControl == US then
				if dartmouthSetOrLay then continue else break
			else
				if opponentSetOrLay then continue else break
			end if
		end if
	end while
	globals.inControl = 1 - globals.inControl
end function

doFoulShots = function(who)
	if rnd > 0.49 then
		if rnd > 0.75 then
			print "Both shots missed."
		else
			print "Shooter makes one shot and misses one."
			score[who] += 1
		end if
	else
		print "Shooter makes both shots."
		score[who] += 2
	end if
	printScore
end function

opponentPlay = function
	print
	while true
		globals.timer += 1
		if timer == 50 then return endFirstHalf
		if time == 92 then
			print "  *** Two minutes left in the game ***"; print
		end if
		if shotType == 1 or shotType == 2 then
			print "Jump Shot"
			if opponentJumpShot then continue else break
		else		// (if shot type >= 3)
			if shotType > 3 then print "Set shot." else print "Lay up."
			if opponentSetOrLay then continue else break
		end if
	end while
	globals.inControl = US
end function

// Constants
THEM = 0
US = 1

// Main program
inputDefense "Your starting defense will be? "
print
opponent = input("Choose your opponent? ")
score = [0,0]		// score for each team (US and THEM)
gameOver = false
inControl = centerJump
timer = 0
while not gameOver
	print
	if inControl == US then
		inputShot
		playOneSide
	else
		shotType = ceil(10 / 4 * rnd + 1)
		playOneSide
	end if
	if timer >= 100 then
		if checkGameOver then break
		timer = 93
		dartmouthHasBall = centerJump
	end if
end while
