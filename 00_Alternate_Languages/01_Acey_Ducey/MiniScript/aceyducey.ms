print " "*26 + "Acey Ducey Card Game"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print
print
print "Acey-ducey is played in the following manner."
print "The dealer (computer) deals two cards face up."
print "You have an option to bet or not bet depending"
print "on whether or not you feel the card will have"
print "a value between the first two."
print "If you do not want to bet, input a 0."

cards = range(2,10) + ["Jack", "Queen", "King", "Ace"]

while true
	money = 100
	
	while true
		print "You now have " + money + " dollars."
		print
		print "Here are your next two cards:"
		while true
			A = floor(rnd * cards.len)
			B = floor(rnd * cards.len)
			if B > A then break
		end while
		print cards[A]
		print cards[B]
		bet = input("What is your bet? ").val
		while bet > money
			print "Sorry, my friend, but you bet too much."
			print "You have only " + money + " dollars to bet."
			bet = input("What is your bet? ").val
		end while
		if bet == 0 then
			print "Chicken!!"
			continue
		end if
		C = floor(rnd * cards.len)
		print cards[C]
				
		if C <= A or C >= B	then
			print "Sorry, you lose."
			money -= bet
			if money <= 0 then break
		else
			print "You win!!!"
			money += bet
		end if
	end while
	
	print
	print
	print "Sorry, friend, but you blew your wad."
	print; print
	again = input("Try again (yes or no)? ").lower
	if again and again[0] == "n" then break
end while

print "O.K., hope you had fun!"