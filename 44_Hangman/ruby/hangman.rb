class Canvas
	BUFFER = []
	def initialize width = 12, height = 12, fill = " "
		for i in (0...height) do
			line = []
			for i in (0...width) do
				line << ""
			end
			BUFFER << line
		end

		clear
	end

	def render
		lines = []
		for line in BUFFER do
			lines << line.join("")
		end

		return lines.join("\n")
	end

	def put s, x, y
		BUFFER[y][x] = s[0]
	end

	private
		def clear fill = " "
			for row in BUFFER do
				for x in (0...(row.length)) do
					row[x] = fill
				end
			end
		end
end

def init_gallows canvas
	for i in (0...12) do
		canvas.put("X", 0, i)
	end

	for i in (0...7) do
		canvas.put("X", i, 0)
	end

	canvas.put("X", 6, 1)
end

def draw_head canvas
	canvas.put("-", 5, 2)
	canvas.put("-", 6, 2)
	canvas.put("-", 7, 2)
	canvas.put("(", 4, 3)
	canvas.put(".", 5, 3)
	canvas.put(".", 7, 3)
	canvas.put(")", 8, 3)
	canvas.put("-", 5, 4)
	canvas.put("-", 6, 4)
	canvas.put("-", 7, 4)
end

def draw_body canvas
	for i in (5...9) do
		canvas.put("X", 6, i)
	end
end

def draw_right_arm canvas
	for i in (3...8) do
		canvas.put("\\", i - 1, i)
	end
end

def draw_left_arm canvas
	canvas.put("/", 10, 3)
	canvas.put("/", 9, 4)
	canvas.put("/", 8, 5)
	canvas.put("/", 7, 6)
end

def draw_right_leg canvas
	canvas.put("/", 5, 9)
	canvas.put("/", 4, 10)
end

def draw_left_leg canvas
	canvas.put("\\", 7, 9)
	canvas.put("\\", 8, 10)
end

def draw_left_hand canvas
	canvas.put("\\", 10, 2)
end

def draw_right_hand canvas
	canvas.put("/", 2, 2)
end

def draw_left_foot canvas
	canvas.put("\\", 9, 11)
	canvas.put("-", 10, 11)
end

def draw_right_foot canvas
	canvas.put("-", 2, 11)
	canvas.put("/", 3, 11)
end

PHASES = [
	["First, we draw a head", 'draw_head'],
	["Now we draw a body.", 'draw_body'],
	["Next we draw an arm.", 'draw_right_arm'],
	["this time it's the other arm.", 'draw_left_arm'],
	["Now, let's draw the right leg.", 'draw_right_leg'],
	["This time we draw the left leg.", 'draw_left_leg'],
	["Now we put up a hand.", 'draw_left_hand'],
	["Next the other hand.", 'draw_right_hand'],
	["Now we draw one foot", 'draw_left_foot'],
	["Here's the other foot -- you're hung!!", 'draw_right_foot'],
]

WORDS = [
	"GUM",
	"SIN",
	"FOR",
	"CRY",
	"LUG",
	"BYE",
	"FLY",
	"UGLY",
	"EACH",
	"FROM",
	"WORK",
	"TALK",
	"WITH",
	"SELF",
	"PIZZA",
	"THING",
	"FEIGN",
	"FIEND",
	"ELBOW",
	"FAULT",
	"DIRTY",
	"BUDGET",
	"SPIRIT",
	"QUAINT",
	"MAIDEN",
	"ESCORT",
	"PICKAX",
	"EXAMPLE",
	"TENSION",
	"QUININE",
	"KIDNEY",
	"REPLICA",
	"SLEEPER",
	"TRIANGLE",
	"KANGAROO",
	"MAHOGANY",
	"SERGEANT",
	"SEQUENCE",
	"MOUSTACHE",
	"DANGEROUS",
	"SCIENTIST",
	"DIFFERENT",
	"QUIESCENT",
	"MAGISTRATE",
	"ERRONEOUSLY",
	"LOUDSPEAKER",
	"PHYTOTOXIC",
	"MATRIMONIAL",
	"PARASYMPATHOMIMETIC",
	"THIGMOTROPISM",
]

def play_game guess_target
	wrong_guesses = 0
	guess_progress = ["-"] * guess_target.length
	guess_list = []

	gallows = Canvas.new
	init_gallows(gallows)

	guess_count = 0
	while true
		puts "Here are the letters you used:"
		puts "#{guess_list.join(",")}\n"
		puts "#{guess_progress.join("")}\n"

		guess_letter = ""
		guess_word = ""
		while guess_letter == ""
			print "What is your guess? "
			guess_letter = gets.chomp!.upcase[0]
			if !guess_letter.match?(/[[:alpha:]]/)
				guess_letter = ""
				puts "Only letters are allowed!"
			elsif guess_list.include?(guess_letter)
				guess_letter = ""
				puts "You guessed that letter before!"
			end
		end

		guess_list << guess_letter
		guess_count += 1

		if guess_target.include?(guess_letter)
			indices = (0...guess_target.length).find_all { |i| guess_target[i,1] == guess_letter }

			for i in indices do
				guess_progress[i] = guess_letter
			end

			if guess_progress.join("") == guess_target
				puts "You found the word!"
				break
			else
				puts "\n#{guess_progress.join("")}\n"

				while guess_word == ""
					print "What is your guess for the word? "
					guess_word = gets.chomp!.upcase
					if !guess_word.match?(/[[:alpha:]]/)
						guess_word = ""
						puts "Only words are allowed!"
					end
				end

				if guess_word == guess_target
					puts "Right!! It took you #{guess_count} guesses!"
					break
				end
			end
		else
			comment, draw_bodypart = PHASES[wrong_guesses]

			puts comment
			method(draw_bodypart).call(gallows)
			puts gallows.render()

			wrong_guesses += 1
			puts "Sorry, that letter isn't in the word."

			if wrong_guesses == 10
				puts "Sorry, you lose. The word was #{guess_target}"
				break
			end
		end
	end
end


def main
	puts "#{(" " * 32)}HANGMAN"

	shuffled = WORDS.shuffle(random: Random.new)
	current_word = 0
	word_count = shuffled.length

	keep_playing = true
	while keep_playing

		play_game(shuffled[current_word])
		current_word += 1

		if current_word == word_count
			puts "You did all the words!!"
			keep_playing = false
		else
			print "Want another word? (yes or no) "
			a = gets.chomp!.upcase
			keep_playing = true if a == 'Y' || a == 'y' || a == 'Yes' || a == 'YES' || a == 'yes'
		end
	end
	puts "It's been fun! Bye for now."
end

if __FILE__ == $0
	main
end