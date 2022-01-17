class CRAPSGAME

    # class variables start with a double "@"
    @@standings = 0

    def displayHeading
        puts "CRAPS".center(80)
        puts "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".center(80)
        puts "\n\n\n"
        puts "2,3,12 are losers"
        puts "4,5,6,8,9,10 are points"
        puts "7,11 are natural winners.\n\n"
    end

    def displayStanding
        if @@standings < 0
            print "you are in the hole by "
        elsif @@standings == 0
            print "you currently have "
        else
            # show how much money we currently have
            print "you now have won "
        end
           # print the absolute value of the amount in the standings
        puts @@standings.abs.to_s + " dollars"
    end

    # dice can come up 2 through 12
    # so return a minimum of 2 and add 0 through 10 to that
    def rollDice
        puts "I will now throw the dice"
        return rand(5) + rand(5) + 2
    end

    def placeBet
        print "How much do you want to wager? "
        wager = gets.strip.to_i
        return wager
    end

    def loseBetBy amount
        @@standings -= amount
    end

    def winBetBy amount
        @@standings += amount
    end

    def askQuit?
        print "\nDo you want to play again? "
        # just the first character, make it uppercase
        again = gets.strip.upcase[0]
        return again != "Y"
    end

    def pointRoll point, wager
        while true do
            puts " is the point."
            puts " I will roll again when you press Enter."
            waitForIt = gets
            roll = rollDice
            print roll.to_s

            # the only critical rolls here are 7 and the previous roll
            # if anything else comes up we roll again.
            case roll.to_i
                when 7
                    puts " craps - you lose"
                    loseBetBy wager
                    break
                when point
                    puts " is a winner! congrats!"
                    puts "at 2 to 1 odds pays you " + (2 * wager).to_s + " dollars"
                    winBetBy 2 * wager
                    break
                else
                    print " no point - " + point.to_s
           end
        end
    end

    def play
        displayHeading
        
        while true do
            wagerAmount = placeBet
            roll = rollDice
            print roll.to_s
            case roll
                when 2
                    puts " snake eyes - you lose"
                    loseBetBy wagerAmount
                when 3, 12
                    puts " craps - you lose"
                    loseBetBy wagerAmount
                when 4, 5, 6, 8, 9, 10
                    pointRoll roll, wagerAmount
                when 7, 11
                    puts " a natural - a winner"
                    puts "pays even money: " + wagerAmount.to_s + " dollars"
                    winBetBy wagerAmount
            end
            displayStanding
            if askQuit?
                endPlay
            end
        end
    end

    def endPlay
        case
            when @@standings < 0
                puts "Too bad. You are in the hole " + @@standings.abs.to_s + " dollars. Come again."
            when @@standings > 0
                puts "Congratulations --- You came out a winner of " + @@standings.to_s + " dollars. Come again!"
            when @@standings == 0
                puts "Congratulations --- You came out even, not bad for an amateur"
        end
        exit
    end
end

craps = CRAPSGAME.new
craps.play

