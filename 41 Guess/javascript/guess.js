// GUESS
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

function make_space()
{
    for (h = 1; h <= 5; h++)
        print("\n");
}

// Main control section
async function main()
{
    while (1) {
        print(tab(33) + "GUESS\n");
        print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
        print("\n");
        print("\n");
        print("\n");
        print("THIS IS A NUMBER GUESSING GAME. I'LL THINK\n");
        print("OF A NUMBER BETWEEN 1 AND ANY LIMIT YOU WANT.\n");
        print("THEN YOU HAVE TO GUESS WHAT IT IS.\n");
        print("\n");
        
        print("WHAT LIMIT DO YOU WANT");
        l = parseInt(await input());
        print("\n");
        l1 = Math.floor(Math.log(l) / Math.log(2)) + 1;
        while (1) {
            print("I'M THINKING OF A NUMBER BETWEEN 1 AND " + l + "\n");
            g = 1;
            print("NOW YOU TRY TO GUESS WHAT IT IS.\n");
            m = Math.floor(l * Math.random() + 1);
            while (1) {
                n = parseInt(await input());
                if (n <= 0) {
                    make_space();
                    break;
                }
                if (n == m) {
                    print("THAT'S IT! YOU GOT IT IN " + g + " TRIES.\n");
                    if (g == l1) {
                        print("GOOD.\n");
                    } else if (g < l1) {
                        print("VERY GOOD.\n");
                    } else {
                        print("YOU SHOULD HAVE BEEN TO GET IT IN ONLY " + l1 + "\n");
                    }
                    make_space();
                    break;
                }
                g++;
                if (n > m)
                    print("TOO HIGH. TRY A SMALLER ANSWER.\n");
                else
                    print("TOO LOW. TRY A BIGGER ANSWER.\n");
            }
            if (n <= 0)
                break;
        }
    }
}

main();
