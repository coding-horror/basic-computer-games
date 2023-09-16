degToRad = function(n)
	return n * pi / 180
end function

radToDeg = function(n)
	return n * 180 / pi
end function

roundDown = function(n, r)
	return floor(n / r) * r
end function

getCoord = function(distance, radX, radZ)
	xc = sin(radZ)*cos(radX)*distance
	yc = sin(radZ)*sin(radX)*distance
	zc = cos(radZ)*distance
	return [xc,yc,zc]
end function

distanceBetween = function (d1,d2)
	return ((d1[0]-d2[0])^2 + (d1[1]-d2[1])^2 + (d1[2]-d2[2])^2)^.5
end function

coordStr = function(coords)
	return "X = " + round(coords[0]) +
	  "   Y = " + round(coords[1]) + "   Z = " + round(coords[2])
end function

print " " * 33 + "TARGET"
print " " * 15 + "Creative Computing   Morristown, New Jersey"
print; print; print

print "You are the weapons officer on the Starship Enterprise"
print "and this is a test to see how accurae a shot you"
print "are in a 3-dimensional range. You will be told"
print "the radian offset for the X and Z axes, the location"
print "of the target in 3-dimensional rectangular coordinates,"
print "the approximate number of degrees from the X and Z"
print "axes, and the approximate distance to the target."
print "You will then proceed to shoot at the target until it is"
print "destroyed!"
print; print
print "Good luck!"
roundToList = [20,10,2,1]
ready = true
while ready
	turns = -1
	radX = rnd * 2 * pi
	radZ = rnd * 2 * pi
	print "Radians from X axis = " + radX + "  from Z axis = " + radZ
	
	distance = 100000 * rnd * rnd
	coords = getCoord(distance, radX, radZ)
	
	print "Target sighted: Approx Coordinates: " + coordStr(coords)
	
	gameRunning = true
	while gameRunning
		turns += 1
		if turns >=4 then
			estDistance = distance
		else
			estDistance = roundDown(distance, roundToList[turns])
		end if
		
		print "    Estimated Distance: " + 	estDistance
		print
		tx = input ("Input angle deviation from X in degrees: ").val
		tz = input ("Input angle deviation from Z in degrees: ").val
		tdist = input ("Input distance: ").val
		print
		if tdist < 20 then
			print "You blew yourself up!!"
			gameRunning = false
		else
			tx = degToRad(tx)
			tz = degToRad(tz)
			
			print "Radians from X-axis = " + tx + " from Z-axis = " + tz
			targeted = getCoord(tdist, tx,tz)
			distBet = distanceBetween(coords, targeted)
			if distBet > 20 then
				dx = targeted[0] - coords[0]
				dy = targeted[1] - coords[1]
				dz = targeted[2] - coords[2]
				xMsg = {false: "Shot in front of target ", true: "Shot behind target "}
				print xMsg[dx<0] + dx + " kilometers."
				yMsg = {false: "Shot to left of target ", true: "Shot to right of target "}
				print yMsg[dy<0] + dy + " kilometers."
				zMsg = {false: "Shot above target ", true: "Shot below target "}
				print zMsg[dz<0] + dz + " kilometers."
				
				print "Approx position of explosion: " + coordStr(targeted)
				print "    Distance from target = " + distBet
				print 
				print
				
			else
				print
				print " * * * HIT * * *  Target is non-functional"
				print
				print "Distance of explosion from target was " + distBet + "kilometers."
				print
				print "Mission accomplished in " + (turns+1) + " shots."
				print
				gameRunning = false
			end if
		end if
	end while
	print
	ans = input("Ready for next target? ").lower
	ready = ans and  ans[0].lower == "y"
	print
end while
