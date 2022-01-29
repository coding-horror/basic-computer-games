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

// These are the words that the game knows about> If you want a bigger challenge you could add more words to the array
const WORDS = ["DINKY", "SMOKE", "WATER", "GLASS", "TRAIN",
             "MIGHT", "FIRST", "CANDY", "CHAMP", "WOULD",
             "CLUMP", "DOPEY"];
const WORD_COUNT = WORDS.length;

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
    outer: while (1) {
        print("\n");
        print("\n");
        print("YOU ARE STARTING A NEW GAME...\n");

        const secretWord = WORDS[Math.floor(Math.random() * WORD_COUNT)];

        let guessCount = 0;
        // This array holds the letters which have been found in the correct position across all guesses
        // For instance if the word is "PLAIN" and the guesses so far are
        // "SHALL" ("A" correct) and "CLIMB" ("L" correct) then it will hold "-LA--"
        const knownLetters = [];
        for (let i = 0; i < 5; i++)
            knownLetters[i] = "-";

        let guess = undefined;
        while (1) {
            print("GUESS A FIVE LETTER WORD");
            guess = (await input()).toUpperCase();
            guessCount++;
            if (secretWord === guess) {
                // The player has guessed correctly
                break;
            }

            if (guess.charAt(0) === "?") {
                // Player has given up
                print("THE SECRET WORD IS " + secretWord + "\n");
                print("\n");
                // Start a new game by going to the start of the outer while loop
                continue outer;
            }

            if (guess.length !== 5) {
                print("YOU MUST GUESS A 5 LETTER WORD.  START AGAIN.\n");
                print("\n");
                guessCount--;
                continue;
            }

            // Two things happen in this double loop:
            // 1. Letters which are in both the guessed and secret words are put in the lettersInCommon array
            // 2. Letters which are in the correct position in the guessed word are added to the knownLetters array
            let lettersInCommonCount = 0;
            const lettersInCommon = [];
            for (let i = 0; i < 5; i++) {// loop round characters in secret word
                let secretWordCharacter = secretWord.charAt(i);
                for (let j = 0; j < 5; j++) {// loop round characters in guessed word
                    let guessedWordCharacter = guess.charAt(j);
                    if (secretWordCharacter === guessedWordCharacter) {
                        lettersInCommon[lettersInCommonCount] = guessedWordCharacter;
                        if (i === j) {
                            // Letter is in the exact position so add to the known letters array
                            knownLetters[j] = guessedWordCharacter;
                        }
                        lettersInCommonCount++;
                    }
                }
            }

            const lettersInCommonText = lettersInCommon.join("");
            print("THERE WERE " + lettersInCommonCount + " MATCHES AND THE COMMON LETTERS WERE... " + lettersInCommonText + "\n");

            const knownLettersText = knownLetters.join("");
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

        print("YOU HAVE GUESSED THE WORD.  IT TOOK " + guessCount + " GUESSES!\n");
        print("\n");

        print("WANT TO PLAY AGAIN");
        const playAgainResponse = (await input()).toUpperCase();
        if (playAgainResponse !== "YES")
            break;
    }
}

main();
