#!/usr/bin/env ruby
MAX_TRIES = 6
RANGE = (1..100)

def intro
  puts <<~END_OF_INTRO
    #{'HI LO'.center(74)}
    #{"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n".center(76)}
    THIS IS THE GAME OF HI LO.\n
    YOU WILL HAVE #{MAX_TRIES} TRIES TO GUESS THE AMOUNT OF MONEY IN THE
    HI LO JACKPOT, WHICH IS BETWEEN #{RANGE.min} AND #{RANGE.max} DOLLARS.  IF YOU
    GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!
    THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY.  HOWEVER,
    IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS.\n\n
  END_OF_INTRO
end

def make_guess
  puts 'YOUR GUESS?'
  @guess = gets.to_i
end

def check_guess
  if @guess == @number
    @guessed_correctly = true
    @total_winnings += @number
    puts <<~END_OF_WIN_TEXT
      GOT IT!!!!!!!!!!   YOU WIN #{@number} DOLLARS.
      YOUR TOTAL WINNINGS ARE NOW #{@total_winnings} DOLLARS.
    END_OF_WIN_TEXT
  else
    puts "YOUR GUESS IS TOO #{@guess > @number ? 'HIGH' : 'LOW'}.\n\n"
  end
end

def blew_it
  @total_winnings = 0
  puts "YOU BLEW IT...TOO BAD...THE NUMBER WAS #{@number}"
end

def outro
  puts "\nSO LONG.  HOPE YOU ENJOYED YOURSELF!!!"
end

intro
@total_winnings = 0
loop do
  @guessed_correctly = false
  @number = rand(RANGE)
  MAX_TRIES.times do
    make_guess
    check_guess
    break if @guessed_correctly
  end
  blew_it unless @guessed_correctly
  puts "\nPLAY AGAIN (YES OR NO)?"
  break if gets.start_with?(/n/i)
end
outro
