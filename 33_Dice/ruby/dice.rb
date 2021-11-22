def intro
  puts "                                  DICE
               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



THIS PROGRAM SIMULATES THE ROLLING OF A
PAIR OF DICE.
YOU ENTER THE NUMBER OF TIMES YOU WANT THE COMPUTER TO
'ROLL' THE DICE.  WATCH OUT, VERY LARGE NUMBERS TAKE
A LONG TIME.  IN PARTICULAR, NUMBERS OVER 5000.

"
end

def get_rolls
  while true
    number = gets.chomp
    return number.to_i if /^\d+$/.match(number)
    puts "!NUMBER EXPECTED - RETRY INPUT LINE"
    print "? "
  end
end

def dice_roll = rand(6) + 1 # ruby 3, woot!

def print_rolls(rolls)
  values = Array.new(11, 0)
  (1..rolls).each { values[dice_roll + dice_roll - 2] += 1 }
  puts "\nTOTAL SPOTS   NUMBER OF TIMES"
  (0..10).each { |k| puts " %-2d            %-2d" % [k + 2, values[k]] }
end

def main
  intro
  loop do
    print "HOW MANY ROLLS? "
    rolls = get_rolls

    print_rolls(rolls)

    print "\n\nTRY AGAIN? "
    option = (gets || '').chomp.upcase
    break unless option == 'YES'
    puts
  end
end

trap "SIGINT" do puts; exit 130 end

main