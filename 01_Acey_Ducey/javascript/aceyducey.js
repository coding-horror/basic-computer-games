// UTILITY VARIABLES

// By default:
// — Browsers have a window object
// — Node.js does not
// Checking for an undefined window object is a loose check
// to enable browser and Node.js support
const isRunningInBrowser = typeof window !== 'undefined';

// To easily validate input strings with utility functions
const validLowerCaseYesStrings = ['yes', 'y'];
const validLowerCaseNoStrings = ['no', 'n'];
const validLowerCaseYesAndNoStrings = [
    ...validLowerCaseYesStrings,
    ...validLowerCaseNoStrings,
];
// UTILITY VARIABLES

// Function to get a random number (card) 2-14 (ACE is 14)
function getRandomCard() {
    // In our game, the value of ACE is greater than face cards;
    // instead of having the value of ACE be 1, we’ll have it be 14.
    // So, we want to shift the range of random numbers from 1-13 to 2-14
    let min = 2;
    let max = 14;
    // Return random integer between two values, inclusive
    return Math.floor(Math.random() * (max - min + 1) + min);
}

function newGameCards() {
    let cardOne = getRandomCard();
    let cardTwo = getRandomCard();
    let cardThree = getRandomCard();
    // We want:
    // 1. cardOne and cardTwo to be different cards
    // 2. cardOne to be lower than cardTwo
    // So, while cardOne is greater than or equal too cardTwo
    // we will continue to generate random cards.
    while (cardOne >= cardTwo) {
        cardOne = getRandomCard();
        cardTwo = getRandomCard();
    }
    return [cardOne, cardTwo, cardThree];
}

// Function to get card value
function getCardValue(card) {
    let faceOrAce = {
        11: 'JACK',
        12: 'QUEEN',
        13: 'KING',
        14: 'ACE',
    };
    // If card value matches a key in faceOrAce, use faceOrAce value;
    // Else, return undefined and handle with the Nullish Coalescing Operator (??)
    // and default to card value.
    let cardValue = faceOrAce[card] ?? card;
    return cardValue;
}

print(spaces(26) + 'ACEY DUCEY CARD GAME');
print(spaces(15) + 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n');
print('ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER');
print('THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP');
print('YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING');
print('ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE');
print('A VALUE BETWEEN THE FIRST TWO.');
print("IF YOU DO NOT WANT TO BET, INPUT '0'");

main();

async function main() {
    let bet;
    let availableDollars = 100;

    // Loop game forever
    while (true) {
        let [cardOne, cardTwo, cardThree] = newGameCards();

        print(`YOU NOW HAVE ${availableDollars} DOLLARS.\n`);

        print('HERE ARE YOUR NEXT TWO CARDS: ');
        print(getCardValue(cardOne));
        print(getCardValue(cardTwo));
        print('');

        // Loop until receiving a valid bet
        let validBet = false;
        while (!validBet) {
            print('\nWHAT IS YOUR BET? ');
            bet = parseInt(await input(), 10);
            let minimumRequiredBet = 0;
            if (bet > minimumRequiredBet) {
                if (bet > availableDollars) {
                    print('SORRY, MY FRIEND, BUT YOU BET TOO MUCH.');
                    print(`YOU HAVE ONLY ${availableDollars} DOLLARS TO BET.`);
                } else {
                    validBet = true;
                }
            } else {
                // Does not meet minimum required bet
                print('CHICKEN!!');
                print('');
            }
        }

        print('\n\nHERE IS THE CARD WE DREW: ');
        print(getCardValue(cardThree));

        // Determine if player won or lost
        if (cardThree > cardOne && cardThree < cardTwo) {
            print('YOU WIN!!!');
            availableDollars = availableDollars + bet;
        } else {
            print('SORRY, YOU LOSE');

            if (bet >= availableDollars) {
                print('');
                print('');
                print('SORRY, FRIEND, BUT YOU BLEW YOUR WAD.');
                print('');
                print('');
                print('TRY AGAIN (YES OR NO)');

                let tryAgainInput = await input();

                print('');
                print('');

                if (isValidYesNoString(tryAgainInput)) {
                    availableDollars = 100;
                } else {
                    print('O.K., HOPE YOU HAD FUN!');
                    break;
                }
            } else {
                availableDollars = availableDollars - bet;
            }
        }
    }
}

// UTILITY FUNCTIONS
function isValidYesNoString(string) {
    return validLowerCaseYesAndNoStrings.includes(string.toLowerCase());
}

function isValidYesString(string) {
    return validLowerCaseYesStrings.includes(string.toLowerCase());
}

function isValidNoString(string) {
    return validLowerCaseNoStrings.includes(string.toLowerCase());
}

function print(string) {
    if (isRunningInBrowser) {
        // Adds trailing newline to match console.log behavior
        document
            .getElementById('output')
            .appendChild(document.createTextNode(string + '\n'));
    } else {
        console.log(string);
    }
}

function input() {
    if (isRunningInBrowser) {
        // Accept input from the browser DOM input
        return new Promise((resolve) => {
            const outputElement = document.querySelector('#output');
            const inputElement = document.createElement('input');
            outputElement.append(inputElement);
            inputElement.focus();

            inputElement.addEventListener('keydown', (event) => {
                if (event.key === 'Enter') {
                    const result = inputElement.value;
                    inputElement.remove();
                    print(result);
                    print('');
                    resolve(result);
                }
            });
        });
    } else {
        // Accept input from the command line in Node.js
        // See: https://nodejs.dev/learn/accept-input-from-the-command-line-in-nodejs
        return new Promise(function (resolve) {
            const readline = require('readline').createInterface({
                input: process.stdin,
                output: process.stdout,
            });
            readline.question('', function (input) {
                resolve(input);
                readline.close();
            });
        });
    }
}

function printInline(string) {
    if (isRunningInBrowser) {
        document
            .getElementById('output')
            .appendChild(document.createTextNode(string));
    } else {
        process.stdout.write(string);
    }
}

function spaces(numberOfSpaces) {
    return ' '.repeat(numberOfSpaces);
}

// UTILITY FUNCTIONS
