print " "*33 + "POKER"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

print "Welcome to the casino.  We each have $200."
print "I will open the betting before the draw; you open after."
print "To fold bet 0; to check bet .5."
print "Enough talk -- let's get down to business."
print

askYesNo = function(prompt)
	while true
		yn = input(prompt + "? ").lower + " "
		if yn[0] == "y" then return "yes"
		if yn[0] == "n" then return "no"
		print "Answer yes or no, please."
	end while
end function
askNumber = function(prompt, maxQty=3, minQty=1, maxErr=null, minErr=null)
	while true
		value = input(prompt + "? ").val
		if minQty <= value <= maxQty then return value
		if value < minQty and minErr != null then
			print minErr
		else if maxErr != null then
			print maxErr
		else
			print "Enter a value between " + minQty + " and " + maxQty + ", please."
		end if
	end while
end function
random = function(n)
	return floor(n * rnd)
end function
rand10 = function
	return floor(10 * rnd)
end function
pad = function(s, width)
	return s + " " * (width - s.len)
end function

// Bonus little feature when running in Mini Micro: display a header bar
// always at the top of the screen, showing current balances.  Does nothing
// on other platforms.
drawHeaders = function
	if version.hostName != "Mini Micro" then return
	display(2).mode = displayMode.text; td = display(2)
	td.color = color.black; td.backColor = text.color
	td.row = 25; td.column = 0
	td.print pad("Computer: $" + computer.balance, 25) +
	  pad("Table: $" + Table.pot, 25) + 
	  pad("Player: $" + human.balance, 18)
end function


// Card class: represents a single playing card
Card = {}
Card.rank = 0		// from 2 to 14 (Ace)
Card.suit = "Clubs"
Card.keep = false	// temp flag, used to note which cards to keep vs. discard
Card.make = function(rank, suit)
	result = new Card
	result.rank = rank
	result.suit = suit
	return result
end function
Card.rankStr = function(plural=false)
	if self.rank > 10 then
		return ["Jack", "Queen", "King", "Ace"][self.rank-11] + "s"*plural
	else
		return str(self.rank) + "'s" * plural
	end if
end function
Card.str = function
	return self.rankStr + " of " + self.suit
end function

// Prepare a standard deck of 52 cards, and functions to draw and discard
deck = []
for suit in ["Clubs", "Diamonds", "Hearts", "Spades"]
	for rank in range(2, 14)
		deck.push Card.make(rank, suit)
	end for
end for
deck.shuffle
discardPile = []
drawCard = function
	if not deck then
		globals.deck = discardPile
		deck.shuffle
		globals.discardPile = []
	end if
	return deck.pop
end function
discard = function(cardOrCards)
	if cardOrCards isa Hand then
		globals.discardPile += cardOrCards.cards
	else if cardOrCards isa list then
		globals.discardPile += cardOrCards
	else
		discardPile.push cardOrCards
	end if
end function

// Hand ranks: how we compare Poker hands
HandRank = {}
HandRank.value = 0
HandRank.str = function(highCard); return ""; end function
HandRank.make = function(value)
	result = new HandRank
	result.value = value
	return result
end function

HandRank.None = new HandRank
HandRank.Schmaltz = HandRank.make(1)
HandRank.Schmaltz.str = function(c); return "schmaltz, " + c.rankStr + " high"; end function
HandRank.PartialStraight = HandRank.make(2)
HandRank.PartialStraight.str = function(c); return ""; end function	// (no display string; this is used only internally)
HandRank.Pair = HandRank.make(3)
HandRank.Pair.str = function(c); return "a pair of " + c.rankStr(true); end function
HandRank.TwoPair = HandRank.make(4)
HandRank.TwoPair.str = function(c); return "two pair, " + c.rankStr(true); end function
HandRank.Three = HandRank.make(5)
HandRank.Three.str = function(c); return "three " + c.rankStr(true); end function
HandRank.Straight = HandRank.make(6)
HandRank.Straight.str = function(c); return "straight, " + c.rankStr + " high"; end function
HandRank.Flush = HandRank.make(7)
HandRank.Flush.str = function(c); return "a flush in " + c.suit; end function
HandRank.FullHouse = HandRank.make(8)
HandRank.FullHouse.str = function(c); return "full house, " + c.rankStr(true); end function
HandRank.Four = HandRank.make(9)
HandRank.Four.str = function(c); return "four " + c.rankStr(true); end function
// Note: original code does not detect a straight flush or royal flush.

