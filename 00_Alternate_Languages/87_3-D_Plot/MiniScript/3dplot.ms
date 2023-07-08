// 3dPlot
//
// Converted from BASIC to MiniScript by Joe Strout

print " "*32 + "3D PLOT"
print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print; print; print
e = 2.71828

fna = function(z)
	return 30 * e^(-z*z/100)
end function

for x in range(-30, 30, 1.5)
	lastZ = 0
	y1 = 5 * floor(sqrt(900-x*x)/5)
	for y in range(y1, -y1, -5)
		z = floor(25+fna(sqrt(x*x+y*y))-.7*y)
		if z > lastZ then
			print " "*(z-lastZ) + "*", ""
			lastZ = z
		end if
	end for
	print
	wait 0.1  // (optional)
end for
