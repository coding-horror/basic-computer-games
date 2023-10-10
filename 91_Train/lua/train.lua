print [[
            TRAIN
 CREATIVE COMPUTING  MORRISTOWN, NEW JERSY



 TIME - SPEED DISTANCE EXERCISE]]

math.randomseed(os.time())

local ERROR_MARGIN <const> = 5.0

function play()
    local car_speed = 25*math.random() + 40--Between 40 and 64
    local delta_time = 15*math.random() + 5--Between 5 and 19
    local train_speed = 19*math.random() + 20--Between 20 and 38

    print( string.format("\nA CAR TRAVELING AT %u MPH CAN MAKE A CERTAIN TRIP IN %u HOURS LESS THAN A TRAIN TRAVELING AT %u MPH.", car_speed, delta_time, train_speed) )

    local try = true
    local input
    while try do
        io.write("HOW LONG DOES THE TRIP TAKE BY CAR? ")
        input = io.read("n")
        if input == nil then
            print("<!>PLEASE INSERT A NUMBER<!>")
        else
            try = false
        end
        io.read()
    end

    local car_time = delta_time * train_speed / (car_speed - train_speed)
    local percent = ( math.abs(car_time-input) * 100 / car_time + .5)

    if percent > ERROR_MARGIN then
        print( string.format("SORRY. YOU WERE OFF BY %f PERCENT.", percent) )
    else
        print( string.format("GOOD! ANSWER WITHIN %f PERCENT.", percent) )
    end
    
    print( string.format("CORRECT ANSWER IS %f HOURS.", car_time) )
end

function game_loop()
    local keep_playing = true
    while keep_playing do
        play()
        io.write("\nANOTHER PROBLEM (YES OR NO)? ")
        answer = io.read("l")
        
        if not (answer == "YES" or answer == "Y" or answer == "yes" or answer == "y") then
            keep_playing = false
        end
    end

end

game_loop()