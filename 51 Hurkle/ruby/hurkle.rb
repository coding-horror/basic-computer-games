puts " " * 33 + "HURKLE"
puts " " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
3.times { puts }
$n = 5
$g = 10
puts
puts "A HURKLE IS HIDING ON A " + $g.to_s + " BY " + $g.to_s + " GRID. HOMEBASE"
puts "ON THE GRID IS POINT 0,0 IN THE SOUTHWEST CORNER,"
puts "AND ANY POINT ON THE GRID IS DESIGNATED BY A"
puts "PAIR OF WHOLE NUMBERS SEPERATED BY A COMMA. THE FIRST"
puts "NUMBER IS THE HORIZONTAL POSITION AND THE SECOND NUMBER"
puts "IS THE VERTICAL POSITION. YOU MUST TRY TO"
puts "GUESS THE HURKLE'S GRIDPOINT. YOU GET " + $n.to_s + " TRIES."
puts "AFTER EACH TRY, I WILL TELL YOU THE APPROXIMATE"
puts "DIRECTION TO GO TO LOOK FOR THE HURKLE."
puts

def main
  loop do
    $a = rand($g).floor
    $b = rand($g).floor
    found = false
    (1..$n).each do |k|
      print "GUESS # " + k.to_s + " "
      print "? "
      x, y = gets.chomp.split(",").map(&:to_i)
      if (x-$a).abs + (y-$b).abs == 0
        you_found_him(k)
        found = true
        break
      end
      say_where_to_go(x, y)
      puts
    end
    if not found
      puts
      puts "SORRY, THAT'S " + $n.to_s + " GUESSES."
      puts "THE HURKLE IS AT " + $a.to_s + "," + $b.to_s
    end
    puts
    puts "LET'S PLAY AGAIN, HURKLE IS HIDING."
    puts
  end
end

def you_found_him(k)
  puts
  puts "YOU FOUND HIM IN " + k.to_s + " GUESSES!"
end

def say_where_to_go(x, y)
  print "GO "
  if not y == $b
    if not y < $b
      print "SOUTH"
    else
      print "NORTH"
    end
  end
  if not x == $a
    if not x < $a
      print "WEST"
    else
      print "EAST"
    end
  end
  puts
end

main
