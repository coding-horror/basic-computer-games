#A class representing the internal state of a single game of flip flop
# state represents the list of X's (A in the original code)
# guesses represents the number of guesses the user has made (C in the original code)
# seed represents the random seed for an instance of the game (Q in the original code)
Game = Struct.new(:state, :guesses, :seed) do

  #The original BASIC program used 1 indexed arrays while Ruby has 0-indexed arrays.
  #We can't use 0 indexed arrays for the flip functions or we'll get divide by zero errors.
  #These convenience functions allow us to modify and access internal game state in a 1-indexed fashion
  def flip_letter(letter_number)
    index = letter_number -1
    if self.state[index] == 'X'
      self.state[index] = 'O'
    else
      self.state[index] = 'X'
    end
  end

  def letter_at(letter_number)
    self.state[letter_number - 1]
  end
end

def print_welcome
  puts 'FLIPFLOP'.center(72)
  puts 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY'.center(72)
  puts <<~EOS

    THE OBJECT OF THIS PUZZLE IS TO CHANGE THIS:

    X X X X X X X X X X

    TO THIS:

    O O O O O O O O O O

    BY TYPING THE NUMBER CORRESPONDING TO THE POSITION OF THE
    LETTER ON SOME NUMBERS, ONE POSITION WILL CHANGE, ON
    OTHERS, TWO WILL CHANGE.  TO RESET LINE TO ALL X'S, TYPE 0
    (ZERO) AND TO START OVER IN THE MIDDLE OF A GAME, TYPE
    11 (ELEVEN).
  EOS
end

def print_starting_message
  puts <<~EOS

  HERE IS THE STARTING LINE OF X'S.

  1 2 3 4 5 6 7 8 9 10
  X X X X X X X X X X
  
  EOS
end

#Create a new game with [X,X,X,X,X,X,X,X,X,X] as the state
#0 as the number of guesses and a random seed between 0 and 1
def generate_new_game
  Game.new(Array.new(10, 'X'), 0, rand())
end

#Given a game, an index, and a shuffle function, flip one or more letters
def shuffle_board(game, index, shuffle_function)
  n = method(shuffle_function).call(game, index)

  if game.letter_at(n) == "O"
    game.flip_letter(n)
    if index == n
      n = shuffle_board(game, index, shuffle_function)
    end
  else
    game.flip_letter(n)
  end
  return n
end

#Shuffle logic copied from original BASIC code
def shuffle_function1(game, index) 
  r = Math.tan(game.seed + index / game.seed - index) - Math.sin(game.seed / index) + 336 * Math.sin(8 * index)
  n = r - r.floor
  (10 * n).floor
end

def shuffle_function2(game, index)
  r = 0.592 * (1/ Math.tan(game.seed / index + game.seed)) / Math.sin(index * 2 + game.seed) - Math.cos(index)
  n = r - r.floor
  (10 * n)
end

def play_game
  print_starting_message
  game = generate_new_game
  working_index = nil

  loop do
    puts "INPUT THE NUMBER"
    input = gets.chomp.downcase

    #See if the user input a valid integer, fail otherwise
    if numeric_input = Integer(input, exception: false)

      #If 11 is entered, we're done with this version of the game
      if numeric_input == 11
        return :restart
      end

      if numeric_input > 11
        puts 'ILLEGAL ENTRY--TRY AGAIN.'
        next #illegal entries don't count towards your guesses
      end

      if working_index == numeric_input
        game.flip_letter(numeric_input)
        working_index = shuffle_board(game, numeric_input, :shuffle_function2)
      #If 0 is entered, we want to reset the state, but not the random seed or number of guesses and keep playing
      elsif numeric_input == 0
        game.state = Array.new(10, 'X')
      elsif game.letter_at(numeric_input) == "O"
        game.flip_letter(numeric_input)
        if numeric_input == working_index
          working_index = shuffle_board(game, numeric_input, :shuffle_function1)
        end
      else
        game.flip_letter(numeric_input)
        working_index = shuffle_board(game, numeric_input, :shuffle_function1)
      end
    else
      puts 'ILLEGAL ENTRY--TRY AGAIN.'
      next #illegal entries don't count towards your guesses
    end

    game.guesses += 1
    puts '1 2 3 4 5 6 7 8 9 10'
    puts game.state.join(' ')
    
    if game.state.all? { |x| x == 'O' }
      if game.guesses > 12 
        puts "TRY HARDER NEXT TIME. IT TOOK YOU #{game.guesses} GUESSES."
      else
        puts "VERY GOOD.  YOU GUESSED IT IN ONLY #{game.guesses} GUESSES."
      end
      #game is complete
      return
    end
  end
end



#Execution starts
print_welcome
loop do
  result = play_game
  if result == :restart
    next
  end

  puts 'DO YOU WANT TO TRY ANOTHER PUZZLE'
  if gets.chomp.downcase[0] == 'n'
    break
  end
end
