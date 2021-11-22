// POETRY
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
    print(tab(30) + "POETRY\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    
    times = 0;
    
    i = 1;
    j = 1;
    k = 0;
    u = 0;
    while (1) {
        if (j == 1) {
            switch (i) {
                case 1:
                    print("MIDNIGHT DREARY");
                    break;
                case 2:
                    print("FIERY EYES");
                    break;
                case 3:
                    print("BIRD OF FIEND");
                    break;
                case 4:
                    print("THING OF EVIL");
                    break;
                case 5:
                    print("PROPHET");
                    break;
            }
        } else if (j == 2) {
            switch (i) {
                case 1:
                    print("BEGUILING ME");
                    u = 2;
                    break;
                case 2:
                    print("THRILLED ME");
                    break;
                case 3:
                    print("STILL SITTING....");
                    u = 0;
                    break;
                case 4:
                    print("NEVER FLITTING");
                    u = 2;
                    break;
                case 5:
                    print("BURNED");
                    break;
            }
        } else if (j == 3) {
            switch (i) {
                case 1:
                    print("AND MY SOUL");
                    break;
                case 2:
                    print("DARKNESS THERE");
                    break;
                case 3:
                    print("SHALL BE LIFTED");
                    break;
                case 4:
                    print("QUOTH THE RAVEN");
                    break;
                case 5:
                    if (u == 0)
                        break;
                    print("SIGN OF PARTING");
                    break;
            }
        } else if (j == 4) {
            switch (i) {
                case 1:
                    print("NOTHING MORE");
                    break;
                case 2:
                    print("YET AGAIN");
                    break;
                case 3:
                    print("SLOWLY CREEPING");
                    break;
                case 4:
                    print("...EVERMORE");
                    break;
                case 5:
                    print("NEVERMORE");
                    break;
            }
        }
        if (u != 0 && Math.random() <= 0.19) {
            print(",");
            u = 2;
        }
        if (Math.random() <= 0.65) {
            print(" ");
            u++;
        } else {
            print("\n");
            u = 0;
        }
        while (1) {
            i = Math.floor(Math.floor(10 * Math.random()) / 2) + 1;
            j++;
            k++;
            if (u == 0 && j % 2 == 0)
                print("     ");
            if (j != 5)
                break;
            j = 0;
            print("\n");
            if (k <= 20)
                continue;
            print("\n");
            u = 0;
            k = 0;
            j = 2;
            break;
        }
        if (u == 0 && k == 0 && j == 2 && ++times == 10)
            break;
    }
}

main();
