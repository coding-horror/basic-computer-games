print " "*33 + "Bounce"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
t = [0]*21
print "This simulation lets you specify the initial velocity"
print "of a ball thrown straight up, and the coefficient of"
print "elasticity of the ball.  Please use a decimal fraction"
print "coefficiency (less than 1)."
print
print "You also specify the time increment to be used in"
print "'strobing' the ball's flight (try .1 initially)."
print

addToLine = function(line, tabPos, textToAdd)
	return line + " " * (floor(tabPos) - line.len) + textToAdd
end function

while true
	s2 = input("Time increment (sec)? ").val
	print
	v = input("Velocity (fps)? ").val
	print
	c = input("Coefficient? ").val
	print
	print "feet"
	print
	s1 = floor(70/(v/(16*s2)))
	for i in range(1, s1)
		t[i]=v*c^(i-1)/16
	end for
	for h in range(floor(-16*(v/32)^2+v^2/32+.5), 0, -0.5)
		line = ""
		if floor(h)==h then line = str(h)
		l=0
		for i in range(1, s1)
			for time in range(0, t[i], s2)
				l=l+s2
				if abs(h-(.5*(-32)*time^2+v*c^(i-1)*time))<=.25 then
					line = addToLine(line, l/s2, "0")
				end if
			end for
			time = t[i+1]/2
			if -16*time^2+v*c^(i-1)*time < h then break
		end for
		print line
	end for
	print " " + "." * floor((l+1)/s2+1)
	line = " 0"
	for i in range(1, l+.9995)
		line = addToLine(line, i/s2, i)
	end for
	print line
	print " " * floor((l+1)/(2*s2)-2) + "seconds"
	print
end while
