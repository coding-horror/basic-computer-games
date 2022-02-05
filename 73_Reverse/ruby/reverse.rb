ARRAYSIZE = 9
$digitArray = Array.new(ARRAYSIZE)
$winningArray = Array.new(ARRAYSIZE)

# Method to print the rules
def displayTheRules
    puts "This is the game of 'Reverse'.  to win, all you have"
    puts "to do is arrange a list of numbers (1 through " + ARRAYSIZE.to_s + ")"
    puts "in numerical order from left to right.  to move, you"
    puts "tell me how many numbers (counting from the left) to"
    puts "reverse.  For example, if the current list is:"
    puts "2 3 4 5 1 6 7 8 9"
    puts "and you reverse 4, the result will be:"
    puts "5 4 3 2 1 6 7 8 9"
    puts "Now if you reverse 5, you win!"
    puts "1 2 3 4 5 6 7 8 9"
    puts "No doubt you will like this game, but"
    puts "if you want to quit, reverse 0 (zero)."
end

# Method to print the list
def printList
    puts "\n" + $digitArray.join(" ") + "\n\n"
end

# Zero-based arrays contain digits 1-9
# Make a random array and an ordered winning answer array A[0] to A[N]
def makeRandomList
    for kIndex in 0..ARRAYSIZE-1 do
        $digitArray[kIndex] = kIndex+1
        $winningArray[kIndex] = kIndex+1
    end
    # now randomize the digit array order
    $digitArray.shuffle!
end

def checkForWin? (triesSoFar)
    # Check for a win (all array elements in order)
    if $digitArray == $winningArray then
        puts "You won it in " + triesSoFar.to_s + " moves!!!\n\n"
        puts "try again (yes or no)?"
        tryAgain = gets.strip.upcase
        if tryAgain == "YES" then
            return true
        end
        puts "\nO.K. Hope you had fun!!"
        exit
    end
    return false
end

def reverseIt (howManyToReverse, triesSoFar)
    # REVERSE R NUMBERS AND PRINT NEW LIST

    # extract and reverse the first howManyToReverse elements of the array
    subArray = $digitArray.take(howManyToReverse)
    subArray.reverse!
    
    # get the remaining elements of the original array
    endArray = $digitArray.slice(howManyToReverse, ARRAYSIZE)
    # append those elements to the reversed elements
    $digitArray = subArray.concat(endArray)

    # if we got all in order, randomize again
    isWinner = checkForWin?(triesSoFar)
    if isWinner == true then
        makeRandomList
    end
    printList # always print the newly ordered list
    return isWinner
end

def askHowManyToReverse
    puts "How many shall I reverse?";
    rNumber = gets.to_i
    if rNumber > 0 then
        if rNumber > ARRAYSIZE then
            puts "Oops! Too many! I can reverse at most " + ARRAYSIZE.to_s
        end
    else
        rNumber = 0 # zero or negative values end the game
    end
    return rNumber
end

puts "REVERSE"
puts "Creative Computing  Morristown, New Jersey\n\n\n"
puts "REVERSE -- A game of skill\n\n"

puts "Do you want the rules?"
wantRules = gets.strip.upcase
if wantRules == "YES" then
    displayTheRules
end

makeRandomList
howManyTries = 0
puts "\nHere we go ... the list is:"
printList # display the initial list
# start the game loop
r = askHowManyToReverse
while r != 0 do # zero will end the game
    if r <= ARRAYSIZE then
        howManyTries = howManyTries+1
        if reverseIt(r, howManyTries) then
            howManyTries = 0
        end
    end
    r = askHowManyToReverse
end

