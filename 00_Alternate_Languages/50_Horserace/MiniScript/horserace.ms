print " "*31 + "Horserace"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

print "Welcome to South Portland High Racetrack"
print "                      ...owned by Laurie Chevalier"

// show directions, if wanted
x = input("Do you want directions? ").lower
if not x or x[0] != "n" then
	print "Up to 10 may play.  A table of odds will be printed.  You"
	print "may bet any + amount under 100000 on one horse."
	print "During the race, a horse will be shown by its"
	print "number.  The horses race down the paper!"
	print
end if

// get player names
qtyPlayers = input("How many want to bet? ").val
if qtyPlayers < 1 then exit
print "When ? appears, type name"
playerNames = [null]
for i in range(1, qtyPlayers)
	playerNames.push input("?")
end for
print
pick = [null] + [0]*qtyPlayers
bet = [null] + [0]*qtyPlayers

d = [0] * 9		// odds denominator
r = 0			// odds numerator

printInColumns = function(col0, col1, col2)
	print (col0+" "*16)[16] + (col1+" "*10)[:10] + (col2+" "*10)[:10]
end function

setup = function
	// initialize the horses
	globals.names = [null] +
	  "Joe Maw,L.B.J.,Mr.Washburn,Miss Karen,Jolly,Horse,Jelly Do Not,Midnight".split(",")
	for i in range(1,8)
		d[i] = floor(10*rnd+1)
	end for
	globals.r = d.sum

	// print odds table
	printInColumns "Horse", "Number", "Odds"
	for i in range(1,8)
		printInColumns names[i], i, round(r/d[i],2) + ":1"
	end for

	// get the players' bets
	print "--------------------------------------------------"
	print "Place your bets...Horse # then Amount"
	for j in range(1, qtyPlayers)
		while true
			s = input(playerNames[j] + "? ").replace(",", " ").split
			if s.len < 2 then s.push -1
			pick[j] = s[0].val
			bet[j] = s[-1].val
			if pick[j] < 1 or pick[j] > 8 or bet[j] < 1 or bet[j] >=100000 then
				print "You can't do that!"
			else
				break
			end if
		end while
	end for
end function

// Draw a racetrack, with each horse the given number
// of lines down from START to FINISH.
trackLen = 23
drawTrack = function(horsePositions)
	print
	if version.hostName == "Mini Micro" then clear else print char(27)+"c"
	if horsePositions[1] == 0 then print "1 2 3 4 5 6 7 8" else print
	print "XXXXSTARTXXXX"
	for row in range(1, trackLen)
		for h in range(1,8)
			p = horsePositions[h]
			if p > trackLen then p = trackLen
			if p == row then print h, " "
		end for
		print
	end for
	print "XXXXFINISHXXXX", ""
	if version.hostName != "Mini Micro" then print
end function

runRace = function
	pos = [0]*9
	maxPos = 0
	while true
		drawTrack pos
		wait 1
		if maxPos >= trackLen then break
		for i in range(1,8)
			q = floor(100*rnd+1)
			x = floor(r/d[i]+0.5)
			if q < 10 then
				speed = 1
			else if q < x+17 then
				speed = 2
			else if q < x+37 then
				speed = 3
			else if q < x+57 then
				speed = 4
			else if q < x+77 then
				speed = 5
			else if q < x+92 then
				speed = 6
			else
				speed = 7
			end if
			pos[i] += speed
			if pos[i] > maxPos then maxPos = pos[i]
		end for
	end while

	print
	print "---------------------------------------------"
	print
	print "The race results are:"
	results = []
	for i in range(1,8)
		results.push {"num":i, "pos":pos[i]}
	end for
	results.sort "pos", false
	for place in range(1, 8)
		h = results[place-1].num
		print " " + place + " Place Horse No. " + h + "   at " + round(r/d[h],2) + ":1"
		print
	end for
	for p in range(1, qtyPlayers)
		if pick[p] == results[0].num then
			print playerNames[p] + " wins $" + round((r/d[pick[p]])*bet[p], 2)
		end if
	end for
end function

// Main loop
while true
	setup
	runRace
	print "Do you want to bet on the next race ?"
	yn = input("Yes or no? ").lower
	if not yn or yn[0] != "y" then break
end while

