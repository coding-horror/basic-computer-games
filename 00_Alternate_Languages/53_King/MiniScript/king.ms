print " "*34 + "KING"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

yearsRequired = 8
rallods = floor(60000+(1000*rnd)-(1000*rnd))
countrymen = floor(500+(10*rnd)-(10*rnd))
landArea = 2000
foreignWorkers = 0
sellableLandExplained = false
tourism = 0
year = 0

zs = input("Do you want instructions? ").lower
if zs == "again" then
	while true
		year = input("How many years had you been in office when interrupted? ").val
		if 0 <= year < yearsRequired then break
		print "   Come on, your term in office is only " + yearsRequired + " years."
	end while
	rallods = input("How much did you have in the treasury? ").val
	countrymen = input("How many countrymen? ").val
	foreignWorkers = input("How many foreign workers? ").val
	while true
		landArea = input("How many square miles of land? ").val
		if 1000 <= landArea <= 2000 then break
		print "   Come on, you started with 1000 sq. miles of farm land"
		print "   and 1000 sq. miles of forest land."
	end while
else if not zs or zs[0] != "n" then
	print; print; print
	print "Congratulations! You've just been elected premier of Setats"
	print "Detinu, a small communist island 30 by 70 miles long. Your"
	print "job is to decide upon the contry's budget and distribute"
	print "money to your countrymen from the communal treasury."
	print "The money system is rallods, and each person needs 100"
	print "rallods per year to survive. Your country's income comes"
	print "from farm produce and tourists visiting your magnificent"
	print "forests, hunting, fishing, etc. Half your land is farm land"
	print "which also has an excellent mineral content and may be sold"
	print "to foreign industry (strip mining) who import and support"
	print "their own workers. Crops cost between 10 and 15 rallods per"
	print "square mile to plant."
	print "Your goal is to complete your " + yearsRequired + " year term of office."
	print "Good luck!"
end if

print


// Bonus little feature when running in Mini Micro: display a status bar with
// key game stats at the top of the screen.
updateStatusBar = function
	if version.hostName != "Mini Micro" then return
	display(2).mode = displayMode.text; td = display(2)
	s = " Rallods: " + (rallods + " "*8)[:8]
	s += " Land: " + (landArea + " "*6)[:6]
	s += " Countrymen: " + (countrymen + " "*6)[:6]
	s += " Foreigners: " + (foreignWorkers + " "*5)[:5]
	td.color = color.black; td.backColor = text.color
	td.row = 25; td.column = 0;	td.print s
end function

min0 = function(x)
	if x < 0 then return 0 else return x
end function
floorMin0 = function(x)
	return min0(floor(x))
end function
stop = function
	updateStatusBar
	print; print; exit
end function

