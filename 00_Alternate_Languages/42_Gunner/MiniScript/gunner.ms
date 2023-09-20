print " "*30 + "Gunner"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "You are the officer-in-charge, giving orders to a gun"
print "crew, telling them the degrees of elevation you estimate"
print "will place a projectile on target.  A hit within 100 yards"
print "of the target will destroy it."; print

// Select a target and give the player up to 5 tries to hit it.
// Return the number of shots taken, or set globals.gameOver to true.
playOneTarget = function(maxRange)
	globals.gameOver = false
	targetDist = floor(maxRange * (.1 + .8 * rnd))
	shot = 0
	print "Distance to the target is " + targetDist + " yards."
	print
	while true
		print
		degrees = input("Elevation? ").val
		if degrees > 89 then
			print "Maximum elevation is 89 degrees."
			continue
		else if degrees < 1 then
			print "Minimum elevation is one degree."
			continue
		end if
		shot += 1
		if shot >= 6 then
			globals.gameOver = true
			return
		end if
		radiansX2 = 2 * degrees * pi/180
		throw = maxRange * sin(radiansX2)
		diff = floor(targetDist - throw)
		if abs(diff) < 100 then
			print "*** TARGET DESTROYED ***  " + shot + " rounds of ammunition expended."
			return shot
		end if
		if diff > 0 then
			print "Short of target by " + diff + " yards."
		else
			print "Over target by " + abs(diff) + " yards."
		end if
	end while
end function

playOneGame = function
	maxRange = floor(40000*rnd + 20000)
	print "Maximum range of your gun is " + maxRange + " yards."
	shots = 0
	for targetNum in range(1,4)
		shots += playOneTarget(maxRange)
		if gameOver then break
		if targetNum < 4 then
			print
			print "The forward observer has sighted more enemy activity..."
		end if
	end for
	if gameOver then
		print; print "Boom !!!!   You have just been destroyed"
		print "by the enemy."; print; print; print
	else
		print; print; print "Total rounds expended were: " + shots
	end if
	if shots > 18 or gameOver then
		print "Better go back to font sill for refresher training!"
	else
		print "Nice shooting !!"
	end if
end function

// Main loop
while true
	playOneGame
	print; yn = input("Try again (Y or N)? ").upper
	if not yn or yn[0] != "Y" then break
end while
print; print "OK.  Return to base camp."
