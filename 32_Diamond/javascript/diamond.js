// DIAMOND
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
    print(tab(33) + "DIAMOND\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("FOR A PRETTY DIAMOND PATTERN,\n");
    print("TYPE IN AN ODD NUMBER BETWEEN 5 AND 21");
    r = parseInt(await input());
    q = Math.floor(60 / r);
    as = "CC"
    x = 1;
    y = r;
    z = 2;
    for (l = 1; l <= q; l++) {
        for (n = x; z < 0 ? n >= y : n <= y; n += z) {
            str = "";
            while (str.length < (r - n) / 2)
                str += " ";
            for (m = 1; m <= q; m++) {
                c = 1;
                for (a = 1; a <= n; a++) {
                    if (c > as.length)
                        str += "!";
                    else
                        str += as[c++ - 1];
                }
                if (m == q)
                    break;
                while (str.length < r * m + (r - n) / 2)
                    str += " ";
            }
            print(str + "\n");
        }
        if (x != 1) {
            x = 1;
            y = r;
            z = 2;
        } else {
            x = r - 2;
            y = 1;
            z = -2;
            l--;
        }
    }
}

main();
