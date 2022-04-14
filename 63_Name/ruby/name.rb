def is_yes_ish answer
    cleaned = answer.upcase
    return true if ["Y", "YES"].include? cleaned
    return false
end

def main
    puts " " * 34 + "NAME"

    puts "HELLO."
    puts "MY NAME iS COMPUTER."
    print "WHAT'S YOUR NAME (FIRST AND LAST)? "
    name = gets.chomp!
    puts ""
    name_as_list = name.split("")
    reversed_name = name_as_list.reverse.join("")

    puts "THANK YOU, #{reversed_name}.\n"
    puts "OOPS!  I GUESS I GOT IT BACKWARDS.  A SMART"
    puts "COMPUTER LIKE ME SHOULDN'T MAKE A MISTAKE LIKE THAT!\n\n"
    puts "BUT I JUST NOTICED YOUR LETTERS ARE OUT OF ORDER."

    sorted_name = name_as_list.sort.join("")
    puts "LET'S PUT THEM IN ORDER LIKE THIS: #{sorted_name}\n\n"
    print "DON'T YOU LIKE THAT BETTER? "
    like_answer = gets.chomp!
    puts
    if is_yes_ish(like_answer)
        puts "I KNEW YOU'D AGREE!!"
    else
        puts "I'M SORRY YOU DON'T LIKE IT THAT WAY."
    end

    puts ""
    puts "I REALLY ENJOYED MEETING YOU, #{name}."
    puts "HAVE A NICE DAY!"
end


if __FILE__ == $0
    main
end