
getNum = function(prompt, maxVal=5)
	while true
		num = floor(input(prompt + "? ").val)
		if num > 0 and num <= maxVal then return num
		print "Try again..."
	end while
end function

showReturn = function
	print "You made it through tremendous flak!!" 
end function

showShotDown = function
	print "* * * * boom * * * *"
	print "You have been shot down....."
	print "Dearly beloved, we are gathered here today to pay our"
	print "last tribute..."	
end function

showSuccess = function
	print "Direct hit!!!! " + floor(100*rnd) + " killed."
	print "Mission successful."
end function

// Function to calculate the mission result for all nations except Japan.
doNonJapanResult = function
	print
	d = input("How many missions have you flown? ").val
	while d >= 160
		print "Missions, not miles..."
		print "150 missions is high even for old-timers."
		d = input("Now then, how many missions have you flown? ").val
	end while
	print
	if d >= 100 then print "That's pushing the odds!"
	if d < 25 then print "Fresh out of training, eh?"
	print
	if d >= 160 * rnd then
		showSuccess
	else
		print "Missed target by " + floor(2+30*rnd) + " miles!"
		print "Now you're really in for it !!"; print
		r = getNum("Does the enemy have guns(1), missiles(2), or both(3)")
		print
		if r != 2 then
			s = input("What's the percent hit rate of enemy gunners (10 to 50)? ").val
			if s<10 then
				print "You lie, but you'll pay..."
				showShotDown
				return
			end if
		end if
		print
		print
		if r > 1 then t = 35 else t = 0
		if s + t > 100 * rnd then
			showShotDown
		else
			showReturn
		end if
	end if
end function

s = 0	// hit rate of enemy gunners
r = 0	// whether enemy has guns(1), missiles(2), or both(3)

// Main Loop
while true
	print "You are a pilot in a World War II bomber."
	a = getNum("What side -- Italy(1), Allies(2), Japan(3), Germany(4)", 4)
	
	if a == 1 then		// Italy
		b = getNum("Your target -- Albania(1), Greece(2), North Africa(3)")
		print
		print ["Should be easy -- you're flying a nazi-made plane.",
			"Be careful!!!", "You're going for the oil, eh?"][b-1]
		doNonJapanResult

	else if a == 2 then	// Allies
		g = getNum("Aircraft -- Liberator(1), B-29(2), B-17(3), Lancaster(4)", 4)
		print ["You've got 2 tons of bombs flying for Ploesti.",
			"You're dumping the A-bomb on Hiroshima.",
			"You're chasing the Aismark in the North Sea.",
			"You're busting a German heavy water plant in the Ruhr."][g-1]
		doNonJapanResult

	else if a == 3 then	// Japan (different logic than all others)
		print "You're flying a kamikaze mission over the USS Lexington."
		isFirst = input("Your first kamikaze mission(y or n)? ").lower
		if isFirst and isFirst[0] == "n" then
			s = 0
			showReturn
		else
			print
			if rnd > 0.65 then showSuccess else showShotDown
		end if

	else				// Germany
		m = getNum("A nazi, eh?  Oh well.  Are you going for Russia(1)," +
		  char(13) + "England(2), or France(3)")
		print ["You're nearing Stalingrad.",
			"Nearing London.  Be careful, they've got radar.",
			"Nearing Versailles.  Duck soup.  They're nearly defenseless."][m-1]
		doNonJapanResult
	end if
	
	print; print; print; another = input("Another mission (y or n)? ").lower
	if not another or another[0] != "y" then
		print "Chicken !!!" ; print ; break
	end if
end while
