// WORD
//
// Converted from BASIC to Javascript by Oscar Toledo G. (nanochess)
//

function print(str)
{
    document.getElementById("output").appendChild(document.createTextNode(str));
}

function input()
{

    return new Promise(function (resolve) {
                       const input_element = document.createElement("INPUT");

                       print("? ");
                       input_element.setAttribute("type", "text");
                       input_element.setAttribute("length", "50");
                       document.getElementById("output").appendChild(input_element);
                       input_element.focus();
                       input_element.addEventListener("keydown", function (event) {
                                                      if (event.keyCode === 13) {
                                                          const input_str = input_element.value;
                                                          document.getElementById("output").removeChild(input_element);
                                                          print(input_str);
                                                          print("\n");
                                                          resolve(input_str);
                                                      }
                                                      });
                       });
}

function tab(space)
{
    let str = "";
    while (space-- > 0)
        str += " ";
    return str;
}

const words = ["DINKY", "SMOKE", "WATER", "GLASS", "TRAIN",
             "MIGHT", "FIRST", "CANDY", "CHAMP", "WOULD",
             "CLUMP", "DOPEY"];

const secretWordDetails = [];
const knownLettersDetails = [];
const guessDetails = [];
const lettersInCommonDetails = [];

// Main control section
async function main()
{
    print(tab(33) + "WORD\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("I AM THINKING OF A WORD -- YOU GUESS IT.  I WILL GIVE YOU\n");
    print("CLUES TO HELP YOU GET IT.  GOOD LUCK!!\n");
    print("\n");
    print("\n");
    while (1) {
        print("\n");
        print("\n");
        print("YOU ARE STARTING A NEW GAME...\n");

        const n = words.length;
        const secretWord = words[Math.floor(Math.random() * n)];

        let guessCount = 0;
        secretWordDetails[0] = secretWord.length;
        for (let i = 1; i <= secretWord.length; i++)
            secretWordDetails[i] = secretWord.charCodeAt(i - 1);
        for (let i = 1; i <= 5; i++)
            knownLettersDetails[i] = "-".charCodeAt(0);
        for (let j = 1; j <= 5; j++)
            lettersInCommonDetails[j] = 0;

        let guess = undefined;
        while (1) {
            print("GUESS A FIVE LETTER WORD");
            guess = (await input()).toUpperCase();
            guessCount++;
            if (secretWord === guess)
                break;
            for (let i = 1; i <= 7; i++)
                lettersInCommonDetails[i] = 0;

            // store detail about the guess
            guessDetails[0] = guess.length;
            for (let i = 1; i <= guess.length; i++) {
                guessDetails[i] = guess.charCodeAt(i - 1);
            }

            if (guessDetails[1] === 63) {
                print("THE SECRET WORD IS " + secretWord + "\n");
                print("\n");
                break;
            }
            if (guessDetails[0] !== 5) {
                print("YOU MUST GUESS A 5 LETTER WORD.  START AGAIN.\n");
                print("\n");
                guessCount--;
                continue;
            }
            let lettersInCommonCount = 0;
            let nextCorrectLetterIndex = 1;
            for (let i = 1; i <= 5; i++) {
                for (let j = 1; j <= 5; j++) {
                    if (secretWordDetails[i] === guessDetails[j]) {
                        lettersInCommonDetails[nextCorrectLetterIndex] = guessDetails[j];
                        nextCorrectLetterIndex++;
                        if (i === j)
                            knownLettersDetails[j] = guessDetails[j];
                        lettersInCommonCount++;
                    }
                }
            }
            knownLettersDetails[0] = 5;
            lettersInCommonDetails[0] = lettersInCommonCount;

            let lettersInCommonText = "";
            for (let i = 1; i <= lettersInCommonDetails[0]; i++)
                lettersInCommonText += String.fromCharCode(lettersInCommonDetails[i]);
            print("THERE WERE " + lettersInCommonCount + " MATCHES AND THE COMMON LETTERS WERE... " + lettersInCommonText + "\n");

            let knownLettersText = "";
            for (let i = 1; i <= knownLettersDetails[0]; i++)
                knownLettersText += String.fromCharCode(knownLettersDetails[i]);
            print("FROM THE EXACT LETTER MATCHES, YOU KNOW............ " + knownLettersText + "\n");
            if (knownLettersText === secretWord) {
                guess = knownLettersText;
                break;
            }
            if (lettersInCommonCount <= 1) {
                print("\n");
                print("IF YOU GIVE UP, TYPE '?' FOR YOUR NEXT GUESS.\n");
                print("\n");
            }
        }
        if (secretWord === guess) {
            print("YOU HAVE GUESSED THE WORD.  IT TOOK " + guessCount + " GUESSES!\n");
            print("\n");
        } else {
            continue;
        }
        print("WANT TO PLAY AGAIN");
        const playAgainResponse = (await input()).toUpperCase();
        if (playAgainResponse !== "YES")
            break;
    }
}

main();
