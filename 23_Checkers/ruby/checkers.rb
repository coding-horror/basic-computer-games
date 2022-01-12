#!/usr/bin/env ruby

# Checkers in Ruby, Version 1
#
# This version of the game attempts to preserve the underlying
# algorithm(s) and feel of the BASIC version while using more modern
# coding techniques.  Specifically:
#
# 1. The major data structures (the board and current move, known as S
#    and R in the BASIC version) have been turned into classes.  In
#    addition, I made a class for coordinates so that you don't always
#    have to deal with pairs of numbers.
#
# 2. Much of the functionality associated with this data has been moved
#    into methods of these classes in line with the philosophy that objects
#    are smart data.
#
# 3. While I've kept the board as a single object (not global, though),
#    this program will create many Move objects (i.e. copies of the move
#    under consideration) rather than operating on a single global
#    instance.
#
# 4. The rest of the code has been extracted into Ruby functions with
#    all variables as local as reasonably possible.
#
# 5. Pieces are now represented with Symbols instead of integers; this
#    made it *much* easier to understand what was going on.
#
# 6. There are various internal sanity checks.  They fail by throwing
#    a string as an exception.  (This is generally frowned upon if
#    you're going to catch the exception later but we never do that;
#    an exception here means a bug in the software and the way to fix
#    that is to fix the program.)
#
# And probably other stuff.
#


# Note: I've ordered the various major definitions here from (roughly)
# general to specific so that if you read the code starting from the
# beginning, you'll (hopefully) get a big-picture view first and then
# get into details.  Normally, I'd order things by topic and define
# things before using them, which is a better ordering.  So in this
# case, do what I say and not what I do.


#
# Some global constants
#

BOARD_TEXT_INDENT = 30  # Number of spaces to indent the board when printing

# Various constants related to the game of Checkers.
#
# (Yes, they're obvious but if you see BOARD_WIDTH, you know this is
# related to board dimensions in a way that you wouldn't if you saw
# '8'.)
BOARD_WIDTH = 8
KING_ROW_X = 0
KING_ROW_Y = BOARD_WIDTH - 1



# This is the mainline routine of the program.  Ruby doesn't require
# that you put this in a function but this way, your local variables
# are contained here.  It's also neater, IMO.
#
# The name 'main' isn't special; it's just a function.  The last line
# of this program is a call to it.
def main
  print <<EOF
                                Checkers
         Inspiration: Creative Computing  Morristown, New Jersey


          This is the game of checkers.  The computer is X,
          and you are O.  The computer will move first.
          Squares are referred to by a coordinate system.
                    (0,0) is the lower left corner
                    (0,7) is the upper left corner
                    (7,0) is the lower right corner
                    (7,7) is the upper right corner
          The computer will tell you when you have another
          jump.

          Enter a blank line if you cannot make a move.  If this
          not after a jump, it means you have lost the game.



EOF

  board = Board.new
  winner = game_loop(board)

  puts board.text
  puts
  puts "#{winner ? 'I' : 'You'} win."
end


# This is the main game loop.  Returns true if I win and false if the
# player wins.  Each of my_turn and players_turn return false if they
# could not move.  (Recall that the game ends when one player can't
# move.)
def game_loop(board)
  while true
    my_turn(board)      or return false
    players_turn(board) or return true
  end
end


#
# My (i.e. computer's) turn
#

# Play my turn, returning true if it could make at least one move and
# false otherwise.  This is pretty simple because the heavy lifting is
# done by the methods of the Board and Move classes.
def my_turn(board, jumpStart = nil)
  # Print the initial board
  puts board.text

  # Find the candidate move.  If this is a move after a jump, the
  # starting point will be in jumpStart and we use only that.
  # Otherwise, we check all available pieces.
  bestMove = Move.invalid
  candidates = jumpStart ? [jumpStart] : board.my_pieces
  for coord in candidates
    bestMove = bestMove.betterOf( board.bestMoveFrom(coord, !!jumpStart) )
  end

  # If we can't find a move, we're done
  if !bestMove.valid?
    puts "I can't make another #{jumpStart ? 'jump' : 'move'}."
    return false
  end

  # Do the move
  puts "My move: #{bestMove}"
  board.make_move!(bestMove)

  # Repeat (recursively) if we can make another jump
  my_turn(board, bestMove.to) if bestMove.jump?

  # No loss yet!
  return true
end




#
# Player's turn
#


