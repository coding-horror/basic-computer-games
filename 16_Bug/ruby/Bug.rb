class BugGame
  YES = ["Y", "y", "Yes", "YES"]
  NO  = ["N", "n", "No", "NO"]

  YOU = { body: 0, neck: 0, head: 0, antennaes: 0, legs: 0, arms: 0, name: "YOU" }
  I   = { body: 0, neck: 0, head: 0, antennaes: 0, legs: 0, arms: 0, name: "I" }
  def initialize
    puts "The Game Bug"
    puts "I HOPE YOU ENJOY THIS GAME."
    instructions
  end

  def run
    loop do
      # YOU FIRST
      play YOU
      if is_completed? YOU
        puts "\n\n\n\nYOU WON"
        break
      end
      puts "\n"

      # I SECOND
      play I
      if is_completed? I
        puts "\n\n\n\nI WON"
        break
      end

      loop do
        puts "Do you want the pictures? [Y,y,Yes,YES] [N,n,No,NO]"
        answer = gets.chomp!
        if YES.include?(answer) || NO.include?(answer)
          if YES.include?(answer)
            puts "--- YOUR BUG ---"
            print_bug YOU
            puts "\n\n--- MY BUG ---"
            print_bug I
          end
          break
        end
      end
    end
  end

  private
    def play player
      number = Random.rand(1..6)
      case number
      when 1
        if player[:body].eql? 0
          player[:body] = 1
          puts "#{player[:name]} have acquired a body"
        else
          puts "#{player[:name]} already have a body"
        end
      when 2
        one_part player, :neck, :body
      when 3
        one_part player, :head, :neck
      when 4
        two_parts player, :antennaes, :head
      when 5
        two_parts player, :legs, :body
      when 6
        two_parts player, :arms, :body
      end
    end

    def one_part player, part, part_needed
      if player[part].eql? 0
        if player[part_needed].eql? 0
          puts "#{player[:name]} need to have a #{part_needed.to_s} first"
        else
          player[part] = 1
          puts "#{player[:name]} have acquired a #{part.to_s}"
        end
      else
        puts "#{player[:name]} already have a #{part.to_s}"
      end
    end

    def two_parts player, part, part_needed
      if player[part].eql? 0
        if player[part_needed].eql? 0
          puts "#{player[:name]} need to have a #{part_needed.to_s} first"
        else
          player[part] = 1
          puts "#{player[:name]} have acquired first #{part.to_s.chop}"
        end
      else
        if player[part].eql? 2
          puts "#{player[:name]} already have 2 #{part.to_s}"
        else
          player[part] = 2
          puts "#{player[:name]} have acquired second #{part.to_s.chop}"
        end
      end
    end

    def is_completed? player
      player[:body].eql?(1) && player[:neck].eql?(1) && player[:head].eql?(1) && player[:antennaes].eql?(2) && player[:legs].eql?(2) && player[:arms].eql?(2)
    end

    def print_bug player
      antennae_and_leg player, :antennaes
      head player
      neck player
      body player
      antennae_and_leg player, :legs
    end

    def instructions
      loop do
        puts "Do you want an instruction? [Y,y,Yes,YES] [N,n,No,NO]"
        answer = gets.chomp!

        if YES.include?(answer) || NO.include?(answer)
          if YES.include?(answer)
            puts "THE OBJECT OF BUG IS TO FINISH YOUR BUG BEFORE I FINISH"
            puts "MINE. EACH NUMBER STANDS FOR A PART OF THE BUG BODY."
            puts "I WILL ROLL THE DIE FOR YOU, TELL YOU WHAT I ROLLED FOR YOU"
            puts "WHAT THE NUMBER STANDS FOR, AND IF YOU CAN GET THE PART."
            puts "IF YOU CAN GET THE PART I WILL GIVE IT TO YOU."
            puts "THE SAME WILL HAPPEN ON MY TURN."
            puts "IF THERE IS A CHANGE IN EITHER BUG I WILL GIVE YOU THE"
            puts "OPTION OF SEEING THE PICTURES OF THE BUGS."
            puts "THE NUMBERS STAND FOR PARTS AS FOLLOWS:\n\n\n"
            puts "Number    Part          Required Part #"
            puts "1         Body          1"
            puts "2         Neck          1"
            puts "3         Head          2"
            puts "4      (2)antennaes     3"
            puts "5      (2)arms          1"
            puts "6      (2)Legs          1"
          end
          break
        end
      end
    end

    def antennae_and_leg player, part
      if !player[part].eql? 0
        for i in (1...5) do
          if player[part].eql? 1
            puts "     N"
          else
            puts "     N    N"
          end
        end
      end
    end

    def head player
      if !player[:head].eql? 0
        puts "     NNNNNNN"
        puts "     N     N"
        puts "     N O O N"
        puts "     N     N"
        puts "     N  V  N"
        puts "     NNNNNNN"
      end
    end

    def neck player
      if !player[:neck].eql? 0
        puts "     N  N"
      end
    end

    def body player
      if !player[:body].eql? 0
        puts "     NNNNNNNNN"
        for i in (1...5) do
          if i.eql? 2
            if player[:arms].eql? 1
              puts "NNNNNN       N"
            elsif player[:arms].eql? 2
              
              puts "NNNNNN       NNNNNN"
            end
          else
            puts "     N       N"
          end
        end
        puts "     NNNNNNNNN"
      end
    end
end


if __FILE__ == $0
  bug = BugGame.new
  puts "\n\nNOW WE START THE GAME\n\n"
  bug.run
end