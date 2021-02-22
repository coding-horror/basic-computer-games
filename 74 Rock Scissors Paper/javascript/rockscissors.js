// ROCK, SCISSORS, PAPER
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

// Main control section
async function main()
{
    print(tab(21) + "GAME OF ROCK, SCISSORS, PAPER\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    while (1) {
        print("HOW MANY GAMES");
        q = parseInt(await input());
        if (q >= 11)
            print("SORRY, BUT WE AREN'T ALLOWED TO PLAY THAT MANY.\n");
        else
            break;
    }
    h = 0;  // Human
    c = 0;  // Computer
    for (g = 1; g <= q; g++ ) {
        print("\n");
        print("GAME NUMBER " + g + "\n");
        x = Math.floor(Math.random() * 3 + 1);
        while (1) {
            print("3=ROCK...2=SCISSORS...1=PAPER\n");
            print("1...2...3...WHAT'S YOUR CHOICE");
            k = parseInt(await input());
            if (k != 1 && k != 2 && k != 3)
                print("INVALID.\n");
            else
                break;
        }
        print("THIS IS MY CHOICE...");
        switch (x) {
            case 1:
                print("...PAPER\n");
                break;
            case 2:
                print("...SCISSORS\n");
                break;
            case 3:
                print("...ROCK\n");
                break;
        }
        if (x == k) {
            print("TIE GAME.  NO WINNER.\n");
        } else if ((x > k && (k != 1 || x != 3)) || (x == 1 && k == 3)) {
            print("WOW!  I WIN!!!\n");
            c++;
        } else {
            print("YOU WIN!!!\n");
            h++;
        }
    }
    print("\n");
    print("HERE IS THE FINAL GAME SCORE:\n");
    print("I HAVE WON " + c + " GAME(S).\n");
    print("YOU HAVE WON " + h + " GAME(S).\n");
    print("AND " + (q - (c + h)) + " GAME(S) ENDED IN A TIE.\n");
    print("\n");
    print("THANKS FOR PLAYING!!\n");
}

main();
