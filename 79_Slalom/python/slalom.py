from random import random

print("SLALOM".rjust(39))
print("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n".rjust(57))

medals = {
	"gold": 0,
	"silver": 0,
	"bronze": 0,
}
max_speeds = [14,18,26,29,18,25,28,32,29,20,29,29,25,21,26,29,20,21,20,18,26,25,33,31,22]

def ask(question):
	print(question, end="? ")
	return input().upper()

def ask_int(question):
	reply = ask(question)
	return int(reply) if reply.isnumeric() else -1

def pre_run():
	print("\nTYPE \"INS\" FOR INSTRUCTIONS")
	print("TYPE \"MAX\" FOR APPROXIMATE MAXIMUM SPEEDS")
	print("TYPE \"RUN\" FOR THE BEGINNING OF THE RACE")
	cmd = ask("COMMAND--")
	while cmd != "RUN":
		if cmd == "INS":
			print("\n*** SLALOM: THIS IS THE 1976 WINTER OLYMPIC GIANT SLALOM.  YOU ARE")
			print("            THE AMERICAN TEAM'S ONLY HOPE OF A GOLD MEDAL.\n")
			print("     0 -- TYPE THIS IS YOU WANT TO SEE HOW LONG YOU'VE TAKEN.")
			print("     1 -- TYPE THIS IF YOU WANT TO SPEED UP A LOT.")
			print("     2 -- TYPE THIS IF YOU WANT TO SPEED UP A LITTLE.")
			print("     3 -- TYPE THIS IF YOU WANT TO SPEED UP A TEENSY.")
			print("     4 -- TYPE THIS IF YOU WANT TO KEEP GOING THE SAME SPEED.")
			print("     5 -- TYPE THIS IF YOU WANT TO CHECK A TEENSY.")
			print("     6 -- TYPE THIS IF YOU WANT TO CHECK A LITTLE.")
			print("     7 -- TYPE THIS IF YOU WANT TO CHECK A LOT.")
			print("     8 -- TYPE THIS IF YOU WANT TO CHEAT AND TRY TO SKIP A GATE.\n")
			print(" THE PLACE TO USE THESE OPTIONS IS WHEN THE COMPUTER ASKS:\n")
			print("OPTION?\n")
			print("                GOOD LUCK!\n")
			cmd = ask("COMMAND--")
		elif cmd == "MAX":
			print("GATE MAX")
			print(" # M.P.H.")
			print("----------")
			for i in range(0, gates):
				print(f" {i + 1}  {max_speeds[i]}")
			cmd = ask("COMMAND--")
		else:
			cmd = ask(f"\"{cmd}\" IS AN ILLEGAL COMMAND--RETRY")

def run():
	global medals
	print("THE STARTER COUNTS DOWN...5...4...3...2...1...GO!")
	time = 0
	speed = int(random() * (18 - 9) + 9)
	print("YOU'RE OFF")
	for i in range(0, gates):
		while True:
			print(f"\nHERE COMES GATE #{i + 1}:")
			print(f" {int(speed)}M.P.H.")
			old_speed = speed
			opt = ask_int("OPTION")
			while opt < 1 or opt > 8:
				if(opt == 0):
					print(f"YOU'VE TAKEN {int(time)}SECONDS.")
				else:
					print("WHAT?")
				opt = ask_int("OPTION")

			if opt == 8:
				print("***CHEAT")
				if random() < .7:
					print("AN OFFICIAL CAUGHT YOU!")
					print(f"YOU TOOK {int(time + random())}SECONDS.")
					return
				else:
					print("YOU MADE IT!")
					time += 1.5
			else:
				match opt:
					case 1:
						speed += int(random() * (10 - 5) + 5)

					case 2:
						speed += int(random() * (5 - 3) + 3)

					case 3:
						speed += int(random() * (4 - 1) + 1)

					case 5:
						speed -= int(random() * (4 - 1) + 1)

					case 6:
						speed -= int(random() * (5 - 3) + 3)

					case 7:
						speed -= int(random() * (10 - 5) + 5)
				print(f" {int(speed)}M.P.H.")
				if speed > max_speeds[i]:
					if random() < ((speed - max_speeds[i]) * .1) + .2:
						print(f"YOU WENT OVER THE MAXIMUM SPEED AND {'SNAGGED A FLAG' if random() < .5 else 'WIPED OUT'}!")
						print(f"YOU TOOK {int(time + random())}SECONDS")
						return
					else:
						print("YOU WENT OVER THE NAXIMUM SPEED AND MADE IT!")
				if speed > max_speeds[i] - 1:
					print("CLOSE ONE!")
			if speed < 7:
				print("LET'S BE REALISTIC, OK?  LET'S GO BACK AND TRY AGAIN...")
				speed = old_speed
			else:
				time += max_speeds[i] - speed + 1
				if speed > max_speeds[i]:
					time += .5
				break
	print(f"\nYOU TOOK {int(time + random())}SECONDS.")
	avg = time / gates
	if avg < 1.5 - (lvl * .1):
		print("YOU WON A GOLD MEDAL!")
		medals["gold"] += 1
	elif avg < 2.9 - (lvl * .1):
		print("YOU WON A SILVER MEDAL!")
		medals["silver"] += 1
	elif avg < 4.4 - (lvl * .01):
		print("YOU WON A BRONZE MEDAL!")
		medals["bronze"] += 1

while True:
	gates = ask_int("HOW MANY GATES DOES THIS COURSE HAVE (1 TO 25)")
	if gates < 1:
		print("TRY AGAIN,")
	else:
		if(gates > 25):
			print("25 IS THE LIMIT.")
		break

pre_run()

while True:
	lvl = ask_int("RATE YOURSELF AS A SKIER, (1=WORST, 3=BEST)")
	if lvl < 1 or lvl > 3:
		print("THE BOUNDS ARE 1-3.")
	else:
		break

while True:
	run()
	while True:
		answer = ask("DO YOU WANT TO PLAY AGAIN?")
		if answer == "YES" or answer == "NO":
			break
		else:
			print("PLEASE TYPE 'YES' OR 'NO'")
	if answer == "NO":
		break

print("THANKS FOR THE RACE")
if medals["gold"] > 0:
	print(f"GOLD MEDALS: {medals['gold']}")
if medals["silver"] > 0:
	print(f"SILVER MEDALS: {medals['silver']}")
if medals["bronze"] > 0:
	print(f"BRONZE MEDALS: {medals['bronze']}")
