import std/[random,strutils]

var
  bet, cardA, cardB, cardC, stash: int
  retry: bool = true

proc printGreeting() =
  echo(spaces(26),"ACEY DUCEY CARD GAME")
  echo(spaces(15),"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
  echo("\n")
  echo("ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER ")
  echo("THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP")
  echo("YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING")
  echo("ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE")
  echo("A VALUE BETWEEN THE FIRST TWO.")
  echo("IF YOU DO NOT WANT TO BET, INPUT A 0")
  echo("")

proc printBalance() =
  echo("YOU NOW HAVE ", stash," DOLLARS.")
  echo("")

proc printCard(aCard: int) =
  case aCard:
  of 11: echo("=== JACK ===")
  of 12: echo("=== QUEEN ===")
  of 13: echo("=== KING ===")
  of 14: echo("=== ACE ===")
  else: echo("=== ", aCard, " ===")

proc drawDealerCards() =
  echo("HERE ARE YOUR NEXT TWO CARDS: ")
  cardA = rand(2..14)
  cardB = cardA # Copy cardA, so we can test cardB to be different
  while cardB == cardA:
    cardB = rand(2..14)
  if cardA > cardB: # Make sure cardA is the smaller card
    swap(cardA, cardB)
  echo("")
  printCard(cardA)
  echo("")
  printCard(cardB)
  echo("")

proc drawPlayerCard() =
  cardC = rand(2..14)
  printCard(cardC)
  echo("")

proc getBet(): int =
  result = stash + 1 #ensure we enter the loop
  while (result < 0) or (result > stash):
    echo("WHAT IS YOUR BET: ")
    result = readLine(stdin).parseInt()
    if result > stash:
      echo("SORRY, MY FRIEND, BUT YOU BET TOO MUCH.")
      echo("YOU HAVE ONLY ", stash," DOLLARS TO BET.")
    if bet == 0:
      echo("CHICKEN!!")

proc tryAgain(): bool =
  echo("TRY AGAIN (YES OR NO)")
  var answer = readLine(stdin).normalize()
  case answer:
    of "yes", "y": result = true
    else: result = false

printGreeting()
while retry:
  stash = 100
  while stash > 0:
    printBalance()
    drawDealerCards()
    bet = getBet()
    echo("")
    drawPlayerCard()
    if (cardC >= cardA) and (cardC <= cardB):
      echo("YOU WIN!!!")
      stash += bet
    else:
      echo("SORRY, YOU LOSE")
      stash -= bet
  echo("SORRY, FRIEND, BUT YOU BLEW YOUR WAD.");
  echo("")
  retry = tryAgain()
echo("O.K., HOPE YOU HAD FUN!");
