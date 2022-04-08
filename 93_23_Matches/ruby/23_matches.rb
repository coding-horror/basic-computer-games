class Matches
    def initialize
        puts " " * 31 + "23 MATCHES"
        puts "This is a game called '23 Matches'."
        puts "When it is your turn, you may take one, two, or three"
        puts "matches. The object of the game is not to have to take"
        puts "the last match."
        puts "Let's flip a coin to see who goes first."
        puts "If it comes up heads, I will win the toss."

        while true
            play_game
            print "Play again? (yes or no) "
            answer = gets.chomp!.upcase
            break unless ["Y", "YES"].include? answer
        end
    end

    private
        def play_game
            matches = 23
            humans_turn = rand(0..1) == 1
            if humans_turn
                puts "Tails! You go first."
                prompt_human = "How many do you wish to remove? "
            else
                puts "Heads! I win! Ha! Ha!"
                puts "Prepare to lose, meatball-nose!!"
            end

            choice_human = 2

            while matches > 0
                if humans_turn
                    choice_human = 0
                    if matches == 1
                        choice_human = 1
                    end

                    while choice_human == 0
                        print "#{prompt_human}[1,2,3] "
                        choice_human = gets.chomp!

                        if ![1, 2, 3].include?(choice_human.to_i) || choice_human.to_i > matches
                            choice_human = 0
                            puts "Very funny! Dummy!"
                            puts "Do you want to play or goof around?"
                            prompt_human = "Now, how many matches do you want "
                        end
                    end

                    matches = matches - choice_human.to_i

                    if matches == 0
                        puts "You poor boob! You took the last match! I gotcha!!"
                        puts "Ha ! Ha ! I beat you !!"
                        puts "Good bye loser!"
                    else
                        puts "There are now #{matches} matches remaining."
                    end
                else
                    choice_computer = 4 - choice_human.to_i
                    if matches == 1
                        choice_computer = 1
                    elsif (1 < matches) && (matches < 4)
                        choice_computer = matches - 1
                    end

                    matches = matches - choice_computer
                    if matches == 0
                        puts "You won, floppy ears !"
                        puts "Think you're pretty smart !"
                        puts "Let's play again and I'll blow your shoes off !!"
                    else
                        puts "My turn ! I remove #{choice_computer} matches"
                        puts "The number of matches is now #{matches}"
                    end
                end

                humans_turn = !humans_turn
                prompt_human = "Your turn -- you may take 1, 2 or 3 matches.\nHow many do you wish to remove "
            end
        end
end

if __FILE__ == $0
    Matches.new
end