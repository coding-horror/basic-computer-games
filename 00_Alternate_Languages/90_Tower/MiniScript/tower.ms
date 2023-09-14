kInvalidDisk = 100
kNotTopDisk = 200
kNotTower = 300
kGameOver = 300

Tower = {"disks": []}
Tower.init = function
	noob = new Tower
	noob.disks = []
	return noob
end function

Tower.height = function
	return self.disks.len
end function

Tower.top = function
	if self.height == 0 then return 100
	return self.disks[-1]
end function

Game = {}
Game["towers"] = []
Game["numOfDisks"] = 0
Game["rangeOfDisks"] = []
Game["selectedDisk"] = 0
Game["selectedDiskOn"] = 0
Game["selectedTower"] = 0
Game["inputErrors"] = 0
Game["turns"] = 0

Game.display = function
	print
	for r in range(7,1,-1)
		rowstr = ""
		for tower in self.towers
			if r > tower.height then
				rowstr += " " * 12 + "#" + " " * 7
			else
				spaces = (15 - tower.disks[r-1])/2
				disks = " " * 4 + tower.disks[r-1]
				rowstr += disks[-5:] + " " * spaces
				rowstr += "#" * tower.disks[r-1]
				rowstr += " " * spaces
			end if
			rowstr += " "
		end for
		print rowstr
	end for
	rowstr = (" " * 5 + "=" * 15 + " ") * 3
	print rowstr
	print
end function

Game.init = function(num)
	if num < 1 or num > 7 then
		self.inputErrors += 1
		return false
	end if
	Game.towers = []
	for i in range(0,2)
		Game.towers.push(Tower.init)
	end for
	
	first = self.towers[0]
	first.disks = range(15, 17 - num * 2, -2)
	self.numOfDisks = num
	self.rangeOfDisks = range(17 -num * 2, 15, 2)
	
	// This game doesn't like to be bothered
	// and keeps track of how many incorrect inputs
	// are made before it stops the game
	self.inputErrors = 0
	self.turns = 0
	return true
end function

Game.diskStatus = function
	n = self.selectedDisk
	if self.rangeOfDisks.indexOf(n) == null then
		self.inputErrors +=1
		return kInvalidDisk
	end if
	self.inputErrors = 0
	for i in range(0, self.towers.len - 1)
		if self.towers[i].top == n then
			self.selectedDiskOn = i
			self.inputErrors = 0
			return i
		end if
	end for
	return kNotTopDisk
end function

Game.pickDisk = function
	self.selectedDisk = input("Which disk would you like to move? ").val
	return self.diskStatus
end function

Game.pickTower = function
	self.selectedTower = input("Place disk on which needle? ").val - 1
	if not(0<= self.selectedTower and self.selectedTower <= 2) then
		self.inputErrors += 1
		return kNotTower
	end if
	return self.selectedTower
end function

Game.doneWithYou = function
	return self.inputErrors >= 2
end function

Game.isFinish = function
	return self.towers[0].disks.len == 0 and self.towers[1].disks.len == 0
end function

Game.move = function
	print "Take turn # " + (self.turns + 1)
	status = -1
	self.inputErrors = 0
	while 1
		status = self.pickDisk
		if 0 <= status and status <= 2 then break
		if status == kInvalidDisk and self.doneWithYou then
			print "Stop wasting my time. Go bother someone else."
			exit
		else if status == kInvalidDisk then
			msg = "Illegal entry ... you may only type "
			msg += self.rangeOfDisks[0:-1].join(",") + " "
			if self.rangeOfDisks.len > 1 then
				msg += "or "
			end if
			msg += "15"
			print msg
		else if status == kNotTopDisk then
			print "That disk is below another. Make another choice."
		end if
	end while
	
	self.inputErrors = 0
	while 1
		status = self.pickTower
		if 0 <= status and status <= 2 then break
		if status == kNotTower and self.doneWithYou then
			print "I tried to warn you. But you wouldn't listen."
			print "By bye, big shot."
			exit
		else if status == kNotTower then
			print "I'll assume you hit the wrong ket this time. But watch it"
			print "I only allow one mistake."
		end if
	end while
	
	if self.selectedDisk > self.towers[self.selectedTower].top then
		print "You can't place a larger disk on a top of a smaller one,"
		print "it may crush it!"
	else
		n=self.towers[self.selectedDiskOn].disks.pop
		self.towers[self.selectedTower].disks.push(n)
		self.turns += 1
		self.inputErrors = 0
	end if
end function


print " " * 33 + "TOWERS"
print " " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print;print
print "You must transfer the disks from the left to the right"
print "tower, one at a time, never putting a larger disk on a"
print "smaller disk."
print

ans = "Y"
while ans[0].upper == "Y"
	while 1
		disks = input ("How many disks do you want to move (7 is MAX)? ").val
		status = Game.init(disks)
		if status == false and Game.doneWithYou then
			print "All right, wise guy, if you can't play the game right, I'll"
			print "take my puzzle and go home. So long."
			exit
		else if not status then
			print "Sorry, but I can't do that job for you"
		else
			break
		end if
	end while
	
	while not Game.isFinish
		Game.display
		
		Game.move
	end while
	Game.display
	print "Congratulations!"
	print "You performed the task in " + Game.turns + " moves."
	print
	ans = input("Play again (Yes or No)? ")
end while
print
print "Thanks for the game!"
