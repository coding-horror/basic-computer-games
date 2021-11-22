// FLIPFLOP
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

var as = [];

// Main program
async function main()
{
    print(tab(32) + "FLIPFLOP\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    // *** Created by Michael Cass
    print("THE OBJECT OF THIS PUZZLE IS TO CHANGE THIS:\n");
    print("\n");
    print("X X X X X X X X X X\n");
    print("\n");
    print("TO THIS:\n");
    print("\n");
    print("O O O O O O O O O O\n");
    print("\n");
    print("BY TYPING THE NUMBER CORRESPONDING TO THE POSITION OF THE\n");
    print("LETTER ON SOME NUMBERS, ONE POSITION WILL CHANGE, ON\n");
    print("OTHERS, TWO WILL CHANGE.  TO RESET LINE TO ALL X'S, TYPE 0\n");
    print("(ZERO) AND TO START OVER IN THE MIDDLE OF A GAME, TYPE \n");
    print("11 (ELEVEN).\n");
    print("\n");
    while (1) {
        start = 1;
        do {
            z = 1;
            if (start == 1) {
                m = 0;
                q = Math.random();
                print("HERE IS THE STARTING LINE OF X'S.\n");
                print("\n");
                c = 0;
                start = 2;
            }
            if (start == 2) {
                print("1 2 3 4 5 6 7 8 9 10\n");
                print("X X X X X X X X X X\n");
                print("\n");
                for (x = 1; x <= 10; x++)
                    as[x] = "X";
                start = 0;
            }
            print("INPUT THE NUMBER");
            while (1) {
                n = parseInt(await input());
                if (n >= 0 && n <= 11)
                    break;
                print("ILLEGAL ENTRY--TRY AGAIN.\n");
            }
            if (n == 11) {
                start = 1;
                continue;
            }
            if (n == 0) {
                start = 2;
                continue;
            }
            if (m != n) {
                m = n;
                as[n] = (as[n] == "O" ? "X" : "O");
                do {
                    r = Math.tan(q + n / q - n) - Math.sin(q / n) + 336 * Math.sin(8 * n);
                    n = r - Math.floor(r);
                    n = Math.floor(10 * n);
                    as[n] = (as[n] == "O" ? "X" : "O");
                } while (m == n) ;
            } else {
                as[n] = (as[n] == "O" ? "X" : "O");
                do {
                    r = 0.592 * (1 / Math.tan(q / n + q)) / Math.sin(n * 2 + q) - Math.cos(n);
                    n = r - Math.floor(r);
                    n = Math.floor(10 * n);
                    as[n] = (as[n] == "O" ? "X" : "O");
                } while (m == n) ;
            }
            print("1 2 3 4 5 6 7 8 9 10\n");
            for (z = 1; z <= 10; z++)
                print(as[z] + " ");
            c++;
            print("\n");
            for (z = 1; z <= 10; z++) {
                if (as[z] != "O")
                    break;
            }
        } while (z <= 10) ;
        if (c <= 12) {
            print("VERY GOOD.  YOU GUESSED IT IN ONLY " + c + " GUESSES.\n");
        } else {
            print("TRY HARDER NEXT TIME.  IT TOOK YOU " + c + " GUESSES.\n");
        }
        print("DO YOU WANT TO TRY ANOTHER PUZZLE");
        str = await input();
        if (str.substr(0, 1) == "N")
            break;
    }
    print("\n");
}

main();
