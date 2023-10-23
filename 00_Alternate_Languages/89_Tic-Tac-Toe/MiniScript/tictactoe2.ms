solutions = [[0,1,2],[3,4,5],[6,7,8]]
solutions += [[0,3,6],[1,4,7],[2,5,8]]
solutions += [[0,4,8],[2,4,6]]

TicTacToe = {}
TicTacToe.grid = (" " * 9).split("")
TicTacToe.markers = {true: "X",false:"O"}
TicTacToe.AI = "X"
TicTacToe.human = "O"
TicTacToe.next = "X"
TicTacToe.lastMove = -1

TicTacToe.show = function
	print " " + self.grid[0:3].join(" ! ")
	print "---+---+---"
	print " " + self.grid[3:6].join(" ! ")
	print "---+---+---"
	print " " + self.grid[6:].join(" ! ")
	print
	print
end function

TicTacToe.set = function(player, pos)
	if self.grid[pos] != " " then
		return false
	end if
	self.grid[pos] = player
	self.lastMove = pos
	return true
end function

TicTacToe.setMarkers = function(mark)
	self.human = self.markers[mark == "X"]
	self.AI = self.markers[mark == "O"]
end function

TicTacToe.getWinner = function
	for mark in self.markers.values
		for solution in solutions
			cnt = 0
			for i in solution
				cnt += (self.grid[i] == mark)
			end for
			if cnt == 3 then return mark
		end for
	end for
	return null
end function

TicTacToe.potentialWins = function
	potential = {"X": [], "O": []}
	for mark in self.markers.values
		for solution in solutions
			cnt = 0
			emptyCells = [] 
			for i in solution
				cnt += (self.grid[i] == mark)
				if self.grid[i] == " " then emptyCells.push(i)
			end for 
			if cnt == 2 and emptyCells.len == 1 then potential[mark].push(emptyCells[0])
		end for
	end for
	return potential
end function

TicTacToe.moveAvailable = function
	return self.grid.indexOf(" ") != null
end function

TicTacToe.selectAI = function
	if self.grid[4] == " " then return 4
	
	potential = self.potentialWins
	
	AIWins = potential[self.AI]
	if AIWins.len >0 then return AIWins[0]
	
	HumanWins = potential[self.human]
	
	if HumanWins.len > 0 then return HumanWins[0]
	
	if [1,3,5,7].indexOf(self.lastMove) != null then
		for corner in [8,6,2,0]
			if self.grid[corner] == " " then
				self.grid[corner] = self.AI
				potential = self.potentialWins
				self.grid[corner] = " "
				AIWins = potential[self.AI]
				if AIWins.len > 0 then return corner
			end if
		end for
	else
		for side in [1,3,5,7]
			if self.grid[side] == " " then
				self.grid[side] = self.AI
				potential = self.potentialWins
				self.grid[side] = " "
				AIWins = potential[self.AI]
				if AIWins.len > 0 then return side
			end if
		end for
	end if
	for ix in range(0,8)
		if self.grid[ix] == " " then return ix
	end for
	
	return null		
end function

print " " * 15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "The board is numbered."
print; print; print
print " 1 2 3"
print " 4 5 6"
print " 7 8 9"
print
ans = input("Do you want to 'X' or 'O'? ")
if ans.upper != "X" then ans = "O"
TicTacToe.setMarkers(ans.upper)
winner = null
print
while TicTacToe.moveAvailable and winner == null
	
	if TicTacToe.next == TicTacToe.AI then
		move = TicTacToe.selectAI
		TicTacToe.set(TicTacToe.AI, move)
		TicTacToe.next = TicTacToe.human
		print "The computer moves to..."
	else
		move = input("Where do you move? ").val
		if move < 1 or move > 9 then
			print "Thanks for the game."
			exit
		else if not TicTacToe.set(TicTacToe.human, move-1) then
			print "That square is occupied."
			print
			continue
		else
			TicTacToe.next = TicTacToe.AI
		end if
	end if
	
	TicTacToe.show
	winner = TicTacToe.getWinner
end while

if winner == null then
	print "It's a draw. Thank you."
else if winner == TicTacToe.AI then
	print "I win, turkey!!"
else 
	print "You beat me! Good game!"
end if

