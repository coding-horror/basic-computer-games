print " "*30 + "Slots"
print " "*15 + "Creative Computing  Morristown  New Jersey"
print; print; print

// PRODUCED BY FRED MIRABELLE AND BOB HARPER ON JAN 29, 1973
// IT SIMULATES THE SLOT MACHINE.
// (Ported to MiniScript by Joe Strout on Oct 04, 2023)

print "You are in the H&M casino,in front of one of our"
print "one-arm bandits. Bet from $1 to $100."
print "To pull the arm, punch the return key after making your bet."

symbols = ["BAR", "BELL", "ORANGE", "LEMON", "PLUM", "CHERRY"]


winTriple = function(symbol, bet)
	if symbol == "BAR" then
		print "***JACKPOT***"
		globals.profit += 101 * bet
	else
		print "**TOP DOLLAR**"
		globals.profit += 11 * bet
	end if
	print "You won!"
end function

winDouble = function(symbol, bet)
	if symbol == "BAR" then
		print "*DOUBLE BAR*"
		globals.profit += 6 * bet
	else
		print "Double!"
		globals.profit += 3 * bet
	end if
	print "You won!"
end function

lose = function(bet)
	print "You lost."
	globals.profit -= bet
end function

calcWinLoss = function(spun, bet)
	if spun[0] == spun[1] then
		if spun[0] == spun[2] then
			winTriple spun[0], bet
		else
			winDouble spun[0], bet
		end if
	else if spun[0] == spun[2] then
		winDouble spun[0], bet
	else if spun[1] == spun[2] then
		winDouble spun[1], bet
	else
		lose bet
	end if
end function

ringBells = function(qty=5)
	// I believe all the obnoxious beeping was to slow down the game
	// and build suspense as each "wheel" appears.  Our version:
	wait 0.1
	for i in range(1, qty)
		print char(7), ""
		wait 0.05
	end for
end function

// Main program
profit = 0
while true
	print
	
	// Get bet
	while true
		bet = input("Your bet? ").val
		if 1 <= bet <= 100 then break
		if bet < 1 then print "Minimum bet is $1" else print "House limits are $100"
	end while
	
	// Spin 3 wheels (randomly picking a symbol for each one)
	spun = []
	spun.push symbols[rnd * symbols.len]
	spun.push symbols[rnd * symbols.len]
	spun.push symbols[rnd * symbols.len]
	print
	ringBells 10; print spun[0], " "
	ringBells 5;  print spun[1], " "
	ringBells 5;  print spun[2]
	print
	
	// Calculate and display win/loss
	wait 0.5
	calcWinLoss spun, bet
	
	// Show new state, and maybe play again
	print "Your standings are $ " + profit	
	yn = input("Again? ").lower + " "
	if yn[0] != "y" then break
end while

if profit == 0 then
	print "Hey, you broke even."
else if profit > 0 then
	print "Collect your winnings from the H&M cashier."
else
	print "Pay up!  Please leave your money on the terminal."
end if
