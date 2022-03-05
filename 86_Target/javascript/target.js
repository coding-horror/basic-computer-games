// TARGET
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
    print(tab(33) + "TARGET\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    r = 0;  // 1 in original
    r1 = 57.296;
    p = Math.PI;
    print("YOU ARE THE WEAPONS OFFICER ON THE STARSHIP ENTERPRISE\n");
    print("AND THIS IS A TEST TO SEE HOW ACCURATE A SHOT YOU\n");
    print("ARE IN A THREE-DIMENSIONAL RANGE.  YOU WILL BE TOLD\n");
    print("THE RADIAN OFFSET FOR THE X AND Z AXES, THE LOCATION\n");
    print("OF THE TARGET IN THREE DIMENSIONAL RECTANGULAR COORDINATES,\n");
    print("THE APPROXIMATE NUMBER OF DEGREES FROM THE X AND Z\n");
    print("AXES, AND THE APPROXIMATE DISTANCE TO THE TARGET.\n");
    print("YOU WILL THEN PROCEEED TO SHOOT AT THE TARGET UNTIL IT IS\n");
    print("DESTROYED!\n");
    print("\n");
    print("GOOD LUCK!!\n");
    print("\n");
    print("\n");
    while (1) {
        a = Math.random() * 2 * p;
        b = Math.random() * 2 * p;
        q = Math.floor(a * r1);
        w = Math.floor(b * r1);
        print("RADIANS FROM X AXIS = " + a + "   FROM Z AXIS = " + b + "\n");
        p1 = 100000 * Math.random() + Math.random();
        x = Math.sin(b) * Math.cos(a) * p1;
        y = Math.sin(b) * Math.sin(a) * p1;
        z = Math.cos(b) * p1;
        print("TARGET SIGHTED: APPROXIMATE COORDINATES:  X=" + x + "  Y=" + y + "  Z=" + z + "\n");
        while (1) {
            r++;
            switch (r) {
                case 1:
                    p3 = Math.floor(p1 * 0.05) * 20;
                    break;
                case 2:
                    p3 = Math.floor(p1 * 0.1) * 10;
                    break;
                case 3:
                    p3 = Math.floor(p1 * 0.5) * 2;
                    break;
                case 4:
                    p3 = Math.floor(p1);
                    break;
                case 5:
                    p3 = p1;
                    break;
            }
            print("     ESTIMATED DISTANCE: " + p3 + "\n");
            print("\n");
            print("INPUT ANGLE DEVIATION FROM X, DEVIATION FROM Z, DISTANCE");
            str = await input();
            a1 = parseInt(str);
            b1 = parseInt(str.substr(str.indexOf(",") + 1));
            p2 = parseInt(str.substr(str.lastIndexOf(",") + 1));
            print("\n");
            if (p2 < 20) {
                print("YOU BLEW YOURSELF UP!!\n");
                break;
            }
            a1 /= r1;
            b1 /= r1;
            print("RADIANS FROM X AXIS = " + a1 + "  ");
            print("FROM Z AXIS = " + b1 + "\n");
            x1 = p2 * Math.sin(b1) * Math.cos(a1);
            y1 = p2 * Math.sin(b1) * Math.sin(a1);
            z1 = p2 * Math.cos(b1);
            d = Math.sqrt((x1 - x) * (x1 - x) + (y1 - y) * (y1 - y) + (z1 - z) * (z1 - z));
            if (d <= 20) {
                print("\n");
                print(" * * * HIT * * *   TARGET IS NON-FUNCTIONAL\n");
                print("\n");
                print("DISTANCE OF EXPLOSION FROM TARGET WAS " + d + " KILOMETERS.");
                print("\n");
                print("MISSION ACCOMPLISHED IN " + r + " SHOTS.\n");
                r = 0;
                for (i = 1; i <= 5; i++)
                    print("\n");
                print("NEXT TARGET...\n");
                print("\n");
                break;
            }
            x2 = x1 - x;
            y2 = y1 - y;
            z2 = z1 - z;
            if (x2 >= 0)
                print("SHOT IN FRONT OF TARGET " + x2 + " KILOMETERS.\n");
            else
                print("SHOT BEHIND TARGET " + -x2 + " KILOMETERS.\n");
            if (y2 >= 0)
                print("SHOT TO LEFT OF TARGET " + y2 + " KILOMETERS.\n");
            else
                print("SHOT TO RIGHT OF TARGET " + -y2 + " KILOMETERS.\n");
            if (z2 >= 0)
                print("SHOT ABOVE TARGET " + z2 + " KILOMETERS.\n");
            else
                print("SHOT BELOW TARGET " + -z2 + " KILOMETERS.\n");
            print("APPROX POSITION OF EXPLOSION:  X=" + x1 + "   Y=" + y1 + "   Z=" + z1 + "\n");
            print("     DISTANCE FROM TARGET = " + d + "\n");
            print("\n");
            print("\n");
            print("\n");
        }
    }
}

main();
