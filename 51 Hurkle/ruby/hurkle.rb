MAX_GUESSES = 5
GRID_SIZE = 10

class Point < Object
  attr_accessor :x
  attr_accessor :y

  def initialize(text="")
    x, y = text.split(",").map(&:strip)
    @x = (x || rand(GRID_SIZE).floor).to_i
    @y = (y || rand(GRID_SIZE).floor).to_i
  end

  def to_s
    "#{@x}, #{@y}"
  end

  def ==(other_point)
    @x == other_point.x && @y == other_point.y
  end

  def direction_to(other_point)
    (  @y < other_point.y ? "NORTH" : "SOUTH" unless @y == other_point.y ).to_s +
      (@x < other_point.x ? "EAST"  : "WEST"  unless @x == other_point.x ).to_s
  end
end

def main
  say_introduction

  loop do
    hurkle_point = Point.new
    found = false
    (1..MAX_GUESSES).each do |guess_num|
      found = guess(hurkle_point, guess_num)
      break if found
    end
    say_failure(hurkle_point) if not found
    say_play_again
  end

end

def say_introduction
  puts " " * 33 + "HURKLE"
  puts " " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
  3.times { puts }
  puts
  puts "A HURKLE IS HIDING ON A #{GRID_SIZE} BY #{GRID_SIZE} GRID. HOMEBASE"
  puts "ON THE GRID IS POINT 0,0 IN THE SOUTHWEST CORNER,"
  puts "AND ANY POINT ON THE GRID IS DESIGNATED BY A"
  puts "PAIR OF WHOLE NUMBERS SEPERATED BY A COMMA. THE FIRST"
  puts "NUMBER IS THE HORIZONTAL POSITION AND THE SECOND NUMBER"
  puts "IS THE VERTICAL POSITION. YOU MUST TRY TO"
  puts "GUESS THE HURKLE'S GRIDPOINT. YOU GET #{MAX_GUESSES} TRIES."
  puts "AFTER EACH TRY, I WILL TELL YOU THE APPROXIMATE"
  puts "DIRECTION TO GO TO LOOK FOR THE HURKLE."
  puts
end

def guess(hurkle_point, guess_num)
  print "GUESS # #{guess_num} ? "
  guess_point = Point.new(gets.chomp)
  if guess_point == hurkle_point
    say_success(guess_num)
    return true
  end
  say_where_to_go(hurkle_point, guess_point)
  false
end

def say_success(guess_num)
  puts
  puts "YOU FOUND IT IN #{guess_num} GUESSES!"
end

def say_where_to_go(hurkle_point, guess_point)
  puts "GO #{guess_point.direction_to(hurkle_point)}"
  puts
end

def say_failure(hurkle_point)
  puts
  puts "SORRY, THAT'S " + MAX_GUESSES.to_s + " GUESSES."
  puts "THE HURKLE IS AT #{hurkle_point}"
end

def say_play_again
  puts
  puts "LET'S PLAY AGAIN, HURKLE IS HIDING."
  puts
end

main
