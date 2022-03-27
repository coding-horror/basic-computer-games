require_relative "./model/hand.rb"
require_relative "./model/player.rb"
require_relative "./model/card_kind.rb"
require_relative "./model/pack.rb"

class Game

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

  def play_dealer
    while @dealer_hand.total < 17
      card = @pack.draw
      @dealer_hand.hit card
    end

    if !@dealer_hand.is_busted?
      @dealer_hand.stand
    end

    # TODO: print dealer hand
  end

  def settle
    @players.each_entry do |player|
      @dealer_balance -= player.update_balance @dealer_hand
    end

    # TODO: print players balances
  end


  def print_players_and_dealer_hands
    puts "PLAYER\t#{@players.map(&:id).join("\t")}\tDEALER"
    # TODO: Check for split hands
    puts "      \t#{@players.map {|p| p.hand.cards[0].label}.join("\t")}\t#{@dealer_hand.cards[0].label}"
    puts "      \t#{@players.map {|p| p.hand.cards[1].label}.join("\t")}"
  end

  def play_hand player, hand
    name = "PLAYER #{player.id}"

    print "#{name}? "

    # TODO: redo this loop logic isn't working properly
    while not case gets.strip
    when "H"
      while hand.is_playing?
        card = @pack.draw
        hand.hit card
        print "RECEIVED #{card.label} "

        if hand.is_busted?
          puts "...BUSTED"
          return
        end

        print "HIT? "
        while case gets.strip
        when "S"
          hand.stand
          true
        when "H"
          true
        else
          print "(H)IT or (S)TAND? "
          false
        end
        end
      end
      true
    when "S"
      hand.stand
      true
    when "/"
      if player.can_split?
        player.split

        play_hand "#{name} - Hand 1", player.hand
        play_hand "#{name} - Hand 2", player.split_hand

        return
      else
        puts "CANNOT SPLIT"
        false
      end
    when "D"
      card = @pack.draw
      hand.double_down card
      print "RECEIVED #{card.label}"
      true
    else
      print "#{name}? "
      false
    end
    end

    puts "TOTAL IS #{hand.total}"
  end
end