# Prompt the player for a move and then play it.  If the player can
# make multiple moves (i.e. jump), repeat. Return true if at least one
# move was played, false otherwise.
#
# Note that this does not enforce all of the rules of the game; it
# assumes the player knows the rules and will stick to them.  In
# particular, the player is not required to jump if possible.  This
# behaviour was inherited from the BASIC version but I've made some
# improvements so we will catch most illegal moves; in that case, the
# player is prompted for another move.
def players_turn(board)
  from = nil

  while true
    move = get_player_move(board, from)

    # If the player declines to enter a move, we're done.  If this is
    # the first move ('from' will be nil in this case), it's a forfeit
    # and we return false to indicate the player has lost.  But if
    # it's a jump, the player was able to move and therefor has not
    # lost so we return true.  (Note: this requires the player to not
    # cheat, which is kind of a bug but that's how the original worked
    # as well.)
    return false    if !from && !move
    return true     if from && !move

    board.make_move!(move)
    return true unless move.jump?

    # If the player can jump again, repeat from the new position.
    from = move.to
  end

  # Can't reach here

end


# Prompt the player for a move, read it, check if it's legal and
# return it if it is or try again if it isn't.  If the player declines
# to move, returns nil.  If this is a jump following a previous jump,
# the second argument will be the source (i.e. Move.from) and we don't
# ask for it here..
def get_player_move(board, jumpFrom = nil)

  puts "You can jump again!" if jumpFrom

  while true
    puts board.text
    puts "Enter your move:"

    puts "From? #{jumpFrom}" if jumpFrom
    from = jumpFrom || read_coord("From? ")

    to = read_coord("To? ") unless !from

    # If the player entered a blank line, it's a forfeit
    if !from || !to
      return nil if jumpFrom || confirm_quit()
    end

    move = Move.new(from, to)
    return move if board.legal_move?(false, move)

    print "\nThis move is not legal!\n\n"
  end
end


# Prompt the player for the x,y coordinates of a piece on the board,
# repeating until a valid coordinate is provided and return it.  If
# the player enters a blank line, returns nil; this is interpreted as
# declining to move.
def read_coord(prompt)
  while true
    print prompt
    STDOUT.flush

    line = (STDIN.gets || "").strip
    return nil if line == ''

    coord = parse_coord(line)

    return coord if coord
    puts "Invalid input; try again!"
  end
end

# Ask the player if they wish to quit; return true if they do, false
# otherwise.  Tends to assume false unless given an explicit yes.
def confirm_quit
  print "Really forfeit (y/n) "
  STDOUT.flush
  line = STDIN.gets.strip

  return !!(line =~ /^y(es)?$/i)     # For extra credit, explain why I use '!!'
end

# Parse a string containing x,y coordinates to produce and return a
# new Coord object.  Returns nil if the string is not a valid
# coordinate.
def parse_coord(coord)
  coord = coord.gsub(/\s/, '')
  return nil unless coord =~ /^\d,\d$/

  parts = coord.split(/,/)
  result = Coord.new(parts[0].to_i, parts[1].to_i)
  return nil unless result.valid?
  return result
end



#
# Classes
#


