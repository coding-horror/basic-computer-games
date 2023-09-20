print " "*26 + "Civil War"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
// ORIGINAL GAME DESIGN: CRAM, GOODIE, HIBBARD LEXINGTON H.S.
// MODIFICATIONS: G. PAUL, R. HESS (TIES), 1973
// Conversion to MiniScript: J. Strout, 2023

sides = ["Confederate", "Union"]

getYesNo = function(prompt)
	while true
		yn = input(prompt + "? ").lower
		if yn and yn[0] == "y" then return "YES"
		if yn and yn[0] == "n" then return "NO"
		print "Yes or no -- "
	end while
end function

instructions = function
	print; print; print; print
	print "This is a civil war simulation."
	print "To play type a response when the computer asks."
	print "Remember that all factors are interrelated and that your"
	print "responses could change history. Facts and figures used are"
	print "based on the actual occurrence. Most battles tend to result"
	print "as they did in the civil war, but it all depends on you!!"
	print
	print "The object of the game is to win as many battles as ";
	print "possible."
	print
	print "Your choices for defensive strategy are:"
	print "        (1) artillery attack"
	print "        (2) fortification against frontal attack"
	print "        (3) fortification against flanking maneuvers"
	print "        (4) falling back"
	print " Your choices for offensive strategy are:"
	print "        (1) artillery attack"
	print "        (2) frontal attack"
	print "        (3) flanking maneuvers"
	print "        (4) encirclement"
	print "You may surrender by typing a '5' for your strategy."
end function

historicalBattles = []
addBattle = function(name, men0, men1, cas0, cas1, offDef, desc)
	battle = {}
	battle.name = name
	battle.men = [men0, men1]
	battle.casualties = [cas0, cas1]
	battle.offDef = offDef		// 1=Confederate Defense, 2=both Offense, 3=Confederate Offense
	battle.description = desc
	historicalBattles.push battle
end function

loadData = function
	// Historical data
	addBattle "Bull Run",18000,18500,1967,2708,1, [
"July 21, 1861.  Gen. Beauregard, commanding the South, met",
"Union forces with Gen. Mcdowell in a premature battle at",
"Bull Run. Gen. Jackson helped push back the Union attack."]

	addBattle "Shiloh",40000.,44894.,10699,13047,3, [
"April 6-7, 1862.  The Confederate surprise attack at",
"Shiloh failed due to poor organization."]

	addBattle "Seven Days",95000.,115000.,20614,15849,3, [
"June 25-july 1, 1862.  General Lee (CSA) upheld the",
"offensive throughout the battle and forced Gen. McClellan",
"and the Union forces away from Richmond."]

	addBattle "Second Bull Run",54000.,63000.,10000,14000,2, [
"Aug 29-30, 1862.  The combined Confederate forces under Lee",
"and Jackson drove the Union forces back into washington."]

	addBattle "Antietam",40000.,50000.,10000,12000,3, [
"Sept 17, 1862.  The South failed to incorporate Maryland",
"into the Confederacy."]

	addBattle "Fredericksburg",75000.,120000.,5377,12653,1, [
"Dec 13, 1862.  The Confederacy under Lee successfully",
"repulsed an attack by the Union under Gen. Burnside."]

	addBattle "Murfreesboro",38000.,45000.,11000,12000,1, [
"Dec 31, 1862.  The South under Gen. Bragg won a close battle."]

	addBattle "Chancellorsville",32000,90000.,13000,17197,2, [
"May 1-6, 1863.  The South had a costly victory and lost",
"one of their outstanding generals, 'Stonewall' Jackson."]

	addBattle "Vicksburg",50000.,70000.,12000,19000,1, [
"July 4, 1863.  Vicksburg was a costly defeat for the South",
"because it gave the Union access to the Mississippi."]

	addBattle "Gettysburg",72500.,85000.,20000,23000,3, [
"July 1-3, 1863.  A Southern mistake by Gen. Lee at Gettysburg",
"cost them one of the most crucial battles of the war."]

	addBattle "Chickamauga",66000.,60000.,18000,16000,2, [
"Sept. 15, 1863. Confusion in a forest near Chickamauga led",
"to a costly Southern victory."]

	addBattle "Chattanooga",37000.,60000.,36700.,5800,2, [
"Nov. 25, 1863. After the South had sieged Gen. Rosencrans'",
"army for three months, Gen. Grant broke the siege."]

	addBattle "Spotsylvania",62000.,110000.,17723,18000,2, [
"May 5, 1864.  Grant's plan to keep Lee isolated began to",
"fail here, and continued at Cold Harbor and Petersburg."]

	addBattle "Atlanta",65000.,100000.,8500,3700,1, [
"August, 1864.  Sherman and three veteran armies converged",
"on Atlanta and dealt the death blow to the Confederacy."]

