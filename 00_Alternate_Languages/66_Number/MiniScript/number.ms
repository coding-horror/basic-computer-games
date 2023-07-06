// Number Game
// originally by Tom Adametx
// Ported from BASIC to MiniScript by Joe Strout, 2023

print " "*33 + "NUMBER"
print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print; print; print

print "You have 100 points.  By guessing numbers from 1 to 5, you"
print "can gain or lose points depending on how close you get to"
print "a random number selected by the computer."; print
print "You occasionally will get a jackpot which will double(!)"
print "your point count.  You win when you get to 500 points."
print

P = 100
fnr = function; return ceil(5*rnd); end function
while true
	guess = input("Guess a number from 1 to 5: ").val
	R = fnr
	S = fnr
	T = fnr
	U = fnr
	V = fnr
	if guess == R then
		P = P - 5
	else if guess == S then
		P = P + 5
	else if guess == T then
		P = P+P
		print "You hit the jackpot!!!"
	else if guess == U then
		P = P + 1
	else if guess == V then
		P = P - floor(P*0.5)
	else if guess > 5 then
		continue
	end if
	if P > 500 then
		print "!!!!You win!!!! with " + P + " points."
		break
	end if
	print "You have " + P + " points."; print
end while
