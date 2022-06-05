-- Ported by Brian Wilkins (BrianWilkinsFL)
-- Influenced by bombsaway.py
-- Requires: Lua 5.4.x

missileHitRate = 0
gunsHitRate = 0

nanMessage = "Invalid user entry! Please use a number and try again."
function has_key(table, key)
   return table[key]~=nil
end

function choice(prompt, table)
    resp = getInput(prompt)
    while not has_key(table, resp) do
       print("TRY AGAIN...")
       resp = getInput(prompt)
    end
    return resp
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

function playerSurvived()
    print("YOU MADE IT THROUGH TREMENDOUS FLAK!!")
end

function playerDeath()
    print("* * * * BOOM * * * *")
    print("YOU HAVE BEEN SHOT DOWN.....")
    print("DEARLY BELOVED, WE ARE GATHERED HERE TODAY TO PAY OUR")
    print("LAST TRIBUTE...")
end

-- Enhancement: Partially killed don't count so floor the float
function missionSuccess()
   print("DIRECT HIT!!!! " .. math.floor(100*math.random()) .. " KILLED.")
   print("MISSION SUCCESSFUL.")
end

function missionFailure()
    weapons_choices = {
        ["1"] = "GUNS",
        ["2"] = "MISSILES",
        ["3"] = "BOTH",
    }
    print("MISSED TARGET BY " .. math.floor(2 + 30 * math.random()) .. " MILES!")
    print("NOW YOU'RE REALLY IN FOR IT !!")
    print()
    enemy_weapons = choice("DOES THE ENEMY HAVE GUNS(1), MISSILES(2), OR BOTH(3)? ", weapons_choices)

    if enemy_weapons == "2" or enemy_weapons == "3" then
        missileHitRate = 35
    end

    if enemy_weapons == "2" then
       -- gunsHitRate is a reused global variable so
       -- its possible that previously selected gunsHitRate
       -- will be used here leading to interesting
       -- randomness
       if missileHitRate+gunsHitRate > 100*math.random() then
          playerDeath()
       else
          playerSurvived()
       end
    end

    if enemy_weapons == "1" or enemy_weapons == "3" then
        while true do
           resp = getInput("WHAT'S THE PERCENT HIT RATE OF ENEMY GUNNERS (10 TO 50)? ")
           if assert(tonumber(resp)) then
              gunsHitRate = tonumber(resp)
              break
           end
        end
        if gunsHitRate < 10 then
           print("YOU LIE, BUT YOU'LL PAY...")
           playerDeath()
           return
        end
        if missileHitRate+gunsHitRate > 100*math.random() then
           playerDeath()
        else
           playerSurvived()
        end
    end
end

-- override assert so Lua doesn't throw an error
-- and stack trace
-- We just want the user to enter in a correct value
assert = function(truth, message)
   if not truth then
      print(message)
   end
   return truth
end

-- Added logic to verify user is actually entering in a number
function commence_non_kamikaze_attack()
    local numMissions = 0
    while numMissions < 160 do
       while numMissions == 0 do
          resp = getInput("HOW MANY MISSIONS HAVE YOU FLOWN? ")
          if assert(tonumber(resp), nanMessage) then
             numMissions = tonumber(resp)
             break
          end
       end
       if numMissions < 160 then break end

       print("MISSIONS, NOT MILES...")
       print("150 MISSIONS IS HIGH EVEN FOR OLD-TIMERS.")
       print("NOW THEN, ")
       numMissions = 0
    end

    if numMissions >= 100 then
       print("THAT'S PUSHING THE ODDS!")
    end

    if numMissions < 25 then
       print("FRESH OUT OF TRAINING, EH?")
    end

    if numMissions >= 160*math.random() then 
        missionSuccess()
    else 
        missionFailure() 
    end
end

function playItaly()
    targets_to_messages = {
       ["1"] = "SHOULD BE EASY -- YOU'RE FLYING A NAZI-MADE PLANE.",
       ["2"] = "BE CAREFUL!!!",
       ["3"] = "YOU'RE GOING FOR THE OIL, EH?",
    }

    target = choice("YOUR TARGET -- ALBANIA(1), GREECE(2), NORTH AFRICA(3)? ", targets_to_messages)
    print(targets_to_messages[target])
    commence_non_kamikaze_attack()
end

function playAllies()
    aircraft_to_message = {
        ["1"] = "YOU'VE GOT 2 TONS OF BOMBS FLYING FOR PLOESTI.",
        ["2"] = "YOU'RE DUMPING THE A-BOMB ON HIROSHIMA.",
        ["3"] = "YOU'RE CHASING THE BISMARK IN THE NORTH SEA.",
        ["4"] = "YOU'RE BUSTING A GERMAN HEAVY WATER PLANT IN THE RUHR.",
    }
    aircraft = choice("AIRCRAFT -- LIBERATOR(1), B-29(2), B-17(3), LANCASTER(4)? ", aircraft_to_message)
    print(aircraft_to_message[aircraft])
    commence_non_kamikaze_attack()
end

function playJapan()
    print("YOU'RE FLYING A KAMIKAZE MISSION OVER THE USS LEXINGTON.")
    first_mission = getInput("YOUR FIRST KAMIKAZE MISSION? (Y OR N)? "):match("[yYnN].*")
    if first_mission:lower() == "n" then
        return playerDeath()
    end
    if math.random() > 0.65 then
       return missionSuccess()
    else
       playerDeath()
    end
end

function playGermany()
    targets_to_messages = {
        ["1"] = "YOU'RE NEARING STALINGRAD.",
        ["2"] = "NEARING LONDON.  BE CAREFUL, THEY'VE GOT RADAR.",
        ["3"] = "NEARING VERSAILLES.  DUCK SOUP.  THEY'RE NEARLY DEFENSELESS.",
    }
    target = choice("A NAZI, EH?  OH WELL.  ARE YOU GOING FOR RUSSIA(1),\nENGLAND(2), OR FRANCE(3)? ", targets_to_messages)

    print(targets_to_messages[target])
    return commence_non_kamikaze_attack()
end

function playGame()
   sides = {
        ["1"] = playItaly,
        ["2"] = playAllies,
        ["3"] = playJapan,
        ["4"] = playGermany
   }
   print("YOU ARE A PILOT IN A WORLD WAR II BOMBER.")

   target = choice("WHAT SIDE -- ITALY(1), ALLIES(2), JAPAN(3), GERMANY(4)? ", sides)
   sides[target]()
end

local again = true
while again do
   playGame()
   again = getInput("ANOTHER MISSION (Y OR N)? "):match("[yY].*")
end

print("CHICKEN !!!")