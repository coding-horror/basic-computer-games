// TRAIN
//
// Converted from BASIC to MiniScript by Ryushinaka and Joe Strout

while true
	print "TRAIN"
	print "CREATIVE COMPUTER  MORRISTOWN, NEW JERSEY"
	print ""
	print "TIME - SPEED DISTANCE EXERCISE"
	
	carSpeed = floor(25*rnd + 40)
	difference = floor(15*rnd + 5)
	trainSpeed = floor(19*rnd + 20)
	print " A car traveling " + carSpeed + " MPH can make a certain trip in"
	print difference + " hours less than a train traveling at " + trainSpeed + " MPH."
	answer = input("How long does the trip take by car? ").val
	carTime = difference*trainSpeed/(carSpeed-trainSpeed)
	error = round(abs((carTime-answer)*100/answer))
	if error < 5 then
		print "GOOD! Answer within " + error + " PERCENT."
	else
		print "Sorry. You were off by " + floor(error) + " percent."
		print "Correct answer is " + round(carTime, 1) + " hours."
	end if
	
	answer = input("Another problem (YES or NO)? ")
	if not answer or answer[0].upper != "Y" then break
end while
