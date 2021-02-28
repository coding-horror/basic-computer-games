# Bagels
# Number guessing game.
# Original source unknown but suspected to be
# Lawrence Hall of Science, U.C. Berkely

def print_instructions
  puts
  puts 'I am thinking of a three-digit number. Try to guess'
  puts 'my number and i will give you clues as follows:'
  puts '   PICO   - one digit correct but in the wrong position'
  puts '   FERMI  - one digit correct and in the right position'
  puts '   BAGELS - no digits correct'
end

def generate_target
  target = rand(100..999) while target.to_s.split('').uniq.size < 3
  target
end

def puts_clue_for(guess_number, target_number)
  guess_array = guess_number.to_s.split('')
  target_array = target_number.to_s.split('')

  clues = [].tap do |clue|
    guess_array.each_with_index do |n, i|
      if target_array[i] == n
        clue << 'FERMI'
      elsif target_array.include?(n)
        clue << 'PICO'
      end
    end
  end

  # sort clues so that FERMIs come before PICOs, but
  # you don't know which response responds to which number
  puts clues.length > 0 ? clues.sort.join(' ') : 'BAGELS'
end

player_points = 0
desire_to_play = true

puts 'Bagels'.center(72)
puts 'Creative Computing  Morristown, New Jersey'.center(72)

5.times { puts }

puts 'Would you like to the rules? [Y]es or [N]o.'
instructions_request = gets.chomp.downcase

print_instructions if %w[yes y].include?(instructions_request)

while desire_to_play
  target = generate_target

  2.times { puts }
  puts 'OK. I have a number in mind.'

  puts target

  guess_count = 0
  guess = 0

  while (guess != target) && guess_count < 20
    guess_count += 1

    puts "Guess ##{guess_count}:"

    raw_guess = gets.chomp

    if raw_guess =~ /[^0-9]/
      puts 'What?'
      next
    end

    if raw_guess.length != 3
      puts 'Try guessing a three digit number'
      next
    end

    if raw_guess.split('').uniq.size < 3
      puts 'Oh, I forgot to tell you: the number I have in mind has no two digits the same.'
      next
    end

    guess = raw_guess.to_i

    if guess != target
      puts_clue_for(guess, target)
      puts
    end
  end

  if guess == target
    player_points += 1

    puts 'You got it!!!'
    puts
  else
    puts 'Oh well.'
    puts "That's twenty guesses. My number was #{target_number_array.join('')}."
  end

  puts
  puts 'Would you like to play again? [Y]es or [N]o'
  play_again_request = gets.chomp
  desire_to_play = %w[yes y].include?(play_again_request)
end

puts "A #{player_points} point bagels buff!!"
puts 'Hope you had fun. Bye.'
