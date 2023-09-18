words = [["first", "start", "beginning", "onset", "initial"],
["similar", "alike", "same", "like", "resembling"],
["model", "pattern", "prototype", "standard", "criterion"],
["small", "insignificant", "little", "tiny", "minute"],
["stop", "halt", "stay", "arrest", "check", "standstill"],
["house", "dwelling", "residence", "domicile", "lodging", "habitation"],
["pit", "hole", "hollow", "well", "gulf", "chasm", "abyss"],
["push", "shove", "thrust", "prod","poke","butt", "press"],
["red", "rouge", "scarlet", "crimson", "flame", "ruby"],
["pain", "suffering", "hurt", "misery", "distress", "ache", "discomfort"]]

words.shuffle

responses = ["Right","Correct","Fine","Good!","Check"]

print " " * 33 + "SYNONYM"
print " " * 15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "A synonym of a word means another word in the English"
print "language which has the same or very nearly the same meaning."
print "I choose a word -- you type a synonym."
print "If you can't think a synonym, type the word 'HELP'"
print "and I will tell you a synonym."
print

for synonyms in words
	word = synonyms[0]
	synonyms = synonyms[1:]
	responses.shuffle
	
	print
	while 1
		guess = input("    What is a synonym of " + word + "? ").lower
		if guess == "help" then
			synonyms.shuffle
			print "**** A synonym of " + word + " is " + synonyms[0] + "."
			print
		else if guess == word or synonyms.indexOf(guess) == null then
			print "    Try again."
		else
			print responses[0]
			break
		end if
	end while
end for
print
print "Synonym drill completed."
