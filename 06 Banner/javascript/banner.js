// BANNER
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

var letters = [" ",0,0,0,0,0,0,0,
               "A",505,37,35,34,35,37,505,
               "G",125,131,258,258,290,163,101,
               "E",512,274,274,274,274,258,258,
               "T",2,2,2,512,2,2,2,
               "W",256,257,129,65,129,257,256,
               "L",512,257,257,257,257,257,257,
               "S",69,139,274,274,274,163,69,
               "O",125,131,258,258,258,131,125,
               "N",512,7,9,17,33,193,512,
               "F",512,18,18,18,18,2,2,
               "K",512,17,17,41,69,131,258,
               "B",512,274,274,274,274,274,239,
               "D",512,258,258,258,258,131,125,
               "H",512,17,17,17,17,17,512,
               "M",512,7,13,25,13,7,512,
               "?",5,3,2,354,18,11,5,
               "U",128,129,257,257,257,129,128,
               "R",512,18,18,50,82,146,271,
               "P",512,18,18,18,18,18,15,
               "Q",125,131,258,258,322,131,381,
               "Y",8,9,17,481,17,9,8,
               "V",64,65,129,257,129,65,64,
               "X",388,69,41,17,41,69,388,
               "Z",386,322,290,274,266,262,260,
               "I",258,258,258,512,258,258,258,
               "C",125,131,258,258,258,131,69,
               "J",65,129,257,257,257,129,128,
               "1",0,0,261,259,512,257,257,
               "2",261,387,322,290,274,267,261,
               "*",69,41,17,512,17,41,69,
               "3",66,130,258,274,266,150,100,
               "4",33,49,41,37,35,512,33,
               "5",160,274,274,274,274,274,226,
               "6",194,291,293,297,305,289,193,
               "7",258,130,66,34,18,10,8,
               "8",69,171,274,274,274,171,69,
               "9",263,138,74,42,26,10,7,
               "=",41,41,41,41,41,41,41,
               "!",1,1,1,384,1,1,1,
               "0",57,69,131,258,131,69,57,
               ".",1,1,129,449,129,1,1];

f = [];
j = [];
s = [];

// Main program
async function main()
{
    print("HORIZONTAL");
    x = parseInt(await input());
    print("VERTICAL");
    y = parseInt(await input());
    print("CENTERED");
    ls = await input();
    g1 = 0;
    if (ls > "P")
        g1 = 1;
    print("CHARACTER (TYPE 'ALL' IF YOU WANT CHARACTER BEING PRINTED)");
    ms = await input();
    print("STATEMENT");
    as = await input();
    print("SET PAGE");	// This means to prepare printer, just press Enter
    os = await input();
    
    for (t = 0; t < as.length; t++) {
        ps = as.substr(t, 1);
        for (o = 0; o < 50 * 8; o += 8) {
            if (letters[o] == ps) {
                for (u = 1; u <= 7; u++)
                    s[u] = letters[o + u];
                break;
            }
        }
        if (o == 50 * 8) {
            ps = " ";
            o = 0;
        }
//      print("Doing " + o + "\n");
        if (o == 0) {
            for (h = 1; h <= 7 * x; h++)
                print("\n");
        } else {
            xs = ms;
            if (ms == "ALL")
                xs = ps;
            for (u = 1; u <= 7; u++) {
                // An inefficient way of extracting bits
                // but good enough in BASIC because there
                // aren't bit shifting operators.
                for (k = 8; k >= 0; k--) {
                    if (Math.pow(2, k) >= s[u]) {
                        j[9 - k] = 0;
                    } else {
                        j[9 - k] = 1;
                        s[u] -= Math.pow(2, k);
                        if (s[u] == 1) {
                            f[u] = 9 - k;
                            break;
                        }
                    }
                }
                for (t1 = 1; t1 <= x; t1++) {
                    str = tab((63 - 4.5 * y) * g1 / xs.length + 1);
                    for (b = 1; b <= f[u]; b++) {
                        if (j[b] == 0) {
                            for (i = 1; i <= y; i++)
                                str += tab(xs.length);
                        } else {
                            for (i = 1; i <= y; i++)
                                str += xs;
                        }
                    }
                    print(str + "\n");	
                }
            }
            for (h = 1; h <= 2 * x; h++) 
                print("\n");
        }
    }    
}

main();
