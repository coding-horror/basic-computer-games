// BAGELS
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

print(tab(33) + "BAGELS\n");
print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");

// *** Bagles number guessing game
// *** Original source unknown but suspected to be
// *** Lawrence Hall of Science, U.C. Berkeley

a1 = [0,0,0,0];
a = [0,0,0,0];
b = [0,0,0,0];

y = 0;
t = 255;

print("\n");
print("\n");
print("\n");

// Main program
async function main()
{
    while (1) {
        print("WOULD YOU LIKE THE RULES (YES OR NO)");
        str = await input();
        if (str.substr(0, 1) != "N") {
            print("\n");
            print("I AM THINKING OF A THREE-DIGIT NUMBER.  TRY TO GUESS\n");
            print("MY NUMBER AND I WILL GIVE YOU CLUES AS FOLLOWS:\n");
            print("   PICO   - ONE DIGIT CORRECT BUT IN THE WRONG POSITION\n");
            print("   FERMI  - ONE DIGIT CORRECT AND IN THE RIGHT POSITION\n");
            print("   BAGELS - NO DIGITS CORRECT\n");
        }
        for (i = 1; i <= 3; i++) {
            do {
                a[i] = Math.floor(Math.random() * 10);
                for (j = i - 1; j >= 1; j--) {
                    if (a[i] == a[j])
                        break;
                }
            } while (j >= 1) ;
        }
        print("\n");
        print("O.K.  I HAVE A NUMBER IN MIND.\n");
        for (i = 1; i <= 20; i++) {
            while (1) {
                print("GUESS #" + i);
                str = await input();
                if (str.length != 3) {
                    print("TRY GUESSING A THREE-DIGIT NUMBER.\n");
                    continue;
                }
                for (z = 1; z <= 3; z++)
                    a1[z] = str.charCodeAt(z - 1);
                for (j = 1; j <= 3; j++) {
                    if (a1[j] < 48 || a1[j] > 57)
                        break;
                    b[j] = a1[j] - 48;
                }
                if (j <= 3) {
                    print("WHAT?");
                    continue;
                }
                if (b[1] == b[2] || b[2] == b[3] || b[3] == b[1]) {
                    print("OH, I FORGOT TO TELL YOU THAT THE NUMBER I HAVE IN MIND\n");
                    print("HAS NO TWO DIGITS THE SAME.\n");
                    continue;
                }
                break;
            }
            c = 0;
            d = 0;
            for (j = 1; j <= 2; j++) {
                if (a[j] == b[j + 1])
                    c++;
                if (a[j + 1] == b[j])
                    c++;
            }
            if (a[1] == b[3])
                c++;
            if (a[3] == b[1])
                c++;
            for (j = 1; j <= 3; j++) {
                if (a[j] == b[j])
                    d++;
            }
            if (d == 3)
                break;
            for (j = 0; j < c; j++)
                print("PICO ");
            for (j = 0; j < d; j++)
                print("FERMI ");
            if (c + d == 0)
                print("BAGELS");
            print("\n");
        }
        if (i <= 20) {
            print("YOU GOT IT!!!\n");
            print("\n");
        } else {
            print("OH WELL.\n");
            print("THAT'S A TWENTY GUESS.  MY NUMBER WAS " + a[1] + a[2] + a[3]);
        }
        y++;
        print("PLAY AGAIN (YES OR NO)");
        str = await input();
        if (str.substr(0, 1) != "Y")
            break;
    }
    if (y == 0)
        print("HOPE YOU HAD FUN.  BYE.\n");
    else
        print("\nA " + y + " POINT BAGELS BUFF!!\n");

}

main();
