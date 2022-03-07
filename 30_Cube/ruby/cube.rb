$landmines = Array.new

$currentLocation = "111"

$standings = 500 # starting amount

def getYesOrNoResponseTo prompt
    print prompt
    # strip leading and trailing whitespace from entry
    yesno = gets.strip.upcase[0]
    yesno == "Y"
end

def greeting
    puts "CUBE".center(80)
    puts "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".center(80)
    puts "\n\n\n"
    response = getYesOrNoResponseTo "Do you want to see the INSTRUCTIONS?"
    if response
        puts "This is a game in which you will be playing against the"
        puts "random decision of the computer. The field of play is a"
        puts "cube of size 3. Any of your 27 locations can be designated"
        puts "by inputting three numbers such as 231."
        puts "At the start you are automatically at location 1,1,1.\n"
        puts "The object of the game is to get to location 3,3,3.\n"

        puts "\nONE MINOR DETAIL:"
        puts "The computer will pick five random locations at which it will"
        puts "plant land mines. if you hit one of these locations you lose.\n"

        puts "\nONE OTHER DETAIL:"
        puts "You may move only one space in one direction each move."
        puts "For example: from 1,1,2 you may move to 2,1,2 or 1,1,3."
        puts "You may not change more than one number on the same move."
        puts "If you make an illegal move, you lose and the computer takes"
        puts "the money you may have bet on that round."
        puts ""
        puts "When stating the amount of a wager, enter only the number"
        puts "of dollars (example: 250)  You are automatically started with"
        puts "500 dollars in your account."
        puts
        puts "Good luck!"
    end
end

def landMindStringFrom x, y, z
    landMine = x.to_s + y.to_s + z.to_s
    return landMine
end

def assignLandMines
    $landmines.clear

    # put five unique entries into the landmines array
    while $landmines.size < 5 do
        a = rand(3)+1
        b = rand(3)+1
        c = rand(3)+1
        landmine = landMindStringFrom(a, b, c)
        if !$landmines.include?(landmine) && landmine != "333"
# puts landmine     # debugging
            $landmines.push landmine
        end
    end
    $currentLocation = "111"
end

def initializePot
    $standings = 500 # starting amount
end

def startGame
    assignLandMines
    displayStandings
    response = getYesOrNoResponseTo "WANT TO MAKE A WAGER? "
    if response
        print "HOW MUCH? "
        while true do
        wager = gets.strip.tr('^0-9', '').to_i
            if $standings < wager
                puts "TRIED TO FOOL ME; BET AGAIN ";
            else
                break
            end
        end
    else
        wager = 0
    end

    # start at location 1,1,1
    $currentLocation = "111"
    return wager
end

def goodbye
    puts "TOUGH LUCK!"
    puts ""
    puts "GOODBYE."
    exit
end

def bust
    puts "YOU BUST."
    goodbye
end

def tryAgain
    again = getYesOrNoResponseTo "WANT TO TRY AGAIN? "
    if not again
        exit
    end
end

def isLegalMove? newLocation
    # test for illegal moves
    # can only change one variable per move
    # newLocation is the proposed new position
    # can only move one space from the current position

    moveX = newLocation[0].to_i
    moveY = newLocation[1].to_i
    moveZ = newLocation[2].to_i

    # currentX, currentY, currentZ contains the current position
    currentX = $currentLocation[0].to_i
    currentY = $currentLocation[1].to_i
    currentZ = $currentLocation[2].to_i

    isLegalMove = true
    errorString = ""
   # ensure we're not moving off the cube
    if not (moveX.between?(1,3) && moveY.between?(1,3) && moveZ.between?(1,3))
        errorString = "You moved off the cube!"
        return errorString
    end

    # test for only one move from current position
    if not moveX.between?(currentX-1,currentX+1)
        isLegalMove = false
    end
    if not moveY.between?(currentY-1,currentY+1)
        isLegalMove = false
    end
    if not moveZ.between?(currentZ-1,currentZ+1)
        isLegalMove = false
    end
        if not isLegalMove
            errorString = "You've gone too far"
        end

    # only allow change to one variable at a time
    if isLegalMove
        if moveX != currentX
            if moveY != currentY or moveZ != currentZ
                isLegalMove = false
            end
        end
        if moveY != currentY
            if moveX != currentX or moveZ != currentZ
                isLegalMove = false
            end
        end
        if moveZ != currentZ
            if moveY != currentY or moveX != currentX
                isLegalMove = false
            end
        end
        if not isLegalMove
            errorString = "You made too many changes"
        end
   end

    return errorString
end

def displayStandings
    print "You now have " + $standings.to_s
    if $standings > 1
        puts " dollars"
    else
        puts " dollar"
    end
end

def didWin? location
    location == "333"
end

def youWin amount
    $standings += amount
    puts "*** You win " + amount.to_s + " dollars! ***\n\n"
    displayStandings
    tryAgain
    assignLandMines
    puts "*** new cube ***"
    puts "different landmine locations"
    puts "starting over at location 111"
end

def youLose amount
    # subtract the bet amount from the standings
    if amount > 0
        puts "You lose " + amount.to_s + " dollars!\n\n"
        $standings -= amount
    end
    if $standings <= 0
        # no money left, so end the game
        bust
    else
        displayStandings
    end
    tryAgain
    $currentLocation = "111"
    puts "starting over at location 111"
end

def landMine betAmount
    puts "******BANG******"
    puts "You hit a land mine at " + $currentLocation + "!"
    youLose betAmount
end

def gameLoop betAmount
    while true do
        puts ""
        print "IT'S YOUR MOVE:  "
        # allow only integers: strip anything else from input
        moveToLocation = gets.strip.tr('^0-9', '')

        # test for illegal moves
        # can only change one variable per move
        # moveToLocation is the proposed new position

        error = isLegalMove?(moveToLocation)
        if error == ""
            # assign the new position
            $currentLocation = moveToLocation

            # test for win
            if didWin?(moveToLocation)
                youWin betAmount
            end

            # haven't won yet, test the land mines
            if $landmines.include? moveToLocation
                landMine betAmount
            end

        else
            puts "Illegal move: " + error
            youLose betAmount
        end

    end
end


greeting
initializePot
gameLoop startGame
