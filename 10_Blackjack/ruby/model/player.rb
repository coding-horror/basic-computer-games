require "./hand.rb"

module Model
class Player
  def initialize(id)
    @id = id
    @balance = 0

    @current_hand = nil

    @hand = nil
    @split_hand = nil
  end

  attr_reader :id, :balance

  def is_playing?
    not @current_hand.nil? and @current_hand.is_playing?
  end

  ## Begining of hand dealing actions
  def deal_initial_hand(hand)
    @hand = hand
    @current_hand = @hand
    @split_hand = nil
  end

  def has_split_hand?
    @split_hand.present?
  end

  def can_split?
    not has_split_hand? and @hand.can_split?
  end

  def split
    throw "can't split" unless can_split?

    @hand, @split_hand = @hand.split
    @current_hand = @hand
  end

  ## Player hand(s) actions
  def hit(card)
    @current_hand.hit card

    update_current_hand if @current_hand.is_busted?
  end

  def double_down(card)
    @current_hand.double_down(card)

    update_current_hand if @current_hand.is_busted?
  end

  def stand
    @current_hand.stand
    update_current_hand
  end

  def bet_insurance(insurance_bet)
  end

  ## End of hand dealing actions

  def update_balance(dealer_hand)
    balance_update = 0

    balance_update += get_balance_update(@hand, dealer_hand)
    if has_split_hand? then
      balance_update += get_balance_update(@split_hand, dealer_hand)
    end

    @balance += balance_update

    balance_update
  end


  private

  def get_balance_update(hand, dealer_hand)
    if (dealer_hand.is_busted and not hand.is_busted?)
        or (dealer_hand.total < hand.total) then
      return hand.bet
    elsif (hand.is_busted? and not @dealer_hand.is_busted?)
        or dealer_hand.total > hand.total then
      return -hand.bet
    end

    return 0
  end

  def update_current_hand
    if @current_hand != @split_hand and not @split_hand.nil? then
      @current_hand = @split_hand
    else
      @current_hand = nil
    end
  end
end
end
