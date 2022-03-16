#!/usr/bin/env ruby

# Kinema
# reinterpreted from BASIC by stephan.com

puts 'Letter'.center(80)
puts 'Adapted by stephan.com'.center(80)
puts "\n\n\n"

puts "Letter guessing game\n\n"

puts "I'll think of a letter of the alphabet, A to Z."
puts "Try to guess my letter and I'll give you clues"
puts "as to how close you're getting to my letter."

def win(turns)
  puts "\nyou got it in #{turns} guesses!!"
  return puts "but it shouldn't take more than 5 guesses!" if turns > 5

  puts "good job !!!!!\a\a\a"
end

def play
  letter = ('A'..'Z').to_a.sample
  guess = nil
  turn = 0

  puts "\nO.K., I have a letter.  Start guessing."

  until guess == letter
    puts "\nWhat is your guess?"

    guess = gets.strip.chars.first.upcase
    turn += 1

    puts 'Too low.  Try a higher letter.' if guess < letter
    puts 'Too high.  Try a lower letter.' if guess > letter
  end
  win(turn)
end

loop do
  play
  puts "\nlet's play again....."
end
