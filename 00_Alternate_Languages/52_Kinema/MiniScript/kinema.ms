// Kinema
//
// Ported from BASIC to MiniScript by Joe Strout

print " "*33 + "KINEMA"
print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print; print; print

checkAnswer = function(prompt, correctValue)
	answer = input(prompt).val
	right = abs((answer - correctValue)/answer) < 0.15
	if right then
		print "Close enough!"
	else
		print "Not even close...."
	end if
	print "Correct answer is " + correctValue
	return right
end function

doOneRun = function
	print; print
	rightCount = 0
	V = 5 + floor(35*rnd)
	print "A ball is thrown upwards at " + V + " meters per second."
	print
	rightCount += checkAnswer("How high will it go (in meters)? ", 0.05 * V^2)
	rightCount += checkAnswer("How long until it returns (in seconds)? ", V/5)
	t = 1 + floor(2*V*rnd)/10
	rightCount += checkAnswer("What will its velocity be after " + t +
	     " seconds? ", V-10*t)
	print
	print rightCount + " right out of 3."
	if rightCount >= 2 then print "  Not bad."
end function

// main loop (press control-C to break out)
while true
	doOneRun
end while
