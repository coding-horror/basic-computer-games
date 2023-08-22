print " "*33 + "Hockey"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
// ROBERT PUOPOLO ALG. 1 140 MCCOWAN 6/7/73 HOCKEY

getYesNo = function(prompt)
	while true
		yn = input(prompt + "? ").lower
		if yn and yn[0] == "y" then return "yes"
		if yn and yn[0] == "n" then return "no"
		print "Answer yes or no!!"
	end while
end function

getTwoStrings = function(prompt)
	s = input(prompt + "? ").replace(", ", ",").split(",")
	answer1 = s[0]
	if s.len < 2 then
		answer2 = input("?? ")
	else
		answer2 = s[1]
	end if
	return [answer1, answer2]
end function

getNumber = function(prompt, minVal = 1, maxVal = 999)
	while true
		num = input(prompt + "? ").val
		if minVal <= num <= maxVal then return num
	end while
end function

printWithTab = function(s)
	print s.replace("\t", char(9))
end function

ha = [0] * 21
ta = [0] * 6
t1 = [0] * 6
t2 = [0] * 6
t3 = [0] * 6
aNames = [""] * 8	// Team 1 player names and team name
bNames = [""] * 8	// Team 2 player names and team name
x = 1
print; print; print
if getYesNo("Would you like the instructions") == "yes" then
	print
	print "This is a simulated hockey game."
	print "Question     Response"
	print "pass         Type in the number of passes you would"
	print "             like to make, from 0 to 3."
	print "shot         Type the number corresponding to the shot"
	print "             you want to make.  Enter:"
	print "             1 for a slapshot"
	print "             2 for a wristshot"
	print "             3 for a backhand"
	print "             4 for a snap shot"
	print "area         Type in the number corresponding to"
	print "             the area you are aiming at.  Enter:"
	print "             1 for upper left hand corner"
	print "             2 for upper right hand corner"
	print "             3 for lower left hand corner"
	print "             4 for lower right hand corner"
	print
	print "At the start of the game, you will be asked for the names"
	print "of your players.  They are entered in the order: "
	print "left wing, center, right wing, left defense,"
	print "right defense, goalkeeper.  Any other input required will"
	print "have explanatory instructions."
end if

setup = function
	teamNames = getTwoStrings("Enter the two teams")
	aNames[7] = teamNames[0]
	bNames[7] = teamNames[1]
	print
	globals.roundsInGame = getNumber("Enter the number of minutes in a game")
	print
	print "Would the " + aNames[7] + " coach enter his team"
	print
	for i in range(1, 6)
		aNames[i] = input("Player " + i + "? ")
	end for
	print
	print "Would the " + bNames[7] + " coach do the same"
	print
	for t in range(1, 6)
		bNames[t] = input("Player " + t + "? ")
	end for
	print
	rs = input("Input the referee for this game? ")
	print
	print " "*10 + aNames[7] + " starting lineup"
	for t in range(1, 6)
		print aNames[t]
	end for
	print
	print " "*10 + bNames[7] + " starting lineup"
	for t in range(1, 6)
		print bNames[t]
	end for
	print
	print "We're ready for tonights opening face-off."
	print rs + " will drop the puck between " + aNames[2] + " and " + bNames[2]
end function

shootAndScore = function(shootingTeam, shooter, asst1, asst2, z)
	while true
		ha[20] = floor(100 * rnd) + 1
		if ha[20] % z != 0 then break
		a2 = floor(100 * rnd) + 1
		if a2 % 4 == 0 then
			if shootingTeam == 1 then
				print "Save " + bNames[6] + "  -= 1  rebound"
			else
				print "Save " + aNames[6] + "  -= 1  follow up"
			end if
			continue
		end if
	end while

	if shootingTeam == 1 then
		print "Goal " + aNames[7]
		ha[9] += 1
	else
		print "Score " + bNames[7]
		ha[8] += 1
	end if
	print char(7) * 25
	print "Score: "
	if ha[8] <= ha[9] then
		printWithTab aNames[7] + ": " + ha[9] + "\t" + bNames[7] + ": " + ha[8]
	else
		printWithTab bNames[7] + ": " + ha[8] + "\t" + aNames[7] + ": " + ha[9]
	end if
	if shootingTeam == 1 then
		print "Goal scored by: " + aNames[shooter]
		if asst1 then
			if asst2 then
				print " assisted by: " + aNames[asst1] + " and " + aNames[asst2]
			else
				print " assisted by: " + aNames[asst1]
			end if
		else
			print " unassisted."
		end if
		ta[shooter] += 1
		t1[asst1] += 1
		t1[asst2] += 1
	else
		print "Goal scored by: " + bNames[shooter]
		if asst1 then
			if asst2 then
				print " assisted by: " + bNames[asst1] + " and " + bNames[asst2]
			else
				print " assisted by: " + bNames[asst1]
			end if
		else
			print " unassisted."
		end if
		t2[shooter] += 1
		t3[asst1] += 1
		t3[asst2] += 1
	end if
