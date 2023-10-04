print " "*30 + "POETRY"
print " "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
print; print; print

I = 0; J = 0; K = 0; U = 0

// Note: infinite loop.  Press control-C to break.
while true
	if J == 1 then
		print ["MIDNIGHT DREARY", "FIERY EYES", 
		 "BIRD OR FIEND", "THING OF EVIL", "PROPHET"][I-1], ""
	else if J == 2 then
		if I == 1 or I == 4 then U = 2
		if I == 3 then U = 0
		print ["BEGUILING ME", "THRILLED ME", 
		 "STILL SITTING....", "NEVER FLITTING", "BURNED"][I-1], ""
	else if J == 3 then
		if U != 0 or I < 5 then
			print ["AND MY SOUL", "DARKNESS THERE", 
			 "SHALL BE LIFTED", "QUOTH THE RAVEN", "SIGN OF PARTING"][I-1], ""
		end if
	else if J == 4 then
		print ["NOTHING MORE", "YET AGAIN", 
		  "SLOWLY CREEPING", "...EVERMORE", "NEVERMORE"][I-1], ""
	end if
	if U != 0 and rnd <= 0.19 then
		print ",", ""
		U = 2
	end if
	if rnd <= 0.65 then
		print " ", ""
		U += 1
	else
		print
		U = 0
	end if
	while true
		I =floor(floor(10*rnd)/2)+1
		J += 1
		K += 1
		if U == 0 and floor(J/2) == J/2 then print "     ", ""
		if J <= 5 then break
		J = 0
		print
		if K <= 20 then continue
		print
		U = 0
		K = 0
		break
	end while
end while
