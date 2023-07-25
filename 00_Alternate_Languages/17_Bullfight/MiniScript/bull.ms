print " "*34 + "Bull"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

getYesNo = function(prompt)
	while true
		ans = input(prompt + "? ").lower
		if ans and (ans[0] == "y" or ans[0] == "n") then return ans[0]
		print "Incorrect answer - - please type 'yes' or 'no'."
	end while
end function

if getYesNo("Do you want instructions") == "y" then
	print "Hello, all you bloodlovers and aficionados."
	print "Here is your big chance to kill a bull."
	print
	print "On each pass of the bull, you may try"
	print "0 - Veronica (dangerous inside move of the cape)"
	print "1 - Less dangerous outside move of the cape"
	print "2 - Ordinary swirl of the cape."
	print
	print "Instead of the above, you may try to kill the bull"
	print "on any turn: 4 (over the horns), 5 (in the chest)."
	print "But if I were you,"
	print "I wouldn't try it before the seventh pass."
	print
	print "The crowd will determine what award you deserve"
	print "(posthumously if necessary)."
	print "The braver you are, the better the award you receive."
	print
	print "The better the job the picadores and toreadores do,"
	print "the better your chances are."
	print; input "(Press return.)"
end if
print; print
bravery = 1
outcome = 1
qualities = [null, "superb", "good", "fair", "poor", "awful"]

// Select a bull (level 1-5, lower numbers are tougher)
bullLevel = floor(rnd*5+1)
print "You have drawn a " + qualities[bullLevel] + " bull."
if bullLevel > 4 then print "You're lucky."
if bullLevel < 2 then print "Good luck.  You'll need it."
print

// Simulate one of the preliminary types of bullfighters
// (picodores or toreadores).  Return their effect, 0.1 - 0.5.
simPreliminary = function(fighterType)
	effect = 0.1
	temp = 3 / bullLevel * rnd
	if temp < 0.87 then effect = 0.2
	if temp < 0.63 then effect = 0.3
	if temp < 0.5 then effect = 0.4
	if temp < 0.37 then effect = 0.5
	t = floor(10 * effect + 0.2)	// (get quality in range 1 - 5)
	print "The " + fighterType + " did a " + qualities[t] + " job."
	if t == 5 then
		if fighterType == "picadores" then
			print floor(rnd*2+1) + " of the horses of the picadores killed."
		end if
		print floor(rnd*2+1) + " of the " + fighterType + " killed."
	else if t == 4 then
		if rnd > 0.5 then
			print "One of the " + fighterType + " killed."
		else
			print "No " + fighterType + " were killed."
		end if
	end if
	print
	return effect
end function

picaEffect = simPreliminary("picadores")
toreEffect = simPreliminary("toreadores")

getGored = function
	while not done
		if rnd > 0.5 then
			print "You are dead."
			globals.bravery = 1.5
			globals.done = true
		else
			print "You are still alive."; print
			if getYesNo("Do you run from the ring") == "y" then
				print "Coward"
				globals.bravery = 0
				globals.done = true
			else
				print "You are brave.  Stupid, but brave."
				if rnd > 0.5 then
					globals.bravery = 2
					break
				else
					print "You are gored again!"
				end if
			end if
		end if
	end while
end function

pass = 0
courage = 1		// cumulative effect of cape choices
bravery = 1		// set mainly by outcomes after getting gored
victory = false	// true if we kill the bull
done = false

while not done
	pass += 1
	print
	print "Pass number " + pass
	if pass < 3 then
		print "The bull is charging at you!  You are the matador--"
		tryKill = (getYesNo("do you want to kill the bull") == "y")
	else
		tryKill = (getYesNo("Here comes the bull.  Try for a kill") == "y")
	end if
	if tryKill then
		print; print "It is the moment of truth."; print
		h = input("How do you try to kill the bull? " ).val
		if h != 4 and h != 5 then
			print "You panicked.  The bull gored you."
			getGored
			break
		end if
		k = (6-bullLevel) * 10 * rnd / ((picaEffect + toreEffect) * 5 * pass)
		if h == 4 then
			victory = (k <= 0.8)
		else
			victory = (k <= 0.2)
		end if
		if victory then
			print "You killed the bull!"
		else
			print "The bull has gored you!"
			getGored
		end if
		done = true
	else
		if pass < 3 then
			capeMove = input("What move do you make with the cape? ").val
		else
			capeMove = input("Cape move? ").val
		end if
		while capeMove < 0 or capeMove > 2 or capeMove != floor(capeMove)
			print "Don't panic, you idiot!  Put down a correct number"
			capeMove = input.val
		end while
		m = [3, 2, 0.5][capeMove]
		courage += m
		f = (6-bullLevel+m/10)*rnd / ((picaEffect+toreEffect+pass/10)*5)
		if f >= 0.51 then
			print "The bull has gored you!"
			getGored			
		end if
	end if
end while

// Final outcome
if bravery == 0 then
	print "The crowd boos for ten minutes.  If you ever dare to show"
	print "your face in a ring again, they swear they will kill you--"
	print "unless the bull does first."
else
	fnd = (4.5+courage/6-(picaEffect+toreEffect)*2.5+4*bravery+2*(victory+1)-pass^2/120-bullLevel)
	fnc = function; return fnd * rnd; end function
	if bravery == 2 then
		print "The crowd cheers wildly!"
	else if victory then
		print "The crowd cheers!"; print
	end if
	print "The crowd awards you"
	if fnc < 2.4 then
		print "nothing at all."
	else if fnc < 4.9 then
		print "one ear of the bull."
	else if fnc < 7.4 then
		print "Both ears of the bull!"
		print "Ole!"
	else
		print "Ole!  You are 'Muy Hombre!"" Ole!  Ole!"
	end if
end if
print
print "Adios"; print; print; print

	
		