while true
	landPrice = floor(10*rnd+95)
	plantingArea = 0
	pollutionControl = 0
	deaths = 0
	
	print
	print "You now have " + rallods + " rallods in the treasury."
	print floor(countrymen) + " countrymen, ", ""
	costToPlant = floor(((rnd/2)*10+10))
	if foreignWorkers != 0 then
		print floor(foreignWorkers) + " foreign workers, ", ""
	end if
	print "and " + floor(landArea) + " sq. miles of land."
	print "This year industry will buy land for " + landPrice, ""
	print " rallods per square mile."
	print "Land currently costs " + costToPlant + " rallods per square mile to plant."
	print
	
	updateStatusBar
	while true
		sellToIndustry = input("How many square miles do you wish to sell to industry? ").val
		if sellToIndustry < 0 then continue
		if sellToIndustry <= landArea-1000 then break
		print "***  Think again. You only have " + (landArea-1000) + " square miles of farm land."
		if not sellableLandExplained then
			print;print "(Foreign industry will only buy farm land because"
			print "forest land is uneconomical to strip mine due to trees,"
			print "thicker top soil, etc.)"
			sellableLandExplained = true
		end if
	end while
	landArea = floor(landArea-sellToIndustry)
	rallods = floor(rallods+(sellToIndustry*landPrice))
	
	updateStatusBar
	while true
		welfare = input("How many rallods will you distribute among your countrymen? ").val
		if welfare < 0 then continue
		if welfare <= rallods then break
		print "   Think again. You've only " + rallods + " rallods in the treasury"
	end while
	rallods = floor(rallods-welfare)
	
	updateStatusBar
	while rallods > 0
		plantingArea = input("How many square miles do you wish to plant? ").val
		if plantingArea < 0 then continue
		if plantingArea > countrymen*2 then
			print "   Sorry, but each countryman can only plant 2 sq. miles."
			continue
		end if
		if plantingArea > landArea-1000 then
			print "   Sorry, but you've only " + (landArea-1000) + " sq. miles of farm land."
			continue
		end if
		plantingCost = floor(plantingArea * costToPlant)
		if plantingCost <= rallods then break
		print "   Think again. You've only " + rallods + " rallods left in the treasury."
	end while
	rallods -= plantingCost
	
	updateStatusBar
	while rallods > 0
		pollutionControl = input("How many rallods do you wish to spend on pollution control? ").val
		if pollutionControl < 0 then continue
		if pollutionControl <= rallods then break
		print "   Think again. You only have " + rallods + " rallods remaining."
	end while
	
	if sellToIndustry == 0 and welfare == 0 and plantingArea == 0 and pollutionControl == 0 then
		print
		print "Goodbye."
		print "(If you wish to continue this game at a later date, answer"
		print "'again' when asked if you want instructions at the start"
		print "of the game.)"
		exit
	end if
	
	print
	print
	
	rallods = floor(rallods-pollutionControl)
	updateStatusBar
	
	original_rallods = rallods
	
	starvationDeaths = floorMin0(countrymen - welfare/100)
	if starvationDeaths then
		if welfare/100 < 50 then
			print
			print
			print "Over one third of the popultation has died since you"
			print "were elected to office. The people (remaining)"
			print "hate your guts."
			if rnd > .5 then
				print "You have been thrown out of office and are now"
				print "residing in prison."
			else
				print "You have been assassinated."
			end if
			countrymen -= starvationDeaths
			stop		
		end if
		print starvationDeaths + " countrymen died of starvation"
	end if
	
	pollutionDeaths = floorMin0(rnd*(2000-landArea))
	if pollutionControl >= 25 then
		pollutionDeaths = floor(pollutionDeaths/(pollutionControl/25))
	end if
	if pollutionDeaths > 0 then
		print pollutionDeaths + " countrymen died of carbon-monoxide and dust inhalation"
	end if
	
	deaths = starvationDeaths + pollutionDeaths
	if deaths then
		print "   You were forced to spend " + floor(deaths*9), ""
		print " rallods on funeral expenses"
		rallods -= deaths * 9
	end if
	
	if rallods < 0 then
		print "   Insufficient reserves to cover cost - land was sold"
		landArea = floorMin0(landArea+(rallods/landPrice))
		rallods = 0
	end if
	
	countrymen = min0(countrymen - deaths)
	
	if sellToIndustry then
		newForeigners = floor(sellToIndustry+(rnd*10)-(rnd*20))
		if foreignWorkers == 0 then newForeigners += 20
		foreignWorkers += newForeigners
		print newForeigners + " workers came to the country and ", ""
	end if
	
	immigration = floor(((welfare/100-countrymen)/10)+(pollutionControl/25)-((2000-landArea)/50)-(pollutionDeaths/2))
	print abs(immigration) + " countrymen ", ""
	if immigration < 0 then print "came to", "" else print "left", ""
	print " the island."
	countrymen = floorMin0(countrymen + immigration)
	
	cropLoss = floor(((2000-landArea)*((rnd+1.5)/2)))
	if cropLoss > plantingArea then cropLoss = plantingArea
	if foreignWorkers > 0 then print "Of " + floor(plantingArea) + " sq. miles planted,", ""
	print " you harvested " + floor(plantingArea-cropLoss) + " sq. miles of crops."
	if cropLoss then
		print "   (Due to air and water pollution from foreign industry.)"
	end if
	agriculturalIncome = floor((plantingArea-cropLoss)*(landPrice/2))
	print "Making " + agriculturalIncome + " rallods."
	rallods += agriculturalIncome
	
	v1 = floor(((countrymen-immigration)*22)+(rnd*500))
	v2 = floor((2000-landArea)*15)
	prevTourism = tourism
	tourism = abs(floor(v1-v2))
	print " You made " + tourism + " rallods from tourist trade."
	if v2 > 2 and tourism < prevTourism then
		print "   Decrease because ", ""
		g1 = 10*rnd
		if g1 <= 2 then
			print "fish population has dwindled due to water pollution."
		else if g1 <= 4 then
			print "air pollution is killing game bird population."
		else if g1 <= 6 then
			print "mineral baths are being ruined by water pollution."
		else if g1 <= 8 then
			print "unpleasant smog is discouraging sun bathers."
		else
			print "hotels are looking shabby due to smog grit."
		end if
	end if
	rallods += tourism
	updateStatusBar
	
	if deaths > 200 then
		print
		print
		print deaths + "countrymen died in one year!!!!!"
		print "due to this extreme mismanagement, you have not only"
		print "been impeached and thrown out of office, but you"
		m6 = floor(rnd*10)
		if m6 <= 3 then 1670
		if m6 <= 6 then 1680
		if m6 <= 10 then 1690
		print "also had your left eye gouged out!"
		goto 1590
		print "have also gained a very bad reputation."
		goto 1590
		print "have also been declared national fink."
		stop
	else if countrymen < 343 then
		print
		print
		print "Over one third of the popultation has died since you"
		print "were elected to office. The people (remaining)"
		print "hate your guts."
		if rnd > .5 then
			print "You have been thrown out of office and are now"
			print "residing in prison."
		else
			print "You have been assassinated."
		end if
		stop
	else if (original_rallods/100) > 5 and deaths - pollutionDeaths >= 2 then
		print
		print "Money was left over in the treasury which you did"
		print "not spend. As a result, some of your countrymen died"
		print "of starvation. The public is enraged and you have"
		print "been forced to either resign or commit suicide."
		print "The choice is yours."
		print "If you choose the latter, please turn off your computer"
		print "before proceeding."
		stop
	else if foreignWorkers > countrymen then
		print
		print
		print "The number of foreign workers has exceeded the number"
		print "of countrymen. As a minority, they have revolted and"
		print "taken over the country."
		if rnd > .5 then
			print "You have been thrown out of office and are now"
			print "residing in prison."
		else
			print "You have been assassinated."
		end if
		stop
	else if year == yearsRequired-1 then
		print
		print
		print "Congratulations!!!!!!!!!!!!!!!!!!"
		print "You have succesfully completed your " + yearsRequired + " year term"
		print "of office. You were, of course, extremely lucky, but"
		print "nevertheless, it's quite an achievement. Goodbye and good"
		print "luck - you'll probably need it if you're the type that"
		print "plays this game."
		stop
	end if
	
	updateStatusBar
	wait
	year += 1
