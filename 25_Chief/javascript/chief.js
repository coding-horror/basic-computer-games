// CHIEF
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
    print(tab(30) + "CHIEF\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("I AM CHIEF NUMBERS FREEK, THE GREAT INDIAN MATH GOD.\n");
    print("ARE YOU READY TO TAKE THE TEST YOU CALLED ME OUT FOR");
    a = await input();
    if (a.substr(0, 1) != "Y")
        print("SHUT UP, PALE FACE WITH WIE TONGUE.\n");
    print(" TAKE A NUMBER AND ADD 3. DIVIDE THIS NUMBER BY 5 AND\n");
    print("MULTIPLY BY 8. DIVIDE BY 5 AND ADD THE SAME. SUBTRACT 1.\n");
    print("  WHAT DO YOU HAVE");
    b = parseFloat(await input());
    c = (b + 1 - 5) * 5 / 8 * 5 - 3;
    print("I BET YOUR NUMBER WAS " + Math.floor(c + 0.5) + ". AM I RIGHT");
    d = await input();
    if (d.substr(0, 1) != "Y") {
        print("WHAT WAS YOUR ORIGINAL NUMBER");
        k = parseFloat(await input());
        f = k + 3;
        g = f / 5;
        h = g * 8;
        i = h / 5 + 5;
        j = i - 1;
        print("SO YOU THINK YOU'RE SO SMART, EH?\n");
        print("NOW WATCH.\n");
        print(k + " PLUS 3 EQUALS " + f + ". THIS DIVIDED BY 5 EQUALS " + g + ";\n");
        print("THIS TIMES 8 EQUALS " + h + ". IF WE DIVIDE BY 5 AND ADD 5,\n");
        print("WE GET " + i + ", WHICH, MINUS 1, EQUALS " + j + ".\n");
        print("NOW DO YOU BELIEVE ME");
        z = await input();
        if (z.substr(0, 1) != "Y") {
            print("YOU HAVE MADE ME MAD!!!\n");
            print("THERE MUST BE A GREAT LIGHTNING BOLT!\n");
            print("\n");
            print("\n");
            for (x = 30; x >= 22; x--)
                print(tab(x) + "X X\n");
            print(tab(21) + "X XXX\n");
            print(tab(20) + "X   X\n");
            print(tab(19) + "XX X\n");
            for (y = 20; y >= 13; y--)
                print(tab(y) + "X X\n");
            print(tab(12) + "XX\n");
            print(tab(11) + "X\n");
            print(tab(10) + "*\n");
            print("\n");
            print("#########################\n");
            print("\n");
            print("I HOPE YOU BELIEVE ME NOW, FOR YOUR SAKE!!\n");
            return;
        }
    }
    print("BYE!!!\n");
}

main();
