within5pct = function(n)
	return n * (1 + rnd / 20 - rnd / 20)
end function

splatMsg = ["Requiescat in pace.", "May the Angel of Heaven lead you into Paradise.",
			"Rest in piece.", "Son-Of-A-Gun.", "#$%&&%!$", 
			"A kick in the pants is a boost if you're headed right.",
			"Hmm. Should have picked a shorter time.", "Mutter. Mutter. Mutter.",
			"Pushing up daisies.", "Easy come, easy go."]

history = []
//clear		// (works on Mini Micro only)
print " " * 33 + "Splat"
print " " * 15 + "Creative Computing  Morristown, New Jersey"
print;print;print
print "Welcome to 'Splat' -- the game that simulates a parachute"
print "jump. Try to open your chute at the last possible"
print "moment without going splat."
ans = "y"
while ans == "y"
	print;print

	distance = floor(9001 * rnd) + 1000
	
	ans = input("Select your own terminal velocity (Yes or No)? ") + " "
	if ans[0].lower == "y" then
		terminalVel = input("What terminal velocity (mi/hr)? ").val
	else
		terminalVel = floor(1000 * rnd)
		print "Ok. Terminal velocity = " + terminalVel + "mi/hr"
	end if

	terminalVel = terminalVel * 5280/3600
	velocity = within5pct(terminalVel)

	ans = input("Want to select acceleration due to gravity (Yes or No)? ") + " "
	if ans[0].lower == "y" then
		acceleration = input("What acceleration (ft/sec/sec)? ").val
		acceleration = within5pct(acceleration)
	else
		bodies = ["Mercury", "Venus", "Earth", "the moon", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "the Sun"]
		gravity = [12.2, 28.3,32.16,5.15,12.5,85.2,37.6,33.8,39.6,896]
		
		initialStmt = ["Fine. You're on ", "All right. You're on ", "Then you're on "]
		
		i = floor(rnd * 10)
		acceleration = gravity[i]
		stmt = initialStmt[i%3] + bodies[i] + ". Acceleration=" + acceleration + " ft/sec/sec."
		print stmt
	end if
		
	actAccel = within5pct(acceleration)
	print
	print "    Altitude         = " + distance + " ft"
	print "    Term. Velocity   = " + terminalVel + " ft/sec +/-5%"
	print "    Acceleration     = " + acceleration + " ft/sec/sec +/-5%"
	print "Set the timer for your freefall."
	sec = input("How many seconds? ").val

	print "Here we go."; print

	print "Time (sec)" + char(9) + "Dist to Fall (ft)"
	print "==========" + char(9) + "================="
	termVelHit = false
	for i in range(0, sec, sec/8)
		sec = velocity/actAccel
		if i <= sec and termVelHit == false then
			dist = distance - ((actAccel/2)*i^2)
		else if termVelHit == false then
			termVelHit = true
			print "Terminal velocity reached a T plus " + velocity/actAccel + " seconds."
		end if
		if termVelHit then
			dist = distance - ((velocity^2/(2*actAccel))+(velocity*(i-sec)))
		end if

		if dist <= 0 then break
		print (" " + i + " " * 9)[:9] + char(9) + " " + dist
	end for

	if dist > 0 then
		print "Chute open"
		history.push(dist)
		numJumps = history.len
		numLowerJumps = 0
		for d in history
			numLowerJumps += (dist <= d)
		end for
		
		jumpDiff = numJumps - numLowerJumps
		if numJumps < 4 then
			ordinal = ["1st", "2nd", "3rd"]
			print "Amazing!! Not bad for your " + ordinal[numJumps-1] + " successful jump!!!"
		else if jumpDiff <= numJumps * 0.10 then
			print "Wow! That's some jumping. Of the " + numJumps + " successful jumps"
			print "before yours, only " + jumpDiff + " opened their chutes lower than"
			print "you did."
		else if jumpDiff <= numJumps * 0.25 then
			print "Pretty good! " + numJumps + " successful jumps preceded yours and only"
			print jumpDiff + " of them got lower than you did before their chute"
			print "opened."
		else if jumpDiff <= numJumps * 0.50 then
			print "Not bad. There have been " + numJumps + " successful jumps before yours."
			print "You were beaten out by  " + jumpDiff + " of them."
		else if jumpDiff <= numJumps * 0.75 then
			print "Conservative, aren't you? You ranked only " + jumpDiff + " in the"
			print numJumps + " successful jumps before yours."
		else if jumpDiff <= numJumps * 0.90 then
			print "Humph! Don't you have any sporting blood? There were"
			print numJumps + " successful jumps before yours and you came in " + numLowerJumps + " jumps"
			print "better than the worst. Shape up!!!"
		else
			print "Hey! You pulled the rip code much too soon. " + numJumps + " successful"
			print "jumps before yours and you came in number " + jumpDiff + "! Get with it."
		end if
	else if dist <= 0 and not termVelHit then
		print (2 * distance / actAccel) ^ .5 + " " * 5 + "Splat"
	else if dist <= 0 and termVelHit then
		print velocity/actAccel + ((distance - (velocity^2/(2*actAccel)))/velocity) + " " *5 + "Splat"
	end if

	if dist <=0 then
		splatMsg.shuffle
		print splatMsg[0]
		print "I'll give you another chance."
	end if

	ans = input("Do you want to play again? ") + " "
	ans = ans[0].lower
end while

print "S" * 10
print