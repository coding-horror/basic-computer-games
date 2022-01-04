// WAR
//
// Original conversion from BASIC to Javascript by Oscar Toledo G. (nanochess)
//

function print(str) {
    document.getElementById("output").appendChild(document.createTextNode(str));
}

function tab(space) {
    let str = "";
    while (space-- > 0) {
        str += " ";
    }
    return str;
}

function input() {
    return new Promise(function (resolve) {
        const input_element = document.createElement("INPUT");

        print("? ");
        input_element.setAttribute("type", "text");
        input_element.setAttribute("length", "50");
        document.getElementById("output").appendChild(input_element);
        input_element.focus();
        input_element.addEventListener("keydown", function (event) {
            if (event.keyCode == 13) {
                const input_str = input_element.value;
                document.getElementById("output").removeChild(input_element);
                print(input_str);
                print("\n");
                resolve(input_str);
            }
        });
    });
}

async function askYesOrNo(question) {
    while (1) {
        print(question);
        const str = await input();
        if (str == "YES") {
            return true;
        }
        else if (str == "NO") {
            return false;
        }
        else {
            print("YES OR NO, PLEASE.  ");
        }
    }
}

async function askAboutInstructions() {
    const playerWantsInstructions = await askYesOrNo("DO YOU WANT DIRECTIONS");
    if (playerWantsInstructions) {
        print("THE COMPUTER GIVES YOU AND IT A 'CARD'.  THE HIGHER CARD\n");
        print("(NUMERICALLY) WINS.  THE GAME ENDS WHEN YOU CHOOSE NOT TO\n");
        print("CONTINUE OR WHEN YOU HAVE FINISHED THE PACK.\n");
    }
    print("\n");
    print("\n");
}

function createGameDeck(cards, gameSize) {
    const deck = [];
    const deckSize = cards.length;
    for (let j = 0; j < gameSize; j++) {
        let card;

        // Compute a new card index until we find one that isn't already in the new deck
        do {
            card = Math.floor(deckSize * Math.random());
        } while (deck.includes(card));
        deck[j] = card
    }
    return deck;
}

function computeCardValue(cardIndex) {
    return Math.floor(cardIndex / 4);
}

function printGameOver(playerScore, computerScore) {
    print("\n");
    print("\n");
    print(`WE HAVE RUN OUT OF CARDS.  FINAL SCORE:  YOU: ${playerScore}  THE COMPUTER: ${computerScore}\n`);
    print("\n");
}

function printTitle() {
    print(tab(33) + "WAR\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THIS IS THE CARD GAME OF WAR.  EACH CARD IS GIVEN BY SUIT-#\n");
    print("AS S-7 FOR SPADE 7.  ");
}

function printCards(playerCard, computerCard) {
    print("\n");
    print(`YOU: ${playerCard}\tCOMPUTER: ${computerCard}\n`);
}

const cards = [
    "S-2", "H-2", "C-2", "D-2",
    "S-3", "H-3", "C-3", "D-3",
    "S-4", "H-4", "C-4", "D-4",
    "S-5", "H-5", "C-5", "D-5",
    "S-6", "H-6", "C-6", "D-6",
    "S-7", "H-7", "C-7", "D-7",
    "S-8", "H-8", "C-8", "D-8",
    "S-9", "H-9", "C-9", "D-9",
    "S-10", "H-10", "C-10", "D-10",
    "S-J", "H-J", "C-J", "D-J",
    "S-Q", "H-Q", "C-Q", "D-Q",
    "S-K", "H-K", "C-K", "D-K",
    "S-A", "H-A", "C-A", "D-A"
];

// Main control section
async function main() {
    printTitle();
    await askAboutInstructions();
    
    let computerScore = 0;
    let playerScore = 0;
    
    // Generate a random deck
    const gameSize = 4;
    const deck = createGameDeck(cards, gameSize);
    let shouldContinuePlaying = true;
    
    while (deck.length > 0 && shouldContinuePlaying) {
        const m1 = deck.shift();    // Take a card
        const m2 = deck.shift();    // Take a card
        printCards(cards[m1], cards[m2]);

        const playerCardValue = computeCardValue(m1);
        const computerCardValue = computeCardValue(m2);
        if (playerCardValue < computerCardValue) {
            computerScore++;
            print("THE COMPUTER WINS!!! YOU HAVE " + playerScore + " AND THE COMPUTER HAS " + computerScore + "\n");
        } else if (playerCardValue > computerCardValue) {
            playerScore++;
            print("YOU WIN. YOU HAVE " + playerScore + " AND THE COMPUTER HAS " + computerScore + "\n");
        } else {
            print("TIE.  NO SCORE CHANGE.\n");
        }

        if (deck.length === 0) {
            printGameOver(playerScore, computerScore);
        }
        else {
            shouldContinuePlaying = await askYesOrNo("DO YOU WANT TO CONTINUE");
        }
    }
    print("THANKS FOR PLAYING.  IT WAS FUN.\n");
    print("\n");
}

main();
