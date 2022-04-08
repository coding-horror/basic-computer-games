class Hello
  def start
    puts  "HELLO.  MY NAME IS CREATIVE COMPUTER.\n\n"
    print "WHAT'S YOUR NAME? "
    user_name = gets.chomp!

    ask_enjoy_question(user_name)

    ask_question_loop(user_name)

    isHonest = ask_for_fee(user_name)
    
    if isHonest
      happy_goodbye(user_name)
    else
      unhappy_goodbye(user_name)
    end
  end
  private
    def get_yes_or_no
      msg = gets.chomp!
      if msg.upcase() == "YES"
          return true, true, msg
      elsif msg.upcase() == "NO"
          return true, false, msg
      else
          return false, false, msg
      end
    end

    def ask_enjoy_question user_name
      print "\nHI THERE, #{user_name}, ARE YOU ENJOYING YOURSELF HERE? "

      while true
        valid, value, msg = get_yes_or_no()

        if valid
          if value
            puts "\nI'M GLAD TO HEAR THAT, #{user_name}."
            break
          else
            puts "\nOH, I'M SORRY TO HEAR THAT, #{user_name}. MAYBE WE CAN"
            puts "BRIGHTEN UP YOUR VISIT A BIT."
            break
          end
        else
          puts "\n#{user_name}, I DON'T UNDERSTAND YOUR ANSWER OF '#{msg}'."
          print "PLEASE ANSWER 'YES' OR 'NO'.  DO YOU LIKE IT HERE? "
        end
      end
    end

    def prompt_for_problems user_name
      puts "\nSAY, #{user_name}, I CAN SOLVE ALL KINDS OF PROBLEMS EXCEPT" 
      puts "THOSE DEALING WITH GREECE.  WHAT KIND OF PROBLEMS DO"
      print "YOU HAVE? (ANSWER SEX, HEALTH, MONEY, OR JOB) "
  
      problem_type = gets.chomp!
      return problem_type
    end

    def prompt_too_much_or_too_little
      answer = gets.chomp!
      if answer.upcase() == "TOO MUCH"
        return true, true
      elsif answer.upcase() == "TOO LITTLE"
        return true, false
      else
        return false, false
      end
    end

    def solve_sex_problem user_name
      print "\nIS YOUR PROBLEM TOO MUCH OR TOO LITTLE? "
      while true
        valid, too_much = prompt_too_much_or_too_little()
        if valid
          if too_much
            puts "\nYOU CALL THAT A PROBLEM?!!  I SHOULD HAVE SUCH PROBLEMS!"
            puts "IF IT BOTHERS YOU, #{user_name}, TAKE A COLD SHOWER."
            break
          else
            puts "\nWHY ARE YOU HERE IN SUFFERN, #{user_name}?  YOU SHOULD BE"
            puts "IN TOKYO OR NEW YORK OR AMSTERDAM OR SOMEPLACE WITH SOME"
            puts "REAL ACTION."
            break
          end
        else
          puts "\nDON'T GET ALL SHOOK, #{user_name}, JUST ANSWER THE QUESTION"
          print "WITH 'TOO MUCH' OR 'TOO LITTLE'.  WHICH IS IT? "
        end
      end
    end

    def solve_health_problem user_name
      puts "\nMY ADVICE TO YOU #{user_name} IS:"
      puts "     1.  TAKE TWO ASPRIN"
      puts "     2.  DRINK PLENTY OF FLUIDS (ORANGE JUICE, NOT BEER!)"
      puts "     3.  GO TO BED (ALONE)"
    end

    def solve_money_problem user_name
      puts "\nSORRY, #{user_name}, I'M BROKE TOO.  WHY DON'T YOU SELL"
      puts "ENCYCLOPEADIAS OR MARRY SOMEONE RICH OR STOP EATING"
      puts "SO YOU WON'T NEED SO MUCH MONEY?"
    end

    def solve_job_problem user_name
      puts "\nI CAN SYMPATHIZE WITH YOU #{user_name}.  I HAVE TO WORK"
      puts "VERY LONG HOURS FOR NO PAY -- AND SOME OF MY BOSSES"
      puts "REALLY BEAT ON MY KEYBOARD.  MY ADVICE TO YOU, #{user_name},"
      puts "IS TO OPEN A RETAIL COMPUTER STORE.  IT'S GREAT FUN."
    end

    def alert_unknown_problem_type user_name, problem_type
      puts "\nOH, #{user_name}, YOUR ANSWER OF #{problem_type} IS GREEK TO ME."
    end

    def ask_question_loop user_name
      while true
        problem_type = prompt_for_problems(user_name)
        if problem_type == "SEX"
          solve_sex_problem(user_name)
        elsif problem_type == "HEALTH"
          solve_health_problem(user_name)
        elsif problem_type == "MONEY"
          solve_money_problem(user_name)
        elsif problem_type == "JOB"
          solve_job_problem(user_name)
        else
          alert_unknown_problem_type(user_name, problem_type)
        end

        while true
          print "\nANY MORE PROBLEMS YOU WANT SOLVED, #{user_name}? "

          valid, value, msg = get_yes_or_no()
          if valid
            if value
              puts "\nWHAT KIND (SEX, MONEY, HEALTH, JOB)"
              break
            else
              return
            end
          else
            puts "\nJUST A SIMPLE 'YES' OR 'NO' PLEASE, #{user_name}."
          end
        end
      end
    end

    def ask_for_fee user_name
      puts "\nTHAT WILL BE $5.00 FOR THE ADVICE, #{user_name}."
      puts "PLEASE LEAVE THE MONEY ON THE TERMINAL."
      sleep(3)
      print "\n\nDID YOU LEAVE THE MONEY? "
  
      while true
        valid, value, msg = get_yes_or_no()
        if valid
            if value
              puts "\nHEY, #{user_name}, YOU LEFT NO MONEY AT ALL!"
              puts "YOU ARE CHEATING ME OUT OF MY HARD-EARNED LIVING."
              puts "\nWHAT A RIP OFF, #{user_name}!!!"
              return false
            else
              puts "\nTHAT'S HONEST, #{user_name}, BUT HOW DO YOU EXPECT"
              puts "ME TO GO ON WITH MY PSYCHOLOGY STUDIES IF MY PATIENTS"
              puts "DON'T PAY THEIR BILLS?"
              return true
            end
        else
          puts "\nYOUR ANSWER OF '#{msg}' CONFUSES ME, #{user_name}."
          print "PLEASE RESPOND WITH 'YES' or 'NO'. "
        end
      end
    end

    def unhappy_goodbye user_name
      puts "\nTAKE A WALK, #{user_name}.\n\n"
    end
  
    def happy_goodbye user_name
      puts "\nNICE MEETING YOU, #{user_name}, HAVE A NICE DAY."
    end
end

if __FILE__ == $0
  hello = Hello.new
  hello.start()
end