end function

setup = function
	print; print; print
	if getYesNo("Are there two generals present") == "YES" then
		globals.numPlayers = 2
	else
		globals.numPlayers = 1
		print; print "You are the Confederacy.  Good luck!"
		print
	end if
	print "Select a battle by typing a number from 1 to 14 on"
	print "request.  Type any other number to end the simulation."
	print "But '0' brings back exact previous battle situation"
	print "allowing you to replay it."
	print
	print "Note: a negative food$ entry causes the program to "
	print "use the entries from the previous battle."
	print
	yn = getYesNo("After requesting a battle, do you wish battle descriptions")
	globals.battleDesc = (yn == "YES")
end function

// Game variables -- the 2-element arrays below represent the stats for
// player 0 (Confederacy) and 1 (Union)
D = [0,0]		// money
F = [0,0]		// food budget
H = [0,0]		// salary budget
B = [0,0]		// ammo budget
men = [0,0]		// men at start of battle?  (M1, M2)
menAdded = [0,0]	// men added (M3, M4)
menAvail = [0,0]	// men available (M5, M6)
P = [0,0]	// casualties?
T = [0,0]	// total losses?
resources = [0,0]	// total resources (money) available to each side (R1, R2)
spending = [0,0]	// total expenditures for each side (Q1, Q2)
morale = [0,0]		// troop morale (O)
strategy = [0,0]	// strategy choice (Y1, Y2)
inflation = [0,0]	// inflation (I1, I2)

R = 0			// previous battle
losses = 0		// Confederate losses
wins = 0		// Confederate wins
unresolved = 0	// battles where neither side clearly won

printInColumns = function(a, b, c)
	print (a + " "*20)[:20] + (b + " "*16)[:16] + c
end function

pickBattle = function
	print; print; print
	globals.replay = false
	while true
		num = input("Which battle do you wish to simulate? ").val
		if num == 0 and R > 0 then
			num = R
			globals.replay = true
		end if
		if 0 < num <= historicalBattles.len then break
	end while
	globals.bat = historicalBattles[num - 1]
	if not replay then
		men[0] = bat.men[0]
		men[1] = bat.men[1]
		
		// inflation calc
		inflation[0] = 10 + (losses - wins) * 2
		inflation[1] = 10 + (wins - losses) * 2

		// money available
		D[0] = 100 * floor((men[0]*(100-inflation[0])/2000) * (1+(resources[0]-spending[0])/(resources[0]+1))+0.5)
		D[1] = 100 * floor(men[1]*(100-inflation[1])/2000 + 0.5)
		if numPlayers == 2 then
			D[1] = 100 * floor((men[1]*(100-inflation[1])/2000) * (1+(resources[1]-spending[1])/(resources[1]+1))+0.5)
		end if
		
		// men available
		menAvail[0] = floor(men[0]*(1 + (P[0]-T[0])/(menAdded[0]+1)))
		menAvail[1] = floor(men[1]*(1 + (P[1]-T[1])/(menAdded[1]+1)))
		globals.F1 = 5/6 * men[0]	// ?!?
		print; print; print; print; print
