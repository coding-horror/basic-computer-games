require_relative "./card_kind.rb"

module Model
class Pack
  def initialize
    @cards = []
    reshuffle
  end

  def reshuffle_if_necessary
    return if @cards.count > 2
    reshuffle
  end

  def draw
    reshuffle_if_necessary
    @cards.pop
  end

  private

  def reshuffle
    puts "RESHUFFLING"
    @cards = 4.times.map {|_| CardKind::KINDS_SET}.flatten
    @cards.shuffle!
  end
end
end
