import "listUtil"
print " "*31 + "Black Jack"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

fna = function(q); return q - 11*(q >= 22); end function
P = [[]]*15	// P(i,j) is the jth card in hand i
Q = [0]*15	// Q(i) is the total value of hand i
C = []		// the deck being dealt from
D = []		// the discard pile
T = [0]*8	// total $ for each player
S = [0]*7	// total $ won/lost this hand for each player
B = [0]*15	// bet for each hand
Z = [0]*7	// insurance for each player

reshuffle = function
	print "Reshuffling"
	globals.C = D
	globals.D = []
	C.shuffle
end function
		
// Function to draw a card from the deck.
getCard = function		// line 100
	if not C then reshuffle
	return C.pop
end function

// Function to evaluate the given hand. Total is usually put into
// Q(handNum).  Totals have the following meaning:
//      2-10...Hard 2-10
//     11-21...Soft 11-21
//     22-32...Hard 11-21
// 33+ or -1...Busted
evalHand = function(handNum)	// line 300
	result = 0
	for card in P[handNum]
		result = addCardToTotal(result, card)
	end for
	return result
end function

// Function to add a card into a total hand value.
addCardToTotal = function(total, card)	// line 500
	x1 = card; if x1 > 10 then x1 = 10
	q1 = total + x1
	if total < 11 then
		if card == 1 then return total + 11	// (ace)
		return q1 + 11 * (q1 >= 11)
	end if
	total = q1 + (total <= 21 and q1 >= 21)
	if total >= 33 then total = -1
	return total
end function

// Print one of those totals as it should appear to the user.
displayTotal = function(total)
	return total - 11 * (total >= 22)
end function

printCard = function(cardNum)
	s = cardNames[cardNum]
	print "  " + (s+" ")[:2], "  "
end function

printCardV2 = function(cardNum)
	s = cardNames[cardNum]
	print "  " + (" " + s)[-2:], "  "
end function

getYesNo = function(prompt)
	while true
		inp = input(prompt).upper
		if inp and (inp[0] == "Y" or inp[0] == "N") then return inp[0]
	end while
end function

getNumber = function(prompt, minVal=0, maxVal=500)
	while true
		result = input(prompt).val
		if result == floor(result) and minVal <= result <= maxVal then return result
	end while
end function

// Get one of a list of one-letter (uppercase) options.
getOption = function(prompt, options)
	while true
		result = input(prompt).upper
		if result and options.indexOf(result[0]) != null then return result[0]
		print "Type " + options[:-1].join(", ") + " or " + options[-1] + " please"
	end while
end function

getBets = function
	print "Bets:"
	for i in range(0, numPlayers-1)
		B[i] = getNumber("# " + (i+1) + "? ", 1, 500)
	end for
end function

playOneRound = function
	globals.Z = [0] * numPlayers
	globals.S = [0] * numPlayers
	if C.len < (numPlayers+1) * 2 then reshuffle
	getBets
	print "PLAYER ", ""
	for i in range(0, numPlayers-1)
		print i+1, "     "
		P[i] = []
	end for
	print "DEALER"
	P[numPlayers] = []
	for row in [1,2]
		print "     ", ""
		for i in range(0, numPlayers)
			P[i].push getCard
			if row == 1 or i < numPlayers then printCard P[i][-1]
		end for
		print
	end for
	// Test for insurance
	if P[numPlayers][0] == 1 and getYesNo("Any insurance? ") == "Y" then
		print "Insurance Bets"
		for i in range(0, numPlayers-1)
			Z[i] = getNumber("# " + (i+1) + "? ", 0, B[i]/2)
			S[i] = Z[i] * (3 * (P[numPlayers][1] >= 10) - 1)	// ?!?
		end for
	end if
	// Test for dealer blackjack
	dealerCard0 = P[numPlayers][0]
	dealerCard1 = P[numPlayers][1]
	if (dealerCard0==1 and dealerCard1 > 9) or
	  (dealerCard0 > 9 and dealerCard1==1) then
		print; print "Dealer has a " + cardNames[dealerCard1] + " in the hole for Blackjack"
		for i in range(0, numPlayers)
			Q[i] = evalHand(i)
		end for
	else
		// no dealer blackjack
		// now play the hands
		for i in range(0, numPlayers-1)
			playHand i
		end for
		Q[numPlayers] = evalHand(numPlayers)	// (evaluate dealer hand)
		// Test for playing the dealer's hand... we only do so if
		// there are any player hands with cards left.
		anyLeft = false
		for i in range(0, numPlayers-1)
			if P[i] or P[i+8] then anyLeft = true
		end for
		if not anyLeft then
			print "Dealer had a " + cardNames[P[numPlayers][1]] + " concealed."
		else
			// Play dealer's hand.
			dispTotal = displayTotal(Q[numPlayers])
			print "Dealer has a " + cardNames[P[numPlayers][1]] + " concealed" +
			  " for a total of " + dispTotal + "."
			while Q[numPlayers] > 0 and dispTotal <= 16
				if P[numPlayers].len == 2 then print "Draws  ", ""
				card = getCard
				print cardNames[card], "    "
				P[numPlayers].push card
				Q[numPlayers] = evalHand(numPlayers)
				dispTotal = displayTotal(Q[numPlayers])
			end while
			if P[numPlayers].len > 2 then print "  ---Total is " + dispTotal
			print
		end if
	end if
	tallyResults
