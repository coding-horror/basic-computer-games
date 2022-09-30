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

print("WHAT'S YOUR NAME (FIRST AND LAST)")

local ns = getInput("? ")
local l = string.len(ns)
print("\n")

local function main()
    print("THANK YOU, " .. string.reverse(ns) .. ".\n")

    print("OOPS!  I GUESS I GOT IT BACKWARDS.  A SMART")
    print("COMPUTER LIKE ME SHOULDN'T MAKE A MISTAKE LIKE THAT!\n")
    print("BUT I JUST NOTICED YOUR LETTERS ARE OUT OF ORDER.\n")
    print("LET'S PUT THEM IN ORDER LIKE THIS: ")
    
    local b = {}
    
    for i = 1, l, 1 do
        local letter = string.sub(ns, i, i)
        b[i] = string.byte(letter)
    end
    
    table.sort(b, function(v1, v2)
        return v1 < v2
    end)
    
    local str = ""
    for _, letter in ipairs(b) do
        str = str .. string.char(letter)
    end
    
    str = string.reverse(str)
    print(str)
    
    print("\n\n")
    print("DON'T YOU LIKE THAT BETTER")
    
    local ds = getInput("? ")
        
    if ds == "YES" then
        print("I KNEW YOU'D AGREE!!\n")
    else
        print("I'M SORRY YOU DON'T LIKE IT THAT WAY.\n")
    end
    
    print("I REALLY ENJOYED MEETING YOU " ..  ns .. ".\n")
    print("HAVE A NICE DAY!\n")
end

main()
