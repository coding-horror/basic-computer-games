import std/[random,strutils]

var
  wager, winnings, rollResult: int
  stillplaying: bool = true

proc tryAgain(): bool =
  echo "WANT TO PLAY AGAIN? (YES OR NO)"
  var answer = readLine(stdin).normalize()
  result = (answer == "y") or (answer == "yes")

proc takePoint(point: int) =
  var flag = true
  while flag:
    var pointRoll: int = (rand 1..6) + (rand 1..6) # roll dice, then add the sum
    if pointRoll == 7:
      echo pointRoll, "- CRAPS. YOU LOSE."
      echo "YOU LOSE ", wager, " DOLLARS."
      winnings -= wager
      flag = false
    if pointRoll == point:
      echo point, "- A WINNER.........CONGRATS!!!!!!!!"
      echo "AT 2 TO 1 ODDS PAYS YOU...LET ME SEE... ", 2*wager, " DOLLARS"
      winnings += (2*wager)
      flag = false
    if flag:
      echo pointRoll, " - NO POINT. I WILL ROLL AGAIN"

echo spaces(33), "CRAPS"
echo spaces(15), "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
echo "\n"
echo "2,3,12 ARE LOSERS; 4,5,6,8,9,10 ARE POINTS; 7,11 ARE NATURAL WINNERS."
winnings = 0

# play the game
while stillplaying:
  echo ""
  echo "INPUT THE AMOUNT OF YOUR WAGER."
  wager = readline(stdin).parseInt()
  echo "I WILL NOW THROW THE DICE"
  rollResult = (rand 1..6) + (rand 1..6) # roll dice, then add the sum
  case rollResult:
    of 7, 11:
      echo rollResult, "- NATURAL....A WINNER!!!!"
      echo rollResult, " PAYS EVEN MONEY, YOU WIN ", wager, " DOLLARS"
      winnings += wager
    of 2:
      echo rollResult, "- SNAKE EYES....YOU LOSE."
      echo "YOU LOSE ", wager, " DOLLARS."
      winnings -= wager
    of 3, 12:
      echo rollResult, "- CRAPS...YOU LOSE."
      echo "YOU LOSE ", wager, " DOLLARS."
      winnings -= wager
    else:
      echo rollResult, " IS THE POINT. I WILL ROLL AGAIN"
      takePoint(rollResult)
  if winnings < 0: echo "YOU ARE NOW UNDER $", winnings
  if winnings > 0: echo "YOU ARE NOW AHEAD $", winnings
  if winnings == 0: echo "YOU ARE NOW EVEN AT 0"
  stillplaying = tryAgain()

# done playing
if winnings < 0: echo "TOO BAD, YOU ARE IN THE HOLE. COME AGAIN."
if winnings > 0: echo "CONGRATULATIONS---YOU CAME OUT A WINNER. COME AGAIN!"
if winnings == 0: echo "CONGRATULATIONS---YOU CAME OUT EVEN, NOT BAD FOR AN AMATEUR"
