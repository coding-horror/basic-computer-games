#!/usr/bin/ruby

class DepthCharge

  def run_game
    output_title()
    while true
      printf("----------\n")
      print_instructions()
      setup_game()
      printf("\n")
      game_loop()
      break if ! get_input_another_game()
    end

    # 420 PRINT "OK.  HOPE YOU ENJOYED YOURSELF." : GOTO 600
    printf("OK.  HOPE YOU ENJOYED YOURSELF.\n")
  end

  def output_title
    printf("--- DEPTH CHARGE ---\n")
    printf("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")
    printf("\n")
  end

  def get_input_y_or_n(message)
    while true
      print(message)

      value = gets.chomp

      if (value == 'Y' || value == 'y')
        return true
      elsif value == 'N' || value == 'n'
        return false
      end

      printf("PLEASE ENTER Y/y OR N/n...\n\n")
    end
  end

  def get_input_positive_integer(message)

    while true
      print(message)
      value = gets.chomp
      if (value == 'd')
        debug_game()
        next
      end

      the_input = Integer(value) rescue nil

      if the_input == nil || the_input < 0
        printf("PLEASE ENTER A POSITIVE NUMBER\n\n")
        next

      end

      return the_input
    end
  end

  def get_search_area_dimension
    # 20 INPUT "DIMENSION OF SEARCH AREA";G: PRINT
    @search_area_dimension = get_input_positive_integer("DIMENSION OF SEARCH AREA: ")
    # 30 N=INT(LOG(G)/LOG(2))+1

    @num_tries = Integer(
      Math.log(@search_area_dimension)/Math.log(2)
    )

  end

  def print_instructions
    # 40 PRINT "YOU ARE THE CAPTAIN OF THE DESTROYER USS COMPUTER"
    # 50 PRINT "AN ENEMY SUB HAS BEEN CAUSING YOU TROUBLE.  YOUR"
    # 60 PRINT "MISSION IS TO DESTROY IT.  YOU HAVE";N;"SHOTS."
    # 70 PRINT "SPECIFY DEPTH CHARGE EXPLOSION POINT WITH A"
    # 80 PRINT "TRIO OF NUMBERS -- THE FIRST TWO ARE THE"
    # 90 PRINT "SURFACE COORDINATES; THE THIRD IS THE DEPTH."
    # 100 PRINT : PRINT "GOOD LUCK !": PRINT
    printf( <<~INSTRUCTIONS
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
    )
  end

  def debug_game
    printf("@enemy_x: %d\n", @enemy_x)
    printf("@enemy_y: %d\n", @enemy_y)
    printf("@enemy_z: %d\n", @enemy_z)
    printf("@num_tries: %d\n", @num_tries)
    printf("@trial: %d\n", @trial)
    printf("\n")
  end

  def setup_game
    get_search_area_dimension()
    setup_enemy()
  end

  def setup_enemy
    # 110 A=INT(G*RND(1)) : B=INT(G*RND(1)) : C=INT(G*RND(1))
    @enemy_x = rand(1..@search_area_dimension)
    @enemy_y = rand(1..@search_area_dimension)
    @enemy_z = rand(1..@search_area_dimension)
    end

  def game_loop
    # 120 FOR D=1 TO N : PRINT : PRINT "TRIAL #";D; : INPUT X,Y,Z
    for @trial in 1..@num_tries do
      output_game_status()

      @shot_x = get_input_positive_integer("X: ")
      @shot_y = get_input_positive_integer("Y: ")
      @shot_z = get_input_positive_integer("Z: ")

      # 130 IF ABS(X-A)+ABS(Y-B)+ABS(Z-C)=0 THEN 300
      if (
        (@enemy_x - @shot_x).abs \
        + (@enemy_y - @shot_y).abs \
        + (@enemy_z - @shot_z).abs \
        == 0
      )
        you_win()
        return
      else
        # 140 GOSUB 500 : PRINT : NEXT D
        missed_shot()
      end
    end

    printf("\n")

    you_lose()

  end

  def output_game_status
    printf("YOU HAVE %d SHOTS REMAINING.\n", @num_tries - @trial + 1)
    printf("TRIAL \#%d\n", @trial)
  end
  def you_win
    printf("B O O M ! ! YOU FOUND IT IN %d TRIES!\n\n", @trial )
  end
  def missed_shot
    missed_directions = []

    # 530 IF X>A THEN PRINT "EAST";
    # 540 IF X<A THEN PRINT "WEST";
    if @shot_x > @enemy_x
      missed_directions.push('TOO FAR EAST')
    elsif @shot_x < @enemy_x
      missed_directions.push('TOO FAR WEST')
    end

    # 510 IF Y>B THEN PRINT "NORTH";
    # 520 IF Y<B THEN PRINT "SOUTH";
    if @shot_y > @enemy_y
      missed_directions.push('TOO FAR NORTH')
    elsif @shot_y < @enemy_y
      missed_directions.push('TOO FAR SOUTH')
    end

    # 560 IF Z>C THEN PRINT " TOO LOW."
    # 570 IF Z<C THEN PRINT " TOO HIGH."
    # 580 IF Z=C THEN PRINT " DEPTH OK."
    if @shot_z > @enemy_z
      missed_directions.push('TOO DEEP')
    elsif @shot_z < @enemy_z
      missed_directions.push('TOO SHALLOW')
    end

    # 500 PRINT "SONAR REPORTS SHOT WAS ";
    printf("SONAR REPORTS SHOT WAS: \n")
    printf("%s\n", "\t" + missed_directions.join("\n\t"))
    # 550 IF Y<>B OR X<>A THEN PRINT " AND";
    # 590 RETURN
  end

  def you_lose
    # You took too long!
    printf("YOU HAVE BEEN TORPEDOED!  ABANDON SHIP!\n")
    printf("THE SUBMARINE WAS AT %d %d %d\n", @enemy_x, @enemy_y, @enemy_z)

  end

  def get_input_another_game
    # 400 PRINT : PRINT: INPUT "ANOTHER GAME (Y OR N)";A$
    return get_input_y_or_n("ANOTHER GAME (Y OR N): ")
    # 410 IF A$="Y" THEN 100
  end
end

game = DepthCharge.new
game.run_game()
