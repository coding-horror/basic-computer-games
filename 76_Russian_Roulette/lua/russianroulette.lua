print [[
                    RUSSIAN ROULETTE
        CREATIVE COMPUTING  MORRISTOWN, NEW JERSY
This is a game of >>>>>>>>>>Russian Roulette
Here is a Revolver

]]

local function parse_input()
    local incorrect_input = true
    local input = nil
    while incorrect_input do
        input = io.read(1)
        if input == "1" or input == "2" then incorrect_input = false end
    end
    return input
end

local function russian_roulette()
    local NUMBER_OF_ROUNDS = 9

    while true do
        local dead = false
        local n = 0
        print("Type '1' to Spin chamber and pull trigger")
        print("Type '2' to Give up")
        print("Go")

        while not dead do
            local choice = parse_input()
            if choice == "2" then break end

            if math.random() > 0.833333333333334 then
                dead = true
            else
                print("CLICK")
                n = n + 1
            end

            if n > NUMBER_OF_ROUNDS then break end
        end

        if dead then
            print("BANG!!!!!   You're Dead!")
            print("Condolences will be sent to your relatives.\n\n\n")
            print("...Next victim...")
        elseif n > NUMBER_OF_ROUNDS then
            print("You win!!!!!")
            print("Let someone else blow his brain out.\n")
        else
            print("     Chicken!!!!!\n\n\n")
            print("...Next victim....")
        end
    end
end

russian_roulette()

