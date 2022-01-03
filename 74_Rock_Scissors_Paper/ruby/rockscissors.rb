SCREEN_WIDTH = 72

MOVE_WORDS = {
  1 => 'PAPER',
  2 => 'SCISSORS',
  3 => 'ROCK'
}

WIN_TABLE = {
  1 => 3,
  2 => 1,
  3 => 2
}

def center_text(text)
  text.rjust((SCREEN_WIDTH / 2) + (text.size / 2))
end

def ask_for_number_of_games
  loop do
    puts "HOW MANY GAMES" 
    response = STDIN.gets.to_i
    return response if response > 0 and response < 11
    puts "SORRY, BUT WE AREN'T ALLOWED TO PLAY THAT MANY."
  end
end

def ask_for_human_move
  loop do
    puts "3=ROCK...2=SCISSORS...1=PAPER"
    puts "1...2...3...WHAT'S YOUR CHOICE"
    response = STDIN.gets.to_i
    return response if [1,2,3].include?(response)
    puts "INVALID"
  end
end

def calculate_result(human_move, computer_move)
  return 'TIE' if human_move == computer_move
  return 'WIN' if WIN_TABLE[human_move] == computer_move
  'LOSE'
end

puts center_text('GAME OF ROCK, SCISSORS, PAPER')
puts center_text('CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY')
puts
puts
puts

number_of_games = ask_for_number_of_games
games_won = 0
games_lost = 0

number_of_games.times do |game_number|
  puts
  puts "GAME NUMBER #{game_number + 1}"
  computer_move = rand(3) + 1
  human_move = ask_for_human_move
  puts "THIS IS MY CHOICE..."
  puts "...#{MOVE_WORDS[computer_move]}"
  
  case calculate_result(human_move, computer_move)
  when 'WIN'
    puts "YOU WIN!!!"
    games_won += 1
  when 'TIE'
    puts "TIE GAME.  NO WINNER."
  when 'LOSE'
    puts "WOW!  I WIN!!!"
    games_lost = games_lost += 1
  end
end

puts
puts "HERE IS THE FINAL GAME SCORE:"
puts "I HAVE WON #{games_lost} GAME(S)."
puts "YOU HAVE WON #{games_won} GAME(S)."
puts "AND #{number_of_games - (games_lost + games_won)} GAME(S) ENDED IN A TIE."
puts "THANKS FOR PLAYING!!"

