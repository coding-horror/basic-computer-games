puts <<~INSTRUCTIONS
                            RUSSIAN ROULETTE
               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



THIS IS A GAME OF >>>>>>>>>>RUSSIAN ROULETTE.

HERE IS A REVOLVER.

INSTRUCTIONS

NUMBER_OF_ROUNDS = 9

def parse_input
    correct_input = false

    while not correct_input
        puts " ?"
        inp = gets.chomp
        if inp == "1" or inp == "2"
            correct_input = true
        end
    end

    inp
end

while true

    dead = false
    n = 0

    puts "TYPE \'1\' TO SPIN CHAMBER AND PULL TRIGGER"
    puts "TYPE \'2\' TO GIVE UP"
    puts "GO"

    while not dead

        inp = parse_input

        if inp == "2"
            break
        end

        if rand > 0.8333333333333334
            dead = true
        else
            puts "- CLICK -"
            n += 1
        end

        if n > NUMBER_OF_ROUNDS
            break
        end

    end

    if dead
        puts "BANG!!!!!   You're Dead!"
        puts "Condolences will be sent to your relatives.\n\n\n"
        puts "...Next victim..."
    else
        if n > NUMBER_OF_ROUNDS
            puts "You win!!!!!"
            puts "Let someone else blow his brain out.\n"
        else
            puts "     Chicken!!!!!\n\n\n"
            puts "...Next victim...."
        end
    end

end
