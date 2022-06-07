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

function has_key(table, key)
   return table[key]~=nil
end
 
function choice(prompt, table)
   resp = getInput(prompt)
   while not has_key(table, resp) do
      print("INVALID.")
      resp = getInput(prompt)
   end
   return resp
end

function playGame(n)
   local computerWins = 0
   local humanWins = 0
   itemChoices = {
        ["1"] = "PAPER",
        ["2"] = "SCISSORS",
        ["3"] = "ROCK",
   }
   print("GAME NUMBER " .. n)

   math.randomseed(os.time())
   computerChoice = math.random(1,3)
  
   humanChoice = choice("3=ROCK...2=SCISSORS...1=PAPER\n1...2...3...WHAT'S YOUR CHOICE? ", itemChoices)
   humanChoice = tonumber(humanChoice)
   print("THIS IS MY CHOICE...")

   print("... " .. itemChoices[tostring(computerChoice)])

   if computerChoice == humanChoice then 
      print("TIE GAME.  NO WINNER.")
   elseif computerChoice > humanChoice then
      if humanChoice ~=1 or computerChoice ~= 3 then 
         computerWins = 1
         print("WOW!  I WIN!!!")
      else
         humanWins = 1
         print("YOU WIN!!!")
      end
   elseif computerChoice == 1 then
      if humanChoice ~= 3 then
         humanWins = 1
         print("YOU WIN!!!")
      else
         computerWins = 1
         print("WOW! I WIN!!!")
      end
   end
   return computerWins, humanWins
end

local games = 0
while games == 0 or games >= 11 do
   resp = getInput("HOW MANY GAMES? ")
   
   assert(tonumber(resp))
   games = tonumber(resp)
   if games < 11 then break end
   
   print("SORRY, BUT WE AREN'T ALLOWED TO PLAY THAT MANY.")
end

totalComputerWins = 0
totalHumanWins = 0

for n=1, games, 1 do
   computerWins, humanWins = playGame(n)
   totalComputerWins = totalComputerWins + computerWins
   totalHumanWins = totalHumanWins + humanWins
end

print("HERE IS THE FINAL GAME SCORE:")
print("I HAVE WON " .. totalComputerWins .. " GAME(S).")
print("YOU HAVE WON " .. totalHumanWins .." GAME(S).")
print("AND " .. games-(totalComputerWins+totalHumanWins) .. " GAME(S) ENDED IN A TIE.")
print("THANKS FOR PLAYING!!")