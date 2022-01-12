// TRAIN
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
    print(tab(33) + "TRAIN\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("TIME - SPEED DISTANCE EXERCISE\n");
    print("\n ");
    while (1) {
        c = Math.floor(25 * Math.random()) + 40;
        d = Math.floor(15 * Math.random()) + 5;
        t = Math.floor(19 * Math.random()) + 20;
        print(" A CAR TRAVELING " + c + " MPH CAN MAKE A CERTAIN TRIP IN\n");
        print(d + " HOURS LESS THAN A TRAIN TRAVELING AT " + t + " MPH.\n");
        print("HOW LONG DOES THE TRIP TAKE BY CAR");
        a = parseFloat(await input());
        v = d * t / (c - t);
        e = Math.floor(Math.abs((v - a) * 100 / a) + 0.5);
        if (e > 5) {
            print("SORRY.  YOU WERE OFF BY " + e + " PERCENT.\n");
        } else {
            print("GOOD! ANSWER WITHIN " + e + " PERCENT.\n");
        }
        print("CORRECT ANSWER IS " + v + " HOURS.\n");
        print("\n");
        print("ANOTHER PROBLEM (YES OR NO)\n");
        str = await input();
        print("\n");
        if (str.substr(0, 1) != "Y")
            break;
    }
}

main();