end function

shootBlocked = function(shootingTeam, shooter)
	s1 = floor(6 * rnd) + 1
	if shootingTeam == 1 then
		if s1 == 1 then
			print "Kick save and a beauty by " + bNames[6]
			print "cleared out by " + bNames[3]
			return false
		else if s1 == 2 then
			print "what a spectacular glove save by " + bNames[6]
			print "and " + bNames[6] + " golfs it into the crowd"
		else if s1 == 3 then
			print "skate save on a low steamer by " + bNames[6]
			return false
		else if s1 == 4 then
			print "pad save by " + bNames[6] + " off the stick"
			print "of " + aNames[shooter] + " and " + bNames[6] + " covers up"
		else if s1 == 5 then
			print "whistles one over the head of " + bNames[6]
			return false
		else if s1 == 6 then
			print bNames[6] + " makes a face save!! and he is hurt"
			print "the defenseman " + bNames[5] + " covers up for him"
		end if
	else
		if s1 == 1 then
			print "stick save by " + aNames[6] +""
			print "and cleared out by " + aNames[4]
			return false
		else if s1 == 2 then
			print "Oh my god!! " + bNames[shooter] + " rattles one off the post"
			print "to the right of " + aNames[6] + " and " + aNames[6] + " covers "
			print "on the loose puck!"
		else if s1 == 3 then
			print "Skate save by " + aNames[6]
			print aNames[6] + " whacks the loose puck into the stands"
		else if s1 == 4 then
			print "Stick save by " + aNames[6] + " and he clears it out himself"
			return false
		else if s1 == 5 then
			print "Kicked out by " + aNames[6]
			print "and it rebounds all the way to center ice"
			return false
		else if s1 == 6 then
			print "Glove save " + aNames[6] + " and he hangs on"
		end if
	end if
end function

