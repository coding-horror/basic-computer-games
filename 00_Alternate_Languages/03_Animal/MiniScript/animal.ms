print " "*32 + "Animal"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "Play 'Guess the Animal'"
print
print "Think of an animal and the computer will try to guess it."
print

// Ask a yes/no question, and return "Y" or "N".
getYesNo = function(prompt)
	while true
		inp = input(prompt + "? ").upper
		if inp and (inp[0] == "Y" or inp[0] == "N") then return inp[0]
		print "Please answer Yes or No."
	end while
end function	

// Our data is stored as a list of little maps.
// Answers have only an "answer" key.
// Questions have a "question" key, plus "ifYes" and "ifNo"
//	keys which map to the index of the next question or answer.
data = [
	{"question":"Does it swim", "ifYes":1, "ifNo":2},
	{"answer":"fish"},
	{"answer":"bird"}]

// List all known animals.
listKnown = function
	print; print "Animals I already know are:"
	for item in data
		if item.hasIndex("answer") then print (item.answer + " "*17)[:17], ""
	end for
	print; print
end function

// Ask the question at curIndex, and handle the user's response.
doQuestion = function
	q = data[curIndex]
	if getYesNo(q.question) == "Y" then
		globals.curIndex = q.ifYes
	else
		globals.curIndex = q.ifNo
	end if
end function

// Check the answer at curIndex.  If incorrect, get a new question
// to put at that point in our data.
checkAnswer = function
	node = data[curIndex]
	inp = getYesNo("Is it a " + node.answer)
	if inp == "Y" then 
		print "Why not try another animal?"
	else
		actual = input("The animal you were thinking of was a? ").lower
		print "Please type in a question that would distinguish a"
		print actual + " from a " + node.answer
		q = {}
		q.question = input
		q.question = q.question[0].upper + q.question[1:] - "?"
		data[curIndex] = q
		k = data.len
		data.push node               // old answer at index k
		data.push {"answer":actual}  // new answer at index k+1
		if getYesNo("For a " + actual + " the answer would be") == "Y" then
			data[curIndex].ifYes = k+1
			data[curIndex].ifNo = k
		else
			data[curIndex].ifNo = k+1
			data[curIndex].ifYes = k
		end if
	end if
end function

// Main loop.  (Press Control-C to break.)
while true
	while true
		inp = input("Are you thinking of an animal? ").upper
		if inp == "LIST" then listKnown
		if inp and inp[0] == "Y" then break
	end while
	curIndex = 0
	while true
		if data[curIndex].hasIndex("question") then
			doQuestion
		else
			checkAnswer
			break
		end if
	end while
end while
