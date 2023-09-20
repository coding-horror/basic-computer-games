print " "*34 + "LEM"
print " "*15 + "Creative Computing  Morristown, New Jersey"
// rockt2 is an interactive game that simulates a lunar
// landing is similar to that of the apollo program.
// There is absolutely no chance involved


printIntro = function
	print
	print "  You are on a lunar landing mission.  as the pilot of"
	print "the lunar excursion module, you will be expected to"
	print "give certain commands to the module navigation system."
	print "The on-board computer will give a running account"
	print "of information needed to navigate the ship."
	print
	input "(Press Return.)"
	print
	print "The attitude angle called for is described as follows."
	print "+ or -180 degrees is directly away from the moon"
	print "-90 degrees is on a tangent in the direction of orbit"
	print "+90 degrees is on a tangent from the direction of orbit"
	print "0 (zero) degrees is directly toward the moon"
	print
	print " "*30 + "-180|+180"
	print " "*34 + "^"
	print " "*27 + "-90 < -+- > +90"
	print " "*34 + "!"
	print " "*34 + "0"
	print " "*21 + "<<<< direction of orbit <<<<"
	print
	print " "*20 + "------ surface of moon ------"
	print
	input
	print
	print "All angles between -180 and +180 degrees are accepted."
	print
	print "1 fuel unit = 1 sec. at max thrust"
	print "Any discrepancies are accounted for in the use of fuel"
	print "for an attitude change."
	print "Available engine power: 0 (zero) and any value between"
	print "10 and 100 percent."
	print
	print "Negative thrust or time is prohibited."
	print
	input
end function

printInOutInfo = function(withExample = true)
	print
	print "Input: time interval in seconds ------ (T)"
	print "       percentage of thrust ---------- (P)"
	print "       attitude angle in degrees ----- (A)"
	print
	if withExample then
		print "For example:"
		print "T,P,A? 10,65,-60"
		print "To abort the mission at any time, enter 0,0,0"
		print
	end if
	print "Output: total time in elapsed seconds"
	print "        height in " + ms
	print "        distance from landing site in " + ms
	print "        vertical velocity in " + ms + "/second"
	print "        horizontal velocity in " + ms + "/second"
	print "        fuel units remaining"
	print
end function

initState = function
	globals.m = 17.95
	globals.f1 = 5.25
	globals.n = 7.5
	globals.r0 = 926
	globals.v0 = 1.29
	globals.t = 0
	globals.h0 = 60
	globals.r = r0+h0
	globals.a = -3.425
	globals.r1 = 0
	globals.a1 = 8.84361e-04
	globals.r3 = 0
	globals.a3 = 0
	globals.m1 = 7.45
	globals.m0 = m1
	globals.b = 750
	globals.t1 = 0
	globals.f = 0
	globals.p = 0
	globals.n = 1
	globals.m2 = 0
	globals.s = 0
	globals.c = 0
end function

getUnits = function(moreHelp=true)
	print
	while true
		print "Input measurement option number? ", ""
		if moreHelp then
			print
			print "Which system of measurement do you prefer?"
			print " 1 = metric     0 = english"
			print "Enter the appropriate number? ", ""
		end if
		k = input.val
		if k == 0 then
			globals.z = 6080
			globals.ms = "feet"
			globals.g3 = .592
			globals.ns = "n.miles"
			globals.g5 = z
			break
		else if k == 1 then
			globals.z = 1852.8
			globals.ms="meters"
			globals.g3 = 3.6
			globals.ns=" kilometers"
			globals.g5 = 1000
			break
		end if
		moreHelp = true
	end while
end function

startFirstGame = function
	initState
	print
	print "Lunar Landing Simulation"
	print
	print "Have you flown an Apollo/LEM mission before", ""
	while true
		qs = input(" (yes or no)? ").lower
		if qs and (qs[0] == "y" or qs[0] == "n") then break
		print "Just answer the question, please, ", ""
	end while
	getUnits (qs[0] == "n")
	if qs[0] == "n" then printIntro
	printInOutInfo
end function

startSubsequentGame = function
	initState
	print
	print "OK, do you want the complete instructions or the input -"
	print "output statements?"
	while true
		print "1 = complete instructions"
		print "2 = input-output statements"
		print "3 = neither"
		b1 = input.val
		if 1 <= b1 <= 3 then break
	end while
	if b1 == 1 then printIntro
	if b1 < 3 then printInOutInfo
end function

