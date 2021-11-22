def intro
  puts "                                 TRAIN
               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



TIME - SPEED DISTANCE EXERCISE

"
end

def get_user_guess
  while true
    begin
      number = Float(gets.chomp)
      return number
    rescue ArgumentError
      # Ignored
    end

    puts "!NUMBER EXPECTED - RETRY INPUT LINE"
    print "? "
  end
end

def main
  intro

  loop do
    car_speed = rand(25) + 40
    car_time = rand(15) + 5
    train_speed = rand(19) + 20

    print " A CAR TRAVELING #{car_speed} MPH CAN MAKE A CERTAIN TRIP IN
 #{car_time} HOURS LESS THAN A TRAIN TRAVELING AT #{train_speed} MPH.
HOW LONG DOES THE TRIP TAKE BY CAR? "
    guess = get_user_guess

    answer = ((car_time * train_speed) / (car_speed - train_speed).to_f).round(5)
    delta = (((answer - guess) * 100 / guess) + 0.5).abs.to_i

    if delta > 5
      puts "SORRY.  YOU WERE OFF BY #{delta} PERCENT."
    else
      puts "GOOD! ANSWER WITHIN #{delta} PERCENT."
    end

    print "CORRECT ANSWER IS #{answer == answer.to_i ? answer.to_i : answer} HOURS.

ANOTHER PROBLEM (YES OR NO)? "
    option = (gets || '').chomp.upcase
    break unless option == 'YES'
  end
end

trap "SIGINT" do puts; exit 130 end

main