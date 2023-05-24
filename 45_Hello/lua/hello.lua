-- HELLO
--
-- Converted from BASIC to Lua by Recanman

local function tab(space)
    local str = ""

    for _ = space, 1, -1 do
        str = str .. " "
    end

    return str
end

-- reused from Bagels.lua
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

print(tab(33) .. "HELLO\n")
print(tab(15) .. "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")
print("\n")
print("\n")
print("\n")

print("HELLO.  MY NAME IS CREATIVE COMPUTER.\n")
print("\n")
print("\n")

print("WHAT'S YOUR NAME")
local ns = getInput("? ")

print("\n")
print("HI THERE, " .. ns .. ", ARE YOU ENJOYING YOURSELF HERE")

while true do
    local bs = getInput("? ")
    print("\n")
    if bs == "YES" then
        print("I'M GLAD TO HEAR THAT, " .. ns .. ".\n")
        print("\n")
        break
    elseif bs == "NO" then
        print("OH, I'M SORRY TO HEAR THAT, " .. ns .. ". MAYBE WE CAN\n")
        print("BRIGHTEN UP YOUR VISIT A BIT.\n")
        break
    else
        print("PLEASE ANSWER 'YES' OR 'NO'.  DO YOU LIKE IT HERE")
    end
end

local function main()
    print("\n")
    print("SAY, " .. ns .. ", I CAN SOLVED ALL KINDS OF PROBLEMS EXCEPT\n")
    print("THOSE DEALING WITH GREECE.  WHAT KIND OF PROBLEMS DO\n")
    print("YOU HAVE (ANSWER SEX, HEALTH, MONEY, OR JOB)")

    while true do
        local cs = getInput("? ")
        print("\n")

        if cs ~= "SEX" and cs ~= "HEALTH" and cs ~= "MONEY" and cs ~= "JOB" then
            print("OH, " .. ns .. ", YOUR ANSWER OF " .. cs .. " IS GREEK TO ME.\n")
        elseif cs == "JOB" then
            print("I CAN SYMPATHIZE WITH YOU " .. ns .. ".  I HAVE TO WORK\n")
            print("VERY LONG HOURS FOR NO PAY -- AND SOME OF MY BOSSES\n")
            print("REALLY BEAT ON MY KEYBOARD.  MY ADVICE TO YOU, " .. ns .. ",\n")
            print("IS TO OPEN A RETAIL COMPUTER STORE.  IT'S GREAT FUN.\n")
        elseif cs == "MONEY" then
            print("SORRY, " .. ns .. ", I'M BROKE TOO.  WHY DON'T YOU SELL\n")
            print("ENCYCLOPEADIAS OR MARRY SOMEONE RICH OR STOP EATING\n")
            print("SO YOU WON'T NEED SO MUCH MONEY?\n")
        elseif cs == "HEALTH" then
            print("MY ADVICE TO YOU " .. ns .. " IS:\n")
            print(tab(5) .. "1.  TAKE TWO ASPRIN\n")
            print(tab(5) .. "2.  DRINK PLENTY OF FLUIDS (ORANGE JUICE, NOT BEER!)\n")
            print(tab(5) .. "3.  GO TO BED (ALONE)\n")
        elseif cs == "SEX" then
            print("IS YOUR PROBLEM TOO MUCH OR TOO LITTLE")

            while true do
                local ds = getInput("? ")
                print("\n")

                if ds == "TOO MUCH" then
                    print("YOU CALL THAT A PROBLEM?!!  I SHOULD HAVE SUCH PROBLEMS!\n")
                    print("IF IT BOTHERS YOU, " .. ns .. ", TAKE A COLD SHOWER.\n")
                    break
                elseif ds == "TOO LITTLE" then
                    print("WHY ARE YOU HERE IN SUFFERN, " .. ns .. "?  YOU SHOULD BE\n")
                    print("IN TOKYO OR NEW YORK OR AMSTERDAM OR SOMEPLACE WITH SOME\n")
                    print("REAL ACTION.\n")
                    break
                else
                    print("DON'T GET ALL SHOOK, " .. ns .. ", JUST ANSWER THE QUESTION\n")
                    print("WITH 'TOO MUCH' OR 'TOO LITTLE'.  WHICH IS IT")
                end
            end
        end

        print("\n")
        print("ANY MORE PROBLEMS YOU WANT SOLVED, " .. ns)

        local es = getInput("? ")

        if es == "YES" then
            print("WHAT KIND (SEX, MONEY, HEALTH, JOB)")
        elseif es == "NO" then
            print("THAT WILL BE $5.00 FOR THE ADVICE, " .. ns .. ".\n")
            print("PLEASE LEAVE THE MONEY ON THE TERMINAL.\n")
            print("\n")
            print("\n")
            print("\n")

            while true do
                print("DID YOU LEAVE THE MONEY")

                local gs = getInput("? ")
                print("\n")

                if gs == "YES" then
                    print("HEY, " .. ns .. "??? YOU LEFT NO MONEY AT ALL!\n")
                    print("YOU ARE CHEATING ME OUT OF MY HARD-EARNED LIVING.\n")
                    print("\n")
                    print("WHAT A RIP OFF, " .. ns .. "!!!\n")
                    print("\n")
                    break
                elseif gs == "NO" then
                    print("THAT'S HONEST, " .. ns .. ", BUT HOW DO YOU EXPECT\n")
                    print("ME TO GO ON WITH MY PSYCHOLOGY STUDIES IF MY PATIENT\n")
                    print("DON'T PAY THEIR BILLS?\n")
                    break
                else
                    print("YOUR ANSWER OF '" .. gs .. "' CONFUSES ME, " .. ns .. ".\n")
                    print("PLEASE RESPOND WITH 'YES' OR 'NO'.\n")
                end
            end

            break
        end
    end

    print("\n")
    print("TAKE A WALK, " .. ns .. ".\n")
    print("\n")
    print("\n")
end

main()