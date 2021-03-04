// STARS
//
// Converted from BASIC to Javascript by Qursch
//

function print(str)
{
    document.getElementById("output").appendChild(document.createTextNode(str));
}

function input()
{
    var input_element;
    var input_str;
    
    return new Promise(function (resolve) {
                       input_element = document.createElement("INPUT");
                       
                       print("? ");
                       input_element.setAttribute("type", "text");
                       input_element.setAttribute("length", "50");
                       document.getElementById("output").appendChild(input_element);
                       input_element.focus();
                       input_str = undefined;
                       input_element.addEventListener("keydown", function (event) {
                                                      if (event.keyCode == 13) {
                                                      input_str = input_element.value;
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
    var str = "";
    while (space-- > 0)
        str += " ";
    return str;
}

var guesses = 7;
var limit = 100;

// Main program
async function main()
{
    print(tab(33) + "STARS\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n\n\n");

    print("DO YOU WANT INSTRUCTIONS? (Y/N)");
    var instructions = await input();
    if(instructions.toLowerCase()[0] == "y") {
        print(`I AM THINKING OF A WHOLE NUMBER FROM 1 TO ${limit}\n`);
        print("TRY TO GUESS MY NUMBER.  AFTER YOU GUESS, I\n");
        print("WILL TYPE ONE OR MORE STARS (*).  THE MORE\n");
        print("STARS I TYPE, THE CLOSER YOU ARE TO MY NUMBER.\n");
        print("ONE STAR (*) MEANS FAR AWAY, SEVEN STARS (*******)\n");
        print(`MEANS REALLY CLOSE!  YOU GET ${guesses} GUESSES.\n\n\n`);
    }
    while (true) {
        var randomNum = Math.floor(Math.random() * 150) + 1;
        console.log(randomNum);

        print("OK, I AM THINKING OF A NUMBER, START GUESSING.\n\n");

        var loss = true;

        for(var guessNum=1; guessNum <= guesses; guessNum++) {
            print("YOUR GUESS");

            var guess = parseInt(await input());

            if(guess == randomNum) {
                loss = false;
                print("*".repeat(79) + "!!!\n");
                print(`YOU GOT IT IN ${guessNum} GUESSES!!! LET'S PLAY AGAIN...\n\n`);
                break;
            }

            var dist = Math.abs(guess - randomNum);
              
            //IMPLEMENT CHECK HERE
        }

        if(loss) {
            print(`SORRY, THAT'S ${guesses} GUESSES. THE NUMBER WAS ${randomNum}\n\n`);
        }
    }
}

main();
