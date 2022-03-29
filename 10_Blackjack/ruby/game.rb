require_relative "./model/hand.rb"
require_relative "./model/player.rb"
require_relative "./model/card_kind.rb"
require_relative "./model/pack.rb"

class Game

  ALLOWED_HAND_ACTIONS = {
    "hit" => ["H", "S"],
    "split" => ["H", "S", "D"],
    "normal" => ["H", "S", "/", "D"]
  }

  def initialize(players_count)
    @pack = Model::Pack.new
    @dealer_balance = 0
    @dealer_hand = nil
    @players = 1.upto(players_count).map { |id| Model::Player.new(id) }
  end

  def start
    loop do
      collect_bets_and_deal
      play_players
      check_for_insurance_bets
      play_dealer
      settle
    end
  end

  private

  def collect_bets_and_deal
    puts "BETS"

    @players.each_entry do |player|
      print "# #{player.id} ? "
      bet = gets.to_i
      player.deal_initial_hand Model::Hand.new(bet, [@pack.draw, @pack.draw])
    end

    @dealer_hand = Model::Hand.new(0, [@pack.draw, @pack.draw])
    print_players_and_dealer_hands
  end

  def play_players
    @players.each_entry do |player|
      play_hand player, player.hand
    end
  end

  def check_for_insurance_bets
    return if @dealer_hand.cards[0].label != "A"

    print "ANY INSURANCE? "
    return if gets.strip != "Y"

    @players.each_entry do |player|
      print "PLAYER #{player.id} INSURANCE BET? "
      player.bet_insurance(gets.to_i)
    end
  end

  def play_dealer
    puts "DEALER HAS A \t#{@dealer_hand.cards[1].label} CONCEALED FOR A TOTAL OF #{@dealer_hand.total}"

    while @dealer_hand.total(is_dealer: true) < 17
      card = @pack.draw
      @dealer_hand.hit card

      puts "DRAWS #{card.label} \t---TOTAL = #{@dealer_hand.total}"
    end

    if !@dealer_hand.is_busted?
      @dealer_hand.stand
    end
  end

  def settle
    @players.each_entry do |player|
      player_balance_update = player.update_balance @dealer_hand
      @dealer_balance -= player_balance_update

      puts "PLAYER #{player.id} #{player_balance_update < 0 ? "LOSES" : "WINS"} \t#{player_balance_update} \tTOTAL=#{player.balance}"
    end

    puts "DEALER'S TOTAL = #{@dealer_balance}"
  end


  def print_players_and_dealer_hands
    puts "PLAYER\t#{@players.map(&:id).join("\t")}\tDEALER"
    puts "      \t#{@players.map {|p| p.hand.cards[0].label}.join("\t")}\t#{@dealer_hand.cards[0].label}"
    puts "      \t#{@players.map {|p| p.hand.cards[1].label}.join("\t")}"
  end

  def play_hand player, hand
    allowed_actions = ALLOWED_HAND_ACTIONS[(hand.is_split_hand || !hand.can_split?) ? "split" : "normal"]
    name = "PLAYER #{player.id}"
    if hand.is_split_hand
      name += " - HAND #{hand === player.hand ? 1 : 2}"
    end

    did_hit = false

    while hand.is_playing?
      print "#{name}? "

      action = gets.strip

      if !allowed_actions.include?(action)
        puts "Possible actions: #{allowed_actions.join(", ")}"
        next
      end

      if action === "/"
        player.split

        play_hand player, player.hand
        play_hand player, player.split_hand

        return
      end

      if action === "S"
        hand.stand
      end

      if action === "D"
        card = @pack.draw
        hand.double_down card

        puts "RECEIVED #{card.label}"
      end

      if action === "H"
        did_hit = true
        allowed_actions = ALLOWED_HAND_ACTIONS["hit"]
        card = @pack.draw
        hand.hit card

        puts "RECEIVED #{card.label}"
      end
    end

    puts "TOTAL IS #{hand.total}"

    if hand.is_busted?
      puts "... BUSTED"
    end
  end
end
