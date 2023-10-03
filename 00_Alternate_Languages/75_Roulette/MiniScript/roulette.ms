print " "*32 + "Roulette"
print " "*15 + "Creative Computing  Morristown  New Jersey"
print; print; print

if version.hostName == "Mini Micro" then
	import "dateTime"
	globals.date = dateTime.str(dateTime.now, "MMM d, yyyy")
else
	globals.date = input("Enter the current date (as in 'Jan 23, 1979') - ")
end if

yn = input("Do you want instructions? ").lower + " "
if yn[0] != "n" then
	print
	print "This is the betting layout"
	print "  (*=Red)"
	print
	print " 1*    2     3*"
	print " 4     5*    6 "
	print " 7*    8     9*"
	print "10    11    12*"
	print "---------------"
	print "13    14*   15 "
	print "16*   17    18*"
	print "19*   20    21*"
	print "22    23*   24 "
	print "---------------"
	print "25*   26    27*"
	print "28    29    30*"
	print "31    32*   33 "
	print "34*   35    36*"
	print "---------------"
	print "    00    0    "
	print
	input "(Press Return at each pause.)"
	print
	print "Types of Bets"
	print
	print "The numbers 1 to 36 signify a straight bet"
	print "on that number."
	print "These pay off 35:1"
	print
	print "The 2:1 bets are:"
	print " 37) 1-12     40) first column"
	print " 38) 13-24    41) second column"
	print " 39) 25-36    42) third column"
	print
	print "The even money bets are:"
	print " 43) 1-18     46) odd"
	print " 44) 19-36    47) red"
	print " 45) even     48) black"
	print
	print " 49)0 and 50)00 pay off 35:1"
	print " NOTE: 0 and 00 do not count under any"
	print "       bets except their own."
	input
	print "When I ask for each bet, type the number"
	print "and the amount, separated by a comma."
	print "For example: to bet $500 on black, type 48,500"
	print "when I ask for a bet."
	print
	print "The minimum bet is $5, the maximum is $500."
	print
end if

redNumbers = [1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36]

// function to convert a number 1-38 to a number/description, like "00"
// or "7 RED"
numDesc = function(number)
	if number == 37 then return "0"
	if number == 38 then return "00"
	s = str(number)
	if redNumbers.indexOf(number) == null then
		return s + " BLACK"
	else
		return s + " RED"
	end if
end function

// function to calculate the payout factor (positive if player wins,
// or -1 if player loses) for the given bet and actual spin.
payoutFactor = function(bet, spin)
	if bet <= 36 then			// straight bet, pays 35:1
		if bet == spin then return 35 else return -1
	else if bet == 49 then		// 0, pays 35:1
		if spin == 37 then return 35 else return -1
	else if bet == 50 then		// 00, pays 35:1
		if spin == 38 then return 35 else return -1
	else if bet == 37 then		// 1-12, pays 2:1
		if 1 <= spin <= 12 then return 2 else return -1
	else if bet == 38 then		// 13-24, pays 2:1
		if 13 <= spin <= 24 then return 2 else return -1
	else if bet == 39 then		// 25-36, pays 2:1
		if 25 <= spin <= 36 then return 2 else return -1
	else if bet == 40 then		// first column, pays 2:1
		if spin % 3 == 1 then return 2 else return -1
	else if bet == 41 then		// second column, pays 2:1
		if spin % 3 == 2 then return 2 else return -1
	else if bet == 42 then		// third column, pays 2:1
		if spin % 3 == 0 then return 2 else return -1
	else if bet == 43 then		// 1-18, even money
		if 1 <= spin <= 18 then return 1 else return -1
	else if bet == 44 then		// 19-36, even money
		if 19 <= spin <= 36 then return 1 else return -1
	else if bet == 45 then		// even number, even money
		if spin % 2 == 0 then return 1 else return -1
	else if bet == 46 then		// odd number, even money
		if spin % 2 == 1 then return 1 else return -1
	else if bet == 47 then		// red, even money
		if redNumbers.indexOf(spin) != null then return 1 else return -1
	else if bet == 48 then		// black, even money
		if redNumbers.indexOf(spin) == null then return 1 else return -1
	end if
	print "Invalid bet " + bet + " in payoutFactor"
end function

playerCash = 1000
houseCash = 100000
x = [0] * 38	// (keeps track of how often each number comes up)
while playerCash > 0
	// Get the player's bets
	numBets = input("How many bets? ").val
	if numBets < 1 then continue
	bets = []; amounts = []
	for i in range(1, numBets)
		while true
			s = input("Number " + i + "? ").replace(",", " ").replace("  ", " ").split
			if s.len != 2 then continue
			bet = s[0].val; amount = s[1].val
			if bets.indexOf(bet) != null then
				print "You made that bet once already,dum-dum"
				continue
			end if
			if 1 <= bet <= 50 and 5 <= amount <= 500 then
				bets.push bet
				amounts.push amount
				break
			end if
		end while
	end for
	
	// Spin the wheel!
	print "Spinning"
	print
	print
	spin = floor(38 * rnd + 1)
	x[spin] += 1
	print numDesc(spin)
	print
	
	// Now, pay out the bets
	for i in bets.indexes
		f = payoutFactor(bets[i], spin)
		if f > 0 then
			print "You win " + f*amounts[i] + " on bet " + i
		else
			print "You lose " + (-f)*amounts[i] + " on bet " + i
		end if
		playerCash += f * amounts[i]
		houseCash -= f * amounts[i]		
	end for
	print
	print "Totals:      ME          YOU"
	print "             " + (houseCash+" "*12)[:12] + playerCash
	if playerCash > 0 and houseCash > 0 then
		yn = input("Again? ").lower + " "
		if yn[0] != "y" then break
	end if
end while

if houseCash < 1 then
	print "You broke the house!"
	playerCash = 101000
end if
if playerCash < 1 then
	print "Oops! You just spent your last dollar!"
	print "Thanks for your money."
	print "I'll use it to buy a solid gold roulette wheel"
	print
else
	name = input("To whom shall I make the check? ")
	print
	print "-"*68
	print " "*55 + "Check No. " + floor(rnd*100)
	print
	print " "*(67 - date.len) + date
	print
	print
	print "Pay to the order of-----" + name + "-----$ " + playerCash
	print
	print
	print " "*10 + "The Memory Bank of New York"
	print
	print " "*35 + "The Computer"
	print " "*35 + "----------X-----"
	print
	print "-"*68
	print "Come back soon!"
end if
