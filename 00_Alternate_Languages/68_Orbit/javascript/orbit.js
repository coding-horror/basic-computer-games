// ORBIT
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

var a = [];

// Main program
async function main()
{
    print(tab(33) + "ORBIT\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("SOMEWHERE ABOVE YOUR PLANET IS A ROMULAN SHIP.\n");
    print("\n");
    print("THE SHIP IS IN A CONSTANT POLAR ORBIT.  ITS\n");
    print("DISTANCE FROM THE CENTER OF YOUR PLANET IS FROM\n");
    print("10,000 TO 30,000 MILES AND AT ITS PRESENT VELOCITY CAN\n");
    print("CIRCLE YOUR PLANET ONCE EVERY 12 TO 36 HOURS.\n");
    print("\n");
    print("UNFORTUNATELY, THEY ARE USING A CLOAKING DEVICE SO\n");
    print("YOU ARE UNABLE TO SEE THEM, BUT WITH A SPECIAL\n");
    print("INSTRUMENT YOU CAN TELL HOW NEAR THEIR SHIP YOUR\n");
    print("PHOTON BOMB EXPLODED.  YOU HAVE SEVEN HOURS UNTIL THEY\n");
    print("HAVE BUILT UP SUFFICIENT POWER IN ORDER TO ESCAPE\n");
    print("YOUR PLANET'S GRAVITY.\n");
    print("\n");
    print("YOUR PLANET HAS ENOUGH POWER TO FIRE ONE BOMB AN HOUR.\n");
    print("\n");
    print("AT THE BEGINNING OF EACH HOUR YOU WILL BE ASKED TO GIVE AN\n");
    print("ANGLE (BETWEEN 0 AND 360) AND A DISTANCE IN UNITS OF\n");
    print("100 MILES (BETWEEN 100 AND 300), AFTER WHICH YOUR BOMB'S\n");
    print("DISTANCE FROM THE ENEMY SHIP WILL BE GIVEN.\n");
    print("\n");
    print("AN EXPLOSION WITHIN 5,000 MILES OF THE ROMULAN SHIP\n");
    print("WILL DESTROY IT.\n");
    print("\n");
    print("BELOW IS A DIAGRAM TO HELP YOU VISUALIZE YOUR PLIGHT.\n");
    print("\n");
    print("\n");
    print("                          90\n");
    print("                    0000000000000\n");
    print("                 0000000000000000000\n");
    print("               000000           000000\n");
    print("             00000                 00000\n");
    print("            00000    XXXXXXXXXXX    00000\n");
    print("           00000    XXXXXXXXXXXXX    00000\n");
    print("          0000     XXXXXXXXXXXXXXX     0000\n");
    print("         0000     XXXXXXXXXXXXXXXXX     0000\n");
    print("        0000     XXXXXXXXXXXXXXXXXXX     0000\n");
    print("180<== 00000     XXXXXXXXXXXXXXXXXXX     00000 ==>0\n");
    print("        0000     XXXXXXXXXXXXXXXXXXX     0000\n");
    print("         0000     XXXXXXXXXXXXXXXXX     0000\n");
    print("          0000     XXXXXXXXXXXXXXX     0000\n");
    print("           00000    XXXXXXXXXXXXX    00000\n");
    print("            00000    XXXXXXXXXXX    00000\n");
    print("             00000                 00000\n");
    print("               000000           000000\n");
    print("                 0000000000000000000\n");
    print("                    0000000000000\n");
    print("                         270\n");
    print("\n");
    print("X - YOUR PLANET\n");
    print("O - THE ORBIT OF THE ROMULAN SHIP\n");
    print("\n");
    print("ON THE ABOVE DIAGRAM, THE ROMULAN SHIP IS CIRCLING\n");
    print("COUNTERCLOCKWISE AROUND YOUR PLANET.  DON'T FORGET THAT\n");
    print("WITHOUT SUFFICIENT POWER THE ROMULAN SHIP'S ALTITUDE\n");
    print("AND ORBITAL RATE WILL REMAIN CONSTANT.\n");
    print("\n");
    print("GOOD LUCK.  THE FEDERATION IS COUNTING ON YOU.\n");
    while (1) {
        a = Math.floor(360 * Math.random());
        d = Math.floor(200 * Math.random() + 200);
        r = Math.floor(20 * Math.random() + 10);
        h = 0;
        while (h < 7) {
            print("\n");
            print("\n");
            print("THIS IS HOUR " + (h + 1) + ", AT WHAT ANGLE DO YOU WISH TO SEND\n");
            print("YOUR PHOTON BOMB");
            a1 = parseFloat(await input());
            print("HOW FAR OUT DO YOU WISH TO DETONATE IT");
            d1 = parseFloat(await input());
            print("\n");
            print("\n");
            a += r;
            if (a >= 360)
                a -= 360;
            t = Math.abs(a - a1);
            if (t >= 180)
                t = 360 - t;
            c = Math.sqrt(d * d + d1 * d1 - 2 * d * d1 * Math.cos(t * Math.PI / 180));
            print("YOUR PHOTON BOMB EXPLODED " + c + "*10^2 MILES FROM THE\n");
            print("ROMULAN SHIP.\n");
            if (c <= 50)
                break;
            h++;
        }
        if (h == 7) {
            print("YOU HAVE ALLOWED THE ROMULANS TO ESCAPE.\n");
        } else {
            print("YOU HAVE SUCCESSFULLY COMPLETED YOUR MISSION.\n");
        }
        print("ANOTHER ROMULAN SHIP HAS GONE INTO ORBIT.\n");
        print("DO YOU WISH TO TRY TO DESTROY IT");
        str = await input();
        if (str != "YES")
            break;
    }
    print("GOOD BYE.\n");
}

main();
