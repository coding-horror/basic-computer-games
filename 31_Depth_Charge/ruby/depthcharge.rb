#!/usr/bin/ruby

class DepthCharge
  def run_game
    output_title

    loop do
      puts "----------"
      print_instructions
      setup_game
      puts
      game_loop
      break unless get_input_another_game
    end

    puts "OK.  HOPE YOU ENJOYED YOURSELF."
  end

  def output_title
    puts "--- DEPTH CHARGE ---"
    puts "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    puts
  end

  def get_input_y_or_n(message)
    loop do
      print message

      value = gets.chomp

      if value.downcase == "y"
        return true
      elsif value.downcase == "n"
        return false
      end

      puts "PLEASE ENTER Y/y OR N/n..."
      puts
    end
  end

  def get_input_positive_integer(message)
    loop do
      print message
      value = gets.chomp

      if value == "d"
        debug_game
        next
      end

      the_input = Integer(value) rescue 0

      if the_input < 1
        puts "PLEASE ENTER A POSITIVE NUMBER"
        puts
        next
      end

      return the_input
    end
  end

  def print_instructions
    puts <<~INSTRUCTIONS
      YOU ARE THE CAPTAIN OF THE DESTROYER USS COMPUTER
      AN ENEMY SUB HAS BEEN CAUSING YOU TROUBLE.  YOUR
      MISSION IS TO DESTROY IT.

      SPECIFY DEPTH CHARGE EXPLOSION POINT WITH A
      TRIO OF NUMBERS -- THE FIRST TWO ARE THE
      SURFACE COORDINATES (X, Y):
          WEST < X < EAST
          SOUTH < Y < NORTH

      THE THIRD IS THE DEPTH (Z):
        SHALLOW < Z < DEEP

      GOOD LUCK !

    INSTRUCTIONS
  end

  def debug_game
    puts "@enemy_x: %d" % @enemy_x
    puts "@enemy_y: %d" % @enemy_y
    puts "@enemy_z: %d" % @enemy_z
    puts "@num_tries: %d" % @num_tries
    puts "@trial: %d" % @trial
    puts
  end

  def setup_game
    @search_area_dimension = get_input_positive_integer("DIMENSION OF SEARCH AREA: ")

    @num_tries = Integer(Math.log(@search_area_dimension) / Math.log(2) + 1)
    setup_enemy
  end

  def setup_enemy
    @enemy_x = rand(1..@search_area_dimension)
    @enemy_y = rand(1..@search_area_dimension)
    @enemy_z = rand(1..@search_area_dimension)
  end

  def game_loop
    for @trial in 1..@num_tries do
      output_game_status()

      @shot_x = get_input_positive_integer("X: ")
      @shot_y = get_input_positive_integer("Y: ")
      @shot_z = get_input_positive_integer("Z: ")


      distance = (@enemy_x - @shot_x).abs +
        (@enemy_y - @shot_y).abs +
        (@enemy_z - @shot_z).abs

      if distance == 0
        you_win
        return
      else
        missed_shot
      end
    end

    puts

    you_lose
  end

  def output_game_status
    puts "YOU HAVE %d SHOTS REMAINING." % @num_tries - @trial + 1
    puts "TRIAL \#%d" % @trial
  end

  def you_win
    puts "\nB O O M ! ! YOU FOUND IT IN %d TRIES!" % @trial
    puts
  end

  def missed_shot
    missed_directions = []

    if @shot_x > @enemy_x
      missed_directions.push('TOO FAR EAST')
    elsif @shot_x < @enemy_x
      missed_directions.push('TOO FAR WEST')
    end

    if @shot_y > @enemy_y
      missed_directions.push('TOO FAR NORTH')
    elsif @shot_y < @enemy_y
      missed_directions.push('TOO FAR SOUTH')
    end

    if @shot_z > @enemy_z
      missed_directions.push('TOO DEEP')
    elsif @shot_z < @enemy_z
      missed_directions.push('TOO SHALLOW')
    end

    puts "SONAR REPORTS SHOT WAS: "
    puts "\t#{missed_directions.join("\n\t")}"
  end

  def you_lose
    puts "YOU HAVE BEEN TORPEDOED!  ABANDON SHIP!"
    puts "THE SUBMARINE WAS AT %d %d %d" % [@enemy_x, @enemy_y, @enemy_z]
  end

  def get_input_another_game
    return get_input_y_or_n("ANOTHER GAME (Y OR N): ")
  end
end

game = DepthCharge.new
game.run_game
