print " "*24 + "Literature Quiz"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

r=0
print "Test your knowledge of children's literature."
print; print "This is a multiple-choice quiz."
print "Type a 1, 2, 3, or 4 after the question mark."
print; print "Good luck!"; 

print; print
print "In pinocchio, what was the name of the cat?"
print "1)Tigger, 2)Cicero, 3)Figaro, 4)Fuipetto";
a = input("?").val
if a!=3 then
	print "Sorry...Figaro was his name."
else
	print "Very good!  Here's another."
	r += 1
end if

print; print
print "From whose garden did Bugs Bunny steal the carrots?"
print "1)Mr. Nixon's, 2)Elmer Fudd's, 3)Clem Judd's, 4)Stromboli's";
a = input("?").val
if a != 2 then
	print "Too bad...it was elmer fudd's garden."
else
	print "Pretty good!"
	r += 1
end if

print; print
print "In the Wizard of Oz, Dorothy's dog was named"
print "1)Cicero, 2)Trixia, 3)King, 4)Toto";
a = input("?").val
if a != 4 then
	print "Back to the books,...Toto was his name."
else
	print "Yea!  You're a real literature giant."
	r += 1
end if

print;print
print "Who was the fair maiden who ate the poison apple?"
print "1)Sleeping Beauty, 2)Cinderella, 3)Snow White, 4)Wendy";
a = input("?").val
if a != 3 then
	print "Oh, come on now...it was Snow White."
else
	print "Good memory!"
	r += 1
end if

print;print
if r == 4 then
	print "Wow!  That's super!  You really know your nursery"
	print "Your next quiz will be on 2nd century Chinese"
	print "literature (ha, ha, ha)"
else if r<2 then
	print "Ugh.  That was definitely not too swift.  Back to"
	print "nursery school for you, my friend."
else
	print "Not bad, but you might spend a little more time"
	print "reading the nursery greats."
end if
