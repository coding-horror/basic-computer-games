// CHEMIST
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
    print(tab(33) + "CHEMIST\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THE FICTITIOUS CHECMICAL KRYPTOCYANIC ACID CAN ONLY BE\n");
    print("DILUTED BY THE RATIO OF 7 PARTS WATER TO 3 PARTS ACID.\n");
    print("IF ANY OTHER RATIO IS ATTEMPTED, THE ACID BECOMES UNSTABLE\n");
    print("AND SOON EXPLODES.  GIVEN THE AMOUNT OF ACID, YOU MUST\n");
    print("DECIDE WHO MUCH WATER TO ADD FOR DILUTION.  IF YOU MISS\n");
    print("YOU FACE THE CONSEQUENCES.\n");
    t = 0;
    while (1) {
        a = Math.floor(Math.random() * 50);
        w = 7 * a / 3;
        print(a + " LITERS OF KRYPTOCYANIC ACID.  HOW MUCH WATER");
        r = parseFloat(await input());
        d = Math.abs(w - r);
        if (d > w / 20) {
            print(" SIZZLE!  YOU HAVE JUST BEEN DESALINATED INTO A BLOB\n");
            print(" OF QUIVERING PROTOPLASM!\n");
            t++;
            if (t == 9)
                break;
            print(" HOWEVER, YOU MAY TRY AGAIN WITH ANOTHER LIFE.\n");
        } else {
            print(" GOOD JOB! YOU MAY BREATHE NOW, BUT DON'T INHALE THE FUMES!\n");
            print("\n");
        }
    }
    print(" YOUR 9 LIVES ARE USED, BUT YOU WILL BE LONG REMEMBERED FOR\n");
    print(" YOUR CONTRIBUTIONS TO THE FIELD OF COMIC BOOK CHEMISTRY.\n");
}

main();
