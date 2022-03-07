def intro
  puts "                               MATH DICE
                 CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY

THIS PROGRAM GENERATES SUCCESSIVE PICTURES OF TWO DICE.
WHEN TWO DICE AND AN EQUAL SIGN FOLLOWED BY A QUESTION
MARK HAVE BEEN PRINTED, TYPE YOUR ANSWER AND THE RETURN KEY.
TO CONCLUDE THE LESSON, TYPE '0' AS YOUR ANSWER.
"
end

def game_play
  num = 0
  sum = 0
  tries = 0
  until num == 2 do
    num+=1
    roll = rand(6) + 1
    print_dice(roll)
    sum = sum + roll
    if num == 1
      print "\n   +\n\n"
    end
    if num == 2
      print "\n   =? "
      ans = gets.chomp
      if ans.to_i == 0
        #END GAME
        exit(0)
      elsif ans.to_i == sum
        puts "RIGHT!"
        puts "THE DICE ROLL AGAIN"
      else
        puts "NO, COUNT THE SPOTS AND GIVE ANOTHER ANSWER"
        print "\n   =? "
        ans = gets.chomp
        if ans.to_i == sum
          puts "RIGHT!"
          puts "THE DICE ROLL AGAIN"
        elsif ans.to_i == 0
          exit(0)
        else
          puts "NO, THE ANSWER IS #{sum}"
        end
      end
    end
  end
end



def print_dice(roll)
  puts " -----"
  if roll == 1
    print_blank
    print_one_mid
    print_blank
  elsif roll == 2
    print_one_left
    print_blank
    print_one_right
  elsif roll == 3
    print_one_left
    print_one_mid
    print_one_right
  elsif roll == 4
    print_two
    print_blank
    print_two
  elsif roll == 5
    print_two
    print_one_mid
    print_two
  elsif roll == 6
    print_two
    print_two
    print_two
  else
    puts "not a legit dice roll"
  end
  puts " -----"
end


def print_one_left
  puts "I *   I"
end

def print_one_mid
  puts "I  *  I"
end

def print_one_right
  puts "I   * I"
end

def print_two
  puts "I * * I"
end

def print_blank
  puts "I     I"
end



def main
  intro
  #Continue playing forever until it terminates with exit in game_play
  while 1 == 1 do
    game_play
  end
end

main
