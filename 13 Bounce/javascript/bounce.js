// BOUNCE
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
    print(tab(33) + "BOUNCE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    ta = [];
    print("THIS SIMULATION LETS YOU SPECIFY THE INITIAL VELOCITY\n");
    print("OF A BALL THROWN STRAIGHT UP, AND THE COEFFICIENT OF\n");
    print("ELASTICITY OF THE BALL.  PLEASE USE A DECIMAL FRACTION\n");
    print("COEFFICIENCY (LESS THAN 1).\n");
    print("\n");
    print("YOU ALSO SPECIFY THE TIME INCREMENT TO BE USED IN\n");
    print("'STROBING' THE BALL'S FLIGHT (TRY .1 INITIALLY).\n");
    print("\n");
    while (1) {
        print("TIME INCREMENT (SEC)");
        s2 = parseFloat(await input());
        print("\n");
        print("VELOCITY (FPS)");
        v = parseFloat(await input());
        print("\n");
        print("COEFFICIENT");
        c = parseFloat(await input());
        print("\n");
        print("FEET\n");
        print("\n");
        s1 = Math.floor(70 / (v / (16 * s2)));
        for (i = 1; i <= s1; i++)
            ta[i] = v * Math.pow(c, i - 1) / 16;
        for (h = Math.floor(-16 * Math.pow(v / 32, 2) + Math.pow(v, 2) / 32 + 0.5); h >= 0; h -= 0.5) {
            str = "";
            if (Math.floor(h) == h)
                str += " " + h + " ";
            l = 0;
            for (i = 1; i <= s1; i++) {
                for (t = 0; t <= ta[i]; t += s2) {
                    l += s2;
                    if (Math.abs(h - (0.5 * (-32) * Math.pow(t, 2) + v * Math.pow(c, i - 1) * t)) <= 0.25) {
                        while (str.length < l / s2)
                            str += " ";
                        str += "0";
                    }
                }
                t = ta[i + 1] / 2;
                if (-16 * Math.pow(t, 2) + v * Math.pow(c, i - 1) * t < h)
                    break;
            }
            print(str + "\n");
        }
        str = " ";
        for (i = 1; i < Math.floor(l + 1) / s2 + 1; i++)
            str += ".";
        print(str + "\n");
        str = " 0";
        for (i = 1; i < Math.floor(l + 0.9995); i++) {
            while (str.length < Math.floor(i / s2))
                str += " ";
            str += i;
        }
        print(str + "\n");
        print(tab(Math.floor(l + 1) / (2 * s2) - 2) + "SECONDS\n");
    }
}

main();
