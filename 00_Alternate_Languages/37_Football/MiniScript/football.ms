player_data = [17,8,4,14,19,3,10,1,7,11,15,9,5,20,13,18,16,2,12,6,
                   20,2,17,5,8,18,12,11,1,4,19,14,10,7,9,15,6,13,16,3]
aa = [0]*21
ba = [0]*21
ca = [0]*41
ha = [0]*3
ta = [0]*3
wa = [0]*3
xa = [0]*3
ya = [0]*3
za = [0]*3
ms = [null, "",""]
da = [0]*3
ps = ["", "PITCHOUT","TRIPLE REVERSE","DRAW","QB SNEAK","END AROUND",
          "DOUBLE REVERSE","LEFT SWEEP","RIGHT SWEEP","OFF TACKLE",
          "WISHBONE OPTION","FLARE PASS","SCREEN PASS",
          "ROLL OUT OPTION","RIGHT CURL","LEFT CURL","WISHBONE OPTION",
          "SIDELINE PASS","HALF-BACK OPTION","RAZZLE-DAZZLE","BOMB!!!!"]
globals.p = 0
globals.t = 0

printFieldHeaders = function
    print "TEAM 1 [0   10   20   30   40   50   60   70   80   90  100] TEAM 2"
    print
end function

printSeparator = function
	print "+" * 67
end function

showBall = function
    print " " * (da[t] + 5 + p / 2) + ms[t]
    printFieldHeaders
end function

showScores = function
    print
    print "TEAM 1 SCORE IS " + ha[1]
    print "TEAM 2 SCORE IS " + ha[2]
    print
    if ha[t] >= e then
        print "TEAM " + t + " WINS*******************"
        return true
    end if
    return false
end function

losePossession = function
    print
    print "** LOSS OF POSSESSION FROM TEAM " + t + " TO TEAM " + ta[t]
    print
    printSeparator
    print
    globals.t = ta[t]
end function

touchdown = function
    print
    print "TOUCHDOWN BY TEAM " + t + " *********************YEA TEAM"
    q = 7
    if rnd <= 0.1 then
        q = 6
        print "EXTRA POINT NO GOOD"
    else
        print "EXTRA POINT GOOD"
    end if
    ha[t] += q
end function

askYesNo = function(prompt)
	while true
		yn = input(prompt + "? ").lower
		if not yn then continue
		if yn[0] == "y" then return "YES"
		if yn[0] == "n" then return "NO"
	end while
end function


print " "*32 + "FOOTBALL"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "Presenting N.F.U. Football (No FORTRAN Used)"
print; print
if askYesNo("Do you want instructions") == "YES" then
	print "This is a football game for two teams in which players must"
	print "prepare a tape with a data statement (1770 for team 1,"
	print  "1780 for team 2) in which each team scrambles nos. 1-20"
	print "These numbers are then assigned to twenty given plays."
	print "A list of nos. and their plays is provided with"
	print "both teams having the same plays. The more similar the"
	print "plays the less yardage gained.  Scores are given"
	print "whenever scores are made. Scores may also be obtained"
	print "by inputting 99,99 for play nos. To punt or attempt a"
	print "field goal, input 77,77 for play numbers. Questions will be"
	print "asked then. On 4th down, you will also be asked whether"
	print "you want to punt or attempt a field goal. If the answer to"
	print "both questions is no it will be assumed you want to"
	print "try and gain yardage. Answer all questions Yes or No."
	print "The game is played until players terminate (control-c)."
	print "Please prepare a tape and run."
end if
print
e = input("Please input score limit on game: ").val
for i in range(1, 40)
	if i <= 20 then
		aa[player_data[i - 1]] = i
	else
		ba[player_data[i - 1]] = i - 20
	end if
	ca[i] = player_data[i - 1]
end for
l = 0
globals.t = 1
while true
	print "TEAM " + t + " PLAY CHART"
	print "NO.      PLAY"
	for i in range(1, 20)
		print (ca[i+1] + " "*6)[:6] + ps[i]
	end for
	l += 20
	globals.t = 2
	print
	print "TEAR OFF HERE----------------------------------------------"
	for x in range(1, 11); print; end for
	wait 3
	if l != 20 then break
end while

