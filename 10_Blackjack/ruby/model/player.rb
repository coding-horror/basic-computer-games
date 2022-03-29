require_relative "./hand.rb"

module Model
class Player
  def initialize(id)
    @id = id
    @balance = 0
    @original_bet = 0
    @insurance = 0

    @hand = nil
    @split_hand = nil
  end

  attr_reader :id, :balance, :hand, :split_hand, :insurance

  ## Begining of hand dealing actions
  def deal_initial_hand(hand)
    @hand = hand
    @split_hand = nil
    @max_insurance = @hand.bet / 2
    @insurance = 0
  end

  def has_split_hand?
    !@split_hand.nil?
  end

  def can_split?
    not has_split_hand? and @hand.can_split?
  end

  def split
    throw "can't split" unless can_split?

    @hand, @split_hand = @hand.split
  end

  def bet_insurance(bet)
    if bet < 0
      bet = 0
      puts "NEGATIVE BET -- using 0 insurance bet"
    end

    if bet > @max_insurance
      bet = @max_insurance
      puts "TOO HIGH -- using max insurance bet of #{bet}"
    end

    @insurance = bet
  end

  ## End of hand dealing actions

  def update_balance(dealer_hand)
    balance_update = 0

    balance_update += get_balance_update(@hand, dealer_hand)
    if has_split_hand? then
      balance_update += get_balance_update(@split_hand, dealer_hand)
    end

    if dealer_hand.is_blackjack?
      balance_update += 2 * @insurance
    else
      balance_update -= @insurance
    end

    @balance += balance_update

    balance_update
  end


  private

  def get_balance_update(hand, dealer_hand)
    if hand.is_busted?
      return -hand.bet
    elsif dealer_hand.is_busted?
      return hand.bet
    elsif dealer_hand.total == hand.total
      return 0
    else
      return (dealer_hand.total < hand.total ? 1 : -1) * hand.bet
    end
  end
end
end