end function

playHand = function(handNum)
	while P[handNum]
		choice = getOption("Player " + (handNum % 8 + 1) + "? ", ["H", "S", "D", "/"])
		if choice == "S" then			// player wants to stand
			Q[handNum] = evalHand(handNum)
			if Q[handNum] == 21 then
				print "Blackjack"
				S[handNum] += 1.5 * B[handNum]
				B[handNum] = 0
				discardHand handNum
			end if
			break
		else if choice == "D" then		// player wants to double down
			print "Not yet supported!"
		else if choice == "H" then		// player wants to hit
			Q[handNum] = evalHand(handNum)
			card = getCard
			print "Received a " + cardNames[card]
			P[handNum].push card
			Q[handNum] = evalHand(handNum)
			if Q[handNum] < 0 then
				print "...busted"
				discardHand handNum
			end if
		else if choice == "/" then		// player wants to split
			print "Not yet supported!"
		end if
	end while
end function

discardHand = function(handNum)
	while P[handNum]
		D.push P[handNum].pop
	end while
	Q[handNum] = 0
end function

tallyResults = function
	dealerTotal = displayTotal(evalHand(numPlayers))
	for i in range(0, numPlayers-1)
		playerHandATotal = displayTotal(evalHand(i))
		playerHandBTotal = displayTotal(evalHand(i+8))
		// calculate S[i], which is the $ won/lost for player i
		S[i] = S[i] + B[i]*sign(playerHandATotal - dealerTotal) + B[i+8]*sign(playerHandBTotal - dealerTotal)
		B[i+8] = 0
		s = "Player " + (i+1) + " "
		s += ["loses", "pushes", "wins"][sign(S[i])+1]		
		if S[i] != 0 then s += " " + abs(S[i])
		T[i] += S[i]
		T[numPlayers] -= S[i]
		s = (s + " "*20)[:20] + " Total = " + T[i]
		print s
		discardHand i
		discardHand i+8
	end for
	print "Dealer's total = " + T[numPlayers]
	print
end function

// Main program starts here			// (Line 1500)
// Initialize
cardNames = "N A 2 3 4 5 6 7 8 9 10 J Q K".split
inputs = "H,S,D,/,"
D = range(1,13) * 4

yesNo = getYesNo("Do you want instructions? ")
if yesNo[0] == "Y" then
	print "This is the game of 21. As many as 7 players may play the"
	print "game. On each deal, bets will be asked for, and the"
	print "players' bets should be typed in. The cards will then be"
	print "dealt, and each player in turn plays his hand. The"
	print "first response should be either 'D', indicating that the"
	print "player is doubling down, 'S', indicating that he is"
	print "standing, 'H', indicating he wants another card, or '/',"
	print "indicating that he wants to split his cards. After the"
	print "initial response, all further responses should be 'S' or"
	print "'H', unless the cards were split, in which case doubling"
	print "down is again permitted. In order to collect for"
	print "blackjack, the initial response should be 'S'."
end if
numPlayers = getNumber("Number of players? ", 1, 7)
print
// main loop!
while true
	playOneRound
end while
