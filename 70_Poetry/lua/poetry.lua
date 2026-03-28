--[[
Poetry 
From the classic book BASIC Computer Games, published by Creative Computing.

This program produces random verse which might loosly be considered in the Japanese Haiku style.
It uses 20 phrases in four groupd of five phrases each and generally cycles through the groups in order.
It inserts commas (random - 19% of the time), indentation (random - 22% of the time), and starts new paragraphs (18% probability, but at least once every 20 phrases).

The phrases in POETRY are somewhat suggestive of Edgar Allan Poe.

The Original author of this program is unkown. It was modified and reworked by Jim Bailey, Peggy Ewing, and Dave Ahl at DEC.

Ported to Lua by Allen West
**CHANGES 
Added a limit so it did not spit out infinite poetry at lightning speed.
The variable U was changed to a boolean for readability. 
Adjusted the randomizing to work with any number of phrases since Dave encouraged users to write their own content for the poems.
]]

limit = 60

phrase_one = {"MIDNIGHT DREARY", "FIERY EYES", "BIRD OR FIEND", "THING OF EVIL", "PROPHET"}
phrase_two = {"BEGUILING ME", "THRILLED ME", "STILL SITTING....", "NEVER FLITTING", "BURNED"}
phrase_three = {"AND MY SOUL", "DARKNESS THERE", "SHALL BE LIFTED", "QUOTH THE RAVEN", "SIGN OF PARTING"}
phrase_four = {"NOTHING MORE", "YET AGAIN", "SLOWLY CREEPING", "...EVERMORE", "NEVERMORE"}

function printintro()
	print("\n               POETRY")
	print("Creative Computing  Morristown, New Jersey")
	print("\n\n")
end

--Insert commas, spaces, new lines,and tabs randomly
function randomformatting()
	--Random comma
	if new_line == false and math.random() < 0.19 then
		io.write(",")
		new_line = false
	end
	--Random space or new line
	if math.random() < 0.65 then
		io.write(" ")
		new_line = false
	else
		print("")
		new_line = true
	end
	--Random Tabs
	if new_line and phrase_set % 2 ~= 0 then
		io.write("	")
	end
end

--Poetry printing loop
function printpoetry(run_length)
	math.randomseed(os.time()) -- lua needs it's random function initialized to work
	run_count = 0 --Added so it doesn't print infinitely
	phrase_set = 0 --var j in original code
	lines_without_break = 0 --var k in original code
	phrase_choice = 0 --var i in original code
	new_line = true  --var u in original code
	while(run_count < run_length) do
		if phrase_set == 0 then
			phrase_choice = math.random(1,#phrase_one)
			io.write(phrase_one[phrase_choice])
		elseif phrase_set == 1 then
			phrase_choice = math.random(1,#phrase_two)
			if phrase_choice == 0 or phrase_choice == 3 then
				new_line = false
			end
			io.write(phrase_two[phrase_choice])
		elseif phrase_set == 2 then
			phrase_choice = math.random(1,#phrase_three)
			if phrase_choice < #phrase_three and new_line == false then
				io.write(phrase_three[phrase_choice])
			end
		elseif phrase_set == 3 then
			phrase_choice = math.random(1,#phrase_four)
			io.write(phrase_four[phrase_choice])
		end
		if phrase_set == 4 then
			phrase_set = 0
			print("")
			if lines_without_break > 20 then
				new_line = true
				lines_without_break = 0
				print("\n")
			end
		else
			phrase_set = phrase_set + 1
			lines_without_break = lines_without_break + 1
			randomformatting()
			run_count = run_count + 1
		end
	end
	print("")
end

printintro()
printpoetry(limit)