# Class to represent the game board and all of the pieces on it.
#
# Like the BASIC version, the board is represented using an 8 by 8
# array.  However, instead of using numbers between -2 and 2, we
# represent board squares with Ruby symbols:
#
#   - :_ (underscore) is an empty square
#   - :o and :x (lowercase) are ordinary pieces for their respective sides
#   - :O and :X (uppercase) are kings for their respective sides
#
# Most of the smarts of the game are also here as methods of this
# class.  So my_turn() doesn't (e.g.) go through the board squares
# looking for the best move; it asks the board for it by calling
# bestMove().
#
class Board

  # Set up the board to the starting position
  def initialize
    @board = []

    col1 = %i{o _ o _ _ _ x _}
    col2 = %i{_ o _ _ _ x _ x}
    4.times {
      @board += [col1.dup, col2.dup]
    }
  end

  # We use [] and []= (the array/hash get and set operators) to get
  # individual board pieces instead of accessing it directly; this
  # prevents errors where you get the row and column wrong in some
  # places and not others.  They're private because it turns out that
  # nothing outside of this class needs them but we could make them
  # public without a lot of trouble.  The position must be a Coord
  # object.
  private
  def [](coord)
    return nil unless coord.valid?
    return @board[coord.x][coord.y]
  end

  def []=(coord, value)
    # Sanity checks
    raise "Invalid coordinate: #{coord}" unless coord.valid?
    raise "Invalid board value: #{value}" unless %i{_ x o X O}.include?(value)

    @board[coord.x][coord.y] = value
  end

  public

  # Return an ASCII picture of the board.  This is what gets printed
  # between turns.
  def text(indent = BOARD_TEXT_INDENT)
    result = ""

    glyph = {_: '.'}    # Replace some symbols when printing for readability

    for y in (0 .. 7).to_a.reverse
      result += ' '*indent + y.to_s + ' '
      for x in (0 .. 7)
        result += glyph.fetch(self[ Coord.new(x, y) ]) {|k| k.to_s}
      end
      result += "\n"
    end

    result += ' '*indent + '  ' + (0..7).map{|n| n.to_s}.join

    return result + "\n\n"
  end

  # Answer various questions about positions on the board
  def mine_at?(coord)
    return %i{x X}.include? self[coord]
  end

  def king_at?(coord)
    return %i{X O}.include? self[coord]
  end

  def opponent_at?(coord)
    return %i{o O}.include? self[coord]
  end

  def empty_at?(coord)
    return self[coord] == :_
  end

  # Return a list of all valid positions on the board
  def all_coords
    coords = []
    for x in 0 .. BOARD_WIDTH - 1
      for y in 0 .. BOARD_WIDTH - 1
        coords.push Coord.new(x, y)
      end
    end

    return coords
  end

  # Return a list of all coords containing my (i.e. X's) pieces.
  def my_pieces
    all_coords.select{|coord| mine_at?(coord)}
  end

  private

  # Return a list of all valid positions on the board.  (To do: skip
  # all light-coloured squares, since they will never hold a piece.)
  def all_coords
    coords = []
    for x in 0 .. BOARD_WIDTH - 1
      for y in 0 .. BOARD_WIDTH - 1
        coords.push Coord.new(x, y)
      end
    end

    return coords
  end

  public

  # Test if the move 'move' is legal; tests for me (i.e. the computer)
  # if 'is_me' is true and for the human player if 'is_me' is false.
  def legal_move?(is_me, move)
    # If the move isn't valid, it's not legal.
    return false unless move.valid?

    # There's a piece belonging to me at the source
    return false unless mine_at?(move.from) == is_me

    # The destination is empty
    return false unless empty_at?(move.to)

    # The source holds one of the players' pieces
    return false if empty_at?(move.from) || mine_at?(move.from) != is_me

    # moving forward if not a king
    if !king_at?(move.from)
      return false if is_me && move.to.y > move.from.y
      return false if !is_me && move.to.y < move.from.y
    end

    # If jumping, that there's an opponent's piece between the start
    # and end.
    return false if
      move.jump? &&
      (empty_at?(move.midpoint) || opponent_at?(move.midpoint) != is_me)

    # Otherwise, it's legal
    return true
  end

  # Perform 'move' on the board.  'move' must be legal; the player
  # performing it is determined by the move's starting ('from')
  # position.
  def make_move!(move)
    piece = self[move.from]

    # Sanity check
    raise "Illegal move: #{move}" unless legal_move?(piece.downcase == :x,move)

    # Promote the piece if it's reached the end row
    piece = piece.upcase if
      (piece == :x && move.to.y == KING_ROW_X) ||
      (piece == :o && move.to.y == KING_ROW_Y)

    # And do the move
    self[move.to] = piece
    self[move.from] = :_

    # Remove the piece jumped over if this is a jump
    self[move.midpoint] = :_ if move.jump?
  end


  # Return the best (i.e. likely to win) move possible for the
  # piece (mine) at 'coord'.
  def bestMoveFrom(coord, mustJump)
    so_far = Move.invalid
    return so_far unless coord.valid?

    offsets = [ [-1, -1], [1, -1]]
    offsets += [ [-1, 1], [1, 1]] if king_at?(coord)

    for ofx, ofy in offsets
      new_coord = coord.by(ofx, ofy)

      if opponent_at?(new_coord)
        new_coord = new_coord.by(ofx, ofy)
      elsif mustJump
        next
      end

      next unless new_coord.valid?

      so_far = so_far.betterOf( ratedMove(coord, new_coord) )
    end

    return so_far
  end


  # Create and return a move for *me* from Coords 'from' to 'to' with
  # its 'rating' set to how good the move looks according to criteria
  # used by the BASIC version of this program.  If the move is
  # illegal, returns an invalid Move object.
  def ratedMove(from, to)
    return Move.invalid unless legal_move?(true, Move.new(from, to))

    rating = 0

    # +2 if it promotes this piece
    rating += 2 if to.y == 0

    # +50 if it takes the opponent's piece.  (Captures are mandatory
    # so we ensure that a capture will always outrank a non-capture.)
    rating += 4 if (from.y - to.y).abs == 2

    # -2 if we're moving away from the king row
    rating -= 2 if from.y == BOARD_WIDTH - 1

    # +1 for putting the piece against a vertical boundary
    rating += 1 if to.x == 0 || to.x == BOARD_WIDTH - 1

    # +1 for each friendly piece behind this one
    [-1, 1].each {|c|
      rating += 1 if mine_at?( to.by(c, -1) )
    }

    # -2 for each opponent's piece that can now capture this one.
    # (This includes a piece that may be captured when moving here;
    # this is a bug.)
    [ -1, 1].each {|c|
      there = to.by(c, -1)
      opposite = to.by(-c, 1)
      rating -= 2 if
        opponent_at?(there) && (empty_at?(opposite) || opposite == from)
    }

    return Move.new(from, to, rating)
  end
