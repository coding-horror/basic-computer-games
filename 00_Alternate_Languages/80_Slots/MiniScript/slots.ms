center = function(s,n)
	h = floor((n - s.len)/2)
	s = " " * h + s + " " * n
	return s[:n]
end function

Machine = {"symbols": ["Bar", "Bell", "Orange", "Lemon", "Plum ", "Cherry"]}
Machine.money = 0
Machine.bet = 0
Machine.playing = true
Machine.reels = [0,0,0]
Machine.results = null

Machine.reelsSpin = function(n, msg, row)
	env = [1,.95,.9,.85,.25,.2,.15,.1,.05,0]
	bell = new Sound
	bell.init .1, 800, env, Sound.sineWave
	for i in range(1,2)
		s1 = new Sound
		s1.init .2, i * 800 + 800, env, Sound.sineWave
		bell.mix(s1, 1 / (2 ^ i))
	end for
	for i in range(1,n)
		bell.play
		ix = 0
		while bell.isPlaying
			text.row = row
			ix = (ix + 1) % self.symbols.len
			print msg + center(self.symbols[ix],8)
		end while
	end for
end function

Machine.makeBet = function()
	while true
		bet = floor(input("Your bet? ").val)
		if bet > 100 then
			print "House limits are $100"
		else if bet < 1 then
			print "Minimum bet is $1"
		else
			break
		end if
	end while
	Machine.bet = bet
end function

Machine.pullHandle = function
	if text.row < 3 then
		print; print
		text.row = text.row + 2
	end if
	row = text.row
	msg = ""
	for i in range(0,2)
		self.reelsSpin(5 + 5 * (i==0), msg, row)
		wait .1
		symIx = floor(rnd * self.symbols.len)
		msg += center(self.symbols[symIx],9)
		self.reels[i] = symIx
		text.row = row 
		print msg
	end for
end function

Machine.winnings = function
	bet = Machine.bet
	barIdx = self.symbols.indexOf("Bar")
	numBars = 0
	multiples = 0
	for i in range(0,2)
		numBars += (self.reels[i] == barIdx)
		multiples += (self.reels[i] == self.reels[(i + 1) % 3])
	end for
	
	if numBars == 3 then return {"won": bet * 101, "msg": "***Jackpot***"}
	if numBars == 2 then return {"won": bet * 6, "msg": "*Double Bar*"}
	if multiples == 3 then return {"won": bet * 11, "msg": "**Top Dollar**"}
	if multiples == 1 then return {"won": bet * 3, "msg": "Double!!"}
	return {"won": -bet, "msg": "You lost."}
end function

Machine.results = function
	result = Machine.winnings
	self.money += result.won
	print result.msg
	if result.won > 0 then
		print "You Won!!"
	end if
	print "Your standings are $" + self.money
	return
end function	

Machine.playAgain = function
	ans = input("Again? ") + " "
	self.playing = (ans[0].lower == "y")
	if self.playing then return
	print
	if self.money < 0 then
		print "Pay up! Please leave your money on the terminal."
	else if self.money == 0 then
		print "Hey, you broke even."
	else
		print "Collect your winnings from the H&M cashier."
	end if
end function

clear
print " " * 30 + "Slots"
print " " * 15 + "Creative Computing  Morristown, New Jersey"
print;print;print
print "You are in the H&M Casino, in front of one of our"
print "one-arm bandits. Bet from $1 to $100."
print "to pull the arm, punch the return key after making your bet."

print
while Machine.playing
	Machine.makeBet
	print
	Machine.pullHandle
	print
	Machine.results
	Machine.playAgain
	print
end while
