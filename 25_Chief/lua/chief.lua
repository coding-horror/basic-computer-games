--[[
Chief

From: BASIC Computer Games (1978)
Edited by David H. Ahl

    In the words of the program author, John Graham, “CHIEF is designed to
    give people (mostly kids) practice in the four operations (addition,
    multiplication, subtraction, and division).

    It does this while giving people some fun. And then, if the people are
    wrong, it shows them how they should have done it.

    CHIEF was written by John Graham of Upper Brookville, New York.

    
Lua port by Alex Conconi, 2022.
]]--


--- Helper function for tabulating messages.
local function tab(n) return string.rep(" ", n) end


--- Generates a multi-line string representing a lightning bolt
local function bolt()
    local bolt_lines = {}
    for n = 29, 21, -1 do
        table.insert(bolt_lines, tab(n) .. "x x")
    end
    table.insert(bolt_lines, tab(20) .. "x xxx")
    table.insert(bolt_lines, tab(19) .. "x   x")
    table.insert(bolt_lines, tab(18) .. "xx x")
    for n = 19, 12, -1 do
        table.insert(bolt_lines, tab(n) .. "x x")
    end
    table.insert(bolt_lines, tab(11) .. "xx")
    table.insert(bolt_lines, tab(10) .. "x")
    table.insert(bolt_lines, tab(9) .. "*\n")
    table.insert(bolt_lines, string.rep("#", 25) .. "\n")
    return table.concat(bolt_lines, "\n")
end


--- Print the prompt and read a yes/no answer from stdin.
local function ask_yes_or_no(prompt)
    io.stdout:write(prompt .. " ")
    local answer = string.lower(io.stdin:read("*l"))
    -- any line starting with a 'y' or 'Y' is considered a 'yes'
    return answer:sub(1, 1) == "y"
end


--- Print the prompt and read a valid number from stdin.
local function ask_number(prompt)
    io.stdout:write(prompt .. " ")
    while true do
        local n = tonumber(io.stdin:read("*l"))
        if n then
            return n
        else
            print("Enter a valid number.")
        end
    end
end


--- Explain the solution to persuade the player.
local function explain_solution()
    local k = ask_number("What was your original number?")
    -- For clarity we kept the same variable names of the original BASIC version
    local f = k + 3
    local g = f / 5
    local h = g * 8
    local i = h / 5 + 5
    local j = i - 1
    print("So you think you're so smart, eh?")
    print("Now watch.")
    print(k .. " plus 3 equals " .. f .. ". This divided by 5 equals " .. g .. ";")
    print("this times 8 equals " .. h .. ". If we divide by 5 and add 5,")
    print("we get " .. i .. ", which, minus 1, equals " .. j .. ".")
end


--- Main game function.
local function chief_game()
    --- Print game introduction and challenge
    print(tab(29) .. "Chief")
    print(tab(14) .. "Creative Computing  Morristown, New Jersey\n\n")
    print("I am Chief Numbers Freek, the great math god.")
    if not ask_yes_or_no("Are you ready to take the test you called me out for?") then
        print("Shut up, wise tongue.")
    end

    -- Print how to obtain the end result.
    print(" Take a number and add 3. Divide this number by 5 and")
    print("multiply by 8. Divide by 5 and add the same. Subtract 1.")

    -- Ask the result end and reverse calculate the original number.
    local end_result = ask_number(" What do you have?")
    local original_number = (end_result + 1 - 5) * 5 / 8 * 5 - 3

    -- If it is an integer we do not want to print any zero decimals.
    local int_part, dec_part = math.modf(original_number)
    if dec_part == 0 then original_number = int_part end

    -- If the player challenges the answer, print the explanation.
    if not ask_yes_or_no("I bet your number was " .. original_number .. ". Am I right?") then
        explain_solution()
        -- If the player does not accept the explanation, zap them.
        if not ask_yes_or_no("Now do you believe me?") then
            print("YOU HAVE MADE ME MAD!!!")
            print("THERE MUST BE A GREAT LIGHTNING BOLT!\n\n")
            print(bolt())
            print("I hope you believe me now, for your sake!!")
        end
    end
end

--- Run the game.
chief_game()
