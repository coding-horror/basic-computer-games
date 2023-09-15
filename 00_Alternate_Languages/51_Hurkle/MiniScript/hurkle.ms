print " "*33 + "Hurcle"
print " "*15 + "Creative Computing  Morristown, New Jersey"
print; print; print
n = 5	// number of guesses allowed
g = 10	// grid size
print
print "A hurkle is hiding on a " + g + " by " + g + " grid. Homebase"
print "on the grid is point 0,0 in the southwest corner,"
print "and any point on the grid is designated by a"
print "pair of whole numbers seperated by a comma. The first"
print "number is the horizontal position and the second number"
print "is the vertical position. You must try to"
print "guess the hurkle's gridpoint. You get " + n + "tries."
print "After each try, I will tell you the approximate"
print "direction to go to look for the hurkle."
print

playOneGame = function
    a = floor(g*rnd)
    b = floor(g*rnd)
    for k in range(1, n)
        s = input("Guess #" + k + "? ").replace(",", " ").split
        x = s[0].val
        y = s[-1].val
        if x == a and y == b then
            print 
            print "You found him in " + k + " guesses!"
            return
        end if
        if y == b then
            if x == a then
                print
                print "You found him in " + k + " guesses!"
                return
            else if x < a then
                dir = "east"
            else
                dir = "west"
            end if
        else if y < b then
            dir = "north"
        else
            dir = "south"
        end if
        print "Go " + dir
    end for
    print "Sorry, that's " + n + " guesses."
    print "The hurkle is at " + a + "," + b
end function

while true
    playOneGame
    print
    print "Let's play again, hurkle is hiding."
    print
end while
