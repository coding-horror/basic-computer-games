#!/usr/bin/env ruby

# Banner
# reinterpreted from BASIC by stephan.com

# this implementation diverges from the original in some notable
# ways, but maintains the same font definition as before as well
# as the same somewhat bizarre way of interpreting it.  It would
# be more efficient to redesign the font to allow `"%09b" % row`
# and then some substitutions.

FONT = {
  ' ' => [0, 0, 0, 0, 0, 0, 0].freeze,
  '!' => [1, 1, 1, 384, 1, 1, 1].freeze,
  '*' => [69, 41, 17, 512, 17, 41, 69].freeze,
  '.' => [1, 1, 129, 449, 129, 1, 1].freeze,
  '0' => [57, 69, 131, 258, 131, 69, 57].freeze,
  '1' => [0, 0, 261, 259, 512, 257, 257].freeze,
  '2' => [261, 387, 322, 290, 274, 267, 261].freeze,
  '3' => [66, 130, 258, 274, 266, 150, 100].freeze,
  '4' => [33, 49, 41, 37, 35, 512, 33].freeze,
  '5' => [160, 274, 274, 274, 274, 274, 226].freeze,
  '6' => [194, 291, 293, 297, 305, 289, 193].freeze,
  '7' => [258, 130, 66, 34, 18, 10, 8].freeze,
  '8' => [69, 171, 274, 274, 274, 171, 69].freeze,
  '9' => [263, 138, 74, 42, 26, 10, 7].freeze,
  '=' => [41, 41, 41, 41, 41, 41, 41].freeze,
  '?' => [5, 3, 2, 354, 18, 11, 5].freeze,
  'a' => [505, 37, 35, 34, 35, 37, 505].freeze,
  'b' => [512, 274, 274, 274, 274, 274, 239].freeze,
  'c' => [125, 131, 258, 258, 258, 131, 69].freeze,
  'd' => [512, 258, 258, 258, 258, 131, 125].freeze,
  'e' => [512, 274, 274, 274, 274, 258, 258].freeze,
  'f' => [512, 18, 18, 18, 18, 2, 2].freeze,
  'g' => [125, 131, 258, 258, 290, 163, 101].freeze,
  'h' => [512, 17, 17, 17, 17, 17, 512].freeze,
  'i' => [258, 258, 258, 512, 258, 258, 258].freeze,
  'j' => [65, 129, 257, 257, 257, 129, 128].freeze,
  'k' => [512, 17, 17, 41, 69, 131, 258].freeze,
  'l' => [512, 257, 257, 257, 257, 257, 257].freeze,
  'm' => [512, 7, 13, 25, 13, 7, 512].freeze,
  'n' => [512, 7, 9, 17, 33, 193, 512].freeze,
  'o' => [125, 131, 258, 258, 258, 131, 125].freeze,
  'p' => [512, 18, 18, 18, 18, 18, 15].freeze,
  'q' => [125, 131, 258, 258, 322, 131, 381].freeze,
  'r' => [512, 18, 18, 50, 82, 146, 271].freeze,
  's' => [69, 139, 274, 274, 274, 163, 69].freeze,
  't' => [2, 2, 2, 512, 2, 2, 2].freeze,
  'u' => [128, 129, 257, 257, 257, 129, 128].freeze,
  'v' => [64, 65, 129, 257, 129, 65, 64].freeze,
  'w' => [256, 257, 129, 65, 129, 257, 256].freeze,
  'x' => [388, 69, 41, 17, 41, 69, 388].freeze,
  'y' => [8, 9, 17, 481, 17, 9, 8].freeze,
  'z' => [386, 322, 290, 274, 266, 262, 260].freeze
}.freeze

puts 'horizontal'
x = gets.strip.to_i
puts 'vertical'
y = gets.strip.to_i
puts 'centered'
centered = gets.strip.downcase.chars.first == 'y'
puts 'character ("all" for character being printed)'
fill = gets.strip.downcase
puts 'statement'
statement = gets.strip.downcase

all = (fill.downcase == 'all')
lenxs = all ? 1 : fill.length
start = 1
start += (63 - 4.5 * y) / lenxs if centered

statement.each_char do |char|
  next puts "\n" * 7 * x if char == ' '

  xs = all ? char : fill
  FONT[char].each do |su|
    print ' ' * start
    8.downto(0) do |k|
      if (1 << k) < su
        print xs * y
        su -= (1 << k)
      else
        print ' ' * (y * lenxs)
      end
    end
    puts
  end

  (2 * x).times { puts }
end
75.times { puts }
