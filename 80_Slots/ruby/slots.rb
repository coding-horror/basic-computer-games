$pot = 0

def greeting
    puts "SLOTS".center(80)
    puts "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".center(80)
    puts "\n\n"

    # PRODUCED BY FRED MIRABELLE AND BOB HARPER ON JAN 29, 1973
    # IT SIMULATES THE SLOT MACHINE.

    puts "You are in the H&M Casino, in front of one of our"
    puts "one-arm bandits. You can bet from $1 to $100."
    puts "To pull the arm, punch the return key after making your bet."
    puts "\nBet zero to end the game."
end

def overLimit
    puts "House Limit is $100"
end

def underMinimum
    puts "Minimum Bet is $1"
end

# bells don't work on my machine. YMMV
# I'm displaying dashes between the reels

def tenBells
    10.times do
        # beep if you can
        print "-"
    end
end

def fiveBells
    "-----"
end

def goodbye
    puts "\n\n\n"
    # end the game
    exit
end

def payUp
    puts "PAY UP!  PLEASE LEAVE YOUR MONEY ON THE TERMINAL."
end

def brokeEven
    puts "HEY, YOU BROKE EVEN."
end

def collectWinnings
    puts "COLLECT YOUR WINNINGS FROM THE H&M CASHIER."
end

def win winType, bet
    case winType
        when "jackpot"
            winMessage = "***JACKPOT***"
            winnings = 101
        when "topDollar"
            winMessage = "**TOP DOLLAR**"
            winnings = 11
        when "doubleBar"
            winMessage = "*DOUBLE BAR!!*"
            winnings = 6
        when "double"
            winMessage = "DOUBLE!!"
            winnings = 3
    end
    puts "\nYou won: " + winMessage
    $pot += (winnings * bet)
end

greeting

#$pot = 0
while true
    reelArray = ["BAR","BELL","ORANGE","LEMON","PLUM","CHERRY"]
    print "\nYOUR BET? "
    # get input, remove leading and trailing whitespace, cast to integer
    bet = gets.strip.to_i

    if bet == 0 then
        goodbye
    elsif bet > 100 then
        overLimit # error if more than $100
    elsif bet < 1 then
        underMinimum # have to bet at least a dollar
    else
        # valid bet, continue
        tenBells # ding

        # assign a random value from the array to each of the three reels
        reel1 = reelArray[rand(5)]
        reel2 = reelArray[rand(5)]
        reel3 = reelArray[rand(5)]

        # print the slot machine reels
        puts "\n\n" + reel1 + fiveBells + reel2 + fiveBells + reel3

        # see if we have a match in the first two reels
        if reel1 == reel2 then
            if reel2 == reel3 then
                if reel3 == "BAR" then
                    # all three reels are "BAR"
                    win "jackpot", bet
                 else
                   # all three reels match but aren't "BAR"
                   win "topDollar", bet
                end
            elsif reel2 == "BAR" then
                # reels 1 and 2 are both "BAR"
                win "doubleBar", bet
             else
                # reels 1 and 2 match but aren't "BAR"
                win "double", bet
            end
        # otherwise see if there's a match in the remaining reels
        elsif reel1 == reel3 or reel2 == reel3 then
            if reel3 == "BAR" then
                # two reels match, both "BAR"
                win "doubleBar", bet
            else
                # two reels match, but not "BAR"
                win "double", bet
            end
        else
            # bad news - no matches
            puts "\nYou lost"
            # decrement your standings by the bet amount
            $pot -= bet
        end

        puts "Your standings are: " + $pot.to_s
        print "\nAgain? " # YES to continue
        # get input, remove leading and trailing whitespace, make uppercase
        again = gets.strip.upcase
        if again != "Y" && again != "YES" then
            # that's enough... evaluate the pot and quit
            if $pot < 0 then
                payUp
            elsif $pot == 0 then
                brokeEven
            else # yay!
                collectWinnings
            end
            goodbye
        end
    end
end
