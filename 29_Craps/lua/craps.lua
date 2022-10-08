--[[
Craps

From: BASIC Computer Games (1978)
Edited by David H. Ahl

    This game simulates the games of craps played according to standard
    Nevada craps table rules. That is:
    1. A 7 or 11 on the first roll wins
    2. A 2, 3, or 12 on the first roll loses
    3. Any other number rolled becomes your "point." You continue to roll;
    if you get your point you win. If you roll a 7, you lose and the dice
    change hands when this happens.

    This version of craps was modified by Steve North of Creative Computing.
    It is based on an original which appeared one day one a computer at DEC.


Lua port by Alex Conconi, 2022
--]]


--- Throw two dice and return their sum.
local function throw_dice()
    return math.random(1, 6) + math.random(1, 6)
end


--- Print prompt and read a number > 0 from stdin.
local function input_number(prompt)
    while true do
        io.write(prompt)
        local number = tonumber(io.stdin:read("*l"))
        if number and number > 0 then
            return number
        else
            print("Please enter a number greater than zero.")
        end
    end
end


--- Print custom balance message depending on balance value
local function print_balance(balance, under_msg, ahead_msg, even_msg)
    if balance < 0 then
        print(under_msg)
    elseif balance > 0 then
        print(ahead_msg)
    else
        print(even_msg)
    end
end


--- Play a round and return winnings or losings.
local function play_round()
    -- Input the wager
    local wager = input_number("Input the amount of your wager: ")

    -- Roll the die for the first time.
    print("I will now throw the dice")
    local first_roll = throw_dice()

    -- A 7 or 11 on the first roll wins.
    if first_roll == 7 or first_roll == 11 then
        print(string.format("%d - natural.... a winner!!!!", first_roll))
        print(string.format("%d pays even money, you win %d dollars", first_roll, wager))
        return wager
    end

    -- A 2, 3, or 12 on the first roll loses.
    if first_roll == 2 or first_roll == 3 or first_roll == 12 then
        if first_roll == 2 then
            -- Special 'you lose' message for 'snake eyes'
            print(string.format("%d - snake eyes.... you lose.", first_roll))
        else
            -- Default 'you lose' message
            print(string.format("%d - craps.... you lose.", first_roll))
        end
        print(string.format("You lose %d dollars", wager))
        return -wager
    end

    -- Any other number rolled becomes the "point".
    -- Continue to roll until rolling a 7 or point.
    print(string.format("%d is the point. I will roll again", first_roll))
    while true do
        local second_roll = throw_dice()
        if second_roll == first_roll then
            -- Player gets point and wins
            print(string.format("%d - a winner.........congrats!!!!!!!!", first_roll))
            print(string.format("%d at 2 to 1 odds pays you...let me see... %d dollars", first_roll, 2 * wager))
            return 2 * wager
        end
        if second_roll == 7 then
            -- Player rolls a 7 and loses
            print(string.format("%d - craps. You lose.", second_roll))
            print(string.format("You lose $ %d", wager))
            return -wager
        end
        --  Continue to roll
        print(string.format("%d  - no point. I will roll again", second_roll))
    end
end


--- Main game function.
local function craps_main()
    -- Print the introduction to the game
    print(string.rep(" ", 32) .. "Craps")
    print(string.rep(" ", 14) .. "Creative Computing  Morristown, New Jersey\n\n")
    print("2,3,12 are losers; 4,5,6,8,9,10 are points; 7,11 are natural winners.")

    -- Initialize random number generator seed
    math.randomseed(os.time())

    -- Initialize balance to track winnings and losings
    local balance = 0

    -- Main game loop
    local keep_playing = true
    while keep_playing do
        -- Play a round
        balance = balance + play_round()

        -- If player's answer is 5, then stop playing
        keep_playing = (input_number("If you want to play again print 5 if not print 2: ") == 5)

        -- Print an update on winnings or losings
        print_balance(
            balance,
            string.format("You are now under $%d", -balance),
            string.format("You are now ahead $%d", balance),
            "You are now even at 0"
        )
    end

    -- Game over, print the goodbye message
    print_balance(
        balance,
        "Too bad, you are in the hole. Come again.",
        "Congratulations---you came out a winner. Come again.",
        "Congratulations---you came out even, not bad for an amateur"
    )
end


--- Run the game.
craps_main()
