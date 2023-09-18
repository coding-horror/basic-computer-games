if version.hostName == "Mini Micro" then clear
print " "*30 + "Rocket"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "Lunar Landing Simulation"
print "----- ------- ----------";  print
yn = input("Do you want instructions (yes or no)? ").lower
if not yn or yn[0] != "n" then
	print
	print "You are landing on the moon and and have taken over manual"
	print "control 1000 feet above a good landing spot. You have a down-"
	print "ward velocity of 50 feet/sec. 150 units of fuel remain."
	print
	print "Here are the rules that govern your Apollo space-craft"
	print "(Press Return after each one):"; print
	print "(1) After each second the height, velocity, and remaining fuel"
	print "    will be reported via Digby your on-board computer."
	input
	print "(2) After the report a '?' will appear. Enter the number"
	print "    of units of fuel you wish to burn during the next"
	print "    second. Each unit of fuel will slow your descent by"
	print "    1 foot/sec."
	input
	print "(3) The maximum thrust of your engine is 30 feet/sec/sec"
	print "    or 30 units of fuel per second."
	input
	print "(4) When you contact the lunar surface, your descent engine"
	print "    will automatically shut down and you will be given a"
	print "    report of your landing speed and remaining fuel."
	input
	print "(5) If you run out of fuel the '?' will no longer appear"
	print "    but your second by second report will continue until"
	print "    you contact the lunar surface."; print
	input
end if

pad = function(s, width=10)
	return (s + " "*width)[:width]
end function

// Bonus little feature when running in Mini Micro: display a header bar
// always at the top of the screen.
drawHeaders = function
	if version.hostName != "Mini Micro" then return
	display(2).mode = displayMode.text; td = display(2)
	td.color = text.color; td.backColor = color.black
	td.row = 25; td.column = 0
	td.print "sec  feet   speed     fuel    plot of distance" + " "*21
end function

while true
	print "Beginning landing procedure.........."; print
	print "G O O D  L U C K ! ! !"
	print; print
	print "sec  feet   speed     fuel    plot of distance"
	drawHeaders
	print
	t=0; h=1000; v=50; fuel=150
	while true
		print pad(t,5) + pad(h,7) + pad(v, 10) + pad(fuel,8) + "|" + " "*floor(h/30) + "*"
		if fuel <= 0 then
			burn = 0
			wait 0.5	// (slight pause for drama and legibility)
		else
			burn = input("?").val
			if burn < 0 then burn = 0
			if burn > 30 then burn=30
			if burn > fuel then
				burn = fuel
				print "**** out of fuel ****"
			end if
		end if
		v1 = v - burn + 5
		fuel -= burn
		h -= .5*(v+v1)
		if h <= 0 then break
		t += 1
		v = v1
	end while
	print "***** CONTACT *****"
	h = h+ .5*(v1+v)
	if burn == 5 then
		d = h/v
	else
		d = (-v+sqrt(v*v+h*(10-2*burn)))/(5-burn)
	end if
	v1 = v + (5-burn)*d
	print "Touchdown at " + (t+d) + " seconds."
	print "Landing velocity = " + round(v1,1) + " feet/sec."
	print fuel + " units of fuel remaining."
	if v1 == 0 then
		print "Congratulations! a perfect landing!!"
		print "Your license will be renewed.......later."
	else if abs(v1) >= 2 then
		print "***** Sorry, but you blew it!!!!"
		print "Appropriate condolences will be sent to your next of kin."
	end if
	print; print; print
	yn = input("Another mission? ").lower
	if not yn or yn[0] != "y" then break
end while
print;  print "Control out.";  print
