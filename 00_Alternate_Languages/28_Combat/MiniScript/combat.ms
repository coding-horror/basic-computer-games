print " "*33 + "Combat"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

printInColumns2 = function(a, b, lineBreak=true)
	print (a+" "*16)[:16] + (b+" "*16)[:16], ""
	if lineBreak then print
end function

printInColumns3 = function(a, b, c, lineBreak=true)
	print (a+" "*16)[:16] + (b+" "*16)[:16] + (c+" "*16)[:16], ""
	if lineBreak then print
end function

// computer forces:
d = 30000		// army
e = 20000		// navy
f = 22000		// air force

print "I am at war with you."; print "We have 72000 soldiers apiece."
while true
	print; print "Distribute your forces."
	printInColumns3 "", "ME", "YOU"
	printInColumns2 "army", d, false
	a = input("?").val
	printInColumns2 "navy", e, false
	b = input("?").val
	printInColumns2 "a. f.", f, false
	c = input("?").val
	if a+b+c <= 72000 then break
end while


print "You attack first. Type (1) for army; (2) for navy;"
print "and (3) for air force."
y = input.val
while true
	x = input("How many men? ").val
	if x < 0 then continue
	if y <= 1 or y > 3 then
		// Army attack
		if x > a then continue
		if x < a/3 then
			print "You lost "+x+" men from your army."
			a=floor(a-x)
		else if x < 2*a/3 then
			print "You lost " + floor(x/3) + " men, but I lost " + floor(2*d/3)
			a=floor(a-x/3)
			d=0		// (message above lied!)
		else
			print "You sunk one of my patrol boats, but I wiped out two"
			print "of your air force bases and 3 army bases."
			a=floor(a/3)
			c=floor(c/3)
			e=floor(2*e/3)
		end if
	else if y == 2 then
		// Naval attack
		if x > b then continue
		if x < e/3 then
			print "Your attack was stopped!"
			b = floor(b-x)
		else if x < 2*e/3 then
			print "You destroyed " + floor(2*e/3) + "of my army."
			e=floor(e/3)
		else
			print "You sunk one of my patrol boats, but I wiped out two"
			print "of your air force bases and 3 army bases."
			a=floor(a/3)
			c=floor(c/3)
			e=floor(2*e/3)
		end if
	else
		// Air force attack
		if x > c then continue
		if x < c/3 then
			print "Your attack was wiped out."
			c = floor(c-x)
		else if x < 2*c/3 then
			print "We had a dogfight. You won - and finished your mission."
			d=floor(2*d/3)
			e=floor(e/3)
			f=floor(f/3)
		else
			print "You wiped out one of my army patrols, but I destroyed"
			print "two navy bases and bombed three army bases."
			a=floor(a/4)
			b=floor(b/3)
			d=floor(2*d/3)
		end if
	end if
	break
end while

result = null		// 1 you win, -1 you lose, 0 tie (treaty)
print
printInColumns3 "", "YOU", "ME"
printInColumns3 "army", a, d
printInColumns3 "navy", b, e
printInColumns3 "a. f.", c, f
print "What is your next move?"
print "army=1  navy=2  air force=3"
g = input.val
while true
	t = input("How many men? ").val
	if t < 0 then continue
	if g <= 1 or g > 3 then
		// Army attack
		if t > a then continue
		if t < d/2 then
			print "I wiped out your attack!"
			a = a-t
		else
			print "You destroyed my army!"
			d=0
		end if
	else if g == 2 then
		// Naval attack
		if t > b then continue
		if t < e/2 then
			print "I sunk two of your battleships, and my air force"
			print "wiped out your ungaurded capitol."	// (sic)
			a = a/4
			b = b/2
		else
			print "Your navy shot down three of my xiii planes,"
			print "and sunk three battleships."
			f = 2*f/3
			e = (e/2)
		end if
	else
		// Air Force attack
		if t > c then continue
		if t > f/2 then
			print "My navy and air force in a combined attack left"
			print "your country in shambles."
			a = a/3
			b = b/3
			c = c/3
		else
			print "One of your planes crashed into my house. I am dead."
			print "My country fell apart."
			result = 1
		end if
	end if
	break
end while

if result == null then
	print
	print "From the results of both of your attacks,"
	result = 0
	if a+b+c > 3/2*(d+e+f) then result = 1
	if a+b+c < 2/3*(d+e+f) then result = -1
end if

if result == 0 then
	print "the treaty of paris concluded that we take our"
	print "respective countries and live in peace."
else if result == 1 then
	print "You won, oh! shucks!!!!"
else
	print "You lost-I conquered your country.  It serves you"
	print "right for playing this stupid game!!!"
end if
