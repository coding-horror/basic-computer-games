########################################################
#
# Acey Ducey
#
# From: BASIC Computer Games (1978)
#       Edited by David Ahl
#
# "This is a simulation of the Acey Ducey card game.
#  In the game, the dealer (the computer) deals two
#  cards face up.  You have an option to bet or not to
#  bet depending on whether or not you feel the next
#  card dealt will have a value between the first two.
#
# "Your initial money is set to $100. The game keeps
#  going on until you lose all your money or interrupt
#  the program.
#
# "The original BASIC program author was Bill Palmby
#  of Prairie View, Illinois."
#
# Ruby port by Christopher Oezbek, 2021
#
# This uses the following techniques:
#
#  - The programm largely consists of a GAME LOOP,
#    which is used to represent one round.
#  - The Kernel function rand(Range) is used to get an
#    Integer in the (inclusive) Range of 2 to 14.
#  - To ensure the user enters a proper Integer
#    via the console, an inline 'rescue' statement is
#    used.
#  - To capture the long text in the introduction, a
#    squiggly HEREDOC string declaration <<~ is used.
#
#
########################################################

puts <<~INSTRUCTIONS
           🂡  ACEY DUCEY CARD GAME 🂱
   CREATIVE COMPUTING - MORRISTOWN, NEW JERSEY

  ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER
  THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP
  YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING
  ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE
  A VALUE BETWEEN THE FIRST TWO.
  IF YOU DO NOT WANT TO BET IN A ROUND, ENTER 0
INSTRUCTIONS

# The player starts with 100$
stake = 100

while true # Game loop
  puts
  puts "YOU NOW HAVE #{stake} DOLLARS."
  puts
  puts "HERE ARE YOUR NEXT TWO CARDS:"

  # Randomly draw two cards and make sure the first card is lower in value than the second
  # Using array destructuring, this sorted array can be assigned to `first_card` and `second_card`
  first_card, second_card = (2...14).to_a.shuffle.pop(2).sort

  # Helper method to convert a numeric card into a String for printing
  def card_name(card)
    case card
    when 11
      "JACK"
    when 12
      "QUEEN"
    when 13
      "KING"
    when 14
      "ACE"
    else
      card
    end
  end

  puts card_name(first_card)
  puts card_name(second_card)
  puts
  puts

  # Loop until the user enters something sensible
  while true
    puts "WHAT IS YOUR BET? ENTER 0 IF YOU DON'T WANT TO BET (CTRL+C TO EXIT)"
    your_bet = Integer(gets.chomp) rescue nil

    if your_bet == nil || your_bet < 0
      puts "PLEASE ENTER A POSITIVE NUMBER"
      puts
      next
    end

    if your_bet > stake
      puts "SORRY, MY FRIEND, BUT YOU BET TOO MUCH."
      puts "YOU HAVE ONLY #{stake} DOLLARS TO BET."
      puts
      next
    end

    break
  end

  if your_bet == 0
    puts "CHICKEN!!"
    next
  end

  puts "THANK YOU! YOUR BET IS #{your_bet} DOLLARS."
  puts ""
  puts "THE THIRD CARD IS:"
  third_card = rand(2..14)
  puts card_name(third_card)
  puts

  if first_card <= third_card && third_card <= second_card
    puts "YOU WIN!!!"
    stake += your_bet
  else
    puts "SORRY, YOU LOSE"
    stake -= your_bet
  end

  if stake == 0
    puts
    puts "SORRY, FRIEND, BUT YOU BLEW YOUR WAD."
    puts

    puts "TRY AGAIN? (YES OR NO)"
    if gets.chomp.downcase.start_with? 'y'
      # Reset cash to 100
      stake = 100
    else
      puts "O.K., HOPE YOU HAD FUN!"
      exit
    end
  end
end
