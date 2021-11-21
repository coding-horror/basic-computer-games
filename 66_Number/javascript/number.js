// NUMBER
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
    print(tab(33) + "NUMBER\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("YOU HAVE 100 POINTS.  BY GUESSING NUMBERS FROM 1 TO 5, YOU\n");
    print("CAN GAIN OR LOSE POINTS DEPENDING UPON HOW CLOSE YOU GET TO\n");
    print("A RANDOM NUMBER SELECTED BY THE COMPUTER.\n");
    print("\n");
    print("YOU OCCASIONALLY WILL GET A JACKPOT WHICH WILL DOUBLE(!)\n");
    print("YOUR POINT COUNT.  YOU WIN WHEN YOU GET 500 POINTS.\n");
    print("\n");
    p = 0;
    while (1) {
        do {
            print("GUESS A NUMBER FROM 1 TO 5");
            g = parseInt(await input());
        } while (g < 1 || g > 5) ;
        r = Math.floor(5 * Math.random() + 1);
        s = Math.floor(5 * Math.random() + 1);
        t = Math.floor(5 * Math.random() + 1);
        u = Math.floor(5 * Math.random() + 1);
        v = Math.floor(5 * Math.random() + 1);
        if (g == r) {
            p -= 5;
        } else if (g == s) {
            p += 5;
        } else if (g == t) {
            p += p;
            print("YOU HIT THE JACKPOT!!!\n");
        } else if (g == u) {
            p += 1;
        } else if (g == v) {
            p -= p * 0.5;
        }
        if (p <= 500) {
            print("YOU HAVE " + p + " POINTS.\n");
            print("\n");
        } else {
            print("!!!!YOU WIN!!!! WITH " + p + " POINTS.\n");
            break;
        }
    }
}

main();