// Hand: represents a set of cards in the hand of one player.
Hand = {}
Hand.cards = null		// list of Card
Hand.rank = null		// HandRank instance
Hand.highCard = null	// reference to which (of self.cards) determines relative value
Hand.afterDraw = false	// true if we've already had a chance to draw cards
Hand.make = function(cards)
	result = new Hand
	result.cards = cards
	result.analyze
	return result
end function
Hand.deal = function
	return Hand.make([drawCard, drawCard, drawCard, drawCard, drawCard])
end function
Hand.replaceCard = function(index)
	discard self.cards[index]
	self.cards[index] = drawCard
end function
Hand.rankStr = function
	return self.rank.str(self.highCard)
end function
Hand.isWeak = function
	return self.rank.value < HandRank.PartialStraight.value or
		 (self.rank.value == HandRank.PartialStraight.value and self.afterDraw) or
		 (self.rank <= HandRank.TwoPair.value and self.highCard.rank <= 6)
end function		 
Hand.beats = function(other)
	if self.rank.value > other.rank.value then return true
	if self.rank.value < other.rank.value then return false
	return self.highCard.rank > other.highCard.rank
end function
Hand.print = function(startingNumber=1)
	num = startingNumber
	for c in self.cards	
		s = " " * (num < 10) + num + " -- " + c.str
		print "  " + s, ""
		if num % 2 == 0 then
			print
		else
			print " " * (28-s.len), ""
		end if
		num += 1
	end for
	if num % 2 == 0 then print
end function
Hand.analyze = function
	allSameSuit = true
	for i in range(1, self.cards.len-1)
		if self.cards[i].suit != self.cards[0].suit then allSameSuit = false
	end for
	if allSameSuit then
		self.rank = HandRank.Flush
		self.highCard = self.cards[0]
		return
	end if
	
	sortedCards = self.cards[:]
	sortedCards.sort "rank"
	self.rank = HandRank.Schmaltz
	for c in sortedCards; c.keep = false; end for
	keepAny = false
	
	for i in range(0, sortedCards.len-2)
		matchesNextCard = (sortedCards[i].rank == sortedCards[i+1].rank)
		if matchesNextCard then
			self.highCard = sortedCards[i]
			matchesPrevCard = (i > 0 and sortedCards[i].rank == sortedCards[i-1].rank)
			sortedCards[i].keep = true
			sortedCards[i+1].keep = true
			keepAny = true
			if self.rank.value < HandRank.Pair.value then
				self.rank = HandRank.Pair
			else if matchesPrevCard and self.rank == HandRank.Pair then
				self.rank = HandRank.Three
			else if self.rank == HandRank.Pair then
				self.rank = HandRank.TwoPair
			else if self.rank == HandRank.TwoPair then
				self.rank = HandRank.FullHouse
			else if matchesPrevCard then
				self.rank = HandRank.Four
			else
				self.rank = HandRank.FullHouse
			end if
		end if
	end for
	if not keepAny then
		if sortedCards[3].rank - sortedCards[0].rank == 3 then
			for i in range(0,3); sortedCards[i].keep = true; end for
			self.rank = HandRank.PartialStraight
		end if
		if sortedCards[4].rank - sortedCards[1].rank == 3 then
			if self.rank == HandRank.PartialStraight then
				self.rank = HandRank.Straight
				sortedCards[4].keep = true
				self.highCard = sortedCards[4]
			else
				self.rank = HandRank.PartialStraight
				for i in range(1,4); sortedCards[i].keep = true; end for
			end if
		end if
	end if
	if self.rank == HandRank.Schmaltz then
		self.highCard = sortedCards[4]
		sortedCards[4].keep = true
		sortedCards[3].keep = true
	end if
	
end function

// Some global constants, just to make the code more understandable
Ante = 5

// Player -- base class for computer and human
Player = {}
Player.balance = 200
Player.hand = null
Player.totalBet = 0
Player.anteUp = function
	self.balance -= Ante
	return Ante