print "P:" + P + "; T:" + T + "; menAdded:" + menAdded
print "men[0]:" + men[0] + "   ...F1:" + F1

		print "This is the battle of " + bat.name
		if battleDesc then
			for line in bat.description
				print line
			end for
		end if
	else
		print bat.name + " Instant Replay"
	end if
	print
	printInColumns " ", "CONFEDERACY", " UNION"
	printInColumns "MEN", "  "+menAvail[0], "  "+menAvail[1]
	printInColumns "MONEY", "$ "+D[0], "$ "+D[1]
	printInColumns "INFLATION", "  "+(inflation[0]+15)+" %", "  " +inflation[1]+" %"
	// (Note: printout lies and shows confederate inflation 15% higher than actual)
	print
end function

getBudget = function(player)
	print sides[player] + " General---How much do you wish to spend for"
	getNum = function(prompt, allowNegative)
		while true
			n = input(prompt)
			if not n then continue
			if n[-1] == "k" then n = n[:-1] + "000"	// (allow entries like "60k")
			if n.val >= 0 or allowNegative then return n.val
			print "Negative values are not allowed."
		end while
	end function
	
	while true
		f = getNum(" - Food......? ", true)
		if f < 0 then
			if resources[player] == 0 then
				print "No previous entries."
				print "How much do you wish to spend for"
				continue
			end if
			break	// keep all previous entries
		end if
		F[player] = f
		H[player] = getNum(" - Salaries..? ", false)
		B[player] = getNum(" - Ammunition? ", false)
		if F[player] + H[player] + B[player] <= D[player] then break
		print "Think again! You have only $" + D[player]
	end while	
end function

calcMorale = function(player)
	m = ((2*F[player]^2 + H[player]^2) / F1^2 + 1)
	print (" "*11 + sides[player])[-11:], " "
	if m >= 10 then
		print "morale is high"
	else if m >= 5 then
		print "morale is fair"
	else
		print "morale is poor"
	end if
	morale[player] = m
end function

getStrategy = function(player)
	if numPlayers == 1 then	prompt = "Your strategy" else prompt = sides[player] + " strategy"
	while true
		strat = input(prompt + "? ").val
		if 0 < strat < 6 then break
		print "Strategy " + strat + " not allowed."
	end while
	strategy[player] = strat
	if strat == 5 then
		print "The " + ["Confederacy", "Union"][player] + " has surrendered."
		globals.gameOver = true
	end if
end function

calcComputerStrategy = function
	// Union strategy is computer chosen
	print "Union strategy is ", ""
	s0 = 0
	r = 100 * rnd
	for i in range(0, 3)
		s0 += unionStrats[i]
		if r < s0 then
			strategy[1] = i+1
			break
		end if
	end for
	print strategy[1]
end function

learnStrategy = function
	// Learn present strategy, start forgetting old ones.
	// - Present strategy of south gains 3*s, others lose s
	//   probability points, unless a strategy falls below 5%.
	s = 3; s0 = 0
	for i in range(0,3)
		if unionStrats[i] < 5 then continue
		unionStrats[i] -= s
		s0 += s
	end for
	unionStrats[strategy[1]-1] += s0	
end function

