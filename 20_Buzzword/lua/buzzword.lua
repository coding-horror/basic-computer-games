--- Buzzword
--- Ported by Brian Wilkins.
--- Updated for modern buzzwords and corporate-speak

print [[
                        BUZZWORD GENERATOR
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY

]]

print [[


THIS PROGRAM PRINTS HIGHLY ACCEPTABLE PHRASES IN
'EDUCATOR-SPEAK' THAT YOU CAN WORK INTO REPORTS
AND SPEECHES.  WHENEVER A QUESTION MARK IS PRINTED,
TYPE A 'Y' FOR ANOTHER PHRASE OR 'N' TO QUIT.
]]

local phraseList = {"ABILITY","BASAL","BEHAVIORAL","CHILD-CENTERED",
               "DIFFERENTIATED","DISCOVERY","FLEXIBLE","HETEROGENEOUS",
               "HOMOGENEOUS","MANIPULATIVE","MODULAR","TAVISTOCK",
               "INDIVIDUALIZED","LEARNING","EVALUATIVE","OBJECTIVE",
               "COGNITIVE","ENRICHMENT","SCHEDULING","HUMANISTIC",
               "INTEGRATED","NON-GRADED","TRAINING","VERTICAL AGE",
               "MOTIVATIONAL","CREATIVE","GROUPING","MODIFICATION",
               "ACCOUNTABILITY","PROCESS","CORE CURRICULUM","ALGORITHM",
               "PERFORMANCE","REINFORCEMENT","OPEN CLASSROOM","RESOURCE",
               "STRUCTURE","FACILITY","ENVIRONMENT"}

--- Credit to https://stackoverflow.com/a/33468353/19232282
--- for the pickPhrase function
local copyPhraseList = {}

function pickPhrase()
   local i
   -- make a copy of the original table if we ran out of phrases
   if #copyPhraseList == 0 then
      for k,v in pairs(phraseList) do
         copyPhraseList[k] = v
      end
   end
               
   -- pick a random element from the copy  
   i = math.random(#copyPhraseList)
   phrase = copyPhraseList[i] 
               
   -- remove phrase from copy
   table.remove(copyPhraseList, i)
               
   return phrase
end

--- Reused from Bagels.lua
function getInput(prompt)
    io.write(prompt)
    io.flush()
    local input = io.read("l") 
    if not input then  --- test for EOF
        print("COME BACK WHEN YOU NEED HELP WITH ANOTHER REPORT!")
        os.exit(0)
    end
    return input
end

for i=1,3,1 do
    ::phrasepick::
    io.write (pickPhrase() .. " ")
    io.write (pickPhrase() .. " ")
    io.write (pickPhrase())
    print()
    io.stdin:flush()
    local response = getInput("? ")
    if response:match("[yY].*") then
       goto phrasepick
    else
       print("COME BACK WHEN YOU NEED HELP WITH ANOTHER REPORT!")
       os.exit(0)
    end
end

