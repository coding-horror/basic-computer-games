--[[
Dice

From: BASIC Computer Games (1978)
Edited by David H. Ahl

    "Not exactly a game, this program simulates rolling
    a pair of dice a large number of times and prints out
    the frequency distribution.  You simply input the
    number of rolls.  It is interesting to see how many
    rolls are necessary to approach the theoretical
    distribution:

    2  1/36  2.7777...%
    3  2/36  5.5555...%
    4  3/36  8.3333...%
    etc.

    "Daniel Freidus wrote this program while in the
     seventh grade at Harrison Jr-Sr High School,
    Harrison, New York."


Lua port by Alex Conconi, 2022.
]]--


local function print_intro()
    print("\n                   Dice")
    print("Creative Computing  Morristown, New Jersey")
    print("\n\n")
    print("This program simulates the rolling of a")
    print("pair of dice.")
    print("You enter the number of times you want the computer to")
    print("'roll' the dice.   Watch out, very large numbers take")
    print("a long time.  In particular, numbers over 5000.")
end


local function ask_how_many_rolls()
    while true do
        -- Print prompt and read a valid number from stdin
        print("\nHow many rolls?")
        local num_rolls = tonumber(io.stdin:read("*l"))
        if num_rolls then
            return num_rolls
        else
            print("Please enter a valid number.")
        end
    end
end


local function ask_try_again()
    while true do
        -- Print prompt and read a yes/no answer from stdin,
        -- accepting only 'yes', 'y', 'no' or 'n' (case insensitive)
        print("\nTry again? ([y]es / [n]o)")
        local answer = string.lower(io.stdin:read("*l"))
        if answer == "yes" or  answer == "y" then
            return true
        elseif answer == "no" or answer == "n" then
            return false
        else
            print("Please answer '[y]es' or '[n]o'.")
        end
    end
end


local function roll_dice(num_rolls)
    -- Initialize a table to track counts of roll outcomes
    local counts = {}
    for i=2, 12 do
        counts[i] = 0
    end

    -- Roll the dice num_rolls times and update outcomes counts accordingly
    for _=1, num_rolls do
        local roll_total = math.random(1, 6) + math.random(1, 6)
        counts[roll_total] = counts[roll_total] + 1
    end

    return counts
end


function print_results(counts)
    print("\nTotal Spots   Number of Times")
    for roll_total, count in pairs(counts) do
        print(string.format(" %-14d%d", roll_total, count))
    end
end


local function dice_main()
    print_intro()

    -- initialize the random number generator
    math.randomseed(os.time())

    -- main game loop
    local keep_playing = true
    while keep_playing do
        local num_rolls = ask_how_many_rolls()
        local counts = roll_dice(num_rolls)
        print_results(counts)
        keep_playing = ask_try_again()
    end
end


dice_main()