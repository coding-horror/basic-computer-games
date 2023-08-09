import std/[random,strutils]

type
  symbol = enum
    PAPER = 1, SCISSORS = 2, ROCK = 3

var
  cpuChoice, playerChoice, turns: int
  cpuWins, playerWins, ties: int = 0

randomize()

# Function: player makes a choice
proc choose(): int =
  echo "3=ROCK...2=SCISSORS...1=PAPER...WHAT'S YOUR CHOICE?"
  result = readLine(stdin).parseInt()

# Function: determine the outcome
proc outcome(p: symbol, c: symbol): string =
  if p == c:
    ties += 1
    result = "TIE GAME.  NO WINNER."
  if p == SCISSORS and c == PAPER:
    playerWins += 1
    result = "SCISSORS CUTS PAPER.  YOU WIN."
  if p == PAPER and c == SCISSORS:
    cpuWins += 1
    result = "SCISSORS CUTS PAPER.  I WIN."
  if p == PAPER and c == ROCK:
    playerWins += 1
    result = "PAPER COVERS ROCK.  YOU WIN."
  if p == ROCK and c == PAPER:
    cpuWins += 1
    result = "PAPER COVERS ROCK.  I WIN."
  if p == ROCK and c == SCISSORS:
    playerWins += 1
    result = "ROCK CRUSHES SCISSORS.  YOU WIN."
  if p == SCISSORS and c == ROCK:
    cpuWins += 1
    result = "ROCK CRUSHES SCISSORS.  I WIN."

# Start the game
echo spaces(21), "GAME OF ROCK, SCISSORS, PAPER"
echo spaces(15), "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
echo "\n"
echo "HOW MANY GAMES?"
turns = readLine(stdin).parseInt()
while turns > 10:
  echo "SORRY, BUT WE AREN'T ALLOWED TO PLAY THAT MANY."
  turns = readLine(stdin).parseInt()

# Play the game
for i in 1..turns:
  echo ""
  echo "GAME NUMBER ", i
  playerChoice = choose()
  while playerChoice != 1 and playerChoice != 2 and playerChoice != 3:
    echo "INVALID"
    playerChoice = choose()
  cpuChoice = rand(1..3) # match against range in symbol
  echo "THIS IS MY CHOICE... ", symbol(cpuChoice)
  echo outcome(symbol(playerChoice), symbol(cpuChoice))

# Results
echo ""
echo "HERE IS THE FINAL GAME SCORE:"
echo "I HAVE WON ", cpuWins," GAME(S)."
echo "YOU HAVE WON ", playerWins," GAME(S)."
echo "AND ", ties," GAME(S) ENDED IN A TIE."
echo ""
echo "THANKS FOR PLAYING!!"
