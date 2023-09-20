print " "*34 + "Name"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "Hello."; print "My name is Creative Computer."
name = input("What's your name (first and last)? ")

s = ""
for i in range(name.len - 1)
	s += name[i]
end for
print; print "Thank you, " + s + "."
print "Oops!  I guess I got it backwards.  A smart"
print "computer like me shouldn't make a mistake like that!"; print
print "But i just noticed your letters are out of order."
s = name.split("").sort.join("")
print "Let's put them in order like this: " + s
yn = input("Don't you like that better? ").lower
print
if yn and yn[0] == "y" then
	print "I knew you'd agree!!"
else
	print "I'm sorry you don't like it that way."
end if
print; print "I really enjoyed meeting you " + name + "."
print "Have a nice day!"
