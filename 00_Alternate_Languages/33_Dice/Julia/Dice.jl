#=
Port of Dice from BASIC Computer Games (1978)

This "game" simulates a given number of dice rolls, and returns the 
count for each possible total.

The only change that has been made from the original program, is that 
when asking if the user wants to play again, any string starting with
y or Y will be accepted, instead of only YES.
=#

function main()
    # Array to store the counts for each total.
    # There are 11 possible totals.
    counts = [0 for i in 1:11]

    # Print intro text
    println("\n                   Dice")
    println("Creative Computing  Morristown, New Jersey")
    println("\n\n")
    println("This program simulates the rolling of a")
    println("pair of dice.")
    println("You enter the number of times you want the computer to")
    println("'roll' the dice.   Watch out, very large numbers take")
    println("a long time.  In particular, numbers over 5000.")

    still_playing = true
    while still_playing
        println()
        print("How many rolls? ")

        # Get user input for number of dice rolls
        rolls = readline()
        rolls = parse(Int64, rolls)

        # Roll dice the specified number of times and update total count
        for _ in 1:rolls
            dice_roll = rand(1:6, 2)
            dice_sum = sum(dice_roll)

            # The index is one less than the sum, as a sum of 1 is impossible,
            # the array will only have 11 values
            counts[dice_sum-1] += 1
        end

        # Display results
        println("\nTotal Spots   Number of Times")
        for i in 1:8
            print(" ")
            print(i+1)
            print("             ")
            println(counts[i])
        end
        for i in 9:11
            print(" ")
            print(i+1)
            print("            ")
            println(counts[i])
        end

        # Ask try again
        print("\nTry Again? ")
        input = readline()
        if length(input) > 0 && uppercase(input)[1] == 'Y'
             # If game is continued, resets total counts
            counts = [0 for i in 1:11]
        else
            still_playing = false
        end
    end
end

if abspath(PROGRAM_FILE) == @__FILE__
    main()
end