getTurnInputs = function
	while true
		print
		inp = input("T,P,A? ").replace(",", " ").replace("  ", " ").split
		if inp.len != 3 then continue
		globals.t1 = inp[0].val	// NOTE: though we prompt for T, P, A,
		globals.f = inp[1].val	// internally these are t1, f, and p respectively.
		globals.p = inp[2].val
		globals.f = f/100
		if t1 < 0 then
			print
			print "This spacecraft is not able to violate the space-";
			print "time continuum."
			continue
		else if t1 == 0 then
			return		// abort mission
		end if
		if f < 0 or f > 1.05 or abs(f-.05) < .05 then
			print
			print "Impossible thrust value: ", ""
			if f < 0 then
				print "negative"
			else if f < 0.5 then
				print "too small"
			else
				print "too large"
			end if
			continue
		end if
		if abs(p) > 180 then
			print
			print "If you want to spin around, go outside the module"
			print "for an E.V.A."
			continue
		end if
		return
	end while
end function

pad = function(num, width=10)
	anum = abs(num)
	if anum >= 10000 then
		s = round(num)
	else if anum >= 10000 then
		s = round(num, 1)
	else if anum > 100 then
		s = round(num, 2)
	else
		s = round(num, 3)
	end if
	return (s + " " * width)[:width]
end function

integrate = function
	n = 20
	if t1 >= 400 then n = t1/20
	globals.t1 = t1/n
	globals.p = p*3.14159/180
	s = sin(p)
	c = cos(p)
	globals.m2 = m0*t1*f/b
	globals.r3 = -.5*r0*((v0/r)^2)+r*a1*a1
	globals.a3 = -2*r1*a1/r

	for i in range(1, n)
		if m1 != 0 then
			globals.m1 = m1-m2
			if m1<=0 then
				globals.f = f*(1+m1/m2)
				globals.m2 = m1+m2
				print "You are out of fuel."
				globals.m1 = 0
			end if
		else
			globals.f = 0
			globals.m2 = 0
		end if
		globals.m = m-.5*m2
		globals.r4 = r3
		globals.r3 = -.5*r0*((v0/r)^2)+r*a1*a1
		globals.r2 = (3*r3-r4)/2+.00526*f1*f*c/m
		globals.a4 = a3
		globals.a3 = -2*r1*a1/r
		globals.a2 = (3*a3-a4)/2+.0056*f1*f*s/(m*r)
		globals.x = r1*t1+.5*r2*t1*t1
		globals.r = r+x
		globals.h0 = h0+x
		globals.r1 = r1+r2*t1
		globals.a = a+a1*t1+.5*a2*t1*t1
		globals.a1 = a1+a2*t1
		globals.m = m-.5*m2
		globals.t = t+t1
		if h0<3.287828e-04 then break
	end for

	globals.h = h0*z
	globals.h1 = r1*z
	globals.d = r0*a*z
	globals.d1 = r*a1*z
	globals.t2 = m1*b/m0

	print " " + [pad(t, 10), pad(h, 10), pad(d, 10), pad(h1, 10), pad(d1, 10), pad(t2, 10)].join
end function

// Do one turn of the game.  Return true if game still in progress,
// or false when game is over (aborted, crashed, or landed).
doOneTurn = function
	if m1 == 0 then
		// out of fuel!
		globals.t1 = 20
		globals.f = 0
		globals.p = 0
	else
		getTurnInputs
	end if
	if t1 == 0 then
		print "Mission abended"
		return false
	end if
	integrate
	
	if h0 < 3.287828e-04 then
		if r1 < -8.21957e-04 or abs(r*a1) > 4.93174e-04 or h0 < -3.287828e-04 then
			print
			print "Crash !!!!!!!!!!!!!!!!"
			print "Your impact created a crater " + abs(h) + " " + ms + " deep."
			x1 = sqr(d1*d1+h1*h1)*g3
			print "At contact you were traveling " + x1 + " " + ns + "/hr"
		else if abs(d)>10*z then
			print "You are down safely - "
			print
			print "But missed the landing site by" + abs(d/g5) + " " + ns + "."
		else
			print
			print "Tranquility Base here -- the Eagle has landed."
			print "Congratulations -- there was no spacecraft damage."
			print "You may now proceed with surface exploration."		
		end if
		return false
	end if
	
	if r0*a>164.474 then
		print
		print "You have been lost in space with no hope of recovery."
		return false
	end if
	return true
end function

startFirstGame
while true
	t1 = 0; f = 0; p = 0
	integrate
	while doOneTurn
	end while
	print
	while true
		qs = input("Do you want to try it again (yes/no)? ").lower
		if qs and (qs[0] == "y" or qs[0] == "n") then break
	end while
	if qs[0] == "n" then
		print
		print "Too bad, the space program hates to lose experienced"
		print "astronauts."
		break
	end if
	startSubsequentGame
end while
