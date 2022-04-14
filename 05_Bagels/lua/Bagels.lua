---
--- Bagels
--- Ported by Joe Nellis.
--- Text displayed is altered slightly from the original program to allow for
--- more (or less) than three digits to be guessed. Change the difficulty to
--- the number of digits you wish in the secret code.
---

--- difficult is number of digits to use
local difficulty = 3

print [[
                                BAGELS
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY

]]

function getInput(prompt)
    io.write(prompt)
    io.flush()
    local input = io.read("l") 
    if not input then  --- test for EOF
        print("GOODBYE")
        os.exit(0)
    end
    return input
end

local needsRules = getInput("WOULD YOU LIKE THE RULES? (YES OR NO) ")
print()
if needsRules:match("[yY].*") then
    print(string.format( [[
I AM THINKING OF A %u DIGIT NUMBER.  TRY TO GUESS
MY NUMBER AND I WILL GIVE YOU CLUES AS FOLLOWS:
   PICO   - ONE DIGIT CORRECT BUT IN THE WRONG POSITION
   FERMI  - ONE DIGIT CORRECT AND IN THE RIGHT POSITION
   BAGELS - NO DIGITS
   ]], difficulty))
end

function play(numDigits, score)
    local digits = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" }
    --- secret number must not have duplicate digits
    --- randomly swap numDigits at the head of this list to create secret number
    for i = 1, numDigits do
        local j = math.random(1, 10)
        digits[i], digits[j] = digits[j], digits[i]
    end

    print "O.K.  I HAVE A NUMBER IN MIND."
    for guessNum = 1, 20 do
        :: GUESS_AGAIN :: ---<!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        local guess = getInput(string.format("GUESS #%u\t?",guessNum))
        if #guess ~= numDigits then
            print("TRY GUESSING A", numDigits, "DIGIT NUMBER.")
            goto GUESS_AGAIN
        elseif not guess:match("^(%d+)$") then
            print("WHAT?")
            goto GUESS_AGAIN
        else
            --- check if user has duplicate digits
            for i = 1, numDigits - 1 do
                for j = i + 1, numDigits do
                    if (guess:sub(i, i) == guess:sub(j, j)) then
                        print("OH, I FORGOT TO TELL YOU THAT THE NUMBER I HAVE")
                        print("IN MIND HAS NO TWO DIGITS THE SAME.")
                        goto GUESS_AGAIN
                    end
                end
            end
        end

        local report = ""
        --- check for picos, right digit, wrong place
        for i = 1, numDigits do
            for j = i + 1, numDigits - 1 + i do
                if (guess:sub(i, i) == digits[(j - 1) % numDigits + 1]) then
                    report = report .. "PICO "
                end
            end
        end
        --- check for fermis, right digit, right place
        for i = 1, numDigits do
            if (guess:sub(i, i) == digits[i]) then
                report = report .. "FERMI "
            end
        end

        if (report == string.rep("FERMI ", numDigits)) then
            print "YOU GOT IT!!!"
            print ""
            score = score + 1
            goto PLAY_AGAIN
        end
        if (report == "") then
            print("BAGELS")
        else
            print(report)
        end
    end
    print "OH WELL."
    print("THAT'S TWENTY GUESSES. MY NUMBER WAS "
            .. table.concat(digits, "", 1, numDigits))

    :: PLAY_AGAIN :: ---<!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    local playAgain = getInput("PLAY AGAIN (YES OR NO)?")
    print()
    if (playAgain:match("[yY].*")) then
        return play(numDigits, score)
    else
        if (score > 0) then
            print("A", score, "POINT BAGELS BUFF!!")
        end
        print "HOPE YOU HAD FUN. BYE."
    end
end

play(difficulty, 0) --- default is numDigits=3, score=0

