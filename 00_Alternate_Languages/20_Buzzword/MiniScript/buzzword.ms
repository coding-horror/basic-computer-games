print " "*26 + "Buzzword Generator"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print

print "This program prints highly acceptable phrases in"
print "'educator-speak' that you can work into reports"
print "and speeches.  Whenever a question mark is printed,"
print "type a 'y' for another phrase or 'n' to quit."

words1 = ["ability","basal","behavioral","child-centered",
	"differentiated","discovery","flexible","heterogeneous",
	"homogeneous","manipulative","modular","tavistock",
	"individualized"]

words2 = ["learning", "evaluative","objective",
	"cognitive","enrichment","scheduling","humanistic",
	"integrated","non-graded","training","vertical age",
	"motivational","creative"]

words3 = ["grouping","modification",
	"accountability","process","core curriculum","algorithm",
	"performance","reinforcement","open classroom","resource",
	"structure","facility","environment"]

list.any = function
	return self[self.len * rnd]
end function

print; print; print "Here's the first phrase:"

while true
	print [words1.any, words2.any, words3.any].join
	print
	yn = input("?").lower
	if yn != "y" then break
end while

print "Come back when you need help with another report!"
