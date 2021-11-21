// KINEMA
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

var q;

function evaluate_answer(str, a)
{
    g = parseFloat(str);
    if (Math.abs((g - a) / a) < 0.15) {
        print("CLOSE ENOUGH.\n");
        q++;
    } else {
        print("NOT EVEN CLOSE....\n");
    }
    print("CORRECT ANSWER IS " + a + "\n\n");
}

// Main program
async function main()
{
    print(tab(33) + "KINEMA\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    while (1) {
        print("\n");
        print("\n");
        q = 0;
        v = 5 + Math.floor(35 * Math.random());
        print("A BALL IS THROWN UPWARDS AT " + v + " METERS PER SECOND.\n");
        print("\n");
        a = 0.5 * Math.pow(v, 2);
        print("HOW HIGH WILL IT GO (IN METERS)");
        str = await input();
        evaluate_answer(str, a);
        a = v / 5;
        print("HOW LONG UNTIL IT RETURNS (IN SECONDS)");
        str = await input();
        evaluate_answer(str, a);
        t = 1 + Math.floor(2 * v * Math.random()) / 10;
        a = v - 10 * t;
        print("WHAT WILL ITS VELOCITY BE AFTER " + t + " SECONDS");
        str = await input();
        evaluate_answer(str, a);
        print("\n");
        print(q + " RIGHT OUT OF 3.");
        if (q < 2)
            continue;
        print("  NOT BAD.\n");
    }
}

main();
