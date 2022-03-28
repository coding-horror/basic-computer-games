module Model
class CardKind
  def initialize(label, value)
    @label = label
    @value = value
  end
  
  private_class_method :new

  TWO = self.new("2", 2)
  THREE = self.new("3", 3)
  FOUR = self.new("4", 4)
  FIVE = self.new("5", 5)
  SIX = self.new("6", 6)
  SEVEN = self.new("7", 7)
  EIGHT = self.new("8", 8)
  NINE = self.new("9", 9)
  TEN = self.new("10", 10)
  JACK = self.new("J", 10)
  QUEEN = self.new("Q", 10)
  KING = self.new("K", 10)
  ACE = self.new("A", 11)

  KINDS_SET = [
    TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN,
    JACK, QUEEN, KING, ACE
  ]

  def same_value?(other_card)
    value == other_card.value
  end

  def +(other)
    throw "other doesn't respond to +" unless other.responds_to? :+

    other.+(@value)
  end 

  attr_reader :label, :value
end
end