doBattle = function
	U = 0; U2 = 0
	// simulated losses -- North
	C6 = (2 * bat.casualties[1]/5) * (1+1/(2*(abs(strategy[1]-strategy[0])+1)))
	C6 = C6 * (1.28 + (5*men[1]/6) / (B[1]+1))
	C6 = floor(C6 * (1+1/morale[1]) + .5)
	// - IF LOSS > MEN PRESENT, RESCALE LOSSES
	E2 = 100/morale[1]		// desertions (Union)
	if floor(C6 + E2) >= menAvail[1] then
		C6 = floor(13*menAvail[1]/20)
		E2 = 7 * C6/13
		U2=1	// Union loss
	end if
	// simulated losses -- South
	C5 = (2 * bat.casualties[0]/5) * (1+1/(2*(abs(strategy[1]-strategy[0])+1)))
	print "step A:" + C5
	C5 = floor(C5 * (1+1/morale[0])*(1.28+F1/(B[0]+1))+.5)
	print "step B:" + C5
	print "based on morale[0]:"+morale[0]+", F1:"+F1+", B[0]:"+B[0]
	E=100/morale[0]
	if C5+100/morale[0] >= men[0]*(1+(P1-T1)/(menAdded[0]+1)) then
		C5 = floor(13*men[0]/20 * (1+(P1-T1)/(menAdded[0]+1)))
	print "step C:" + C5
	print "men: " + men; print "menAdded: " + menAdded; print "P1,T1:" + P1 + "," + T1
		E=7*C5/13		// desertions (Confed)
		U=1			// Confederate loss
	end if
	print
	print; print; printInColumns "", "CONFEDERACY", "UNION"
	if numPlayers == 1 then
		C6 = floor(17 * bat.casualties[1] * bat.casualties[0] / (C5*20))
		E2 = 5 * morale[0]
	end if
	printInColumns "CASUALTIES", C5, C6
	printInColumns "DESERTIONS", floor(E), floor(E2)
	print
	if numPlayers == 2 then
		print "Compared to the actual casualties at " + bat.name
		print "Confederate: " + round(100*C5/bat.casualties[0]) + "% of the original"
		print "Union:       " + round(100*C6/bat.casualties[1]) + "% of the original"
		print
		// Who won?
		print "U:"+U+ "   U2:"+U2
		if (U == 1 and U2 != 1) or (U == U2 and C5+E > C6+E2) then
			print "The Union wins " + bat.name
			globals.losses += 1
		else if (U2 == 1 and U != 1) or (U == U2 and C5+E < C6+E2) then
			print "The confederacy wins " + bat.name
			globals.wins += 1
		else
			print "Battle outcome unresolved"
			globals.unresolved += 1
		end if
	else
		print "Your casualties were " + round(100*C5/bat.casualties[0]) + "% of"
		print "the actual casualties at " + bat.name
		print
		if U == 1 or C5+E >= 17*bat.casualties[1]*bat.casualties[0]/(C5*20)+5*morale[0] then
			print "You lose " + bat.name
			if not replay then globals.losses += 1
		else
			print "You win " + bat.name
			if not replay then globals.wins += 1
		end if
	end if
	if not replay then
		// Cumulative battle factors which alter historical
		// resources availeble.  (If a replay, don't update.)
		T[0] += C5 + E
		T[1] += C6 + E2
		P[0] += bat.casualties[0]
		P[1] += bat.casualties[1]
		spending[0] += F[0] + H[0] + B[0]
		spending[1] += F[1] + H[1] + B[1]
		resources[0] += men[0]*(100-inflation[0])/20
		resources[1] += men[1]*(100-inflation[1])/20
		menAdded[0] += men[0]
		menAdded[1] += men[1]

		learnStrategy
	end if
	print "---------------"
end function

// Main Program
loadData
unionStrats = [25, 25, 25, 25]
P1=0; P2=0; T1=0; T2=0		// cumulative stat thingies
print
if getYesNo("Do you want instructions") == "YES" then instructions
setup
gameOver = false
while not gameOver
	pickBattle
	if gameOver then break
	for i in range(0, numPlayers-1)
		getBudget i
	end for
	for i in range(0, numPlayers-1)
		calcMorale i
	end for
	print "Confederate General---", ""
	if bat.offDef == 3 then
		print "you are on the offensive"
	else if bat.offDef == 1 then
		print "you are on the defensive"
	else
		print "both sides are on the offensive "
	end if
	print
	getStrategy 0
	if numPlayers == 2 then getStrategy 1 else calcComputerStrategy
	if gameOver then break
	doBattle
end while

// Finish off
print; print; print; print; print; print
print "The Confederacy has won " + wins + " battles and lost " + losses
if strategy[0] == 5 or (strategy[1] != 5 and losses > wins) then
	print "The Union has won the war"
else
	print "The Confederacy has won the war"
end if
print "For the " + (wins + losses + unresolved) + " battles fought (excluding reruns)"
print
printInColumns "", "Confederacy", "Union"
printInColumns "Historical Losses", round(P[0]), round(P[1])
printInColumns "Simulated Losses", round(T[0]), round(T[1])
print
printInColumns "    % of Original", round(100*T[0]/P[0]), round(100*T[1]/P[1])
if numPlayers == 1 then
	print
	print "Union intelligence suggsets that the South used "
	print "strategies 1, 2, 3, 4 in the following percentages"
	print unionStrats.join
end if
