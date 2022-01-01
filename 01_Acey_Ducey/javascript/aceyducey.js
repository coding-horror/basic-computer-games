import { readLine, print, spaces } from "./io.js";

const minFaceCard = 11;
const faceCards = {
  11: "JACK",
  12: "QUEEN",
  13: "KING",
  14: "ACE"
};

function randomCard() {
  return Math.floor(Math.random() * 13 + 2);
}

function printCard(card) {
  if (card < minFaceCard) {
    print(card);
  } else {
    print(faceCards[card]);
  }
  print("\n");
}

print(spaces(26) + "ACEY DUCEY CARD GAME\n");
print(spaces(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n");
print("ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER\n");
print("THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP\n");
print("YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING\n");
print("ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE\n");
print("A VALUE BETWEEN THE FIRST TWO.\n");
print("IF YOU DO NOT WANT TO BET, INPUT '0'\n");

let currentMoney = 100;
while (true) {
  print(`YOU NOW HAVE ${currentMoney} DOLLARS.\n\n`);

  let card1, card2, currentBet;
  do {
    print("HERE ARE YOUR NEXT TWO CARDS: \n");
    [card1, card2] = [randomCard(), randomCard()];

    // Ensure we always show cards in order of lowest to highest, and we never
    // get two of the same card.
    do {
      card1 = randomCard();
      card2 = randomCard();
    } while (card1 >= card2);

    printCard(card1);
    printCard(card2);
    print("\n");

    while (true) {
      print("\nWHAT IS YOUR BET? ");
      currentBet = parseInt(await readLine(), 10);

      if (currentBet > 0) {
        if (currentBet > currentMoney) {
          print("SORRY, MY FRIEND, BUT YOU BET TOO MUCH.\n");
          print(`YOU HAVE ONLY ${currentMoney} DOLLARS TO BET.\n`);
          continue;
        }
        break;
      }

      // Invalid bet value. Output an error message and reset to undefined to
      // restart the loop with new cards.
      currentBet = undefined;
      print("CHICKEN!!\n");
      print("\n");
      break;
    }
  } while (currentBet === undefined);

  const actualCard = randomCard();
  print("\n\nHERE IS THE CARD WE DREW:\n")
  printCard(actualCard);
  print("\n\n");

  if (actualCard > card1 && actualCard < card2) {
    print("YOU WIN!!!\n");
    currentMoney += currentBet;
  } else {
    print("SORRY, YOU LOSE\n");
    if (currentBet < currentMoney) {
      currentMoney -= currentBet;
    } else {
      print("\n\nSORRY, FRIEND, BUT YOU BLEW YOUR WAD.\n\n\n");
      print("TRY AGAIN (YES OR NO)");
      const tryAgain = await readLine();
      print("\n\n");
      if (tryAgain.toLowerCase() === "yes") {
        currentMoney = 100;
      } else {
        print("O.K., HOPE YOU HAD FUN!");
        break;
      }
    }
  }
}
