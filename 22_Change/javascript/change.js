// CHANGE
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
    print(tab(33) + "CHANGE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("I, YOUR FRIENDLY MICROCOMPUTER, WILL DETERMINE\n");
    print("THE CORRECT CHANGE FOR ITEMS COSTING UP TO $100.\n");
    print("\n");
    print("\n");
    while (1) {
        print("COST OF ITEM");
        a = parseFloat(await input());
        print("AMOUNT OF PAYMENT");
        p = parseFloat(await input());
        c = p - a;
        m = c;
        if (c == 0) {
            print("CORRECT AMOUNT, THANK YOU.\n");
        } else {
            print("YOUR CHANGE, $" + c + "\n");
            d = Math.floor(c / 10);
            if (d)
                print(d + " TEN DOLLAR BILL(S)\n");
            c -= d * 10;
            e = Math.floor(c / 5);
            if (e)
                print(e + " FIVE DOLLAR BILL(S)\n");
            c -= e * 5;
            f = Math.floor(c);
            if (f)
                print(f + " ONE DOLLAR BILL(S)\n");
            c -= f;
            c *= 100;
            g = Math.floor(c / 50);
            if (g)
                print(g + " ONE HALF DOLLAR(S)\n");
            c -= g * 50;
            h = Math.floor(c / 25);
            if (h)
                print(h + " QUARTER(S)\n");
            c -= h * 25;
            i = Math.floor(c / 10);
            if (i)
                print(i + " DIME(S)\n");
            c -= i * 10;
            j = Math.floor(c / 5);
            if (j)
                print(j + " NICKEL(S)\n");
            c -= j * 5;
            k = Math.floor(c + 0.5);
            if (k)
                print(k + " PENNY(S)\n");
            print("THANK YOU, COME AGAIN.\n");
            print("\n");
            print("\n");
        }
    }
}

main();
