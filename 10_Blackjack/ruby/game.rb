require "./model/hand.rb"
require "./model/player.rb"
require "./model/card_kind.rb"

class Game

  def initialize(players_count)
    @pack = Model::Pack.new
    @dealer_balance = 0
    @dealer_hand = Model::Hand.new
    @palyers = 1.upto(players_count).map { |id| Model::Player.new(id) }
  end

  def start
    loop do
      collect_bets
      deal
      play_players
      play_dealer
      settle
    end
  end

  def collect_bets
  end

  def deal
  end

  def play_players
  end

  def play_dealer
  end

  def settle
  end

end
