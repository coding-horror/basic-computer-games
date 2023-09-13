print " "*31 + "Math Dice"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
print "This program generates successive pictures of two dice."
print "When two dice and an equal sign followed by a question"
print "mark have been printed, type your answer and the return key."
print "To conclude the lesson, type Control-C as your answer."
print

printDie = function(d)
    print
    print " -----"
    if d == 1 then
        print "|     |"
    else if d == 2 or d == 3 then
        print "| *   |"
    else
        print "| * * |"
    end if
    if d == 2 or d == 4 then
        print "|     |"
    else if d == 6 then
        print "| * * |"
    else
        print "|  *  |"
    end if
    if d == 1 then
        print "|     |"
    else if d == 2 or d == 3 then
        print "|   * |"
    else
        print "| * * |"
    end if
    print " -----"
    print
end function

while true
    d1 = floor(6 * rnd + 1)
    printDie d1
    print "   +"
    d2 = floor(6 * rnd + 1)
    printDie d2
    total = d1 + d2
    answer = input("      =? ").val
    if answer != total then
        print "No, count the spots and give another answer."
        answer = input("      =? ").val
    end if
    if answer == total then
        print "Right!"
    else
        print "No, the answer is " + total
    end if
    print
    print "The dice roll again..."
end while

