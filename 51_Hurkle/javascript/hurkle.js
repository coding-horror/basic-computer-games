// BATNUM
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
    print(tab(33) + "HURKLE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    n = 5;
    g = 10;
    print("\n");
    print("A HURKLE IS HIDING ON A " + g + " BY " + g + " GRID. HOMEBASE\n");
    print("ON THE GRID IS POINT 0,0 IN THE SOUTHWEST CORNER,\n");
    print("AND ANY POINT ON THE GRID IS DESIGNATED BY A\n");
    print("PAIR OF WHOLE NUMBERS SEPERATED BY A COMMA. THE FIRST\n");
    print("NUMBER IS THE HORIZONTAL POSITION AND THE SECOND NUMBER\n");
    print("IS THE VERTICAL POSITION. YOU MUST TRY TO\n");
    print("GUESS THE HURKLE'S GRIDPOINT. YOU GET " + n + " TRIES.\n");
    print("AFTER EACH TRY, I WILL TELL YOU THE APPROXIMATE\n");
    print("DIRECTION TO GO TO LOOK FOR THE HURKLE.\n");
    print("\n");
    while (1) {
        a = Math.floor(g * Math.random());
        b = Math.floor(g * Math.random());
        for (k = 1; k <= n; k++) {
            print("GUESS #" + k + " ");
            str = await input();
            x = parseInt(str);
            y = parseInt(str.substr(str.indexOf(",") + 1));
            if (x == a && y == b) {
                print("\n");
                print("YOU FOUND HIM IN " + k + " GUESSES!\n");
                break;
            }
            print("GO ");
            if (y < b) {
                print("NORTH");
            } else if (y > b) {
                print("SOUTH");
            }
            if (x < a) {
                print("EAST\n");
            } else {
                print("WEST\n");
            }
        }
        if (k > n) {
            print("\n");
            print("SORRY, THAT'S " + n + " GUESSES.\n");
            print("THE HURKLE IS AT " + a + "," + b + "\n");
        }
        print("\n");
        print("LET'S PLAY AGAIN, HURKLE IS HIDING.\n");
        print("\n");
    }
}

main();
