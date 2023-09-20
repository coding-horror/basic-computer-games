// Diamond
//
// Ported from BASIC to MiniScript by Joe Strout

print " "*33 + "DIAMOND"
print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print; print; print
print "For a pretty diamond pattern,"
maxw = input("type in an odd number between 5 and 21: ").val
s = "CC" + "!" * maxw
columns = floor(68/maxw)
for row in range(1, columns)
	for w in range(1, maxw, 2) + range(maxw-2, 1, -2)
		print " "*(maxw-w)/2, ""
		for column in range(1, columns)
			print s[:w], ""
			if column < columns then print " "*(maxw-w), ""
		end for
		print
		wait 0.01
	end for
end for