end function
Player.newHand = function
	if self.hand then discard self.hand
	self.hand = Hand.deal
	self.totalBet = 0
end function
Player.addToPot = function(amount)
	self.balance -= amount
	Table.pot += amount
	drawHeaders
end function
Player.win = function
	self.balance += Table.pot
	Table.pot = 0
	drawHeaders
end function

// strategies the computer player might employ
Strategy = {}
Strategy.make = function(name, value=2, drawCount=null)
	result = new Strategy
	result.name = name
	result.value = value
	result.drawCount = drawCount
	return result
end function
Strategy.fold = Strategy.make("FOLD")
Strategy.check = Strategy.make("CHECK")
Strategy.raise = Strategy.make("RAISE", 2)
Strategy.bluff = function(value, drawCount); return Strategy.make("BLUFF", value, drawCount); end function
Strategy.bet = function(value); return Strategy.make("BET", value); end function

// computer player
computer = new Player
computer.strategy = null
computer.newHand = function
	super.newHand
	if self.hand.isWeak then
		if rand10 < 2 then
			self.strategy = Strategy.bluff(23, 2)
		else if rand10 < 2 then
			self.strategy = Strategy.bluff(23, 1)
		else if rand10 < 1 then
			self.strategy = Strategy.bluff(23, 0)
		else
			self.strategy = Strategy.fold
		end if
	else if self.hand.rank.value < HandRank.Three.value then
			if rand10 < 2 then self.strategy = Strategy.bluff(23, null) else self.strategy = Strategy.check
	else if self.hand.rank.value < HandRank.FullHouse.value then
		self.strategy = Strategy.bet(35)
	else
		if rand10 < 1 then self.strategy = Strategy.bet(35) else self.strategy = Strategy.raise
	end if
end function
computer.bet = function(minBet=1, openingBet=false)
	//print "My hand: "; self.hand.print; print "Strategy: " + self.strategy
	if self.balance < minBet then
		print "I fold."
		return 0
	end if
	if openingBet and (self.strategy == Strategy.check or self.strategy == Strategy.fold) then
		print "I check."
		return 0.5
	else if self.strategy == Strategy.fold and human.totalBet > 5 then
		print "I fold."
		return 0
	else if self.strategy == Strategy.check then
		print "I'll see you."
		return minBet
	else if openingBet then
		result = self.strategy.value + rand10
		if result > self.balance then result = self.balance
		if result == 0 then
			print "I check."
			return 0.5
		end if
		print "I'll open with $" + result
		return result	
	else
		bet = self.strategy.value + rand10
		if self.strategy == Strategy.raise then bet += minBet
		if bet > self.balance then bet = self.balance
		raise = bet - minBet		
		if raise <= 0 then
			print "I'll see you."
			return minBet
		else
			print "I'll see you, and raise you " + raise
			return bet
		end if
	end if
end function
computer.drawCards = function
	//print "My hand:"; self.hand.print	
	drawCount = 0
	for c in self.hand.cards; if not c.keep then drawCount += 1; end for
	print "I am taking " + drawCount + " card" + "s" * (drawCount != 1)
	for i in self.hand.cards.indexes
		if not self.hand.cards[i].keep then self.hand.replaceCard i		
	end for
	self.hand.analyze
	//print "My new hand: "; self.hand.print
	if self.strategy.name == "BLUFF" then
		self.strategy = Strategy.bluff(28)
	else if self.hand.isWeak then
		self.strategy = Strategy.fold
	else if self.hand.rank.value < HandRank.Three.value then
		if rand10 == 0 then self.strategy = Strategy.bet(19) else self.strategy = Strategy.raise
	else if self.hand.rank.value < HandRank.FullHouse.value then
		if rand10 == 0 then self.strategy = Strategy.bet(11) else self.strategy = Strategy.bet(19)
	else
		self.strategy = Strategy.raise
	end if		
end function
computer.win = function
	print; print "I win."
	super.win
end function
computer.checkFunds = function
	if self.balance >= Ante then return
	if human.balance < 50 then return	// BUGFIX
	if human.hasWatch or askYesNo("Would you like to buy back your watch for $50") == "no" then
		print "I'm busted.  Conglatulations!"
		exit
	end if
	self.balance += 50
	// Note: original BASIC code does not take money from the player, but let's fix that:
	human.balance -= 50		// BUGFIX
	human.hasWatch = true
	drawHeaders
