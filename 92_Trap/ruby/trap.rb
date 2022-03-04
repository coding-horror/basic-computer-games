#!/usr/bin/env ruby

# Trap
# Steve Ullman, 1972-08-01
# Ruby version Glenn Vanderburg, 2022-03-04

# Change these values to make the game easier or harder.
GUESSES_PER_GAME = 6
NUMBER_UPPER_BOUND = 100

# Put everything in methods for order of presentation; we
# want to be able to refer to methods before declaring them,
# so the code reads nicely from top to bottom.
def main
  print_banner_and_instructions

  loop do
    play_a_game
    break unless yes?("Try again?")
  end
end

def yes?(prompt)
  print "\n#{prompt} "
  answer = gets
  return answer.downcase.start_with?("y")
end

def print_banner_and_instructions
  banner = "Creative Computing -- Morristown, New Jersey"

  puts "Trap!".center(banner.size)
  puts banner
  2.times { puts }

  return unless yes?("Instructions?")

  puts <<~"END"
    I am thinking of a number between 1 and #{NUMBER_UPPER_BOUND}.
    Try to guess my numbrer. On each guess,
    you are to enter 2 numbers, trying to trap
    my number between the two numbers. I will
    tell you if you have trapped my number, if my
    number is larger than your two numbers, or if
    my number is smaller than your two numbers.
    If you want to guess one single number, type
    your guess for both your trap numbers.
    You get #{GUESSES_PER_GAME} guesses to get my number.
  END
end

def play_a_game
  n = choose_number

  GUESSES_PER_GAME.times do |i|
    lower, upper = get_guesses("Guess ##{i+1}?")

    case
    when lower == upper && lower == n
      puts "You got it!!!"
      return
    when n < lower
      puts "My number is smaller than your trap numbers."
    when n > upper
      puts "My number is larger than your trap numbers."
    else
      puts "You have trapped my number"
    end
  end

  puts "Sorry, that's #{GUESSES_PER_GAME} guesses. Number was #{n}"
end

def choose_number
  rand(NUMBER_UPPER_BOUND) + 1
end

def get_guesses(prompt)
  loop do
    print "\n#{prompt} "

    # This is forgiving of input format; it ignores spaces and
    # punctuation, returning only the strings of consecutive
    # digits in the input line.
    guesses = gets.scan(/\d+/)

    if guesses.size != 2
      puts "Please enter two numbers for each guess."
    else
      # convert the strings of digits to integers:
      numbers = guesses.map(&:to_i)
      # and return them, lowest number first:
      return numbers.sort
    end
  end
end

main
