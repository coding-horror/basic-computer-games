print " "*33 + "Hello"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

print "Hello.  My name is Creative Computer."
print
print
ns = input("What's your name? ")
print
print "Hi there, " + ns + ", are you enjoying yourself here?"
while true
	bs = input.lower
	print
	if bs == "yes" then
		print "I'm glad to hear that, " + ns + "."
		print
		break
	else if bs == "no" then
		print "Oh, I'm sorry to hear that, " + ns + ". Maybe we can"
		print "brighten up your visit a bit."
		break
	else
		print "Please answer 'yes' or 'no'.  Do you like it here?"
	end if
end while
print
print "Say, " + ns + ", I can solve all kinds of problems except"
print "those dealing with Greece.  What kind of problems do"
print "you have (answer sex, health, money, or job)?"
while true
	cs = input
	print
	if cs != "sex" and cs != "health" and cs != "money" and cs != "job" then
		print "Oh, " + ns + ", your answer of " + cs + " is Greek to me."
	else if cs == "job" then
		print "I can sympathize with you " + ns + ".  I have to work"
		print "very long hours for no pay -- and some of my bosses"
		print "really beat on my keyboard.  My advice to you, " + ns + ","
		print "is to open a retail computer store.  It's great fun."
	else if cs == "money" then
		print "Sorry, " + ns + ", I'm broke too.  Why don't you sell"
		print "encyclopeadias or marry someone rich or stop eating"
		print "so you won't need so much money?"
	else if cs == "health" then
		print "My advice to you " + ns + " is:"
		print "     1.  Take two asprin"
		print "     2.  Drink plenty of fluids (orange juice, not beer!)"
		print "     3.  Go to bed (alone)"
	else
		print "Is your problem too much or too little?"
		while true
			ds = input.lower
			print
			if ds == "too much" then
				print "You call that a problem?!!  I should have such problems!"
				print "If it bothers you, " + ns + ", take a cold shower."
				break
			else if ds == "too little" then
				print "Why are you here in suffern, " + ns + "?  You should be"
				print "in Tokyo or New York or Amsterdam or someplace with some"
				print "real action."
				break
			else
				print "Don't get all shook, " + ns + ", just answer the question"
				print "with 'too much' or 'too little'.  Which is it?"
			end if
		end while
	end if
	print
	print "Any more problems you want solved, " + ns + "?"
	es = input.lower
	print
	if es == "yes" then
		print "What kind (sex, money, health, job)?"
	else if es == "no" then
		print "That will be $5.00 for the advice, " + ns + "."
		print "Please leave the money on the terminal."
		print
		wait 2
		print
		print
		while true
			gs = input("Did you leave the money? ").lower
			print
			if gs == "yes" then
				print "Hey, " + ns + "??? You left no money at all!"
				print "You are cheating me out of my hard-earned living."
				print
				print "What a rip off, " + ns + "!!!"
				print
				break
			else if gs == "no" then
				print "That's honest, " + ns + ", but how do you expect"
				print "me to go on with my psychology studies if my patient"
				print "don't pay their bills?"
				break
			else
				print "Your answer of '" + gs + "' confuses me, " + ns + "."
				print "Please respond with 'yes' or 'no'."
			end if
		end while
		break
	end if
end while
print
print "Take a walk, " + ns + "."
print
print
