# frozen_string_literal: true

# Set up a constant hash for directions
DIRECTIONS = {
  left: 0,
  up: 1,
  right: 2,
  down: 3
}.freeze

EXIT_DOWN = 1
EXIT_RIGHT = 2

# BASIC arrays are 1-based, unlike Ruby 0-based arrays,
#   and this class simulates that. BASIC arrays are also zero-filled,
#   which is also done here.
class BasicArrayTwoD
  def initialize(rows, cols)
    @val = Array.new(rows) { Array.new(cols, 0) }
  end

  def [](row, col = nil)
    if col
      @val[row - 1][col - 1]
    else
      @val[row - 1]
    end
  end

  def []=(row, col, n)
    @val[row - 1][col - 1] = n
  end

  def to_s
    @val.map { |row| row.join(' ') }.join("\n")
  end
end

def draw_top(entry, width)
  (1..width).each do |i|
    if i == entry
      print i == 1 ? '┏  ' : '┳  '
    else
      print i == 1 ? '┏━━' : '┳━━'
    end
  end

  puts '┓'
end

def draw_row(row)
  print '┃'
  row.each { |val| print val < 2 ? '  ┃' : '   ' }
  puts
  row.each { |val| print val == 0 || val == 2 ? '┣━━' : '┃  ' }
  puts '┫'
end


# 10 PRINT TAB(28);"AMAZING PROGRAM"
# 20 PRINT TAB(15);"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
# 30 PRINT:PRINT:PRINT:PRINT
puts ' ' * 28 + 'AMAZING PROGRAM'
puts ' ' * 15 + 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY'
puts "\n" * 3

# 100 INPUT "WHAT ARE YOUR WIDTH AND LENGTH";H,V
# 102 IF H<>1 AND V<>1 THEN 110
# 104 PRINT "MEANINGLESS DIMENSIONS.  TRY AGAIN.":GOTO 100
def ask_dimensions
  print 'WHAT ARE YOUR WIDTH AND HEIGHT? '
  width = gets.to_i
  print '?? '
  height = gets.to_i
  [width, height]
end

width, height = ask_dimensions
while width <= 1 || height <= 1
  puts "MEANINGLESS DIMENSIONS.  TRY AGAIN."
  width, height = ask_dimensions
end

# 110 DIM W(H,V),V(H,V)
# BASIC programs can have the same variable names for different types,
#   so the v array is not the same as the v int. Here we're renaming the arrays
#   to have more friendly names
used = BasicArrayTwoD.new(height, width)
walls = BasicArrayTwoD.new(height, width)

puts "\n" * 3


# entry represents the location of the opening
entry = (rand * width).round + 1

# Set up our current row and column, starting at the top and the locations of the opening
row = 1
col = entry
c = 1
used[row, col] = c # This marks the opening in the first row
c += 1

while c != width * height + 1 do
  # remove possible directions that are blocked or
  # hit cells that we have already processed
  possible_dirs = DIRECTIONS.keys
  if col == 0 || used[row, col - 1] != 0
    possible_dirs.delete(:left)
  end
  if row == 0 || used[row - 1, col] != 0
    possible_dirs.delete(:up)
  end
  if col == width || used[row, col + 1] != 0
    possible_dirs.delete(:right)
  end
  if row == height || used[row + 1, col] != 0
    possible_dirs.delete(:down)
  end

  # If we can move in a direction, move and make opening
  if possible_dirs.size != 0
    direction = possible_dirs.sample # pick a random direction
    if direction == :left
      col = col - 1
      walls[row, col] = EXIT_RIGHT
    elsif direction == :up
      row = row - 1
      walls[row, col] = EXIT_DOWN
    elsif direction == :right
      walls[row, col] += EXIT_RIGHT
      col = col + 1
    elsif direction == :down
      walls[row, col] += EXIT_DOWN
      row = row + 1
    end
    used[row, col] = c
    c = c + 1
  # otherwise, move to the next used cell, and try again
  else
    while true do
      if col != width
        col += 1
      elsif row != height
        row += 1
        col = 1
      else
        row = col = 1
      end
      break if used[row, col] != 0
    end
  end
end

# Add a random exit
walls[height, (rand * width).round + 1] += 1

# Print the maze
draw_top(entry, width)
(1..height).each do |row|
  draw_row(walls[row])
end
