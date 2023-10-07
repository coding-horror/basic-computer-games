print " "*30 + "Stock Market"
print " "*15 + "Creative Computing  Morristown  New Jersey"
print; print; print

// Stock Market Simulation     -STOCK-
// Revised 8/18/70 (D. Pessel, L. Braun, C. Losik)
// Ported to MiniScript 10/07/23 (J. Strout)


// Ask a yes/no question; return true if yes, false if no
yes = function(prompt)
	while true
		response = input(prompt + " (YES-Type 1, NO-Type 0)? ")
		if not response then continue
		if response == "1" or response[0].lower == "y" then return true
		if response == "0" or response[0].lower == "n" then return false
	end while
end function

printInstructions = function
	print; print
	print "This program plays the stock market.  You will be given"
	print "$10,000 and may buy or sell stocks.  The stock prices will"
	print "be generated randomly and therefore this model does not"
	print "represent exactly what happens on the exchange.  A table"
	print "of available stocks, their prices, and the number of shares"
	print "in your portfolio will be printed.  Following this, the"
	print "initials of each stock will be printed with a question"
	print "mark.  Here you indicate a transaction.  To buy a stock"
	print "type +NNN, to sell a stock type -NNN, where NNN is the"
	print "number of shares.  A brokerage fee of 1% will be charged"
	print "on all transactions.  Note that if a stock's value drops"
	print "to zero it may rebound to a positive value again.  You"
	print "have $10,000 to invest.  Use integers for all your inputs."
	print "(Note:  To get a 'feel' for the market run for at least"
	print "10 days)"
	print "-----Good luck!-----"
	print
	input "(Press Return to continue.)"
end function

randomIndex = function; return floor(rnd * stockPrices.len); end function

// Randomly produce new stock values based on previous day's values.
// N1,N2 are random numbers of days which respectively determine
// when a stock will increase or decrease 10 points.
adjustStockPrices = function
	for i in changePerShare.indexes
		changePerShare[i] = 0
	end for
	// if N1 days have passed, increase a random stock by 10
	if N1 < 1 then
		changePerShare[randomIndex] = 10
		globals.N1 = floor(4.99 * rnd + 1)
	end if
	// if N2 days have passed, decrease a random stock by 10
	if N2 < 1 then
		changePerShare[randomIndex] = -10
		globals.N2 = floor(4.99 * rnd + 1)
	end if
	// Deduct one day from N1 and N2
	globals.N1 -= 1
	globals.N2 -= 1
	// update all stocks
	for i in stockPrices.indexes
		smallChange = rnd
		if smallChange < 0.25 then smallChange = 0.25
		if smallChange > 0.5 then smallChange = 0.5
		changePerShare[i] += marketTrendSlope * stockPrices[i] + smallChange + floor(3 - 6*rnd + 0.5)
		changePerShare[i] = round(changePerShare[i], 2)
		stockPrices[i] += changePerShare[i]
		if stockPrices[i] < 0 then
			changePerShare[i] = 0
			stockPrices[i] = 0
		end if
		stockPrices[i] = round(stockPrices[i], 2)
	end for	
	// After trendDaysLeft, randomly change trend sign and slope
	globals.trendDaysLeft -= 1
	if trendDaysLeft < 1 then
		globals.trendDaysLeft = floor(4.99 * rnd + 1)
		globals.marketTrendSlope = floor(rnd*10 + 0.5)/100
		if rnd < 0.5 then globals.marketTrendSlope = -marketTrendSlope
	end if
end function

pad = function(s, width=20)
	s = str(s)
	return s + " "*(width - s.len)
end function

printInitialPortfolio = function
	print pad("Stock", 30) + pad("Initials", 12) + "Price/Share"
	for i in stockSymbols.indexes	
		print pad(stockNames[i], 32) + pad(stockSymbols[i], 14) + stockPrices[i]
	end for
end function

printMarketResults = function
	print
	print "**********     END OF DAY'S TRADING     **********"
	print
	print
	print pad("Stock", 8) + pad("Price/Share", 14) + pad("Holdings", 12) +
	  pad("Value", 10) + "Net Price Change"
	for i in stockSymbols.indexes
		value = round(stockPrices[i] + sharesOwned[i], 2)
		print pad(stockSymbols[i], 9) + pad(stockPrices[i], 14) +
		  pad(sharesOwned[i], 12) + pad(value, 10) + changePerShare[i]
	end for
	print
end function

printStatus = function
	average = stockPrices.sum / stockPrices.len
	print
	print "New York Stock Exchange Average: " + round(average, 2), ""
	if prevAverage != null then 
		print " Net Change " + round(average - prevAverage, 2)
	else
		print
	end if
	globals.prevAverage = average
	print
	stockValue = 0
	for i in stockPrices.indexes
		stockValue += stockPrices[i] * sharesOwned[i]
	end for
	print "Total stock assets are   $ " + round(stockValue, 2)
	print "Total cash assets are    $ " + round(cash, 2)
	print "Total assets are         $ " + round(stockValue + cash, 2)
	print
	// Uncomment the following line to cheat/debug:
	//print "marketTrendSlope: " + marketTrendSlope + "; trendDaysLeft: " + trendDaysLeft
end function

// Calculate total cost, including brokerage fee, to buy the given
// stock (or if qtyToBuy < 0, negative cash gains minus the fee).
totalCost = function(stockIndex, qtyToBuy)
	baseVal = stockPrices[stockIndex] * qtyToBuy
	fee = round(abs(baseVal) * 0.01, 2)
	return baseVal + fee
end function

buySell = function
	print "What is your transaction in"
	i = 0
	while i < stockSymbols.len
		while true
			qty = input(stockSymbols[i] + "? ")
			if qty == "" or qty == "0" or qty.val != 0 then break
			print "Enter quantity to buy/sell like +10 or -2."
		end while
		qty = qty.val
		if qty < 0 and -qty > sharesOwned[i] then
				print "You have only " + sharesOwned[i] + " of " + stockSymbols[i] + "; try again."
				continue
			end if
		if totalCost(i, qty) > cash then
			print "This would cost " + (totalCost(i, qty) - cash) + " more than you have."
			continue
		end if
		sharesOwned[i] += qty
		globals.cash -= totalCost(i, qty)
		i += 1
	end while
end function

// Introduction
if yes("Do you want the instructions") then printInstructions
print; print

// Initial stock values and trends
stockSymbols = ["IBM", "RCA", "LBJ", "ABC", "CBS"]
stockNames = ["Int. Ballistic Missiles", "Red Cross of America", 
  "Lichtenstein, Bumrap & Joke", "American Bankrupt Co.", "Censured Books Store"]
sharesOwned = [0] * stockSymbols.len
cash = 10000
stockPrices = [100, 85, 150, 140, 110]
changePerShare = [0] * stockSymbols.len
trendDaysLeft = floor(4.99 * rnd + 1)
marketTrendSlope = floor(rnd*10 + 0.5)/100
if rnd > 0.5 then marketTrendSlope = -marketTrendSlope
N1 = 0		// days until a big positive jump in a random price
N2 = 0		// days until a big negative jump in a random price
adjustStockPrices
prevAverage = null

printInitialPortfolio
printStatus
while true
	buySell
	adjustStockPrices
	printMarketResults
	printStatus
	if not yes("Do you wish to continue") then break
end while
print "Hope you had fun!!"

