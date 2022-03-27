require_relative "./game.rb"

def intro
    puts "Welcome to Blackjack"
end

def ask_for_players_count
    puts "How many of you want to join the table?"
    return gets.to_i
end

begin
    intro
    players_count = ask_for_players_count
    Game.new(players_count).start
rescue SystemExit, Interrupt
    exit
rescue => exception
   p exception
end
