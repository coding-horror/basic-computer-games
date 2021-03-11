require 'set'

def intro
  puts "                                ANIMAL
               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



PLAY 'GUESS THE ANIMAL'

THINK OF AN ANIMAL AND THE COMPUTER WILL TRY TO GUESS IT.

"
end

def ask(question)
  print "#{question} "
  (gets || '').chomp.upcase
end

Feature = Struct.new(:question, :yes_guess, :no_guess)

def add_guess(animals, guess)
  guess.is_a?(Struct) ? get_all_animals(guess, animals) : animals.add(guess)
end

def get_all_animals(feature, animals = Set.new)
  add_guess(animals, feature.yes_guess)
  add_guess(animals, feature.no_guess)
  animals
end

def create_feature(current_animal)
  new_animal = ask('THE ANIMAL YOU WERE THINKING OF WAS A ?')
  puts "PLEASE TYPE IN A QUESTION THAT WOULD DISTINGUISH A #{new_animal} FROM A #{current_animal}"
  question = ask('?')
  loop do
    yes_no = ask("FOR A #{new_animal} THE ANSWER WOULD BE ?")
    next unless ['Y', 'N'].include?(yes_no[0])
    guesses = yes_no[0] == 'Y' ? [new_animal, current_animal] : [current_animal, new_animal]
    return Feature.new(question, *guesses)
  end
end

def guess_loop(feature)
  loop do
    answer = ask(feature.question)
    next unless ['Y', 'N'].include?(answer[0])
    answer_is_yes = answer[0] == 'Y'

    name = answer_is_yes ? feature.yes_guess : feature.no_guess
    if name.is_a?(Struct)
      feature = name
      next
    end

    guess = ask("IS IT A #{name}?")
    correct_guess = guess[0] == 'Y'

    if correct_guess
      puts "WHY NOT TRY ANOTHER ANIMAL?"
      break
    end

    if answer_is_yes
      feature.yes_guess = create_feature(name)
    else
      feature.no_guess = create_feature(name)
    end
    break
  end
end

def main
  intro
  feature = Feature.new('DOES IT SWIM?', 'FISH', 'BIRD')

  while true do
    option = ask("ARE YOU THINKING OF AN ANIMAL?")
    if option == 'LIST'
      puts
      puts "ANIMALS I ALREADY KNOW ARE:"
      puts get_all_animals(feature).to_a.join(" " * 15)
      puts
    elsif option[0] == 'Y'
      guess_loop(feature)
    elsif option == ''
      puts
    end
  end
end

trap "SIGINT" do puts; exit 130 end

main