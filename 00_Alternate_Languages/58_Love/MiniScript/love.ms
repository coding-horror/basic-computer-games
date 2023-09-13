print " "*33 + "LOVE"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

print "A tribute to the great american artist, Robert Indiana."
print "His greatest work will be reproduced with a message of"
print "your choice up to 60 characters.  If you can't think of"
print "a message, simple type the word 'LOVE'"; print
msg = input("Your message, please? ")
for i in range(1, 10); print; end for

repeatedMsg = msg * ceil(60 / msg.len)

data = []
data += [60,1,12,26,9,12,3,8,24,17,8,4,6,23,21,6,4,6,22,12,5,6,5]
data += [4,6,21,11,8,6,4,4,6,21,10,10,5,4,4,6,21,9,11,5,4]
data += [4,6,21,8,11,6,4,4,6,21,7,11,7,4,4,6,21,6,11,8,4]
data += [4,6,19,1,1,5,11,9,4,4,6,19,1,1,5,10,10,4,4,6,18,2,1,6,8,11,4]
data += [4,6,17,3,1,7,5,13,4,4,6,15,5,2,23,5,1,29,5,17,8]
data += [1,29,9,9,12,1,13,5,40,1,1,13,5,40,1,4,6,13,3,10,6,12,5,1]
data += [5,6,11,3,11,6,14,3,1,5,6,11,3,11,6,15,2,1]
data += [6,6,9,3,12,6,16,1,1,6,6,9,3,12,6,7,1,10]
data += [7,6,7,3,13,6,6,2,10,7,6,7,3,13,14,10,8,6,5,3,14,6,6,2,10]
data += [8,6,5,3,14,6,7,1,10,9,6,3,3,15,6,16,1,1]
data += [9,6,3,3,15,6,15,2,1,10,6,1,3,16,6,14,3,1,10,10,16,6,12,5,1]
data += [11,8,13,27,1,11,8,13,27,1,60]

for row in range(0, 35)
	s = []
	a1 = 0; p = true
	while a1 < 60
		a = data.pull
		a1 += a
		for i in range(a1-a, a1-1)
			s.push repeatedMsg[i] * p + " " * (not p)
		end for
		p = not p
	end while
	print s.join("")
	wait 0.1	// OPTIONAL; slows printing down so you can see it all
end for
