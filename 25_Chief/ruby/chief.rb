def tab(size)
  str = ''
  size.times do
    str += ' '
  end

  str
end

def input
  gets.chomp
end

def bye
  print "BYE!!!\n"
end

def main
  print tab(30), "CHIEF\n"
  print tab(15), "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n"
  print "\n"
  print "\n"
  print "\n"
  print "I AM CHIEF NUMBERS FREEK, THE GREAT INDIAN MATH GOD.\n"
  print "ARE YOU READY TO TAKE THE TEST YOU CALLED ME OUT FOR\n"

  a = input
  if a != 'YES'
    print "SHUT UP, PALE FACE WITH WISE TONGUE.\n"
  end

  print " TAKE A NUMBER AND ADD 3. DIVIDE THIS NUMBER BY 5 AND\n"
  print "MULTIPLY BY 8. DIVIDE BY 5 AND ADD THE SAME. SUBTRACT 1.\n"
  print  "  WHAT DO YOU HAVE\n"

  b = input.to_f
  c = ((b + 1 - 5) * 5 / 8 * 5 -3).to_f
  print "I BET YOUR NUMBER WAS #{c}. AM I RIGHT\n"

  d = input
  if d == 'YES'
    return bye
  end

  print "WHAT WAS YOUR ORIGINAL NUMBER\n"

  k = input.to_f
  f = k + 3
  g = f / 5
  h = g * 8
  i = h / 5 + 5
  j = i - 1

  print "SO YOU THINK YOU'RE SO SMART, EH?\n"
  print "NOW WATCH.\n"
  print k, " PLUS 3 EQUALS ", f, ". THIS DIVIDED BY 5 EQUALS ", g, ";\n"
  print "THIS TIMES 8 EQUALS ", h, ". IF WE DIVIDE BY 5 AND ADD 5,\n"
  print "WE GET ", i, ", WHICH, MINUS 1, EQUALS ", j, ".\n"
  print "NOW DO YOU BELIEVE ME\n"

  z = input
  if z == 'YES'
    return bye
  end

  print "YOU HAVE MADE ME MAD!!!\n"
  print "THERE MUST BE A GREAT LIGHTNING BOLT!\n"
  print "\n"
  print "\n"

  x = 30
  while x >= 22
    print tab(x), "X X\n"
    x -= 1
  end

  print tab(21), "X XXX\n"
  print tab(20), "X   X\n"
  print tab(19), "XX X\n"

  y = 20
  while y >= 13
    print tab(y), "X X\n"
    y -= 1
  end

  print tab(12), "XX\n"
  print tab(11), "X\n"
  print tab(10), "*\n"

  print "\n"
  print "#########################\n"
  print "\n"
  print "I HOPE YOU BELIEVE ME NOW, FOR YOUR SAKE!!\n"
  return bye
end

main
