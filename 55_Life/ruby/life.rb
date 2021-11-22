#!ruby

# The Pattern class encapsulates everything we would want to know about a
# pattern in our Game of Life: its size, its current pattern of alive and
# dead cells, which generation it's on and how long it can run, and how
# to accept input for itself, print itself, and most importantly iterate
# from one generation to the next.

PATTERN_WIDTH = 80
PATTERN_HEIGHT = 24

class Pattern

  # Begin with a totally empty/dead pattern of fixed size at generation 0.

  def initialize(max_generations: 10)
    @max_generations = max_generations
    @generation = 0
    @population = nil
    @counter = Array.new(PATTERN_HEIGHT) { Array.new(PATTERN_WIDTH, 0) }
    @invalid = false
  end

  # Take input from the console and enter it into the pattern.

  def get_input
    input = []
    done = false

    # Accept the input from the user on the console

    loop do
      print "? "
      line = gets.chomp
      break if line == 'DONE'
      line.gsub!(/^\./, ' ')
      input << line
    end

    # Emit some blank space
    (1..10).each { puts }

    # Center the input in the two-dimensional array
    input_width = input.map { |line| line.length }.max
    width_offset = ((PATTERN_WIDTH - input_width) / 2).floor
    height_offset = ((PATTERN_HEIGHT - input.count) / 2).floor
    # TODO emit error if width > PATTERN_WIDTH or line count > PATTERN_HEIGHT

    # Start by setting each element to 0 if dead or a 1 if alive

    input.each_index do |y|
      line = input[y].ljust(input_width)
      y_offset = y + height_offset
      @counter[y_offset] = [
        [0] * width_offset,
        line.split("").map { |char| char == ' ' ? 0 : 1 },
        [0] * PATTERN_WIDTH
      ].flatten.take(PATTERN_WIDTH)
    end

    @population = @counter.flatten.sum
  end

  # Emit the pattern to the console.

  def display
    puts "GENERATION:#{@generation}\tPOPULATION:#{@population}"
    puts "INVALID!"if @invalid

    @counter.each do |row|
      puts row.map { |cell| ((cell == 0 || cell == 2) ? ' ' : 'X') }.join('')
    end
  end

  # Iterate from one generation to the next, returning true if the
  # game of life should continue, and false if it should terminate.

  def iterate
    @generation = @generation + 1
    return false if @generation > @max_generations

    # Update the counter array with new values.
    # First, change each 2 (dying) to a 0 (dead)
    # and each 3 (born) to a 1 (alive)
    @counter.map! { |row| row.map! { |cell| cell >= 2 ? cell-2 : cell } }

    # Now for each cell, count its neighbors and update it

    @population = 0
    @counter.each_index do |rownum|
      @counter[rownum].each_index do |colnum|
        cell_value = @counter[rownum][colnum]

        # If any cell on the border is set, our small algorithm is not
        # smart enough to correctly check its neighbors, so sadly our
        # pattern becomes invalid. We keep going though

        @invalid = true if cell_value > 0 && (
          rownum == 0 || rownum == PATTERN_HEIGHT-1 || colnum == 0 || colnum == PATTERN_WIDTH-1
        )

        # Count the cell's neighbors (not including itself)

        neighbors = @counter[rownum-1..rownum+1].map { |row| row[colnum-1..colnum+1] }.flatten
        neighbor_count = neighbors.inject(0) do |sum, value|
          sum += (value == 1 || value == 2) ? 1 : 0
        end
        neighbor_count = neighbor_count - cell_value

        # Update this cell based on its neighbor count, either leaving it
        # as a 0 or 1, or setting it to 2 (dying) or 3 (being born)

        if cell_value == 0
          if neighbor_count == 3
            cell_value = 3
            @population = @population + 1
          end
        elsif neighbor_count < 2 || neighbor_count > 3
          cell_value = 2
        else
          @population = @population + 1
        end
        @counter[rownum][colnum] = cell_value

      end
    end

    # If every cell is dead, we are done. Otherwise, keep going up to the
    # maximum number of generations

    @population > 0
  end

end

# The following program code makes use of the Pattern class to create,
# iterate on, and display the Game of Life.

def display_banner
  puts " " * 34 + "LIFE"
  puts " " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
  puts
  puts "PLEASE ENTER YOUR STARTING PATTERN, USING SPACE FOR AN EMPTY CELL"
  puts "AND AN 'X' FOR A FILLED CELL. YOUR PATTERN MAY BE UP TO #{PATTERN_HEIGHT} ROWS"
  puts "OF UP TO #{PATTERN_WIDTH} COLUMNS EACH. TYPE 'DONE' WHEN DONE."
  puts "ENTER YOUR PATTERN:"
end

def main

  display_banner

  pattern = Pattern.new
  pattern.get_input
  pattern.display
  pattern.display while pattern.iterate

end

main
