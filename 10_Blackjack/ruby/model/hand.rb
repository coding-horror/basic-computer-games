require_relative "./card_kind.rb"

module Model
class Hand
  HAND_STATE_PLAYING = :hand_playing
  HAND_STATE_BUSTED = :hand_busted
  HAND_STATE_STAND = :hand_stand
  HAND_STATE_DOUBLED_DOWN = :hand_doubled_down

  def initialize(bet, cards, is_split_hand: false)
    @state = HAND_STATE_PLAYING
    @bet = bet
    @cards = cards
    @total = nil
    @is_split_hand = is_split_hand
  end

  attr_reader :bet, :cards, :is_split_hand

  def is_playing?
    @state == HAND_STATE_PLAYING
  end

  def is_busted?
    @state == HAND_STATE_BUSTED
  end

  def is_standing?
    @state == HAND_STATE_STAND
  end

  def total(is_dealer: false)
    return @total unless @total.nil?
    
    @total = @cards.reduce(0) {|sum, card| sum + card.value}

    if @total > 21
      aces_count = @cards.count {|c| c == CardKind::ACE}
      while ((!is_dealer && @total > 21) || (is_dealer && @total < 16)) && aces_count > 0 do
        @total -= 10
        aces_count -= 1
      end
    end

    @total
  end

  ## Hand actions

  def can_split?
    not @is_split_hand and @cards.length == 2 && @cards[0].same_value?(cards[1])
  end

  def split
    throw "can't split" unless can_split?
    [
      Hand.new(@bet, @cards[0..1], is_split_hand: true),
      Hand.new(@bet, @cards[1..1], is_split_hand: true)
    ]
  end

  def hit(card)
    throw "can't hit" unless is_playing?

    @cards.push(card)
    @total = nil

    check_busted
  end

  def double_down(card)
    throw "can't double down" unless is_playing?

    @bet *= 2
    hit card

    @state = HAND_STATE_DOUBLED_DOWN
  end

  def stand
    throw "can't stand" unless is_playing?

    @state = HAND_STATE_STAND
  end


  private

  def check_busted
    @state = HAND_STATE_BUSTED if total > 21
  end
end
end