end


# Class to hold the X and Y coordinates of a position on the board.
#
# Coord objects are immutable--that is, they never change after
# creation.  Instead, you will always get a modified copy back.
class Coord

  # Coordinates are readable
  attr_reader :x, :y

  # Initialize
  def initialize(x, y)
    @x, @y = [x,y]
  end

  # Test if this move is on the board.
  def valid?
    return x >= 0 && y >= 0 && x < BOARD_WIDTH && y < BOARD_WIDTH
  end

  # Test if this Coord is equal to another Coord.
  def ==(other)
    return other.class == self.class && other.x == @x && other.y == y
  end

  # Return a string that describes this Coord in a human-friendly way.
  def to_s
    return "(#{@x},#{@y})"
  end

  # Return a new Coord whose x and y coordinates have been adjusted by
  # arguments 'x' and 'y'.
  def by(x, y)
    return Coord.new(@x + x, @y + y)
  end
end


# Class to represent a move by a player between two positions,
# possibly with a rating that can be used to select the best of a
# collection of moves.
#
# An (intentionally) invalid move will have a value of nil for both
# 'from' and 'to'.  Most methods other than 'valid?' assume the Move
# is valid.
class Move
  # Readable fields:
  attr_reader :from, :to, :rating

  # The initializer; -99 is the lowest rating from the BASIC version
  # so we use that here as well.
  def initialize(from, to, rating = -99)
    @from, @to, @rating = [from, to, rating]

    # Sanity check; the only invalid Move tolerated is the official
    # one (i.e. with nil for each endpoint.)
    raise "Malformed Move: #{self}" if @from && @to && !valid?
  end

  # Return an invalid Move object.
  def self.invalid
    return self.new(nil, nil, -99)
  end

  # Return true if this is a valid move (i.e. as close to legal as we
  # can determine without seeing the board.)
  def valid?
    # Not valid if @from or @to is nil
    return false unless @from && @to

    # Not valid unless both endpoints are on the board
    return false unless @from.valid? && @to.valid?

    # Not valid unless it's a diagonal move by 1 or 2 squares
    dx, dy = delta
    return false if dx.abs != dy.abs || (dx.abs != 1 && dx.abs != 2)

    # Otherwise, valid
    return true
  end

  # Return true if this move is a jump, false otherwise
  def jump?
    return valid? && magnitude() == 2
  end

  # Return the coordinates of the piece being jumped over by this
  # move.
  def midpoint
    raise "Called 'midpoint' on a non-jump move!" unless jump?
    dx, dy = delta
    return @from.by(dx / dx.abs, dy / dy.abs)
  end

  # Return the better-rated of self or otherMove.
  def betterOf(otherMove)
    return otherMove if !valid?
    return rating > otherMove.rating ? self : otherMove
  end

  # Return a human-friendly string representing this move.
  def to_s
    return "[NOMOVE]" if !@from && !@to     # Well-known invalid move

    jumpover = jump? ?
                 "-> #{midpoint} ->"
               : "->"

    return "#{@from} #{jumpover} #{to}#{valid? ? '' : ' [INVALID]'}"
  end

  private

  # Return the distance (x, y) between the 'from' and 'to' locations.
  def delta
    return [to.x - from.x, to.y - from.y]
  end

  # Return the number of squares this move will take the piece (either
  # 1 or 2).
  def magnitude
    # Note: we assume that this move is a legal move (and therefore
    # diagonal); otherwise, this may not be correct.
    return (to.x - from.x).abs
  end
end



# Start the game
main()
