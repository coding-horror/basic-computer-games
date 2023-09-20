print " "*33 + "Change"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

print "I, your friendly microcomputer, will determine"
print "the correct change for items costing up to $100."
print; print
while true
	itemCost = input("Cost of item? ").val
	if itemCost == 0 then break
	payment = input("Amount of payment? ").val
	change = payment - itemCost
	if change < 0 then
		print "Sorry, you have short-changed me $" + (itemCost - payment)
		continue
	else if change == 0 then
		print "Correct amount, thank you."
		continue
	end if

	print "Your change, $" + change

	dollars = floor(change/10)
	if dollars then print dollars + " ten dollar bill(s)"
	change -= dollars * 10

	fivers = floor(change/5)
	if fivers then print fivers + " five dollar bill(s)"
	change -= fivers * 5

	ones = floor(change)
	if ones then print ones + " one dollar bill(s)"
	change -= ones

	change *= 100		// (now working in cents)

	halfs = floor(change / 50)
	if halfs then print halfs + " one half dollar(s)"
	change -= halfs * 50

	quarters = floor(change / 25)
	if quarters then print quarters + " quarter(s)"
	change -= quarters * 25

	dimes = floor(change / 10)
	if dimes then print dimes + " dime(s)"
	change -= dimes * 10

	nickels = floor(change / 5)
	if nickels then print nickels + " nickel(s)"
	change -= nickels * 5

	pennies = round(change)
	if pennies then print pennies + " penny(s)"
	print "Thank you, come again."
	print; print
end while