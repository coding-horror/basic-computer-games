print " "*30 + "SINE WAVE"
print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print; print; print; print; print
// Remarkable program by David Ahl, ported
// from BASIC to MiniScript by Joe Strout

B = 0
// start long loop
for t in range(0, 40, 0.25)
	A = floor(26 + 25*sin(t))
	print " "*A, ""
	if not B then print "CREATIVE" else print "COMPUTING"
	B = not B
	wait 0.01
end for
