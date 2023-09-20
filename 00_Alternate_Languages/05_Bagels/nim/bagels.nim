import std/[random,strutils]

# BAGLES NUMBER GUESSING GAME
# ORIGINAL SOURCE UNKNOWN BUT SUSPECTED TO BE
# LAWRENCE HALL OF SCIENCE, U.C. BERKELY

var
  a, b: array[1..3, int]
  wincount: int = 0
  prompt: string
  stillplaying: bool = true

randomize() # Seed the random number generator

# Seed 3 unique random numbers; indicate if they're all unique
proc genSeed(): bool =
  for i in 1..3:
    a[i] = rand(0..9)
  if (a[1] == a[2]) or (a[2] == a[3]) or (a[3] == a[1]):
    return false
  return true

# Primary game logic
proc playGame() =
  var youwin, unique: bool = false
  # We want 3 unique random numbers: loop until we get them!
  while unique == false:
    unique = genSeed()
  echo "O.K.  I HAVE A NUMBER IN MIND."
  for i in 1..20:
    var c, d: int = 0
    echo "GUESS #", i
    prompt = readLine(stdin).normalize()
    if (prompt.len() != 3):
      echo "TRY GUESSING A THREE-DIGIT NUMBER."
      continue
    for z in 1..3:
      b[z] = prompt.substr(z-1, z-1).parseInt() # Convert string digits to array ints
    if (b[1] == b[2]) or (b[2] == b[3]) or (b[3] == b[1]):
      echo "OH, I FORGOT TO TELL YOU THAT THE NUMBER I HAVE IN MIND"
      echo "HAS NO TWO DIGITS THE SAME."
    # Figure out the PICOs
    if (a[1] == b[2]): c += 1
    if (a[1] == b[3]): c += 1
    if (a[2] == b[1]): c += 1
    if (a[2] == b[3]): c += 1
    if (a[3] == b[1]): c += 1
    if (a[3] == b[2]): c += 1
    # Determine FERMIs
    for j in 1..3:
      if (a[j] == b[j]): d += 1
    # Reveal clues
    if (d != 3):
      if (c != 0):
        for j in 1..c:
          echo "PICO"
      if (d != 0):
        for j in 1..d:
          echo "FERMI"
      if (c == 0) and (d == 0):
        echo "BAGELS"
    # If we have 3 FERMIs, we win!
    else:
      echo "YOU GOT IT!!!"
      echo ""
      wincount += 1
      youwin = true
      break
  # Only invoke if we've tried 20 guesses without winning
  if not youwin:
    echo "OH WELL."
    echo "THAT'S TWENTY GUESSES.  MY NUMBER WAS ", a[1], a[2], a[3]

# main program
echo spaces(33), "BAGELS"
echo spaces(15), "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
echo "\n\n"
echo "WOULD YOU LIKE THE RULES (YES OR NO)"
prompt = readLine(stdin).normalize()
if prompt.substr(0, 0) == "y":
  echo "I AM THINKING OF A THREE-DIGIT NUMBER.  TRY TO GUESS"
  echo "MY NUMBER AND I WILL GIVE YOU CLUES AS FOLLOWS:"
  echo "   PICO   - ONE DIGIT CORRECT BUT IN THE WRONG POSITION"
  echo "   FERMI  - ONE DIGIT CORRECT AND IN THE RIGHT POSITION"
  echo "   BAGELS - NO DIGITS CORRECT"
  echo ""
while(stillplaying == true):
  playGame()
  echo "PLAY AGAIN (YES OR NO)"
  prompt = readLine(stdin).normalize()
  if prompt.substr(0, 0) != "y":
    stillplaying = false
if wincount > 0:
  echo ""
  echo "A ", wincount, " POINT BAGELS BUFF!!"
echo ""
echo "HOPE YOU HAD FUN.  BYE."
