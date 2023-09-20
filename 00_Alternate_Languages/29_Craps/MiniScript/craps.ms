// Craps
// Ported from BASIC to MiniScript by Joe Strout (2023)

print " "*33 + "CRAPS"
print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print; print; print

print "2,3,12 are losers; 4,5,6,8,9,10 are points; 7,11 are natural winners."
winnings = 0

rollDice = function
	wait 0.5  // (a small delay makes the game more dramatic)
	return ceil(rnd*6) + ceil(rnd*6)
end function

doOneBet = function
	while true
		wager = input("Input the amount of your wager: ").val
		if wager > 0 then break
	end while
	print "I will now throw the dice."
	result = 0		// 1 if we win, 2 if win double, -1 if we lose
	roll = rollDice
	if roll == 7 or roll == 11 then
		print roll + "- natural....a winner!!!!"
		print roll + " pays even money, you win " + wager + " dollars"
		result = 1
	else if roll == 2 then
		print "2- snake eyes....you lose."
		print "You lose " + wager + " dollars."
		result = -1
	else if roll == 3 or roll == 12 then
		print roll + " - craps...you lose."
		print "You lose " + wager + " dollars."
		result = -1		
	else
		point = roll
		print point + " is the point. I will roll again"
		while true
			roll = rollDice
			if roll == 7 then
				print "7 - craps. You lose."
				print "You lose $" + wager
				result = -1
				break
			else if roll == point then
				print roll + "- a winner.........CONGRATS!!!!!!!!"
				print roll + " at 2 to 1 odds pays you...let me see..." + 
				  (2*wager) + " dollars"
				result = 2
				break
			else
				print roll + " - no point. I will roll again"
			end if  
		end while
	end if
	globals.winnings += wager * result
end function

// Main loop.
while true
	doOneBet
	
	while true
		// Why you have to enter 5 to continue is anyone's guess,
		// but that's what the original program did.
		again = input(" If you want to play again print 5 if not print 2: ")
		if again == "5" or again == "2" then break
	end while
	
	if winnings < 0 then
		print "You are now under $" + (-winnings)
	else if winnings > 0 then
		print "You are now ahead $" + winnings
	else
		print "You are now even at 0"
	end if
	if again != "5" then break
end while

if winnings < 0 then
	print "Too bad, you are in the hole. Come again."
else if winnings > 0 then
	print "Congratulations---You came out a winner. Come again!"
else
	print "Congrtulations---You came out even, not bad for an amateur"
end if
