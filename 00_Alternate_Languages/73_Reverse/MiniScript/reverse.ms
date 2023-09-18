num = 9

reverse = function(i)
	if i == null then return i
	ret = []
	for item in i
		ret.insert(0,item)
	end for
	return ret
end function

showRules = function
	print
	print "This is the game of 'Reverse'. To win, all you have"
	print "to do is arrange a list of numbers (1 through " + num + ")"
	print "in numerical order from left to right. To move, you"
	print "tell me how many numbers (counting from the left) to"
	print "reverse. For example, if the current list is:"
	print; print "2 3 4 5 1 6 7 8 9"
	print; print "and you reverse 4, the result will be:"
	print; print "5 4 3 2 1 6 7 8 9"
	print; print "Now if reverse 5, you win!"
	print; print "1 2 3 4 5 6 7 8 9"
	print
	print "No doubt you will like this game, but"
	print "if you want to quit, reverse 0 (zero)."
	print
	return
end function

printState = function
	print;print digits.join(" "); print
end function

print " " * 32 + "Reverse"
print " " * 15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "Reverse -- a game of skill"
print

ans = input("Do you want the rules? ") + " "
if ans != null and ans[0].lower == "y" then showRules

while true
	turns = 0
	digits = range(1, num)
	digits.shuffle
	print;print "Here we go ... the list is:"
	while true
		printState
		amt = input("How many shall I reverse? ").val
		if amt == null or amt == 0 then break
		
		if amt > num then
			print "OOPS! Too many! I can reverse at most " + num
		else
			turns += 1
			digits = reverse(digits[:amt]) + digits[amt:]
		end if
		if digits == range(1,num) then
			printState
			print "You won it in " + turns + " moves!!"
			break
		end if
	end while
	print
	ans = input("Try again (YES or NO)? ") + " "
	print
	if ans == null or ans[0].lower != "y" then break
end while
print "O.K. Hope you had fun!!"
