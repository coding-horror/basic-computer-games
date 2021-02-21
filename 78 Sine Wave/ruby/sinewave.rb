def intro
  puts "                              SINE WAVE
               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n\n\n\n"
end

def main
  intro
  (0..40).step(0.25).each do |t|
    a = (26 + 25 * Math.sin(t)).to_i
    text = (t % 0.5) == 0 ? "CREATIVE" : "COMPUTING"
    puts " " * a + text
  end
end

main