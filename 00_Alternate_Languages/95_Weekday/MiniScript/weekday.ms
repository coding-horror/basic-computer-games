TAB = char(9)

Age = {"m": 0, "d": 0, "y": 0}
Age.init = function(m,d,y)
	noob = new Age
	noob.m = m;noob.d = d;noob.y = y
	return noob
end function

Age.sub = function(a)
	m1 = self.m; d1 = self.d; y1 = self.y
	d1 = d1 - a.d
	if d1 < 0 then
		d1 = d1 + 30
		m1 = m1 - 1
	end if
	m1 = m1 - a.m
	if m1 < 0then
		m1 = m1 + 12
		y1 = y1 - 1
	end if
	y1 = y1 - a.y
	return Age.init(m1,d1,y1)
end function

Age.multiply = function(multiplier)
	ageInDays = self.y *365 + self.m * 30 + self.d + floor(self.m / 2)
	newAge = ageInDays * multiplier
	years = floor(newAge/ 365)
	leftover = newAge % 365
	months = floor(leftover / 30)
	days = floor(leftover % 30)
	return Age.init(months, days, years)
end function

Date = {"m": null, "d": null, "y": null}

// the number of days between the 1st of one month to the next
Date.daysPerMonth = [0,31,28,31,30,31,30, 31,31,30,31,30]
Date.dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday",
"Thursday", "Friday", "Saturday"]

Date.init = function(dt)
	d = dt.split(",")
	if d.len != 3 then return
	noob = new Date
	noob.m = d[0].val
	noob.d = d[1].val
	noob.y = d[2].val
	return noob
end function

Date.diff = function(mdy)
	dday = self.d - mdy.d
	dmonth = self.m - mdy.m
	if dday < 0 then
		dmonth -= 1
		dday += 30
	end if
	
	dyear = self.y - mdy.y
	if dmonth <0 then
		dyear -= 1
		dmonth += 12
	end if
	return Age.init(dmonth, dday, dyear)
end function

Date._isLeapYear = function
	return (self.y % 4 == 0 and self.y % 100 != 0) or self.y % 400 == 0
end function

Date.value = function
	//Not accepting dates Jan 1st 1583 this because the
	//transistion to Gregorian calendar occurred in 1582.
	
	//calculating days since the end of 1582
	years = self.y - 1583
	days = years * 365 + self._leapYears + Date.daysPerMonth[:self.m].sum + self.d
	return days // returns 1 for 1,1,1583
end function

Date.dayOfWeek = function
	// 1,1,1583 is a Saturday
	// Date.value calculates a value of 1 for that date
	return (self.value + 5) % 7
end function

Date.weekday = function
	return Date.dayNames[self.dayOfWeek]
end function

// get # of lear yeaps since the change to Gregorian
Date._leapYears = function
	ly = floor((self.y - 1580) / 4)
	
	//exclude centuries
	centuries = floor((self.y - 1500) / 100)
	
	//unless centuries divisible by 400
	centuries400 = floor((self.y - 1200) / 400) 
	ly = ly - centuries + centuries400
	
	if self._isLeapYear and self.m < 3 then ly -= 1
	return ly
end function

print " "*32 + "WEEKDAY"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

print "WEEKDAY is a computer demonstration that"
print "gives facts about a date of interest to you."
print

mdy = input("Enter today's date in the form: 3,24,1979? ")
today = Date.init(mdy)


mdy = input("Enter day of birth (or other day of interest)? ")
dob = Date.init(mdy)

print
if dob.y < 1583 then
	print "Not prepared to give day of the week prior to 1583"
	exit
end if

verb = " was a "
if today.value < dob.value then verb= " will be a "
if today.value == dob.value then verb = " is a "

if dob.d == 13 and dob.weekday == "Friday" then
	endMsg = " The Thirteenth--Beware!"
else
	endMsg = "."
end if
print dob.m + "/" + dob.d + "/" + dob.y + verb + dob.weekday + endMsg

age = today.diff(dob)

totalAge = Age.init(age.m,age.d,age.y)
if verb == " was a " then
	if dob.d == today.d and dob.m == today.m then print "***HAPPY BIRTHDAY***"
	
	lines= [["", "YEARS", "MONTHS", "DAYS"]]
	lines.push(["", "-----", "------", "----"])
	lines.push(["Your age (if birthdate)", age.y,age.m, age.d])
	
	spent = age.multiply(.35)
	lines.push(["You have slept", spent.y,spent.m, spent.d])
	totalAge = totalAge.sub(spent)
	
	spent = age.multiply(.17)
	lines.push(["You have eaten", spent.y,spent.m, spent.d])
	totalAge = totalAge.sub(spent)
	
	if totalAge.y <= 3 then
		phrase = "You have played"
	else if totalAge.y <= 9 then
		phrase = "You have played/studied"
	else
		phrase = "You have worked/played"
	end if
	
	spent = age.multiply(.23)
	lines.push([phrase, spent.y,spent.m, spent.d])	
	totalAge = totalAge.sub(spent)
	
	relaxed = totalAge
	lines.push(["You have relaxed", relaxed.y, relaxed.m, relaxed.d])
	for line in lines
		col0 = (" " * 25 + line[0])[-25:]
		col1 = (line[1] + " " * 6)[:6]
		col2 = (line[2] + " " * 7)[:7]
		col3 = (line[3] + " " * 5)[:5]
		print (col0+" " + col1+col2+col3)
	end for
end if

print
print " "*16 + "***  Yoy may retire in " + (dob.y + 65) + "  ***"
