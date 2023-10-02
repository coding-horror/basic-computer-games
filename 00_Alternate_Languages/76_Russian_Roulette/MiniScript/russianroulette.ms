print " "*28 + "Russian Roulette"
print " "*15 + "Creative Computing  Morristown  New Jersey"
print; print; print

print "This is a game of >>>>>>>>>>Russian Roulette."

while true
	print; print "Here is a revolver."
	print "Type '1' to spin chamber and pull trigger."
	print "Type '2' to give up."
	print "GO"

	n = 0
	while n < 10
		inp = input("? ").val
		if inp == 2 then
			print "     CHICKEN!!!!!"
			break
		else if rnd > 0.833333 then
			print "     BANG!!!!!   You're dead!"
			print "Condolences will be sent to your relatives."
			break
		else
			n += 1
			print "- CLICK -"
			print
		end if
	end while
	
	if n >= 10 then
		print "You win!!!!!"
		print "Let someone else blow his brains out."
	else
		print; print; print
		print "...Next victim..."
	end if
end while
