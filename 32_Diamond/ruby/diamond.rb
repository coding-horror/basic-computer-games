def intro
  print "                                 DIAMOND
               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



FOR A PRETTY DIAMOND PATTERN,
TYPE IN AN ODD NUMBER BETWEEN 5 AND 21? "
end

def get_facets
  while true
    number = gets.chomp
    return number.to_i if /^\d+$/.match(number)
    puts "!NUMBER EXPECTED - RETRY INPUT LINE"
    print "? "
  end
end

def get_diamond_lines(facets)
  spacers = (facets - 1) / 2
  lines = [' ' * spacers + 'C' + ' ' * spacers]
  lines += (1...facets).step(2).to_a.map { |v|
    spacers -= 1
    ' ' * spacers + 'CC' + '!' * v + ' ' * spacers
  }
  lines + lines[0..-2].reverse
end

def draw_diamonds(lines)
  repeat = 60 / lines[0].length
  (0...repeat).each { lines.map { |l| l * repeat }.each { |l| puts l } }
end

def main
  intro
  facets = get_facets
  puts
  lines = get_diamond_lines(facets)
  draw_diamonds(lines)
end

trap "SIGINT" do puts; exit 130 end

main