#!/usr/bin/env ruby
# reinterpreted from BASIC by stephan.com
class War
  class Card
    class CardError < StandardError; end

    SUITS = %i[spades hearts clubs diamonds].freeze
    PIPS = %i[ace deuce trey four five six seven eight nine ten jack king queen].freeze
    CARDS = SUITS.product(PIPS).freeze
    VALUES = PIPS.zip(1..13).to_h.freeze

    attr_reader :value

    def initialize(suit, pip)
      @suit = suit
      @pip = pip
      raise CardError, 'invalid suit' unless SUITS.include? @suit
      raise CardError, 'invalid pip' unless PIPS.include? @pip

      @value = VALUES[pip]
    end

    def <=>(other)
      @value <=> other.value
    end

    def >(other)
      @value > other.value
    end

    def <(other)
      @value < other.value
    end

    def to_s
      "the #{@pip} of #{@suit}"
    end

    def self.shuffle
      CARDS.map { |suit, pip| new(suit, pip) }.shuffle
    end
  end

  def initialize
    @your_score = 0
    @computer_score = 0
    @your_deck = Card.shuffle
    @computer_deck = Card.shuffle
  end

  def play
    intro

    loop do
      puts "\nYou: #{@your_score} Computer: #{@computer_score}"
      round @your_deck.shift, @computer_deck.shift
      break if empty?

      puts 'Do you want to continue?'
      break unless yesno
    end

    outro
  end

  private

  def round(your_card, computer_card)
    puts "You: #{your_card} vs Computer: #{computer_card}"
    return puts 'Tie. No score change.' if your_card == computer_card

    if computer_card > your_card
      puts "Computer wins with #{computer_card}"
      @computer_score += 1
    else
      puts "You win with #{your_card}"
      @your_score += 1
    end
  end

  def yesno
    loop do
      wants = gets.strip
      return true if wants.downcase == 'yes'
      return false if wants.downcase == 'no'

      puts 'Yes or no, please.'
    end
  end

  def intro
    puts 'War'.center(80)
    puts 'stephan.com'.center(80)
    puts
    puts 'This is the card game of war.'
    puts 'Do you want directions'
    directions if yesno
  end

  def directions
    puts 'The computer gives you and it a \'card\'.  The higher card'
    puts '(numerically) wins.  The game ends when you choose not to'
    puts 'continue or when you have finished the pack.'
    puts
  end

  def outro
    puts "We've run out of cards" if empty?
    puts "Final score:\nYou: #{@your_score}\nComputer: #{@computer_score}"
    puts 'Thanks for playing!'
  end

  def empty?
    @your_deck.empty? || @computer_deck.empty?
  end
end

War.new.play
