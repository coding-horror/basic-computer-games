// NICOMACHUS
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

var str;
var b;

// Main program
async function main()
{
    print(tab(33) + "NICOMA\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("BOOMERANG PUZZLE FROM ARITHMETICA OF NICOMACHUS -- A.D. 90!\n");
    while (1) {
        print("\n");
        print("PLEASE THINK OF A NUMBER BETWEEN 1 AND 100.\n");
        print("YOUR NUMBER DIVIDED BY 3 HAS A REMAINDER OF");
        a = parseInt(await input());
        print("YOUR NUMBER DIVIDED BY 5 HAS A REMAINDER OF");
        b = parseInt(await input());
        print("YOUR NUMBER DIVIDED BY 7 HAS A REMAINDER OF");
        c = parseInt(await input());
        print("\n");
        print("LET ME THINK A MOMENT...\n");
        print("\n");
        d = 70 * a + 21 * b + 15 * c;
        while (d > 105)
            d -= 105;
        print("YOUR NUMBER WAS " + d + ", RIGHT");
        while (1) {
            str = await input();
            print("\n");
            if (str == "YES") {
                print("HOW ABOUT THAT!!\n");
                break;
            } else if (str == "NO") {
                print("I FEEL YOUR ARITHMETIC IS IN ERROR.\n");
                break;
            } else {
                print("EH?  I DON'T UNDERSTAND '" + str + "'  TRY 'YES' OR 'NO'.\n");
            }
        }
        print("\n");
        print("LET'S TRY ANOTHER.\n");
    }
}

main();
