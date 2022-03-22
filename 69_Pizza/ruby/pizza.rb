class Pizza
  STREET_NAMES   = ['1', '2', '3', '4']
  CUSTOMER_NAMES = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P']
  def start
    player_name = print_instructions
    more_directions = yes_no_prompt("DO YOU NEED MORE DIRECTIONS?")

    if more_directions
      print_more_directions(player_name)

      understand = yes_no_prompt("UNDERSTAND?")

      if not understand
        print("\nTHIS JOB IS DEFINITELY TOO DIFFICULT FOR YOU. THANKS ANYWAY")
        return
      end

      puts "\n\nGOOD.  YOU ARE NOW READY TO START TAKING ORDERS. \n"
      puts "GOOD LUCK!!"
    end

    while true
      num_turns = 5
      play_game(num_turns, player_name)

      more = yes_no_prompt("DO YOU WANT TO DELIVER MORE PIZZAS?")
      if not more
        puts "\nO.K. #{player_name}, SEE YOU LATER!"
        return
      end
    end
  end

  private
    def print_street index
      street_number = 3 - index
  
      street_name = STREET_NAMES[street_number]
      line = street_name
  
      space = " " * 5
      for customer_index in (0...4)
        line += space
        customer_name = CUSTOMER_NAMES[4 * street_number + customer_index]
        line += customer_name
      end
      line += space
      line += street_name
      puts "#{line}"
    end
        
    def print_map
      puts "MAP OF THE CITY OF HYATTSVILLE\n\n" 
      print(" -----1-----2-----3-----4-----")
      for i in (0...4)
        for _ in (0...4)
            puts "-"
        end
        print_street(i)
      end
      print(" -----1-----2-----3-----4-----")
    end

    def print_instructions
      puts "PIZZA DELIVERY GAME \n\n"
      print "WHAT IS YOUR FIRST NAME? "
      player_name = gets.chomp!
      puts "\nHi, #{player_name}. IN THIS GAME YOU ARE TO TAKE ORDERS"
      puts "FOR PIZZAS.  THEN YOU ARE TO TELL A DELIVERY BOY"
      puts "WHERE TO DELIVER THE ORDERED PIZZAS. \n\n"

      print_map

      puts "\n\nTHE OUTPUT IS A MAP OF THE HOMES WHERE"
      puts "YOU ARE TO SEND PIZZAS."
      puts "YOUR JOB IS TO GIVE A TRUCK DRIVER"
      puts "THE LOCATION OR COORDINATES OF THE"
      puts "HOME ORDERING THE PIZZA."

      return player_name
    end

    def yes_no_prompt msg
      puts
      while true
        print "#{msg} "
          
        response = gets.chomp

        if response == response.upcase
          if response == "YES"
            return true
          elsif response == "NO"
            return false
          end
        else
          print "'YES' OR 'NO' PLEASE, NOW THEN, "
        end

      end
    end

    def print_more_directions player_name
      puts "\nSOMEBODY WILL ASK FOR A PIZZA TO BE"
      puts "DELIVERED.  THEN A DELIVERY BOY WILL"
      puts "ASK YOU FOR THE LOCATION."
      puts "\nEXAMPLE:"
      puts "THIS IS J.  PLEASE SEND A PIZZA."
      puts "DRIVER TO #{player_name}.  WHERE DOES J LIVE?"
      puts "YOUR ANSWER WOULD BE 2,3"
    end
    
    def calculate_customer_index x,y
      return 4 * (y - 1) + x - 1
    end

    def deliver_to customer_index, customer_name, player_name
      print "  DRIVER TO #{player_name}:  WHERE DOES #{customer_name} LIVE? "
  
      coords = gets.chomp!
      xc, yc = coords.split(/,/).map(&:to_i)

      puts "#{xc}, #{yc}"
      delivery_index = calculate_customer_index(xc, yc)
      if delivery_index == customer_index
        puts "HELLO #{player_name}.  THIS IS #{customer_name}, THANKS FOR THE PIZZA."
        return true
      else  
          delivery_name = CUSTOMER_NAMES[delivery_index]
          puts "THIS IS #{defined?(delivery_name.to_i) ? "Undefined" : delivery_name.to_i}.  I DID NOT ORDER A PIZZA."
          puts "I LIVE AT #{xc},#{yc}"
          return false
      end
    end

    def play_game num_turns, player_name
      for _turn in (1..num_turns)
        x = rand(1..4)
        y = rand(1..4)
        customer_index = calculate_customer_index(x, y)
        customer_name = CUSTOMER_NAMES[customer_index]

        puts "\nHELLO #{player_name}'S PIZZA.  THIS IS #{customer_name}. \n  PLEASE SEND A PIZZA."

        while true
          success = deliver_to(customer_index, customer_name, player_name)
          if success
            break
          end
        end
      end
    end
end

if __FILE__ == $0
  pizza = Pizza.new
  pizza.start()
end
  