playGame = function
	da[1] = 0
	da[2] = 3
	ms[1] = "--->"
	ms[2] = "<---"
	ha[1] = 0
	ha[2] = 0
	ta[1] = 2
	ta[2] = 1
	wa[1] = -1
	wa[2] = 1
	xa[1] = 100
	xa[2] = 0
	ya[1] = 1
	ya[2] = -1
	za[1] = 0
	za[2] = 100
	globals.p = 0
	printFieldHeaders
	print "TEAM 1 defend 0 YD goal -- TEAM 2 defends 100 YD goal."
	globals.t = floor(2 * rnd + 1)
	print
	print "The coin is flipped"
	routine = 1
	while true
		if routine <= 1 then
			globals.p = xa[t] - ya[t] * 40
			printSeparator
			print
			print "Team " + t + " receives kick-off"
			k = floor(26 * rnd + 40)
		end if
		if routine <= 2 then
			globals.p = p - ya[t] * k
		end if
		if routine <= 3 then
			if wa[t] * p >= za[t] + 10 then
				print
				print "Ball went out of endzone --automatic touchback--"
				globals.p = za[t] - wa[t] * 20
				if routine <= 4 then routine = 5
			else
				print "Ball went " + k + " yards.  Now on " + p
				showBall
			end if
		end if
		if routine <= 4 then
			if askYesNo("Team " + t + " do you want to runback") == "YES" then
				k = floor(9 * rnd + 1)
				r = floor(((xa[t] - ya[t] * p + 25) * rnd - 15) / k)
				globals.p = p - wa[t] * r
				print
				print "Runback team " + t + " " + r + " yards"
				g = rnd
				if g < 0.25 then
					losePossession
					routine = 4
					continue
				else if ya[t] * p >= xa[t] then
					touchdown
					if showScores then return
					globals.t = ta[t]
					routine = 1
					continue
				else if wa[t] * p >= za[t] then
					print
					print "Safety against team " + t + " **********************OH-OH"
					ha[ta[t]] += 2
					if showScores then return
					globals.p = za[t] - wa[t] * 20
					if askYesNo("Team " + t + " do you want to punt instead of a kickoff") == "YES" then
						print
						print "Team " + t + " will punt"
						g = rnd
						if g < 0.25 then
							losePossession
							routine = 4
							continue
						end if
						print
						printSeparator
						k = floor(25 * rnd + 35)
						globals.t = ta[t]
						routine = 2
						continue
					end if
					touchdown
					if showScores then return
					globals.t = ta[t]
					routine = 1
					continue
				else
					routine = 5
					continue
				end if
			else		// player does not want to runback
				if wa[t] * p >= za[t] then globals.p = za[t] - wa[t] * 20
			end if
		end if
		if routine <= 5 then
			d = 1
			s = p
		end if
		if routine <= 6 then
			print "=" * 67
			print "TEAM " + t + " DOWN " + d + " ON " + p
			if d == 1 then
				if ya[t] * (p + ya[t] * 10) >= xa[t] then
					c = 8
				else
					c = 4
				end if
			end if
			if c != 8 then
				print " "*27 + (10 - (ya[t] * p - ya[t] * s)) + " yards to 1st down"
			else
				print " "*27 + (xa[t] - ya[t] * p) + " yards"
			end if
			showBall
			if d == 4 then routine = 8
		end if
		if routine <= 7 then
			u = floor(3 * rnd - 1)
			while true
				str = input("Input offensive play, defensive play: ")
				str = str.replace(",", " ").replace("  ", " ").split
				if t == 1 then
					p1 = str[0].val
					p2 = str[1].val
				else
					p2 = str[0].val
					p1 = str[1].val
				end if
				if p1 == 99 then
					if showScores then return
					continue
				end if
				if 1 <= p1 <= 20 and 1 <= p2 <= 20 then break
				print "Illegal play number, check and"
			end while
		end if
		if d == 4 or p1 == 77 then
			if askYesNo("Does team " + t + " want to punt") == "YES" then
				print
				print "Team " + t + " will punt"
				if rnd < 0.25 then
					losePossession
					routine = 4
					continue
				end if
				print
				printSeparator
				k = floor(25 * rnd + 35)
				globals.t = ta[t]
				routine = 2
				continue
			end if
			if askYesNo("Does team " + t + " want to attempt a field goal") == "YES" then
				print
				print "Team " + t + " will attempt a field goal"
				if rnd < 0.025 then
					losePossession
					routine = 4
					continue
				else
					f = floor(35 * rnd + 20)
					print
					print "Kick is " + f + " yards long"
					globals.p = p - wa[t] * f
					if rnd < 0.35 then
						print "Ball went wide"
					else if ya[t] * p >= xa[t] then
						print "FIELD GOLD GOOD FOR TEAM " + t + " *********************YEA"
						q = 3
						ha[t] = ha[t] + q
						if showScores then return
						globals.t = ta[t]
						routine = 1
						continue
					end if
					print "Field goal unsuccesful team " + t + "-----------------too bad"
					print
					printSeparator
					if ya[t] * p < xa[t] + 10 then
						print
						print "Ball now on " + p
						globals.t = ta[t]
						showBall
						routine = 4
						continue
					else
						globals.t = ta[t]
						routine = 3
						continue
					end if
				end if
			else
				routine = 7
				continue
			end if
		end if
		y = floor(abs(aa[p1] - ba[p2]) / 19 * ((xa[t] - ya[t] * p + 25) * rnd - 15))
		print
		if t == 1 and aa[p1] < 11 or t == 2 and ba[p2] < 11 then
			print "The ball was run"
		else if u == 0 then
			print "Pass incomplete team " + t
			y = 0
		else
			if rnd <= 0.025 and y > 2 then
				print "Pass completed"
			else
				print "Quarterback scrambled"
			end if
		end if
		globals.p = p - wa[t] * y
		print
		print "Net yards gained on down " + d + " are " + y

		if rnd <= 0.025 then
			losePossession
			routine = 4
			continue
		else if ya[t] * p >= xa[t] then
			touchdown
			if showScores then return
			globals.t = ta[t]
			routine = 1
			continue
		else if wa[t] * p >= za[t] then
			print
			print "SAFETY AGAINST TEAM " + t + " **********************OH-OH"
			ha[ta[t]] = ha[ta[t]] + 2
			if showScores then return
			globals.p = za[t] - wa[t] * 20
			if askYesNo("Team " + t + " do you want to punt instead of a kickoff") == "YES" then
				print
				print "Team " + t + " will punt"
				if rnd < 0.25 then
					losePossession
					routine = 4
					continue
				end if
				print
				printSeparator
				k = floor(25 * rnd + 35)
				globals.t = ta[t]
				routine = 2
				continue
			end if
			touchdown
			if showScores then return
			globals.t = ta[t]
			routine = 1
		else if ya[t] * p - ya[t] * s >= 10 then
			routine = 5
		else
			d += 1
			if d != 5 then
				routine = 6
			else
				print
				print "Conversion unsuccessful team " + t
				globals.t = ta[t]
				print
				printSeparator
				routine = 5
			end if
		end if
	end while
end function

playGame