end function
		

human = new Player
human.hasWatch = true
human.bet = function(minBet=1, openingBet=false)
	while true
		betStr = input("What is your bet? ")
		bet = betStr.val
		if bet == 0 and betStr != "0" then
			print "Enter 0 to fold, 0.5 to check, or a value of 1 or more to bet."
			continue
		else if bet == 0.5 and openingBet then
			return bet	// (check)
		else if 0 < bet < 1 then
			print "No small change, please."
			continue
		else if bet < minBet then
			print "If you can't see my bet, then fold."
			continue
		else if bet > self.balance then
			print "You can't bet with what you haven't got."
			self.trySellWatch
			continue
		end if
		return bet
	end while
end function
human.drawCards = function
	qty = askNumber("How many cards do you want", 3, 0, "You can't draw more than three cards.")
	if qty == 0 then return
	print "What are their numbers: "
	for i in range(1, qty)
		num = askNumber("", 5, 1)
		self.hand.replaceCard num - 1
	end for
	print "Your new hand:"
	self.hand.print
	self.hand.analyze
end function
human.win = function
	print; print "You win."
	super.win
end function
human.checkFunds = function
	if self.balance < Ante then self.trySellWatch
	if self.balance < Ante then
		print "Your wad is shot.  So long, sucker!"
		exit
	end if
end function
human.trySellWatch = function
	if not self.hasWatch then return
	if rand10 < 7 then
		value = 75
		msg = "I'll give you $" + value + " for it."
	else
		value = 25
		msg = "That's a pretty crummy watch - I'll give you $" + value + "."
	end if
	if computer.balance < value then return	// BUGFIX
	if askYesNo("Would you like to sell your watch") == "no" then return
	print msg
	self.balance += value
	self.hasWatch = false
	// Note: the original BASIC program does not actually take any money from the computer.
	// But let's do it right here:
	computer.balance -= value		// BUGFIX
	drawHeaders
end function

Table = {}
Table.pot = 0


deal = function
	Table.pot += computer.anteUp + human.anteUp
	drawHeaders
	computer.newHand
	human.newHand
end function

// Do a round of betting.  Return true to continue, or false
// if either player folds (ending the hand).
takeBets = function(computerFirst)
	if computerFirst then
		bet = computer.bet(1, true)
		if bet == 0 then
			human.win
			return false
		end if
		if bet == 0.5 then bet = 0	// "check" (no bet, but stay in the hand)
		computer.addToPot bet
	else
		bet = 0
	end if
	raise = bet
	canCheck = (bet == 0)
	while true
		bet = human.bet(raise, canCheck)
		if bet == 0 then
			computer.win
			return false
		end if
		if bet == 0.5 then	// (checked)
			if computerFirst then return true
			bet = 0
		else
			human.addToPot bet
			raise = bet - raise
			if raise == 0 then return true
		end if
		if computerFirst or bet > 0 then canCheck = false
		
		bet = computer.bet(raise, canCheck)
		if bet == 0 then
			human.win
			return false
		end if
		if bet == 0.5 then return true  // (checked)
		canCheck = false
		computer.addToPot bet
		raise = bet - raise
		if raise == 0 then return true
	end while
end function

playHand = function
	print
	computer.checkFunds
	human.checkFunds
	print "The ante is " + Ante + ".  I will deal:"
	print
	
	deal
	
	print "Your hand:"
	human.hand.print
	
	print	
	if not takeBets(true) then return
	
	print; print "Now we draw -- ", ""
	human.drawCards
	computer.drawCards
	
	if not takeBets(false) then return
	
	print; print "Now we compare hands:"
	print "My hand:"
	computer.hand.print 6
	print
	print "You have " + human.hand.rankStr
	print "I have " + computer.hand.rankStr
	if computer.hand.beats(human.hand) then
		computer.win
	else if human.hand.beats(computer.hand) then
		human.win
	else
		print "The hand is drawn."
		print "All $" + Table.pot + " remains in the pot."
	end if
end function

drawHeaders
while true
	playHand
	print "Now I have $" + computer.balance + " and you have $" + human.balance
	if askYesNo("Do you wish to continue") == "no" then break	
end while