doOneRound = function
	control = floor(2 * rnd) + 1
	if control == 1 then
		print aNames[7] + " has control of the puck"
	else
		print bNames[7] + " has control."
	end if
	p = getNumber("Pass", 0, 3)
	for n in range(1, 3)
		ha[n] = 0
	end for
	while true
		for j in range(1, p + 2)
			ha[j] = floor(5 * rnd) + 1
		end for
		if not (ha[j - 1] == ha[j - 2] or (p + 2 >= 3 and (ha[j - 1] == ha[j - 3] or ha[j - 2] == ha[j - 3]))) then break
	end while

	if p == 0 then
		s = getNumber("Shot", 1, 4)
		if control == 1 then
			print aNames[ha[j - 1]], ""
			g = ha[j - 1]
			g1 = 0
			g2 = 0
		else
			print bNames[ha[j - 1]], ""
			g2 = 0
			g2 = 0
			g = ha[j - 1]
		end if
		if s == 1 then
			print " lets a boomer go from the red line!!"
			z = 10
		else if s == 2 then
			print " flips a wristshot down the ice"
			// BUG: missing line 430 in the original caused it to fall through
			// to the s == 3 case.  We'll instead just do:
			z = 2
		else if s == 3 then
			print " backhands one in on the goaltender"
			z = 25
		else
			print " snaps a long flip shot"
			z = 17
		end if
	else
		if control == 1 then
			if p == 1 then
				print aNames[ha[j - 2]] + " leads " + aNames[ha[j - 1]] + " with a perfect pass."
				print aNames[ha[j - 1]] + " cutting in!!!"
				g = ha[j - 1]
				g1 = ha[j - 2]
				g2 = 0
				z1 = 3
			else if p == 2 then
				print aNames[ha[j - 2]] + " gives to a streaking " + aNames[ha[j - 1]]
				print aNames[ha[j - 3]] + " comes down on " + bNames[5] + " and " + bNames[4]
				g = ha[j - 3]
				g1 = ha[j - 1]
				g2 = ha[j - 2]
				z1 = 2
			else if p == 3 then
				print "oh my god!! a ' 4 on 2 ' situation"
				print aNames[ha[j - 3]] + " leads " + aNames[ha[j - 2]]
				print aNames[ha[j - 2]] + " is wheeling through center."
				print aNames[ha[j - 2]] + " gives and goest with " + aNames[ha[j - 1]]
				print "pretty passing!"
				print aNames[ha[j - 1]] + " drops it to " + aNames[ha[j - 4]]
				g = ha[j - 4]
				g1 = ha[j - 1]
				g2 = ha[j - 2]
				z1 = 1
			end if
		else
			if p == 1 then
				print bNames[ha[j - 1]] + " hits " + bNames[ha[j - 2]] + " flying down the left side"
				g = ha[j - 2]
				g1 = ha[j - 1]
				g2 = 0
				z1 = 3
			else if p == 2 then
				print "it's a ' 3 on 2 '!"
				print "only " + aNames[4] + " and " + aNames[5] + " are back."
				print bNames[ha[j - 2]] + " gives off to " + bNames[ha[j - 1]]
				print bNames[ha[j - 1]] + " drops to " + bNames[ha[j - 3]]
				g = ha[j - 3]
				g1 = ha[j - 1]
				g2 = ha[j - 2]
				z1 = 2
			else if p == 3 then
				print " a '3 on 2 ' with a ' trailer '!"
				print bNames[ha[j - 4]] + " gives to " + bNames[ha[j - 2]] + " who shuffles it off to"
				print bNames[ha[j - 1]] + " who fires a wing to wing pass to "
				print bNames[ha[j - 3]] + " aNames he cuts in alone!!"
				g = ha[j - 3]
				g1 = ha[j - 1]
				g2 = ha[j - 2]
				z1 = 1
			end if
		end if
		s = getNumber("Shot", 1, 4)
		if control == 1 then
			print aNames[g], ""
		else
			print bNames[g], ""
		end if
		if s == 1 then
			print " lets a big slap shot go!!"
			z = 4
			z += z1
		else if s == 2 then
			print " rips a wrist shot off"
			z = 2
			z += z1
		else if s == 3 then
			print " gets a backhand off"
			z = 3
			z += z1
		else
			print " snaps off a snap shot"
			z = 2
			z += z1
		end if
	end if
	
	a = getNumber("Area", 1, 4)	// area shot in
	a1 = floor(4 * rnd) + 1		// area vulnerable
	
	if control == 1 then
		globals.teamAShotsOnNet += 1
	else
		globals.teamBShotsOnNet += 1
	end if
	if a == a1 then
		shootAndScore control, g, g1, g2
	else
		shootBlocked control, g
	end if
	return true
end function

// Main program
setup
teamAShotsOnNet = 0
teamBShotsOnNet = 0
for l in range(1, roundsInGame)
	while not doOneRound; end while	// (repeat until it returns true)
	if l < roundsInGame then print "And we're ready for the face-off"
end for



print char(7)*30
print "That's the Siren"
print
print " "*15 + "Final Score:"
if ha[8] <= ha[9] then
	printWithTab aNames[7] + ": " + ha[9] + "\t" + bNames[7] + ": " + ha[8]
else
	printWithTab bNames[7] + ": " + ha[8] + "\t" + aNames[7] + ": " + ha[9]
end if
print
print " "*10 + "Scoring Summary"
print
print " "*25 + aNames[7]
printWithTab "\tName\tGoals\tAssists"
printWithTab "\t -= 1 -= 1\t -= 1 -= 1-\t -= 1 -= 1 -= 1-"
for i in range(1, 5)
	printWithTab "\t" + aNames[i] + "\t" + ta[i] + "\t" + t1[i]
end for
print
print " "*25 + bNames[7]
printWithTab "\tName\tGoals\tAssists"
printWithTab "\t -= 1 -= 1\t -= 1 -= 1-\t -= 1 -= 1 -= 1-"
for t in range(1, 5)
	printWithTab "\t" + bNames[t] + "\t" + t2[t] + "\t" + t3[t]
end for
print
print "Shots on net"
print aNames[7] + ": " + teamAShotsOnNet
print bNames[7] + ": " + teamBShotsOnNet
