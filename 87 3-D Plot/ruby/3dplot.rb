def intro
  puts "                                3D PLOT
               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n\n\n"
end

def fna(z) = 30 * Math.exp(-z * z / 100)

def render
  (-30..30).step(1.5).each do |x|
    l = 0
    y1 = 5 * (Math.sqrt(900 - x * x) / 5).to_i
    y_plot = " " * 80
    (y1..-y1).step(-5).each do |y|
      z = (25 + fna(Math.sqrt(x * x + y * y)) - 0.7 * y).to_i
      next if z <= l
      l = z
      y_plot[z] = '*'
    end
    puts y_plot
  end
end

def main
  intro
  render
end

main