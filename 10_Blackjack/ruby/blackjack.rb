require "./game.rb"

def intro
end

def ask_for_players_count
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
