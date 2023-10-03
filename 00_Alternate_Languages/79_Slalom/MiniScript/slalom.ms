print " "*33 + "Slalom"
print " "*15 + "Creative Computing  Morristown  New Jersey"
print; print; print

gateSpeeds = [14,18,26,29,18,25,28,32,29,20,29,29,25,21,26,29,20,21,20,
		 18,26,25,33,31,22]

medals = {}
medals.gold = 0
medals.silver = 0
medals.bronze = 0

while true
	qtyGates = input("How many gates does this course have (1 to 25)? ").val
	if qtyGates > 25 then
		print "25 is the limit."
		qtyGates = 25
	end if
	if qtyGates >= 1 then break
	print "Try again,"
end while

print "Type ""ins"" for instructions"
print "Type ""max"" for approximate maximum speeds"
print "Type ""run"" for the beginning of the race"
while true
	cmd = input("Command--").lower
	if cmd == "ins" then
		print
		print "*** Slalom: This is the 1976 Winter Olympic Giant Slalom.  You are"
		print "            the American team's only hope of a gold medal."
		print
		print "     0 -- type this if you want to see how long you've taken."
		print "     1 -- type this if you want to speed up a lot."
		print "     2 -- type this if you want to speed up a little."
		print "     3 -- type this if you want to speed up a teensy."
		print "     4 -- type this if you want to keep going the same speed."
		print "     5 -- type this if you want to check a teensy."
		print "     6 -- type this if you want to check a little."
		print "     7 -- type this if you want to check a lot."
		print "     8 -- type this if you want to cheat and try to skip a gate."
		print
		print " The place to use these options is when the computer asks:"
		print
		print "Option?"
		print
		print "                Good luck!"
		print
	else if cmd == "max" then
		print "GATE  MAX"
		print " #   M.P.H."
		print "-----------"
		for i in range(1, qtyGates)
			print " " + i + " "*(i<10) + "   " + gateSpeeds[i-1]
		end for
	else if cmd == "run" then
		break
	end if
end while

while true
	skill = input("Rate yourself as a skier, (1=worst, 3=best)? ").val
	if 1 <= skill <= 3 then break
	print "The bounds are 1-3"
end while

doOneRace = function
	print "The starter counts down...5...4...3...2...1..GO!"
	time = 0
	speed = floor(rnd * 9 + 9)
	print
	print "You're off!"
	gate = 0
	while gate+1 <= qtyGates
		gate += 1
		gateSpeed = gateSpeeds[(gate-1) % gateSpeeds.len]
		print
		print "Here comes gate #" + gate + ":"
		print speed + " M.P.H."
		prevSpeed = speed
		while true
			opt = input("Option? ").val
			if opt == 0 then
				print "You've taken " + time + " seconds."
			else if opt < 1 or opt > 8 then
				print "What?"
			else
				break
			end if
		end while
		if opt == 1 then
			speed += floor(rnd*(10-5)+5)
		else if opt == 2 then
			speed += floor(rnd*(5-3)+3)
		else if opt == 3 then
			speed += floor(rnd*(4-1)+1)
		else if opt == 4 then
			// (no change)
		else if opt == 5 then
			speed -= floor(rnd*(4-1)+1)
		else if opt == 6 then
			speed -= floor(rnd*(5-3)+3)
		else if opt == 7 then
			speed -= floor(rnd*(10-5)+5)
		else if opt == 8 then
			print "***CHEAT"
			if rnd < 0.7 then
				print "An official caught you!"
				print "You took " + round(time+rnd, 3) + " seconds."
				break
			else
				print "You made it!" + char(7)
				time += 1.5
				continue
			end if
		end if
		print speed + " M.P.H."
		if speed > gateSpeed then
			if rnd < (speed - gateSpeed)*0.1 + 0.2 then
				msg = "You went over the maximum speed and "
				if rnd < 0.5 then msg += "snagged a flag!" else msg += "wiped out!"
				print msg
				print "You took " + round(time+rnd, 3) + " seconds."
			else
				print "You went over the maximum speed and made it!"
			end if
		else if speed > gateSpeed - 1 then
			print "Close one!"
		end if
		if speed < 7 then
			print "Let's be realistic, OK?  Let's go back and try again..."
			speed = prevSpeed
			gate -= 1
			continue
		end if
		time += gateSpeed - speed + 1
		if speed > gateSpeed then time += 0.5
	end while

	print
	print "You took " + round(time+rnd, 3) + " seconds."
	avg = time / qtyGates
	if avg < 1.5 - (skill * 0.1) then
		print "You won a gold medal!"
		medals.gold += 1	
	else if avg < 2.9 - (skill * 0.1) then
		print "You won a silver medal"
		medals.silver += 1	
	else if avg < 4.4 - (skill * 0.01) then
		print "You won a bronze medal"
		medals.bronze += 1	
	end if
end function

while true
	doOneRace
	while true
		yesno = input("Do you want to race again? ").lower + " "
		if yesno[0] == "y" or yesno[0] == "n" then break
		print "Please type 'yes' or 'no'"
	end while
	if yesno[0] == "n" then break
	print
end while

print
print "Thanks for the race"
if medals.gold then print "Gold medals: " + medals.gold
if medals.silver then print "Silver medals: " + medals.silver
if medals.bronze then print "Bronze medals: " + medals.bronze
