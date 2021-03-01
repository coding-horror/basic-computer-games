MAX_GUESSES = 5
GRID_SIZE = 10

def main
  say_introduction

  loop do
    $a = rand(GRID_SIZE).floor
    $b = rand(GRID_SIZE).floor
    found = false
    (1..MAX_GUESSES).each do |k|
      print "GUESS # " + k.to_s + " "
      print "? "
      x, y = gets.chomp.split(",").map(&:to_i)
      if (x-$a).abs + (y-$b).abs == 0
        say_success(k)
        found = true
        break
      end
      say_where_to_go(x, y)
      puts
    end
    say_failure if not found
    say_play_again
  end
end

def say_introduction
  puts " " * 33 + "HURKLE"
  puts " " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
  3.times { puts }
  puts
  puts "A HURKLE IS HIDING ON A " + GRID_SIZE.to_s + " BY " + GRID_SIZE.to_s + " GRID. HOMEBASE"
  puts "ON THE GRID IS POINT 0,0 IN THE SOUTHWEST CORNER,"
  puts "AND ANY POINT ON THE GRID IS DESIGNATED BY A"
  puts "PAIR OF WHOLE NUMBERS SEPERATED BY A COMMA. THE FIRST"
  puts "NUMBER IS THE HORIZONTAL POSITION AND THE SECOND NUMBER"
  puts "IS THE VERTICAL POSITION. YOU MUST TRY TO"
  puts "GUESS THE HURKLE'S GRIDPOINT. YOU GET " + MAX_GUESSES.to_s + " TRIES."
  puts "AFTER EACH TRY, I WILL TELL YOU THE APPROXIMATE"
  puts "DIRECTION TO GO TO LOOK FOR THE HURKLE."
  puts
end

def say_success(k)
  puts
  puts "YOU FOUND IT IN " + k.to_s + " GUESSES!"
end

def say_where_to_go(x, y)
  print "GO "
  print y < $b ? "NORTH" : "SOUTH" unless y == $b
  print x < $a ? "EAST"  : "WEST" unless x == $a
  puts
end

def say_failure
  puts
  puts "SORRY, THAT'S " + MAX_GUESSES.to_s + " GUESSES."
  puts "THE HURKLE IS AT " + $a.to_s + "," + $b.to_s
end

def say_play_again
  puts
  puts "LET'S PLAY AGAIN, HURKLE IS HIDING."
  puts
end

main
