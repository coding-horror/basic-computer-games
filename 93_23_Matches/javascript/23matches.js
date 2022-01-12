// 23 MATCHES
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
    print(tab(31) + "23 MATCHES\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print(" THIS IS A GAME CALLED '23 MATCHES'.\n");
    print("\n");
    print("WHEN IT IS YOUR TURN, YOU MAY TAKE ONE, TWO, OR THREE\n");
    print("MATCHES. THE OBJECT OF THE GAME IS NOT TO HAVE TO TAKE\n");
    print("THE LAST MATCH.\n");
    print("\n");
    print("LET'S FLIP A COIN TO SEE WHO GOES FIRST.\n");
    print("IF IT COMES UP HEADS, I WILL WIN THE TOSS.\n");
    print("\n");
    n = 23;
    q = Math.floor(2 * Math.random());
    if (q != 1) {
        print("TAILS! YOU GO FIRST. \n");
        print("\n");
    } else {
        print("HEADS! I WIN! HA! HA!\n");
        print("PREPARE TO LOSE, MEATBALL-NOSE!!\n");
        print("\n");
        print("I TAKE 2 MATCHES\n");
        n -= 2;
    }
    while (1) {
        if (q == 1) {
            print("THE NUMBER OF MATCHES IS NOW " + n + "\n");
            print("\n");
            print("YOUR TURN -- YOU MAY TAKE 1, 2 OR 3 MATCHES.\n");
        }
        print("HOW MANY DO YOU WISH TO REMOVE ");
        while (1) {
            k = parseInt(await input());
            if (k <= 0 || k > 3) {
                print("VERY FUNNY! DUMMY!\n");
                print("DO YOU WANT TO PLAY OR GOOF AROUND?\n");
                print("NOW, HOW MANY MATCHES DO YOU WANT ");
            } else {
                break;
            }
        }
        n -= k;
        print("THERE ARE NOW " + n + " MATCHES REMAINING.\n");
        if (n == 4) {
            z = 3;
        } else if (n == 3) {
            z = 2;
        } else if (n == 2) {
            z = 1;
        } else if (n > 1) {
            z = 4 - k;
        } else {
            print("YOU WON, FLOPPY EARS !\n");
            print("THINK YOU'RE PRETTY SMART !\n");
            print("LETS PLAY AGAIN AND I'LL BLOW YOUR SHOES OFF !!\n");
            break;
        }
        print("MY TURN ! I REMOVE " + z + " MATCHES\n");
        n -= z;
        if (n <= 1) {
            print("\n");
            print("YOU POOR BOOB! YOU TOOK THE LAST MATCH! I GOTCHA!!\n");
            print("HA ! HA ! I BEAT YOU !!!\n");
            print("\n");
            print("GOOD BYE LOSER!\n");
            break;
        }
        q = 1;
    }
    
}

main();
