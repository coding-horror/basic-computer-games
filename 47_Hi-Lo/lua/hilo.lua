local function hilo (randomNum)
   local numTries = 0
   math.randomseed(os.time())

   local randomNum = math.random(1, 100)
   print(randomNum)

   while numTries < 6 do
      print("")

      io.write("YOUR GUESS? ")
   
      local guess = io.read("*n")

      numTries = numTries + 1

      if guess < randomNum then
         print("YOUR GUESS IS TOO LOW")
      end

      if guess > randomNum then
         print("YOUR GUESS IS TOO HIGH")
      end

      if guess == randomNum then
         print("GOT IT!!!!!!!!!!   YOU WIN " .. randomNum .. " DOLLARS.")
         break
      end
    end
 
    if numTries == 6 then
       print("")
       print("YOU BLEW IT...TOO BAD...THE NUMBER WAS " .. randomNum)
       return 0
    else
       return randomNum
    end
end

local THIRTY_FOUR_TABS=string.rep("\t",34)
print(THIRTY_FOUR_TABS, "HI LO")

local FIFTEEN_TABS=string.rep("\t",15)
print(FIFTEEN_TABS, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")

local THREE_NEWLINES=string.rep("\n", 3)
print(THREE_NEWLINES)

print("THIS IS THE GAME OF HI LO.")
print("")
print("YOU WILL HAVE 6 TRIES TO GUESS THE AMOUNT OF MONEY IN THE")
print("HI LO JACKPOT, WHICH IS BETWEEN 1 AND 100 DOLLARS.  IF YOU")
print("GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!")
print("THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY.  HOWEVER,")
print("IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS.")

local wonSoFar = 0

::continue::
local won = 0
local won = hilo(randomNum)
wonSoFar = won + wonSoFar
print("YOUR TOTAL WINNINGS ARE NOW " .. wonSoFar .. " DOLLARS.")

--- This flush is here because if not then it will keep the newline in the
--- input buffer and cause the program to inadvertantly go to the 
--- Invalid Answer!
--- part of the code which we don't want the program to do. Appears to be a
--- Lua-ism.

io.stdin:flush()
io.write("PLAY AGAIN (YES OR NO)? ")
answer = io.read()

while(not(answer == "YES" or answer == "NO")) do
   io.write("Invalid Answer! Try again (YES/NO): ")
   answer = io.read()
end

if answer == "YES" then
   goto continue
else
   print("")
   print("SO LONG.  HOPE YOU ENJOYED YOURSELF!!!") 
   os.exit()
end