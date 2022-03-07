// LOVE
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

var data = [60,1,12,26,9,12,3,8,24,17,8,4,6,23,21,6,4,6,22,12,5,6,5,
            4,6,21,11,8,6,4,4,6,21,10,10,5,4,4,6,21,9,11,5,4,
            4,6,21,8,11,6,4,4,6,21,7,11,7,4,4,6,21,6,11,8,4,
            4,6,19,1,1,5,11,9,4,4,6,19,1,1,5,10,10,4,4,6,18,2,1,6,8,11,4,
            4,6,17,3,1,7,5,13,4,4,6,15,5,2,23,5,1,29,5,17,8,
            1,29,9,9,12,1,13,5,40,1,1,13,5,40,1,4,6,13,3,10,6,12,5,1,
            5,6,11,3,11,6,14,3,1,5,6,11,3,11,6,15,2,1,
            6,6,9,3,12,6,16,1,1,6,6,9,3,12,6,7,1,10,
            7,6,7,3,13,6,6,2,10,7,6,7,3,13,14,10,8,6,5,3,14,6,6,2,10,
            8,6,5,3,14,6,7,1,10,9,6,3,3,15,6,16,1,1,
            9,6,3,3,15,6,15,2,1,10,6,1,3,16,6,14,3,1,10,10,16,6,12,5,1,
            11,8,13,27,1,11,8,13,27,1,60];

// Main program
async function main()
{
    print(tab(33) + "LOVE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("A TRIBUTE TO THE GREAT AMERICAN ARTIST, ROBERT INDIANA.\n");
    print("HIS GREATEST WORK WILL BE REPRODUCED WITH A MESSAGE OF\n");
    print("YOUR CHOICE UP TO 60 CHARACTERS.  IF YOU CAN'T THINK OF\n");
    print("A MESSAGE, SIMPLE TYPE THE WORD 'LOVE'\n");
    print("\n");
    print("YOUR MESSAGE, PLEASE");
    str = await input();
    l = str.length;
    ts = [];
    for (i = 1; i <= 10; i++)
        print("\n");
    ts = "";
    do {
        ts += str;
    } while (ts.length < 60) ;
    pos = 0;
    c = 0;
    while (++c < 37) {
        a1 = 1;
        p = 1;
        print("\n");
        do {
            a = data[pos++];
            a1 += a;
            if (p != 1) {
                for (i = 1; i <= a; i++)
                    print(" ");
                p = 1;
            } else {
                for (i = a1 - a; i <= a1 - 1; i++)
                    print(ts[i]);
                p = 0;
            }
        } while (a1 <= 60) ;
    }
    for (i = 1; i <= 10; i++)
        print("\n");
}

main();
