def print_intro
  print " " * 31 + "GUESS\n"
  print " " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n"
  print "THIS IS A NUMBER GUESSING GAME. I'LL THINK\nOF A NUMBER BETWEEN 1 AND ANY LIMIT YOU WANT.\nTHEN YOU HAVE TO GUESS WHAT IT IS.\n"
end

def game_play(limit,choice_limit)
    random = rand(limit.to_i)+1
    puts "I'M THINKING OF A NUMBER BETWEEN 1 and #{limit}"
    puts "NOW YOU TRY TO GUESS WHAT IT IS."
    print "? "
    ans=0
    guesses=0
    until ans.to_i == random.to_i
      ans = gets.chomp
      guesses += 1
      if ans.to_i > random.to_i
        puts "TOO HIGH. TRY A SMALLER ANSWER."
        print "? "
      elsif ans.to_i < random.to_i
        puts "TOO LOW. TRY A BIGGER ANSWER."
        print "? "
      elsif ans.to_i == random.to_i
        puts "THAT'S IT! YOU GOT IT IN #{guesses} TRIES."
        if guesses.to_i < choice_limit.to_i
          puts "VERY GOOD."
        elsif guesses.to_i == choice_limit.to_i
          puts "GOOD."
        else
          puts "YOU SHOULD HAVE BEEN ABLE TO GET IT IN ONLY #{choice_limit}"
        end
        print "\n\n\n\n\n"
      end
    end
end


def main
  print_intro
  puts "WHAT LIMIT DO YOU WANT"
  limit = gets.chomp
  choice_limit = (Math.log(limit.to_i)/Math.log(2)+1).to_i
  while 1
    game_play(limit,choice_limit)
  end
end

main
