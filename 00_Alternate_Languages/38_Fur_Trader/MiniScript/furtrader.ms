print " "*31 + "Fur Trader"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

furs = [0,0,0,0]	// how many of each type of fur you have
value = [0,0,0,0]	// value of each type of fur
furNames = "mink beaver ermine fox".split

getYesNo = function
	while true
		ans = input("Answer yes or no: ").upper
		if not ans then continue
		if ans[0] == "Y" then return "YES"
		if ans[0] == "N" then return "NO"
	end while
end function

printInstructions = function
	print "You are the leader of a French fur trading expedition in "
	print "1776 leaving the Lake Ontario area to sell furs and get"
	print "supplies for the next year.  You have a choice of three"
	print "forts at which you may trade.  The cost of supplies"
	print "and the amount you receive for your furs will depend"
	print "on the fort that you choose."
	print
end function

pickFort = function
	print
	while true
		print "You may trade your furs at fort 1, fort 2,"
		print "or fort 3.  Fort 1 is Fort Hochelaga (Montreal)"
		print "and is under the protection of the French army."
		print "Fort 2 is Fort Stadacona (Quebec) and is under the"
		print "protection of the French Army.  However, you must"
		print "make a portage and cross the Lachine rapids."
		print "Fort 3 is Fort New York and is under Dutch control."
		print "You must cross through Iroquois land."
		b = input("Answer 1, 2, or 3: ").val
		if b == 1 then
			print "You have chosen the easiest route.  However, the fort"
			print "is far from any seaport.  The value"
			print "you receive for your furs will be low and the cost"
			print "of supplies higher than at Forts Stadacona or New York."		
		else if b == 2 then
			print "You have chosen a hard route.  It is, in comparsion,"
			print "harder than the route to Hochelaga but easier than"
			print "the route to New York.  You will receive an average value"
			print "for your furs and the cost of your supplies will be average."
		else if b == 3 then
			print "You have chosen the most difficult route.  At"
			print "Fort New York you will receive the highest value"
			print "for your furs.  The cost of your supplies"
			print "will be lower than at all the other forts."		
		else
			continue
		end if
		print "Do you want to trade at another fort?"
		if getYesNo == "NO" then return b
	end while
end function

visitFort = function(fort)
	print
	if fort == 1 then
		value[0] = floor((.2*rnd+.7)*100+.5)/100
		value[2] = floor((.2*rnd+.65)*100+.5)/100
		value[1] = floor((.2*rnd+.75)*100+.5)/100
		value[3] = floor((.2*rnd+.8)*100+.5)/100
		print "Supplies at Fort Hochelaga cost $150.00."
		print "Your travel expenses to Hochelaga were $10.00."
		globals.money -= 150 + 10
	else if fort == 2 then	
		value[0] = floor((.3*rnd+.85)*100+.5)/100
		value[2] = floor((.15*rnd+.8)*100+.5)/100
		value[1] = floor((.2*rnd+.9)*100+.5)/100
		p = floor(10*rnd)+1
		if p <= 2 then
			furs[1] = 0
			print "Your beaver were too heavy to carry across"
			print "the portage.  You had to leave the pelts, but found"
			print "them stolen when you returned."
		else if p <= 6 then 
			print "You arrived safely at Fort Stadacona."
		else if p <= 8 then
			for j in range(0,3); furs[j] = 0; end for
			print "Your canoe upset in the Lachine rapids.  You"
			print "lost all your furs."
		else if furs[3] then
			furs[3] = 0
			print "Your fox pelts were not cured properly."
			print "No one will buy them."
		end if
		print "Supplies at Fort Stadacona cost $125.00."
		print "Your travel expenses to Stadacona were $15.00."
		globals.money -= 125 + 15
	else
		value[0] = floor((.15*rnd+1.05)*100+.5)/100
		value[3] = floor((.25*rnd+1.1)*100+.5)/100
		p = floor(10*rnd)+1
		if p <= 2 then
			print "You were attacked by a party of Iroquois."
			print "All people in your trading group were"
			print "killed.  This ends the game."
			globals.gameOver = true
			return
		else if p<=6 then
			print "You were lucky.  You arrived safely"
			print "at Fort New York."
		else if p<=8 then
			for j in range(0,3); furs[j] = 0; end for
			print "You narrowly escaped an iroquois raiding party."
			print "However, you had to leave all your furs behind."
		else
			value[1] /= 2
			value[0] /= 2
			print "Your mink and beaver were damaged on your trip."
			print "You receive only half the current price for these furs."
		end if
		print "Supplies at New York cost $80.00."
		print "Your travel expenses to New York were $25.00."
		globals.money -= 80 + 25
	end if
end function

printInstructions

gameOver = false
money=600
while not gameOver
	print "Do you wish to trade furs?"
	if getYesNo == "NO" then break

	value[2]=floor((.15*rnd+.95)*100+.5)/100		// ermine value
	value[1]=floor((.25*rnd+1.00)*100+.5)/100		// beaver value
	
	print
	print "You have $" + money + " savings."
	print "And 190 furs to begin the expedition."
	print
	print "Your 190 furs are distributed among the following"
	print "kinds of pelts: mink, beaver, ermine and fox."
	print
	furs = [0,0,0,0]
	for j in range(0, 3)
		furs[j] = input("How many " + furNames[j] + " do you have? ").val
		if furs.sum >= 190 then break
	end for
	if furs.sum > 190 then
		print "You may not have that many furs."
		print "Do not try to cheat.  I can add."
		print "You must start again."
		continue
	end if

	fort = pickFort
	visitFort fort
	if gameOver then break

	print
	for j in [1, 3, 2, 0]
		if not furs[j] then continue
		revenue = value[j] * furs[j]
		print "Your " + furNames[j] + " sold for $" + revenue + "."
		money += revenue
	end for
	print
	print "You now have $" + money + " including your previous savings."
end while


