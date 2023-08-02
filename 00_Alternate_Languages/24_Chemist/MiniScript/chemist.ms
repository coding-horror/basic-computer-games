print " "*33 + "Chemist"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "The fictitious chemical kryptocyanic acid can only be"
print "diluted by the ratio of 7 parts water to 3 parts acid."
print "If any other ratio is attempted, the acid becomes unstable"
print "and soon explodes.  Given the amount of acid, you must"
print "decide how much water to add for dilution.  If you miss"
print "you face the consequences."

deaths = 0
while deaths < 9
	acid = floor(rnd * 50)
	water = 7 * acid/3
	response = input(acid +" liters of kryptocyanic acid.  How much water? ").val
	diff = abs(water - response)
	if diff > water/20 then
		print " SIZZLE!  You have just been desalinated into a blob"
		print " of quivering protoplasm!"
		deaths += 1
		if deaths < 9 then
			print " However, you may try again with another life."
		end if
	else
		print " Good job! You may breathe now, but don't inhale the fumes!"
		print
	end if
end while
print " Your 9 lives are used, but you will be long remembered for"
print " your contributions to the field of comic book chemistry."
