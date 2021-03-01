#!/usr/bin/env ruby
# WORD
#
# Converted from BASIC to Ruby


WORDS = ["DINKY", "SMOKE", "WATER", "GRASS", "TRAIN", "MIGHT",
         "FIRST","CANDY", "CHAMP", "WOULD", "CLUMP", "DOPEY"]

def game_loop
  target_word = WORDS.sample.downcase
  guess_count = 0
  guess_progress = ["-"] * 5

  puts "You are starting a new game..."
  while true
    guess_word = ""
    while guess_word == ""
      puts "Guess a five letter word. "
      guess_word = gets.chomp
      if guess_word == "?"
        break
      elsif !guess_word.match(/^[[:alpha:]]+$/) || guess_word.length != 5
        guess_word = ""
        puts "You must guess a five letter word. Start again."
      end
    end
    guess_count += 1
    if guess_word == "?"
      puts "The secret word is #{target_word}"
      break
    else
      common_letters = ""
      matches = 0
      5.times do |i|
        5.times do |j|
          if guess_word[i] == target_word[j]
            matches += 1
            common_letters = common_letters + guess_word[i]
            guess_progress[j] = guess_word[i] if i == j
          end
        end
      end
      puts "There were #{matches} matches and the common letters were... #{common_letters}"
      puts "From the exact letter matches, you know............ #{guess_progress.join}"
      if guess_progress.join == guess_word
        puts "You have guessed the word. It took #{guess_count} guesses!"
        break
      elsif matches < 2
        puts "If you give up, type '?' for you next guess."
      end
    end
  end
end

puts " " * 33 + "WORD"
puts " " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n"
puts "I am thinking of a word -- you guess it. I will give you"
puts "clues to help you get it. Good luck!!\n"

keep_playing = true
while keep_playing
  game_loop
  puts "\n Want to play again? "
  keep_playing = gets.chomp.downcase.index("y") == 0
end
