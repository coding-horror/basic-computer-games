import "stringUtil"

print " "*32 + "Calendar"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

startingDOW = 0
leapYear = false

// Note: while the original program required changes to the code to configure
// it for the current year, in this port we choose to ask the user.
// Here's the function to do that.
getParameters = function
	days = "sunday monday tuesday wednesday thursday friday saturday".split
	globals.startingDOW = 999
	while startingDOW == 999
		ans = input("What is the first day of the week of the year? ").lower
		if not ans then continue
		for i in days.indexes
			if days[i].startsWith(ans) then
				globals.startingDOW = -i
				break
			end if
		end for
	end while
	
	while true
		ans = input("Is it a leap year? ").lower
		if ans and (ans[0] == "y" or ans[0] == "n") then break
	end while
	globals.leapYear = (ans[0] == "y")
	
	while true
		ans = input("Pause after each month? ").lower
		if ans and (ans[0] == "y" or ans[0] == "n") then break
	end while
	globals.pause = (ans[0] == "y")
end function

getParameters
monthNames = [
        " JANUARY ",
        " FEBRUARY",
        "  MARCH  ",
        "  APRIL  ",
        "   MAY   ",
        "   JUNE  ",
        "   JULY  ",
        "  AUGUST ",
        "SEPTEMBER",
        " OCTOBER ",
        " NOVEMBER",
        " DECEMBER",
    ]
monthDays = [31, 28 + leapYear, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]

// Function to print one month calendar.
// month: numeric month number, 0-based (0-11)
printMonth = function(month)
	daysSoFar = monthDays[:month].sum
	daysLeft = monthDays[month:].sum
	print "** " + str(daysSoFar).pad(4) + "*"*18 + " " + monthNames[month] +
		" " + "*"*18 + " " + str(daysLeft).pad(4) + "**"
	print
	print "     S       M       T       W       T       F       S"
	print
	print "*" * 61
	// calculate the day of the week, from 0=Sunday to 6=Saturday
	dow = (daysSoFar - startingDOW) % 7
	print " " * 5 + " " * (8*dow), ""
	for i in range(1, monthDays[month])
		print str(i).pad(8), ""
		dow += 1
		if dow == 7 then
			dow = 0
			print
			if i == monthDays[month] then break
			print; print " " * 5, ""
		end if
	end for
	print
end function

// Main loop.
for month in range(0, 11)
	printMonth month
	print
	if month < 11 and pause then input
end for
