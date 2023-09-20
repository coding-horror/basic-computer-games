print " "*32 + "Bullseye"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

print "In this game, up to 20 players throw darts at a target"
print "with 10, 20, 30, and 40 point zones.  the objective is"
print "to get 200 points."; print
print "throw       description            probable score"
print " 1          fast overarm           bullseye or complete miss"
print " 2          controlled overarm     10, 20 or 30 points"
print " 3          underarm               anything";print
names = []
n = input("How many players? ").val; print
for i in range(0, n-1)
	names.push input("Name of player #" + (i+1) + "? ")
end for
scores = [0] * n

round = 0
while true
	round += 1; print; print "round " + round; print "---------"
	for i in range(0, n-1)
		while true
			print; t = input(names[i] + "'s throw? ").val
			if 1 <= t <= 3 then break
			print "Input 1, 2, or 3!"
		end while
		if t == 1 then
			p1=.65; p2=.55; p3=.5; p4=.5
		else if t == 2 then
			p1=.99; p2=.77; p3=.43; p4=.01
		else
			p1=.95; p2=.75; p3=.45; p4=.05
		end if
		u = rnd
		if u>=p1 then
			print "Bullseye!!  40 points!"; b=40
		else if u>=p2 then
			print "30-point zone!"; b=30
		else if u>=p3 then
			print "20-point zone"; b=20
		else if u>=p4 then
			print "Whew!  10 points."; b=10
		else
			print "Missed the target!  too bad."; b=0
		end if
		scores[i] += b; print "Total score = " + scores[i]
	end for
	winners = []
	for i in range(0, n-1)
		if scores[i] >= 200 then winners.push i
	end for
	if winners then break
end while

print; print "We have a winner!!"; print
for i in winners; print names[i] + " scored " + scores[i] + " points."; end for
print; print "Thanks for the game."

