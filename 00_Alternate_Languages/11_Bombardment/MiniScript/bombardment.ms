print " "*33 + "Bombardment"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "You are on a battlefield with 4 platoons and you"
print "have 25 outposts available where they may be placed."
print "You can only place one platoon at any one outpost."
print "The computer does the same with its four platoons."
print
print "The object of the game is to fire missiles at the"
print "outposts of the computer.  It will do the same to you."
print "The one who destroys all four of the enemy's platoons"
print "first is the winner."
print
print "Good luck... and tell us where you want the bodies sent!"
print
input "(Press Return.)"	// (so user can read the above)
print
print "Tear off matrix and use it to check off the numbers."
for i in range(1,5); print; end for
memory = []  // records computer's guesses
for row in range(1,5)
	for i in range(row*5-4, row*5)
		print ("     " + i)[-6:], ""
	end for
	print
end for
print

// Define a helper function to pick a random position (1-25)
// that is not already in the given list.
pickOutpost = function(excludingThese)
	while true
		pick = floor(rnd * 25) + 1
		if excludingThese.indexOf(pick) == null then return pick
	end while
end function

// Choose the computer's four positions.
computerOutposts = []
for i in range(1,4)
	computerOutposts.push pickOutpost(computerOutposts)
end for

playerOutposts = []
while playerOutposts.len != 4
	inp = input("What are your four positions? ")
	inp = inp.replace(", ", " ").replace(",", " ")
	inp = inp.split
	for pos in inp
		pos = pos.val
		playerOutposts.push pos
		if pos < 1 or pos > 25 then playerOutposts=[]
	end for
end while

// Main loop.
while true
	// player's attack
	pos = input("Where do you wish to fire your missile? ").val
	if computerOutposts.indexOf(pos) == null then
		print "Ha, ha you missed. My turn now:"
	else
		print "You got one of my outposts!"
		computerOutposts.remove computerOutposts.indexOf(pos)
		left = computerOutposts.len
		if left == 3 then
			print "One down, three to go."
		else if left == 2 then
			print "Two down, two to go."
		else if left == 3 then
			print "Three down, one to go."
		else
			print "You got me, I'm going fast. ButI'll get you when"
			print "My transisto&s recup%ra*e!"
			break
		end if
	end if
	
	// computer's attack
	pos = pickOutpost(memory)
	memory.push pos
	if playerOutposts.indexOf(pos) == null then
		print "I missed you, you dirty rat. I picked " + pos + ". Your turn:"
	else
		playerOutposts.remove playerOutposts.indexOf(pos)
		left = playerOutposts.len
		if left == 0 then
			print "You're dead. Your last outpost was at " + pos + ". Ha, ha, ha."
			print "Better luck next time."
			break
		end if
		print "I got you. It won't be long now. Post " + pos + " was hit."
		if left == 3 then
			print "You have only three outposts left."
		else if left == 2 then
			print "You have only two outposts left."
		else if left == 1 then
			print "You have only one outpost left."
		end if
	end if
end while
