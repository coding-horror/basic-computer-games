// BUG
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
                       input_element.addEventListener("keydown",
                           function (event) {
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

function waitNSeconds(n) {
    return new Promise(resolve => setTimeout(resolve, n*1000));
}

function scrollToBottom() {
    window.scrollTo(0, document.body.scrollHeight);
}

function draw_head()
{
    print("        HHHHHHH\n");
    print("        H     H\n");
    print("        H O O H\n");
    print("        H     H\n");
    print("        H  V  H\n");
    print("        HHHHHHH\n");
}

function drawFeelers(feelerCount, character) {
    for (let z = 1; z <= 4; z++) {
        print(tab(10));
        for (let x = 1; x <= feelerCount; x++) {
            print(character + " ");
        }
        print("\n");
    }
}

function drawNeck() {
    for (let z = 1; z <= 2; z++)
        print("          N N\n");
}

function drawBody(computerTailCount) {
    print("     BBBBBBBBBBBB\n");
    for (let z = 1; z <= 2; z++)
        print("     B          B\n");
    if (computerTailCount === 1)
        print("TTTTTB          B\n");
    print("     BBBBBBBBBBBB\n");
}

function drawFeet(computerFeetCount) {
    for (let z = 1; z <= 2; z++) {
        print(tab(5));
        for (let x = 1; x <= computerFeetCount; x++)
            print(" L");
        print("\n");
    }
}

function drawBug(playerFeelerCount, playerHeadCount, playerNeckCount, playerBodyCount, playerTailCount, playerFeetCount, feelerCharacter) {
    if (playerFeelerCount !== 0) {
        drawFeelers(playerFeelerCount, feelerCharacter);
    }
    if (playerHeadCount !== 0)
        draw_head();
    if (playerNeckCount !== 0) {
        drawNeck();
    }
    if (playerBodyCount !== 0) {
        drawBody(playerTailCount)
    }
    if (playerFeetCount !== 0) {
        drawFeet(playerFeetCount);
    }
    for (let z = 1; z <= 4; z++)
        print("\n");
}

// Main program
async function main()
{
    print(tab(34) + "BUG\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    let playerFeelerCount = 0;
    let playerHeadCount = 0;
    let playerNeckCount = 0;
    let playerBodyCount = 0;
    let playerFeetCount = 0;
    let playerTailCount = 0;

    let computerFeelerCount = 0;
    let computerHeadCount = 0;
    let computerNeckCount = 0;
    let computerBodyCount = 0;
    let computerTailCount = 0;
    let computerFeetCount = 0;

    print("THE GAME BUG\n");
    print("I HOPE YOU ENJOY THIS GAME.\n");
    print("\n");
    print("DO YOU WANT INSTRUCTIONS");
    const instructionsRequired = await input();
    if (instructionsRequired.toUpperCase() !== "NO") {
        print("THE OBJECT OF BUG IS TO FINISH YOUR BUG BEFORE I FINISH\n");
        print("MINE. EACH NUMBER STANDS FOR A PART OF THE BUG BODY.\n");
        print("I WILL ROLL THE DIE FOR YOU, TELL YOU WHAT I ROLLED FOR YOU\n");
        print("WHAT THE NUMBER STANDS FOR, AND IF YOU CAN GET THE PART.\n");
        print("IF YOU CAN GET THE PART I WILL GIVE IT TO YOU.\n");
        print("THE SAME WILL HAPPEN ON MY TURN.\n");
        print("IF THERE IS A CHANGE IN EITHER BUG I WILL GIVE YOU THE\n");
        print("OPTION OF SEEING THE PICTURES OF THE BUGS.\n");
        print("THE NUMBERS STAND FOR PARTS AS FOLLOWS:\n");
        print("NUMBER\tPART\tNUMBER OF PART NEEDED\n");
        print("1\tBODY\t1\n");
        print("2\tNECK\t1\n");
        print("3\tHEAD\t1\n");
        print("4\tFEELERS\t2\n");
        print("5\tTAIL\t1\n");
        print("6\tLEGS\t6\n");
        print("\n");
        print("\n");
    }

    let gameInProgress = true;
    while (gameInProgress) {
        let dieRoll = Math.floor(6 * Math.random() + 1);
        let partFound = false;
        print("YOU ROLLED A " + dieRoll + "\n");
        switch (dieRoll) {
            case 1:
                print("1=BODY\n");
                if (playerBodyCount === 0) {
                    print("YOU NOW HAVE A BODY.\n");
                    playerBodyCount = 1;
                    partFound = true;
                } else {
                    print("YOU DO NOT NEED A BODY.\n");
                }
                break;
            case 2:
                print("2=NECK\n");
                if (playerNeckCount === 0) {
                    if (playerBodyCount === 0) {
                        print("YOU DO NOT HAVE A BODY.\n");
                    } else {
                        print("YOU NOW HAVE A NECK.\n");
                        playerNeckCount = 1;
                        partFound = true;
                    }
                } else {
                    print("YOU DO NOT NEED A NECK.\n");
                }
                break;
            case 3:
                print("3=HEAD\n");
                if (playerNeckCount === 0) {
                    print("YOU DO NOT HAVE A NECK.\n");
                } else if (playerHeadCount === 0) {
                    print("YOU NEEDED A HEAD.\n");
                    playerHeadCount = 1;
                    partFound = true;
                } else {
                    print("YOU HAVE A HEAD.\n");
                }
                break;
            case 4:
                print("4=FEELERS\n");
                if (playerHeadCount === 0) {
                    print("YOU DO NOT HAVE A HEAD.\n");
                } else if (playerFeelerCount === 2) {
                    print("YOU HAVE TWO FEELERS ALREADY.\n");
                } else {
                    print("I NOW GIVE YOU A FEELER.\n");
                    playerFeelerCount ++;
                    partFound = true;
                }
                break;
            case 5:
                print("5=TAIL\n");
                if (playerBodyCount === 0) {
                    print("YOU DO NOT HAVE A BODY.\n");
                } else if (playerTailCount === 1) {
                    print("YOU ALREADY HAVE A TAIL.\n");
                } else {
                    print("I NOW GIVE YOU A TAIL.\n");
                    playerTailCount++;
                    partFound = true;
                }
                break;
            case 6:
                print("6=LEG\n");
                if (playerFeetCount === 6) {
                    print("YOU HAVE 6 FEET ALREADY.\n");
                } else if (playerBodyCount === 0) {
                    print("YOU DO NOT HAVE A BODY.\n");
                } else {
                    playerFeetCount++;
                    partFound = true;
                    print("YOU NOW HAVE " + playerFeetCount + " LEGS.\n");
                }
                break;
        }
        dieRoll = Math.floor(6 * Math.random() + 1) ;
        print("\n");
        scrollToBottom();
        await waitNSeconds(1);

        print("I ROLLED A " + dieRoll + "\n");
        switch (dieRoll) {
            case 1:
                print("1=BODY\n");
                if (computerBodyCount === 1) {
                    print("I DO NOT NEED A BODY.\n");
                } else {
                    print("I NOW HAVE A BODY.\n");
                    partFound = true;
                    computerBodyCount = 1;
                }
                break;
            case 2:
                print("2=NECK\n");
                if (computerNeckCount === 1) {
                    print("I DO NOT NEED A NECK.\n");
                } else if (computerBodyCount === 0) {
                    print("I DO NOT HAVE A BODY.\n");
                } else {
                    print("I NOW HAVE A NECK.\n");
                    computerNeckCount = 1;
                    partFound = true;
                }
                break;
            case 3:
                print("3=HEAD\n");
                if (computerNeckCount === 0) {
                    print("I DO NOT HAVE A NECK.\n");
                } else if (computerHeadCount === 1) {
                    print("I DO NOT NEED A HEAD.\n");
                } else {
                    print("I NEEDED A HEAD.\n");
                    computerHeadCount = 1;
                    partFound = true;
                }
                break;
            case 4:
                print("4=FEELERS\n");
                if (computerHeadCount === 0) {
                    print("I DO NOT HAVE A HEAD.\n");
                } else if (computerFeelerCount === 2) {
                    print("I HAVE 2 FEELERS ALREADY.\n");
                } else {
                    print("I GET A FEELER.\n");
                    computerFeelerCount++;
                    partFound = true;
                }
                break;
            case 5:
                print("5=TAIL\n");
                if (computerBodyCount === 0) {
                    print("I DO NOT HAVE A BODY.\n");
                } else if (computerTailCount === 1) {
                    print("I DO NOT NEED A TAIL.\n");
                } else {
                    print("I NOW HAVE A TAIL.\n");
                    computerTailCount = 1;
                    partFound = true;
                }
                break;
            case 6:
                print("6=LEGS\n");
                if (computerFeetCount === 6) {
                    print("I HAVE 6 FEET.\n");
                } else if (computerBodyCount === 0) {
                    print("I DO NOT HAVE A BODY.\n");
                } else {
                    computerFeetCount++;
                    partFound = true;
                    print("I NOW HAVE " + computerFeetCount + " LEGS.\n");
                }
                break;
        }
        if (playerFeelerCount === 2 && playerTailCount === 1 && playerFeetCount === 6) {
            print("YOUR BUG IS FINISHED.\n");
            gameInProgress = false;
        }
        if (computerFeelerCount === 2 && computerBodyCount === 1 && computerFeetCount === 6) {
            print("MY BUG IS FINISHED.\n");
            gameInProgress = false;
        }
        if (!partFound)
            continue;
        print("DO YOU WANT THE PICTURES");
        const showPictures = await input();
        if (showPictures.toUpperCase() === "NO")
            continue;
        print("*****YOUR BUG*****\n");
        print("\n");
        print("\n");
        drawBug(playerFeelerCount, playerHeadCount, playerNeckCount, playerBodyCount, playerTailCount, playerFeetCount, "A");
        print("*****MY BUG*****\n");
        print("\n");
        print("\n");
        drawBug(computerFeelerCount, computerHeadCount, computerNeckCount, computerBodyCount, computerTailCount, computerFeetCount, "F");
        for (let z = 1; z <= 4; z++)
            print("\n");
    }
    print("I HOPE YOU ENJOYED THE GAME, PLAY IT AGAIN SOON!!\n");
    scrollToBottom();
}

main();
