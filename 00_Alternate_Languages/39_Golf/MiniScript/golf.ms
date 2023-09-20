print " "*34 + "Golf"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

 print "Welcome to the creative computing country club,"
 print "an eighteen hole championship layout located a short"
 print "distance from scenic downtown Morristown.  The"
 print "commentator will explain the game as you play."
 print "Enjoy your game; see you at the 19th hole..."
 print;print
 l = [0] * 11
 holesInCourse=18
 totalScore=0
 totalPar=0
 dubChance=.8
 s2=0
 curHole=1
 

getHoleData = function(hole)
	// data for all the holes: distance, par, locOnLeft, and locOnRight for each one
	data = [
	 361,4,4,2,389,4,3,3,206,3,4,2,500,5,7,2,
	 408,4,2,4,359,4,6,4,424,4,4,2,388,4,4,4,
	 196,3,7,2,400,4,7,2,560,5,7,2,132,3,2,2,
	 357,4,4,4,294,4,2,4,475,5,2,3,375,4,4,2,
	 180,3,6,2,550,5,6,6]
	i = (hole-1) * 4
	globals.distance = data[i]
	globals.par = data[i+1]
	globals.locOnRight = data[i+2]
globals.locOnLeft = data[i+3]
end function

startHole = function(hole)
	getHoleData hole
	print
	print "You are at the tee off hole " + hole + " distance " + distance + " yards, par " + par
	globals.totalPar += par
	print "On your right is ", ""
	printLocation locOnRight
	print "On your left is ", ""
	printLocation locOnLeft
end function

// Get player's handicap
while true
	handicap = input("What is your handicap? ").val
	if 0 <= handicap <= 30 then break
	print "PGA handicaps range from 0 to 30."
end while

// Get player's weak point
while true
	print "Difficulties at golf include:"
	print "0=hook, 1=slice, 2=poor distance, 3=trap shots, 4=putting"
	weakness = input("Which one (only one) is your worst? ").val
	if 0 <= weakness <= 4 then break
end while

// End a sentence by printing the name of the given location
printLocation = function(locIdx)
	if locIdx < 1 or locIdx > 6 then
		print "out of bounds."
	else
		print ["fairway.", "rough.", "trees.", "adjacent fairway.", 
			   "trap.", "water."][locIdx-1]
	end if
end function

// Print score for one hole (plus total), and some praise or advice.
printScore = function(hole, score, par, totalScore, totalPar)
	print "Your score on hole " + hole + " was " + score
	print "Total par for " + hole + " holes is " + totalPar + "  Your total is " + totalScore
	if hole == holesInCourse then return
	if score > par+2 then
		print "Keep your head down."
	else if score == par then
		print "A par.  Nice going."
	else if score == par-1 then
		print "A birdie."
	else if score == 1 then
		print "A hole in one."
	else if score == par-2 then
		print "A great big eagle."
	end if 
end function

// Print club advice -- but only once.
clubAdviceGiven = false
printClubAdvice = function
	if clubAdviceGiven then return	// (already done)
	globals.clubAdviceGiven = true
	print "Selection of clubs"
	print "yardage desired                       suggested clubs"
	print "200 to 280 yards                           1 to 4"
	print "100 to 200 yards                          19 to 13"
	print "  0 to 100 yards                          29 to 23"
end function	

doPenalty = function
 	print "Penalty stroke assessed.  Hit from previous location."
 	globals.score += 1
 	globals.j += 1
 	globals.curLoc = 1
 	globals.distance = prevDistance
end function

// Try to get out of a trap. Return true if succeeded, false if failed.
doTrapShot = function
	if weakness == 3 then
		if rnd <= dubChance then
			globals.dubChance *= 0.2
			print "Shot dubbed, still in trap."
			return false
		end if
		globals.dubChance = 0.8
	end if
	globals.distToPin = 1 + (3*floor((80/(40-handicap))*rnd))
	return true
end function

getClub = function
	//print "DEBUG: getClub, with curLoc=" + curLoc
	while true
		club = input("What club do you choose? ").val
		print
		if club < 1 or club > 29 then continue
		if club > 4 and club < 12 then
			print "That club is not in the bag."
			continue
		end if
		if club >= 12 then club -= 6
		if curLoc <= 5 or club == 14 or club == 23 then break
		print "That club is not in the bag."
		print
		continue
	end while
	return club
end function

getSwing = function(club)
	if club <= 13 then return 1 // (full swing)
	while true
		print "Now gauge your distance by a percentage (1 to 100)"
		swing = input("of a full swing? ").val / 100
		print
		if 0 <= swing <= 1 then return swing
		// Given an invalid swing input, the original BASIC code would 
		// print "That club is not in the bag" and go back to choosing a club.
		// But that is convoluted spaghetti, and I'm not doing it.
	end while
