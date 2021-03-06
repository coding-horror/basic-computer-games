// HI-LO
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
    print(tab(34) + "HI LO\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THIS IS THE GAME OF HI LO.\n");
    print("\n");
    print("YOU WILL HAVE 6 TRIES TO GUESS THE AMOUNT OF MONEY IN THE\n");
    print("HI LO JACKPOT, WHICH IS BETWEEN 1 AND 100 DOLLARS.  IF YOU\n");
    print("GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!\n");
    print("THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY.  HOWEVER,\n");
    print("IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS.\n");
    print("\n");
    r = 0;
    while (1) {
        b = 0;
        print("\n");
        y = Math.floor(100 * Math.random());
        for (b = 1; b <= 6; b++) {
            print("YOUR GUESS");
            a = parseInt(await input());
            if (a < y) {
                print("YOUR GUESS IS TOO LOW.\n");
            } else if (a > y) {
                print("YOUR GUESS IS TOO HIGH.\n");
            } else {
                break;
            }
            print("\n");
        }
        if (b > 6) {
            print("YOU BLEW IT...TOO BAD...THE NUMBER WAS " + y + "\n");
            r = 0;
        } else {
            print("GOT IT!!!!!!!!!!   YOU WIN " + y + " DOLLARS.\n");
            r += y;
            print("YOUR TOTAL WINNINGS ARE NOW " + r + " DOLLARS.\n");
        }
        print("\n");
        print("PLAY AGAIN (YES OR NO)");
        str = await input();
        if (str != "YES")
            break;
    }
    print("\n");
    print("SO LONG.  HOPE YOU ENJOYED YOURSELF!!!\n");
}

main();
