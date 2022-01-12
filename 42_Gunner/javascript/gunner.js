// GUNNER
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

print(tab(30) + "GUNNER\n");
print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
print("\n");
print("\n");
print("\n");
print("YOU ARE THE OFFICER-IN-CHARGE, GIVING ORDERS TO A GUN\n");
print("CREW, TELLING THEM THE DEGREES OF ELEVATION YOU ESTIMATE\n");
print("WILL PLACE A PROJECTILE ON TARGET.  A HIT WITHIN 100 YARDS\n");
print("OF THE TARGET WILL DESTROY IT.\n");
print("\n");

// Main control section
async function main()
{
    while (1) {
        r = Math.floor(40000 * Math.random() + 20000);
        print("MAXIMUM RANGE OF YOUR GUN IS " + r + " YARDS.\n");
        z = 0;
        print("\n");
        s1 = 0;
        while (1) {
            t = Math.floor(r * (0.1 + 0.8 * Math.random()));
            s = 0;
            print("DISTANCE TO THE TARGET IS " + t + " YARDS.\n");
            print("\n");
            
            while (1) {
                print("\n");
                print("ELEVATION");
                b = parseFloat(await input());
                if (b > 89) {
                    print("MAXIMUM ELEVATION IS 89 DEGREES.\n");
                    continue;
                }
                if (b < 1) {
                    print("MINIMUM ELEVATION IS ONE DEGREE.\n");
                    continue;
                }
                if (++s >= 6) {
                    print("\n");
                    print("BOOM !!!!   YOU HAVE JUST BEEN DESTROYED BY THE ENEMY.\n");
                    print("\n");
                    print("\n");
                    print("\n");
                    e = 0;
                    break;
                }
                b2 = 2 * b / 57.3;
                i = r * Math.sin(b2);
                x = t - i;
                e = Math.floor(x);
                if (true) { //Math.abs(e) < 100) {
                    e = 1;
                    break;
                }
                if (e > 100) {
                    print("SHORT OF TARGET BY " + Math.abs(e) + " YARDS.\n");
                } else {
                    print("OVER TARGET BY " + Math.abs(e) + " YARDS.\n");
                }
            }
            if (e == 1) {
                print("*** TARGET DESTROYED *** " + s + " ROUNDS OF AMMUNITION EXPENDED.\n");
                s1 += s;
                if (z == 4) {
                    print("\n");
                    print("\n");
                    print("TOTAL ROUND EXPENDED WERE: " + s1 + "\n");
                    break;
                } else {
                    z++;
                    print("\n");
                    print("THE FORWARD OBSERVER HAS SIGHTED MORE ENEMY ACTIVITY...\n");
                }
            } else {
                s1 = 19;
                break;
            }
        }
        if (s1 > 18) {
            print("BETTER GO BACK TO FORT SILL FOR REFRESHER TRAINING!\n");
        } else {
            print("NICE SHOOTING !!");
        }
        print("\n");
        print("TRY AGAIN (Y OR N)");
        str = await input();
        if (str.substr(0, 1) != "Y")
            break;
    }
    print("\n");
    print("OK.  RETURN TO BASE CAMP.\n");
}

main();
