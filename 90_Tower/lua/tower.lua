print [[
            
            TOWERS
CREATIVE COMPUTING  MORRISTOWN, NEW JERSY


]]

local MAX_DISKS <const> = 7
local MAX_DISK_SIZE <const> = 15
local MAX_MOVES <const> = 128
local NUM_TOWERS <const> = 3

local towers = {
    { size = 0, elem = {} },
    { size = 0, elem = {} },
    { size = 0, elem = {} },
}

local total_disks
function ask_how_many_disks()

    local keep_asking = true
    local input
    local errors = 0

    while keep_asking do

        io.write(string.format("HOW MANY DISKS DO YOU WANT TO MOVE (%d IS MAX)? ", MAX_DISKS) )
        input = io.read("*number")
        io.read() --get rid of the remaining newline character

        if input ~= nil and input>0 and input<=MAX_DISKS then
            keep_asking = false

        else
            errors = errors + 1
            if errors > 2 then
                print "ALL RIGHT, WISE GUY, IF YOU CAN'T PLAY THE GAME RIGHT, I'LL"
                print "JUST TAKE MY PUZZLE AND GO HOME. SO LONG."
                os.exit()
            end

            print "SORRY, BUT I CAN'T DO THAT JOB FOR YOU."
        end
    end
    total_disks = input
end

function init_game()
    print("TOWERS OF HANOIR PUZZLE\n") --'\n' indicates a new line

    print "YOU MUST TRANSFER THE DISKS FROM THE LEFT TO THE RIGHT TOWER,"
    print "ONE AT A TIME, NEVER PUTTING A LARGER DISK ON A SMALLER DISK.\n"

    ask_how_many_disks()

    print() -- print() already creates a new line at the end, so an empty print leaves an empty line
    print "IN THIS PROGRAM, WE SHALL REFER TO DISKS BY NUMERICAL CODE."
    print "3 WILL REPRESENT THE SMALLEST DISK, 5 THE NEXT SIZE, 7 THE"
    print "NEXT, AND SO ON, UP TO 15. IF YOU DO THE PUZZLE WITH 2 DISKS,"
    print "THEIR CODE NAMES WOULD BE 13 AND 15, ETC. THE NEEDLES ARE"
    print "NUMBERED FROM LEFT TO RIGHT, 1 TO 3. WE WILL START WITH THE"
    print "DISKS ON NEEDLE 1, AND ATTEMPT TO MOVE THEM TO NEEDLE 3.\n"
    
    print "GOOD LUCK!\n"

    local i = 1
    local max = MAX_DISK_SIZE
    while i<= total_disks do
        towers[1].elem[i] = max
        max = max-2
        i = i+1
    end

    towers[1].size = total_disks

    local idx = 2
    while idx <= NUM_TOWERS do
        towers[idx].size = 0
    end
end

function print_towers()
    local line = MAX_DISKS
    
    while line > 0 do
        local twr = 1
        
        while twr <=3 do
            local rpt = 10
            local offset = 0
            if line <= towers[twr].size then
                offset = (towers[twr].elem[line] - 1) / 2
                rpt = rpt - offset
            end
            io.write( string.rep(' ', rpt) )
            io.write( string.rep('*', offset) )
            io.write '*'
            io.write( string.rep('*', offset) )
            io.write( string.rep(' ', rpt) )
            twr = twr + 1
        end
        print ''
        line = line - 1
    end
    
end

function ask_which_disk()

    local keep_asking = true
    local input
    local errors = 0
    while keep_asking do

        io.write("WHICH DISK WOULD YOU LIKE TO MOVE? ")
        input = io.read("*number")
        io.read() --get rid of the remaining newline character

        if input==nil or input > MAX_DISK_SIZE or input%2==0 or input <= MAX_DISK_SIZE-(total_disks*2) then
            print "ILLEGAL ENTRY... YOU MAY ONLY TYPE 3,5,7,9,11,13 or 15."
            errors = errors + 1
            if errors > 1 then
                print "STOP WASTING MY TIME. GO BOTHER SOMEONE ELSE."
                os.exit()
            end

        --[[
        Since there are only 3 towers, it's easier to do an 'if' with three
        conditions than to do a loop
        ]]
        elseif towers[1].elem[ towers[1].size ] ~= input and
                towers[2].elem[ towers[2].size ] ~= input and
                towers[3].elem[ towers[3].size ] ~= input then

            print "THAT DISK IS BELOW ANOTHER ONE. MAKE ANOTHER CHOICE."
        else
            keep_asking = false 
        end
    end
    
    return input
end

function ask_which_needle(dsk)

    local keep_asking = true
    local input
    local errors = 0

    while keep_asking do

        io.write("PLACE DISK ON WHICH NEEDLE? ")
        input = io.read("*number")
        io.read() --get rid of the remaining newline character

        if input~=nil and towers[input].size > 0 and towers[input].elem[ towers[input].size ] < dsk then
            print "YOU CAN'T PLACE A LARGER DISK ON TOP OF A SMALLER ONE,"
            print "IT MIGHT CRUSH IT!"
            return 0

        elseif input~=nil and input>=1 and input<=3 then
                keep_asking = false

        else
            errors = errors + 1
            if errors > 1 then
                print "I TRIED TO WARN YOU, BUT YOU WOULDN'T LISTEN."
                print "BYE BYE, BIG SHOT."
                os.exit() --Stop program
            else
                print "I'LL ASSUME YOU HIT THE WRONG KEY THIS TIME. BUT WATCH IT,"
                print "I ONLY ALLOW ONE MISTAKE."
            end
        end
    end
    return input
end

function is_game_over()
    if towers[1].size == 0 and towers[2].size == 0 then
        return true
    else
        return false
    end
end

function game_loop()
    local moves = 0
    local dsk 
    local twr_to
    local twr_fr

    while not is_game_over() do
        moves = moves + 1

        if moves > MAX_MOVES then
            print(string.format("SORRY, BUT I HAVE ORDERS TO STOP IF YOU MAKE MORE THAN %d MOVES.", MAX_MOVES))
            os.exit()
        end


        repeat
            dsk = ask_which_disk()
            twr_to = ask_which_needle(dsk)
        until twr_to ~= 0


        if towers[1].elem[ towers[1].size ] == dsk then
            twr_fr = 1
        elseif towers[2].elem[ towers[2].size ] == dsk then
            twr_fr = 2
        else
            twr_fr = 3
        end

        towers[twr_fr].size = towers[twr_fr].size - 1

        towers[twr_to].size = towers[twr_to].size + 1
        towers[twr_to].elem[ towers[twr_to].size ] = dsk

        print_towers()
    end

    return moves
end

function keep_playing()
    
    while true do
        io.write("TRY AGAIN (YES OR NO)? ")
        local input = io.read("*line")

        if input == "YES" or input == "yes" then
            return true
        elseif input == "NO" or input == "no" then
            return false
        else
            print "'YES' OR 'NO' PLEASE"
        end
    end
end

function start_loop()

    while true do
        init_game()
        print_towers()

        local moves = game_loop()

        --check ideal solution
        if moves == (2^total_disks) - 1 then
            print "CONGRATULATIONS!!"
        end

        print ( string.format("YOU HAVE PERFORMED THE TASK IN %d MOVES.\n", moves) )

        if not keep_playing() then
            break
        end
    end

    print "\nTHANKS FOR THE GAME!"
end

start_loop()
