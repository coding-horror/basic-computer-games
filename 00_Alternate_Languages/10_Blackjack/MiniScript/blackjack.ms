import "listUtil"
print " "*31 + "Black Jack"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

fna = function(q); return q - 11*(q >= 22); end function
hands = [[]]*15		// hands(i,j) is the jth card in hand i (P)
handValue = [0]*15	// total value of each hand (Q)
deck = []			// the deck being dealt from (C)
playerMoney = [0]*8	// total $ for each player (T)
roundWinnings = [0]*7 // total $ won/lost this hand for each player (S)
betPerHand = [0]*15	// bet for each hand (B)

reshuffle = function
	print "Reshuffling"
	globals.deck = range(1,13)*4
	deck.shuffle
end function
		
// Function to draw a card from the deck.
getCard = function		// line 100
	if not deck then reshuffle
	return deck.pop
end function

// Function to evaluate the given hand. Total is usually put into
// handValue(handNum).  Totals have the following meaning:
//      2-10...Hard 2-10
//     11-21...Soft 11-21
//     22-32...Hard 11-21
// 33+ or -1...Busted
evalHand = function(handNum)	// line 300
	result = 0
	for card in hands[handNum]
		result = addCardToTotal(result, card)
	end for
	return result
end function

// Function to add a card into a total hand value.
addCardToTotal = function(total, card)	// line 500
	if total < 0 then return total	// (already busted)
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
		print "Please enter a number from " + minVal + " to " + maxVal
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
		betPerHand[i] = getNumber("# " + (i+1) + "? ", 1, 500)
	end for
end function

playOneRound = function
	globals.roundWinnings = [0] * numPlayers
	if deck.len < (numPlayers+1) * 2 then reshuffle
	getBets
	print "PLAYER ", ""
	for i in range(0, numPlayers-1)
		print i+1, "     "
		hands[i] = []
	end for
	print "DEALER"
	hands[numPlayers] = []
	for row in [1,2]
		print "     ", ""
		for i in range(0, numPlayers)
			hands[i].push getCard
			if row == 1 or i < numPlayers then printCard hands[i][-1]
		end for
		print
	end for
	dealerCard0 = hands[numPlayers][0]
	dealerCard1 = hands[numPlayers][1]
	// Test for insurance
	if dealerCard0 == 1 and getYesNo("Any insurance? ") == "Y" then
		print "Insurance Bets"
		for i in range(0, numPlayers-1)
			insurance = getNumber("# " + (i+1) + "? ", 0, betPerHand[i]/2)
			roundWinnings[i] = insurance * (3 * (dealerCard1 >= 10) - 1)
		end for
	end if
	// Test for dealer blackjack
	if (dealerCard0==1 and dealerCard1 > 9) or
	  (dealerCard0 > 9 and dealerCard1==1) then
		print; print "Dealer has a " + cardNames[dealerCard1] + " in the hole for Blackjack"
		for i in range(0, numPlayers)
			handValue[i] = evalHand(i)
		end for
	else
		// no dealer blackjack
		if dealerCard0 == 1 or dealerCard0 >= 10 then
			print; print "No dealer Blackjack."
		end if
		// now play the hands
		for i in range(0, numPlayers-1)
			playHand i
		end for
		handValue[numPlayers] = evalHand(numPlayers)	// (evaluate dealer hand)
		// Test for playing the dealer's hand... we only do so if
		// there are any player hands with cards left.
		anyLeft = false
		for i in range(0, numPlayers-1)
			if hands[i] or hands[i+8] then anyLeft = true
		end for
		if not anyLeft then
			print "Dealer had a " + cardNames[hands[numPlayers][1]] + " concealed."
		else
			// Play dealer's hand.
			dispTotal = displayTotal(handValue[numPlayers])
			print "Dealer has a " + cardNames[hands[numPlayers][1]] + " concealed" +
			  " for a total of " + dispTotal + "."
			while handValue[numPlayers] > 0 and dispTotal <= 16
				card = getCard
				if hands[numPlayers].len == 2 then print "Draws  ", ""
				print cardNames[card], "    "
				hands[numPlayers].push card
				handValue[numPlayers] = evalHand(numPlayers)
				dispTotal = displayTotal(handValue[numPlayers])
			end while
			if hands[numPlayers].len > 2 then
				if handValue[numPlayers] < 0 then print "  ---Busted" else print "  ---Total is " + dispTotal
			end if
			print
		end if
	end if
	tallyResults
