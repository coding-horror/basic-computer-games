require "./card_kind.rb"

module Model
class Hand
  HAND_STATE_PLAYING = :hand_playing
  HAND_STATE_BUSTED = :hand_busted
  HAND_STATE_STAND = :hand_stand

  def initialize(bet: 0, cards: [], is_split_hand: false)
    @state = HAND_STATE_PLAYING
    @bet = bet
    @cards = cards
    @total = nil
    @is_split_hand = is_split_hand
  end

  attr_reader :bet, :cards

  def is_playing?
    @state == HAND_STATE_PLAYING
  end

  def is_busted?
    @state == HAND_STATE_BUSTED
  end

  def is_standing?
    @state == HAND_STATE_STAND
  end

  def total
    return @total unless @total.nil?

    @total = @cards.sum

    if @total > 21
      aces_count = @cards.count {|c| c == CardKind::ACE}
      while @total > 21 && aces_count > 0 do
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
    throw "can't hit" unless @state == HAND_STATE_PLAYING

    @cards.push(card)
    @total = nil

    check_busted
  end

  def double_down(card)
    throw "can't double down" unless @state == HAND_STATE_PLAYING

    @bet *= 2
    hit card
  end

  def stand
    throw "can't double down" unless @state == HAND_STATE_PLAYING

    @state = HAND_STATE_STAND
  end


  private

  def check_busted
    @state = HAND_STATE_BUSTED if total > 21
  end
end
end
