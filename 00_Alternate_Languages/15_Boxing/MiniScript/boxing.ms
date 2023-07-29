print " "*33 + "Boxing"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "Boxing Olympic Style (3 Rounds -- 2 out of 3 Wins)"

playerWins = 0
opponentWins = 0
print
opponentName = input("What is your opponent's name? ")
playerName = input("Input your man's name? ")
print "Different punches are: (1) full swing; (2) hook; (3) uppercut; (4) jab."
playerBest = input("What is your man's best? ").val
playerWeakness = input("What is his vulnerability? " ).val
while true
	opponentBest = floor(4 * rnd + 1)
	opponentWeakness = floor(4 * rnd + 1)
	if opponentBest != opponentWeakness then break
end while
print opponentName + "'s advantage is " + opponentBest + " and vulnerability is secret."
print

playerConnects = function
	print "He connects!"
	if playerPoints > 35 then
		print opponentName + " is knocked cold and " + playerName + " is the winner and champ!"
		globals.done = true
		return
	end if
	globals.playerPoints += 15
end function

doPlayerPunch = function
	p = input(playerName + "'s punch? ").val
	if p == playerBest then globals.playerPoints += 2
	if p == 1 then		// Full Swing
		print playerName + " swings and ", ""
		if opponentWeakness == 4 then	// (probably a bug in original code)
			playerConnects
		else
			x3 = floor(30 * rnd+1)
			if x3 < 10 then
				playerConnects
			else
				print "he misses "
				if playerPoints != 1 then
					print
					print
				end if
			end if
		end if
	else if p == 2 then	// Hook
		print playerName + " gives the hook... ", ""
		if opponentWeakness == 2 then
			globals.playerPoints += 7
		else
			h1 = floor(2 * rnd + 1)
			if h1 == 1 then
				print "But it's blocked!!!!!!!!!!!!!"
			else
				print "Connects..."
				globals.playerPoints += 7
			end if
		end if
	else if p == 3 then	// Uppercut
		print playerName + " tries an uppercut ", ""
		if opponentWeakness == 3 or floor(100 * rnd + 1) < 51 then
			print "and he connects!"
			globals.playerPoints += 4
		else
			print "and it's blocked (lucky block!)"
		end if
	else				// Jab
		print playerName + " jabs at " + opponentName + "'s head ", ""
		if opponentWeakness != 4 and floor(8 * rnd + 1) >= 4 then
			print "It's blocked."
		else
			globals.playerPoints += 3
		end if
	end if
end function

playerKnockedOut = function
	print playerName + " is knocked cold and " + opponentName + " is the winner and champ!"
	globals.done = true
end function

doOpponentPunch = function
	j7 = floor(4 * rnd + 1)
	if j7 == playerBest then globals.opponentPoints += 2
	if j7 == 1 then			// Full swing
		print opponentName + " takes a full swing and ", ""
		if playerWeakness == 1 or floor(60 * rnd + 1) < 30 then
			print "POW!!!!! He hits him right in the face!"
			if opponentPoints > 35 then
				playerKnockedOut
			else
				globals.opponentPoints += 15
			end if
		else
			print "it's blocked!"
		end if
	end if
	if j7 == 2 then	// Hook
		print opponentName + " gets " + playerName + " in the jaw (ouch!)"
		globals.playerPoints += 7
		print "....and again!"
		globals.playerPoints += 5
		if opponentPoints > 35 then
			playerKnockedOut
			return
		end if
		print
		// continue below as if an Uppercut (probably a bug in the original code)
	end if
	if j7 == 2 or j7 == 3 then	// Uppercut, or Hook
		print playerName + " is attacked by an uppercut (oh,oh)..."
		if playerWeakness == 3 or floor(200*rnd+1) <= 75 then
			print "and " + opponentName + " connects..."
			globals.opponentPoints += 8					
		else
			print " blocks and hits " + opponentName + " with a hook."
			globals.playerPoints += 5
		end if
	end if
	if j7 == 4 then			// Jab
		print opponentName + " jabs and ", ""
		if playerWeakness == 4 or floor(7 * rnd + 1) > 4 then
			print "blood spills !!!"
			globals.opponentPoints += 5
		else
			print "It's blocked!"
		end if				
	end if
end function

playOneRound = function
	globals.playerPoints = 0
	globals.opponentPoints = 0
	print "Round " + round + " begins..."
	for r1 in range(1, 7)
		i = floor(10 * rnd + 1)
		if i <= 5 then
			doPlayerPunch
		else
			doOpponentPunch
		end if
		if done then return
	end for		// next R1 (sub-round)
	if playerPoints > opponentPoints then
		print; print playerName + " wins round " + round
		globals.playerWins += 1
	else
		print; print opponentName + " wins round " + round
		globals.opponentWins += 1
	end if
end function

done = false
for round in range(1,3)
	playOneRound
	if done then break
	if opponentWins >= 2 then
		print opponentName + " wins (nice going, " + opponentName + ")."
		break
	else if playerWins >= 2 then
		print playerName + " amazingly wins!!"
		break
	end if
end for	// next round

print
print 
print "and now goodbye from the Olympic arena."
print