end function

playHand = function(handNum, prompt=null, allowSplit=true)
	if not prompt then prompt = "Player " + (handNum % 8 + 1)
	while hands[handNum]
		options = ["H", "S", "D"] + ["/"] * allowSplit
		choice = getOption(prompt + "? ", options)
		if choice == "S" then			// player wants to stand
			handValue[handNum] = evalHand(handNum)
			if handValue[handNum] == 21 and hands[handNum].len == 2 then
				print "Blackjack"
				roundWinnings[handNum] += 1.5 * betPerHand[handNum]
				betPerHand[handNum] = 0
				discardHand handNum
			end if
			break
		else if choice == "D" or choice == "H" then		// hit or double down
			handValue[handNum] = evalHand(handNum)
			if choice == "D" then betPerHand[handNum] *= 2
			card = getCard
			print "Received a " + cardNames[card], "   "
			hands[handNum].push card
			handValue[handNum] = evalHand(handNum)
			if handValue[handNum] < 0 then
				print "...busted", ""
				discardHand handNum
			end if
			print
			if choice == "D" then break
		else if choice == "/" then		// split
			card1 = hands[handNum][0]; if card1 > 10 then card1 = 10
			card2 = hands[handNum][1]; if card2 > 10 then card2 = 10
			if card1 != card2 then
				print "Splitting not allowed."
				continue
			end if
			hand2 = handNum + 8
			hands[hand2] = [hands[handNum].pop]
			betPerHand[hand2] = betPerHand[handNum]
			card = getCard
			print "First hand receives a " + cardNames[card]
			hands[handNum].push card
			card = getCard
			print "Second hand receives a " + cardNames[card]
			hands[hand2].push card
			if card1 != 1 then
				// Now play the two hands
				playHand handNum, "Hand 1", false
				playHand hand2, "Hand 2", false
			end if
			break
		end if
		allowSplit = false
	end while
end function

discardHand = function(handNum)
	hands[handNum] = []
	handValue[handNum] = 0
end function

tallyResults = function
	dealerTotal = displayTotal(evalHand(numPlayers))
	for i in range(0, numPlayers-1)
		playerHandATotal = displayTotal(evalHand(i))
		playerHandBTotal = displayTotal(evalHand(i+8))
		// calculate roundWinnings[i], which is the $ won/lost for player i
		roundWinnings[i] = roundWinnings[i] + betPerHand[i]*sign(playerHandATotal - dealerTotal) + betPerHand[i+8]*sign(playerHandBTotal - dealerTotal)
		betPerHand[i+8] = 0
		s = "Player " + (i+1) + " "
		s += ["loses", "pushes", "wins"][sign(roundWinnings[i])+1]		
		if roundWinnings[i] != 0 then s += " " + abs(roundWinnings[i])
		playerMoney[i] += roundWinnings[i]
		playerMoney[numPlayers] -= roundWinnings[i]
		s = (s + " "*25)[:25] + "Total = " + playerMoney[i]
		print s
		discardHand i
		discardHand i+8
	end for
	print "Dealer's total = " + playerMoney[numPlayers]
	print
end function

// Main program starts here			// (Line 1500)
// Initialize
cardNames = "n A 2 3 4 5 6 7 8 9 10 J Q K".split

if getYesNo("Do you want instructions? ") == "Y" then
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
	print
end if
numPlayers = getNumber("Number of players? ", 1, 7)
print
// main loop!
while true
	playOneRound
end while
