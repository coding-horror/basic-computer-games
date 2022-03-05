// LETTER
//
// Converted from BASIC to Javascript by Oscar Toledo G. (nanochess)
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

// Main program
async function main()
{
    print(tab(33) + "LETTER\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("LETTER GUESSING GAME\n");
    print("\n");
    print("I'LL THINK OF A LETTER OF THE ALPHABET, A TO Z.\n");
    print("TRY TO GUESS MY LETTER AND I'LL GIVE YOU CLUES\n");
    print("AS TO HOW CLOSE YOU'RE GETTING TO MY LETTER.\n");
    while (1) {
        l = 65 + Math.floor(26 * Math.random());
        g = 0;
        print("\n");
        print("O.K., I HAVE A LETTER.  START GUESSING.\n");
        while (1) {

            print("\n");
            print("WHAT IS YOUR GUESS");
            g++;
            str = await input();
            a = str.charCodeAt(0);
            print("\n");
            if (a == l)
                break;
            if (a < l) {
                print("TOO LOW.  TRY A HIGHER LETTER.\n");
            } else {
                print("TOO HIGH.  TRY A LOWER LETTER.\n");
            }
        }
        print("\n");
        print("YOU GOT IT IN " + g + " GUESSES!!\n");
        if (g > 5) {
            print("BUT IT SHOULDN'T TAKE MORE THAN 5 GUESSES!\n");
        } else {
            print("GOOD JOB !!!!!\n");
        }
        print("\n");
        print("LET'S PLAY AGAIN.....");
    }
}

main();
