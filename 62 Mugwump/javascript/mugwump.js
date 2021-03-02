// MUGWUMP
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

var p = [];

// Main program
async function main()
{
    print(tab(33) + "MUGWUMP\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    // Courtesy People's Computer Company
    print("THE OBJECT OF THIS GAME IS TO FIND FOUR MUGWUMPS\n");
    print("HIDDEN ON A 10 BY 10 GRID.  HOMEBASE IS POSITION 0,0.\n");
    print("ANY GUESS YOU MAKE MUST BE TWO NUMBERS WITH EACH\n");
    print("NUMBER BETWEEN 0 AND 9, INCLUSIVE.  FIRST NUMBER\n");
    print("IS DISTANCE TO RIGHT OF HOMEBASE AND SECOND NUMBER\n");
    print("IS DISTANCE ABOVE HOMEBASE.\n");
    print("\n");
    print("YOU GET 10 TRIES.  AFTER EACH TRY, I WILL TELL\n");
    print("YOU HOW FAR YOU ARE FROM EACH MUGWUMP.\n");
    print("\n");
    while (1) {
        for (i = 1; i <= 4; i++) {
            p[i] = [];
            for (j = 1; j <= 2; j++) {
                p[i][j] = Math.floor(10 * Math.random());
            }
        }
        t = 0;
        do {
            t++;
            print("\n");
            print("\n");
            print("TURN NO. " + t + " -- WHAT IS YOUR GUESS");
            str = await input();
            m = parseInt(str);
            n = parseInt(str.substr(str.indexOf(",") + 1));
            for (i = 1; i <= 4; i++) {
                if (p[i][1] == -1)
                    continue;
                if (p[i][1] == m && p[i][2] == n) {
                    p[i][1] = -1;
                    print("YOU HAVE FOUND MUGWUMP " + i + "\n");
                } else {
                    d = Math.sqrt(Math.pow(p[i][1] - m, 2) + Math.pow(p[i][2] - n, 2));
                    print("YOU ARE " + Math.floor(d * 10) / 10 + " UNITS FROM MUGWUMP " + i + "\n");
                }
            }
            for (j = 1; j <= 4; j++) {
                if (p[j][1] != -1)
                    break;
            }
            if (j > 4) {
                print("\n");
                print("YOU GOT THEM ALL IN " + t + " TURNS!\n");
                break;
            }
        } while (t < 10) ;
        if (t == 10) {
            print("\n");
            print("SORRY, THAT'S 10 TRIES.  HERE IS WHERE THEY'RE HIDING:\n");
            for (i = 1; i <= 4; i++) {
                if (p[i][1] != -1)
                    print("MUGWUMP " + i + " IS AT (" + p[i][1] + "," + p[i][2] + ")\n");
            }
        }
        print("\n");
        print("THAT WAS FUN! LET'S PLAY AGAIN.......\n");
        print("FOUR MORE MUGWUMPS ARE NOW IN HIDING.\n");
    }
}

main();
