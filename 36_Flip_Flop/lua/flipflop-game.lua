--[[
Game of FlipFlop
===============

Based on the original BASIC game of FlipFlop from the 1970s.

This is a faithful recreation of the classic game, maintaining the original gameplay
while modernizing the implementation for Lua. For example, the original's complex
trigonometric random number generation has been replaced with Lua's math.random,
as modern random number generators are more sophisticated than those available
in 1970s BASIC.

How to Play:
-----------
The game presents a row of 10 X's. Your goal is to flip all X's to O's.
Enter a number from 1-10 to flip that position. The computer will also make
random moves in response. Special commands:
- Enter 0 to see the current board again
- Enter 11 to quit the game
Try to complete the puzzle in 12 moves or fewer for the best score!

Strategy Tips:
------------
- When you make a move, watch how the computer responds. It will often
  make a predictable counter-move.
- If you enter the same position twice, the computer will use a different
  random pattern for its response.
- Try to work systematically from one end of the board to the other,
  rather than making random moves.
- If you get stuck, sometimes starting over (11) is better than continuing
  with a poor position.

Technical Notes:
--------------
- Written for Lua 5.1 compatibility
- Uses tables for game state management
- Implements original game logic without goto statements
- Preserves the original game's move counting and win conditions
- Maintains the classic text-based interface style

Original BASIC Version:
--------------------
The original game used complex trigonometric functions for random number
generation, likely due to limitations in BASIC's random number capabilities:
    R=TAN(Q+N/Q-N)-SIN(Q/N)+336*SIN(8*N)
    N=R-INT(R)
    N=INT(10*N)

This has been simplified to use Lua's built-in random number generator
while maintaining the same gameplay experience.

Converted from BASIC to Lua by Brian Wilkins
March 2025
]]--

local function showInstructions()
    print("                    FLIP FLOP")
    print("              CREATIVE COMPUTING")
    print("            MORRISTOWN, NEW JERSEY")
    print("\n\n")
    print("THE OBJECT OF THIS GAME IS TO FLIP ALL THE X'S TO O'S.")
    print("WHEN YOU INPUT A NUMBER, THAT NUMBER AND ALL ADJACENT")
    print("NUMBERS WILL BE FLIPPED.")
    print("\n")
    print("INPUT A NUMBER FROM 1 TO 10 TO START THE GAME. TO QUIT")
    print("THE GAME AT ANY TIME, TYPE A 0 (ZERO). TO START OVER WITH")
    print("A NEW PUZZLE IN THE MIDDLE OF A GAME, TYPE A 11 (ELEVEN).")
    print("\n\n")
end

local function printUpdatedBoard(board)
    print("1 2 3 4 5 6 7 8 9 10")
    print(table.concat(board, " "))
    print("\n\n")
end

local function processMove(gameState, position)
    -- Check if current position is O
    if gameState.board[position] == "O" then
        gameState.board[position] = "X"
        if position == gameState["lastMove"] then
            -- Generate random position using simplified random logic
            local R = math.random()  -- between 0 and 1
            local N = math.floor(R * 10) + 1
            if gameState.board[N] == "O" then
                gameState.board[N] = "X"
            else
                gameState.board[N] = "O"
            end
        end
    else
        gameState.board[position] = "O"
        -- Generate another random position
        local R = math.random()
        local N = math.floor(R * 10) + 1
        if gameState.board[N] == "O" then
            gameState.board[N] = "X"
        else
            gameState.board[N] = "O"
        end
    end
end

local function checkWin(gameState)
    -- Check if all positions are O's
    for i = 1, 10 do
        if gameState.board[i] ~= "O" then
            return false
        end
    end
    return true
end

local function initializeGame()
    local gameState = {
        board = {},
        lastMove = nil,
        moves = 0
    }
    
    -- Initialize board
    for i = 1, 10 do
        gameState.board[i] = "X"
    end
    return gameState
end

local function playGame()
    showInstructions()
    
    local gameState = initializeGame()
    local playing = true
    
    print("HERE IS THE STARTING LINE OF X'S.")
    print("\n\n")
    printUpdatedBoard(gameState.board)
    
    while playing do
        print("INPUT THE NUMBER")
        local N = tonumber(io.read())
        
        -- Check if input is valid
        if not N or N ~= math.floor(N) then
            print("ILLEGAL ENTRY--TRY AGAIN.")
        elseif N == 11 then
            playing = false
        elseif N > 11 then
            print("ILLEGAL ENTRY--TRY AGAIN.")
        elseif N == 0 then
            printUpdatedBoard(gameState.board)
        else
            -- Process the move
            processMove(gameState, N)
            gameState["lastMove"] = N
            gameState.moves = gameState.moves + 1
            
            -- Show the updated board
            printUpdatedBoard(gameState.board)
            
            -- Check for win
            if checkWin(gameState) then
                if gameState.moves <= 12 then
                    print(string.format("VERY GOOD. YOU GUESSED IT IN ONLY %d GUESSES.", gameState.moves))
                else
                    print(string.format("TRY HARDER NEXT TIME. IT TOOK YOU %d GUESSES.", gameState.moves))
                end
                
                print("DO YOU WANT TO TRY ANOTHER PUZZLE? (Y/N)")
                local answer = io.read():upper()
                if answer:sub(1,1) == "Y" then
                    gameState = initializeGame()
                    print("HERE IS THE STARTING LINE OF X'S.")
                    print("\n\n")
                    printUpdatedBoard(gameState.board)
                else
                    playing = false
                end
            end
        end
    end
end

playGame()