end function

playOneHole = function
	q = 0		// counts certain kinds of shots on every third hole (?)
	distanceHit = 0
	offLine = 0
	
	// shot loop -- take as many shots as you need for this hole
	while true
		if curLoc < 1 then curLoc = 1
		if curLoc > 6 then
			print "Your shot went out of bounds."
			doPenalty
			distanceHit = 0
		else if curLoc > 5 then
			print "Your shot went into the water."
			doPenalty
			distanceHit = 0
		end if
	
		if score > 0 and distanceHit then
			print "Shot went " + distanceHit + " yards.  It's " + distToPin + " yards from the cup."
			print "Ball is " + floor(offLine) + " yards off line... in ", ""
			printLocation curLoc
		end if
 
		printClubAdvice
	
		club = getClub
		swing = getSwing(club)
		globals.score += 1
		if curLoc == 5 and not doTrapShot then continue
		if club > 14 then club -= 10

		//print "DEBUG Club:"+club + " Swing:"+swing + " Weakness:"+weakness

		if curHole % 3 == 0 then
			if s2 + q + (10*(curHole-1)/18) < (curHole-1)*(72+((handicap+1)/.85))/18 then
				q += 1
				if score % 2 and distance >= 95 then
					globals.distance -= 75
					distanceHit = 0
					print "Ball hit tree - bounced into rough " + distance + " yards from hole."
					continue
				end if
			end if		
		end if
		
		if club >= 4 and curLoc == 2 then
			print "You dubbed it."
			distanceHit = 35
		else if score > 7 and distance < 200 then
			// user is really sucking, let's cut them a break
			putt 1 + (3 * floor((80/(40-handicap))*rnd))
			return
		else
			//print "DEBUG: SWING with handicap:" + handicap + " club:" + club
			distanceHit = floor(((30-handicap)*2.5+187-((30-handicap)*.25+15)*club/2)+25*rnd)
			distanceHit = floor(distanceHit*swing)
			if weakness == 2 then distanceHit = floor(.85*distanceHit)
		end if
		offLine = (rnd/.8)*(2*handicap+16)*abs(tan(distanceHit*.0035))
		distToPin = floor(sqrt(offLine^2+abs(distance-distanceHit)^2))
		//print "DEBUG distance:"+distance+"; distanceHit:"+distanceHit+"; distToPin:"+distToPin+"; offLine:"+offLine
		if distanceHit > distance and distToPin >= 20 then print "Too much club. You're past the hole."

		globals.prevDistance = distance
		globals.distance = distToPin
		if distToPin > 27 then
			if offLine < 30 or j > 0 then
				curLoc = 1
				continue
			end if
			// hook or slice
			s9 = (s2+1)/15
			if weakness == 0 then
				isSlice = floor(s9) == s9
			else
				isSlice = not floor(s9) == s9
			end if
			if isSlice then
				print "You sliced- "
				curLoc = locOnRight
			else
				print "You hooked- "
				curLoc = locOnLeft
			end if
			if offLine > 45 then print "badly."

		else if distToPin > 20 then
			curLoc = 5
		else if distToPin > .5 then
			globals.curLoc = 8		// on the green!
			putt distToPin * 3		// (convert yards to feet, and putt)
			return
		else
			curLoc = 9
			print "You holed it."
			print
			globals.curHole += 1
			return
		end if
	end while
end function

putt = function(distToPin)
	puttAttempts = 0
	while true
		distToPin = abs(floor(distToPin))
		print "On green, " + distToPin + " feet from the pin."
		i = input("Choose your putt potency (1 to 13): ").val
		globals.score += 1
		if score+1 - par > handicap*0.072 + 2 or puttAttempts > 2 then break
		puttAttempts += 1
		if weakness == 4 then
			distToPin -= i*(4+1*rnd)+1
		else
			distToPin -= i*(4+2*rnd)+1.5
		end if
		if -2 <= distToPin <= 2 then break
		if distToPin < 0 then
			print "Passed by cup."
		else
			print "Putt short."
		end if
	end while
	print "You holed it."
	print
	return
end function

// main loop
while true
	curLoc = 0
	j = 0
	s2 += 1
	if curHole > 1 then
	end if

	print

	score = 0
	startHole curHole
	playOneHole

	totalScore += score
	printScore curHole, score, par, totalScore, totalPar
	if curHole == holesInCourse then break

	curHole += 1
end while