end while

//print
//print
//print "the number of foreign workers has exceeded the number"
//print "of countrymen. as a minority, they have revolted and"
//print "taken over the country."
//if rnd<=.5 then 1580
//print "you have been thrown out of office and are now"
//print "residing in prison."
//goto 1590
//print "you have been assassinated."
//print
//print
//exit
//print
//print
//print deaths + "countrymen died in one year!!!!!"
//print "due to this extreme mismanagement, you have not only"
//print "been impeached and thrown out of office, but you"
//m6 = floor(rnd*10)
//if m6 <= 3 then 1670
//if m6 <= 6 then 1680
//if m6 <= 10 then 1690
//print "also had your left eye gouged out!"
//goto 1590
//print "have also gained a very bad reputation."
//goto 1590
//print "have also been declared national fink."
//goto 1590
//
//print
//print
//print "over one third of the popultation has died since you"
//print "were elected to office. the people (remaining)"
//print "hate your guts."
//goto 1570
//if deaths-pollutionDeaths < 2 then 1515
//print
//print "money was left over in the treasury which you did"
//print "not spend. as a result, some of your countrymen died"
//print "of starvation. the public is enraged and you have"
//print "been forced to either resign or commit suicide."
//print "the choice is yours."
//print "if you choose the latter, please turn off your computer"
//print "before proceeding."
//goto 1590
//print
//print
//print "congratulations!!!!!!!!!!!!!!!!!!"
//print "you have succesfully completed your" + yearsRequired + "year term"
//print "of office. you were, of course, extremely lucky, but"
//print "nevertheless, it's quite an achievement. goodbye and good"
//print "luck - you'll probably need it if you're the type that"
//print "plays this game."
//goto 1590
//
//print "how many years had you been in office when interrupted";
//input year
//if year < 0 then 1590
//if year < 8 then 1969
//print "   come on, your term in office is only" + yearsRequired + "years."
//goto 1960
//print "how much did you have in the treasury";
//input rallods
//if rallods < 0 then 1590
//print "how many countrymen";
//input countrymen
//if countrymen < 0 then 1590
//print "how many workers";
//input foreignWorkers
//if foreignWorkers < 0 then 1590
//print "how many square miles of land";
//input landArea
//if landArea < 0 then 1590
//if landArea > 2000 then 1996
//if landArea > 1000 then 100
//print "   come on, you started with 1000 sq. miles of farm land"
//print "   and 10,000 sq. miles of forest land."
//goto 1990
//
//year = year+1
//deaths = 0
//goto 100
//end
