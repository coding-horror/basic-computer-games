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

def generate_number_array
  target = 0
  target = rand(100..999) while target.to_s.split('').uniq.size < 3
  target.to_s.split('')
end

def puts_clue_for(guess_array, target_array)
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
  target_number_array = generate_number_array

  2.times { puts }
  puts 'OK. I have a number in mind.'

  guess_count = 0
  guess_array = []

  while (guess_array != target_number_array) && guess_count < 20
    guess_count += 1

    puts "Guess ##{guess_count}:"

    guess = gets.chomp

    if guess =~ /[^1-9]/
      puts 'What?'
      next
    end

    if guess.length != 3
      puts 'Try guessing a three digit number'
      next
    end

    guess_array = guess.split('')

    if guess_array.uniq.size < 3
      puts 'Oh, I forgot to tell you: the number I have in mind has no two digits the same.'
      next
    end

    if guess_array != target_number_array
      puts_clue_for(guess_array, target_number_array)
      puts
    end
  end

  if guess_array == target_number_array
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
