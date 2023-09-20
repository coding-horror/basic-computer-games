print " "*33 + "Orbit"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "Somewhere above your planet is a Romulan ship."
print
print "The ship is in a constant polar orbit.  Its"
print "distance from the center of your planet is from"
print "10,000 to 30,000 miles and at its present velocity can"
print "circle your planet once every 12 to 36 hours."
print
print "Unfortunately, they are using a cloaking device so"
print "you are unable to see them, but with a special"
print "instrument you can tell how near their ship your"
print "photon bomb exploded.  You have seven hours until they"
print "have built up sufficient power in order to escape"
print "your planet's gravity."
print
print "Your planet has enough power to fire one bomb an hour."
print
print "At the beginning of each hour you will be asked to give an"
print "angle (between 0 and 360) and a distance in units of"
print "100 miles (between 100 and 300), after which your bomb's"
print "distance from the enemy ship will be given."
print
print "An explosion within 5,000 miles of the romulan ship"
print "will destroy it."
print; input "(Press Return.)"
print
print "Below is a diagram to help you visualize your plight."
print
print
print "                          90"
print "                    0000000000000"
print "                 0000000000000000000"
print "               000000           000000"
print "             00000                 00000"
print "            00000    xxxxxxxxxxx    00000"
print "           00000    xxxxxxxxxxxxx    00000"
print "          0000     xxxxxxxxxxxxxxx     0000"
print "         0000     xxxxxxxxxxxxxxxxx     0000"
print "        0000     xxxxxxxxxxxxxxxxxxx     0000"
print "180<== 00000     xxxxxxxxxxxxxxxxxxx     00000 ==>0"
print "        0000     xxxxxxxxxxxxxxxxxxx     0000"
print "         0000     xxxxxxxxxxxxxxxxx     0000"
print "          0000     xxxxxxxxxxxxxxx     0000"
print "           00000    xxxxxxxxxxxxx    00000"
print "            00000    xxxxxxxxxxx    00000"
print "             00000                 00000"
print "               000000           000000"
print "                 0000000000000000000"
print "                    0000000000000"
print "                         270"
print
print "x - your planet"
print "o - the orbit of the romulan ship"
print; input "(Press Return.)"
print
print "On the above diagram, the romulan ship is circling"
print "counterclockwise around your planet.  Don't forget that"
print "without sufficient power the romulan ship's altitude"
print "and orbital rate will remain constant."
print
print "Good luck.  The federation is counting on you."

while true
	a=floor(360*rnd)
	d=floor(200*rnd + 200)
	r=floor(20*rnd + 10)
	for h in range(1,7)
		print
		print
		print "This is hour " + h + ", at what angle do you wish to send"
		a1 = input("your photon bomb? ").val
		d1 = input("How far out do you wish to detonate it? ").val
		print
		print
		a += r
		if a >= 360 then a -= 360
		t = abs(a-a1)
		if t >= 180 then t = 360 - t
		c = sqrt(d*d + d1*d1 - 2*d*d1*cos(t*pi/180))
		print "Your photon bomb exploded " + round(c) + " * 10^2 miles from the"
		print "Romulan ship."
		if c<=50 then break
	end for
	if c <= 50 then
		print "You have succesfully completed your mission."
	else
		print "You have allowed the Romulans to escape."
	end if
	print "Another romulan ship has gone into orbit."
	yn = input("Do you wish to try to destroy it? ").lower + " "
	if yn[0] != "y" then break
end while
print "good bye."
