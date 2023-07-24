print " "*34 + "Bug"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "The game Bug"
print "I hope you enjoy this game."
print
ans = input("Do you want instructions? ").lower
if not ans or ans[0] != "n" then
	print "The object of bug is to finish your bug before i finish"
	print "mine. Each number stands for a part of the bug body."
	print "I will roll the die for you, tell you what i rolled for you"
	print "what the number stands for, and if you can get the part."
	print "If you can get the part I will give it to you."
	print "The same will happen on my turn."
	print "If there is a change in either bug I will give you the"
	print "option of seeing the pictures of the bugs."
	print "Ihe numbers stand for parts as follows:"
	print "Number   Part    Number of part needed"
	print "1        body    1"
	print "2        neck    1"
	print "3        head    1"
	print "4        feelers 2"
	print "5        tail    1"
	print "6        legs    6"
	print
	input "(Press Return.)"	// (wait before starting the game)
	print
end if

// define a class to represent a bug (with all its body parts)
Bug = {}
Bug.body = false
Bug.neck = false
Bug.head = false
Bug.feelers = 0
Bug.tail = false
Bug.legs = 0
Bug.feelerLetter = "F"
Bug.pronoun = "I"

// add a method to determine if the bug is complete
Bug.complete = function
	return self.tail and self.feelers >= 2 and self.legs >= 6
end function

// add a method to draw the bug using print
Bug.draw = function
	if self.feelers then
		for row in range(1,4)
			print " "*10 + (self.feelerLetter + " ") * self.feelers
		end for
	end if
	if self.head then
		print "        HHHHHHH"
		print "        H     H"
		print "        H O O H"
		print "        H     H"
		print "        H  V  H"
		print "        HHHHHHH"
	end if
	if self.neck then
		print "          N N"
		print "          N N"
	end if
	if self.body then
		print "     BBBBBBBBBBBB"
		print "     B          B"
		print "     B          B"
		if self.tail then print "TTTTTB          B"
		print "     BBBBBBBBBBBB"
	end if
	if self.legs then
		for row in [1,2]
			print " "*5 + "L " * self.legs
		end for
	end if
end function

// add a method to add a part, if possible; return true if bug changed
Bug.addPart = function(partNum)
	if partNum == 1 then
		print "1=Body"
		if self.body then
			print self.pronoun + " do not need a body."
		else
			print self.pronoun + " now have a body."
			self.body = true
			return true
		end if
	else if partNum == 2 then
		print "2=neck"
		if self.neck then
			print self.pronoun + " do not need a neck."
		else if not self.body then
			print self.pronoun + " do not have a body."
		else
			print self.pronoun + " now have a neck."
			self.neck = true
			return true
		end if
	else if partNum == 3 then
		print "3=head"
		if self.head then
			print self.pronoun + " have a head."
		else if not self.neck then
			print self.pronoun + " do not have a neck."
		else
			print self.pronoun + " needed a head."
			self.head = true
			return true
		end if
	else if partNum == 4 then
		print "4=feelers"
		if self.feelers >= 2 then
			print self.pronoun + " have two feelers already."
		else if not self.head then
			print self.pronoun + " do not have a head."
		else
			if self.pronoun == "You" then
				print "I now give you a feeler."
			else
				print "I get a feeler."
			end if
			self.feelers += 1
			return true
		end if
	else if partNum == 5 then
		print "5=tail"
		if self.tail then
			print self.pronoun + " already have a tail."
		else if not self.body then
			print self.pronoun + " do not have a body."
		else
			if self.pronoun == "You" then			
				print "I now give you a tail."
			else
				print "I now have a tail."
			end if
			self.tail = true
			return true
		end if
	else if partNum == 6 then
		print "6=legs"
		if self.legs >= 6 then
			print self.pronoun + " have 6 feet."
		else if not self.body then
			print self.pronoun + " do not have a body."
		else
			self.legs += 1
			print self.pronoun + " now have " + self.legs + " leg" + "s"*(self.legs>1) + "."
			return true
		end if
	end if
	return 0
end function


// ...then, instantiate a bug for You (human player) and Me (computer)
you = new Bug
you.feelerLetter = "A"	// (don't ask me why)
you.pronoun = "You"
me = new Bug

// Main loop
while not you.complete and not me.complete
	anyChange = false
	die = floor(6 * rnd + 1)
	print; print "You rolled a " + die
	if you.addPart(die) then anyChange = true
	wait 2
	die = floor(6 * rnd + 1)
	print; print "I rolled a " + die
	if me.addPart(die) then anyChange = true
	if you.complete then print "Your bug is finished."
	if me.complete then print "My bug is finished."
	if anyChange then
		ans = input("Do you want the pictures? ").lower
		if not ans or ans[0] != "n" then
			print "*****Your Bug*****"
			print; print
			you.draw
			wait 2
			print
			print "*****My Bug*****"
			print; print
			me.draw
			wait 2
		end if
	end if
end while
print "I hope you enjoyed the game, play it again soon!!"

