require "set"

class Battlefield
  TOTAL_OUTPOSTS = 25
  TOTAL_PLATOONS = 4

  class << self
    def with_random_platoon_placements
      new((1..TOTAL_OUTPOSTS).to_a.sample(4))
    end

    def valid_platoon_placement?(i)
      i.between?(1, TOTAL_OUTPOSTS)
    end
  end

  def initialize(platoon_placements)
    @platoon_placements = Set.new(platoon_placements)
  end

  # Fires a missile at the specified outpost. Returns true if it hits a
  # platoon, false otherwise.
  def fire_missile_at(outpost)
    !@platoon_placements.delete?(outpost).nil?
  end

  def remaining_platoons_count
    @platoon_placements.size
  end
end

class Game
  def initialize(player_battlefield:, computer_battlefield:)
    @player_battlefield = player_battlefield
    @computer_battlefield = computer_battlefield
  end

  def play
    player_choice = get_integer_answer_to("WHERE DO YOU WISH TO FIRE")

    if @computer_battlefield.fire_missile_at(player_choice)
      print_player_success_message
      return if @computer_battlefield.remaining_platoons_count == 0
    else
      puts "HA, HA YOU MISSED. MY TURN NOW:"
    end
    puts

    computer_choice = rand(1..Battlefield::TOTAL_OUTPOSTS)

    if @player_battlefield.fire_missile_at(computer_choice)
      print_computer_success_message(computer_choice)
    else
      puts "I MISSED YOU, YOU DIRTY RAT. I PICKED #{computer_choice}. YOUR TURN:"
    end
    puts
  end

  def finished?
    @player_battlefield.remaining_platoons_count == 0 ||
      @computer_battlefield.remaining_platoons_count == 0
  end

  private

  def get_integer_answer_to(question)
    puts question
    $stdin.gets.strip.to_i
  end

  def print_computer_success_message(computer_choice)
    if @player_battlefield.remaining_platoons_count == 0
      puts "YOU'RE DEAD. YOUR LAST OUTPOST WAS AT #{computer_choice}. HA, HA, HA."
      puts "BETTER LUCK NEXT TIME."
      return
    end

    puts "I GOT YOU. IT WON'T BE LONG NOW. POST #{computer_choice} WAS HIT."

    case @player_battlefield.remaining_platoons_count
    when 1
      puts "YOU HAVE ONLY ONE OUTPOST LEFT"
    when 2
      puts "YOU HAVE ONLY TWO OUTPOSTS LEFT"
    when 3
      puts "YOU HAVE ONLY THREE OUTPOSTS LEFT"
    end
  end

  def print_player_success_message
    if @computer_battlefield.remaining_platoons_count == 0
      puts "YOU GOT ME, I'M GOING FAST. BUT I'LL GET YOU WHEN"
      puts "MY TRANSISTO&S RECUP%RA*E!"
      return
    end

    puts "YOU GOT ONE OF MY OUTPOSTS!"

    case @computer_battlefield.remaining_platoons_count
    when 1
      puts "THREE DOWN, ONE TO GO."
    when 2
      puts "TWO DOWN, TWO TO GO."
    when 3
      puts "ONE DOWN, THREE TO GO."
    end
  end
end

INTRODUCTION = <<~TEXT
                                   BOMBARDMENT
                 CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



  YOU ARE ON A BATTLEFIELD WITH 4 PLATOONS AND YOU
  HAVE 25 OUTPOSTS AVAILABLE WHERE THEY MAY BE PLACED.
  YOU CAN ONLY PLACE ONE PLATOON AT ANY ONE OUTPOST.
  THE COMPUTER DOES THE SAME WITH ITS FOUR PLATOONS.

  THE OBJECT OF THE GAME IS TO FIRE MISSLES AT THE
  OUTPOSTS OF THE COMPUTER.  IT WILL DO THE SAME TO YOU.
  THE ONE WHO DESTROYS ALL FOUR OF THE ENEMY'S PLATOONS
  FIRST IS THE WINNER.

  GOOD LUCK... AND TELL US WHERE YOU WANT THE BODIES SENT!

  TEAR OFF MATRIX AND USE IT TO CHECK OFF THE NUMBERS.
TEXT

def print_matrix
  (1..25).each do |i|
    print i
    print i % 5 == 0 ? "\n" : "\t"
  end
end

def ask_for_platoon_placements
  puts "WAHT ARE YOUR FOUR POSITIONS?"
  platoon_placements = $stdin.gets.strip.split(",").map(&:to_i)

  if platoon_placements.size == 4 &&
      platoon_placements.all? { |i| Battlefield.valid_platoon_placement?(i) }
    platoon_placements
  else
    puts "POSITIONS SHOULD BE BETWEEN 1 TO 25, DELIMITED BY COMMA."
    ask_for_platoon_placements
  end
end

def start_game
  puts INTRODUCTION
  puts
  print_matrix
  puts
  platoon_placements = ask_for_platoon_placements
  puts

  game = Game.new(
    player_battlefield: Battlefield.new(platoon_placements),
    computer_battlefield: Battlefield.with_random_platoon_placements
  )

  game.play until game.finished?
end

start_game
