#!/usr/bin/env ruby

# Kinema
# reinterpreted from BASIC by stephan.com

EPSILON = 0.15

def close?(guess, answer)
  (guess-answer).abs < answer * EPSILON
end

def ask(text, answer)
  puts text
  guess = gets.strip.to_f
  if close?(guess, answer)
    puts 'Close enough'
    @score += 1
  else
    puts 'Not even close....'
  end

  puts "Correct answer is #{answer}"
end

puts 'Kinema'.center(80)
puts 'Adapted by stephan.com'.center(80)
puts; puts; puts;

loop do
  puts; puts
  @score = 0
  v = 5 + rand(35)

  puts "A ball is thrown upwards at #{v} meters per second"

  ask 'How high will it go? (in meters)', 0.05 * v * v
  ask 'How long until it returns? (in seconds)', v/5.0

  t = 1 + rand(2*v)/10.0
  ask "What will its velocity be after #{t} seconds?", v - 10 * t
  puts
  print "#{@score} right out of 3."
  print " not bad" if @score > 1
  puts
end
