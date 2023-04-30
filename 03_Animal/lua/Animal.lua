--
-- Animal.lua
--

-- maintain a flat table/list of all the known animals
local animals = {
    "FISH",
    "BIRD"
}

-- store the questions as a binary tree with each node having a 
-- "y" or "n" branch
-- if the node has a member named "a" it's an answer, with an index
-- into the animals list.
-- Otherwise, it's a question that leads to more nodes.
local questionTree = {
    q = "DOES IT SWIM",
    y = { a = 1 },
    n = { a = 2 }
}

-- print the given prompt string and then wait for input
-- loops until a non-empty input is given
-- returns the input as an upper-case string
function askPrompt(promptString)
    local answered = false 
    local a
    while (not answered) do 
        print(promptString)
        a = io.read()
        a = string.upper(a)
        if (string.len(a) > 0) then 
            answered = true 
        end
    end
    return a
end

-- print the given prompt string and then wait for the
-- user to enter a string beginning with "Y" or "N"
function askYesOrNo(promptString)
    local a
    while ((a ~= "Y") and (a ~= "N")) do 
        a = askPrompt(promptString)
        a = a:sub(1,1)
    end
    return a
end

-- prints the introductory text from the original BASIC program
function printIntro()
    print(string.format("%32s", " ") .. "ANIMAL")
    print(string.format("%15s", " ") .. "CREATIVE COMPUTING   MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()
    print("PLAY 'GUESS THE ANIMAL'")
    print("THINK OF AN ANIMAL AND THE COMPUTER WILL TRY TO GUESS IT.")
    print()
end

-- prints the animals known in the source list
function listKnownAnimals()
    print()
    print("ANIMALS I ALREADY KNOW ARE:")

    local x
    local item
    for x = 1,#animals do 
        -- use string.format to space each animal in a 12-character-wide "cell"
        item = string.format("%-12s", animals[x])

        -- io.write() works like print(), but doesn't automatically add a carriage return/newline
        io.write(item)

        -- every fifth item, start a new line
        if ((x % 5) == 0) then 
            io.write("\n")
        end
    end

    print()
    print()
end

-- Prompts the user for info about the animal they were thinking of, then
-- uses that to add a new branch to the tree
--      curNode: the node in the tree where the computer made a wrong guess
--      branch: the answer the user gave to curNode's question
function addAnimalToTree(curNode, branch)
    local newAnimal 
    local curResponse = curNode[branch]
    local guessedIndex = curResponse.a
    local guessedAnimal = animals[guessedIndex]
    local newQuestion, newAnswer, newIndex
    local newNode

    newAnimal = askPrompt("THE ANIMAL YOU WERE THINKING OF WAS A ?")
    newQuestion = askPrompt("PLEASE TYPE IN A QUESTION THAT WOULD DISTINGUISH A "..
        tostring(newAnimal).." FROM A "..tostring(guessedAnimal))
    newAnswer = askYesOrNo("FOR A "..tostring(newAnimal).." THE ANSWER WOULD BE?")

    -- add the new animal to the master list at the end, and
    -- save off its index in the list
    table.insert(animals, newAnimal)
    newIndex = #animals

    -- create a new node for the question we just learned
    newNode = {}
    newNode.q = newQuestion
    if (newAnswer == "Y") then 
        newNode.y = { a = newIndex }
        newNode.n = { a = guessedIndex }
    else 
        newNode.y = { a = guessedIndex }
        newNode.n = { a = newIndex }
    end 

    -- replace the previous answer with our new node
    curNode[branch] = newNode
end

-- Starts at the root of the question tree and asks questions about
-- the user's animal until the computer hits an "a" answer node and tries
-- to make a guess
function askAboutAnimal()
    local curNode = questionTree
    local finished = false
    local response, responseIndex
    local nextNode, animalName
    while (not finished) do 
        response = askYesOrNo(curNode.q .. "?")
        
        -- convert the response "Y" or "N" to the lowercase "y" or "n" that we use to name our branches
        branch = string.lower(response)
        nextNode = curNode[branch]
        
        -- is the next node an answer node, or another question?
        if (nextNode.a ~= nil) then 
            -- it's an answer, so make a guess
            animalName = animals[nextNode.a]
            response = askYesOrNo("IS IT A "..tostring(animalName).."?")
            if (response == "Y") then
                -- we got the correct answer, so prompt for a new animal
                print()
                print("WHY NOT TRY ANOTHER ANIMAL?")
            else 
                -- incorrect answer, so add a new entry at this point in the tree
                addAnimalToTree(curNode, branch)
            end

            -- whether we were right or wrong, we're finished with this round
            finished = true
        else 
            -- it's another question, so advance down the tree
            curNode = nextNode
        end
    end
end

-- MAIN CONTROL SECTION

printIntro()

-- loop forever until the player requests an exit by entering a blank line
local exitRequested = false
local answer 

while (not exitRequested) do
    print("ARE YOU THINKING OF AN ANIMAL?")
    answer = io.read()
    answer = string.upper(answer)

    if (string.len(answer) == 0) then
        exitRequested = true
    elseif (answer:sub(1,4) == "LIST") then
        listKnownAnimals()
    elseif (answer:sub(1,1) == "Y") then
        askAboutAnimal()
    end
end
