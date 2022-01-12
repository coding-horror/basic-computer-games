# frozen_string_literal: true

DEBUG = !ENV['DEBUG'].nil?

require 'io/console' if DEBUG

# BASIC arrays are 1-based, unlike Ruby 0-based arrays,
# and this class simulates that. BASIC arrays are zero-filled,
# which is also done here. While we could easily update the
# algorithm to work with zero-based arrays, this class makes
# the problem easier to reason about, row or col 1 are the
# first row or column.
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

  def to_s(width: max_width, row_hilite: nil, col_hilite: nil)
    @val.map.with_index do |row, row_index|
      row.map.with_index do |val, col_index|
        if row_hilite == row_index + 1 && col_hilite == col_index + 1
          "[#{val.to_s.center(width)}]"
        else
          val.to_s.center(width + 2)
        end
      end.join
    end.join("\n")
  end

  def max_width
    @val.flat_map { |row| row.map { |val| val.to_s.length } }.sort.last
  end
end

class Maze
  EXIT_DOWN = 1
  EXIT_RIGHT = 2

  # Set up a constant hash for directions
  # The values represent the direction of the move as changes to row, col
  # and the type of exit when moving in that direction
  DIRECTIONS = {
    left: { row: 0, col: -1, exit: EXIT_RIGHT },
    up: { row: -1, col: 0, exit: EXIT_DOWN },
    right: { row: 0, col: 1, exit: EXIT_RIGHT },
    down: { row: 1, col: 0, exit: EXIT_DOWN }
  }.freeze

  attr_reader :width, :height, :used, :walls, :entry

  def initialize(width, height)
    @width = width
    @height = height

    @used = BasicArrayTwoD.new(height, width)
    @walls = BasicArrayTwoD.new(height, width)

    create
  end

  def draw
    # Print the maze
    draw_top(entry, width)
    (1..height - 1).each do |row|
      draw_row(walls[row])
    end
    draw_bottom(walls[height])
  end

  private

  def create
    # entry represents the location of the opening
    @entry = (rand * width).round + 1

    # Set up our current row and column, starting at the top and the locations of the opening
    row = 1
    col = entry
    c = 1
    used[row, col] = c # This marks the opening in the first row
    c += 1

    while c != width * height + 1 do
      debug walls, row, col
      # remove possible directions that are blocked or
      # hit cells that we have already processed
      possible_dirs = DIRECTIONS.reject do |dir, change|
        nrow = row + change[:row]
        ncol = col + change[:col]
        nrow < 1 || nrow > height || ncol < 1 || ncol > width || used[nrow, ncol] != 0
      end.keys

      # If we can move in a direction, move and make opening
      if possible_dirs.size != 0
        direction = possible_dirs.sample
        change = DIRECTIONS[direction] # pick a random direction
        if %i[left up].include?(direction)
          row += change[:row]
          col += change[:col]
          walls[row, col] = change[:exit]
        else
          walls[row, col] += change[:exit]
          row += change[:row]
          col += change[:col]
        end
        used[row, col] = c
        c = c + 1
      # otherwise, move to the next used cell, and try again
      else
        loop do
          if col != width
            col += 1
          elsif row != height
            row += 1
            col = 1
          else
            row = col = 1
          end
          break if used[row, col] != 0
          debug walls, row, col
        end
      end
    end

    # Add a random exit
    walls[height, (rand * width).round] += 1
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
    row.each.with_index do |val, col|
      print val < 2 ? '  ┃' : '   '
    end
    puts
    row.each.with_index do |val, col|
      print val == 0 || val == 2 ? (col == 0 ? '┣━━' : '╋━━') : (col == 0 ? '┃  ' : '┫  ')
    end
    puts '┫'
  end

  def draw_bottom(row)
    print '┃'
    row.each.with_index do |val, col|
      print val < 2 ? '  ┃' : '   '
    end
    puts
    row.each.with_index do |val, col|
      print val == 0 || val == 2 ? (col == 0 ? '┗━━' : '┻━━') : (col == 0 ? '┗  ' : '┻  ')
    end
    puts '┛'
  end

  def debug(walls, row, col)
    return unless DEBUG

    STDOUT.clear_screen
    puts walls.to_s(row_hilite: row, col_hilite: col)
    sleep 0.1
  end
end

class Amazing
  def run
    draw_header

    width, height = ask_dimensions
    while width <= 1 || height <= 1
      puts "MEANINGLESS DIMENSIONS.  TRY AGAIN."
      width, height = ask_dimensions
    end

    maze = Maze.new(width, height)
    puts "\n" * 3
    maze.draw
  end

  def draw_header
    puts ' ' * 28 + 'AMAZING PROGRAM'
    puts ' ' * 15 + 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY'
    puts "\n" * 3
  end

  def ask_dimensions
    print 'WHAT ARE YOUR WIDTH AND HEIGHT? '
    width = gets.to_i
    print '?? '
    height = gets.to_i
    [width, height]
  end
end

Amazing.new.run