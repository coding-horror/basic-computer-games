print " "*33 + "Pizza"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "Pizza Delivery Game"; print
name = input("What is your first name? "); print
print "Hi, " + name + ".  In this game you are to take orders"
print "for pizzas.  Then you are to tell a delivery boy"
print "where to deliver the ordered pizzas."; print; print

// Convert a house name like "G" into coordinates like [3,2].
nameToCoords = function(name)
	idx = name.code - "A".code
	row = floor(idx / 4)
	col = idx % 4
	return [col+1, row+1]
end function

// Convert house coordinates like [3,2] into a house name like "G".
coordsToName = function(coords)
	idx = (coords[1]-1)*4 + (coords[0]-1)
	return char("A".code + idx)
end function

askYesNo = function(prompt)
	while true
		yn = input(prompt + "? ").lower + " "
		if yn[0] == "y" then return "yes"
		if yn[0] == "n" then return "no"
		print "'Yes' or 'no' please, now then,"
	end while
end function	

input "(Press Return.)"; print

print "Map of the city of Hyattsville"; print
print " -----1-----2-----3-----4-----"
for row in range(4, 1)
	print "-"; print "-"; print "-"
	s = row + "     "
	for col in range(1, 4)
		s += coordsToName([col, row]) + "     "
	end for
	s += row
	print s
end for
print "-"; print "-"; print "-"
print " -----1-----2-----3-----4-----"

input
print "The output is a map of the homes where"
print "you are to send pizzas."; print
print "Your job is to give a truck driver"
print "the location or coordinates of the"
print "home ordering the pizza."; print
if askYesNo("Do you need more directions") == "yes" then
	print; print "Somebody will ask for a pizza to be"
	print "delivered.  Then a delivery boy will"
	print "ask you for the location.";
	print "     Example:"
	print "This is J.  Please send a pizza."
	print "Driver to " + name + ".  Where does j live?"
	print "Your answer would be 2,3"; print
	if askYesNo("Understand") == "no" then
		print "This job is definitely too difficult for you. Thanks anyway"
		exit
	end if
	print "Good.  you are now ready to start taking orders."; print
	print "Good luck!!"; print
end if

while true
	for turn in range(1,5)
		coords = [floor(rnd*4+1), floor(rnd*4+1)]
		buyer = coordsToName(coords)
		print "Hello " + name + "'s Pizza.  This is " + buyer + 
		  ".  Please send a pizza."
		while true
			while true
				inp = input("  Driver to " + name + ":  Where does " + buyer + " live? ")
				inp = inp.replace(",", " ").replace("  ", " ")
				inp = inp.split + ["0","0"]
				guess = [inp[0].val, inp[1].val]
				if 1 <= guess[0] <= 4 and 1 <= guess[1] <= 4 then break
			end while
			if guess == coords then
				print "Hello " + name + ".  This is " + buyer + ", thanks for the pizza."
				break
			else
				print "This is " + coordsToName(guess) + ".  I did not order a pizza."
				print "I live at " + guess.join(",")
			end if
		end while
		print
	end for
	if askYesNo("Do you want to deliver more pizzas") == "no" then break
end while

print; print "O.K. " + name + ", see you later!"; print
