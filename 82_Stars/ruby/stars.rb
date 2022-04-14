class Stars
  MAX_NUM = 100
  MAX_GUESSES = 7

  def start
    print "Do you want instructions? (Y/N) "
    response = gets.chomp!

    if response.upcase[0] == "Y"
      print_instructions()
    end

    still_playing = true
    while still_playing
      secret_number = rand(1..MAX_NUM)
      puts "\n\nOK, I am thinking of a number, start guessing."

      guess_number = 0
      player_has_won = false

      while (guess_number < MAX_GUESSES) and not player_has_won
        puts "\n"
        guess = get_guess()
        guess_number += 1

        if guess == secret_number
          player_has_won = true
            puts "**************************************************!!!"
            puts "You got it in #{guess_number} guesses!!!\n\n"
        else
          print_stars(secret_number, guess)
        end
      end

      if not player_has_won
        puts "\nSorry, that's #{guess_number} guesses, number was #{secret_number}\n"
      end

      print "Play again? (Y/N) "
      response = gets.chomp!

      if response.upcase[0] != "Y"
        still_playing = false
      end
      
    end
  end

  private
    def print_instructions
      puts "I am thinking of a whole number from 1 to #{MAX_NUM}"
      puts "Try to guess my number.  After you guess, I"
      puts "will type one or more stars (*).  The more"
      puts "stars I type, the closer you are to my number."
      puts "one star (*) means far away, seven stars (*******)"
      puts "means really close!  You get #{MAX_GUESSES} guesses."
    end

    def get_guess
      valid_response = false
      while not valid_response
        print "Your guess? "
        guess = gets.chomp!

        if guess.match?(/[[:digit:]]/)
          valid_response = true
          guess = guess.to_i
        end
      end

      return guess
    end

    def print_stars secret_number, guess
      diff = (guess - secret_number).abs
      stars = ""
      for i in 0..7
        if diff < 2**i
          stars += "*"
        end
      end
      print(stars)
    end
end


if __FILE__ == $0
  stars = Stars.new
  stars.start()
end