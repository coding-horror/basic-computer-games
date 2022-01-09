require 'strscan'

# Prints a number according to Vintage Basic's PRINT statement
# @param n The number to print
def print_number(n)
  # PRINT adds padding after a number and before a positive number
  print ' ' if n >= 0
  print n.to_s
  print ' '
end

# Mimic the INPUT statement using Vintage Basic as a reference
# @param prompt The prompt to show to the user
# @return An array of strings representing the inputted values
def input(prompt)
  prompt_suffix = '? '
  print "#{prompt}#{prompt_suffix}"

  input = gets.chomp.strip
  scanner = StringScanner.new(input)
  input_values = []

  until scanner.eos?
    scanner.scan(/\s+/)

    if scanner.check(/"/)
      scanner.scan(/"/)
      next_string = scanner.scan_until(/"/)

      if next_string
        # Remove the trailing close quote
        next_string.chomp!('"')
      else
        # No close quote – Vintage Basic crashes in this case
        raise ArgumentError('Unmatched quotes in input')
      end
    elsif scanner.exist?(/,/)
      next_string = scanner.scan_until(/,/).chomp(',')
    else
      next_string = scanner.scan_until(/\s+|$/).rstrip
    end

    input_values << next_string
  end

  input_values << '' if input_values.empty?

  input_values
end

class Game
  def initialize(history, non_win_count)
    @beans = Array.new(13, 3)
    @beans[6] = 0
    @beans[13] = 0

    @turn_counter = 0

    @history = history
    @non_win_count = non_win_count
  end

  # @return [Boolean] True if the computer did not win the game
  def play
    while true
      print_beans

      move = get_move("YOUR MOVE")
      home_pit = 6
      computer_home_pit = 13

      last_pit = perform_move(move, home_pit)

      print_beans

      break if game_over

      if home_pit == last_pit
        second_move = get_move("AGAIN")

        perform_move(second_move, home_pit)

        print_beans

        break if game_over
      end

      computer_move, computer_last_pit = get_computer_move
      print "MY MOVE IS #{computer_move - 6}"

      break if game_over

      if computer_last_pit == computer_home_pit
        second_computer_move, _ = get_computer_move
        print ",#{second_computer_move - 6}"

        break if game_over
      end
    end

    end_game
  end

  private

  def game_over
    @beans[0...6].all? { |b| b == 0 } || @beans[7...13].all? { |b| b == 0 }
  end

  # @return [Boolean] True if the computer did not win
  def end_game
    puts
    puts "GAME OVER"

    difference = @beans[6] - @beans[13]

    if difference < 0
      puts "I WIN BY #{-difference} POINTS"

      return
    end

    puts "YOU WIN BY #{difference} POINTS" if difference > 0
    puts "DRAWN GAME" if difference == 0

    difference >= 0
  end

  # @param [Integer] move
  # @param [Integer] home_pit
  def perform_move(move, home_pit)
    last_pit = distribute_beans(move, home_pit)

    update_history(move)

    last_pit
  end

  def update_history(current_move)
    k = current_move % 7
    @turn_counter += 1

    # Add the move to the history
    @history[@non_win_count] = @history[@non_win_count] * 6 + k if @turn_counter < 9
  end

  def print_beans
    puts

    # Print computer beans
    print ' ' * 3
    @beans[7...13].reverse.each { |bean_count| print_bean(bean_count) }
    puts

    # Print home beans
    print_bean(@beans[13])
    print ' ' * 23
    print_number(@beans[6]) # This is not print_bean in line with the original version
    puts

    # Print player beans
    print ' ' * 3
    @beans[0...6].each { |bean_count| print_bean(bean_count) }
    puts

    puts
  end

  def get_move(prompt)
    move = get_integer_input(prompt)

    while move < 1 || move > 6 || @beans[move - 1] == 0
      puts "ILLEGAL MOVE"
      move = get_integer_input("AGAIN")
    end

    move - 1
  end

  def distribute_beans(start_pit, home_pit, beans = @beans)
    beans_to_distribute = beans[start_pit]
    beans[start_pit] = 0

    current_pit = start_pit

    (0...beans_to_distribute).each do
      current_pit = (current_pit + 1) % beans.size
      beans[current_pit] += 1
    end

    # If the last pit was empty before we put a bean in it (and it's not a scoring pit), add beans to score
    if beans[current_pit] == 1 && current_pit != 6 && current_pit != 13 && beans[12 - current_pit] != 0
      beans[home_pit] = beans[home_pit] + beans[12 - current_pit] + 1
      beans[current_pit] = 0
      beans[12 - current_pit] = 0
    end

    current_pit
  end

  def print_bean(bean_count)
    print ' ' if bean_count < 10
    print_number(bean_count)
  end

  def get_integer_input(prompt)
    integer_value = nil

    input_values = input(prompt)

    while integer_value.nil?
      print '!EXTRA INPUT IGNORED' if (input_values.size > 1)

      value = input_values.first

      begin
        integer_value = Integer(value)
      rescue
        puts '!NUMBER EXPECTED - RETRY INPUT LINE'
        input_values = input('')
      end
    end

    integer_value
  end

  def get_computer_move
    d = -99
    home_pit = 13

    chosen_move = 7

    # Test all possible moves
    (7...13).each do |move_under_test|
      # Create a copy of the beans to test against
      beans_copy = @beans.dup

      # If the move is not legal, skip it
      next if beans_copy[move_under_test] == 0

      # Determine the best response the player may make to this move
      player_max_score = 0

      # Make the move under test against the copy
      distribute_beans(move_under_test, home_pit, beans_copy)

      # Test every player response
      (0...6).each do |i|
        # Skip the move if it would be illegal
        next if beans_copy[i] == 0

        # Determine the last
        landing_with_overflow = beans_copy[i] + i
        # If landing > 13 the player has put a bean in both home pits
        player_move_score = (landing_with_overflow > 14) ? 1 : 0
        # Find the actual pit
        landing = landing_with_overflow % 14

        # If the landing pit is empty, the player will steal beans
        if beans_copy[landing] == 0 && landing != 6 && landing != 13
          player_move_score = beans_copy[12 - landing] + player_move_score
        end

        # Update the max score if this move is the best player move
        player_max_score = player_move_score if player_move_score > player_max_score
      end

      # Final score for move is computer score, minus the player's score and any player gains from their best move
      final_score = beans_copy[13] - beans_copy[6] - player_max_score

      if @turn_counter < 9
        k = move_under_test % 7

        (0...@non_win_count).each do |i|
          # Penalise move if it was used in a losing game
          final_score = final_score - 2 if @history[@non_win_count] * 6 + k == ((Float(@history[i]) / 6 ** (7 - @turn_counter)) + 0.1).floor
        end
      end

      # Choose the move if it is the best move found so far
      if final_score >= d
        chosen_move = move_under_test
        d = final_score
      end
    end

    last_pit = perform_move(chosen_move, home_pit)

    [chosen_move, last_pit]
  end
end

puts 'AWARI'.center(80)
puts 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY'.center(80)

# Initialise stable variables
history = Array.new(50)
non_win_count = 0

# APPLICATION LOOP
while true
  puts
  puts

  history[non_win_count] = 0

  game = Game.new(history, non_win_count)

  computer_didnt_win = game.play
  non_win_count += 1 if computer_didnt_win
end
