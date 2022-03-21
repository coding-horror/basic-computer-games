class Basketball

  def initialize
    @time = 0
    @score = [0, 0]
    @defense_choices = [6, 6.5, 7, 7.5]
    @shot = nil
    @shot_choices = [0, 1, 2, 3, 4]
    @z1 = nil

    puts "Υou will be Dartmouth captain and playmaker."
    puts "Call shots as follows:"
    puts "1. Long (30ft.) Jump Shot"
    puts "2. Short (15 ft.) Jump Shot"
    puts "3. Lay up; 4. Set Shot\n\n"
    
    puts "Both teams will use the same defense. Call Defense as follows:"
    puts "6. Press"
    puts "6.5 Man-to-Man"
    puts "7. Zone"
    puts "7.5 None.\n\n"

    puts "To change defense, just type 0 as your next shot."

    @defense = get_defense @defense_choices


    puts "\nChoose your opponent? "
    @opponent = gets.chomp!
    start_of_period
  end

  def dartmouth_ball
    while true
      puts "Your shot? " 
      @shot = gets.chomp!
      if @shot_choices.include? @shot.to_i
        break
      end
    end

    if @time < 100 or Random.rand < 0.5
      if @shot.to_i == 1 or @shot.to_i == 2
        dartmouth_jump_shot
      else
        dartmouth_non_jump_shot
      end
    else
      if @score[0] != @score[1]
        puts "\n   ***** End Of Game *****" 
        puts "Final Score: Dartmouth: #{@score[1].to_s}  #{@opponent}: #{@score[0].to_s}"
      else
        puts "\n   ***** End Of Second Half *****"
        puts "Score at end of regulation time:"
        puts "     Dartmouth: #{@score[1]} #{@opponent}: #{@score[0]}"

        puts "Begin two minute overtime period" 
        @time = 93
        start_of_period
      end
    end
  end

  def add_points team, points
    @score[team] += points
    print_score
  end

  def ball_passed_back
    puts "Ball passed back to you"
  end

  def change_defense
    @defense = get_defense @defense_choices, "new"
    dartmouth_ball()
  end

  def foul_shots team
    puts "Shooter fouled.  Two shots."
    if Random.rand > 0.49
      if Random.rand > 0.75
        puts "Both shots missed."
      else
        puts "Shooter makes one shot and misses one."
        @score[team] += 1
      end
    else
      puts "Shooter makes both shots."
      @score[team] += 2
    end
  end

  def halftime
    puts "\n   ***** End of first half *****\n" 
    print_score
    start_of_period
  end

  def print_score
    puts "Score:  #{@score[1]} to #{@score[0]}\n" 
  end

  def start_of_period
    puts "Center jump" 
    if Random.rand > 0.6
      puts "Dartmouth controls the tap.\n" 
      dartmouth_ball()
    else
      puts "#{@opponent} controls the tap.\n" 
      opponent_ball
    end
  end

  def two_minute_warning
    puts "   *** Two minutes left in the game ***" 
  end

  def dartmouth_jump_shot
    @time += 1
    if @time == 50
      halftime
    elsif @time == 92
      two_minute_warning
    end
    puts "Jumpshot"

    if Random.rand > 0.341 * @defense.to_i / 8
      if Random.rand > 0.682 * @defense.to_i / 8
        if Random.rand > 0.782 * @defense.to_i / 8
          if Random.rand > 0.843 * @defense.to_i / 8
            puts "Charging foul. Dartmouth loses ball.\n" 
            opponent_ball
          else
            foul_shots 1
            opponent_ball
          end
        else
          if Random.rand > 0.5
            puts "Shot is blocked. Ball controlled by #{@opponent}\n"
            opponent_ball
          else
            puts "Shot is blocked. Ball controlled by Dartmouth."
            dartmouth_ball()
          end
        end
      else
        puts "Shot is off target." 
        if @defense.to_i / 6 * Random.rand > 0.45
          puts "Rebound to " + @opponent + "\n" 
          opponent_ball
        else
          puts "Dartmouth controls the rebound." 
          if Random.rand > 0.4
            if @defense.to_i == 6 and Random.rand > 0.6
              puts "Pass stolen by #{@opponent}, easy lay up" 
              add_points(0, 2)
              dartmouth_ball()
            else
              ball_passed_back
            end
          else
            puts "\n" 
            dartmouth_non_jump_shot
          end
        end
      end
    else
      puts "Shot is good." 
      add_points(1, 2)
      opponent_ball
    end
  end

  def dartmouth_non_jump_shot
    @time += 1
    if @time == 50
      halftime
    elsif @time == 92
      two_minute_warning
    end

    if @shot.to_i == 4
      puts "Set shot." 
    elsif @shot.to_i == 3
      puts "Lay up." 
    elsif @shot.to_i == 0
      change_defense
    end

    if 7 / @defense.to_i * Random.rand > 0.4
      if 7 / @defense.to_i * Random.rand > 0.7
        if 7 / @defense.to_i * Random.rand > 0.875
          if 7 / @defense.to_i * Random.rand > 0.925
            puts "Charging foul. Dartmouth loses the ball.\n"
            opponent_ball
          else
            puts "Shot blocked. #{@opponent}'s ball.\n" 
            opponent_ball
          end
        else
          foul_shots(1)
          opponent_ball
        end
      else
        puts "Shot is off the rim." 
        if Random.rand > 2 / 3
          puts "Dartmouth controls the rebound."
          if Random.rand > 0.4
            puts "Ball passed back to you.\n"
            dartmouth_ball()
          else
            dartmouth_non_jump_shot
          end
        else
          puts "#{@opponent} controls the rebound.\n"
          opponent_ball
        end
      end
    else
      puts "Shot is good. Two points."
      add_points(1, 2)
      opponent_ball
    end
  end

  def opponent_jumpshot
    puts "Jump Shot." 
    if 8 / @defense.to_i * Random.rand > 0.35
      if 8 / @defense.to_i * Random.rand > 0.75
        if 8 / @defense.to_i * Random.rand > 0.9
          puts "Offensive foul. Dartmouth's ball.\n" 
          dartmouth_ball()
        else
          foul_shots(0)
          dartmouth_ball()
        end
      else
        puts "Shot is off the rim." 
        if @defense.to_i / 6 * Random.rand > 0.5
          puts "#{@opponent} controls the rebound." 
          if @defense.to_i == 6
            if Random.rand > 0.75
              puts "Ball stolen. Easy lay up for Dartmouth." 
              add_points(1, 2)
              opponent_ball
            else
              if Random.rand > 0.5
                puts ""
                opponent_non_jumpshot
              else
                puts "Pass back to #{@opponent} guard.\n" 
                opponent_ball
              end
            end
          else
            if Random.rand > 0.5
              opponent_non_jumpshot
            else
              puts "Pass back to #{@opponent} guard.\n" 
              opponent_ball
            end
          end
        else
          puts "Dartmouth controls the rebound.\n" 
          dartmouth_ball()
        end
      end
    else
      puts "Shot is good." 
      add_points(0, 2)
      dartmouth_ball()
    end
  end

  def opponent_non_jumpshot
    if @z1 > 3
        puts "Set shot." 
    else
      puts "Lay up" 
    end

    if 7 / @defense.to_i * Random.rand > 0.413
      puts "Shot is missed." 
      if @defense.to_i / 6 * Random.rand > 0.5
        puts "#{@opponent} controls the rebound." 
        if @defense.to_i == 6
            if Random.rand > 0.75
              puts "Ball stolen. Easy lay up for Dartmouth." 
              add_points(1, 2)
              opponent_ball
            else
              if Random.rand > 0.5
                  puts "" 
                  opponent_non_jumpshot
              else
                puts "Pass back to #{@opponent} guard.\n" 
                opponent_ball
              end
            end
        else
          if Random.rand > 0.5
            puts "" 
            opponent_non_jumpshot
          else
            puts "Pass back to #{@opponent} guard\n" 
            opponent_ball
          end
        end
      else
        puts "Dartmouth controls the rebound.\n"
        dartmouth_ball()
      end
    else
      puts "Shot is good." 
      add_points(0, 2)
      dartmouth_ball()
    end
  end

  def opponent_ball
    @time += 1
    if @time == 50
      halftime
    end

    @z1 = 10 / 4 * Random.rand + 1
    
    if @z1 > 2
      opponent_non_jumpshot
    else
      opponent_jumpshot
    end
  end

  def get_defense defense_choices, type = "starting"
    while true
      puts "Your #{type} defense will be? [6 - Press] [6.5 - Man-to-Man] [7 - Zone] [7.5 None]"
      defense = gets.chomp!
      if defense_choices.include? defense.to_i
        break
      end
    end 

    return defense.to_i
  end
end

b = Basketball.new
